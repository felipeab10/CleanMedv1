using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class TipoPrestador
    {
        public int TipoPrestadorId { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        [Remote("TipoPrestadorExiste","TipoPrestadores",AdditionalFields ="TipoPrestadorId")]
        public string Descricao { get; set; }
    }
}
