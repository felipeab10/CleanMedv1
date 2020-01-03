using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class SobreUsuario
    {
        public int SobreUsuarioId { get; set; }
        public string Formacao { get; set; }
        public string Foto { get; set; }
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public int? CepId { get; set; }
        public Cep Cep { get; set; }
    }
}
