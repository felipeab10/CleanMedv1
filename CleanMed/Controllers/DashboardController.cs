using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanMed.Data;
using CleanMed.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CleanMed.Controllers
{
    public class DashboardController : Controller
    {
        private readonly Contexto _contexto;
        public DashboardController(Contexto contexto)
        {
            _contexto = contexto;
        }
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult GraficoTotalPacienteCadastrados()
        {
            var TotalPaciente = _contexto.Pacientes.Count();
            return Json(TotalPaciente);
        }
        public JsonResult GraficoStatusAgendamento(DateTime dataAgenda)
        {
            GraficoStatusAgendamentoViewModel status = new GraficoStatusAgendamentoViewModel();
            status.Agendados = _contexto.Agendamentos
                .Include(a => a.AgendaMedica)
                .Where(a => a.AgendaMedica.DataAgenda == dataAgenda).Count(a => a.StatusAgendamento == "Agendado");
            status.Livre = _contexto.Agendamentos
                .Include(a => a.AgendaMedica)
                .Where(a => a.AgendaMedica.DataAgenda == dataAgenda).Count(a => a.StatusAgendamento == "Livre");
            status.Confirmados = _contexto.Agendamentos
                .Include(a => a.AgendaMedica)
                .Where(a => a.AgendaMedica.DataAgenda == dataAgenda).Count(a => a.StatusAgendamento == "Confirmado");
            status.Cancelados = _contexto.Agendamentos
                .Include(a => a.AgendaMedica)
                .Where(a => a.AgendaMedica.DataAgenda == dataAgenda).Count(a => a.StatusAgendamento == "Cancelado");
            status.Excluidos = _contexto.Agendamentos
                .Include(a => a.AgendaMedica)
                .Where(a => a.AgendaMedica.DataAgenda == dataAgenda).Count(a => a.StatusAgendamento == "Excluido");
            return Json(status);
        }
    }
}