using CleanMed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Dados.Interface
{
    public interface IItemAgendamentoRepositorio:IRepositorioGenerico<ItemAgendamento>
    {
        Task<bool> ItemAgendamentoExiste(string Descricao);
        Task<bool> ItemAgendamentoExiste(string Descricao,int ItemAgendamentoId);
    }
}
