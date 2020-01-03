using CleanMed.Dados.Interface;
using CleanMed.Data;
using CleanMed.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CleanMed.Dados.Repositorio
{
    public class UsuarioRepositorio : RepositorioGenerico<Usuario>, IUsuarioRepositorio
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;
        private readonly Contexto _contexto;
        public UsuarioRepositorio( SignInManager<Usuario> signInManager, UserManager<Usuario> userManager,Contexto contexto) : base(contexto)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _contexto = contexto;
        }
        public async Task<Usuario> PegarUsuarioLogado(ClaimsPrincipal usuario)
        {
            return await _userManager.GetUserAsync(usuario);
        }

        public async Task<IdentityResult> SalvarUsuario(Usuario usuario, string senha)
        {
            return await _userManager.CreateAsync(usuario, senha);
        }

        public async Task AtribuirNivelAcesso(Usuario usuario,string nivelAcesso)
        {
             await _userManager.AddToRoleAsync(usuario, nivelAcesso);
        }
        public async Task EfetuarLogin(Usuario usuario, bool lembrar)
        {
            await _signInManager.SignInAsync(usuario, lembrar);
        }
        public async Task EfetuarLogout()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<Usuario> PegarUsuarioPeloEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task AtualizarUsuario(Usuario usuario)
        {
            await _userManager.UpdateAsync(usuario);
        }

        public async Task<bool> UsuarioExisteNome(string Nome, DateTime DataNascimento)
        {
            return await _contexto.Usuarios.AnyAsync(u => u.Nome.ToUpper() == Nome.ToUpper() && u.DataNascimento == DataNascimento);
        }

        public async Task<bool> UsuarioExisteNome(string Nome, DateTime DataNascimento, string UsuarioId)
        {
            return await _contexto.Usuarios.AnyAsync(u => u.Nome.ToUpper() == Nome.ToUpper() && u.DataNascimento == DataNascimento && u.Id != UsuarioId);
        }

        public async Task<bool> UsuarioExisteCPF(string CPF)
        {
            return await _contexto.Usuarios.AnyAsync(u => u.CPF == CPF);
        }

        public async Task<bool> UsuarioExisteCPF(string CPF, string UsuarioId)
        {
            return await _contexto.Usuarios.AnyAsync(u => u.CPF == CPF && u.Id != UsuarioId);
        }

        public async Task<bool> UsuarioExisteUserName(string UserName)
        {
            return await _contexto.Usuarios.AnyAsync(u => u.UserName.ToUpper() == UserName.ToUpper());
        }

        public async Task<bool> UsuarioExisteUserName(string UserName, string UsuarioId)
        {
            return await _contexto.Usuarios.AnyAsync(u => u.UserName.ToUpper() == UserName.ToUpper() && u.Id != UsuarioId);
        }
    }
}
