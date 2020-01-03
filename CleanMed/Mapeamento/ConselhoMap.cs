using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class ConselhoMap : IEntityTypeConfiguration<Conselho>
    {
        public void Configure(EntityTypeBuilder<Conselho> builder)
        {
            builder.HasKey(c => c.ConselhoId);
            builder.Property(c => c.Descricao).IsRequired().HasMaxLength(100);

            builder.ToTable("Conselhos");
        }
    }
}
