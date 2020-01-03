using CleanMed.Dados.Interface;
using CleanMed.Data;
using CleanMed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Dados.Repositorio
{
    public class ContatoRepositorio:RepositorioGenerico<Contato>,IContatoRepositorio
    {
        private readonly Contexto _contexto;
        public ContatoRepositorio(Contexto contexto):base (contexto)
        {
            _contexto = contexto;
        }
    }
}
