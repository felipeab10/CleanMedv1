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
    [Authorize(Roles = "ADMINISTRADOR,FATURAMENTO")]
    public class TipoPrestadoresController : Controller
    {
        private readonly Contexto _context;
        private readonly ITipoPrestadorRepositorio _tipoPrestadorRepositorio;
        private readonly ILogger<TipoPrestadoresController> _logger;

        public TipoPrestadoresController(Contexto context, ITipoPrestadorRepositorio tipoPrestadorRepositorio, ILogger<TipoPrestadoresController> logger)
        {
            _tipoPrestadorRepositorio = tipoPrestadorRepositorio;
            _logger = logger;
            _context = context;
        }

        // GET: TipoPrestadores
        public async Task<IActionResult> Index(int? pageNumber, int searchId, string searchDescricao)
        {
            ViewData["CurrentFilter"] = searchId;
            ViewData["CurrentFilter"] = searchDescricao;

            var tipoPrestador = from s in _context.TipoPrestadores
                            select s;
            if (searchId > 0)
            {

                tipoPrestador = tipoPrestador.Where(s => s.TipoPrestadorId == searchId);
            }
            if (!String.IsNullOrEmpty(searchDescricao))
            {
                tipoPrestador = tipoPrestador.Where(s => s.Descricao.Contains(searchDescricao));
            }

            int pageSize = 5;
            return View(await PaginatedList<TipoPrestador>.CreateAsync(tipoPrestador.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

      
       

        // GET: TipoPrestadores/Create
        public IActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TipoPrestadorId,Descricao")] TipoPrestador tipoPrestador)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Adicionando Tipo de prestador");
                await _tipoPrestadorRepositorio.Inserir(tipoPrestador);
                _logger.LogInformation("Tipo de prestador adicionado");
                TempData["Mensagem"] = "Adicionado com sucesso";
                return RedirectToAction(nameof(Index));
            }
            _logger.LogError("Erro ao adicionar Tipo de Prestador");
            return View(tipoPrestador);
        }

        // GET: TipoPrestadores/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Tipo de Prestador não encontrado");
                return NotFound();
            }
            _logger.LogInformation("Procurando tipo de prestador");
            var tipoPrestador = await _tipoPrestadorRepositorio.PegarPeloId(id);
            if (tipoPrestador == null)
            {
                _logger.LogError("Tipo de Prestador não encontrado");
                return NotFound();
            }
            return View(tipoPrestador);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TipoPrestadorId,Descricao")] TipoPrestador tipoPrestador)
        {
            if (id != tipoPrestador.TipoPrestadorId)
            {
                _logger.LogError("Tipo de Prestador não encontrado");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Atualizando Tipo de prestador");
                await _tipoPrestadorRepositorio.Atualizar(tipoPrestador);
                _logger.LogInformation("Tipo de prestador atualizado com sucessso");
                TempData["Mensagem"] = "Atualizado com sucesso";
                return RedirectToAction(nameof(Index));
            }
            return View(tipoPrestador);
        }

        

      

      public async Task<JsonResult> TipoPrestadorExiste(string Descricao, int TipoPrestadorId)
        {
            if(TipoPrestadorId == 0)
            {
                if (await _tipoPrestadorRepositorio.TipoPrestadorExiste(Descricao))
                    return Json("Tipo de Prestador já cadastrado");
                    return Json(true);
            }
            if(await _tipoPrestadorRepositorio.TipoPrestadorExiste(Descricao,TipoPrestadorId))
                return Json("Tipo de Prestador já cadastrado");
            return Json(true);
        }
        public JsonResult LocalizarTipoPrestador(int id)
        {
            var tipoPrestador = _context.TipoPrestadores.Where(t => t.TipoPrestadorId == id).FirstOrDefault();
                return Json(tipoPrestador);
        }
    }
}
