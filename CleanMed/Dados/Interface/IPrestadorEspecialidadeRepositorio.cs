using CleanMed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Dados.Interface
{
    public interface IPrestadorEspecialidadeRepositorio:IRepositorioGenerico<PrestadorEspecialidade>
    {
        Task<bool> EspecialidadePrestadorExiste(int EspecialidadeId, int PrestadorId);
    }
}
