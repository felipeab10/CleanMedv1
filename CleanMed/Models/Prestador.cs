using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Models
{
    public class Prestador
    {
        public int PrestadorId { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        //[Remote("PrestadorExisteNome", "Prestadores", AdditionalFields = "PrestadorId")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string CPF { get; set; }
        public string Sexo { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        //[Remote("PrestadorExisteNumeroCrm","Prestadores",AdditionalFields ="PrestadorId")]
        public string NumeroCrm { get; set; }
        public bool Status { get; set; }
        public string Telefone { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int TipoPrestadorId { get; set; }
        public TipoPrestador TipoPrestador { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int ConselhoId { get; set; }
        public Conselho Conselho { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Valor inválido, não pode ser 0 ou valor negativo")]
        public string Numero { get; set; }
        [DataType(DataType.EmailAddress,ErrorMessage = "Email Inválido")]
        public string Email { get; set; }
        public ICollection<PrestadorEspecialidade> PrestadoresEspecialidades { get; set; }

        public int? CepId { get; set; }
        public Cep Cep { get; set; }
        public ICollection<ItemAgendamentoPrestador> ItemAgendamentoPrestadores { get; set; }

        public int id 
        {
            get { return PrestadorId; }
        }
        public string text
        {
            get { return Nome; }
        }

    }
}
