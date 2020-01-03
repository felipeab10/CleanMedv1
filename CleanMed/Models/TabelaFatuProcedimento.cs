using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class TabelaFatuProcedimento
    {
        public int TabelaFatuProcedimentoId { get; set; }
        [Required(ErrorMessage ="Campo Obrigatório")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data de Vigência")]
        public DateTime DataVigencia { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        [Display(Name = "Procedimento")]
        public int ProcedimentoId { get; set; }
        public Procedimento Procedimento { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        [Display(Name = "Valor")]
        
        
        public decimal ValorTotal { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public bool Status { get; set; } = true;
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int TabelaFaturamentoId { get; set; }
        public TabelaFaturamento TabelaFaturamento { get; set; }

    }
}
