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
    public class ConselhoRepositorio : RepositorioGenerico<Conselho>, IConselhoRepositorio
    {
        private readonly Contexto _contexto;
        public ConselhoRepositorio(Contexto contexto) : base(contexto)
        {
            _contexto = contexto;
        }

        public async Task<bool> CRMExiste(string Descricao)
        {
            return await _contexto.Conselhos.AnyAsync(c => c.Descricao.ToUpper() == Descricao.ToUpper());
        }

        public async Task<bool> CRMExiste(string Descricao, int ConselhoId)
        {
            return await _contexto.Conselhos.AnyAsync(c => c.Descricao.ToUpper() == Descricao.ToUpper() && c.ConselhoId != ConselhoId);
        }
    }
}
