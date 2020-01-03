using CleanMed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Dados.Interface
{
   public interface IEmpresaRepositorio:IRepositorioGenerico<Empresa>
    {
        Task<bool> EmpresaExisteCNPJ(string CNPJ);
        Task<bool> EmpresaExisteCNPJ(string CNPJ,int EmpresaId);
    }
}
