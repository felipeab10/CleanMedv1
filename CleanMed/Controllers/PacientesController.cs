using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanMed.Dados.Interface;
using CleanMed.Data;
using CleanMed.Models;
using CleanMed.Servicos;
using CleanMed.ViewModels;
using jsreport.AspNetCore;
using jsreport.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rotativa.AspNetCore;


namespace CleanMed.Controllers
{
    [Authorize]
    public class PacientesController : Controller
    {
        private readonly IPacienteRepositorio _pacienteRepositorio;
        private readonly ILogger<PacientesController> _logger;
        private readonly Contexto _contexto;
        public PacientesController(IPacienteRepositorio pacienteRepositorio, ILogger<PacientesController> logger,Contexto contexto)
        {
            _pacienteRepositorio = pacienteRepositorio;
            _logger = logger;
            _contexto = contexto;
        }
        public async Task<IActionResult> Index(int? pageNumber, string SearchString,int SearchPacienteId,DateTime? SearchPacienteData)
        {
            ViewData["currentFilter"] = SearchString;
            ViewData["SearchPacienteId"] = SearchPacienteId;
            ViewData["SearchPacienteData"] = SearchPacienteData;

            var paciente = from s in _contexto.Pacientes
                           select s;
            if (!String.IsNullOrEmpty(SearchString))
            {
                paciente = paciente.Where(a => a.Nome.Contains(SearchString.ToUpper())||a.CPF.Contains(SearchString));
            }
            if (SearchPacienteId != 0)
            {
                paciente = paciente.Where(a => a.PacienteId == SearchPacienteId);
            }
           
            if (SearchPacienteData != null)
            {
                DateTime validate = DateTime.Now.Date;
                DateTime outraData = Convert.ToDateTime("01/01/1918");
                if((SearchPacienteData <= validate || SearchPacienteData >= outraData)) { 
                //DateTime DtNascimento = DateTime.Parse(SearchPacienteData);
                paciente = paciente.Where(a => a.DataNascimento == SearchPacienteData);
                }
               
            }
            int pageSize = 7;
            return View( await PaginatedList<Paciente>.CreateAsync(paciente.AsNoTracking().OrderBy(a=> a.Nome), pageNumber ?? 1, pageSize));
        }
        public IActionResult Create()
        {
            var listSexo = new SelectList(new[] 
            { 
                new {Name = "Masculino",ID="Masculino"},
                new {Name = "Feminino",ID="Feminino"},
                new {Name = "Indefinido",ID="Indefinido"},
            }, "Name", "ID");
            ViewData["SexoId"] = listSexo;
            var listEstadoCivil = new SelectList(new[]
           {
                new {Name = "Solteiro(a)",ID="Solteiro(a)"},
                new {Name = "Casado(a)",ID="Casado(a)"},
                new {Name = "Viúvo(a)",ID="Viúvo(a)"},
                new {Name = "Separado judicialmente",ID="Separado judicialmente"},
                new {Name = "Divorciado(a)",ID="Divorciado(a)"},
            }, "Name", "ID");
            ViewData["EstadoCivilId"] = listEstadoCivil;

            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PacienteViewModel pacienteView,int? CEPBD)
        {
            if (ModelState.IsValid)
            {
               
                _logger.LogInformation("Adicionando novo Paciente");
                Paciente paciente = new Paciente();
                paciente.Nome = pacienteView.Nome.ToUpper();
               
                paciente.CPF = pacienteView.CPF;
                paciente.Sexo = pacienteView.Sexo;
                paciente.Estado_Civil = pacienteView.Estado_Civil;
                paciente.RG = pacienteView.RG;
                paciente.Telefone = pacienteView.Telefone;
                paciente.Numero = pacienteView.Numero;
                paciente.SemCPF = pacienteView.SemCPF;
                DateTime dt_cadastro = DateTime.Now;
                paciente.dt_cadastro = dt_cadastro;
                paciente.StatusId = true;
                DateTime validate = DateTime.Now.Date;
                DateTime outraData = Convert.ToDateTime("01/01/1918");
                if ((pacienteView.DataNascimento.Date > validate || pacienteView.DataNascimento.Date <= outraData))
                {
                    ModelState.AddModelError("DataNascimento", "informe uma data válida");
                    return View(pacienteView);
                }
                else
                {
                    paciente.DataNascimento = pacienteView.DataNascimento;
                }
                if (CEPBD == 0)
                {
                    Cep cep = new Cep();
                    cep.CEP = pacienteView.CEP;
                    cep.Logradouro = pacienteView.Logradouro;
                    cep.Bairro = pacienteView.Bairro;
                    cep.Cidade = pacienteView.Cidade;
                    cep.UF = pacienteView.UF;
                    cep.Complemento = pacienteView.Complemento;
                    _logger.LogInformation("Adicionando novo CEP");
                    _contexto.Cep.Add(cep);
                    await _contexto.SaveChangesAsync();
                    _logger.LogInformation("Associando cep ao paciente");
                    paciente.CepId = cep.CepId;
                }
                else
                {
                    paciente.CepId = CEPBD;
                }
                await _pacienteRepositorio.Inserir(paciente);
                _logger.LogInformation("Paciente Adicionado");
               TempData["Mensagem"] = "Adicionado com Sucesso!";
                return RedirectToAction("Index", "Pacientes");
            }
            _logger.LogError("Erro ao adicionar paciente");
            var listSexo = new SelectList(new[]
           {
                new {Name = "Masculino",ID="Masculino"},
                new {Name = "Feminino",ID="Feminino"},
                new {Name = "Indefinido",ID="Indefinido"},
            }, "Name", "ID");
            ViewData["SexoId"] = listSexo;
            var listEstadoCivil = new SelectList(new[]
           {
                new {Name = "Solteiro(a)",ID="Solteiro(a)"},
                new {Name = "Casado(a)",ID="Casado(a)"},
                new {Name = "Viúvo(a)",ID="Viúvo(a)"},
                new {Name = "Separado judicialmente",ID="Separado judicialmente"},
                new {Name = "Divorciado(a)",ID="Divorciado(a)"},
            }, "Name", "ID");
            ViewData["EstadoCivilId"] = listEstadoCivil;
           
            return View(pacienteView);
        }
       
        
        public  IActionResult Paciente(int PacienteId)
        {
            if(PacienteId == 0)
            {
                _logger.LogError("Paciente não encontrado");
                return NotFound();
            }
           
            ViewData["PacienteId"] = PacienteId;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int PacienteId,int? CEPBD,PacienteViewModel pacienteView)
        {
            if(PacienteId == 0)
            {
                _logger.LogError("Paciente não encontrado");
                return NotFound();
            }
          
            if (ModelState.IsValid)
            {
                var paciente = await _pacienteRepositorio.PegarPeloId(pacienteView.PacienteId);
                paciente.Nome = pacienteView.Nome;
                paciente.CPF = pacienteView.CPF;
                paciente.Sexo = pacienteView.Sexo;
                paciente.Estado_Civil = pacienteView.Estado_Civil;
                paciente.Telefone = pacienteView.Telefone;
                paciente.Email = pacienteView.Email;
                paciente.StatusId = pacienteView.StatusId;
                paciente.Numero = pacienteView.Numero;
                paciente.SemCPF = pacienteView.SemCPF;
                paciente.Email = pacienteView.Email;
                DateTime validate = DateTime.Now.Date;
                DateTime outraData = Convert.ToDateTime("01/01/1918");
                
                if ((pacienteView.DataNascimento.Date > validate || pacienteView.DataNascimento.Date <= outraData))
                {
                    ModelState.AddModelError("DataNascimento", "informe uma data válida");
                    ViewData["PacienteId"] = PacienteId;
                    return View("Paciente");
                }
                else
                {
                    paciente.DataNascimento = pacienteView.DataNascimento;
                }
                if (CEPBD == 0)
                {
                    Cep cep = new Cep();
                    cep.CEP = pacienteView.CEP;
                    cep.Logradouro = pacienteView.Logradouro;
                    cep.Bairro = pacienteView.Bairro;
                    cep.Cidade = pacienteView.Cidade;
                    cep.UF = pacienteView.UF;
                    cep.Complemento = pacienteView.Complemento;
                    _logger.LogInformation("Adicionando novo CEP");
                    _contexto.Cep.Add(cep);
                    await _contexto.SaveChangesAsync();
                    _logger.LogInformation("Associando cep ao paciente");
                    paciente.CepId = cep.CepId;
                }
                else
                {
                    paciente.CepId = CEPBD;
                }
                await _pacienteRepositorio.Atualizar(paciente);
                TempData["Mensagem"] = "Atualizado com sucesso";
                _logger.LogInformation("Atualizado com sucesso");
                return RedirectToAction("Index");
            }
            _logger.LogError("Erro ao atualizar paciente");
            ViewData["PacienteId"] = PacienteId;
            return View("Paciente");
        }

