using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanMed.Dados.Interface;
using CleanMed.Data;
using CleanMed.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMed.Controllers
{
    [Authorize]
    public class ContatosController : Controller
    {
        private readonly IContatoRepositorio _contatoRepositorio;
        private readonly Contexto _contexto;
        private readonly ILogger<ContatosController> _logger;
        public ContatosController(IContatoRepositorio contatoRepositorio, ILogger<ContatosController> logger,Contexto contexto)
        {
            _contatoRepositorio = contatoRepositorio;
            _logger = logger;
            _contexto = contexto;
        }
        public async Task<IActionResult> Index(int PacienteId)
        {
            if(PacienteId == 0)
            {
                _logger.LogError("Paciente não encontrado");
                return NotFound();
            }
            _logger.LogInformation("Buscando contatos do paciente");
            var contato = await _contexto.Contatos.Where(a => a.PacienteId == PacienteId).ToListAsync();
            ViewData["PacienteId"] = PacienteId;
            return View(contato);
        }
        public IActionResult Create(int PacienteId)
        {
            if(PacienteId == 0)
            {
                _logger.LogError("Paciente não encontrado");
                return NotFound();
            }
            ViewBag.PacienteId = PacienteId;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contato contato, int PacienteId)
        {
            if(contato.PacienteId != PacienteId)
            {
                _logger.LogError("Paciente diferente");
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Adicionando contato ao paciente");
                await _contatoRepositorio.Inserir(contato);
                _logger.LogInformation("contato adicionado ao paciente");
                TempData["Mensagem"] = "Adicionado com sucesso!";
                return RedirectToAction("Paciente", "Pacientes", new { PacienteId = contato.PacienteId });
            }
            _logger.LogError("Erro ao adicionar contato");
            return View(contato);
        }

      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Contato contato, int PacienteId)
        {
            if((PacienteId == 0 || contato.PacienteId != PacienteId ))
            {
                _logger.LogError("Paciente Não localizado");
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Atualizando Contato do paciente");
                await _contatoRepositorio.Atualizar(contato);
                _logger.LogInformation("Contato atualizado");
                TempData["Mensagem"] = "Atualizado com sucesso!";
                return RedirectToAction("Paciente", "Pacientes", new { PacienteId = contato.PacienteId });
            }
            _logger.LogError("Erro ao atualizar o contato");
            return View(contato);
        }
        public async Task<JsonResult> Delete(int id)
        {
            if(id == 0)
            {
                _logger.LogError("Contato não encontrado");
              
                return Json("Contato não encontrado");
            }
            _logger.LogInformation("Excluindo contato");
            await _contatoRepositorio.Excluir(id);
            _logger.LogInformation("Contato excluido");
            TempData["Mensagem"] = "Excluido com sucesso!";
            return Json("Excluido com sucesso");
        }
        public JsonResult PegarContato(int ContatoId)
        {
            var contato = _contexto.Contatos.Where(c => c.ContatoId == ContatoId).FirstOrDefault();
            return Json(contato);
        }
    }
}