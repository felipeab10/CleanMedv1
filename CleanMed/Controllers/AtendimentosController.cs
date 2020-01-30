using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanMed.Dados.Interface;
using CleanMed.Data;
using CleanMed.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CleanMed.Controllers
{
    public class AtendimentosController : Controller
    {
        private readonly Contexto _contexto;
        private readonly IAtendimentoRepositorio _atendimentoRepositorio;
        private readonly ILogger<AtendimentosController> _logger;

        public AtendimentosController(Contexto contexto, IAtendimentoRepositorio atendimentoRepositorio, ILogger<AtendimentosController> logger)
        {
            _contexto = contexto;
            _atendimentoRepositorio = atendimentoRepositorio;
            _logger = logger;
        }
        public IActionResult GerarAtendimento(string AgendamentoId)
        {

            if(AgendamentoId != null)
            {
                var arr = AgendamentoId.Split(',');
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

                                   select new AtendimentoViewModel
                                   {
                                       AgendaMedicaId = a.AgendaMedicaId,
                                       AgendamentoId = age.AgendamentoId,
                                       HoraAgenda = age.HoraAgenda,
                                       NomePrestador = p.Nome,
                                       PrestadorId = p.PrestadorId,
                                       NomeItemAgendamento = i.Descricao,
                                       DataAgenda = a.DataAgenda,
                                       PacienteId = pa.PacienteId,
                                       NomePaciente = pa.Nome,
                                       DataNascimento = pa.DataNascimento,
                                       StatusAgendamento = age.StatusAgendamento,
                                       Telefone = pa.Telefone,
                                       ConvenioNome = c.Nome,
                                   });
                return View(agendamento);
            }
            return new JsonResult(false);
        }
    }
}