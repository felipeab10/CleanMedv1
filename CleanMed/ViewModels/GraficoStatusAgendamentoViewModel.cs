using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.ViewModels
{
    public class GraficoStatusAgendamentoViewModel
    {
        public double Agendados { get; set; }
        public double Confirmados { get; set; }
        public double Atendidos { get; set; }
        public double Cancelados { get; set; }
        public double Excluidos { get; set; }
        public double Livre { get; set; }

    }
}
