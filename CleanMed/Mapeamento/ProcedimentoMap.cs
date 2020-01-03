using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class ProcedimentoMap : IEntityTypeConfiguration<Procedimento>
    {
        public void Configure(EntityTypeBuilder<Procedimento> builder)
        {
            builder.HasKey(a => a.ProcedimentoId);
            builder.Property(a => a.Descricao).IsRequired();
            builder.Property(a => a.IdadeMaxima).IsRequired(false);
            builder.Property(a => a.IdadeMinima).IsRequired(false);
            builder.Property(a => a.Sexo).IsRequired();

            builder.HasOne(a => a.GrupoFaturamento);
            builder.ToTable("Procedimentos");
        }
    }
}
