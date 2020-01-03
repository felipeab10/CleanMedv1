using CleanMed.Data;
using CleanMed.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.ViewComponents
{
    public class EspecialidadeViewComponent:ViewComponent
    {
        private readonly Contexto _contexto;
        public EspecialidadeViewComponent(Contexto contexto)
        {
            _contexto = contexto;
        }
        public async Task<IViewComponentResult> InvokeAsync(int PrestadorId)
        {
            Especialidade especialidade = new Especialidade();
            var innerjoin = await (from e in _contexto.Especialidades
                                   join ep in _contexto.PrestadoresEspecialidades
                                   on e.EspecialidadeId equals ep.EspecialidadeId
                                   join p in _contexto.Prestadores
                                   on ep.PrestadorId equals p.PrestadorId
                                   where p.PrestadorId == PrestadorId
                                   select e).ToListAsync();
            ViewData["PrestadorId"] = PrestadorId;
            ViewData["EspecialidadeId"] = new SelectList(_contexto.Especialidades ,"EspecialidadeId","Descricao");
            return View(innerjoin);
        }
    }
}
