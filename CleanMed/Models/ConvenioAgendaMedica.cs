using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class ConvenioAgendaMedica
    {
        public int ConvenioId { get; set; }
        public Convenio Convenio { get; set; }
        public int AgendaMedicaId { get; set; }
        public AgendaMedica AgendaMedica { get; set; }
    }
}
