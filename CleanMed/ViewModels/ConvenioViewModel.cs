using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.ViewModels
{
    public class ConvenioViewModel
    {
        public int ConvenioId { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(160, ErrorMessage = "Maximo 160 caracteres")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(160, ErrorMessage = "Maximo 160 caracteres")]
        public string RazaoSocial { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(160, ErrorMessage = "Maximo 160 caracteres")]
        [Remote("ConvenioExiste","Convenios",AdditionalFields ="ConvenioId")]
        public string CNPJ { get; set; }
        public string InscricaoMunicipal { get; set; }
        public string InscricaoEstadual { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(160, ErrorMessage = "Maximo 160 caracteres")]
        [Remote("ConvenioExisteRegistroANS","Convenios",AdditionalFields ="ConvenioId")]
        public string RegistroAns { get; set; }
        public bool Status { get; set; }
        public DateTime DtCadastro { get; set; } = DateTime.Now;
        public DateTime DtAlteracao { get; set; } = DateTime.Now;
        [MaxLength(6)]
        [Range(1,int.MaxValue,ErrorMessage ="Número inválido")]
        public string Numero { get; set; }
        public string Logo { get; set; }
        public int CepId { get; set; }
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
        [MaxLength(2, ErrorMessage = "Maximo de 2 Caracteres")]
        public string UF { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(10, ErrorMessage = "Maximo de 10 Caracteres")]
        public string CEP { get; set; }

        public string Complemento { get; set; }
    }
}
