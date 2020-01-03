using CleanMed.Dados.Interface;
using CleanMed.Data;
using CleanMed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Dados.Repositorio
{
    public class TabelaFatuProcedimentoRepositorio:RepositorioGenerico<TabelaFatuProcedimento>, ITabelaFatuProcedimentoRepositorio
    {
        private readonly Contexto _contexto;
        public TabelaFatuProcedimentoRepositorio(Contexto contexto) : base(contexto)
        {
            _contexto = contexto;
        }
    }
}
