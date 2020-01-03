using CleanMed.Dados.Interface;
using CleanMed.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Dados.Repositorio
{
    public class RepositorioGenerico<TEntity> : IRepositorioGenerico<TEntity> where TEntity : class
    {
        private readonly Contexto _contexto;

        public RepositorioGenerico(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task Atualizar(TEntity entity)
        {
            _contexto.Set<TEntity>().Update(entity);
            await _contexto.SaveChangesAsync();
        }

        public async Task Excluir(int id)
        {
            var entity = await PegarPeloId(id);
            _contexto.Set<TEntity>().Remove(entity);
            await _contexto.SaveChangesAsync();
        }
        public async Task Excluir(string id)
        {
            var entity = await PegarPeloId(id);
            _contexto.Set<TEntity>().Remove(entity);
            await _contexto.SaveChangesAsync();
        }

        public async Task Inserir(TEntity entity)
        {
            await _contexto.Set<TEntity>().AddAsync(entity);
            await _contexto.SaveChangesAsync();
        }

        public async Task<TEntity> PegarPeloId(int id)
        {
            return await _contexto.Set<TEntity>().FindAsync(id);
        }
        public async Task<TEntity> PegarPeloId(string id)
        {
            return await _contexto.Set<TEntity>().FindAsync(id);
        }

        public IQueryable<TEntity> PegarTodos()
        {
            return _contexto.Set<TEntity>();
        }
    }
}
