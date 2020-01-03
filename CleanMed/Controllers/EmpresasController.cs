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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using jsreport.AspNetCore;
using jsreport.Types;
using Microsoft.AspNetCore.Authorization;

namespace CleanMed.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR")]
    public class EmpresasController : Controller
    {
        private readonly Contexto _context;
        private readonly IEmpresaRepositorio _empresaRepositorio;
        private readonly ILogger<EmpresasController> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        public EmpresasController(Contexto context, IEmpresaRepositorio empresaRepositorio, ILogger<EmpresasController> logger, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _empresaRepositorio = empresaRepositorio;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Empresas
        public async Task<IActionResult> Index(int? pageNumber, int searchId, string searchDescricao,string searchCNPJ)
        {
            ViewData["searchId"] = searchId;
            ViewData["searchDescricao"] = searchDescricao;
            ViewData["searchCNPJ"] = searchCNPJ;


            var empresas = from s in _context.Empresas
                                select s;
            if (searchId > 0)
            {

                empresas = empresas.Where(s => s.EmpresaId == searchId);
            }
            if (!String.IsNullOrEmpty(searchDescricao))
            {
                empresas = empresas.Where(s => s.NomeFantasia.Contains(searchDescricao) 
                                        || s.RazaoSocial.Contains(searchDescricao));
            }
            if (!String.IsNullOrEmpty(searchCNPJ))
            {
                empresas = empresas.Where(s => s.CNPJ.Contains(searchCNPJ));
                                       
            }

           
            int pageSize = 5;
            return View(await PaginatedList<Empresa>.CreateAsync(empresas.AsNoTracking().Include(e=> e.Cep).OrderBy(e => e.EmpresaId), pageNumber ?? 1, pageSize));
        }

        // GET: Empresas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas
                .Include(e => e.Cep)
                .FirstOrDefaultAsync(m => m.EmpresaId == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // GET: Empresas/Create
        public IActionResult Create()
        {
            var listStatus = new SelectList(new[]
            {
                   new {ID="true",Name="Ativo" },
                   new {ID="false",Name="Inativo" },

               },"ID", "Name");
            ViewData["StatusId"] = listStatus;
            var listUnidade = new SelectList(new[]
            {
                   new {ID="true",Name="Sim" },
                   new {ID="false",Name="Não" },

               }, "ID", "Name");
            ViewData["UnidadeId"] = listUnidade;
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( EmpresaViewModel empresaViewModel,int? CEPBD,IFormFile Logo)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Adicionando empresa");
                Empresa empresa = new Empresa();
                empresa.NomeFantasia = empresaViewModel.NomeFantasia;
                empresa.RazaoSocial = empresaViewModel.RazaoSocial;
                empresa.CNPJ = empresaViewModel.CNPJ;
                empresa.Unidade = empresaViewModel.Unidade;
                empresa.Status = empresaViewModel.Status;
                empresa.InscricaoEstadual = empresaViewModel.InscricaoEstadual;
                empresa.InscricaoMunicipal = empresaViewModel.InscricaoMunicipal;
                empresa.CNES = empresaViewModel.CNES;
                empresa.Numero = empresaViewModel.Numero;
                _logger.LogInformation("Validando logo da empresa");
                if (Logo != null)
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
                        empresa.Logo = "~/Imagens/" + newFileName;
                    }
                }
                _logger.LogInformation("Validando endereço da empresa");
               if((CEPBD == null || CEPBD == 0 ))
                {
                        _logger.LogInformation("Adicionando novo endereço ao banco");
                        Cep ceps = new Cep();
                        ceps.Logradouro = empresaViewModel.Logradouro;
                        ceps.CEP = empresaViewModel.CEP;
                        ceps.Bairro = empresaViewModel.Bairro;
                        ceps.Cidade = empresaViewModel.Cidade;
                        ceps.UF = empresaViewModel.UF;
                        ceps.Complemento = empresaViewModel.Complemento;
                        _context.Add(ceps);
                        await _context.SaveChangesAsync();
                        _logger.LogInformation("associando a empresa");
                        empresa.CepId = ceps.CepId;

                }
                else
                {
                    empresa.CepId = CEPBD;
                }

                await _empresaRepositorio.Inserir(empresa);
                _logger.LogInformation("Empresa adicionada");
                TempData["Mensagem"] = "Adicionado com sucesso";
                return RedirectToAction(nameof(Index));
            }

            _logger.LogError("Erro ao adicionar empresa");
            var listStatus = new SelectList(new[]
            {
                   new {ID="true",Name="Ativo" },
                   new {ID="false",Name="Inativo" },

               }, "ID", "Name");
            ViewData["StatusId"] = listStatus;
            var listUnidade = new SelectList(new[]
            {
                   new {ID="true",Name="Sim" },
                   new {ID="false",Name="Não" },

               }, "ID", "Name");
            ViewData["UnidadeId"] = listUnidade;
          
            return View(empresaViewModel);
        }

        // GET: Empresas/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Empresa não encontrada");
                return NotFound();
            }

            var empresa = await _empresaRepositorio.PegarPeloId(id);
            EmpresaViewModel empresaView = new EmpresaViewModel();
            empresaView.EmpresaId = empresa.EmpresaId;
            empresaView.NomeFantasia = empresa.NomeFantasia;
            empresaView.CNPJ = empresa.CNPJ;
            empresaView.Unidade = empresa.Unidade;
            empresaView.Status = empresa.Status;
            empresaView.RazaoSocial = empresa.RazaoSocial;
            empresaView.InscricaoEstadual = empresa.InscricaoEstadual;
            empresaView.InscricaoMunicipal = empresa.InscricaoMunicipal;
            empresaView.CNES = empresa.CNES;
            empresaView.Logo = empresa.Logo;
            empresaView.Numero = empresa.Numero;
            if (empresa.CepId != null)
            {
                var cep = _context.Cep.FirstOrDefault(c => c.CepId == empresa.CepId);
                empresaView.CepId = cep.CepId;
                empresaView.CEP = cep.CEP;
                empresaView.Logradouro = cep.Logradouro;
                empresaView.Bairro = cep.Bairro;
                empresaView.Cidade = cep.Cidade;
                empresaView.UF = cep.UF;
                empresaView.Complemento = cep.Complemento;
            }


            if (empresa == null)
            {
                _logger.LogError("Empresa não encontrada");
                return NotFound();
            }
            var listStatus = new SelectList(new[]
           {
                   new {ID="true",Name="Ativo" },
                   new {ID="false",Name="Inativo" },

               }, "ID", "Name");
            ViewData["StatusId"] = listStatus;
            var listUnidade = new SelectList(new[]
            {
                   new {ID="true",Name="Sim" },
                   new {ID="false",Name="Não" },

               }, "ID", "Name");
            ViewData["UnidadeId"] = listUnidade;
            return View(empresaView);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmpresaViewModel empresaViewModel,int? CEPBD,IFormFile Logo)
        {
            if (id != empresaViewModel.EmpresaId)
            {
                _logger.LogError("Empresa recebida diferenta da gravada no BD");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Atualizando a empresa");
                var empresa = await _empresaRepositorio.PegarPeloId(empresaViewModel.EmpresaId);
                empresa.NomeFantasia = empresaViewModel.NomeFantasia;
                empresa.RazaoSocial = empresaViewModel.RazaoSocial;
                empresa.CNPJ = empresaViewModel.CNPJ;
                empresa.Unidade = empresaViewModel.Unidade;
                empresa.Status = empresaViewModel.Status;
                empresa.InscricaoEstadual = empresaViewModel.InscricaoEstadual;
                empresa.InscricaoMunicipal = empresaViewModel.InscricaoMunicipal;
                empresa.CNES = empresaViewModel.CNES;
                empresa.Numero = empresaViewModel.Numero;
                _logger.LogInformation("Validando a logo da empresa");
                if (Logo != null)
                {
                   if(empresa.Logo != null)
                    {
                        string LogoEmpresa = empresa.Logo;
                        LogoEmpresa = LogoEmpresa.Replace("~", "wwwroot");
                        System.IO.File.Delete(LogoEmpresa);
                        _logger.LogInformation("Logo antigo da empresa removido");
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
                        empresa.Logo = "~/Imagens/" + newFileName;
                    }
                }
                _logger.LogInformation("Validando endereço da empresa");
                if (CEPBD == 0)
                {

                   
                        _logger.LogInformation("Adicionando novo endereço ao banco");
                        Cep ceps = new Cep();
                        ceps.Logradouro = empresaViewModel.Logradouro;
                        ceps.CEP = empresaViewModel.CEP;
                        ceps.Bairro = empresaViewModel.Bairro;
                        ceps.Cidade = empresaViewModel.Cidade;
                        ceps.UF = empresaViewModel.UF;
                        ceps.Complemento = empresaViewModel.Complemento;
                        _context.Add(ceps);
                        await _context.SaveChangesAsync();
                        _logger.LogInformation("associando a empresa");
                        empresa.CepId = ceps.CepId;

                }
                else
                {
                    empresa.CepId = CEPBD;
                }

                await _empresaRepositorio.Atualizar(empresa);
                _logger.LogInformation("Empresa Atualizada");
                TempData["Mensagem"] = "Atualizado com sucesso";
                return RedirectToAction(nameof(Index));
            }

            _logger.LogError("Erro ao atualizar a empresa");
            var listStatus = new SelectList(new[]
          {
                   new {ID="true",Name="Ativo" },
                   new {ID="false",Name="Inativo" },

               }, "ID", "Name");
            ViewData["StatusId"] = listStatus;
            var listUnidade = new SelectList(new[]
            {
                   new {ID="true",Name="Sim" },
                   new {ID="false",Name="Não" },

               }, "ID", "Name");
            ViewData["UnidadeId"] = listUnidade;
            return View(empresaViewModel);
        }
        [MiddlewareFilter(typeof(JsReportPipeline))]
        public async Task<IActionResult> VisualizarEmpresaPDF(int EmpresaId)
        {
            if (EmpresaId == 0)
            {
                _logger.LogInformation("Convênio não encontrado");
                return NotFound();
            }
            DateTime dt_atual = DateTime.Now;
            var empresa = await _empresaRepositorio.PegarPeloId(EmpresaId);
            EmpresaViewModel empresaView = new EmpresaViewModel();
            empresaView.EmpresaId = empresa.EmpresaId;
            empresaView.NomeFantasia = empresa.NomeFantasia;
            empresaView.CNPJ = empresa.CNPJ;
            empresaView.Unidade = empresa.Unidade;
            empresaView.Status = empresa.Status;
            empresaView.RazaoSocial = empresa.RazaoSocial;
            empresaView.InscricaoEstadual = empresa.InscricaoEstadual;
            empresaView.InscricaoMunicipal = empresa.InscricaoMunicipal;
            empresaView.CNES = empresa.CNES;
            empresaView.Numero = empresa.Numero;
          
      
            if (empresa.CepId != null)
            {
                var cep = _context.Cep.FirstOrDefault(c => c.CepId == empresa.CepId);
                empresaView.CepId = cep.CepId;
                empresaView.CEP = cep.CEP;
                empresaView.Logradouro = cep.Logradouro;
                empresaView.Bairro = cep.Bairro;
                empresaView.Cidade = cep.Cidade;
                empresaView.UF = cep.UF;
                empresaView.Complemento = cep.Complemento;
            }

            TempData["DtCadastro"] = empresa.DtCadastro;
            HttpContext.JsReportFeature().Recipe(Recipe.ChromePdf);
            return View("_CadastroEmpresa", empresaView);
            //return new ViewAsPdf("_CadastroConvenio", convenioView) { FileName = "Cadastro_" + convenio.Nome + "_" + dt_atual + ".pdf" };

        }
        public async Task<JsonResult> EmpresaExisteCNPJ(string CNPJ, int EmpresaId)
        {
            if(EmpresaId == 0)
            {
                if (await _empresaRepositorio.EmpresaExisteCNPJ(CNPJ))
                {
                    var empresa = _context.Empresas.Where(e => e.CNPJ == CNPJ).FirstOrDefault();
                    return Json(empresa);
                   
                }
                return Json(true);
            }
            if(await _empresaRepositorio.EmpresaExisteCNPJ(CNPJ, EmpresaId))
            {
                var empresa = _context.Empresas.Where(e => e.CNPJ == CNPJ && e.EmpresaId == EmpresaId).FirstOrDefault();
                return Json(empresa);
               
            }
            return Json(true);
        }

    }
}
