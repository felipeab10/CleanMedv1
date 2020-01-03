using CleanMed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Dados.Interface
{
  public interface IConvenioRepositorio:IRepositorioGenerico<Convenio>
    {
        Task<bool> ConvenioExiste(string CNPJ);
        Task<bool> ConvenioExiste(string CNPJ,int ConvenioId);

        Task<bool> ConvenioExisteRegistroANS(string RegistroAns);
        Task<bool> ConvenioExisteRegistroANS(string RegistroAns, int ConvenioId);
    }
}
