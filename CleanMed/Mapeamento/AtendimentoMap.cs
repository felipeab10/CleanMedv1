using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class AtendimentoMap : IEntityTypeConfiguration<Atendimento>
    {
        public void Configure(EntityTypeBuilder<Atendimento> builder)
        {
            builder.HasKey(a => a.AtendimentoId);
            builder.Property(a => a.DataAtendimento).IsRequired();
            builder.Property(a => a.HoraAtendimento).IsRequired();
            builder.Property(a => a.NrCarteira).IsRequired();
            builder.Property(a => a.SnEmAtendimento).IsRequired(false);
            builder.Property(a => a.SnAcompanhante).IsRequired(false);
            builder.Property(a => a.SnRetorno).IsRequired(false);
            builder.Property(a => a.PrioridadeAtendimento);

            builder.HasOne(a => a.Paciente);
            builder.HasOne(a => a.Setor);
            builder.HasOne(a => a.Convenio);
            builder.HasOne(a => a.Usuario);

            builder.ToTable("Atendimentos");

        }
    }
}
