using CleanMed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Dados.Interface
{
   public interface ICartaoConvenioRepositorio:IRepositorioGenerico<CartaoConvenio>
    {
        Task<bool> CartaoPacienteExiste(int PacienteId,string NumeroCarteira);
    }
}
