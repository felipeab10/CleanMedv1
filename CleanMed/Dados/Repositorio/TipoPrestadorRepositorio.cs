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
    public class TipoPrestadorRepositorio : RepositorioGenerico<TipoPrestador>, ITipoPrestadorRepositorio
    {
        private readonly Contexto _contexto;
        public TipoPrestadorRepositorio(Contexto contexto) : base(contexto)
        {
            _contexto = contexto;
        }
        public async Task<bool> TipoPrestadorExiste(string Descricao)
        {
            return await _contexto.TipoPrestadores.AnyAsync(t => t.Descricao.ToUpper() == Descricao.ToUpper());
        }

        public async Task<bool> TipoPrestadorExiste(string Descricao, int TipoPrestadorId)
        {
            return await _contexto.TipoPrestadores.AnyAsync(t => t.Descricao.ToUpper() == Descricao.ToUpper() && t.TipoPrestadorId != TipoPrestadorId);
        }
    }
}
