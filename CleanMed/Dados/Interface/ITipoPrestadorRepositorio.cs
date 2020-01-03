using CleanMed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Dados.Interface
{
    public interface ITipoPrestadorRepositorio:IRepositorioGenerico<TipoPrestador>
    {
        Task<bool> TipoPrestadorExiste(string Descricao);
        Task<bool> TipoPrestadorExiste(string Descricao,int TipoPrestadorId);
    }
}
