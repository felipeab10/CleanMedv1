using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class AgendaMedica
    {
        public int AgendaMedicaId { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataAgenda { get; set; }
        public int? RecursoAgendamentoId { get; set; }
        public RecursoAgendamento RecursoAgendamento { get; set; }
        public int? PrestadorId { get; set; }
        public Prestador Prestador { get; set; }
        [DisplayFormat(DataFormatString = "{0:T}", ApplyFormatInEditMode = true)]
        public DateTime HoraInicio { get; set; }
        [DisplayFormat(DataFormatString = "{0:T}", ApplyFormatInEditMode = true)]
        public DateTime HoraFim { get; set; }
        public bool StatusAgenda { get; set; }
        public DateTime DataLiberacao { get; set; }
        public int QtAtendimento { get; set; }
        public int QtEncaixe { get; set; }
        public DateTime DtCadastro { get; set; } = DateTime.Now;
        public string Observacao { get; set; }
        public string ThemeColor { get; set; }
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public int? EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
        public TimeSpan QtTempoMedio { get; set; }
        public int? SetorId { get; set; }
        public Setor Setor { get; set; }
        public string TipoAgenda { get; set; }
        public ICollection<ItemAgendaMedica> ItensAgendasMedica { get; set; }
        public ICollection<ConvenioAgendaMedica> ConveniosAgendasMedica { get; set; }

    }
}
