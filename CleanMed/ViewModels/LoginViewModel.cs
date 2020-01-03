using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Campo Obrigatório")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        [DataType(DataType.Password,ErrorMessage ="Senha inválida")]
        public string PasswordHash { get; set; }
    }
}
