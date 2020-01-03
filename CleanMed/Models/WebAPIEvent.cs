using CleanMed.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class WebAPIEvent
    {
        public int id { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }

        public static explicit operator WebAPIEvent(Agendamento schedulerEvent)
        {
            return new WebAPIEvent
            {
                id = schedulerEvent.AgendamentoId,
                title = schedulerEvent.PacienteId.ToString(),
                start = schedulerEvent.HoraAgenda.ToString("dd-MM-yyyy HH:mm"),
                end = schedulerEvent.HoraAgenda.AddMinutes(30).ToString("dd-MM-yyyy HH:mm"),
            };
        }

        public static explicit operator Agendamento(WebAPIEvent schedulerEvent)
        {
            return new Agendamento
            {
                AgendamentoId = schedulerEvent.id,
                Color = schedulerEvent.title,
                HoraAgenda = DateTime.Parse(
                    schedulerEvent.start,
                    System.Globalization.CultureInfo.InvariantCulture),
                //HoraAgenda = DateTime.Parse(
                   // schedulerEvent.end_date,
                   // System.Globalization.CultureInfo.InvariantCulture)
            };
        }
    }
}
