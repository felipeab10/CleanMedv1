using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class RecursoAgendamento
    {
        public int RecursoAgendamentoId { get; set; }
        [Required(ErrorMessage ="Campo obrigatório")]
        [Remote("RecursoAgendamentoExiste","RecursoAgendamentos",AdditionalFields ="RecursoAgendamentoId")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(4,ErrorMessage ="Campo limitado a 4 caracteres")]
        public string Sigla { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Tipo { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public bool Status { get; set; } = true;
    }
}
