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
    public class ConvenioRepositorio : RepositorioGenerico<Convenio>, IConvenioRepositorio
    {
        public readonly Contexto _contexto;
        public ConvenioRepositorio(Contexto contexto):base(contexto)
        {
            _contexto = contexto;
        }
        public async Task<bool> ConvenioExiste(string CNPJ)
        {
            return await _contexto.Convenios.AnyAsync(a => a.CNPJ ==  CNPJ);
        }

        public async Task<bool> ConvenioExiste(string CNPJ, int ConvenioId)
        {
            return await _contexto.Convenios.AnyAsync(a => a.CNPJ == CNPJ && a.ConvenioId != ConvenioId);
        }
        public async Task<bool> ConvenioExisteRegistroANS(string RegistroAns)
        {
            return await _contexto.Convenios.AnyAsync(a => a.RegistroAns == RegistroAns);
        }

        public async Task<bool> ConvenioExisteRegistroANS(string RegistroAns, int ConvenioId)
        {
            return await _contexto.Convenios.AnyAsync(a => a.RegistroAns == RegistroAns && a.ConvenioId != ConvenioId);
        }
    }
}
