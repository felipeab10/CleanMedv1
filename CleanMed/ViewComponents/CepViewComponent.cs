using CleanMed.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.ViewComponents
{
    public class CepViewComponent:ViewComponent
    {
        private readonly Contexto _contexto;
        public CepViewComponent(Contexto contexto)
        {
            _contexto = contexto;
        }
        public async Task<IViewComponentResult> InvokeAsync(int PrestadorId)
        {
            return View();
        }

    }
}
