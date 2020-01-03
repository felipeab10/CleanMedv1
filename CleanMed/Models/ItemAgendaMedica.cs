using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class ItemAgendaMedica
    {
        public int ItemAgendamentoId { get; set; }
        public ItemAgendamento ItemAgendamento { get; set; }
        public int AgendaMedicaId { get; set; }
        public AgendaMedica AgendaMedica { get; set; }
    }
}
