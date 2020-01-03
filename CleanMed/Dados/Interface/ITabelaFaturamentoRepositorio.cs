using CleanMed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Dados.Interface
{
    public interface ITabelaFaturamentoRepositorio:IRepositorioGenerico<TabelaFaturamento>
    {
        Task<bool> TabelaFaturamentoExiste(string Descricao);
        Task<bool> TabelaFaturamentoExiste(string Descricao,int TabelaFaturamentoId);
    }
}
