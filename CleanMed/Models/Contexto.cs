using System;
using System.Collections.Generic;
using System.Text;
using CleanMed.Mapeamento;
using CleanMed.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanMed.Data
{
    public class Contexto : IdentityDbContext<Usuario,NiveisAcesso, string>
    {
        public DbSet<Paciente> Pacientes { get; set; }
       
        public DbSet<Contato> Contatos { get; set; }
        public DbSet<Conselho> Conselhos { get; set; }
        public DbSet<Especialidade> Especialidades { get; set; }
        public DbSet<TipoPrestador> TipoPrestadores { get; set; }
        public DbSet<Cep> Cep { get; set; }
        public DbSet<Prestador> Prestadores { get; set; }
        public DbSet<PrestadorEspecialidade> PrestadoresEspecialidades { get; set; }
        public DbSet<Convenio> Convenios { get; set; }
        public DbSet<CartaoConvenio> CartaoConvenios { get; set; }
        public DbSet<Setor> Setores { get; set; }
        public DbSet<Exame> Exames { get; set; }
        public DbSet<GrupoFaturamento> GrupoFaturamentos { get; set; }
        public DbSet<Procedimento> Procedimentos { get; set; }
        public DbSet<TabelaFaturamento> TabelaFaturamentos { get; set; }
        public DbSet<TabelaFatuProcedimento> TabelaFatuProcedimentos { get; set; }
        public DbSet<ItemAgendamento> ItemAgendamentos { get; set; }
        public DbSet<ItemAgendamentoPrestador> ItemAgendamentoPrestadores { get; set; }
        public DbSet<RecursoAgendamento> RecursoAgendamentos { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<NiveisAcesso> NiveisAcessos { get; set; }
        public DbSet<UsuarioEmpresa> UsuariosEmpresas { get; set; }
        public DbSet<SobreUsuario> SobreUsuarios { get; set; }
        public DbSet<AgendaMedica> AgendasMedicas { get; set; }
        public DbSet<ItemAgendaMedica> ItensAgendasMedica { get; set; }
        public DbSet<ConvenioAgendaMedica> ConveniosAgendasMedica { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }
        public DbSet<AgendamentoLog> AgendamentoLogs { get; set; }
        public DbSet<MotivoCancelamento> MotivoCancelamentos { get; set; }


        public Contexto(DbContextOptions<Contexto> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
           
            builder.ApplyConfiguration(new PacienteMap());
            builder.ApplyConfiguration(new ContatoMap());
            builder.ApplyConfiguration(new ConselhoMap());
            builder.ApplyConfiguration(new EspecialidadeMap());
            builder.ApplyConfiguration(new TipoPrestadorMap());
            builder.ApplyConfiguration(new CepMap());
            builder.ApplyConfiguration(new PrestadorMap());
            builder.ApplyConfiguration(new PrestadorEspecialidadeMap());
            builder.ApplyConfiguration(new ConvenioMap());
            builder.ApplyConfiguration(new CartaoConvenioMap());
            builder.ApplyConfiguration(new ExameMap());
            builder.ApplyConfiguration(new SetorMap());
            builder.ApplyConfiguration(new GrupoFaturamentoMap());
            builder.ApplyConfiguration(new ProcedimentoMap());
            builder.ApplyConfiguration(new TabelaFaturamentoMap());
            builder.ApplyConfiguration(new TabelaFatuProcedimentoMap());
            builder.ApplyConfiguration(new ItemAgendamentoMap());
            builder.ApplyConfiguration(new ItemAgendamentoPrestadorMap());
            builder.ApplyConfiguration(new RecursoAgendamentoMap());
            builder.ApplyConfiguration(new EmpresaMap());
            builder.ApplyConfiguration(new UsuarioMap());
            builder.ApplyConfiguration(new NivelAcessoMap());
            builder.ApplyConfiguration(new UsuarioEmpresaMap());
            builder.ApplyConfiguration(new SobreUsuarioMap());
            builder.ApplyConfiguration(new AgendaMedicaMap());
            builder.ApplyConfiguration(new ItemAgendaMedicaMap());
            builder.ApplyConfiguration(new ConvenioAgendaMedicaMap());
            builder.ApplyConfiguration(new AgendamentoMap());
            builder.ApplyConfiguration(new AgendamentoLogMap());
            builder.ApplyConfiguration(new MotivoCancelamentoMap());
        




        }

    }
}
