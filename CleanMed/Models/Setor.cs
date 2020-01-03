using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class Setor
    {
        public int SetorId { get; set; }
        [Required(ErrorMessage ="Campo Obrigatório")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string TipoSetor { get; set; }
        public bool Status { get; set; } = true;

    }
}
