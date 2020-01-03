using CleanMed.Dados.Interface;
using CleanMed.Data;
using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Dados.Repositorio
{
    public class RecursoAgendamentoRepositorio : RepositorioGenerico<RecursoAgendamento>, IRecursoAgendamentoRepositorio
    {
        private readonly Contexto _Contexto;
        public RecursoAgendamentoRepositorio(Contexto contexto) : base(contexto)
        {
            _Contexto = contexto;
        }
        public async Task<bool> RecursoAgendamentoExiste(string Descricao)
        {
            return await _Contexto.RecursoAgendamentos.AnyAsync(r => r.Descricao.ToUpper() == Descricao.ToUpper());
        }

        public async Task<bool> RecursoAgendamentoExiste(string Descricao, int RecursoAgendamentoId)
        {
            return await _Contexto.RecursoAgendamentos.AnyAsync(r => r.Descricao.ToUpper() == Descricao.ToUpper() && r.RecursoAgendamentoId != RecursoAgendamentoId);
        }
    }
}
