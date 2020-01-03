using CleanMed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Dados.Interface
{
    public interface IEspecialidadeRepositorio:IRepositorioGenerico<Especialidade>
    {
        Task<bool> EspecialidadeExiste(string Descricao);
        Task<bool> EspecialidadeExiste(string Descricao,int EspecialidadeId);
    }
}
