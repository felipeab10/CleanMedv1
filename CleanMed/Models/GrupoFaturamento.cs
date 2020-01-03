using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class GrupoFaturamento
    {
        public int GrupoFaturamentoId { get; set; }
        [Remote("GrupoFaturamentoExiste","GrupoFaturamentos",AdditionalFields = "GrupoFaturamentoId")]
        [Required(ErrorMessage ="Campo obrigatório")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Tipo { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public bool Status { get; set; }
    }
}
