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
    public class EmpresaRepositorio : RepositorioGenerico<Empresa>, IEmpresaRepositorio
    {
        private readonly Contexto _contexto;
        public EmpresaRepositorio(Contexto contexto) : base(contexto)
        {
            _contexto = contexto;
        }

        public async Task<bool> EmpresaExisteCNPJ(string CNPJ)
        {
            return await _contexto.Empresas.AnyAsync(e => e.CNPJ == CNPJ);
        }

        public async Task<bool> EmpresaExisteCNPJ(string CNPJ, int EmpresaId)
        {
            return await _contexto.Empresas.AnyAsync(e => e.CNPJ == CNPJ && e.EmpresaId != EmpresaId);
        }
    }
}
