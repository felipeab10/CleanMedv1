using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CleanMed.Dados.Interface;
using CleanMed.Data;
using CleanMed.Models;
using CleanMed.Servicos;
using CleanMed.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMed.Controllers
{


    public class AgendasMedicasController : Controller
    {
        private readonly Contexto _contexto;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IAgendaMedicaRepositorio _agendaMedicaRepositorio;
        private readonly ILogger<AgendasMedicasController> _logger;
        private readonly IAgendamentoRepositorio _agendamentoRepositorio;
        private readonly ICartaoConvenioRepositorio _cartaoConvenioRepositorio;
        private readonly IPacienteRepositorio _pacienteRepositorio;
        public AgendasMedicasController(Contexto contexto, IUsuarioRepositorio usuarioRepositorio, IAgendaMedicaRepositorio agendaMedicaRepositorio, ILogger<AgendasMedicasController> logger, IAgendamentoRepositorio agendamentoRepositorio, ICartaoConvenioRepositorio cartaoConvenioRepositorio, IPacienteRepositorio pacienteRepositorio)
        {
            _contexto = contexto;
            _usuarioRepositorio = usuarioRepositorio;
            _agendaMedicaRepositorio = agendaMedicaRepositorio;
            _logger = logger;
            _agendamentoRepositorio = agendamentoRepositorio;
            _cartaoConvenioRepositorio = cartaoConvenioRepositorio;
            _pacienteRepositorio = pacienteRepositorio;
        }


        public async Task<IActionResult> Index(int? pageNumber, string SearchDataAgenda, int SearchPrestadorId, int SearchItemAgendamentoId)
        {



            var UsuarioLogado = await _usuarioRepositorio.PegarUsuarioLogado(User);
            if (UsuarioLogado == null)
            {
                return RedirectToAction("Logout", "Usuarios");
            }

            //pegando as agendas
            var agendas = from a in _contexto.AgendasMedicas
                          join p in _contexto.Prestadores
                          on a.PrestadorId equals p.PrestadorId
                          into Prestador
                          from p in Prestador.DefaultIfEmpty()
                          join it in _contexto.ItensAgendasMedica
                          on a.AgendaMedicaId equals it.AgendaMedicaId
                          //into ItemAgendaMedica from it in ItemAgendaMedica.DefaultIfEmpty()
                          join i in _contexto.ItemAgendamentos
                          on it.ItemAgendamentoId equals i.ItemAgendamentoId
                          //into ItemAgendamento from i in ItemAgendamento.DefaultIfEmpty()
                          join r in _contexto.RecursoAgendamentos
                          on i.RecursoAgendamentoId equals r.RecursoAgendamentoId
                          into RecursoAgendamento
                          from r in RecursoAgendamento.DefaultIfEmpty()
                          join s in _contexto.Setores
                          on a.SetorId equals s.SetorId
                          into Setores
                          from s in Setores.DefaultIfEmpty()
                          select new AgendasMedicasViewModel
                          {
                              AgendaMedicaId = a.AgendaMedicaId,
                              PrestadorId = a.PrestadorId,
                              ItemAgendamentoId = it.ItemAgendamentoId,
                              DataAgenda = a.DataAgenda,
                              HoraInicio = a.HoraInicio,
                              HoraFim = a.HoraFim,
                              DataLiberacao = a.DataLiberacao,
                              QtAtendimento = a.QtAtendimento,
                              QtEncaixe = a.QtEncaixe,
                              DtCadastro = a.DtCadastro,
                              ObservacaoAgendamento = a.Observacao,
                              ThemeColor = a.ThemeColor,
                              UsuarioId = a.UsuarioId,
                              QtTempoMedio = a.QtTempoMedio,
                              SetorId = a.SetorId == null ? 0 : a.SetorId,
                              NomeItemAgendamento = i.Descricao,
                              TipoAgenda = a.TipoAgenda
                          };
            if (!String.IsNullOrEmpty(SearchDataAgenda))
            {
                DateTime dt = DateTime.Parse(SearchDataAgenda);
                agendas = agendas.Where(d => d.DataAgenda == dt);
                ViewData["SearchDataAgenda"] = SearchDataAgenda;

                if (SearchPrestadorId != 0)
                {
                    agendas = agendas.Where(p => p.PrestadorId == SearchPrestadorId);
                    ViewData["SearchPrestadorId"] = SearchPrestadorId;

                    TempData["NomePrestador"] = _contexto.Prestadores.Where(p => p.PrestadorId == SearchPrestadorId).Select(p => p.Nome).First();
                    TempData["CRMPrestador"] = _contexto.Prestadores.Where(p => p.PrestadorId == SearchPrestadorId).Select(p => p.NumeroCrm).First();
                }
                if (SearchItemAgendamentoId != 0)
                {
                    agendas = agendas.Where(p => p.ItemAgendamentoId == SearchItemAgendamentoId);
                    ViewData["SearchItemAgendamentoId"] = SearchItemAgendamentoId;
                    ViewData["ItemAgendamentoId"] = new SelectList(_contexto.ItemAgendamentos, "ItemAgendamentoId", "Descricao", SearchItemAgendamentoId);
                }

                ViewData["PrestadorId"] = new SelectList(_contexto.Prestadores, "PrestadorId", "Nome");
                ViewData["ItemAgendamentoId"] = new SelectList(_contexto.ItemAgendamentos, "ItemAgendamentoId", "Descricao");
                int pageSize = 7;
                ViewData["UsuarioId"] = UsuarioLogado.Id;

                return View(await PaginatedList<AgendasMedicasViewModel>.CreateAsync(agendas.AsNoTracking().OrderBy(a => a.HoraInicio), pageNumber ?? 1, pageSize));
            }
            ViewData["PrestadorId"] = new SelectList(_contexto.Prestadores, "PrestadorId", "Nome");
            ViewData["ItemAgendamentoId"] = new SelectList(_contexto.ItemAgendamentos, "ItemAgendamentoId", "Descricao");
            return View();
        }

        public async Task<IActionResult> AgendaCentral(int? pageNumber, string SearchDataAgenda, int[] SearchPrestadorId, int[] SearchItemAgendamentoId, string SearchPeriodo, int SearchPaciente)
        {


            ViewData["PrestadorId"] = new SelectList(_contexto.Prestadores, "PrestadorId", "Nome");
            var UsuarioLogado = await _usuarioRepositorio.PegarUsuarioLogado(User);
            if (UsuarioLogado == null)
            {
                return RedirectToAction("Logout", "Usuarios");
            }

            //pegando as agendas
           

            var agendas = from a in _contexto.AgendasMedicas
                          join age in _contexto.Agendamentos
                          on a.AgendaMedicaId equals age.AgendaMedicaId
                          join p in _contexto.Prestadores
                          on a.PrestadorId equals p.PrestadorId
                          into Prestador
                          from p in Prestador.DefaultIfEmpty()
                          join it in _contexto.ItensAgendasMedica
                          on a.AgendaMedicaId equals it.AgendaMedicaId
                          //into ItemAgendaMedica from it in ItemAgendaMedica.DefaultIfEmpty()
                          join i in _contexto.ItemAgendamentos
                          on it.ItemAgendamentoId equals i.ItemAgendamentoId
                          //into ItemAgendamento from i in ItemAgendamento.DefaultIfEmpty()
                          join r in _contexto.RecursoAgendamentos
                          on i.RecursoAgendamentoId equals r.RecursoAgendamentoId
                          into RecursoAgendamento
                          from r in RecursoAgendamento.DefaultIfEmpty()
                          join s in _contexto.Setores
                          on a.SetorId equals s.SetorId
                          into Setores
                          from s in Setores.DefaultIfEmpty()
                          
                          join pa in _contexto.Pacientes
                          on age.PacienteId equals pa.PacienteId
                          into Pacientes
                          from pa in Pacientes.DefaultIfEmpty()
                          select new 
                          
                          AgendasMedicasViewModel
                          {
                              AgendaMedicaId = a.AgendaMedicaId,
                              PrestadorId = p.PrestadorId,
                              ItemAgendamentoId = it.ItemAgendamentoId,
                              DataAgenda = a.DataAgenda,
                              HoraInicio = a.HoraInicio,
                              HoraFim = a.HoraFim,
                              StatusAgendamento = age.StatusAgendamento,
                              Bloqueado = age.Bloqueado,
                              DataLiberacao = a.DataLiberacao,
                              QtAtendimento = a.QtAtendimento,
                              QtEncaixe = a.QtEncaixe,
                              DtCadastro = a.DtCadastro,
                              ObservacaoAgendamento = a.Observacao,
                              ThemeColor = a.ThemeColor,
                              UsuarioId = a.UsuarioId,
                              QtTempoMedio = a.QtTempoMedio,
                              SetorId = a.SetorId == null ? 0 : a.SetorId,
                              NomeItemAgendamento = i.Descricao,
                              TipoAgenda = a.TipoAgenda,
                              HoraAgenda = age.HoraAgenda,
                              NomePrestador = p.Nome,
                              SetorNome = s.Descricao,
                              PacienteId = pa.PacienteId,
                              NmPaciente = pa.Nome,
                              Color = age.Color,
                              AgendamentoId = age.AgendamentoId,
                          };
            
          
            if (!String.IsNullOrEmpty(SearchDataAgenda))
            {
                DateTime dt = DateTime.Parse(SearchDataAgenda);
                agendas = agendas.Where(d => d.DataAgenda == dt);
                ViewData["SearchDataAgenda"] = SearchDataAgenda;
            }
          
               
                ViewData["HorarioLivre"] = agendas.Count(a => a.StatusAgendamento == "Livre");
                ViewData["HorarioBloqueado"] = agendas.Count(a => a.Bloqueado == true);
                if (SearchPrestadorId.Length != 0)
                {
                    agendas = from t in agendas
                              join p in _contexto.Prestadores
                              on t.PrestadorId equals p.PrestadorId
                             
                              //where t.PrestadorId != null
                              where SearchPrestadorId.Contains(p.PrestadorId)
                              select t;
                    ViewData["SearchPrestadorId"] = SearchPrestadorId; 
                   
                    ViewData["HorarioBloqueado"] = agendas.Count(a => a.Bloqueado == true);
                    ViewData["HorarioLivre"] = agendas.Count(a => a.StatusAgendamento == "Livre");
                    //TempData["NomePrestador"] = _contexto.Prestadores.Where(p => p.PrestadorId == SearchPrestadorId).Select(p => p.Nome).First();
                    //TempData["CRMPrestador"] = _contexto.Prestadores.Where(p => p.PrestadorId == SearchPrestadorId).Select(p => p.NumeroCrm).First();
                }
                if (SearchItemAgendamentoId.Length != 0)
                {
                agendas = from t in agendas
                          join i in _contexto.ItensAgendasMedica
                          on t.ItemAgendamentoId equals i.ItemAgendamentoId
                          where SearchItemAgendamentoId.Contains(t.ItemAgendamentoId)
                          select t;
                    //agendas = agendas.Where(p => p.ItemAgendamentoId == SearchItemAgendamentoId);
                    ViewData["HorarioBloqueado"] = agendas.Count(a => a.Bloqueado == true);
                    ViewData["SearchItemAgendamentoId"] = SearchItemAgendamentoId;
                TempData["SearchItemAgendamentoId"] = SearchItemAgendamentoId;
                    ViewData["ItemAgendamentoId"] = new SelectList(_contexto.ItemAgendamentos, "ItemAgendamentoId", "Descricao", SearchItemAgendamentoId);
                    ViewData["HorarioLivre"] = agendas.Count(a => a.StatusAgendamento == "Livre");
                }
                if((!String.IsNullOrEmpty(SearchPeriodo) && SearchPeriodo == "Matutino"))
                {
                    string h = "06:00";
                    string hf = "11:59";
                    TimeSpan HoraInicio =TimeSpan.Parse(h);
                    TimeSpan HorarioFim = TimeSpan.Parse(hf);
                    agendas = agendas.Where(a => a.HoraAgenda.TimeOfDay >= HoraInicio && a.HoraAgenda.TimeOfDay <= HorarioFim);
                    ViewData["SearchPeriodo"] = SearchPeriodo;
                    ViewData["HorarioLivre"] = agendas.Count(a => a.StatusAgendamento == "Livre");
                    ViewData["HorarioBloqueado"] = agendas.Count(a => a.Bloqueado == true);
                }
                if ((!String.IsNullOrEmpty(SearchPeriodo) && SearchPeriodo == "Vespertino"))
                {
                    string h = "12:00";
                    string hf = "17:59";
                    TimeSpan HoraInicio = TimeSpan.Parse(h);
                    TimeSpan HorarioFim = TimeSpan.Parse(hf);
                    agendas = agendas.Where(a => a.HoraAgenda.TimeOfDay >= HoraInicio && a.HoraAgenda.TimeOfDay <= HorarioFim);
                    ViewData["SearchPeriodo"] = SearchPeriodo;
                    ViewData["HorarioLivre"] = agendas.Count(a => a.StatusAgendamento == "Livre");
                    ViewData["HorarioBloqueado"] = agendas.Count(a => a.Bloqueado == true);
                }
                if ((!String.IsNullOrEmpty(SearchPeriodo) && SearchPeriodo == "Noturno"))
                {
                    string h = "18:00";
                    string hf = "23:59";
                    TimeSpan HoraInicio = TimeSpan.Parse(h);
                    TimeSpan HorarioFim = TimeSpan.Parse(hf);
                    agendas = agendas.Where(a => a.HoraAgenda.TimeOfDay >= HoraInicio && a.HoraAgenda.TimeOfDay <= HorarioFim);
                    ViewData["SearchPeriodo"] = SearchPeriodo;
                    ViewData["HorarioLivre"] = agendas.Count(a => a.StatusAgendamento == "Livre");
                    ViewData["HorarioBloqueado"] = agendas.Count(a => a.Bloqueado == true);
                }
                if ((!String.IsNullOrEmpty(SearchPeriodo) && SearchPeriodo == "Madrugada"))

                {
                    string h = "00:00";
                    string hf = "05:59";
                    TimeSpan HoraInicio = TimeSpan.Parse(h);
                    TimeSpan HorarioFim = TimeSpan.Parse(hf);
                    agendas = agendas.Where(a => a.HoraAgenda.TimeOfDay >= HoraInicio && a.HoraAgenda.TimeOfDay <= HorarioFim);
                    ViewData["SearchPeriodo"] = SearchPeriodo;
                    ViewData["HorarioLivre"] = agendas.Count(a => a.StatusAgendamento == "Livre");
                    ViewData["HorarioBloqueado"] = agendas.Count(a => a.Bloqueado == true);
                }
                if(SearchPaciente != 0)
                {
                    agendas = agendas.Where(a => a.PacienteId == SearchPaciente).OrderBy(a => a.HoraAgenda);
                        ViewData["SearchPaciente"] = SearchPaciente;
                        ViewData["HorarioLivre"] = 0;


                }
                ViewData["PacienteId"] = new SelectList(_contexto.Pacientes, "PacienteId", "Nome");
                ViewData["PrestadorId"] = new SelectList(_contexto.Prestadores, "PrestadorId", "Nome");
                ViewData["ItemAgendamentoId"] = new SelectList(_contexto.ItemAgendamentos, "ItemAgendamentoId", "Descricao");
                int pageSize = 30;
                ViewData["UsuarioId"] = UsuarioLogado.Id;
                 ViewData["SearchItemAgendamentoId"] = SearchItemAgendamentoId;

            //return View(agendas);
            return View(await PaginatedList<AgendasMedicasViewModel>.CreateAsync(agendas.AsNoTracking().OrderByDescending(a => a.DataAgenda), pageNumber ?? 1, pageSize));
           
            //ViewData["PacienteId"] = new SelectList(_contexto.Pacientes, "PacienteId", "Nome");
            //ViewData["PrestadorId"] = new SelectList(_contexto.Prestadores, "PrestadorId", "Nome");
            //ViewData["ItemAgendamentoId"] = new SelectList(_contexto.ItemAgendamentos, "ItemAgendamentoId", "Descricao");
            //return View();
        }




            public async Task<IActionResult> NovaAgenda()
        {
            var UsuarioLogado = await _usuarioRepositorio.PegarUsuarioLogado(User);
            if (UsuarioLogado == null)
            {
                return RedirectToAction("Logout", "Usuarios");
            }
            ViewData["UsuarioId"] = UsuarioLogado.Id;
            ViewData["PacienteId"] = new SelectList(_contexto.Pacientes, "PacienteId", "Nome");
            ViewData["ConvenioId"] = new SelectList(_contexto.Convenios, "ConvenioId", "Nome");
            var listSexo = new SelectList(new[]
            {
                new {Name = "Masculino",ID="Masculino"},
                new {Name = "Feminino",ID="Feminino"},
                new {Name = "Indefinido",ID="Indefinido"},
            }, "Name", "ID");
            ViewData["SexoId"] = listSexo;
            var listTipoAgenda = new SelectList(new[]
            {
                new {Name = "Imagem",ID="Imagem"},
                new {Name = "Laboratório",ID="Laboratório"},
                new {Name = "Ambulatório",ID="Ambulatório"},
            }, "Name", "ID");
            ViewData["TipoAgendaId"] = listTipoAgenda;
            var listStatusAgenda = new SelectList(new[]
           {
                new {Name = "true",ID="Ativo"},
                new {Name = "false",ID="Inativo"},
               
            }, "Name", "ID");
            ViewData["StatusAgendaId"] = listStatusAgenda;
            ViewData["SetorId"] = new SelectList(_contexto.Setores, "SetorId", "Descricao");
            ViewData["PrestadorId"] = new SelectList(_contexto.Prestadores, "PrestadorId", "Nome");
            ViewData["RecursoAgendamentoId"] = new SelectList(_contexto.RecursoAgendamentos, "RecursoAgendamentoId", "Descricao");
            ViewData["ItemAgendamentoId"] = new SelectList(_contexto.ItemAgendamentos, "ItemAgendamentoId", "Descricao");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NovaAgenda(AgendaMedica agendaMedica,int ItemAgendamentoId)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Adicionando Agendamento");
                await _agendaMedicaRepositorio.Inserir(agendaMedica);
                if (ItemAgendamentoId != 0)
                {
                    _logger.LogInformation("Adicionando item de agendamento");
                    ItemAgendaMedica itemAgenda = new ItemAgendaMedica();
                    itemAgenda.ItemAgendamentoId = ItemAgendamentoId;
                    itemAgenda.AgendaMedicaId = agendaMedica.AgendaMedicaId;
                    _contexto.ItensAgendasMedica.Add(itemAgenda);
                    await _contexto.SaveChangesAsync();
                }
                _logger.LogInformation("Criando horarios na agenda");
                var dataAgenda = agendaMedica.DataAgenda.Date;
                var horaAgenda = agendaMedica.HoraInicio.TimeOfDay;
                for(int i = 0; i < agendaMedica.QtAtendimento; i++)
                {
                    Agendamento agendamento = new Agendamento();
                    agendamento.AgendaMedicaId = agendaMedica.AgendaMedicaId;
                    agendamento.StatusAgendamento = "Livre";
                    var usuarioLogado = await _usuarioRepositorio.PegarUsuarioLogado(User);
                    agendamento.UsuarioId = usuarioLogado.Id;
                    agendamento.SNEncaixe = false;
                    //agendamento.ItemAgendamentoId = ItemAgendamentoId;
                    agendamento.HoraAgenda = dataAgenda + horaAgenda;
                 
                   if(i > 0)
                    {
                        horaAgenda = horaAgenda + agendaMedica.QtTempoMedio;
                        agendamento.HoraAgenda = dataAgenda + horaAgenda;
                    }
                    _contexto.Agendamentos.Add(agendamento);
                    await _contexto.SaveChangesAsync();
                }
                _logger.LogInformation("Agenda adicionado");
                TempData["Mensagem"] = "Agenda adicionada com sucesso";
                return RedirectToAction("Index","AgendasMedicas");
            }
            _logger.LogError("Erro, informações invalidas");
            return View(agendaMedica);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if(id == 0)
            {
                _logger.LogError("agenda não encontrada");
                return NotFound();
            }
            var agenda = await _contexto.AgendasMedicas
                
                .FirstAsync();
            ViewData["PacienteId"] = new SelectList(_contexto.Pacientes, "PacienteId", "Nome");
            return View(agenda);
        }
       
       

        [HttpGet]
        
        public ActionResult PesquisarPaciente(string q)
                {
           
                if (!String.IsNullOrEmpty(q)) {
                            Regex dtnascimento = new Regex(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$");
                Match match = dtnascimento.Match(q);
                if (match.Success)
                {
                    var dt = DateTime.Parse(q);
                    var listDtPaciente = _contexto.Pacientes.Where(p => p.DataNascimento == dt).ToList();
                    return Json(new { items = listDtPaciente });
                }

                Regex CPF = new Regex((@"^([0-9][0-9][0-9])(\.)([0-9][0-9][0-9])(\.)([0-9][0-9][0-9])(\-)\d{2}$"));
                Match matchCPF = CPF.Match(q);
                if (matchCPF.Success)
                {
                    var listCPFPaciente = _contexto.Pacientes.Where(p => p.CPF == q).ToList();
                    return Json(new { items = listCPFPaciente });
                }
                Regex Id = new Regex("[0-9]");
                Match matchID = Id.Match(q);
                if (matchID.Success)
                {
                    var pacientId = Convert.ToInt32(q);
                    var listCPFPaciente = _contexto.Pacientes.Where(p => p.PacienteId == pacientId).ToList();
                    return Json(new { items = listCPFPaciente });
                }
                else
                {
                    var listNome = _contexto.Pacientes.Where(p => p.Nome.Contains(q.ToUpper())).ToList();
                    /* var listNome =(from p in _contexto.Pacientes
                                  join  c in _contexto.CartaoConvenios
                                  on p.PacienteId equals c.PacienteId
                                  join con in _contexto.Convenios
                                  on c.ConvenioId equals con.ConvenioId
                                  where p.Nome.Contains(q.ToUpper())
                                  select new {
                                      id = p.PacienteId,
                                      Nome = p.Nome,
                                      Telefone = p.Telefone,
                                      text = p.Nome,
                                      CPF = p.CPF,
                                      DataNascimento = p.DataNascimento,
                                      ConvenioId = c.ConvenioId,
                                      ConvenioNome = con.Nome,
                                      NumeroCarteira = c.NumeroCarteira,
                                      ValidadeCarteira = c.Validade
                                  } ).ToList();
                   */
                    return Json(new { items = listNome });
                }
              
            }
          
            return Json(true);
           
        }
        public JsonResult PesquisarInfoPaciente(int PacienteId)
        {
            var paciente = (from p in _contexto.Pacientes
                            join c in _contexto.CartaoConvenios
                            on p.PacienteId equals c.PacienteId
                            into Cartao
                            from c in Cartao.DefaultIfEmpty()
                            join con in _contexto.Convenios
                            on c.ConvenioId equals con.ConvenioId
                            into Convenios 
                            from con in Convenios.DefaultIfEmpty()
                            where p.PacienteId == PacienteId
                            select new
                            {
                                ConvenioId = c.ConvenioId,
                                ConvenioNome = con.Nome,
                                NumeroCarteira = c.NumeroCarteira,
                                ValidadeCarteira = c.Validade,
                                DataNascimento = p.DataNascimento,
                                CPF = p.CPF,
                                Telefone = p.Telefone,
                            }).FirstOrDefault();
            
            return Json(paciente);
           
        }
        [HttpGet]
        public ActionResult PesquisarConvenio(string q)
        {
            var listConvenio = _contexto.Convenios.Where(c => c.Nome.Contains(q.ToUpper())).ToList();
            return Json(new { items = listConvenio });
        }

        [HttpGet]
        public ActionResult PesquisarPrestador(string q)
        {
            var listPrestador = _contexto.Prestadores.Where(c => c.Nome.Contains(q.ToUpper())).ToList();
            return Json(new { items = listPrestador });
        }

        [HttpGet]
        public ActionResult PesquisarItemAgendamento(string q)
        {
            var listItemAgendamento = _contexto.ItemAgendamentos.Where(c => c.Descricao.Contains(q.ToUpper())).ToList();
            return Json(new { items = listItemAgendamento });
        }

        [HttpPost]
        public async Task<IActionResult> GetEvents(string SearchDataAgenda, int[] PrestadorId)
        {
            
            var agendas =  (from a in _contexto.Agendamentos
                                join age in _contexto.AgendasMedicas
                                on a.AgendaMedicaId equals age.AgendaMedicaId
                                join p in _contexto.Prestadores
                                on age.PrestadorId equals p.PrestadorId
                                into Prestador
                                from p in Prestador.DefaultIfEmpty()
                                join it in _contexto.ItensAgendasMedica
                                on age.AgendaMedicaId equals it.AgendaMedicaId
                                join i in _contexto.ItemAgendamentos
                                on it.ItemAgendamentoId equals i.ItemAgendamentoId
                                join pa in _contexto.Pacientes
                                on a.PacienteId equals pa.PacienteId
                                into Pacientes
                                from pa in Pacientes.DefaultIfEmpty()
                                select new {

                                    id = a.AgendamentoId,
                                    title = pa.Nome,
                                    start = a.HoraAgenda,
                                    end = a.HoraAgenda + age.QtTempoMedio,
                                    color = a.Color,
                                    allDay = false,
                                    slotDuration = age.QtTempoMedio,
                                    slotLabelInterval = age.QtTempoMedio,
                                    minTime = age.HoraInicio,
                                    maxTime = age.HoraFim,
                                    StatusAgendamento = a.StatusAgendamento,
                                    ItemAgendamento = i.Descricao,
                                    StatusAgenda = a.StatusAgendamento,
                                    PrestadorNome = p.Nome,
                                    DataAgenda = age.DataAgenda,
                                    PrestadorId = p.PrestadorId,

                                });
            if (!String.IsNullOrEmpty(SearchDataAgenda))
            {
                DateTime dt = DateTime.Parse(SearchDataAgenda);
                agendas = agendas.Where(d => d.DataAgenda == dt);
            }
            int[] zero = { };
            if(PrestadorId.Length != 0 )
            {

                agendas = agendas.Where(p => PrestadorId.Contains(p.PrestadorId));

            }
           
            /*_contexto.Agendamentos.Include(a=> a.AgendaMedica)
             * .ThenInclude(a=> a.Prestador)
             * .Include(a => a.ItemAgendamento)
             * .Select(e => new { 
        id = e.AgendamentoId,
        title = e.Paciente.Nome,
        start = e.HoraAgenda,
        end = e.HoraAgenda + e.AgendaMedica.QtTempoMedio,
        color = e.Color,
        allDay = false,
        slotDuration = e.AgendaMedica.QtTempoMedio,
        slotLabelInterval = e.AgendaMedica.QtTempoMedio,
        minTime = e.AgendaMedica.HoraInicio,
        maxTime = e.AgendaMedica.HoraFim,
        StatusAgendamento = e.StatusAgendamento,
        ItemAgendamento = e.ItemAgendamento.Descricao,
        StatusAgenda = e.StatusAgendamento,
        PrestadorNome = e.AgendaMedica.Prestador.Nome,
        }).ToList();
        */
        if(SearchDataAgenda == null)
            {
                var dtDia = DateTime.Now.ToShortDateString();
                var agendaDia = (from a in _contexto.Agendamentos
                                 join age in _contexto.AgendasMedicas
                                 on a.AgendaMedicaId equals age.AgendaMedicaId
                                 join p in _contexto.Prestadores
                                 on age.PrestadorId equals p.PrestadorId
                                 into Prestador
                                 from p in Prestador.DefaultIfEmpty()
                                 join it in _contexto.ItensAgendasMedica
                                 on age.AgendaMedicaId equals it.AgendaMedicaId
                                 join i in _contexto.ItemAgendamentos
                                 on it.ItemAgendamentoId equals i.ItemAgendamentoId
                                 join pa in _contexto.Pacientes
                                 on a.PacienteId equals pa.PacienteId
                                 into Pacientes
                                 from pa in Pacientes.DefaultIfEmpty()
                                 where age.DataAgenda.Date  == DateTime.Now.Date
                                 select new
                                 {

                                     id = a.AgendamentoId,
                                     title = pa.Nome,
                                     start = a.HoraAgenda,
                                     end = a.HoraAgenda + age.QtTempoMedio,
                                     color = a.Color,
                                     allDay = false,
                                     slotDuration = age.QtTempoMedio,
                                     slotLabelInterval = age.QtTempoMedio,
                                     minTime = age.HoraInicio,
                                     maxTime = age.HoraFim,
                                     StatusAgendamento = a.StatusAgendamento,
                                     ItemAgendamento = i.Descricao,
                                     StatusAgenda = a.StatusAgendamento,
                                     PrestadorNome = p.Nome,
                                     DataAgenda = age.DataAgenda,

                                 });
                var teste = "";

                return new JsonResult(agendaDia.ToList());
            }
            return new JsonResult(agendas.ToList());
        }
        public async Task<IActionResult> horarioLivre(string id)
        {
            if(id.Length != 0)
            {
                var arr = id.Split(',');
               int[] horarios = Array.ConvertAll(arr, int.Parse);
                var horarioLivre = (from a in _contexto.AgendasMedicas
                                   join age in _contexto.Agendamentos
                                   on a.AgendaMedicaId equals age.AgendaMedicaId
                                   join p in _contexto.Prestadores
                                   on a.PrestadorId equals p.PrestadorId
                                   into Prestador
                                   from p in Prestador.DefaultIfEmpty()
                                   join it in _contexto.ItensAgendasMedica
                                   on a.AgendaMedicaId equals it.AgendaMedicaId
                                   join i in _contexto.ItemAgendamentos
                                   on it.ItemAgendamentoId equals i.ItemAgendamentoId
                                   where horarios.Contains(age.AgendamentoId)
                                    select new AgendasMedicasViewModel
                                    {
                                        AgendaMedicaId = a.AgendaMedicaId,
                                        AgendamentoId = age.AgendamentoId,
                                        HoraAgenda = age.HoraAgenda,
                                        NomePrestador = p.Nome,
                                        PrestadorId = p.PrestadorId,
                                        NomeItemAgendamento = i.Descricao,
                                        DataAgenda = a.DataAgenda,
                                    }).ToList();
                /*;
                */
                //return new JsonResult(horarioLivre);
                ViewData["ConvenioId"] = new SelectList(_contexto.Convenios, "ConvenioId", "Nome");
                ViewData["AgendamentoId"] = id;
                return View(horarioLivre);
            }
            else
            {
                //return new JsonResult("null");
                return new JsonResult("Nenhum horário selecionado");
            }
            /*
            if(id != 0) {
                var agendamento = await _agendamentoRepositorio.PegarPeloId(id);
                var listaItemAgendamento = _contexto.ItensAgendasMedica
                    .Include(a=> a.ItemAgendamento)
                    .Where(i => i.AgendaMedicaId == agendamento.AgendaMedicaId).Select(a=> a.ItemAgendamento).ToList();
                var prestador = _contexto.AgendasMedicas
                    .Include(p => p.Prestador)
                    .Where(p => p.AgendaMedicaId == agendamento.AgendaMedicaId)
                    .Select(p => p.Prestador).First();
                AgendasMedicasViewModel agendamentoView = new AgendasMedicasViewModel();
                agendamentoView.AgendaMedicaId = agendamento.AgendaMedicaId;
                agendamentoView.AgendamentoId = id;
                agendamentoView.HoraAgenda = agendamento.HoraAgenda;
                agendamentoView.NomePrestador = prestador.Nome;
                agendamentoView.PrestadorId = prestador.PrestadorId;
                
                //agendamentoView.DataNascimento = DateTime.Now;
                ViewData["ItemAgendamentoId"] = new SelectList(listaItemAgendamento, "ItemAgendamentoId", "Descricao");
                ViewData["ConvenioId"] = new SelectList(_contexto.Convenios,"ConvenioId", "Nome");
               // return PartialView("_GetHorarioLivrePartial", agendamentoView);
                 return View(agendamentoView);
                 
            }
            return RedirectToAction("AgendaCentral");
            */
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> agendarPaciente(AgendasMedicasViewModel agendasMedicasViewModel, string AgendamentoId)
        {
           
                if(AgendamentoId.Length != 0)
                {
                    var arr = AgendamentoId.Split(',');
                    int[] horarios = Array.ConvertAll(arr, int.Parse);
                    for(int i = 0;  i < horarios.Length;  i++)
                    {
                        foreach(var item in horarios)  
                           if(item != 0)
                        {
                            var agendamento = await _contexto.Agendamentos
                           .Where(a => a.AgendamentoId == item)
                           .FirstOrDefaultAsync();
                            Console.WriteLine(agendamento);
                            //Manipulando tabela agendamento
                            agendamento.PacienteId = agendasMedicasViewModel.PacienteId;
                           // agendamento.ItemAgendamentoId = agendasMedicasViewModel.ItemAgendamentoId;
                            agendamento.ConvenioId = agendasMedicasViewModel.ConvenioId;
                            agendamento.StatusAgendamento = "Agendado";
                            agendamento.Color = "#2196f3";
                            agendamento.ObservacaoAgendamento = agendasMedicasViewModel.ObservacaoAgendamento;
                            //Gravando log na tabela
                            /* AgendamentoLog agendamentoLog = new AgendamentoLog();
                             agendamentoLog.AgendamentoId = agendamento.AgendamentoId;
                             agendamentoLog.Dt_Acao = DateTime.Now;
                             agendamentoLog.Acao = "Agendado";
                             agendamentoLog.PacienteId = agendasMedicasViewModel.PacienteId;
                             var usuario = await _usuarioRepositorio.PegarUsuarioLogado(User);
                             agendamentoLog.UsuarioId = usuario.Id;
                             _contexto.AgendamentoLogs.Add(agendamentoLog);
                             */
                            _contexto.Agendamentos.Update(agendamento);
                            await _contexto.SaveChangesAsync();
                            /*
                                if (!await _cartaoConvenioRepositorio.CartaoPacienteExiste(agendasMedicasViewModel.PacienteId, agendasMedicasViewModel.NumeroCartaoConvenio))
                                {
                                    CartaoConvenio cartao = new CartaoConvenio();
                                    cartao.ConvenioId = agendasMedicasViewModel.ConvenioId;
                                    cartao.NumeroCarteira = agendasMedicasViewModel.NumeroCartaoConvenio;
                                    cartao.PacienteId = agendasMedicasViewModel.PacienteId;
                                    cartao.Status = true;
                                    cartao.Validade = agendasMedicasViewModel.CartaoValidade;
                                    _contexto.CartaoConvenios.Add(cartao);
                                    await _contexto.SaveChangesAsync();

                                }
                                */

                            if (i + 1 == horarios.Length)
                            {
                                TempData["Mensagem"] = "Agendado com sucesso";
                                return Json(true);
                            }
                        }
                   
                }

                }

                //Manipulando tabela de CartaoConvenio
                
            
            
                ModelState.AddModelError("PacienteId", "Campo Obrigatório");
                ModelState.AddModelError("DataNascimento", "Campo Obrigatório");
                ModelState.AddModelError("ItemAgendamentoId", "Campo Obrigatório");
                ModelState.AddModelError("ConvenioId", "Campo Obrigatório");
               
           
            var listaItemAgendamento = _contexto.ItensAgendasMedica
                   .Include(a => a.ItemAgendamento)
                   .Where(i => i.AgendaMedicaId == agendasMedicasViewModel.AgendaMedicaId).Select(a => a.ItemAgendamento).ToList();
            ViewData["ItemAgendamentoId"] = new SelectList(listaItemAgendamento, "ItemAgendamentoId", "Descricao");
            ViewData["ConvenioId"] = new SelectList(_contexto.Convenios, "ConvenioId", "Nome");
            return View("horarioLivre", agendasMedicasViewModel);
                //return Json(  false );
            
        }
        public IActionResult Agendado(int id)
        {
            var agendado = _contexto.Agendamentos
                .Include(p=> p.Paciente)
                .Include(c => c.Convenio)
                .Include(u => u.Usuario)
                .Include(p=> p.AgendaMedica)
                .ThenInclude(p=> p.Prestador)
                .Where(a => a.AgendamentoId == id)
                .FirstOrDefault();
            var itemAgendamento = _contexto.ItemAgendamentos
                .Where(i => i.ItemAgendamentoId == agendado.ItemAgendamentoId)
                .FirstOrDefault();
        
            AgendasMedicasViewModel agendamento = new AgendasMedicasViewModel();
            agendamento.AgendamentoId = agendado.AgendamentoId;
            agendamento.HoraAgenda = agendado.HoraAgenda;
            agendamento.PacienteId = agendado.Paciente.PacienteId;
            agendamento.NmPaciente = agendado.Paciente.Nome;
            agendamento.DataNascimento = agendado.Paciente.DataNascimento;
            agendamento.Telefone = agendado.Paciente.Telefone;
            agendamento.PrestadorId = agendado.AgendaMedica.PrestadorId;
            agendamento.NomePrestador = agendado.AgendaMedica.Prestador.Nome;
            agendamento.ItemAgendamentoId = itemAgendamento.ItemAgendamentoId;
            agendamento.NomeItemAgendamento = itemAgendamento.Descricao;
            agendamento.ConvenioNome = agendado.Convenio.Nome;
            agendamento.UsuarioId = agendado.UsuarioId;
            agendamento.UsuarioNome = agendado.Usuario.Nome;
            agendamento.ObservacaoAgendamento = agendado.ObservacaoAgendamento;
            return View(agendamento);
        }
        public IActionResult PacienteConfirmadoExiste(int[] HRSelecionado)
        {

            for (int i = 0; i < HRSelecionado.Length; i++)
            {
                foreach (var item in HRSelecionado) { 
                    
                    if (PacienteConfirmadoBool(item))
                    {
                        string[] horarios = HRSelecionado.Select(x => x.ToString()).ToArray();
                        return new JsonResult(horarios);
                    }
                    if (PacienteConfirmadoNull(item)) {
                        return new JsonResult("semagendamento");
                    }
                    return new JsonResult(false);
                }
               
            }
            return new JsonResult(false);
        }
        public bool PacienteConfirmadoBool(int AgendamentoId)
        {
            var agendamento = _contexto.Agendamentos.Where(i => i.AgendamentoId == AgendamentoId).FirstOrDefault();
            return _contexto.Agendamentos.Any(i => i.AgendamentoId == AgendamentoId && i.PacienteId == agendamento.PacienteId && i.StatusAgendamento == "Agendado");
        }
        public bool PacienteConfirmadoNull(int AgendamentoId)
        {
            return _contexto.Agendamentos.Any(i => i.AgendamentoId == AgendamentoId && i.PacienteId == null);
        }

        [HttpGet]
        public IActionResult ConfirmarPaciente(string id)
        {
            if (id.Length != 0)
            {
                var arr = id.Split(',');
                int[] horarios = Array.ConvertAll(arr, int.Parse);
                var horarioLivre = (from a in _contexto.AgendasMedicas
                                    join age in _contexto.Agendamentos
                                    on a.AgendaMedicaId equals age.AgendaMedicaId
                                    join p in _contexto.Prestadores
                                    on a.PrestadorId equals p.PrestadorId
                                    into Prestador
                                    from p in Prestador.DefaultIfEmpty()
                                    join it in _contexto.ItensAgendasMedica
                                    on a.AgendaMedicaId equals it.AgendaMedicaId
                                    join i in _contexto.ItemAgendamentos
                                    on it.ItemAgendamentoId equals i.ItemAgendamentoId
                                    join pa in _contexto.Pacientes
                                    on age.PacienteId equals pa.PacienteId
                                    
                                    join c in _contexto.Convenios
                                    on age.ConvenioId equals c.ConvenioId
                                    where horarios.Contains(age.AgendamentoId)
                                   
                                    select new AgendasMedicasViewModel
                                    {
                                        AgendaMedicaId = a.AgendaMedicaId,
                                        AgendamentoId = age.AgendamentoId,
                                        HoraAgenda = age.HoraAgenda,
                                        NomePrestador = p.Nome,
                                        PrestadorId = p.PrestadorId,
                                        NomeItemAgendamento = i.Descricao,
                                        DataAgenda = a.DataAgenda,
                                        PacienteId = pa.PacienteId,
                                        NmPaciente = pa.Nome,
                                        DataNascimento = pa.DataNascimento,
                                        StatusAgendamento = age.StatusAgendamento,
                                        Telefone = pa.Telefone,
                                        ConvenioNome = c.Nome,
                                    });
                ViewData["TipoConfirmacaoId"] = new SelectList(new[]{
                     new {Name = "O MESMO",ID= "O MESMO" },
                     new {Name = "PAI",ID= "PAI" },
                new {Name = "AMIGO (A)",ID= "AMIGO (A)" },
                new {Name = "AVO (A)",ID= "AVO (A)" },
                new {Name = "CUNHADO (A)",ID= "CUNHADO (A)" },
                new {Name = "ESPOSO (A)",ID= "ESPOSO (A)" },
                new {Name = "FILHO (A)",ID= "FILHO (A)" },
                new {Name = "GENRO",ID= "GENRO" },
                new {Name = "IRMA",ID= "IRMA" },
                new {Name = "IRMAO",ID= "IRMAO" },
                new {Name = "MAE",ID= "MAE" },
                new {Name = "NORA",ID= "NORA" },
                new {Name = "PRIMO (A)",ID= "PRIMO (A)" },
                new {Name = "SOBRINHO (A)",ID= "SOBRINHO (A)" },
                new {Name = "SOGRO (A)",ID= "SOGRO (A)" },
                new {Name = "TIO (A)",ID= "TIO (A)" },
                new {Name = "COMPANHEIRO (A)",ID= "COMPANHEIRO (A)" },
               
                new {Name = "NETO (A)",ID= "NETO (A)" },
                new {Name = "FUNCIONARIO (A)",ID= "FUNCIONARIO (A)" },
                new {Name = "NAMORADO (A)",ID= "NAMORADO (A)" },
                    }, "ID", "Name");
                ViewData["AgendamentoId"] = id;
                return View(horarioLivre);
            }
            return new JsonResult("Nenhum horário selecionado");
        }

        public IActionResult verLogs(int id)
        {
            var logAgendamento = _contexto.AgendamentoLogs
                .Include(u=> u.Usuario)
                .Include(a => a.Agendamento)
                .Include(a=> a.Paciente)
                .Where(a => a.AgendamentoId == id)
                .ToList();
            return View(logAgendamento);
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmarPaciente(string AgendamentoId,string TipoConfirmacao,string Nomecontato,string ObservacaoConfirmacao)
        {
            if(AgendamentoId != null)
            {
                var arr = AgendamentoId.Split(',');
                int[] horarios = Array.ConvertAll(arr, int.Parse);
                for(int i = 0; i < horarios.Length; i++)
                {
                    foreach (var item in horarios)
                    {
                        var agendamento = _contexto.Agendamentos.Where(i => i.AgendamentoId == item).FirstOrDefault();
                        agendamento.StatusAgendamento = "Confirmado";
                        agendamento.Color = "#ff9800";
                        ConfirmacaoAgendamento confirmacao = new ConfirmacaoAgendamento();
                        confirmacao.AgendamentoId = item;
                        confirmacao.TipoConfirmacao = TipoConfirmacao;
                        confirmacao.Nomecontato = Nomecontato;
                        confirmacao.ObservacaoConfirmacao = ObservacaoConfirmacao;
                        _contexto.Add(confirmacao);
                        _contexto.Update(agendamento);
                        await _contexto.SaveChangesAsync();
                    }
                    TempData["Mensagem"] = "Confirmado com sucesso";
                    return new JsonResult(true);
                }
                TempData["Mensagem"] = "Confirmado com sucesso";
                return new JsonResult(true);
            }
            else
            {
                return NotFound();
            }



            /*
            if(id != 0)
            {
                _logger.LogInformation("Confirmando pacinete");
                var agendamento = _contexto.Agendamentos.Where(a => a.AgendamentoId == id).FirstOrDefault();
                agendamento.StatusAgendamento = "Confirmado";
                agendamento.Color = "#ff9800";
                if (!String.IsNullOrEmpty(ObservacaoAgendamento))
                {
                    agendamento.ObservacaoAgendamento = ObservacaoAgendamento;
                }
                //Inserindo informação na tabela de log do agendamento
                AgendamentoLog log = new AgendamentoLog();
                log.AgendamentoId = agendamento.AgendamentoId;
                log.Dt_Acao = DateTime.Now;
                log.Acao = "Confirmar Paciente";
                log.PacienteId = agendamento.PacienteId;
                var usuario = await _usuarioRepositorio.PegarUsuarioLogado(User);
                log.UsuarioId = usuario.Id;
                _contexto.AgendamentoLogs.Add(log);
                _contexto.Agendamentos.Update(agendamento);
                await _contexto.SaveChangesAsync();
               
                return Json(true);
            }
            return NotFound();
            */
        }
        public async Task<IActionResult> ExcluirAgendamentoPaciente(int id)
        {
            if(id != 0)
            {
                var agendamento = _contexto.Agendamentos.Where(a => a.AgendamentoId == id).FirstOrDefault();
                //Salvando no log
                AgendamentoLog log = new AgendamentoLog();
                log.AgendamentoId = agendamento.AgendamentoId;
                log.Dt_Acao = DateTime.Now;
                log.Acao = "Exclusão de agendamento";
                var usuario = await _usuarioRepositorio.PegarUsuarioLogado(User);
                log.UsuarioId = usuario.Id;
                log.PacienteId = agendamento.PacienteId;
                _contexto.AgendamentoLogs.Add(log);
               await _contexto.SaveChangesAsync();
                //
                agendamento.PacienteId = null;
                agendamento.ObservacaoAgendamento = null;
                agendamento.VlAltura = 0;
                agendamento.Qtpeso = 0;
                agendamento.ConvenioId = null;
                agendamento.ItemAgendamentoId = null;
                agendamento.StatusAgendamento = "Livre";
                agendamento.Color = "#4caf50";
               
                 _contexto.Agendamentos.Update(agendamento);
                 await _contexto.SaveChangesAsync();


               
               
               
               
                return Json(true);
            }
            return NotFound();
        }
        [HttpGet]
        public IActionResult Cancelamento(int id)
        {
            ViewData["MotivoCancelamento"] = new SelectList(_contexto.MotivoCancelamentos,"MotivoCancelamentoId","Descricao");
            ViewData["AgendamentoId"] = id;
            return View();
        }

        [HttpPost]
       
        public async Task<IActionResult> Cancelamento(int id,int MotivoCancelamentoId)
        {
            if(id != 0)
            {
                var agendamento = await _contexto.Agendamentos.Where(a => a.AgendamentoId == id).FirstOrDefaultAsync();
                //Salvando log
                AgendamentoLog log = new AgendamentoLog();
                log.AgendamentoId = agendamento.AgendamentoId;
                log.Dt_Acao = DateTime.Now;
                log.Acao = "Cancelamento de horário";
                var usuario = await _usuarioRepositorio.PegarUsuarioLogado(User);
                log.UsuarioId = usuario.Id;
                log.PacienteId = agendamento.PacienteId;
                _contexto.AgendamentoLogs.Add(log);
                await _contexto.SaveChangesAsync();
                //
                agendamento.StatusAgendamento = "Cancelado";
                agendamento.MotivoCancelamentoId = MotivoCancelamentoId;
                agendamento.Color = "#e53935";
                _contexto.Agendamentos.Update(agendamento);
                await _contexto.SaveChangesAsync();
               
                return Json(true);
            }
            else
            {
                return Json(false);
            }

        }
        public IActionResult Cancelado(int id)
        {
            var agendado = _contexto.Agendamentos
                .Include(p => p.Paciente)
                .Include(c => c.Convenio)
                .Include(u => u.Usuario)
                .Include(p => p.AgendaMedica)
                .ThenInclude(p => p.Prestador)
                .Where(a => a.AgendamentoId == id)
                .FirstOrDefault();
            var itemAgendamento = _contexto.ItemAgendamentos
                .Where(i => i.ItemAgendamentoId == agendado.ItemAgendamentoId)
                .FirstOrDefault();
            var motivoCancelamento = _contexto.MotivoCancelamentos
                .Where(a => a.MotivoCancelamentoId == agendado.MotivoCancelamentoId)
                .FirstOrDefault();
            AgendasMedicasViewModel agendamento = new AgendasMedicasViewModel();
            agendamento.AgendamentoId = agendado.AgendamentoId;
            agendamento.HoraAgenda = agendado.HoraAgenda;
            agendamento.PacienteId = agendado.Paciente.PacienteId;
            agendamento.NmPaciente = agendado.Paciente.Nome;
            agendamento.DataNascimento = agendado.Paciente.DataNascimento;
            agendamento.Telefone = agendado.Paciente.Telefone;
            agendamento.PrestadorId = agendado.AgendaMedica.PrestadorId;
            agendamento.NomePrestador = agendado.AgendaMedica.Prestador.Nome;
            agendamento.ItemAgendamentoId = itemAgendamento.ItemAgendamentoId;
            agendamento.NomeItemAgendamento = itemAgendamento.Descricao;
            agendamento.ConvenioNome = agendado.Convenio.Nome;
            agendamento.UsuarioId = agendado.UsuarioId;
            agendamento.UsuarioNome = agendado.Usuario.Nome;
            agendamento.ObservacaoAgendamento = agendado.ObservacaoAgendamento;
            agendamento.StatusAgendamento = agendado.StatusAgendamento;
            agendamento.MotivoCancelamentoId = motivoCancelamento.MotivoCancelamentoId;
            agendamento.MotivoDescricao = motivoCancelamento.Descricao;
            return View(agendamento);
        }
        public async Task<IActionResult> ReverterCancelamento(int id)
        {
            if(id != 0)
            {
                var agendamento = _contexto.Agendamentos.Where(a => a.AgendamentoId == id).FirstOrDefault();
                //Salvando Log
                AgendamentoLog log = new AgendamentoLog();
                log.AgendamentoId = agendamento.AgendamentoId;
                log.Dt_Acao = DateTime.Now;
                log.Acao = "Reverter Cancelamento";
                var usuario = await _usuarioRepositorio.PegarUsuarioLogado(User);
                log.UsuarioId = usuario.Id;
                log.PacienteId = agendamento.PacienteId;
                _contexto.AgendamentoLogs.Add(log);
                await _contexto.SaveChangesAsync();
                //
                agendamento.StatusAgendamento = "Agendado";
                agendamento.MotivoCancelamentoId = null;
                agendamento.Color = "#2196f3";
                _contexto.Agendamentos.Update(agendamento);
                await _contexto.SaveChangesAsync();
               
                return Json(true);
            }
            return NotFound();
        }
        public IActionResult modalAddPaciente()
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
        public async Task<IActionResult> modalAddPaciente(PacienteViewModel pacienteView, int? CEPBD)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Adicionando novo Paciente");
                Paciente paciente = new Paciente();
                paciente.Nome = pacienteView.Nome.ToUpper();
                paciente.DataNascimento = pacienteView.DataNascimento;
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
                return Json(true);
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
      
        public IActionResult Agendar(int[] HRSelecionado)
        {
            
            for (int i = 0; i < HRSelecionado.Length; i++)
            {
               foreach(var item in HRSelecionado)
                if(HorarioDisponivel(item))
                {
                    return new JsonResult(false);
                }
            }
            string[] horarios = HRSelecionado.Select(x => x.ToString()).ToArray();
            return new JsonResult(horarios);
        }
        public bool HorarioDisponivel(int AgendamentoId)
        {
            return  _contexto.Agendamentos.Any(i => i.AgendamentoId == AgendamentoId && i.PacienteId != null);
        }
       
        public IActionResult VerificaCancelamento(int[] HRSelecionado)
        {

            for (int i = 0; i < HRSelecionado.Length; i++)
            {
                foreach (var item in HRSelecionado) {
                    if (VerificarStatusLivre(item))
                    {
                        return new JsonResult("Livre");
                    }
                    if (VerificarStatusCancelado(item))
                    {
                        string[] horarios = HRSelecionado.Select(x => x.ToString()).ToArray();
                        return new JsonResult(new {resposta = "Cancelado",data = horarios });
                    }
                    if (VerificaCancelamentoBool(item))
                    {
                        string[] horarios = HRSelecionado.Select(x => x.ToString()).ToArray();
                        return new JsonResult(horarios);
                        
                    }
                   
                    return new JsonResult(false);
                }
               
            }
            return new JsonResult(false);
        }
        public bool VerificaCancelamentoBool(int AgendamentoId)
        {
            return _contexto.Agendamentos.Any(i => i.AgendamentoId == AgendamentoId && i.PacienteId != null);
        }
        public bool VerificarStatusLivre(int AgendamentoId)
        {
            return _contexto.Agendamentos.Any(i => i.AgendamentoId == AgendamentoId && i.StatusAgendamento == "Livre");
        }
        public bool VerificarStatusCancelado(int AgendamentoId)
        {
            return _contexto.Agendamentos.Any(i => i.AgendamentoId == AgendamentoId && i.StatusAgendamento == "Cancelado");
        }

        public IActionResult CancelarPost(string id)
        {
            if (id.Length != 0)
            {
                var arr = id.Split(',');
                int[] horarios = Array.ConvertAll(arr, int.Parse);
                var agendamento = (from a in _contexto.AgendasMedicas
                                    join age in _contexto.Agendamentos
                                    on a.AgendaMedicaId equals age.AgendaMedicaId
                                    join p in _contexto.Prestadores
                                    on a.PrestadorId equals p.PrestadorId
                                    into Prestador
                                    from p in Prestador.DefaultIfEmpty()
                                    join it in _contexto.ItensAgendasMedica
                                    on a.AgendaMedicaId equals it.AgendaMedicaId
                                    join i in _contexto.ItemAgendamentos
                                    on it.ItemAgendamentoId equals i.ItemAgendamentoId
                                    join pa in _contexto.Pacientes
                                    on age.PacienteId equals pa.PacienteId

                                    join c in _contexto.Convenios
                                    on age.ConvenioId equals c.ConvenioId
                                    where horarios.Contains(age.AgendamentoId)

                                    select new AgendasMedicasViewModel
                                    {
                                        AgendaMedicaId = a.AgendaMedicaId,
                                        AgendamentoId = age.AgendamentoId,
                                        HoraAgenda = age.HoraAgenda,
                                        NomePrestador = p.Nome,
                                        PrestadorId = p.PrestadorId,
                                        NomeItemAgendamento = i.Descricao,
                                        DataAgenda = a.DataAgenda,
                                        PacienteId = pa.PacienteId,
                                        NmPaciente = pa.Nome,
                                        DataNascimento = pa.DataNascimento,
                                        StatusAgendamento = age.StatusAgendamento,
                                        Telefone = pa.Telefone,
                                        ConvenioNome = c.Nome,
                                    });
                ViewData["AgendamentoId"] = id;
                ViewData["MotivoCancelamento"] = new SelectList(_contexto.MotivoCancelamentos, "MotivoCancelamentoId", "Descricao");
                return View("Cancelado",agendamento);
            }
            return new JsonResult("Nenhum horário selecionado");
        }
        public IActionResult ReverterPost(string id)
        {
            if (id.Length != 0)
            {
                var arr = id.Split(',');
                int[] horarios = Array.ConvertAll(arr, int.Parse);
                var agendamento = (from a in _contexto.AgendasMedicas
                                   join age in _contexto.Agendamentos
                                   on a.AgendaMedicaId equals age.AgendaMedicaId
                                   join p in _contexto.Prestadores
                                   on a.PrestadorId equals p.PrestadorId
                                   into Prestador
                                   from p in Prestador.DefaultIfEmpty()
                                   join it in _contexto.ItensAgendasMedica
                                   on a.AgendaMedicaId equals it.AgendaMedicaId
                                   join i in _contexto.ItemAgendamentos
                                   on it.ItemAgendamentoId equals i.ItemAgendamentoId
                                   join pa in _contexto.Pacientes
                                   on age.PacienteId equals pa.PacienteId

                                   join c in _contexto.Convenios
                                   on age.ConvenioId equals c.ConvenioId
                                   where horarios.Contains(age.AgendamentoId)

                                   select new AgendasMedicasViewModel
                                   {
                                       AgendaMedicaId = a.AgendaMedicaId,
                                       AgendamentoId = age.AgendamentoId,
                                       HoraAgenda = age.HoraAgenda,
                                       NomePrestador = p.Nome,
                                       PrestadorId = p.PrestadorId,
                                       NomeItemAgendamento = i.Descricao,
                                       DataAgenda = a.DataAgenda,
                                       PacienteId = pa.PacienteId,
                                       NmPaciente = pa.Nome,
                                       DataNascimento = pa.DataNascimento,
                                       StatusAgendamento = age.StatusAgendamento,
                                       Telefone = pa.Telefone,
                                       ConvenioNome = c.Nome,
                                   });
                ViewData["AgendamentoId"] = id;
                //ViewData["MotivoCancelamento"] = new SelectList(_contexto.MotivoCancelamentos, "MotivoCancelamentoId", "Descricao");
                return View(agendamento);
            }
            return new JsonResult("Nenhum horário selecionado");
        }
        [HttpPost]
        public async Task<IActionResult> CancelarAgendamento(string AgendamentoId,int MotivoCancelamentoId)
        {
            if(AgendamentoId != null)
            {
                var arr = AgendamentoId.Split(',');
                int[] horarios = Array.ConvertAll(arr, int.Parse);
                for (int i = 0; i < horarios.Length; i++)
                {
                    foreach(var item in horarios)
                    {
                        var agendamento = _contexto.Agendamentos.Where(i => i.AgendamentoId == item).FirstOrDefault();
                        agendamento.StatusAgendamento = "Cancelado";
                        agendamento.MotivoCancelamentoId = MotivoCancelamentoId;
                        agendamento.Color = "#e53935";
                        _contexto.Agendamentos.Update(agendamento);
                        await _contexto.SaveChangesAsync();
                    }
                    TempData["Mensagem"] = "Cancelado com sucesso";
                    return new JsonResult(true);
                }
                TempData["Mensagem"] = "Cancelado com sucesso";
                return new JsonResult(true);
            }
            else
            {
                return new JsonResult(false);
            }

        }
           
    }
}