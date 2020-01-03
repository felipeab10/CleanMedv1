using CleanMed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Dados.Interface
{
    public interface IGrupoFaturamentoRepositorio:IRepositorioGenerico<GrupoFaturamento>
    {
        Task<bool> GrupoFaturamentoExiste(string Descricao);
        Task<bool> GrupoFaturamentoExiste(string Descricao, int GrupoFaturamentoId);
    }
}
