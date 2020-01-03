using CleanMed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Dados.Interface
{
    public interface IRecursoAgendamentoRepositorio:IRepositorioGenerico<RecursoAgendamento>
    {
        Task<bool> RecursoAgendamentoExiste(string Descricao);
        Task<bool> RecursoAgendamentoExiste(string Descricao, int RecursoAgendamentoId);
    }
}
