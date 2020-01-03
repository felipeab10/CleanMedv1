using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class Exame
    {
        public int ExameId { get; set; }
        [Required(ErrorMessage ="Campo obrigatório")]
        [MaxLength(160)]
        public string Descricao { get; set; }
        public string Mnemonico { get; set; }
        public bool Status { get; set; }
        public int? DiaEntrega { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Sexo { get; set; }
       
        public string PreparoExame { get; set; }
        public Setor Setor { get; set; }
        public int? SetorId { get; set; }
        public Especialidade Especialidade { get; set; }
        public int? EspecialidadeId { get; set; }
        
    }
}
