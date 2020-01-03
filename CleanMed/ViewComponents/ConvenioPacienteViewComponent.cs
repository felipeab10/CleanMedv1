using CleanMed.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.ViewComponents
{
    public class ConvenioPacienteViewComponent:ViewComponent
    {
        private readonly Contexto _contexto;
        public ConvenioPacienteViewComponent(Contexto contexto)
        {
            _contexto = contexto;
        }
        public async Task<IViewComponentResult> InvokeAsync(int PacienteId)
        {
            var paciente = await _contexto.CartaoConvenios.Include(a => a.Paciente).Include(c => c.Convenio).Where(c => c.PacienteId == PacienteId).ToListAsync();
            if(paciente.Count >= 1)
            {
                ViewData["ConvenioId"] = new SelectList(_contexto.Convenios,"ConvenioId","Nome",paciente.Select(a=> a.ConvenioId).First());
                ViewData["PacienteId"] = PacienteId;
               
                ViewData["StatusId"] = new SelectList(new[] {
                        new{Name = "true",ID="Ativo"},
                        new{Name = "false",ID="Inativo"},

                }, "Name", "ID", paciente.Select(a => a.Status).First());
                return View(paciente);
            }
            ViewData["ConvenioId"] = new SelectList(_contexto.Convenios, "ConvenioId", "Nome");
           
            ViewData["StatusId"] = new SelectList(new[] {
                        new{Name = "true",ID="Ativo"},
                        new{Name = "false",ID="Inativo"},

                }, "Name", "ID");
            ViewData["PacienteId"] = PacienteId;
            return View();
        }
    }
}
