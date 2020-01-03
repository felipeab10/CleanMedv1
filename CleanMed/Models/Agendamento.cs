using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class Agendamento
    {
        public int AgendamentoId { get; set; }
        public int AgendaMedicaId { get; set; }
        public AgendaMedica AgendaMedica { get; set; }
        public DateTime HoraAgenda { get; set; }
        public int? PacienteId { get; set; }
        public Paciente Paciente { get; set; }
        public int VlAltura { get; set; }
        public int Qtpeso { get; set; }
        public string StatusAgendamento { get; set; }
        public bool? SNEncaixe { get; set; }
        public int? AtendimentoId { get; set; }
        public string? UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public int? ConvenioId { get; set; }
        public Convenio Convenio { get; set; }
        public int? ItemAgendamentoId { get; set; }
        public ItemAgendamento ItemAgendamento { get; set; }
        public bool Bloqueado { get; set; } = false;
        public string Color { get; set; } = "#4caf50";
        public string ObservacaoAgendamento { get; set; }
        public int? MotivoCancelamentoId { get; set; }
        public MotivoCancelamento MotivoCancelamento { get; set; }

    }
}
