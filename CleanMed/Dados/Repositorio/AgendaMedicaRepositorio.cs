using CleanMed.Dados.Interface;
using CleanMed.Data;
using CleanMed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Dados.Repositorio
{
    public class AgendaMedicaRepositorio:RepositorioGenerico<AgendaMedica>, IAgendaMedicaRepositorio
    {
        private readonly Contexto _Contexto;
        public AgendaMedicaRepositorio(Contexto contexto):base(contexto)
        {
            _Contexto = contexto;
        }
    }
}
