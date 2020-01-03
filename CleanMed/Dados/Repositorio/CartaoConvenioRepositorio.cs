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
    public class CartaoConvenioRepositorio:RepositorioGenerico<CartaoConvenio>,ICartaoConvenioRepositorio
    {
        private readonly Contexto _contexto;
        public CartaoConvenioRepositorio(Contexto contexto):base(contexto)
        {
            _contexto = contexto;
        }

        public async Task<bool> CartaoPacienteExiste(int PacienteId, string NumeroCarteira)
        {
            return await _contexto.CartaoConvenios.AnyAsync(a => a.PacienteId == PacienteId && a.NumeroCarteira == NumeroCarteira);
        }
    }
}
