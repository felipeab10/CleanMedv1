using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class CepMap : IEntityTypeConfiguration<Cep>
    {
        public void Configure(EntityTypeBuilder<Cep> builder)
        {
            builder.HasKey(c => c.CepId);
            builder.Property(e => e.Logradouro).IsRequired().HasMaxLength(200);
            builder.Property(e => e.Bairro).IsRequired().HasMaxLength(200);
            builder.Property(e => e.UF).IsRequired().HasMaxLength(200);
            builder.Property(e => e.CEP).IsRequired().HasMaxLength(10);
            builder.Property(e => e.Complemento).HasMaxLength(200);
            builder.Property(e => e.Cidade).IsRequired().HasMaxLength(200);

            builder.ToTable("Cep");
        }
    }
}
