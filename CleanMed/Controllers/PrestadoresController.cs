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
using CleanMed.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CleanMed.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR,FATURAMENTO,AGENDAMENTO,RECEPCAO")]
    public class PrestadoresController : Controller
    {
        private readonly Contexto _context;
        private readonly IPrestadorRepositorio _prestadorRepositorio;
        private readonly ILogger<PrestadoresController> _logger;
        public PrestadoresController(Contexto context, IPrestadorRepositorio prestadorRepositorio, ILogger<PrestadoresController> logger)
        {
            _context = context;
            _prestadorRepositorio = prestadorRepositorio;
            _logger = logger;
        }

        // GET: Prestadores
        public async Task<IActionResult> Index(int? pageNumber, int searchId, string searchNome, string searchCrm)
        {
            ViewData["CurrentFilter"] = searchId;
            ViewData["searchNome"] = searchNome;
            ViewData["searchCrm"] = searchCrm;

            var prestador = from s in _context.Prestadores
                            select s;
            if (searchId > 0)
            {

                prestador = prestador.Where(s => s.PrestadorId == searchId);
            }
            if (!String.IsNullOrEmpty(searchNome))
            {
                prestador = prestador.Where(s => s.Nome.Contains(searchNome));
            }
            if (!String.IsNullOrEmpty(searchCrm))
            {
                prestador = prestador.Where(s => s.NumeroCrm.Contains(searchCrm));
            }

            int pageSize = 10;
            return View(await PaginatedList<Prestador>.CreateAsync(prestador.AsNoTracking().OrderBy(a=> a.Nome), pageNumber ?? 1, pageSize));
        }

        
        public IActionResult Details(int PrestadorId)
        {
            if (PrestadorId == 0)
            {
                _logger.LogError("Prestador não localizado");
                return NotFound();
            }
            ViewData["PrestadorId"] = PrestadorId;

            return View();
        }
        public IActionResult Prestador(int PrestadorId)
        {
            if (PrestadorId == 0)
            {
                _logger.LogError("Prestador não localizado");
                return NotFound();
            }
            ViewData["PrestadorId"] = PrestadorId;
            return View();
        }

        // GET: Prestadores/Create
        public IActionResult Create()
        {
            ViewData["ConselhoId"] = new SelectList(_context.Conselhos, "ConselhoId", "Descricao");
            ViewData["TipoPrestadorId"] = new SelectList(_context.TipoPrestadores, "TipoPrestadorId", "Descricao");
            ViewData["SexoId"] = new SelectList(new[] {
            
                new {Name = "Masculino",ID="Masculino"},
                new {Name = "Feminino",ID="Feminino"},
                new {Name = "Indefinido",ID="Indefinido"},
            
            }, "Name", "ID");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrestadorViewModel prestadorView,int? CEPBD)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Adicionando Prestador");

                Prestador prestador = new Prestador();

                if (CEPBD == 0)
                {
                    Cep cep = new Cep();
                    cep.CEP = prestadorView.CEP;
                    cep.Logradouro = prestadorView.Logradouro;
                    cep.Bairro = prestadorView.Bairro;
                    cep.Cidade = prestadorView.Cidade;
                    cep.UF = prestadorView.UF;
                    cep.Complemento = prestadorView.Complemento;
                    _logger.LogInformation("Adicionando novo CEP");
                    _context.Cep.Add(cep);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Associando cep ao paciente");
                    prestador.CepId = cep.CepId;
                }
                else
                {
                    prestador.CepId = CEPBD;
                }
                prestador.Status = true;
                prestador.Nome = prestadorView.Nome;
                prestador.DataNascimento = prestadorView.DataNascimento;
                prestador.CPF = prestadorView.CPF;
                prestador.Sexo = prestadorView.Sexo;
                prestador.Telefone = prestadorView.Telefone;
                prestador.ConselhoId = prestadorView.ConselhoId;
                prestador.NumeroCrm = prestadorView.NumeroCrm;
                prestador.TipoPrestadorId = prestadorView.TipoPrestadorId;
                prestador.Numero = prestadorView.Numero;
                await _prestadorRepositorio.Inserir(prestador);
                _logger.LogInformation("Prestador Adicionado");
                TempData["Mensagem"] = "Adicionado com sucesso";
                return RedirectToAction("Index", "Prestadores");
            }
            ViewData["ConselhoId"] = new SelectList(_context.Conselhos, "ConselhoId", "Descricao", prestadorView.ConselhoId);
            ViewData["TipoPrestadorId"] = new SelectList(_context.TipoPrestadores, "TipoPrestadorId", "Descricao", prestadorView.TipoPrestadorId);
            ViewData["SexoId"] = new SelectList(new[] {

                new {Name = "Masculino",ID="Masculino"},
                new {Name = "Feminino",ID="Feminino"},
                new {Name = "Indefinido",ID="Indefinido"},

            }, "Name", "ID");
            return View(prestadorView);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( PrestadorViewModel prestadorView,int PrestadorId, int CEPBD)
        { 
            if (ModelState.IsValid)
            {
                
                _logger.LogInformation("Procurando Prestador");
                var prestador = await _prestadorRepositorio.PegarPeloId(prestadorView.PrestadorId);
                prestador.Nome = prestadorView.Nome;
                prestador.DataNascimento = prestadorView.DataNascimento;
                prestador.CPF = prestadorView.CPF;
                prestador.ConselhoId = prestadorView.ConselhoId;
                prestador.NumeroCrm = prestadorView.NumeroCrm;
                prestador.TipoPrestadorId = prestadorView.TipoPrestadorId;
                prestador.Telefone = prestadorView.Telefone;
                prestador.Email = prestadorView.Email;
                prestador.Status = prestadorView.Status;
                prestador.Numero = prestadorView.Numero;
                _logger.LogInformation("Atualizando registros do prestador");
                await _prestadorRepositorio.Atualizar(prestador);
                _logger.LogInformation("Prestador Atualizado");
            
                if(CEPBD == 0)
                {
                    Cep cep = new Cep();
                    cep.CEP = prestadorView.CEP;
                    cep.Logradouro = prestadorView.Logradouro;
                    cep.Bairro = prestadorView.Bairro;
                    cep.Cidade = prestadorView.Cidade;
                    cep.UF = prestadorView.UF;
                    cep.Complemento = prestadorView.Complemento;
                    _context.Add(cep);
                    await _context.SaveChangesAsync();
                    prestador.CepId = cep.CepId;
                    await _prestadorRepositorio.Atualizar(prestador);
                }else
                prestador.CepId = CEPBD;
                await _prestadorRepositorio.Atualizar(prestador);
                TempData["Mensagem"] = "Atualizado com sucesso";
                return RedirectToAction("Index", "Prestadores");

              
            }
         
            ViewData["ConselhoId"] = new SelectList(_context.Conselhos, "ConselhoId", "Descricao");
            ViewData["TipoPrestadorId"] = new SelectList(_context.TipoPrestadores, "TipoPrestadorId", "Descricao");
            _logger.LogError("Erro Ao atualizar");
            ViewData["PrestadorId"] = PrestadorId;
            return View("Prestador",prestadorView);
        }


        public async Task<JsonResult> PrestadorExisteNome(string Nome, string DataNascimento, int PrestadorId)
        {
            if (PrestadorId == 0)
            {
                var dt = DataNascimento;
                DateTime oDate = Convert.ToDateTime(dt);
                if (await _prestadorRepositorio.PrestadorExisteNome(Nome, oDate))
                {
                    var prestador = _context.Prestadores.Where(p => p.Nome.ToUpper() == Nome.ToUpper() && p.DataNascimento == oDate).FirstOrDefault();
                    return Json(prestador);
                }
                return Json(true);
            }
            var dts = DataNascimento;
            DateTime oDates = Convert.ToDateTime(dts);
            if (await _prestadorRepositorio.PrestadorExisteNome(Nome, oDates, PrestadorId))
            {
                var prestadores = _context.Prestadores.Where(p => p.Nome.ToUpper() == Nome.ToUpper() && p.DataNascimento == oDates).FirstOrDefault();
                return Json(prestadores);
            }
            return Json(true);
        }

        public async Task<JsonResult> PrestadorExisteCPF(string CPF, int PrestadorId)
        {
            if (PrestadorId == 0)
            {
               
                if (await _prestadorRepositorio.PrestadorExisteCPF(CPF))
                {
                    var prestador = _context.Prestadores.Where(p => p.CPF == CPF).FirstOrDefault();
                    return Json(prestador);
                }
                return Json(true);
            }
           
            if (await _prestadorRepositorio.PrestadorExisteCPF(CPF, PrestadorId))
            {
                var prestadores = _context.Prestadores.Where(p => p.CPF == CPF).FirstOrDefault();
                return Json(prestadores);
            }
            return Json(true);
        }


        public async Task<JsonResult> PrestadorExisteNumeroCrm(string NumeroCrm,int ConselhoId, int PrestadorId)
        {
            if (PrestadorId == 0)
            {

                if (await _prestadorRepositorio.PrestadorExisteNumeroCrm(NumeroCrm, ConselhoId))
                {
                    var prestador = _context.Prestadores.Where(p => p.NumeroCrm == NumeroCrm && p.ConselhoId == ConselhoId).FirstOrDefault();
                    return Json(prestador);
                }
                return Json(true);
            }

            if (await _prestadorRepositorio.PrestadorExisteNumeroCrm(NumeroCrm, ConselhoId, PrestadorId))
            {
                var prestadores = _context.Prestadores.Where(p => p.NumeroCrm == NumeroCrm && p.ConselhoId == ConselhoId).FirstOrDefault();
                return Json(prestadores);
            }
            return Json(true);
        }



        public JsonResult LocalizarCEP(string CEP)
        {
           var endereco = _context.Cep.FirstOrDefault(a => a.CEP == CEP);
          
            return Json(endereco);
        }
        public JsonResult ValidaDataNascimento(string DataNascimento)
        {
            var dt = DateTime.Parse(DataNascimento);
            if ((!_prestadorRepositorio.DataAniversario(dt)))
                return Json("Data inválida");
            return Json(true);
        }
    }
}
