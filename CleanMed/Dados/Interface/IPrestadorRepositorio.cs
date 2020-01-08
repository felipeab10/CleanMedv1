using CleanMed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Dados.Interface
{
    public interface IPrestadorRepositorio:IRepositorioGenerico<Prestador>
    {
        Task<bool> PrestadorExisteNome(string Nome, DateTime DataNascimento);
        Task<bool> PrestadorExisteNome(string Nome, DateTime DataNascimento, int PrestadorId);
        Task<bool> PrestadorExisteNumeroCrm(string NumeroCrm,int ConselhoId);
        Task<bool> PrestadorExisteNumeroCrm(string NumeroCrm,int ConselhoId,int PrestadorId);
        Task<bool> PrestadorExisteCPF(string CPF);
        Task<bool> PrestadorExisteCPF(string CPF, int PrestadorId);
        bool DataAniversario(DateTime DataNascimento);

    }
}
