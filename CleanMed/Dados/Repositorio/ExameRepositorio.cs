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
    public class ExameRepositorio : RepositorioGenerico<Exame>, IExameRepositorio
    {
        private readonly Contexto _contexto;
        public ExameRepositorio(Contexto contexto) : base(contexto)
        {
            _contexto = contexto;
        }
        public async Task<bool> ExameExiste(string Descricao)
        {
            return await _contexto.Exames.AnyAsync(e => e.Descricao.ToUpper() == Descricao.ToUpper());
        }

        public async Task<bool> ExameExiste(string Descricao, int ExameId)
        {
            return await _contexto.Exames.AnyAsync(e => e.Descricao.ToUpper() == Descricao.ToUpper() && e.ExameId != ExameId);
        }
    }
}
