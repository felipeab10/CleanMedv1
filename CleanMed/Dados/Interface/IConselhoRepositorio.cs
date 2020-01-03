using CleanMed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Dados.Interface
{
   public interface IConselhoRepositorio:IRepositorioGenerico<Conselho>
    {
        Task<bool> CRMExiste(string Descricao);
        Task<bool> CRMExiste(string Descricao, int ConselhoId);
    }
}
