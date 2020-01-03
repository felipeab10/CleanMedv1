using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class GrupoFaturamentoMap : IEntityTypeConfiguration<GrupoFaturamento>
    {
        public void Configure(EntityTypeBuilder<GrupoFaturamento> builder)
        {
            builder.HasKey(a => a.GrupoFaturamentoId);
            builder.Property(a => a.Descricao).IsRequired();
            builder.Property(a => a.Tipo).IsRequired();
            builder.Property(a => a.Status);
            builder.ToTable("GrupoFaturamentos");
        }
    }
}
