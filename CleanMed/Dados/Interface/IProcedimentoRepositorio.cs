using CleanMed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Dados.Interface
{
    public interface IProcedimentoRepositorio:IRepositorioGenerico<Procedimento>
    {
        Task<bool> ProcedimentoExiste(string Descricao);
        Task<bool> ProcedimentoExiste(string Descricao,int ProcedimentoId);
    }
}
