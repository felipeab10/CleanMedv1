using CleanMed.Servicos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.ViewModels
{
    public class EmpresaViewModel
    {
        public int EmpresaId { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        [Display(Name = "Nome Fantasia")]
        public string NomeFantasia { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        [Display(Name = "Razão Social")]
        public string RazaoSocial { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        //[Remote("EmpresaExisteCNPJ", "Empresas", AdditionalFields = "EmpresaId")]
        [CustomValidationCNPJ(ErrorMessage ="'{0}' Inválido")]
        public string CNPJ { get; set; }
        [Display(Name = "Inscrição Estadual")]
        public string InscricaoEstadual { get; set; }
        [Display(Name = "Inscricão Municipal")]
        public string InscricaoMunicipal { get; set; }
        public string CNES { get; set; }
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
        [MaxLength(200, ErrorMessage = "Maximo de 200 Caracteres")]
        public string UF { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(10, ErrorMessage = "Maximo de 10 Caracteres")]
        public string CEP { get; set; }

        public string Complemento { get; set; }
        public string Numero { get; set; }
        public string Telefone { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public bool Unidade { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public bool Status { get; set; }

    }
}
