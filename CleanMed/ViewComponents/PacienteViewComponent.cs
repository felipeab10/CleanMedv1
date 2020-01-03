using CleanMed.Data;
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
    public class PacienteViewComponent:ViewComponent
    {
        private readonly Contexto _contexto;
        public PacienteViewComponent(Contexto contexto)
        {
            _contexto = contexto;
        }
        public async Task<IViewComponentResult> InvokeAsync(int PacienteId)
        {
            var paciente = await _contexto.Pacientes.FirstOrDefaultAsync(a => a.PacienteId == PacienteId);
            PacienteViewModel pacienteView = new PacienteViewModel();
            pacienteView.Nome = paciente.Nome;
            pacienteView.DataNascimento = paciente.DataNascimento;
            pacienteView.CPF = paciente.CPF;
            pacienteView.Sexo = paciente.Sexo;
            pacienteView.Estado_Civil = paciente.Estado_Civil;
            pacienteView.StatusId = paciente.StatusId;
            pacienteView.RG = paciente.RG;
            pacienteView.Telefone = paciente.Telefone;
            pacienteView.Email = paciente.Email;
            if(paciente.CepId != null)
            {
                var cep = _contexto.Cep.Where(a => a.CepId == paciente.CepId).FirstOrDefault();
                pacienteView.CepId = cep.CepId;
                pacienteView.CEP = cep.CEP;
                pacienteView.Logradouro = cep.Logradouro;
                pacienteView.Bairro = cep.Bairro;
                pacienteView.Cidade = cep.Cidade;
                pacienteView.UF = cep.UF;
                pacienteView.Complemento = cep.Complemento;
                pacienteView.Numero = paciente.Numero;
            }
            var listSexo = new SelectList(new[]
         {
                new {Name = "Masculino",ID="Masculino"},
                new {Name = "Feminino",ID="Feminino"},
                new {Name = "Indefinido",ID="Indefinido"},
            }, "Name", "ID");
            ViewData["SexoId"] = listSexo;
            var listEstadoCivil = new SelectList(new[]
           {
                new {Name = "Solteiro(a)",ID="Solteiro(a)"},
                new {Name = "Casado(a)",ID="Casado(a)"},
                new {Name = "Viúvo(a)",ID="Viúvo(a)"},
                new {Name = "Separado judicialmente",ID="Separado judicialmente"},
                new {Name = "Divorciado(a)",ID="Divorciado(a)"},
            }, "Name", "ID");
            ViewData["EstadoCivilId"] = listEstadoCivil;
            var listStatus = new SelectList(new[] {
                new{Name = "true",ID = "Ativo"},
                new{Name = "false",ID = "Inativo"}

            },"Name","ID");
            ViewData["StatusId"] = listStatus;
            return View(pacienteView);
        }
    }
}
