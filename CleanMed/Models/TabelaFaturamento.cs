using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class TabelaFaturamento
    {
        public int TabelaFaturamentoId { get; set; }
        [Required(ErrorMessage ="Campo obrigatório")]
        [Remote("TabelaFaturamentoExiste","TabelaFaturamentos",AdditionalFields ="TabelaFaturamentoId")]
        public string Descricao { get; set; }
        public int ConvenioId { get; set; }
        public Convenio Convenio { get; set; }
    }
}
