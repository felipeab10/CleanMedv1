using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.ViewModels
{
    public class UserInformationViewModel
    {
        public  string Id { get; set; }
        public string Nome { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataNascimento { get; set; }
        public string CPF { get; set; }
        public  string UserName { get; set; }
        public  string Email { get; set; }
        public bool Status { get; set; }
        public string NomeSetor { get; set; }
        public string Telefone { get; set; }
        public string NivelAcesso { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DtCadastro { get; set; }
        public string Formacao { get; set; }
        public string Foto { get; set; }
        public int CepId { get; set; }
        public string PasswordHash { get; set; }
        public int SobreUsuarioId { get; set; }
        public string Logradouro { get; set; }
      
        public string Bairro { get; set; }
       
        public string Cidade { get; set; }
        
        public string UF { get; set; }
       
        public string CEP { get; set; }
        public string Numero { get; set; }

    }
}
