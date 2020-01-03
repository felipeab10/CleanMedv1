using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.ViewModels
{
    public class CartaoPacienteViewModel
    {
        public int PacienteId { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int ConvenioId { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string NumeroCarteira { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Validade { get; set; }
        public bool Status { get; set; } = true;
        public string ConvenioNome { get; set; }
        public int CartaoConvenio { get; set; }
    }
}
