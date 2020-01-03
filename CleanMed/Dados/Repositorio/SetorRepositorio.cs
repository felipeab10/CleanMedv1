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
    public class SetorRepositorio : RepositorioGenerico<Setor>, ISetorRepositorio
    {
        private readonly Contexto _contexto;
        public SetorRepositorio(Contexto contexto) : base(contexto)
        {
            _contexto = contexto;
        }
        public async Task<bool> SetorExiste(string Descricao)
        {
            return await _contexto.Setores.AnyAsync(s => s.Descricao.ToUpper() == Descricao.ToUpper());
        }

        public async Task<bool> SetorExiste(string Descricao, int SetorId)
        {
            return await _contexto.Setores.AnyAsync(s => s.Descricao.ToUpper() == Descricao.ToUpper() && s.SetorId != SetorId);
        }
    }
}
