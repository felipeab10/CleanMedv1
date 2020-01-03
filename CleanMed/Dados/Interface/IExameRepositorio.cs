using CleanMed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Dados.Interface
{
    public interface IExameRepositorio:IRepositorioGenerico<Exame>
    {
        Task<bool> ExameExiste(string Descricao);
        Task<bool> ExameExiste(string Descricao,int ExameId);
    }
}
