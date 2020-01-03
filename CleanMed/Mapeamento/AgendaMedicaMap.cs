using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class AgendaMedicaMap : IEntityTypeConfiguration<AgendaMedica>
    {
        public void Configure(EntityTypeBuilder<AgendaMedica> builder)
        {
            builder.HasKey(a => a.AgendaMedicaId);
            builder.Property(a => a.DataAgenda);
            builder.Property(a => a.HoraInicio);
            builder.Property(a => a.HoraFim);
            builder.Property(a => a.StatusAgenda);
            builder.Property(a => a.DataLiberacao);
            builder.Property(a => a.DtCadastro);
            builder.Property(a => a.QtAtendimento);
            builder.Property(a => a.QtEncaixe);
            builder.Property(a => a.Observacao).IsRequired(false);
            builder.Property(a => a.ThemeColor).IsRequired(false);
            builder.Property(a => a.QtTempoMedio);
            builder.Property(a => a.TipoAgenda).IsRequired();

            builder.HasOne(a => a.RecursoAgendamento);
            builder.HasOne(a => a.Prestador);
            builder.HasOne(a => a.Empresa);
            builder.HasOne(a => a.Usuario);
            builder.HasOne(a => a.Setor);
            builder.ToTable("AgendasMedicas");

        }
    }
}
