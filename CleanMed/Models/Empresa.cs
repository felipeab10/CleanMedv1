using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class Empresa
    {
        public int EmpresaId { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string NomeFantasia { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string RazaoSocial { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
       // [Remote("EmpresaExisteCNPJ","Empresas",AdditionalFields ="EmpresaId")]
        public string CNPJ { get; set; }
        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipal { get; set; }
        public string CNES { get; set; }
        public int? CepId { get; set; }
        public Cep Cep { get; set; }
        public string Numero { get; set; }
        public string Telefone { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public bool Unidade { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public bool Status { get; set; }
        public string Logo { get; set; }
        public DateTime DtCadastro { get; set; } = DateTime.Now;
        
        public ICollection<UsuarioEmpresa> UsuariosEmpresas { get; set; }
    }
}
