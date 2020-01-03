using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class CartaoConvenio
    {
        public int CartaoConvenioId { get; set; }
        public Convenio Convenio { get; set; }
        public int ConvenioId { get; set; }
        public ICollection<Convenio> Convenios { get; set; }
        public Paciente Paciente { get; set; }
        public int PacienteId { get; set; }
        public ICollection<Paciente> Pacientes { get; set; }
       
        [Required(ErrorMessage = "Campo Obrigatório")]
       
        [Range(1, int.MaxValue, ErrorMessage = "valor inválido")]
        public string NumeroCarteira { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Validade { get; set; }
        public bool Status { get; set; } = true;
    }
}
