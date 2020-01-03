using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CleanMed.Data;
using CleanMed.Models;
using CleanMed.Dados.Interface;
using Microsoft.Extensions.Logging;
using CleanMed.Servicos;
using Microsoft.AspNetCore.Authorization;

namespace CleanMed.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR,FATURAMENTO,AGENDAMENTO")]
    public class ItemAgendamentosController : Controller
    {
        private readonly Contexto _context;
        private readonly IItemAgendamentoRepositorio _itemAgendamentoRepositorio;
        private readonly ILogger<ItemAgendamentosController> _logger;

        public ItemAgendamentosController(Contexto context, IItemAgendamentoRepositorio itemAgendamentoRepositorio, ILogger<ItemAgendamentosController> logger)
        {
            _context = context;
            _itemAgendamentoRepositorio = itemAgendamentoRepositorio;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int? pageNumber, int searchId, string searchDescricao, int searchExameId)
        {
            ViewData["CurrentFilter"] = searchId;
            ViewData["CurrentFilter"] = searchDescricao;


            var itemAgendamentos = from s in _context.ItemAgendamentos
                                select s;
            if (searchId > 0)
            {

                itemAgendamentos = itemAgendamentos.Where(s => s.ItemAgendamentoId == searchId);
            }
            if (!String.IsNullOrEmpty(searchDescricao))
            {
                itemAgendamentos = itemAgendamentos.Where(s => s.Descricao.Contains(searchDescricao));
            }
            if (searchExameId > 0)
            {

                itemAgendamentos = itemAgendamentos.Where(s => s.ExameId == searchExameId);
            }
            ViewData["ExameId"] = new SelectList(_context.Exames, "ExameId", "Descricao");
            int pageSize = 5;
            return View(await PaginatedList<ItemAgendamento>.CreateAsync(itemAgendamentos.AsNoTracking().Include(a => a.Exame), pageNumber ?? 1, pageSize));
        }


        // GET: ItemAgendamentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemAgendamento = await _context.ItemAgendamentos
                .Include(i => i.Exame)
                .FirstOrDefaultAsync(m => m.ItemAgendamentoId == id);
            if (itemAgendamento == null)
            {
                return NotFound();
            }

            return View(itemAgendamento);
        }

        // GET: ItemAgendamentos/Create
        public IActionResult Create()
        {
            _logger.LogInformation("Abrindo view create para adicionar novo item para agendamento");
            ViewData["ExameId"] = new SelectList(_context.Exames, "ExameId", "Descricao");
            ViewData["RecursoAgendamentoId"] = new SelectList(_context.RecursoAgendamentos, "RecursoAgendamentoId", "Descricao");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemAgendamento itemAgendamento)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Adicionando Item para agendamento");
                await _itemAgendamentoRepositorio.Inserir(itemAgendamento);
                _logger.LogInformation("Item para agendamento adicionado com sucesso");
                TempData["Mensagem"] = "Adicionado com sucesso";
                return RedirectToAction(nameof(Index));
            }
            _logger.LogError("Erro ao adicionar o item para agendamento");
            ViewData["ExameId"] = new SelectList(_context.Exames, "ExameId", "Descricao", itemAgendamento.ExameId);
            ViewData["RecursoAgendamentoId"] = new SelectList(_context.RecursoAgendamentos, "RecursoAgendamentoId", "Descricao");
            return View(itemAgendamento);
        }

        // GET: ItemAgendamentos/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Item para agendamento não encontrado");
                return NotFound();
            }

            var itemAgendamento = await _itemAgendamentoRepositorio.PegarPeloId(id);
            if (itemAgendamento == null)
            {
                _logger.LogError("Item para agendamento não encontrado");
                return NotFound();
            }
            _logger.LogInformation("Retornando para a view");
            ViewData["ExameId"] = new SelectList(_context.Exames, "ExameId", "Descricao", itemAgendamento.ExameId);
            ViewData["RecursoAgendamentoId"] = new SelectList(_context.RecursoAgendamentos, "RecursoAgendamentoId", "Descricao");
            return View(itemAgendamento);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  ItemAgendamento itemAgendamento)
        {
            if (id != itemAgendamento.ItemAgendamentoId)
            {
                _logger.LogError("Item para agendamento não encontrado");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Atualizando item para agendamento");
                await _itemAgendamentoRepositorio.Atualizar(itemAgendamento);
                _logger.LogInformation("Item para agendamento atualizado com sucesso");
                TempData["Mensagem"] = "Atualizado com sucesso";
                return RedirectToAction(nameof(Index));
            }
            _logger.LogError("Erro ao atualizar item para agendamento");
            ViewData["ExameId"] = new SelectList(_context.Exames, "ExameId", "Descricao", itemAgendamento.ExameId);
            ViewData["RecursoAgendamentoId"] = new SelectList(_context.RecursoAgendamentos, "RecursoAgendamentoId", "Descricao");
            return View(itemAgendamento);
        }

        public JsonResult GetExame(string term)
        {
            var Exames = _context.Exames.Where(e => e.Descricao.StartsWith(term)).Select(e => new { label = e.Descricao, id = e.ExameId });
            return Json(Exames);
        }

        public JsonResult GetRecurso(string term)
        {
            var Recurso = _context.RecursoAgendamentos.Where(e => e.Descricao.StartsWith(term)).Select(e => new { label = e.Descricao, id = e.RecursoAgendamentoId });
            return Json(Recurso);
        }
        public JsonResult GetItemAgendamento(string term)
        {
            var Recurso = _context.ItemAgendamentos.Where(e => e.Descricao.StartsWith(term)).Select(e => new { label = e.Descricao, id = e.ItemAgendamentoId });
            return Json(Recurso);
        }
        public async Task<JsonResult> ItemAgendamentoExiste(string Descricao, int ItemAgendamentoId)
        {
            if(ItemAgendamentoId == 0)
            {
                if (await _itemAgendamentoRepositorio.ItemAgendamentoExiste(Descricao))
                    return Json("Item para agendamento já cadastrado!");
                return Json(true);
            }
            if(await _itemAgendamentoRepositorio.ItemAgendamentoExiste(Descricao,ItemAgendamentoId))
                return Json("Item para agendamento já cadastrado!");
            return Json(true);
        }
       
    }
}