        public async Task<JsonResult> PacienteExisteCPF(string CPF , int PacienteId )
        {
            if(CPF == null)
            {
                return Json(false);
            }
            if(PacienteId == 0)
            {
                if (await _pacienteRepositorio.PacienteExisteCPF(CPF)) 
                { 
                  var paciente =  _contexto.Pacientes.Where(p => p.CPF == CPF).FirstOrDefault();
                    ViewData["PacienteId"] = paciente.PacienteId;
                    return Json(paciente);
                }
                return Json(false);

            }
            if (await _pacienteRepositorio.PacienteExisteCPF(CPF, PacienteId))
            {
                var paciente = _contexto.Pacientes.Where(p => p.CPF == CPF).FirstOrDefault();
                ViewData["PacienteId"] = paciente.PacienteId;
                return Json(paciente);
            }
                 return Json(false);
        }
        public async Task<JsonResult> PacienteExisteNome(string Nome,string DataNascimento, int PacienteId)
        {
                if(PacienteId == 0)
            {
                var dt = DataNascimento;
                DateTime oDate = Convert.ToDateTime(dt);
                if(await _pacienteRepositorio.PacienteExisteNome(Nome, oDate)) {
                    var paciente = _contexto.Pacientes.Where(p => p.Nome.ToUpper() == Nome.ToUpper() && p.DataNascimento ==  oDate).FirstOrDefault();
                     return Json(paciente);
                }
                return Json(true);
            }
            var dts = DataNascimento;
            DateTime oDates = Convert.ToDateTime(dts);
            if (await _pacienteRepositorio.PacienteExisteNome(Nome, oDates, PacienteId))
            {
                var paciente = _contexto.Pacientes.Where(p => p.Nome.ToUpper() == Nome.ToUpper() && p.DataNascimento == oDates).FirstOrDefault();
                return Json(paciente);
            }
                return Json(true);
        }

