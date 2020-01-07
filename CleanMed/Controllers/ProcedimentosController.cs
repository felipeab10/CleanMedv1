using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CleanMed.Data;
using CleanMed.Models;
using CleanMed.Servicos;
using CleanMed.Dados.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace CleanMed.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR,FATURAMENTO")]
    public class ProcedimentosController : Controller
    {
        private readonly Contexto _context;
        private readonly IProcedimentoRepositorio _procedimentoRepositorio;
        private readonly ILogger<ProcedimentosController> _logger;
        public ProcedimentosController(Contexto context, IProcedimentoRepositorio procedimentoRepositorio, ILogger<ProcedimentosController> logger)
        {
            _context = context;
            _procedimentoRepositorio = procedimentoRepositorio;
            _logger = logger;
        }

        // GET: Procedimentos
        public async Task<IActionResult> Index(int? pageNumber, int searchId, string searchDescricao,int searchGrupoFaturamentoId)
        {
            ViewData["CurrentFilter"] = searchId;
            ViewData["CurrentFilter"] = searchDescricao;
            

            var procedimentos = from s in _context.Procedimentos
                                    select s;
            if (searchId > 0)
            {

                procedimentos = procedimentos.Where(s => s.ProcedimentoId == searchId);
            }
            if (!String.IsNullOrEmpty(searchDescricao))
            {
                procedimentos = procedimentos.Where(s => s.Descricao.Contains(searchDescricao));
            }
            if (searchGrupoFaturamentoId > 0)
            {

                procedimentos = procedimentos.Where(s => s.GrupoFaturamentoId == searchGrupoFaturamentoId);
            }
            ViewData["GrupoFaturamentoId"] = new SelectList(_context.GrupoFaturamentos, "GrupoFaturamentoId", "Descricao");
            int pageSize = 5;
            return View(await PaginatedList<Procedimento>.CreateAsync(procedimentos.AsNoTracking().Include(a=> a.GrupoFaturamento), pageNumber ?? 1, pageSize));
        }


        // GET: Procedimentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var procedimento = await _context.Procedimentos
                .Include(p => p.GrupoFaturamento)
                .FirstOrDefaultAsync(m => m.ProcedimentoId == id);
            if (procedimento == null)
            {
                return NotFound();
            }

            return View(procedimento);
        }

        // GET: Procedimentos/Create
        public IActionResult Create()
        {
            _logger.LogInformation("Abrindo view create");
            ViewData["SexoId"] = new SelectList(new [] {

                new {ID = "Masculino", Name="Masculino"},
                new {ID = "Feminino", Name="Feminino"},
                new {ID = "Ambos", Name="Ambos"},

            }, "ID", "Name");
            ViewData["GrupoFaturamentoId"] = new SelectList(_context.GrupoFaturamentos, "GrupoFaturamentoId", "Descricao");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Procedimento procedimento)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Adicionando procedimento");
                await _procedimentoRepositorio.Inserir(procedimento);
                _logger.LogInformation("Procedimento adicionado");
                TempData["Mensagem"] = "Adicionado com sucesso";
                return RedirectToAction(nameof(Index));
            }
            ViewData["SexoId"] = new SelectList(new[] {

                new {ID = "Masculino", Name="Masculino"},
                new {ID = "Feminino", Name="Feminino"},
                new {ID = "Ambos", Name="Ambos"},

            }, "ID", "Name");
            _logger.LogError("Erro ao adicionar procedimento");
            ViewData["GrupoFaturamentoId"] = new SelectList(_context.GrupoFaturamentos, "GrupoFaturamentoId", "Descricao", procedimento.GrupoFaturamentoId);
            return View(procedimento);
        }

        // GET: Procedimentos/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Procedimento não encontrado");
                return NotFound();
            }

            var procedimento = await _procedimentoRepositorio.PegarPeloId(id);
            if (procedimento == null)
            {
                _logger.LogError("Procedimento não encontrado");
                return NotFound();
            }
            ViewData["SexoId"] = new SelectList(new[] {

                new {ID = "Masculino", Name="Masculino"},
                new {ID = "Feminino", Name="Feminino"},
                new {ID = "Ambos", Name="Ambos"},

            }, "ID", "Name");
            _logger.LogInformation("Retornando dados para a view edit do procedimento");
            ViewData["GrupoFaturamentoId"] = new SelectList(_context.GrupoFaturamentos, "GrupoFaturamentoId", "Descricao", procedimento.GrupoFaturamentoId);
            return View(procedimento);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Procedimento procedimento)
        {
            if (id != procedimento.ProcedimentoId)
            {
                _logger.LogError("Procedimento não encontrado");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Atualizando procedimento");
                await _procedimentoRepositorio.Atualizar(procedimento);
                _logger.LogInformation("Procedimento atualizado");
                TempData["Mensagem"] = "Atualizado com sucesso";
                return RedirectToAction(nameof(Index));
            }
            ViewData["SexoId"] = new SelectList(new[] {

                new {ID = "Masculino", Name="Masculino"},
                new {ID = "Feminino", Name="Feminino"},
                new {ID = "Ambos", Name="Ambos"},

            }, "ID", "Name");
            ViewData["GrupoFaturamentoId"] = new SelectList(_context.GrupoFaturamentos, "GrupoFaturamentoId", "Descricao", procedimento.GrupoFaturamentoId);
            return View(procedimento);
        }

        public async Task<JsonResult> ProcedimentoExiste(string Descricao, int ProcedimentoId)
        {
            if(ProcedimentoId == 0)
            {
                if (await _procedimentoRepositorio.ProcedimentoExiste(Descricao))
                    return Json("Procedimento já cadastrado");
                return Json(true);
            }
            if(await _procedimentoRepositorio.ProcedimentoExiste(Descricao, ProcedimentoId))
                return Json("Procedimento já cadastrado");
            return Json(true);
        }
    }
}
