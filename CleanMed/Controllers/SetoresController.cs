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
    //[Authorize(Roles = "ADMINISTRADOR")]
    public class SetoresController : Controller
    {
        private readonly ISetorRepositorio _setorRepositorio;
        private readonly ILogger<SetoresController> _logger;
        private readonly Contexto _contexto;
        public SetoresController(ISetorRepositorio setorRepositorio, ILogger<SetoresController> logger, Contexto contexto)
        {
            _setorRepositorio = setorRepositorio;
            _logger = logger;
            _contexto = contexto;
        }

        public async Task<IActionResult> Index(int? pageNumber, int searchId, string searchDescricao)
        {
            ViewData["CurrentFilter"] = searchId;
            ViewData["CurrentFilter"] = searchDescricao;

            var Setores = from s in _contexto.Setores
                                select s;
            if (searchId > 0)
            {

                Setores = Setores.Where(s => s.SetorId == searchId);
            }
            if (!String.IsNullOrEmpty(searchDescricao))
            {
                Setores = Setores.Where(s => s.Descricao.Contains(searchDescricao));
            }

            int pageSize = 5;
            return View(await PaginatedList<Setor>.CreateAsync(Setores.AsNoTracking().OrderBy(s => s.Descricao), pageNumber ?? 1, pageSize));
        }
        public IActionResult Create()
        {
            _logger.LogInformation("Carregando View de Create");
            ViewData["TipoSetorId"] = new SelectList(new[] {
                new {Name = "Ambulatorio",ID = "Ambulatorio"},
                new {Name = "Exame",ID = "Exame"},
                new {Name = "Externo",ID = "Externo"},
                new {Name = "Administrativo",ID = "Administrativo"},
                }, "Name", "ID").OrderBy(c => c.Text);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Setor setor)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Adicionando setor");
                await _setorRepositorio.Inserir(setor);
                TempData["Mensagem"] = "Adicionado com sucesso!";
                _logger.LogInformation("Setor Adicionado");
               
                return RedirectToAction("Index");
            }
            ViewData["TipoSetorId"] = new SelectList(new[] {
                new {Name = "Ambulatorio",ID = "Ambulatorio"},
                new {Name = "Exame",ID = "Exame"},
                new {Name = "Externo",ID = "Externo"},
                new {Name = "Administrativo",ID = "Administrativo"},
                }, "Name", "ID").OrderBy(c => c.Text);
            _logger.LogError("Erro ao adicionar setor");
            return View(setor);
        }
        public async Task<IActionResult> Edit(int id)
        {
            if(id == 0)
            {
                _logger.LogError("Setor não encontrado");
                return NotFound();
            }
            _logger.LogInformation("Localizando setor");
            var setor = await _setorRepositorio.PegarPeloId(id);
            ViewData["TipoSetorId"] = new SelectList(new[] {
                new {Name = "Ambulatorio",ID = "Ambulatorio"},
                new {Name = "Exame",ID = "Exame"},
                new {Name = "Externo",ID = "Externo"},
                new {Name = "Administrativo",ID = "Administrativo"},
                }, "Name", "ID").OrderBy(c => c.Text);
            return View(setor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Setor setor)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Atualizando setor");
                await _setorRepositorio.Atualizar(setor);
                _logger.LogInformation("Setor Atualizado");
                TempData["Mensagem"] = "Atualizado com sucesso";
                return RedirectToAction("Index");

            }
            ViewData["TipoSetorId"] = new SelectList(new[] {
                new {Name = "Ambulatorio",ID = "Ambulatorio"},
                new {Name = "Exame",ID = "Exame"},
                new {Name = "Externo",ID = "Externo"},
                new {Name = "Administrativo",ID = "Administrativo"},
                }, "Name", "ID").OrderBy(c => c.Text);
            _logger.LogError("Erro ao atualizar setor");
            return View(setor);
        }
        public JsonResult GetSetor(string term)
        {
            var setor = _contexto.Setores.Where(s => s.Descricao.Contains(term.ToUpper())).Select(s => new { label = s.Descricao, id = s.SetorId });
            return Json(setor);
        }
    }
}