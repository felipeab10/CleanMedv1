using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.ViewModels
{
    public class AtendimentoViewModel
    {
        public int AgendaMedicaId { get; set; }
        public int AgendamentoId { get; set; }
         public DateTime HoraAgenda { get; set; }
        public string NomePrestador { get; set; }
        public int PrestadorId { get; set; }
        public string NomeItemAgendamento { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataAgenda { get; set; }
        public int? RecursoAgendamentoId { get; set; }
        public string RecursoNome { get; set; }
        public int PacienteId { get; set; }
        public string NomePaciente { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataNascimento { get; set; }
        public string CPFPaciente { get; set; }
        public string Telefone { get; set; }
        public int SetorId { get; set; }
        public int ConvenioId { get; set; }
        public string ConvenioNome { get; set; }
        public DateTime DataAtendimento { get; set; }
        public DateTime HoraAtendimento { get; set; }
        public string NrCarteira { get; set; }
        public bool? SnEmAtendimento { get; set; } = false;
        public string PrioridadeAtendimento { get; set; }
        public bool? SnRetorno { get; set; } = false;
        public bool? SnAcompanhante { get; set; } = false;
        public string UsuarioId { get; set; }
        public string StatusAgendamento { get; set; }
    }
}