        public JsonResult LocalizarCEP(string CEP)
        {
            var endereco = _contexto.Cep.FirstOrDefault(a => a.CEP == CEP);

            return Json(endereco);
        }


        [MiddlewareFilter(typeof(JsReportPipeline))]
        public async Task<IActionResult> VisualizarPacientePDF(int PacienteId)
        {
            DateTime dt_atual = DateTime.Now;
            var paciente = await _contexto.Pacientes.FirstOrDefaultAsync(a => a.PacienteId == PacienteId);
            //var EnderecoPaciente = await _contexto.Enderecos.FirstOrDefaultAsync(a => a.PacienteId == PacienteId);
            PacienteViewModel pacienteViewModel = new PacienteViewModel();
            pacienteViewModel.Nome = paciente.Nome;
            pacienteViewModel.DataNascimento = paciente.DataNascimento;
            pacienteViewModel.CPF = paciente.CPF;
            pacienteViewModel.Sexo = paciente.Sexo;
            pacienteViewModel.Estado_Civil = paciente.Estado_Civil;
            pacienteViewModel.Email = paciente.Email;
            pacienteViewModel.RG = paciente.RG;
            pacienteViewModel.Telefone = paciente.Telefone;
            pacienteViewModel.StatusId = paciente.StatusId;
            pacienteViewModel.dt_cadastro = paciente.dt_cadastro;
           // pacienteViewModel.Enderecos = await _contexto.Enderecos.Where(e => e.PacienteId == PacienteId).ToListAsync();
            //pacienteViewModel.Contatos = await _contexto.Contatos.Where(e => e.PacienteId == PacienteId).ToListAsync();
            //pacienteViewModel.CEP = EnderecoPaciente.CEP;
            //pacienteViewModel.Logradouro = EnderecoPaciente.Logradouro;
            //pacienteViewModel.Bairro = EnderecoPaciente.Bairro;
            //pacienteViewModel.Cidade = EnderecoPaciente.Cidade;
            //pacienteViewModel.UF = EnderecoPaciente.UF;
            //pacienteViewModel.Numero = EnderecoPaciente.Numero;
            //pacienteViewModel.Complemento = EnderecoPaciente.Complemento;
            ViewBag.PacienteId = PacienteId;
            HttpContext.JsReportFeature().Recipe(Recipe.ChromePdf);
            return View("PDF",pacienteViewModel);
            //return new ViewAsPdf("PDF", pacienteViewModel) { FileName = "Ficha_" + paciente.Nome + "_" + dt_atual + ".pdf" };

        }
        public JsonResult GetPaciente(string term)
        {
            var Paciente = _contexto.Pacientes.Where(e => e.Nome.StartsWith(term) || e.DataNascimento.ToString() == term).Select(e => new { label = e.Nome, id = e.PacienteId });
            return Json(Paciente);
        }
       
        public async Task<JsonResult> addPacienteFromAgendamento(Paciente paciente)
        {
           
                _logger.LogInformation("Adicionando paciente através do agendamento,cadastro provisório");
                paciente.dt_cadastro = DateTime.Now;
                paciente.StatusId = true;    
                await _pacienteRepositorio.Inserir(paciente);
                return Json(true);
           
         
        }
        public JsonResult ValidaDataNascimento(DateTime DataNascimento)
        {
         
            if ((!_pacienteRepositorio.DataAniversario(DataNascimento)))
                return Json("Data inválida");
            return Json(true);
        }
    }
}