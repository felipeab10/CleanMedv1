using CleanMed.Models;
using CleanMed.Servicos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CleanMed.ViewModels
{
    public class PacienteViewModel
    {
        [Display(Name = "Id")]
        public int PacienteId { get; set; }
        //[Remote("PacienteExisteNome","Pacientes",AdditionalFields ="PacienteId")]
        [Required(ErrorMessage ="Campo Obrigatório")]
        public string Nome { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date,ErrorMessage ="informe uma data válida")]
        [Remote("ValidaDataNascimento","Pacientes")]
        public DateTime DataNascimento { get; set; }


        //[Remote("PacienteExisteCPF","Pacientes",AdditionalFields ="PacienteId")]
        [CustomValidationCPF(ErrorMessage = "CPF inválido")]
        public string CPF { get; set; }
        public bool SemCPF { get; set; } 
        public string Sexo { get; set; }
        public string Estado_Civil { get; set; }
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Informe um email válido...")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Informe um e-mail válido")]
        public string Email { get; set; }
        public string RG { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Informe um telefone válido")]
        public string Telefone { get; set; }
        public bool StatusId { get; set; }
        public DateTime dt_cadastro { get; set; }
        public string Numero { get; set; }
        public IEnumerable<Contato> Contatos { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(200, ErrorMessage = "Maximo de 200 Caracteres")]
        public string Logradouro { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(200, ErrorMessage = "Maximo de 200 Caracteres")]
        public string Bairro { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(200, ErrorMessage = "Maximo de 200 Caracteres")]
        public string Cidade { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(200, ErrorMessage = "Maximo de 200 Caracteres")]
        public string UF { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(10, ErrorMessage = "Maximo de 10 Caracteres")]
        public string CEP { get; set; }

        public string Complemento { get; set; }
        public int CepId { get; set; }
        public string GenerateSlug()
        {
            string phrase = string.Format("{0}-{1}", PacienteId, Nome);

            string str = RemoveAccent(phrase).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        private string RemoveAccent(string text)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }

}
