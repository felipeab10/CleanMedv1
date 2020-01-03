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
using CleanMed.Servicos;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace CleanMed.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR,FATURAMENTO")]
    public class ConselhosController : Controller
    {
        private readonly IConselhoRepositorio _conselhoRepositorio;
        private readonly Contexto _context;
        private readonly ILogger<ConselhosController> _logger;

        public ConselhosController(Contexto context, IConselhoRepositorio conselhoRepositorio, ILogger<ConselhosController> logger)
        {
            _logger = logger;
            _conselhoRepositorio = conselhoRepositorio;
            _context = context;
        }

        // GET: Conselhos
        public async Task<IActionResult> Index(int? pageNumber, int searchId,string searchDescricao)
        {
            ViewData["CurrentFilter"] = searchId;
            ViewData["CurrentFilter"] = searchDescricao;

            var Conselhos = from s in _context.Conselhos
                                select s;
                if (searchId > 0)
                {
                    
                Conselhos = Conselhos.Where(s => s.ConselhoId == searchId);
                }
            if (!String.IsNullOrEmpty(searchDescricao))
            {
                Conselhos = Conselhos.Where(s => s.Descricao.Contains(searchDescricao.ToUpper()));
            }

                int pageSize = 5;
                return View(await PaginatedList<Conselho>.CreateAsync(Conselhos.AsNoTracking().OrderBy(c => c.Descricao), pageNumber ?? 1, pageSize));
            
           
        }
        
       

        // GET: Conselhos/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConselhoId,Descricao")] Conselho conselho)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Adicionando Conselho");
                await _conselhoRepositorio.Inserir(conselho);
                _logger.LogInformation("Conselho Adicionado");
                TempData["Mensagem"] = "Adicionado com Sucesso.";
                return RedirectToAction(nameof(Index));
            }
            _logger.LogError("Erro ao Adicionar o conselho");
            return View(conselho);
        }

        // GET: Conselhos/Edit/5
        public async Task<IActionResult> Edit(int id)
        {

            _logger.LogInformation("Buscando Conselho");
            var conselho = await _conselhoRepositorio.PegarPeloId(id);
            if (conselho == null)
            {
                _logger.LogError("Conselho não encontrado");
                return NotFound();
            }
            return View(conselho);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConselhoId,Descricao")] Conselho conselho)
        {
            if (id != conselho.ConselhoId)
            {
                _logger.LogError("Conselho não encontrado");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Atualizando Conselho");
                await _conselhoRepositorio.Atualizar(conselho);
                _logger.LogInformation("Conselho Atualizado");
                TempData["Mensagem"] = "Atualizado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            return View(conselho);
        }
        public async Task<JsonResult> CRMExiste(string Descricao,int ConselhoId)
        {
            if(ConselhoId == 0)
            {
                if (await _conselhoRepositorio.CRMExiste(Descricao))
                    return Json("Conselho já cadastrado");
                    return Json(true);
            }
            if (await _conselhoRepositorio.CRMExiste(Descricao, ConselhoId))
                return Json("Conselho já cadastrado");
                 return Json(true);

        }
        public JsonResult LocalizarConselho(int id)
        {
            var conselho = _context.Conselhos.Where(c => c.ConselhoId == id).FirstOrDefault();
            return Json(conselho);
        }


    }
}
