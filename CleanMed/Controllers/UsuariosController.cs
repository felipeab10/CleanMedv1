using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Threading.Tasks;
using CleanMed.Dados.Interface;
using CleanMed.Data;
using CleanMed.Models;
using CleanMed.Servicos;
using CleanMed.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanMed.Controllers
{
    // [Authorize(Roles =  "ADMINISTRADOR" )]

    public class UsuariosController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ILogger<UsuariosController> _logger;
        private readonly Contexto _contexto;
        private readonly IHostingEnvironment _hostingEnvironment;

        public UsuariosController(IUsuarioRepositorio usuarioRepositorio, ILogger<UsuariosController> logger, Contexto contexto, IHostingEnvironment hostingEnvironment)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _logger = logger;
            _contexto = contexto;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index(int? pageNumber, string searchId, string searchNome, int searchSetorId, string searchStatusId)
        {
            ViewData["searchId"] = searchId;
            ViewData["searchNome"] = searchNome;
            ViewData["searchSetorId"] = searchSetorId;
            ViewData["searchStatusId"] = searchStatusId;


            var usuarios = from s in _contexto.Usuarios
                           select s;
            if (!String.IsNullOrEmpty(searchId))
            {

                usuarios = usuarios.Where(s => s.Id == searchId);
            }
            if (!String.IsNullOrEmpty(searchNome))
            {
                usuarios = usuarios.Where(s => s.Nome.Contains(searchNome)
                || s.UserName.Contains(searchNome));
            }
            if (searchSetorId > 0)
            {
                usuarios = usuarios.Where(s => s.SetorId == searchSetorId);


            }
            if (searchStatusId == "true")
            {
                bool teste = true;
                usuarios = usuarios.Where(s => s.Status == teste);
            }
            if (searchStatusId == "false")
            {
                bool teste = false;
                usuarios = usuarios.Where(s => s.Status == teste);
            }

            ViewData["SetorId"] = new SelectList(_contexto.Setores, "SetorId", "Descricao");
            var listStatus = new SelectList(new[] {
                new {ID = "true",NAME = "Ativo" },
                new {ID = "false",NAME = "Inativo" },
            }, "ID", "NAME");
            ViewData["StatusId"] = listStatus;

            int pageSize = 5;
            return View(await PaginatedList<Usuario>.CreateAsync(usuarios.AsNoTracking().Include(s => s.Setor).Include(s => s.Cep).OrderBy(u => u.Nome), pageNumber ?? 1, pageSize));
        }
        public IActionResult Create()
        {
            ViewData["NivelAcessoId"] = new SelectList(_contexto.NiveisAcessos, "Name", "Name");
            ViewData["SetorId"] = new SelectList(_contexto.Setores, "SetorId", "Descricao");
            var listStatus = new SelectList(new[]
            {
                new{ID = "true",NAME = "Ativo"},
                new{ID = "false",NAME = "Inativo"},
            }, "ID", "NAME");
            ViewData["StatusId"] = listStatus;
            _logger.LogInformation("Abrindo view de adicionar usuário");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario, string NivelAcessoId)
        {
            if (ModelState.IsValid)
            {


                usuario.NormalizedUserName = usuario.UserName.ToUpper();
                usuario.NormalizedEmail = usuario.Email.ToUpper();
                var senhaCriptografada = Criptografia.Codifica(usuario.PasswordHash);
                usuario.PasswordHash = senhaCriptografada;
                if (ValidarCPF(usuario.CPF))
                {
                    ModelState.AddModelError("CPF", "CPF já cadastrado");
                    ViewData["NivelAcessoId"] = new SelectList(_contexto.NiveisAcessos, "Name", "Name");
                    ViewData["SetorId"] = new SelectList(_contexto.Setores, "SetorId", "Descricao");
                    var listStatuss = new SelectList(new[]
                    {
                         new{ID = "true",NAME = "Ativo"},
                         new{ID = "false",NAME = "Inativo"},
                    }, "ID", "NAME");
                    ViewData["StatusId"] = listStatuss;
                    return View(usuario);
                }
                _logger.LogInformation("Adicionando usuário");
                await _usuarioRepositorio.Inserir(usuario);
                _logger.LogInformation("Usuário Adicionado");


                if (NivelAcessoId != null)
                {
                    _logger.LogInformation("Atribuindo nível de acesso ao usuárioa adicionado");
                    await _usuarioRepositorio.AtribuirNivelAcesso(usuario, NivelAcessoId);
                }
                TempData["Mensagem"] = "Adicionado com sucesso";
                return RedirectToAction("Index");
            }

            ViewData["NivelAcessoId"] = new SelectList(_contexto.NiveisAcessos, "Name", "Name");
            ViewData["SetorId"] = new SelectList(_contexto.Setores, "SetorId", "Descricao");
            var listStatus = new SelectList(new[]
            {
                new{ID = "true",NAME = "Ativo"},
                new{ID = "false",NAME = "Inativo"},
            }, "ID", "NAME");
            ViewData["StatusId"] = listStatus;
            _logger.LogError("Informações Inválidas");
            return View(usuario);
        }

        public async Task<IActionResult> Edit(string UserName)
        {
            if (UserName == null)
            {
                _logger.LogError("Username não encontrado");
                return NotFound();
            }
            _logger.LogInformation("Procurando usuário pelo username");
            var username = await _contexto.Usuarios.Where(u => u.UserName == UserName).FirstAsync();
            if (username == null)
            {
                _logger.LogError("Username não encontrado");
                return NotFound();
            }
            //NiveisAcesso niveisAcesso = new NiveisAcesso();
            var nivelUser = (from n in _contexto.NiveisAcessos
                             join r in _contexto.UserRoles
                             on n.Id equals r.RoleId
                             join u in _contexto.Usuarios
                             on r.UserId equals u.Id
                             where u.UserName == username.UserName
                             select n).FirstOrDefault();


            ViewData["NivelAcessoId"] = new SelectList(_contexto.NiveisAcessos, "Name", "Name", nivelUser);
            ViewData["SetorId"] = new SelectList(_contexto.Setores, "SetorId", "Descricao");
            var listStatus = new SelectList(new[]
            {
                new{ID = "true",NAME = "Ativo"},
                new{ID = "false",NAME = "Inativo"},
            }, "ID", "NAME");
            ViewData["StatusId"] = listStatus;
            _logger.LogInformation("enviando usuário para view edit");
            return View(username);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string Id, Usuario usuario)
        {
            if (Id != usuario.Id)
            {
                _logger.LogError("Id enviado diferente do modelo enviado");
                return NotFound();
            }
            _logger.LogInformation("Validando se o modelo é valido");
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Atualizando Usuário");

                _logger.LogInformation("Pegando usuário pelo model usuario.id");
                var userName = await _usuarioRepositorio.PegarPeloId(usuario.Id);
                userName.Nome = usuario.Nome;
                userName.DataNascimento = usuario.DataNascimento;
                userName.CPF = usuario.CPF;
                userName.SetorId = usuario.SetorId;
                userName.Status = usuario.Status;
                userName.UserName = usuario.UserName;
                userName.Email = usuario.Email;
                userName.Telefone = usuario.Telefone;
                userName.Observacao = usuario.Observacao;
                userName.NormalizedUserName = usuario.UserName.ToUpper();
                userName.NormalizedEmail = usuario.Email.ToUpper();
                if (usuario.PasswordHash != null)
                {

                    var senhaCriptografada = Criptografia.Codifica(usuario.PasswordHash);
                    userName.PasswordHash = senhaCriptografada;
                }
                await _usuarioRepositorio.Atualizar(userName);
                _logger.LogInformation("Usuário atualizado");
                TempData["Mensagem"] = "Atualizado com sucesso";
                return RedirectToAction("Index");
            }
            _logger.LogInformation("Modelo não é valido");

            var nivelUser = (from n in _contexto.NiveisAcessos
                             join r in _contexto.UserRoles
                             on n.Id equals r.RoleId
                             join u in _contexto.Usuarios
                             on r.UserId equals u.Id
                             where u.UserName == usuario.UserName
                             select n).FirstOrDefault();



            ViewData["NivelAcessoId"] = new SelectList(_contexto.NiveisAcessos, "Name", "Name", nivelUser);
            ViewData["SetorId"] = new SelectList(_contexto.Setores, "SetorId", "Descricao");
            var listStatus = new SelectList(new[]
            {
                new{ID = "true",NAME = "Ativo"},
                new{ID = "false",NAME = "Inativo"},
            }, "ID", "NAME");
            ViewData["StatusId"] = listStatus;
            _logger.LogError("Informações inválidas");
            return View(usuario);
        }

        public async Task<IActionResult> NivelAcesso(string UsuarioId)
        {
            var nivelAcesso = await (from n in _contexto.NiveisAcessos
                                     join r in _contexto.UserRoles
                                     on n.Id equals r.RoleId
                                     join u in _contexto.Usuarios
                                     on r.UserId equals u.Id
                                     where u.Id == UsuarioId
                                     orderby u.Nome
                                     select n).ToListAsync();
            ViewData["UsuarioId"] = UsuarioId;
            return View(nivelAcesso);
        }
        public IActionResult AddNivelUsuario(string UsuarioId)
        {
            ViewData["NivelAcessoId"] = new SelectList(_contexto.NiveisAcessos, "Name", "Name");
            ViewData["UsuarioId"] = UsuarioId;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNivelUsuario(string UsuarioId, string NivelAcessoId)
        {
            _logger.LogInformation("Procurando usuario pelo Id");
            var usuario = await _usuarioRepositorio.PegarPeloId(UsuarioId);
            _logger.LogInformation("Atribuindo Nível acesso ao usuário");
            await _usuarioRepositorio.AtribuirNivelAcesso(usuario, NivelAcessoId);
            _logger.LogInformation("Nível de acesso atribuido");
            TempData["Mensagem"] = "Adicionado com sucesso.";
            return RedirectToAction("Index");
        }
        public bool ValidarCPF(string CPF)
        {
            return _contexto.Usuarios.Any(u => u.CPF == CPF);
        }
        public async Task<JsonResult> RemoveNivelAcessoUsuario(string NivelAcessoId, string UsuarioId)
        {
            var nivelAcesso = _contexto.UserRoles.Where(a => a.RoleId == NivelAcessoId && a.UserId == UsuarioId).FirstOrDefault();
            _contexto.Remove(nivelAcesso);
            await _contexto.SaveChangesAsync();
            TempData["Mensagem"] = "Removido com sucesso";
            return Json("Permissão Removido");
        }
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                //_logger.LogInformation("Usuário logado fazendo logout");
                //await _usuarioRepositorio.EfetuarLogout();
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                _logger.LogInformation("Entrando na pagina de login");
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, bool lembrar)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Pegando usuário pelo username");
                var usuario = await _contexto.Usuarios.FirstOrDefaultAsync(u => u.UserName == loginViewModel.UserName);
                PasswordHasher<Usuario> passwordHasher = new PasswordHasher<Usuario>();
                if (usuario != null)
                {
                    _logger.LogInformation("verificando informações do usuario");
                    if (Criptografia.Compara(loginViewModel.PasswordHash, usuario.PasswordHash))
                    {
                        _logger.LogInformation("Informações corretas,Efetuando login do usuário");
                        await _usuarioRepositorio.EfetuarLogin(usuario, false);
                       // TempData["Mensagem"] = " Bem-vindo ao sistema!";
                        TempData["User"] = usuario.Nome;
                        return RedirectToAction("Index", "Dashboard");
                    }
                    _logger.LogError("Usuário ou senha não confere");
                    TempData["Validacao"] = "Usuário ou senha não confere";
                    return View(loginViewModel);
                }
                _logger.LogError("Erro ao efetuar o login, verifique as informações digitadas");
                TempData["Validacao"] = "Erro ao efetuar o login, verifique as informações digitadas";
                return View(loginViewModel);
            }
            _logger.LogError("O modelo não é valido");
            TempData["Validacao"] = "Verifique o usuário e senha digitado";
            return View(loginViewModel);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _usuarioRepositorio.EfetuarLogout();
            return RedirectToAction("Login", "Usuarios");
        }
        public async Task<IActionResult> PerfilUsuario(string UsuarioId)
        {
            _logger.LogInformation("Pegando o Usuário logado");
            var usuarioLogado = await _usuarioRepositorio.PegarUsuarioLogado(User);
            if (usuarioLogado.Id != null)
            {
                _logger.LogInformation("Buscando usuário atraves do id");
                var usuario = await _contexto.Usuarios.Where(u => u.Id == UsuarioId).Include(u => u.Setor).FirstOrDefaultAsync();
                var nivelAcesso = (from n in _contexto.NiveisAcessos
                                   join ur in _contexto.UserRoles
                                   on n.Id equals ur.RoleId
                                   join u in _contexto.Usuarios
                                   on ur.UserId equals u.Id
                                   where u.Id == UsuarioId
                                   select n).Select(e => e.Name).First();
                var sobreUsuario = _contexto.SobreUsuarios.FirstOrDefault(u => u.UsuarioId == UsuarioId);
                UserInformationViewModel user = new UserInformationViewModel();
                user.Id = usuario.Id;
                user.Nome = usuario.Nome;
                user.CPF = usuario.CPF;
                user.Email = usuario.Email;
                user.UserName = usuario.UserName;
                user.DataNascimento = usuario.DataNascimento;
                user.NomeSetor = usuario.Setor.Descricao;
                user.NivelAcesso = nivelAcesso;
                user.Telefone = usuario.Telefone;
                user.Numero = usuario.Numero;

                if (sobreUsuario != null)
                {
                    user.Formacao = sobreUsuario.Formacao;
                    user.Foto = sobreUsuario.Foto;
                    user.SobreUsuarioId = sobreUsuario.SobreUsuarioId;
                    if (sobreUsuario.CepId != null)
                    {
                        var cep = _contexto.Cep.FirstOrDefault(c => c.CepId == sobreUsuario.CepId);
                        user.CEP = cep.CEP;
                        user.Logradouro = cep.Logradouro;
                        user.Bairro = cep.Bairro;
                        user.Cidade = cep.Cidade;
                        user.UF = cep.UF;
                        user.CepId = cep.CepId;
                    }
                }

                return View(user);
            }
            _logger.LogError("Usuário diferente do logado no computador");
            return RedirectToAction("Login", "Usuarios");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PerfilUsuario(UserInformationViewModel user, IFormFile FotoLocal, int? CEPBD)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Verificando se o perfil do usuário ja existe");
                if (user.SobreUsuarioId == 0)
                {
                    _logger.LogInformation("Cricando perfil do usuário");
                    SobreUsuario sobre = new SobreUsuario();
                    sobre.Formacao = user.Formacao;
                    sobre.UsuarioId = user.Id;
                    _logger.LogInformation("Buscando usuário para possiveis alteracoes");
                    var usuario = _contexto.Usuarios.FirstOrDefault(u => u.Id == user.Id);
                    if (FotoLocal != null)
                    {
                        _logger.LogInformation("Enviando foto");
                        _logger.LogInformation("Criando link da pasta");

                        var fileName = ContentDispositionHeaderValue.Parse(FotoLocal.ContentDisposition).FileName.Trim('"');
                        //Assigning Unique Filename (Guid)
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                        //Getting file Extension
                        var FileExtension = Path.GetExtension(fileName);

                        // concating  FileName + FileExtension
                        var newFileName = myUniqueFileName + FileExtension;
                        // Combines two strings into a path.
                        fileName = Path.Combine(_hostingEnvironment.WebRootPath, "Imagens") + $@"\{newFileName}";
                        // if you want to store path of folder in database


                        using (FileStream fs = System.IO.File.Create(fileName))
                        {
                            FotoLocal.CopyTo(fs);
                            fs.Flush();
                            sobre.Foto = "~/Imagens/" + newFileName;
                        }
                    }
                    _logger.LogInformation("validando cep");
                    if (CEPBD != null)
                    {
                        switch (CEPBD)
                        {

                            case null:
                                _logger.LogInformation("Adicionando novo endereço ao banco");
                                Cep ceps = new Cep();
                                ceps.Logradouro = user.Logradouro;
                                ceps.CEP = user.CEP;
                                ceps.Bairro = user.Bairro;
                                ceps.Cidade = user.Cidade;
                                ceps.UF = user.UF;

                                _contexto.Add(ceps);
                                await _contexto.SaveChangesAsync();
                                _logger.LogInformation("associando ao usuário");
                                sobre.CepId = ceps.CepId;
                                break;
                            default:
                                sobre.CepId = CEPBD;
                                break;

                        }
                    }
                    _logger.LogInformation("Validando se o usuário vai alterar a senha");
                    if (user.PasswordHash != null)
                    {
                        _logger.LogInformation("Criptografando senha do usuário");
                        var senhaCriptografada = Criptografia.Codifica(user.PasswordHash);
                        usuario.PasswordHash = senhaCriptografada;

                        _contexto.Update(usuario);
                        await _contexto.SaveChangesAsync();
                        _logger.LogInformation("Atualizando senha do usuário");
                    }
                    //Atualizando informações complementares do usuário
                    var infoComplementares = _contexto.Usuarios.Where(u => u.Id == user.Id).FirstOrDefault();
                    infoComplementares.Telefone = user.Telefone;
                    infoComplementares.Email = user.Email;
                    infoComplementares.NormalizedEmail = user.Email.ToUpper();
                    infoComplementares.Numero = user.Numero;
                    _contexto.Update(infoComplementares);
                    await _contexto.SaveChangesAsync();
                    //fim da atualização complementar;

                    _logger.LogInformation("Adicionando Informações complementares do usuario");
                    _contexto.SobreUsuarios.Add(sobre);
                    await _contexto.SaveChangesAsync();
                    TempData["Mensagem"] = "Atualizado com sucesso";
                    return RedirectToAction("PerfilUsuario", new { UsuarioId = user.Id });
                }
                else
                {
                    _logger.LogInformation("Usuário já possui perfil, atualizando então");
                    var sobreUsuario = _contexto.SobreUsuarios.FirstOrDefault(s => s.SobreUsuarioId == user.SobreUsuarioId);
                    _logger.LogInformation("procurando perfil do usuário para atualização");
                    sobreUsuario.Formacao = user.Formacao;
                    _logger.LogInformation("Validando CEP");
                    switch (CEPBD)
                    {

                        case null:
                            _logger.LogInformation("Adicionando novo endereço ao banco");
                            Cep ceps = new Cep();
                            ceps.Logradouro = user.Logradouro;
                            ceps.CEP = user.CEP;
                            ceps.Bairro = user.Bairro;
                            ceps.Cidade = user.Cidade;
                            ceps.UF = user.UF;

                            _contexto.Add(ceps);
                            await _contexto.SaveChangesAsync();
                            _logger.LogInformation("associando ao convênio");
                            sobreUsuario.CepId = ceps.CepId;
                            break;
                        default:
                            sobreUsuario.CepId = CEPBD;
                            break;

                    }
                    if (FotoLocal != null)
                    {
                        if (sobreUsuario.Foto != null)
                        {
                            _logger.LogInformation("Excluindo foto antiga do servidor e adicionando nova");
                            string FotoUsuario = sobreUsuario.Foto;
                            FotoUsuario = FotoUsuario.Replace("~", "wwwroot");
                            System.IO.File.Delete(FotoUsuario);
                        }

                        _logger.LogInformation("Criando link da pasta");

                        var fileName = ContentDispositionHeaderValue.Parse(FotoLocal.ContentDisposition).FileName.Trim('"');
                        //Assigning Unique Filename (Guid)
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                        //Getting file Extension
                        var FileExtension = Path.GetExtension(fileName);

                        // concating  FileName + FileExtension
                        var newFileName = myUniqueFileName + FileExtension;
                        // Combines two strings into a path.
                        fileName = Path.Combine(_hostingEnvironment.WebRootPath, "Imagens/Usuarios") + $@"\{newFileName}";
                        // if you want to store path of folder in database


                        using (FileStream fs = System.IO.File.Create(fileName))
                        {
                            FotoLocal.CopyTo(fs);
                            fs.Flush();
                            sobreUsuario.Foto = "~/Imagens/Usuarios/" + newFileName;
                        }
                    }
                    if (user.PasswordHash != null)
                    {
                        _logger.LogInformation("Alterando senha do usuário");
                        var usuarioSenha = _contexto.Usuarios.FirstOrDefault(u => u.Id == user.Id);
                        var senhaCriptografada = Criptografia.Codifica(user.PasswordHash);
                        usuarioSenha.PasswordHash = senhaCriptografada;
                        _contexto.Update(usuarioSenha);
                        await _contexto.SaveChangesAsync();
                    }
                    //Atualizando informações complementares do usuário
                    var infoComplementares = _contexto.Usuarios.Where(u => u.Id == user.Id).FirstOrDefault();
                    infoComplementares.Telefone = user.Telefone;
                    infoComplementares.Email = user.Email;
                    infoComplementares.NormalizedEmail = user.Email.ToUpper();
                    infoComplementares.Numero = user.Numero;
                    _contexto.Update(infoComplementares);
                    await _contexto.SaveChangesAsync();
                    //fim da atualização complementar;
                    _logger.LogInformation("Atualizando perfil do usuário");
                    _contexto.SobreUsuarios.Update(sobreUsuario);
                    await _contexto.SaveChangesAsync();
                    TempData["Mensagem"] = "Atualizado com sucesso";
                    return RedirectToAction("PerfilUsuario", new { UsuarioId = user.Id });
                }
            }
            _logger.LogError("informações digitadas inválidas");
            return View(user);
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            _logger.LogInformation("Retornando pagina de bloqueio.");
            return View();
        }

        public async Task<JsonResult> UsuarioExisteNome(string Nome, string DataNascimento, string UsuarioId)
        {
            if (UsuarioId == null)
            {
                var dt = Convert.ToDateTime(DataNascimento);
                if (await _usuarioRepositorio.UsuarioExisteNome(Nome, dt))
                {
                    var usuario = _contexto.Usuarios.Where(u => u.Nome.ToUpper() == Nome.ToUpper() && u.DataNascimento == dt).FirstOrDefault();
                    return Json(usuario);
                }
                return Json(true);
            }
            var dt_edit = Convert.ToDateTime(DataNascimento);
            if (await _usuarioRepositorio.UsuarioExisteNome(Nome, dt_edit, UsuarioId))
            {
                var usuario = _contexto.Usuarios.Where(u => u.Nome.ToUpper() == Nome.ToUpper() && u.DataNascimento == dt_edit).FirstOrDefault();
                return Json(usuario);
            }
            return Json(true);
        }

        public async Task<JsonResult> UsuarioExisteCPF(string CPF, string UsuarioId)
        {
            if (UsuarioId == null)
            {

                if (await _usuarioRepositorio.UsuarioExisteCPF(CPF))
                {
                    var usuario = _contexto.Usuarios.Where(u => u.CPF == CPF).FirstOrDefault();
                    return Json(usuario);
                }
                return Json(true);
            }
            if (await _usuarioRepositorio.UsuarioExisteCPF(CPF, UsuarioId))
            {
                var usuario = _contexto.Usuarios.Where(u => u.CPF == CPF && u.Id == UsuarioId).FirstOrDefault();
                return Json(usuario);
            }
            return Json(true);
        }

        public async Task<JsonResult> UsuarioExisteUserName(string UserName, string UsuarioId)
        {
            if (UsuarioId == null)
            {

                if (await _usuarioRepositorio.UsuarioExisteUserName(UserName))
                {
                    var usuario = _contexto.Usuarios.Where(u => u.UserName == UserName).FirstOrDefault();
                    return Json(usuario);
                }
                return Json(true);
            }
            if (await _usuarioRepositorio.UsuarioExisteUserName(UserName, UsuarioId))
            {
                var usuario = _contexto.Usuarios.Where(u => u.UserName == UserName && u.Id == UsuarioId).FirstOrDefault();
                return Json(usuario);
            }
            return Json(true);
        }
        public async Task<IActionResult> EmpresaUsuario(string UsuarioId)
        {
            if (UsuarioId == null)
            {
                _logger.LogError("Erro, id do usuário não encontrado");
                return NotFound();
            }
            var empresa = await _contexto.UsuariosEmpresas.Where(e => e.UsuarioId == UsuarioId).Include(e => e.Empresa).Include(e => e.Usuario).OrderBy(e => e.Empresa.NomeFantasia).ToListAsync();
            if (empresa != null)
            {
                ViewData["UsuarioId"] = UsuarioId;
                _logger.LogInformation("Retornando lista de empresa cadastrada para o usuário de forma ordenada");
                return View(empresa);
            }
            _logger.LogInformation("Retornando view sem lista de empresas cadastras");
            ViewData["UsuarioId"] = UsuarioId;
            return View();
        }
        public IActionResult AddEmpresaUsuario(string UsuarioId)
        {
            ViewData["EmpresaId"] = new SelectList(_contexto.Empresas.OrderBy(e => e.NomeFantasia), "EmpresaId", "NomeFantasia");
            ViewData["UsuarioId"] = UsuarioId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmpresaUsuario(string UsuarioId, int EmpresaId)
        {
            _logger.LogInformation("Associando usuário a uma ou mais empresa");
            if (ModelState.IsValid)
            {
                if (await EmpresaUsuarioExiste(UsuarioId, EmpresaId) != true)
                {
                    UsuarioEmpresa empresa = new UsuarioEmpresa();
                    empresa.EmpresaId = EmpresaId;
                    empresa.UsuarioId = UsuarioId;
                    _contexto.Add(empresa);
                    await _contexto.SaveChangesAsync();
                    TempData["Mensagem"] = "Adicionado com sucesso";
                    return RedirectToAction("Index", "Usuarios");
                }
                else
                {
                    TempData["Validacao"] = "Empresa ja cadastrada para o usuario";
                    return RedirectToAction("Index", "Usuarios");
                }
            }
            ViewData["EmpresaId"] = new SelectList(_contexto.Empresas.OrderBy(e => e.NomeFantasia), "EmpresaId", "NomeFantasia");
            ViewData["UsuarioId"] = UsuarioId;
            TempData["Validacao"] = "Erro ao adicionar empresa ao usuario, verifique as informações digitadas";
            return View();
        }
        public async Task<bool> EmpresaUsuarioExiste(string UsuarioId, int EmpresaId)
        {
            return await _contexto.UsuariosEmpresas.AnyAsync(u => u.UsuarioId == UsuarioId && u.EmpresaId == EmpresaId);
        }
        public async Task<JsonResult> RemoveEmpresaUsuario(int EmpresaId, string UsuarioId)
        {
            var Empresa = _contexto.UsuariosEmpresas.Where(a => a.EmpresaId == EmpresaId && a.UsuarioId == UsuarioId).FirstOrDefault();
            _contexto.Remove(Empresa);
            await _contexto.SaveChangesAsync();
            TempData["Mensagem"] = "Removido com sucesso";
            return Json("Permissão Removido");
        }
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");


        }
    }
    }