using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class Especialidade
    {
        public int EspecialidadeId { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [Remote("EspecialidadeExiste", "Especialidades", AdditionalFields = "EspecialidadeId")]
        public string Descricao { get; set; }
       
        public ICollection<PrestadorEspecialidade> PrestadoresEspecialidades { get; set; }
    }
}
