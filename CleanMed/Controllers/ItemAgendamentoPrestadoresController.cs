using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CleanMed.Data;
using CleanMed.Models;
using Microsoft.Extensions.Logging;
using CleanMed.Servicos;
using CleanMed.Dados.Interface;
using Microsoft.AspNetCore.Authorization;

namespace CleanMed.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR,FATURAMENTO,AGENDAMENTO")]
    public class ItemAgendamentoPrestadoresController : Controller
    {
        private readonly Contexto _context;
        private readonly IItemAgendamentoPrestadorRepositorio _itemAgendamentoPrestadorRepositorio;
        private readonly ILogger<ItemAgendamentoPrestadoresController> _logger;

        public ItemAgendamentoPrestadoresController(Contexto context, ILogger<ItemAgendamentoPrestadoresController> logger, IItemAgendamentoPrestadorRepositorio itemAgendamentoPrestadorRepositorio)
        {
            _context = context;
            _logger = logger;
            _itemAgendamentoPrestadorRepositorio = itemAgendamentoPrestadorRepositorio;
        }

        public async Task<IActionResult> Index(int? pageNumber,int searchPrestadorId, int ItemAgendamentoId)
        {
        
            ViewData["CurrentFilter"] = searchPrestadorId;

            if(ItemAgendamentoId != 0)
            {
                ViewData["NomeItemAgendamento"] = _context.ItemAgendamentos.Where(a => a.ItemAgendamentoId == ItemAgendamentoId).Select(a => a.Descricao).First();
            }
            var itemAgendamentoPrestador = from s in _context.ItemAgendamentoPrestadores
                                           where s.ItemAgendamentoId == ItemAgendamentoId
                                             select s;
            if (searchPrestadorId > 0)
            {

                itemAgendamentoPrestador = itemAgendamentoPrestador.Where(s => s.PrestadorId == searchPrestadorId);
            }
            ViewData["ItemAgendamentoId"] = ItemAgendamentoId;
            ViewData["PrestadorId"] = new SelectList(_context.Prestadores, "PrestadorId", "Nome");
            ViewData["GrupoFaturamentoId"] = new SelectList(_context.GrupoFaturamentos, "GrupoFaturamentoId", "Descricao");
            int pageSize = 5;
            return View(await PaginatedList<ItemAgendamentoPrestador>.CreateAsync(itemAgendamentoPrestador.AsNoTracking().Include(a => a.Prestador).Include(a => a.itemAgendamento), pageNumber ?? 1, pageSize));
        }

        // GET: ItemAgendamentoPrestadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemAgendamentoPrestador = await _context.ItemAgendamentoPrestadores
                .Include(i => i.Prestador)
                .Include(i => i.itemAgendamento)
                .FirstOrDefaultAsync(m => m.PrestadorId == id);
            if (itemAgendamentoPrestador == null)
            {
                return NotFound();
            }

            return View(itemAgendamentoPrestador);
        }

        // GET: ItemAgendamentoPrestadores/Create
        public IActionResult Create(int ItemAgendamentoId)
        {
            _logger.LogInformation("Abrindo view de create");
            ViewData["PrestadorId"] = new SelectList(_context.Prestadores, "PrestadorId", "CPF");
            ViewData["ItemAgendamentoId"] = new SelectList(_context.ItemAgendamentos, "ItemAgendamentoId", "Descricao");
            ViewData["ItemAgendamentoId"] = ItemAgendamentoId;
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( ItemAgendamentoPrestador itemAgendamentoPrestador,int ItemAgendamentoId)
        {
            if (ModelState.IsValid)
            {
                if(await ItemPrestadorExiste(itemAgendamentoPrestador.ItemAgendamentoId, itemAgendamentoPrestador.PrestadorId))
                {
                    TempData["Validacao"] = "Esse prestador já esta adicionado para esse item";
                    return RedirectToAction("index", new { ItemAgendamentoId = ItemAgendamentoId });
                }
               
                _logger.LogInformation("Associando prestador ao item de agendamento");
                _context.Add(itemAgendamentoPrestador);
                await _context.SaveChangesAsync();
                TempData["Mensagem"] = "Adicionado com sucesso";
                return RedirectToAction("index",new { ItemAgendamentoId = ItemAgendamentoId });
            }
            ViewData["PrestadorId"] = new SelectList(_context.Prestadores, "PrestadorId", "CPF", itemAgendamentoPrestador.PrestadorId);
            ViewData["ItemAgendamentoId"] = new SelectList(_context.ItemAgendamentos, "ItemAgendamentoId", "Descricao", itemAgendamentoPrestador.ItemAgendamentoId);
            return View(itemAgendamentoPrestador);
        }

        public JsonResult GetPrestador(string term)
        {
            var examePrestador =  _context.Prestadores.Where(p => p.Nome.Contains(term)).Select(p => new {label = p.Nome, id=p.PrestadorId });
            return Json(examePrestador);
        }


        public async Task<JsonResult> Delete(int ItemAgendamentoId)
        {
            _logger.LogInformation("Deletando Associação de prestador com Item de agendamento");
            var itemAgendamentoPrestador = await _context.ItemAgendamentoPrestadores.FirstOrDefaultAsync(a => a.ItemAgendamentoId == ItemAgendamentoId);
            _context.ItemAgendamentoPrestadores.Remove(itemAgendamentoPrestador);
            await _context.SaveChangesAsync();
            TempData["Mensagem"] = "Excluido com sucesso";
            return Json("Excluido com Sucesso");
        }

       
        public async Task<bool> ItemPrestadorExiste(int ItemAgendamentoId,int PrestadorId)
        {
           return await _context.ItemAgendamentoPrestadores.AnyAsync(i => i.ItemAgendamentoId == ItemAgendamentoId &&  i.PrestadorId == PrestadorId);
           
        }

       
    }
}
