using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class TipoPrestadorMap : IEntityTypeConfiguration<TipoPrestador>
    {
        public void Configure(EntityTypeBuilder<TipoPrestador> builder)
        {
            builder.HasKey(c => c.TipoPrestadorId);
            builder.Property(c => c.Descricao).IsRequired().HasMaxLength(100);

            builder.ToTable("TipoPrestadores");
        }
    }
}
