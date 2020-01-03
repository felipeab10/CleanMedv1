using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class AgendamentoMap : IEntityTypeConfiguration<Agendamento>
    {
        public void Configure(EntityTypeBuilder<Agendamento> builder)
        {
            builder.HasKey(a => a.AgendamentoId);
            builder.Property(a => a.HoraAgenda).IsRequired();
            builder.Property(a => a.VlAltura);
            builder.Property(a => a.Qtpeso);
            builder.Property(a => a.StatusAgendamento).IsRequired();
            builder.Property(a => a.SNEncaixe).IsRequired(false);
            builder.Property(a => a.AtendimentoId).IsRequired(false);
            builder.Property(a => a.UsuarioId).IsRequired();
            builder.Property(a => a.Bloqueado).IsRequired(true);
            builder.Property(a => a.Color);
            builder.Property(a => a.ObservacaoAgendamento);
            builder.HasOne(a => a.AgendaMedica);
            builder.HasOne(a => a.Paciente);
            builder.HasOne(a => a.ItemAgendamento);
            builder.HasOne(a => a.Usuario);
            builder.HasOne(a => a.Convenio);
            builder.HasOne(a => a.MotivoCancelamento);


            builder.ToTable("Agendamentos");
        }
    }
}
