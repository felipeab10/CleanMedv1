using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class TabelaFatuProcedimentoMap : IEntityTypeConfiguration<TabelaFatuProcedimento>
    {
        public void Configure(EntityTypeBuilder<TabelaFatuProcedimento> builder)
        {
            builder.HasKey(a => a.TabelaFatuProcedimentoId);
            builder.Property(a => a.DataVigencia).IsRequired().HasColumnType("date");
            builder.Property(a => a.Status);
            builder.Property(a => a.ValorTotal).IsRequired();

            builder.HasOne(a => a.Procedimento);
            builder.HasOne(a => a.TabelaFaturamento);

            builder.ToTable("TabelaFatuProcedimentos");
        }
    }
}
