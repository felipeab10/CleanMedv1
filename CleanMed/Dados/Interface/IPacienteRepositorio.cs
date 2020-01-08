using CleanMed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Dados.Interface
{
   public interface IPacienteRepositorio:IRepositorioGenerico<Paciente>
    {
        Task<bool> PacienteExisteCPF(string CPF);
        Task<bool> PacienteExisteCPF(string CPF,int PacienteId);
        Task<bool> PacienteExisteNome(string Nome, DateTime DataNascimento);
        Task<bool> PacienteExisteNome(string Nome, DateTime DataNascimento,int PacienteId);
        bool DataAniversario(DateTime DataNascimento);
    }
}
