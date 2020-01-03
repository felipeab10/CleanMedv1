using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanMed.Dados.Interface;
using CleanMed.Data;
using CleanMed.Models;
using CleanMed.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMed.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR")]
    public class ExamesController : Controller
    {
        private readonly IExameRepositorio _exameRepositorio;
        private readonly ILogger<ExamesController> _logger;
        private readonly Contexto _contexto;
        public ExamesController(IExameRepositorio exameRepositorio, ILogger<ExamesController> logger, Contexto contexto)
        {
            _contexto = contexto;
            _exameRepositorio = exameRepositorio;
            _logger = logger;
        }
        public async Task<IActionResult> Index(int? pageNumber, int searchId, string searchDescricao)
        {
            ViewData["CurrentFilter"] = searchId;
            ViewData["CurrentFilter"] = searchDescricao;

            var Exames = from s in _contexto.Exames
                          select s;
            if (searchId > 0)
            {

                Exames = Exames.Where(s => s.ExameId == searchId);
            }
            if (!String.IsNullOrEmpty(searchDescricao))
            {
                Exames = Exames.Where(s => s.Descricao.Contains(searchDescricao));
            }

            int pageSize = 5;
            return View(await PaginatedList<Exame>.CreateAsync(Exames.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        public IActionResult Create()
        {
            _logger.LogInformation("Abrindo view create");
            ViewData["SetorId"] = new SelectList(_contexto.Setores, "SetorId", "Descricao");
            ViewData["EspecialidadeId"] = new SelectList(_contexto.Especialidades, "EspecialidadeId", "Descricao");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Exame exame)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Adicionando Exame");
                await _exameRepositorio.Inserir(exame);
                _logger.LogInformation("Exame Adicionado com sucesso");
                TempData["Mensagem"] = "Adicionado com sucesso";
                return RedirectToAction("Index");
            }
            _logger.LogError("Erro ao adicionar");
            ViewData["SetorId"] = new SelectList(_contexto.Setores, "SetorId", "Descricao");
            ViewData["EspecialidadeId"] = new SelectList(_contexto.Especialidades, "EspecialidadeId", "Descricao");
            return View(exame);
        }
        public async Task<IActionResult> Edit(int id)
        {
            if(id == 0)
            {
                _logger.LogWarning("Exame não encontrado");
                return NotFound();
            }
            _logger.LogInformation("Atualizando Exame");
            ViewData["SetorId"] = new SelectList(_contexto.Setores, "SetorId", "Descricao");
            ViewData["EspecialidadeId"] = new SelectList(_contexto.Especialidades, "EspecialidadeId", "Descricao");

            return View(await _exameRepositorio.PegarPeloId(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Exame exame)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Atualizando exame");
                await _exameRepositorio.Atualizar(exame);
                _logger.LogInformation("Exame atualizado");
                TempData["Mensagem"] = "Atualizado com sucesso";
                return RedirectToAction("Index");

            }
            _logger.LogError("erro ao atualizar");
            return View(exame);
        }
    }
}