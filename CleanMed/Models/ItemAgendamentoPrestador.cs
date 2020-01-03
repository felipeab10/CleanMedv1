using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class ItemAgendamentoPrestador
    {
        [Required(ErrorMessage ="Campo Obrigatório")]
        public int PrestadorId { get; set; }
        public Prestador Prestador { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int ItemAgendamentoId { get; set; }
        public ItemAgendamento itemAgendamento { get; set; }
    }
}
