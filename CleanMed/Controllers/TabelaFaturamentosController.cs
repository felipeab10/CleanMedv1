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
    public class TabelaFaturamentosController : Controller
    {
        private readonly Contexto _context;
        private readonly ITabelaFaturamentoRepositorio _tabelaFaturamentoRepositorio;
        private readonly ILogger<TabelaFaturamentosController> _logger;
        public TabelaFaturamentosController(Contexto context, ITabelaFaturamentoRepositorio tabelaFaturamentoRepositorio, ILogger<TabelaFaturamentosController> logger)
        {
            _context = context;
            _logger = logger;
            _tabelaFaturamentoRepositorio = tabelaFaturamentoRepositorio;
        }

        // GET: TabelaFaturamentos
        public async Task<IActionResult> Index(int? pageNumber, int searchId, string searchDescricao,int ConvenioId)
        {
            ViewData["CurrentFilter"] = searchId;
            ViewData["CurrentFilter"] = searchDescricao;
            ViewData["ConvenioId"] = ConvenioId;
            TempData["ConvenioNome"] = _context.Convenios.Where(a => a.ConvenioId == ConvenioId).Select(a=>a.Nome).First();

            var tabelaFaturamento = from s in _context.TabelaFaturamentos
                                where s.ConvenioId == ConvenioId 
                                    select s;
            if (searchId > 0)
            {

                tabelaFaturamento = tabelaFaturamento.Where(s => s.TabelaFaturamentoId == searchId);
            }
            if (!String.IsNullOrEmpty(searchDescricao))
            {
                tabelaFaturamento = tabelaFaturamento.Where(s => s.Descricao.Contains(searchDescricao));
            }
           
           
            int pageSize = 5;
            return View(await PaginatedList<TabelaFaturamento>.CreateAsync(tabelaFaturamento.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: TabelaFaturamentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tabelaFaturamento = await _context.TabelaFaturamentos
                .FirstOrDefaultAsync(m => m.TabelaFaturamentoId == id);
            if (tabelaFaturamento == null)
            {
                return NotFound();
            }

            return View(tabelaFaturamento);
        }

        // GET: TabelaFaturamentos/Create
        public IActionResult Create(int id)
        {
            _logger.LogInformation("Abrindo view create tabela faturamento");
            ViewData["ConvenioId"] = id;
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TabelaFaturamento tabelaFaturamento,int ConvenioId)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Adicionando Tabela de Faturamento");
                await _tabelaFaturamentoRepositorio.Inserir(tabelaFaturamento);
                _logger.LogInformation("Tabela de faturamento adicionado");
                TempData["Mensagem"] = "Adicionado com sucesso";
                return RedirectToAction("Index","TabelaFaturamentos",new {ConvenioId = ConvenioId });
            }
            return View(tabelaFaturamento);
        }

        // GET: TabelaFaturamentos/Edit/5
        public async Task<IActionResult> Edit(int id, int ConvenioId)
        {
            if (id == 0)
            {
                _logger.LogError("Tabela de faturamento não localizado");
                return NotFound();
            }
            _logger.LogInformation("Localizando tabela de faturamento");
            var tabelaFaturamento = await _tabelaFaturamentoRepositorio.PegarPeloId(id);
            if (tabelaFaturamento == null)
            {
                _logger.LogError("Tabela de faturamento não localizado");
                return NotFound();
            }
            _logger.LogInformation("Retornando dados para view edit da tabela de faturamento");
            return View(tabelaFaturamento);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TabelaFaturamento tabelaFaturamento, int ConvenioId)
        {
            if (id != tabelaFaturamento.TabelaFaturamentoId)
            {
                _logger.LogError("tabela de faturamento diferente do banco");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Atualizando Tabela de faturamento");
                await _tabelaFaturamentoRepositorio.Atualizar(tabelaFaturamento);
                _logger.LogInformation("Tabela de faturamento atualizado");
                TempData["Mensagem"] = "Atualizado com sucesso";
                return RedirectToAction("Index", "TabelaFaturamentos", new { ConvenioId = ConvenioId });
            }
            _logger.LogError("Erro ao atualizar tabela de faturamento");
            return View(tabelaFaturamento);
        }

        public async Task<JsonResult> TabelaFaturamentoExiste(string Descricao,int TabelaFaturamentoId)
        {
            if(TabelaFaturamentoId == 0)
            {
                if (await _tabelaFaturamentoRepositorio.TabelaFaturamentoExiste(Descricao))
                    return Json("Tabela de faturamento já cadastrado");
                return Json(true);
            }
            if(await _tabelaFaturamentoRepositorio.TabelaFaturamentoExiste(Descricao, TabelaFaturamentoId))
                return Json("Tabela de faturamento já cadastrado");
            return Json(true);
        }
    }
}
