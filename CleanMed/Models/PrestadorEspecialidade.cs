using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class PrestadorEspecialidade
    {
        public int PrestadorId { get; set; }
        public Prestador Prestador { get; set; }
        
        public int EspecialidadeId { get; set; }
        public Especialidade Especialidade { get; set; }
    }
}
