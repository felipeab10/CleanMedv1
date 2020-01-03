using CleanMed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Dados.Interface
{
   public interface ISetorRepositorio:IRepositorioGenerico<Setor>
    {
        Task<bool> SetorExiste(string Descricao);
        Task<bool> SetorExiste(string Descricao, int SetorId);
    }
}
