using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class Usuario:IdentityUser
    {
        public override string Id { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        [Display(Name = "Data de Nascimento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataNascimento { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinLength(14,ErrorMessage ="CPF Inválido")]
        [MaxLength(14,ErrorMessage ="CPF Inválido")]
        public string CPF { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MaxLength(120)]
        [Display(Name = "Nome de Usuário")]
        public override string UserName { get; set; }
        
        [Display(Name = "Senha")]
        [DataType(DataType.Password,ErrorMessage ="Informe uma senha válida")]
        public override string PasswordHash { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public override string Email { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public bool Status { get; set; }
        public string Observacao { get; set; }
        [Display(Name = "Setor")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int? SetorId { get; set; }
        public Setor Setor { get; set; }
        public int? CepId { get; set; }
        public Cep Cep { get; set; }
        public string Numero { get; set; }
        public string Telefone { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtCadastro { get; set; } = DateTime.Now;
        public ICollection<UsuarioEmpresa> UsuariosEmpresas { get; set; }
    }
}
