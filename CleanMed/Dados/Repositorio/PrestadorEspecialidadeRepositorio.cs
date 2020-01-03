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
    public class PrestadorEspecialidadeRepositorio : RepositorioGenerico<PrestadorRepositorio>, IPrestadorEspecialidadeRepositorio
    {
        private readonly Contexto _contexto;
        public PrestadorEspecialidadeRepositorio(Contexto contexto) : base(contexto)
        {
            _contexto = contexto;
        }

        public Task Atualizar(PrestadorEspecialidade entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EspecialidadePrestadorExiste(int EspecialidadeId, int PrestadorId)
        {
            return await _contexto.PrestadoresEspecialidades.AnyAsync(a=> a.EspecialidadeId == EspecialidadeId && a.PrestadorId == PrestadorId);
        }

        public Task Inserir(PrestadorEspecialidade entity)
        {
            throw new NotImplementedException();
        }

        Task<PrestadorEspecialidade> IRepositorioGenerico<PrestadorEspecialidade>.PegarPeloId(int id)
        {
            throw new NotImplementedException();
        }

        Task<PrestadorEspecialidade> IRepositorioGenerico<PrestadorEspecialidade>.PegarPeloId(string id)
        {
            throw new NotImplementedException();
        }

        IQueryable<PrestadorEspecialidade> IRepositorioGenerico<PrestadorEspecialidade>.PegarTodos()
        {
            throw new NotImplementedException();
        }
    }
}
