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
    public class PrestadorRepositorio : RepositorioGenerico<Prestador>, IPrestadorRepositorio
    {
        private readonly Contexto _contexto;
        public PrestadorRepositorio(Contexto contexto):base (contexto)
        {
            _contexto = contexto;
        }

        public bool DataAniversario(DateTime DataNascimento)
        {
            //var dt = DateTime.Parse(DataNascimento);
            var dtMin = DateTime.Parse("01/01/1900");
            var dtMax = DateTime.Parse("01/01/2050");
            if ((DataNascimento <= dtMin || DataNascimento >= DateTime.Now))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> PrestadorExisteNome(string Nome, DateTime DataNascimento)
        {
            return await _contexto.Prestadores.AnyAsync(p => p.Nome.ToUpper() == Nome.ToUpper() && p.DataNascimento == DataNascimento);
        }

        public async Task<bool> PrestadorExisteNome(string Nome, DateTime DataNascimento, int PrestadorId)
        {
            return await _contexto.Prestadores.AnyAsync(p => p.Nome.ToUpper() == Nome.ToUpper() && p.DataNascimento == DataNascimento && p.PrestadorId != PrestadorId);
        }

        public async Task<bool> PrestadorExisteNumeroCrm(string NumeroCrm, int ConselhoId )
        {
            return await _contexto.Prestadores.AnyAsync(p => p.NumeroCrm.ToUpper() == NumeroCrm.ToUpper() && p.ConselhoId == ConselhoId);
        }

        public async Task<bool> PrestadorExisteNumeroCrm(string NumeroCrm, int ConselhoId, int PrestadorId)
        {
            return await _contexto.Prestadores.AnyAsync(p => p.NumeroCrm.ToUpper() == NumeroCrm.ToUpper() && p.ConselhoId == ConselhoId && p.PrestadorId != PrestadorId);
        }

        public async Task<bool> PrestadorExisteCPF(string CPF)
        {
            return await _contexto.Prestadores.AnyAsync(p => p.CPF == CPF);
        }

        public async Task<bool> PrestadorExisteCPF(string CPF, int PrestadorId)
        {
            return await _contexto.Prestadores.AnyAsync(p => p.CPF == CPF && p.PrestadorId != PrestadorId);
        }
    }
}
