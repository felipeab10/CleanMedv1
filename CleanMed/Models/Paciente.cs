using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class Paciente
    {
        [Display(Name = "Id")]
        public int PacienteId { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [MaxLength(160,ErrorMessage ="Campo Maximo de 160 caracteres")]
        public string Nome { get; set; }
       
        [Required(ErrorMessage = "Campo Obrigatório")]
       
        [Display(Name = "Data de Nascimento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Remote("PacienteExisteNome", "Pacientes", AdditionalFields = "Nome")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [Remote("PacienteExisteCPF", "Pacientes")]
        public string CPF { get; set; }
        public bool SemCPF { get; set; }
        public string Sexo { get; set; }
        public string Estado_Civil { get; set; }
        public string Email { get; set; }
        public string RG { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Telefone { get; set; }
        public DateTime dt_cadastro { get; set; }
        public bool StatusId { get; set; }
        public int? CepId { get; set; }
        public Cep Cep { get; set; }
        public string Numero { get; set; }
       
        public CartaoConvenio cartaoConvenio { get; set; }

        public int id
        {
            get { return PacienteId; }
        }
        public string text
        {
           get { return Nome; }
            // get { return Nome + "-" + DataNascimento.ToShortDateString() + " CPF-"+ CPF; }
        }
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
