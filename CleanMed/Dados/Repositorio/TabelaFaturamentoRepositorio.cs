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
    public class TabelaFaturamentoRepositorio : RepositorioGenerico<TabelaFaturamento>, ITabelaFaturamentoRepositorio
    {
        private readonly Contexto _contexto;
        public TabelaFaturamentoRepositorio(Contexto contexto):base (contexto)
        {
            _contexto = contexto;
        }
        public async Task<bool> TabelaFaturamentoExiste(string Descricao)
        {
            return await _contexto.TabelaFaturamentos.AnyAsync(a => a.Descricao.ToUpper() == Descricao.ToUpper());
        }

        public async Task<bool> TabelaFaturamentoExiste(string Descricao, int TabelaFaturamentoId)
        {
            return await _contexto.TabelaFaturamentos.AnyAsync(a => a.Descricao.ToUpper() == Descricao.ToUpper() && a.TabelaFaturamentoId != TabelaFaturamentoId);
        }
    }
}
