using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class PrestadorMap : IEntityTypeConfiguration<Prestador>
    {
        public void Configure(EntityTypeBuilder<Prestador> builder)
        {
            builder.HasKey(c => c.PrestadorId);
            builder.Property(c => c.Nome).IsRequired().HasMaxLength(160);
            builder.HasIndex(c => c.NumeroCrm);
            builder.Property(c => c.NumeroCrm).IsRequired();
            builder.HasIndex(c => c.CPF);
            builder.Property(c => c.CPF).IsRequired().HasMaxLength(16);
            builder.Property(c => c.DataNascimento).HasColumnType("Date");
            builder.Property(c => c.Telefone).HasMaxLength(20);
            builder.Property(c => c.Numero).IsRequired(false).HasMaxLength(5);
            builder.HasOne(c => c.Conselho);
            builder.HasOne(c => c.TipoPrestador);
            builder.Property(c => c.Status);
            builder.Property(c => c.Email).IsRequired(false).HasMaxLength(160);
            builder.Property(c => c.CepId).IsRequired(false);
            builder.Property(c => c.Sexo);

            builder.HasOne(c => c.Cep);

            builder.ToTable("Prestadores");
        }
    }
}
