using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class ConfirmacaoAgendamento
    {
        public int ConfirmacaoAgendamentoId { get; set; }
        [Required(ErrorMessage ="Campo Obrigatório")]
        public string TipoConfirmacao { get; set; }
        public string Nomecontato { get; set; }
        public string ObservacaoConfirmacao { get; set; }
        public int AgendamentoId { get; set; }
        public Agendamento Agendamento { get; set; }

    }
}
