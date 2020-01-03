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
    public class ProcedimentoRepositorio:RepositorioGenerico<Procedimento>,IProcedimentoRepositorio
    {
        private readonly Contexto _Contexto;
        public ProcedimentoRepositorio(Contexto contexto) : base(contexto)
        {
            _Contexto = contexto;
        }

        public async Task<bool> ProcedimentoExiste(string Descricao)
        {
            return await _Contexto.Procedimentos.AnyAsync(a => a.Descricao.ToUpper() == Descricao.ToUpper());
        }

        public async Task<bool> ProcedimentoExiste(string Descricao, int ProcedimentoId)
        {
            return await _Contexto.Procedimentos.AnyAsync(a => a.Descricao.ToUpper() == Descricao.ToUpper() && a.ProcedimentoId != ProcedimentoId);
        }
    }
}
