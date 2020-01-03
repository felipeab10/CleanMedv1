using CleanMed.Dados.Interface;
using CleanMed.Data;
using CleanMed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Dados.Repositorio
{
    public class ItemAgendamentoPrestadorRepositorio : RepositorioGenerico<ItemAgendamentoPrestador>, IItemAgendamentoPrestadorRepositorio
    {
        private readonly Contexto _contexto;
        public ItemAgendamentoPrestadorRepositorio(Contexto contexto) : base(contexto)
        {
            _contexto = contexto;
        }
    }
}
