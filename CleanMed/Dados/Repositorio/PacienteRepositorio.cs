﻿using CleanMed.Dados.Interface;
using CleanMed.Data;
using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Dados.Repositorio
{
    public class PacienteRepositorio : RepositorioGenerico<Paciente>, IPacienteRepositorio
    {
        private readonly Contexto _contexto;
        public PacienteRepositorio(Contexto contexto) : base(contexto)
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

        public async Task<bool> PacienteExisteCPF(string CPF)
        {
            return await _contexto.Pacientes.AnyAsync(c => c.CPF == CPF);
        }

        public async Task<bool> PacienteExisteCPF(string CPF ,int PacienteId)
        {
            return await _contexto.Pacientes.AnyAsync(c => c.CPF == CPF && c.PacienteId != PacienteId);
        }

        public async Task<bool> PacienteExisteNome(string Nome, DateTime DataNascimento)
        {
            return await _contexto.Pacientes.AnyAsync(c => c.Nome.ToUpper() == Nome.ToUpper() && c.DataNascimento == DataNascimento);
        }

        public async Task<bool> PacienteExisteNome(string Nome, DateTime DataNascimento, int PacienteId)
        {
            return await _contexto.Pacientes.AnyAsync(c => c.Nome.ToUpper() == Nome.ToUpper() && c.DataNascimento == DataNascimento && c.PacienteId != PacienteId);
        }
    }
}
