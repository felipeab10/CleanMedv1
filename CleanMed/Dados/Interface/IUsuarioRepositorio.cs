using CleanMed.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CleanMed.Dados.Interface
{
    public interface IUsuarioRepositorio:IRepositorioGenerico<Usuario>
    {
        Task<Usuario> PegarUsuarioLogado(ClaimsPrincipal usuario);
        Task<IdentityResult> SalvarUsuario(Usuario usuario,string senha);
        Task AtualizarUsuario(Usuario usuario);
        Task AtribuirNivelAcesso(Usuario usuario, string nivelAcesso);
        Task EfetuarLogin(Usuario usuario,bool lembrar);
        Task EfetuarLogout();
        Task<Usuario> PegarUsuarioPeloEmail(string email);
        Task<bool> UsuarioExisteNome(string Nome, DateTime DataNascimento);
        Task<bool> UsuarioExisteNome(string Nome, DateTime DataNascimento, string UsuarioId);
        Task<bool> UsuarioExisteCPF(string CPF);
        Task<bool> UsuarioExisteCPF(string CPF,string UsuarioId);
        Task<bool> UsuarioExisteUserName(string UserName);
        Task<bool> UsuarioExisteUserName(string UserName, string UsuarioId);
    }
}
