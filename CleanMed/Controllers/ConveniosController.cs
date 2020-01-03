using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CleanMed.Dados.Interface;
using CleanMed.Data;
using CleanMed.Models;
using CleanMed.Servicos;
using CleanMed.ViewModels;
using jsreport.AspNetCore;
using jsreport.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rotativa.AspNetCore;

namespace CleanMed.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR,FATURAMENTO")]
    public class ConveniosController : Controller
    {
        private readonly IConvenioRepositorio _convenioRepositorio;
        private readonly ILogger<ConveniosController> _logger;
        private readonly Contexto _contexto;
        private readonly IHostingEnvironment _hostingEnvironment;
        public ConveniosController(IConvenioRepositorio convenioRepositorio, ILogger<ConveniosController> logger, Contexto contexto, IHostingEnvironment hostingEnvironment)
        {
            _convenioRepositorio = convenioRepositorio;
            _logger = logger;
            _contexto = contexto;
            _hostingEnvironment = hostingEnvironment; 
        }
        public async Task<IActionResult> Index(int? pageNumber, int searchId, string searchDescricao)
        {
            _logger.LogInformation("Buscando todos os convênios cadastrados");
            ViewData["CurrentFilter"] = searchId;
            ViewData["CurrentFilter"] = searchDescricao;

            var convenios = from s in _contexto.Convenios
                                select s;
            if (searchId > 0)
            {

                convenios = convenios.Where(s => s.ConvenioId == searchId);
            }
            if (!String.IsNullOrEmpty(searchDescricao))
            {
                convenios = convenios.Where(s => s.Nome.Contains(searchDescricao));
            }

            int pageSize = 5;
            return View(await PaginatedList<Convenio>.CreateAsync(convenios.AsNoTracking().OrderBy(c => c.Nome), pageNumber ?? 1, pageSize));
        }
        public IActionResult Create()
        {
            _logger.LogInformation("Pagina de Criação");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ConvenioViewModel convenioView,int? CEPBD, IFormFile Logo)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Adicionando convênio");
                Convenio convenio = new Convenio();
                convenio.Nome = convenioView.Nome;
                convenio.CNPJ = convenioView.CNPJ;
                convenio.RazaoSocial = convenioView.RazaoSocial;
                convenio.Telefone = convenioView.Telefone;
                convenio.InscricaoMunicipal = convenioView.InscricaoMunicipal;
                convenio.InscricaoEstadual = convenioView.InscricaoEstadual;
                convenio.RegistroAns = convenioView.RegistroAns;
                convenio.Email = convenioView.Email;
                convenio.Status = true;
                convenio.Numero = convenioView.Numero;
                if(Logo != null)
                {
                    _logger.LogInformation("Criando link da pasta");
                   
                   var fileName = ContentDispositionHeaderValue.Parse(Logo.ContentDisposition).FileName.Trim('"');
                    //Assigning Unique Filename (Guid)
                    var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                    //Getting file Extension
                    var FileExtension = Path.GetExtension(fileName);

                    // concating  FileName + FileExtension
                   var newFileName = myUniqueFileName + FileExtension;
                    // Combines two strings into a path.
                    fileName = Path.Combine(_hostingEnvironment.WebRootPath, "Imagens") + $@"\{newFileName}";
                    // if you want to store path of folder in database
                   

                    using (FileStream fs = System.IO.File.Create(fileName))
                    {
                        Logo.CopyTo(fs);
                        fs.Flush();
                        convenio.Logo = "~/Imagens/" + newFileName;
                    }
                }

                if(CEPBD == 0)
                {
                        _logger.LogInformation("Adicionando novo endereço ao banco");
                        Cep ceps = new Cep();
                        ceps.Logradouro = convenioView.Logradouro;
                        ceps.CEP = convenioView.CEP;
                        ceps.Bairro = convenioView.Bairro;
                        ceps.Cidade = convenioView.Cidade;
                        ceps.UF = convenioView.UF;
                        ceps.Complemento = convenioView.Complemento;
                        _contexto.Add(ceps);
                        await _contexto.SaveChangesAsync();
                        _logger.LogInformation("associando ao convênio");
                        convenio.CepId = ceps.CepId;

                }
                else
                {
                    convenio.CepId = CEPBD;
                }
                await _convenioRepositorio.Inserir(convenio);
                _logger.LogInformation("Convênio Adicionado");
                TempData["Mensagem"] ="Adicionado com sucesso!";
                return RedirectToAction("Index");
            }
            _logger.LogError("Erro ao adicionar");
            return View(convenioView);
        }
        public async Task<IActionResult> Edit(int id)
        {
            if(id == 0)
            {
                _logger.LogInformation("Convênio não encontrado");
                return NotFound();
            }
            var convenio = await _convenioRepositorio.PegarPeloId(id);
            ConvenioViewModel convenioView = new ConvenioViewModel();
            convenioView.Nome = convenio.Nome;
            convenioView.CNPJ = convenio.CNPJ;
            convenioView.RazaoSocial = convenio.RazaoSocial;
            convenioView.Telefone = convenio.Telefone;
            convenioView.InscricaoMunicipal = convenio.InscricaoMunicipal;
            convenioView.InscricaoEstadual = convenio.InscricaoEstadual;
            convenioView.RegistroAns = convenio.RegistroAns;
            convenioView.Email = convenio.Email;
            convenioView.Numero = convenio.Numero;
            convenioView.ConvenioId = convenio.ConvenioId;
            convenioView.Logo = convenio.Logo;
            TempData["DtCadastro"] = convenio.DtCadastro;
            if (convenio.CepId != null)
            {
                var cep = await _contexto.Cep.FirstOrDefaultAsync(a => a.CepId == convenio.CepId);
                convenioView.CEP = cep.CEP;
                convenioView.Logradouro = cep.Logradouro;
                convenioView.Bairro = cep.Bairro;
                convenioView.Cidade = cep.Cidade;
                convenioView.UF = cep.UF;
                convenioView.Complemento = cep.Complemento;
                convenioView.CepId = cep.CepId;
            }
            TempData["DtCadastro"] = convenio.DtCadastro;
            return View(convenioView);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,ConvenioViewModel convenioView,DateTime DtCadastro,int? CEPBD, IFormFile Logo)
        {
            if(id == 0)
            {
                _logger.LogError("Convênio não encontrado");
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _logger.LogInformation("atualizando convênio");
                var convenio = await _convenioRepositorio.PegarPeloId(id);
                convenio.Nome = convenioView.Nome;
                convenio.CNPJ = convenioView.CNPJ;
                convenio.RazaoSocial = convenioView.RazaoSocial;
                convenio.Telefone = convenioView.Telefone;
                convenio.InscricaoMunicipal = convenioView.InscricaoMunicipal;
                convenio.InscricaoEstadual = convenioView.InscricaoEstadual;
                convenio.RegistroAns = convenioView.RegistroAns;
                convenio.Email = convenioView.Email;
                convenio.Status = true;
                convenio.Numero = convenioView.Numero;
                convenio.DtCadastro = DtCadastro;
                convenio.DtAlteracao = DateTime.Now;
                if (Logo != null)
                {
                    if(convenio.Logo != null)
                    {
                        string LogoConvenio = convenio.Logo;
                        LogoConvenio = LogoConvenio.Replace("~", "wwwroot");
                        System.IO.File.Delete(LogoConvenio);
                    }
                   
                    _logger.LogInformation("Criando link da pasta");

                    var fileName = ContentDispositionHeaderValue.Parse(Logo.ContentDisposition).FileName.Trim('"');
                    //Assigning Unique Filename (Guid)
                    var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                    //Getting file Extension
                    var FileExtension = Path.GetExtension(fileName);

                    // concating  FileName + FileExtension
                    var newFileName = myUniqueFileName + FileExtension;
                    // Combines two strings into a path.
                    fileName = Path.Combine(_hostingEnvironment.WebRootPath, "Imagens") + $@"\{newFileName}";
                    // if you want to store path of folder in database


                    using (FileStream fs = System.IO.File.Create(fileName))
                    {
                        Logo.CopyTo(fs);
                        fs.Flush();
                        convenio.Logo = "~/Imagens/" + newFileName;
                    }
                }
                if (CEPBD == 0)
                {
                        _logger.LogInformation("Adicionando novo endeço ao banco");
                        Cep ceps = new Cep();
                        ceps.Logradouro = convenioView.Logradouro;
                        ceps.CEP = convenioView.CEP;
                        ceps.Bairro = convenioView.Bairro;
                        ceps.Cidade = convenioView.Cidade;
                        ceps.UF = convenioView.UF;
                        ceps.Complemento = convenioView.Complemento;
                        _contexto.Add(ceps);
                        await _contexto.SaveChangesAsync();
                        _logger.LogInformation("associando ao convênio");
                        convenio.CepId = ceps.CepId;
                }
                else
                {
                    convenio.CepId = CEPBD;
                }
                await _convenioRepositorio.Atualizar(convenio);
                _logger.LogInformation("Convênio Atualizado");
                TempData["Mensagem"] = "Atualizado com sucesso!";
                return RedirectToAction("Index");

            }
            _logger.LogError("Erro ao atualizar");
            return View(convenioView);
        }
        public async Task<JsonResult> ConvenioExiste(string CNPJ, int ConvenioId)
        {
            if(ConvenioId == 0)
            {
                if (await _convenioRepositorio.ConvenioExiste(CNPJ))
                    return Json("CNPJ já cadastrado!");
                return Json(true);
            }
            if (await _convenioRepositorio.ConvenioExiste(CNPJ, ConvenioId))
                return Json("CNPJ já cadastrado!");
            return Json(true);
        }
        public async Task<JsonResult> ConvenioExisteRegistroANS(string RegistroAns, int ConvenioId)
        {
            if (ConvenioId == 0)
            {
                if (await _convenioRepositorio.ConvenioExisteRegistroANS(RegistroAns))
                    return Json("Registro ANS já cadastrado!");
                return Json(true);
            }
            if (await _convenioRepositorio.ConvenioExisteRegistroANS(RegistroAns, ConvenioId))
                return Json("Registro ANS já cadastrado!");
            return Json(true);
        }

        [MiddlewareFilter(typeof(JsReportPipeline))]
        public async Task<IActionResult> VisualizarConvenioPDF(int ConvenioId)
        {
            if (ConvenioId == 0)
            {
                _logger.LogInformation("Convênio não encontrado");
                return NotFound();
            }
            DateTime dt_atual = DateTime.Now;
            var convenio = await _convenioRepositorio.PegarPeloId(ConvenioId);
            ConvenioViewModel convenioView = new ConvenioViewModel();
            convenioView.Nome = convenio.Nome;
            convenioView.CNPJ = convenio.CNPJ;
            convenioView.RazaoSocial = convenio.RazaoSocial;
            convenioView.Telefone = convenio.Telefone;
            convenioView.InscricaoMunicipal = convenio.InscricaoMunicipal;
            convenioView.InscricaoEstadual = convenio.InscricaoEstadual;
            convenioView.RegistroAns = convenio.RegistroAns;
            convenioView.Email = convenio.Email;
            convenioView.Numero = convenio.Numero;
            convenioView.ConvenioId = convenio.ConvenioId;
            convenioView.Logo = convenio.Logo;
            TempData["DtCadastro"] = convenio.DtCadastro;
            if (convenio.CepId != null)
            {
                var cep = await _contexto.Cep.FirstOrDefaultAsync(a => a.CepId == convenio.CepId);
                convenioView.CEP = cep.CEP;
                convenioView.Logradouro = cep.Logradouro;
                convenioView.Bairro = cep.Bairro;
                convenioView.Cidade = cep.Cidade;
                convenioView.UF = cep.UF;
                convenioView.Complemento = cep.Complemento;
            }
            TempData["DtCadastro"] = convenio.DtCadastro;
            HttpContext.JsReportFeature().Recipe(Recipe.ChromePdf);
            return View("_CadastroConvenio", convenioView);
            //return new ViewAsPdf("_CadastroConvenio", convenioView) { FileName = "Cadastro_" + convenio.Nome + "_" + dt_atual + ".pdf" };

        }
        public async Task<IActionResult> ConvenioPaciente(int PacienteId)
        {
            if(PacienteId == 0)
            {
                _logger.LogError("Paciente não encontrado");
                return NotFound();
            }
            var convenioPaciente = await (from p in _contexto.Pacientes
                                          join c in _contexto.CartaoConvenios
                                          on p.PacienteId equals c.PacienteId
                                          join a in _contexto.Convenios
                                          on c.ConvenioId equals a.ConvenioId
                                          where p.PacienteId == PacienteId
                                          select new CartaoPacienteViewModel
                                    {
                                        ConvenioId = c.ConvenioId,
                                        PacienteId = p.PacienteId,
                                        NumeroCarteira = c.NumeroCarteira,
                                        Validade = c.Validade,
                                        Status = c.Status,
                                        ConvenioNome = a.Nome,
                                        CartaoConvenio = c.CartaoConvenioId

                                    }
                                    ).ToListAsync();
            
            ViewData["PacienteId"] = PacienteId;
           
            return View(convenioPaciente);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddConvenioPaciente(int PacienteId,CartaoConvenio cartaoConvenio)
        {
            
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Adicionando convênio para o paciente");
                _contexto.Add(cartaoConvenio);
                await _contexto.SaveChangesAsync();
                TempData["Mensagem"] = "Adicionado com sucesso!";
                return RedirectToAction("Paciente","Pacientes",new { PacienteId = PacienteId });
            }

            ViewData["ConvenioId"] = new SelectList(_contexto.Convenios, "ConvenioId", "Nome");
            ViewData["PacienteId"] = PacienteId;
            return View(cartaoConvenio);
        }
        public async Task<JsonResult> GetCartaoPaciente(int PacienteId, int CartaoConvenioId)
        {
            if( CartaoConvenioId == 0)
            {
                _logger.LogError("Não foram encontrado os paramentros");
                return Json("Erro, paciente ou cartão não encontrado");
            }
            _logger.LogInformation("buscando cartão de convênio do paciente");
            var cartaoConvenio = await _contexto.CartaoConvenios.FirstOrDefaultAsync(a => a.CartaoConvenioId == CartaoConvenioId);
            _logger.LogInformation("Retornando dados via json");
          
            return Json(cartaoConvenio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConvenioPaciente(int PacienteId,int CartaoConvenioId, CartaoConvenio cartaoConvenio)
        {
            if ((PacienteId == 0 || CartaoConvenioId == 0))
            {
                _logger.LogError("Não foram encontrado os paramentros");
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Atualizando cartão de convênio do paciente");
                _contexto.Update(cartaoConvenio);
                await _contexto.SaveChangesAsync();
                TempData["Mensagem"] = "Atualizado com sucesso!";
                return RedirectToAction("Paciente", "Pacientes", new { PacienteId = PacienteId });

            }
            _logger.LogError("Erro ao atualizar a carteira convênio do paciente");
            ViewData["ConvenioId"] = new SelectList(_contexto.Convenios, "ConvenioId", "Nome");
            return View(cartaoConvenio);
        }

        public JsonResult GetConvenio(string term)
        {

            var convenio =  _contexto.Convenios.Where(c => c.Nome.Contains(term)).Select( c => new {label = c.Nome,id = c.ConvenioId });
            return Json(convenio);
        
        }
    }
    
}