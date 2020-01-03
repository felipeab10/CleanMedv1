using CleanMed.Data;
using CleanMed.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.ViewComponents
{
    public class UserInformationViewComponent:ViewComponent
    {
        private readonly Contexto _contexto;
        public UserInformationViewComponent(Contexto contexto)
        {
            _contexto = contexto;
        }
        public async Task<IViewComponentResult> InvokeAsync(string Id)
        {
            var usuario =  _contexto.Usuarios.FirstOrDefault(u => u.Id == Id);
            var nivelAcesso = (from n in _contexto.NiveisAcessos
                               join r in _contexto.UserRoles
                               on n.Id equals r.RoleId
                               join u in _contexto.Usuarios
                               on r.UserId equals u.Id
                               where u.Id == Id
                               select n.Name).FirstOrDefault();
            var Setor = (from s in _contexto.Setores
                         join u in _contexto.Usuarios
                         on s.SetorId equals u.SetorId
                         where u.Id == Id
                         select s.Descricao).FirstOrDefault();
            var sobreUsuario = _contexto.SobreUsuarios.Where(s => s.UsuarioId == Id).FirstOrDefault();

            UserInformationViewModel user = new UserInformationViewModel();
            user.Nome = usuario.Nome;
            user.NivelAcesso = nivelAcesso;
            user.NomeSetor = Setor;
            user.DtCadastro = usuario.DtCadastro;
            user.Id = usuario.Id;
           if(sobreUsuario != null)
            {
                user.Foto = sobreUsuario.Foto;
            }
          
           
            return View(user);
        }
        
    }
}
