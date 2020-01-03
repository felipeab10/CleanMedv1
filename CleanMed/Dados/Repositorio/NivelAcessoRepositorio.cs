using CleanMed.Dados.Interface;
using CleanMed.Data;
using CleanMed.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Dados.Repositorio
{
    public class NivelAcessoRepositorio : RepositorioGenerico<NiveisAcesso>, INivelAcessoRepositorio
    {
        private readonly Contexto _contexto;
        private readonly RoleManager<NiveisAcesso> _roleManager;
        public NivelAcessoRepositorio(Contexto contexto, RoleManager<NiveisAcesso> roleManager) : base(contexto)
        {
            _contexto = contexto;
            _roleManager = roleManager;
        }

        public async Task<bool> NivelAcessoExiste(string NivelAcesso)
        {
            return await _roleManager.RoleExistsAsync(NivelAcesso);
        }
    }
}
