using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class Convenio
    {
        public int ConvenioId { get; set; }
        [Required(ErrorMessage ="Campo obrigatório")]
        [MaxLength(160,ErrorMessage ="Maximo 160 caracteres")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(160, ErrorMessage = "Maximo 160 caracteres")]
        public string RazaoSocial { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(160, ErrorMessage = "Maximo 160 caracteres")]
        public string CNPJ { get; set; }
        public string InscricaoMunicipal { get; set; }
        public string InscricaoEstadual { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(160, ErrorMessage = "Maximo 160 caracteres")]
        public string RegistroAns { get; set; }
        public bool Status { get; set; }
        public DateTime DtCadastro { get; set; } = DateTime.Now;
        public DateTime DtAlteracao { get; set; } = DateTime.Now;
        public int? CepId { get; set; }
        public Cep Cep { get; set; }
        public string Numero { get; set; }
        public string Logo { get; set; }
      
        public CartaoConvenio cartaoConvenio { get; set; }
        public ICollection<ConvenioAgendaMedica> ConveniosAgendasMedica { get; set; }
        public int id
        {
            get { return ConvenioId; }
        }
        public string text
        {
            get { return Nome; }
            // get { return Nome + "-" + DataNascimento.ToShortDateString() + " CPF-"+ CPF; }
        }
    }
}
