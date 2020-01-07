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
    public class GrupoFaturamentosController : Controller
    {
        private readonly Contexto _context;
        private readonly IGrupoFaturamentoRepositorio _grupoFaturamentoRepositorio;
        private readonly ILogger<GrupoFaturamentosController> _logger;

        public GrupoFaturamentosController(Contexto context, IGrupoFaturamentoRepositorio grupoFaturamentoRepositorio, ILogger<GrupoFaturamentosController> logger)
        {
            _context = context;
            _grupoFaturamentoRepositorio = grupoFaturamentoRepositorio;
            _logger = logger;
        }

        // GET: GrupoFaturamentos
        public async Task<IActionResult> Index(int? pageNumber, int searchId, string searchDescricao)
        {
            ViewData["CurrentFilter"] = searchId;
            ViewData["CurrentFilter"] = searchDescricao;

            var grupoFaturamentos = from s in _context.GrupoFaturamentos
                                    select s;
            if (searchId > 0)
            {

                grupoFaturamentos = grupoFaturamentos.Where(s => s.GrupoFaturamentoId == searchId);
            }
            if (!String.IsNullOrEmpty(searchDescricao))
            {
                grupoFaturamentos = grupoFaturamentos.Where(s => s.Descricao.Contains(searchDescricao));
            }

            int pageSize = 5;
            return View(await PaginatedList<GrupoFaturamento>.CreateAsync(grupoFaturamentos.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
           

        
        public IActionResult Create()
        {
            ViewData["TipoGrupoId"] = new SelectList(new[] {
            
            new {ID="Serviços Hospitalares",Name="Serviços Hospitalares"},
            new {ID="Serviços Profissionais",Name="Serviços Profissionais"},
            new {ID="Serviços Diagnósticos",Name="Serviços Diagnósticos"},
            new {ID="Medicamentos",Name="Medicamentos"},
            new {ID="Materiais",Name="Materiais"},
            new {ID="Medicamentos & Materiais",Name="Medicamentos & Materiais"},
            new {ID="Outros Lançamentos",Name="Outros Lançamentos"},

            }, "ID", "Name");
            ViewData["StatusId"] = new SelectList(new[] {
            new {ID="true",Name="Ativo"},
            new {ID="false",Name="Inativo"},
            }, "ID", "Name");
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GrupoFaturamento grupoFaturamento)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Adicionando novo Grupo de Faturamento");
                await _grupoFaturamentoRepositorio.Inserir(grupoFaturamento);
                _logger.LogInformation("Grupo de faturamento adicionado");
                TempData["Mensagem"] = "Adicionado com sucesso";
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoGrupoId"] = new SelectList(new[] {

            new {ID="Serviços Hospitalares",Name="Serviços Hospitalares"},
            new {ID="Serviços Profissionais",Name="Serviços Profissionais"},
            new {ID="Serviços Diagnósticos",Name="Serviços Diagnósticos"},
            new {ID="Medicamentos",Name="Medicamentos"},
            new {ID="Materiais",Name="Materiais"},
            new {ID="Medicamentos & Materiais",Name="Medicamentos & Materiais"},
            new {ID="Outros Lançamentos",Name="Outros Lançamentos"},

            }, "ID", "Name");
            ViewData["StatusId"] = new SelectList(new[] {
            new {ID="true",Name="Ativo"},
            new {ID="false",Name="Inativo"},
            }, "ID", "Name");
            _logger.LogError("Erro ao adicionar");
            return View(grupoFaturamento);
        }

        // GET: GrupoFaturamentos/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Grupo de faturamento não localizado");
                return NotFound();
            }

            var grupoFaturamento = await _grupoFaturamentoRepositorio.PegarPeloId(id);
            if (grupoFaturamento == null)
            {
                _logger.LogError("Grupo de faturamento não localizado");
                return NotFound();
            }
            ViewData["TipoGrupoId"] = new SelectList(new[] {

            new {ID="Serviços Hospitalares",Name="Serviços Hospitalares"},
            new {ID="Serviços Profissionais",Name="Serviços Profissionais"},
            new {ID="Serviços Diagnósticos",Name="Serviços Diagnósticos"},
            new {ID="Medicamentos",Name="Medicamentos"},
            new {ID="Materiais",Name="Materiais"},
            new {ID="Medicamentos & Materiais",Name="Medicamentos & Materiais"},
            new {ID="Outros Lançamentos",Name="Outros Lançamentos"},

            }, "ID", "Name");
            ViewData["StatusId"] = new SelectList(new[] {
            new {ID="true",Name="Ativo"},
            new {ID="false",Name="Inativo"},
            }, "ID", "Name");
            _logger.LogInformation("Abrindo view de edit");
            return View(grupoFaturamento);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  GrupoFaturamento grupoFaturamento)
        {
            if (id != grupoFaturamento.GrupoFaturamentoId)
            {
                _logger.LogError("Grupo de faturamento não localizado");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Atualizando grupo de faturametno");
                await _grupoFaturamentoRepositorio.Atualizar(grupoFaturamento);
                _logger.LogInformation("Grupo de faturamento atualizado");
                TempData["Mensagem"] = "Atualizado com sucesso";
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoGrupoId"] = new SelectList(new[] {

            new {ID="Serviços Hospitalares",Name="Serviços Hospitalares"},
            new {ID="Serviços Profissionais",Name="Serviços Profissionais"},
            new {ID="Serviços Diagnósticos",Name="Serviços Diagnósticos"},
            new {ID="Medicamentos",Name="Medicamentos"},
            new {ID="Materiais",Name="Materiais"},
            new {ID="Medicamentos & Materiais",Name="Medicamentos & Materiais"},
            new {ID="Outros Lançamentos",Name="Outros Lançamentos"},

            }, "ID", "Name");
            ViewData["StatusId"] = new SelectList(new[] {
            new {ID="true",Name="Ativo"},
            new {ID="false",Name="Inativo"},
            }, "ID", "Name");
            return View(grupoFaturamento);
        }

        public async Task<IActionResult> GrupoFaturamentoExiste(string Descricao,int GrupoFaturamentoId)
        {
            if(GrupoFaturamentoId == 0)
            {
                if (await _grupoFaturamentoRepositorio.GrupoFaturamentoExiste(Descricao))
                    return Json("Grupo de faturamento já cadastrado");
                return Json(true);
            }
            if(await _grupoFaturamentoRepositorio.GrupoFaturamentoExiste(Descricao,GrupoFaturamentoId))
                return Json("Grupo de faturamento já cadastrado");
            return Json(true);
        }
    }
}
