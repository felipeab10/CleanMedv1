using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class AgendamentoLog
    {
        public int AgendamentoLogId { get; set; }
        public DateTime Dt_Acao { get; set; }
        public string Acao { get; set; }
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public int AgendamentoId { get; set; }
        public Agendamento Agendamento { get; set; }
        public int? PacienteId { get; set; }
        public Paciente Paciente { get; set; }
    }
}
