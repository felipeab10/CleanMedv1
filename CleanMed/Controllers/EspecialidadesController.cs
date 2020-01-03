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
    public class EspecialidadesController : Controller
    {
        private readonly IEspecialidadeRepositorio _especialidadeRepositorio;
        private readonly ILogger<EspecialidadesController> _logger;
        private readonly Contexto _context;
        private readonly IPrestadorEspecialidadeRepositorio _prestadorEspecialidadeRepositorio;

        public EspecialidadesController(Contexto context, IEspecialidadeRepositorio especialidadeRepositorio, ILogger<EspecialidadesController> logger, IPrestadorEspecialidadeRepositorio prestadorEspecialidadeRepositorio)
        {
            _especialidadeRepositorio = especialidadeRepositorio;
            _logger = logger;
            _context = context;
            _prestadorEspecialidadeRepositorio = prestadorEspecialidadeRepositorio;
        }

        // GET: Especialidades
        public async Task<IActionResult> Index(int? pageNumber, int searchId, string searchDescricao)
        {
            ViewData["CurrentFilter"] = searchId;
            ViewData["CurrentFilter"] = searchDescricao;

            var especialidade = from s in _context.Especialidades
                                select s;
            if (searchId > 0)
            {

                especialidade = especialidade.Where(s => s.EspecialidadeId == searchId);
            }
            if (!String.IsNullOrEmpty(searchDescricao))
            {
                especialidade = especialidade.Where(s => s.Descricao.Contains(searchDescricao.ToUpper()));
            }

            int pageSize = 5;
            return View(await PaginatedList<Especialidade>.CreateAsync(especialidade.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

      

        // GET: Especialidades/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Especialidade especialidade)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Adicionando Especialidade");
                await _especialidadeRepositorio.Inserir(especialidade);
                _logger.LogInformation("Especialidade Médica Adicionada");
                TempData["Mensagem"] = "Adicionado com sucesso";
                return RedirectToAction(nameof(Index));
            }
            _logger.LogError("Especialidade médica não esta valida");
            return View(especialidade);
        }

        // GET: Especialidades/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Especialidade Médica não encontrada");
                return NotFound();
            }

            _logger.LogInformation("Procurando Especialidade Médica");
            var especialidade = await _especialidadeRepositorio.PegarPeloId(id);
            if (especialidade == null)
            {
                _logger.LogError("Especialidade Médica não encontrada");
                return NotFound();
            }
            return View(especialidade);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Especialidade especialidade)
        {
            if (id != especialidade.EspecialidadeId)
            {
                _logger.LogError("Especialidade Médica diferente do que foi informado");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Atualizando Especialidade médica");
                await _especialidadeRepositorio.Atualizar(especialidade);
                _logger.LogInformation("Especialidade Médica Atualizada com sucesso");
                TempData["Mensagem"] = "Atualizado com sucesso";
                return RedirectToAction(nameof(Index));
            }
            return View(especialidade);
        }

        
        public async Task<JsonResult> EspecialidadeExiste(string Descricao, int EspecialidadeId)
        {
            if(EspecialidadeId == 0)
            {
                if (await _especialidadeRepositorio.EspecialidadeExiste(Descricao))
                    return Json("Especialidade Já Cadastrada");
                return Json(true);
            }
            if(await _especialidadeRepositorio.EspecialidadeExiste(Descricao, EspecialidadeId))
                return Json("Especialidade Já Cadastrada");
            return Json(true);
        }

        public async Task<IActionResult> Especialidade(int PrestadorId)
        {
            if(PrestadorId == 0)
            {
                _logger.LogError("Id do prestador não encontrado");
                return NotFound();
            }

            var especialidade = await (from e in _context.Especialidades
                                       join ep in _context.PrestadoresEspecialidades
                                       on e.EspecialidadeId equals ep.EspecialidadeId
                                       join p in _context.Prestadores
                                       on ep.PrestadorId equals p.PrestadorId
                                       where p.PrestadorId == PrestadorId
                                       select e).ToListAsync();
            
            ViewData["PrestadorId"] = PrestadorId;
            ViewData["EspecialidadeId"] = new SelectList(_context.Especialidades, "EspecialidadeId", "Descricao");
            return View(especialidade);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssociarEspecialidade(int EspecialidadeId, int PrestadorId)
        {
            var teste = await _context.PrestadoresEspecialidades.FirstOrDefaultAsync(a => a.EspecialidadeId == EspecialidadeId && a.PrestadorId == PrestadorId);
           if(teste != null)
            {
                TempData["Validacao"] = "Especialidade ja cadastrada para esse prestador";
                return RedirectToAction("Prestador", "Prestadores", new { PrestadorId = PrestadorId });
            }
            PrestadorEspecialidade prestadorEspecialidade = new PrestadorEspecialidade();
            prestadorEspecialidade.EspecialidadeId = EspecialidadeId;
            prestadorEspecialidade.PrestadorId = PrestadorId;
            _context.PrestadoresEspecialidades.Add(prestadorEspecialidade);
            await _context.SaveChangesAsync();
            TempData["Mensagem"] = "Adicionado com sucesso" ;
            return RedirectToAction("Prestador", "Prestadores", new { PrestadorId = PrestadorId });

        }
        public async Task<JsonResult> DesassociarEspecialidade(int EspecialidadeId,int PrestadorId)
        {
            var prestadorEspecialidade = await _context.PrestadoresEspecialidades.Where(e => e.EspecialidadeId == EspecialidadeId && e.PrestadorId == PrestadorId).FirstOrDefaultAsync();
            _context.PrestadoresEspecialidades.Remove(prestadorEspecialidade);
            await _context.SaveChangesAsync();
            TempData["Mensagem"] = "Excluido com sucesso";
            return Json("Excluido com sucesso");
        }
       public JsonResult LocalizarEspecialidade(int id)
        {
            var especialidade = _context.Especialidades.Where(e => e.EspecialidadeId == id).FirstOrDefault();
                return Json(especialidade);
        }
    }
}
