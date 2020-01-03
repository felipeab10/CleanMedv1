using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class UsuarioEmpresa
    {
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}
