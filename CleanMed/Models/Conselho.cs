using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class Conselho
    {
        public int ConselhoId { get; set; }
        [Required(ErrorMessage ="Campo Obrigatório")]
        [Remote("CRMExiste","Conselhos",AdditionalFields = "ConselhoId")]
        public string Descricao { get; set; }
    }
}
