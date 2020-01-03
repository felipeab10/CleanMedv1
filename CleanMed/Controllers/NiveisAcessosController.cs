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
    //[Authorize(Roles = "ADMINISTRADOR")]
    public class NiveisAcessosController : Controller
    {
        private readonly Contexto _context;
        private readonly INivelAcessoRepositorio _nivelAcessoRepositorio;
        private readonly ILogger<NiveisAcessosController> _logger;

        public NiveisAcessosController(Contexto context, INivelAcessoRepositorio nivelAcessoRepositorio, ILogger<NiveisAcessosController> logger)
        {
            _context = context;
            _nivelAcessoRepositorio = nivelAcessoRepositorio;
            _logger = logger;
        }

        // GET: NiveisAcessos
        public async Task<IActionResult> Index(int? pageNumber, string searchId, string searchDescricao)
        {
            ViewData["searchId"] = searchId;
            ViewData["searchDescricao"] = searchDescricao;


            var niveisAcessos = from s in _context.NiveisAcessos
                                select s;
            if (!String.IsNullOrEmpty(searchId))
            {

                niveisAcessos = niveisAcessos.Where(s => s.Id == searchId);
            }
            if (!String.IsNullOrEmpty(searchDescricao))
            {
                niveisAcessos = niveisAcessos.Where(s => s.Descricao.Contains(searchDescricao));
            }
          
            ViewData["GrupoFaturamentoId"] = new SelectList(_context.GrupoFaturamentos, "GrupoFaturamentoId", "Descricao");
            int pageSize = 5;
            return View(await PaginatedList<NiveisAcesso>.CreateAsync(niveisAcessos.AsNoTracking().OrderBy(n => n.Name), pageNumber ?? 1, pageSize));
        }

       

        // GET: NiveisAcessos/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NiveisAcesso niveisAcesso)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Validando se o nível acesso existe");
                bool nivelExiste = await _nivelAcessoRepositorio.NivelAcessoExiste(niveisAcesso.Name);
                if (!nivelExiste)
                {
                    _logger.LogInformation("Adicionando novo nível de acesso");
                    niveisAcesso.Name = niveisAcesso.Name.ToUpper();
                    niveisAcesso.NormalizedName = niveisAcesso.Name.ToUpper();
                    _logger.LogInformation("Nível de acesso adicionado");
                    await _nivelAcessoRepositorio.Inserir(niveisAcesso);

                    TempData["Mensagem"] = "Adicionado com sucesso";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Mensagem"] = "Adicionado com sucesso";
                TempData["Validacao"] = "Nivel de acesso ja existe";
                return RedirectToAction(nameof(Index));
            }
            _logger.LogError("Erro ao adicionar nível de acesso");
            return View(niveisAcesso);
        }

        // GET: NiveisAcessos/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                _logger.LogError("nível de acesso não encontrado");
                return NotFound();
            }
            _logger.LogInformation("Localizando nível de acesso");
            var niveisAcesso = await _nivelAcessoRepositorio.PegarPeloId(id);
            if (niveisAcesso == null)
            {
                _logger.LogError("nível de acesso não encontrado");
                return NotFound();
            }
            TempData["ConcurrencyStamp"] = niveisAcesso.ConcurrencyStamp;
           
            return View(niveisAcesso);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,  NiveisAcesso niveisAcesso)
        {
            if (id != niveisAcesso.Id)
            {
                _logger.LogError("nível de acesso não encontrado");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Atualizando nível de acesso");
                niveisAcesso.Name = niveisAcesso.Name.ToUpper();
                niveisAcesso.NormalizedName = niveisAcesso.Name.ToUpper();
                await _nivelAcessoRepositorio.Atualizar(niveisAcesso);
                _logger.LogInformation("Nível de acesso atualizado");
                TempData["Mensagem"] = "Atualizado com sucesso";
               
                return RedirectToAction(nameof(Index));
            }
            _logger.LogError("erro ao atualizar, informações inválidas");
            return View(niveisAcesso);
        }

       

        
        [HttpPost]
      
        public async Task<JsonResult> Delete(string id)
        {

            _logger.LogInformation("Excluindo nível de acesso");
            await _nivelAcessoRepositorio.Excluir(id);
            _logger.LogInformation("Nível de acesso excluido");
            TempData["Mensagem"] = "Excluido com sucesso";
            return Json("Excluido com sucesso");
        }

      public async Task<JsonResult> NivelAcessoExiste(string NivelAcesso)
        {
            if(await _nivelAcessoRepositorio.NivelAcessoExiste(NivelAcesso)) 
            {
                return Json("Nível acesso já Cadastrado");
               
            }
            return Json(true);
        }
    }
}
