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
    public class GrupoFaturamentoRepositorio:RepositorioGenerico<GrupoFaturamento>,IGrupoFaturamentoRepositorio
    {
        private readonly Contexto _Contexto;
        public GrupoFaturamentoRepositorio(Contexto contexto) :base(contexto)
        {
            _Contexto = contexto;
        }

        public async Task<bool> GrupoFaturamentoExiste(string Descricao)
        {
            return await _Contexto.GrupoFaturamentos.AnyAsync(a => a.Descricao.ToUpper() == Descricao.ToUpper());
        }

        public async Task<bool> GrupoFaturamentoExiste(string Descricao, int GrupoFaturamentoId)
        {
            return await _Contexto.GrupoFaturamentos.AnyAsync(a => a.Descricao.ToUpper() == Descricao.ToUpper() && a.GrupoFaturamentoId != GrupoFaturamentoId);
        }
    }
}
