using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class Procedimento
    {
        public int ProcedimentoId { get; set; }
        [Required(ErrorMessage ="Campo Obrigatório")]
        [Display(Name = "Descrição")]
        [Remote("ProcedimentoExiste","Procedimentos",AdditionalFields ="ProcedimentoId")]
        public string Descricao { get; set; }
        [Display(Name = "Idade Maxima")]
        public int? IdadeMaxima { get; set; }
        [Display(Name = "Idade Mínima")]
        public int? IdadeMinima { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Sexo { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [Display(Name = "Grupo de Faturamento")]
        public int GrupoFaturamentoId { get; set; }
        public GrupoFaturamento GrupoFaturamento { get; set; }
    }
}
