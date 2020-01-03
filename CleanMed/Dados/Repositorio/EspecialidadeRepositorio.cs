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
    public class EspecialidadeRepositorio : RepositorioGenerico<Especialidade>, IEspecialidadeRepositorio
    {
        private readonly Contexto _contexto;
        public EspecialidadeRepositorio(Contexto contexto): base(contexto)
        {
            _contexto = contexto;
        }
        public async Task<bool> EspecialidadeExiste(string Descricao)
        {
            return await _contexto.Especialidades.AnyAsync(e => e.Descricao.ToUpper() == Descricao.ToUpper());
        }

        public async Task<bool> EspecialidadeExiste(string Descricao, int EspecialidadeId)
        {
            return await _contexto.Especialidades.AnyAsync(e => e.Descricao.ToUpper() == Descricao.ToUpper() && e.EspecialidadeId != EspecialidadeId);
        }
    }
}
