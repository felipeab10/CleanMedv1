using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class TabelaFaturamentoMap : IEntityTypeConfiguration<TabelaFaturamento>
    {
        public void Configure(EntityTypeBuilder<TabelaFaturamento> builder)
        {
            builder.HasKey(a => a.TabelaFaturamentoId);
            builder.Property(a => a.Descricao).IsRequired();

            builder.HasOne(a => a.Convenio);
            builder.ToTable("TabelaFaturamentos");
        }
    }
}
