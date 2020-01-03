using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanMed.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanMed.Controllers
{
    [Authorize]
       public class CepController : Controller
    {
        private readonly Contexto _contexto;
        public CepController(Contexto contexto)
        {
            _contexto = contexto;
        }
        public JsonResult LocalizarCEP(string CEP)
        {
            var endereco = _contexto.Cep.FirstOrDefault(a => a.CEP == CEP);

            return Json(endereco);
        }
    }
}