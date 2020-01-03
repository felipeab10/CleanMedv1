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
    public class ContatoViewComponent:ViewComponent
    {
        private readonly Contexto _contexto;
        public ContatoViewComponent(Contexto contexto)
        {
            _contexto = contexto;
        }
        public async Task<IViewComponentResult> InvokeAsync(int PacienteId)
        {
            var contatoExiste = await _contexto.Contatos.Where(c => c.PacienteId == PacienteId).ToListAsync();
            if(contatoExiste.Count >= 1)
            {
               
                ViewData["PacienteId"] = PacienteId;
                ViewData["ParentescoId"] = new SelectList(new[] {

                new {Name = "PAI",ID= "PAI" },
                new {Name = "AMIGO (A)",ID= "AMIGO (A)" },
                new {Name = "AVO (A)",ID= "AVO (A)" },
                new {Name = "CUNHADO (A)",ID= "CUNHADO (A)" },
                new {Name = "ESPOSO (A)",ID= "ESPOSO (A)" },
                new {Name = "FILHO (A)",ID= "FILHO (A)" },
                new {Name = "GENRO",ID= "GENRO" },
                new {Name = "IRMA",ID= "IRMA" },
                new {Name = "IRMAO",ID= "IRMAO" },
                new {Name = "MAE",ID= "MAE" },
                new {Name = "NORA",ID= "NORA" },
                new {Name = "PRIMO (A)",ID= "PRIMO (A)" },
                new {Name = "SOBRINHO (A)",ID= "SOBRINHO (A)" },
                new {Name = "SOGRO (A)",ID= "SOGRO (A)" },
                new {Name = "TIO (A)",ID= "TIO (A)" },
                new {Name = "COMPANHEIRO (A)",ID= "COMPANHEIRO (A)" },
                new {Name = "O MESMO",ID= "O MESMO" },
                new {Name = "NETO (A)",ID= "NETO (A)" },
                new {Name = "FUNCIONARIO (A)",ID= "FUNCIONARIO (A)" },
                new {Name = "NAMORADO (A)",ID= "NAMORADO (A)" },


            }, "Name", "ID", contatoExiste.Select(a => a.Parentesco).First());
                return View(contatoExiste);
            }
        
           ViewData["ParentescoId"] = new SelectList(new[] {

                new {Name = "PAI",ID= "PAI" },
                new {Name = "AMIGO (A)",ID= "AMIGO (A)" },
                new {Name = "AVO (A)",ID= "AVO (A)" },
                new {Name = "CUNHADO (A)",ID= "CUNHADO (A)" },
                new {Name = "ESPOSO (A)",ID= "ESPOSO (A)" },
                new {Name = "FILHO (A)",ID= "FILHO (A)" },
                new {Name = "GENRO",ID= "GENRO" },
                new {Name = "IRMA",ID= "IRMA" },
                new {Name = "IRMAO",ID= "IRMAO" },
                new {Name = "MAE",ID= "MAE" },
                new {Name = "NORA",ID= "NORA" },
                new {Name = "PRIMO (A)",ID= "PRIMO (A)" },
                new {Name = "SOBRINHO (A)",ID= "SOBRINHO (A)" },
                new {Name = "SOGRO (A)",ID= "SOGRO (A)" },
                new {Name = "TIO (A)",ID= "TIO (A)" },
                new {Name = "COMPANHEIRO (A)",ID= "COMPANHEIRO (A)" },
                new {Name = "O MESMO",ID= "O MESMO" },
                new {Name = "NETO (A)",ID= "NETO (A)" },
                new {Name = "FUNCIONARIO (A)",ID= "FUNCIONARIO (A)" },
                new {Name = "NAMORADO (A)",ID= "NAMORADO (A)" },

            }, "Name", "ID");
            ViewData["PacienteId"] = PacienteId;
           return View();
            
        }
    }
}
