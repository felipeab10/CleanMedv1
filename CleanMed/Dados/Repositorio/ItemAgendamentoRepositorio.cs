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
    public class ItemAgendamentoRepositorio : RepositorioGenerico<ItemAgendamento>, IItemAgendamentoRepositorio
    {
        private readonly Contexto _contexto;
        public ItemAgendamentoRepositorio(Contexto contexto):base(contexto)
        {
            _contexto = contexto;
        }
        public async Task<bool> ItemAgendamentoExiste(string Descricao)
        {
            return await _contexto.ItemAgendamentos.AnyAsync(i => i.Descricao.ToUpper() == Descricao.ToUpper());
        }

        public async Task<bool> ItemAgendamentoExiste(string Descricao, int ItemAgendamentoId)
        {
            return await _contexto.ItemAgendamentos.AnyAsync(i => i.Descricao.ToUpper() == Descricao.ToUpper() && i.ItemAgendamentoId != ItemAgendamentoId);
        }
    }
}
