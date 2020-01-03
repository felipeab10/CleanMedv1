using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace CleanMed.Models
{
    public class ItemAgendamento
    {
        public int ItemAgendamentoId { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        [Remote("ItemAgendamentoExiste", "ItemAgendamentos", AdditionalFields = "ItemAgendamentoId")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Tipo { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public bool Status { get; set; }

        [Display(Name = "Exame")]
        public int? ExameId { get; set; }
        public Exame Exame { get; set; }
        [Display(Name = "Recurso")]
        public int? RecursoAgendamentoId { get; set; }
        public RecursoAgendamento RecursoAgendamento { get; set; }
        public ICollection<ItemAgendamentoPrestador> ItemAgendamentoPrestadores { get; set; }
        public ICollection<ItemAgendaMedica> ItensAgendasMedica { get; set; }
        public int id
        {
            get { return ItemAgendamentoId; }
        }
        public string text
        {
            get { return Descricao; }
        }
    }
}
