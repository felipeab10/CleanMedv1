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
    public class RecursoAgendamentosController : Controller
    {
        private readonly Contexto _context;
        private readonly IRecursoAgendamentoRepositorio _recursoAgendamentoRepositorio;
        private readonly ILogger<RecursoAgendamentosController> _logger;

        public RecursoAgendamentosController(Contexto context, IRecursoAgendamentoRepositorio recursoAgendamentoRepositorio, ILogger<RecursoAgendamentosController> logger)
        {
            _context = context;
            _recursoAgendamentoRepositorio = recursoAgendamentoRepositorio;
            _logger = logger;
        }

        // GET: RecursoAgendamentos
        public async Task<IActionResult> Index(int? pageNumber, int searchId, string searchDescricao)
        {
            ViewData["CurrentFilter"] = searchId;
            ViewData["CurrentFilter"] = searchDescricao;


            var recursoAgendamento = from s in _context.RecursoAgendamentos
                                select s;
            if (searchId > 0)
            {

                recursoAgendamento = recursoAgendamento.Where(s => s.RecursoAgendamentoId == searchId);
            }
            if (!String.IsNullOrEmpty(searchDescricao))
            {
                recursoAgendamento = recursoAgendamento.Where(s => s.Descricao.Contains(searchDescricao));

            }
            ViewData["GrupoFaturamentoId"] = new SelectList(_context.GrupoFaturamentos, "GrupoFaturamentoId", "Descricao");
            int pageSize = 5;
            return View(await PaginatedList<RecursoAgendamento>.CreateAsync(recursoAgendamento.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: RecursoAgendamentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recursoAgendamento = await _context.RecursoAgendamentos
                .FirstOrDefaultAsync(m => m.RecursoAgendamentoId == id);
            if (recursoAgendamento == null)
            {
                return NotFound();
            }

            return View(recursoAgendamento);
        }

        // GET: RecursoAgendamentos/Create
        public IActionResult Create()
        {
            var listStatus = new SelectList(new[]
            {
            new{ID="true",Name="Ativo"},
            new{ID="false",Name="Inativo"},
            }, "ID", "Name");
            ViewData["Status"] = listStatus;

            var listTipo = new SelectList(new[]
            {
                new{ID="Equipamento",Name="Equipamento"},
                new{ID="Sala",Name="Sala"},
                new{ID="Prestador",Name="Prestador"},
                new{ID="Outros",Name="Outros"},
            }, "ID", "Name");
            ViewData["Tipo"] = listTipo;
            
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecursoAgendamento recursoAgendamento)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Adicionando Recurso para agendamento");
                await _recursoAgendamentoRepositorio.Inserir(recursoAgendamento);
                _logger.LogInformation("Recurso para agendamento adicionado com sucesso");
                TempData["Mensagem"] = "Adicionado com sucesso";
                return RedirectToAction(nameof(Index));
            }
            _logger.LogError("Erro ao adicionar recurso para agendamento");
            var listStatus = new SelectList(new[]
           {
           
            new{ID="true",Name="Ativo"},
            new{ID="false",Name="Inativo"},
            }, "ID", "Name");
            ViewData["Status"] = listStatus;

            var listTipo = new SelectList(new[]
            {
                new{ID="Equipamento",Name="Equipamento"},
                new{ID="Sala",Name="Sala"},
                new{ID="Prestador",Name="Prestador"},
                new{ID="Outros",Name="Outros"},
            }, "ID", "Name");
            ViewData["Tipo"] = listTipo;
            return View(recursoAgendamento);
        }

        // GET: RecursoAgendamentos/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Recurso para agendamento não encontrado");
                return NotFound();
            }

            var recursoAgendamento = await _recursoAgendamentoRepositorio.PegarPeloId(id);
            if (recursoAgendamento == null)
            {
                _logger.LogError("Recurso para agendamento não encontrado");
                return NotFound();
            }
            var listStatus = new SelectList(new[]
          {

            new{ID="true",Name="Ativo"},
            new{ID="false",Name="Inativo"},
            }, "ID", "Name");
            ViewData["Status"] = listStatus;

            var listTipo = new SelectList(new[]
            {
                new{ID="Equipamento",Name="Equipamento"},
                new{ID="Sala",Name="Sala"},
                new{ID="Prestador",Name="Prestador"},
                new{ID="Outros",Name="Outros"},
            }, "ID", "Name");
            ViewData["Tipo"] = listTipo;
            return View(recursoAgendamento);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  RecursoAgendamento recursoAgendamento)
        {
            if (id != recursoAgendamento.RecursoAgendamentoId)
            {
                _logger.LogError("Recurso para agendamento não encontrado");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Atualizando recurso para agendamento");
                await _recursoAgendamentoRepositorio.Atualizar(recursoAgendamento);
                _logger.LogInformation("Recurso para agendamento atualizado");
                TempData["Mensagem"] = "Atualizado com sucesso";
                return RedirectToAction(nameof(Index));
            }
            _logger.LogError("Erro ao atualizar o recurso para agendamento");
            var listStatus = new SelectList(new[]
         {

            new{ID="true",Name="Ativo"},
            new{ID="false",Name="Inativo"},
            }, "ID", "Name");
            ViewData["Status"] = listStatus;

            var listTipo = new SelectList(new[]
            {
                new{ID="Equipamento",Name="Equipamento"},
                new{ID="Sala",Name="Sala"},
                new{ID="Prestador",Name="Prestador"},
                new{ID="Outros",Name="Outros"},
            }, "ID", "Name");
            return View(recursoAgendamento);
        }

       public async Task<JsonResult> RecursoAgendamentoExiste(string Descricao, int RecursoAgendamentoId)
        {
            if(RecursoAgendamentoId == 0)
            {
                if (await _recursoAgendamentoRepositorio.RecursoAgendamentoExiste(Descricao))
                    return Json("Recurso já cadastrado");
                return Json(true);
            }
            if(await _recursoAgendamentoRepositorio.RecursoAgendamentoExiste(Descricao,RecursoAgendamentoId))
                return Json("Recurso já cadastrado");
            return Json(true);
        }
    }
}
