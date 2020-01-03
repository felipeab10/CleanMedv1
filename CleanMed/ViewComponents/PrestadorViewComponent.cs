using CleanMed.Data;
using CleanMed.Models;
using CleanMed.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.ViewComponents
{
    public class PrestadorViewComponent:ViewComponent
    {
        private readonly Contexto _contexto;
        public PrestadorViewComponent(Contexto contexto)
        {
            _contexto = contexto;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(int PrestadorId)
        {
            var prestador = await _contexto.Prestadores.Include(a => a.Cep).FirstOrDefaultAsync(a=> a.PrestadorId == PrestadorId);
            PrestadorViewModel prestadorView = new PrestadorViewModel();
            prestadorView.Nome = prestador.Nome;
            prestadorView.PrestadorId = prestador.PrestadorId;
            prestadorView.DataNascimento = prestador.DataNascimento;
            prestadorView.CPF = prestador.CPF;
            prestadorView.ConselhoId = prestador.ConselhoId;
            prestadorView.NumeroCrm = prestador.NumeroCrm;
            prestadorView.TipoPrestadorId = prestador.TipoPrestadorId;
            prestadorView.Telefone = prestador.Telefone;
            prestadorView.Email = prestador.Email;
            prestadorView.Status = prestador.Status;
            prestadorView.Sexo = prestador.Sexo;
            prestadorView.Numero = prestador.Numero;
            if(prestador.CepId != null)
            {
                var cep = await _contexto.Cep.FirstOrDefaultAsync(a => a.CepId == prestador.CepId);
                prestadorView.CepId = cep.CepId;
                prestadorView.CEP = cep.CEP;
                prestadorView.Logradouro = cep.Logradouro;
                prestadorView.Bairro = cep.Bairro;
                prestadorView.Cidade = cep.Cidade;
                prestadorView.UF = cep.UF;
                prestadorView.Complemento = cep.Complemento;
            }

            ViewData["ConselhoId"] = new SelectList(_contexto.Conselhos, "ConselhoId", "Descricao");
            ViewData["TipoPrestadorId"] = new SelectList(_contexto.TipoPrestadores,"TipoPrestadorId","Descricao");
            ViewData["PrestadorId"] = PrestadorId;
            ViewData["StatusId"] = new SelectList(new[] { 
            
                new {Name = "true",ID="Ativo"},
                new {Name = "false",ID="Inativo"},
            }, "Name", "ID");
            ViewData["SexoId"] = new SelectList(new[] {

                new {Name = "Masculino",ID="Masculino"},
                new {Name = "Feminino",ID="Feminino"},
                new {Name = "Indefinido",ID="Indefinido"},
            }, "Name", "ID",prestador.Sexo);

            return View(prestadorView);


      }
     
    }
}
