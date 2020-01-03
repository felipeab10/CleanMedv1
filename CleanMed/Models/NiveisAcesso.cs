using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class NiveisAcesso:IdentityRole
    {
        
        [Required(ErrorMessage ="Campo Obrigatório")]
       
        public string Descricao { get; set; }
       
    }
}
