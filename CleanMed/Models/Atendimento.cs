using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class Atendimento
    {
        public int AtendimentoId { get; set; }
        public int PacienteId { get; set; }
        public Paciente Paciente { get; set; }
        public int SetorId { get; set; }
        public Setor Setor { get; set; }
        public int ConvenioId { get; set; }
        public Convenio Convenio { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataAtendimento { get; set; }
        [DisplayFormat(DataFormatString = "{0:T}", ApplyFormatInEditMode = true)]
        public DateTime HoraAtendimento { get; set; }
        public int? PrestadorId { get; set; }
        public Prestador Prestador { get; set; }
        public string NrCarteira { get; set; }
        public bool? SnEmAtendimento { get; set; } = false;
        public int MyProperty { get; set; }
        public string PrioridadeAtendimento { get; set; }
        public bool? SnRetorno { get; set; } = false;
        public bool? SnAcompanhante { get; set; } = false;
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
      
    }
}
