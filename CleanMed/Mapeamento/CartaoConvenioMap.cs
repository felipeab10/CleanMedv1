using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class CartaoConvenioMap : IEntityTypeConfiguration<CartaoConvenio>
    {
        public void Configure(EntityTypeBuilder<CartaoConvenio> builder)
        {
            builder.HasKey(c => c.CartaoConvenioId);
            builder.Property(c => c.NumeroCarteira).IsRequired().HasMaxLength(30);
            builder.Property(c => c.Status);
            builder.Property(c => c.Validade).HasColumnType("date");

            builder.HasMany(c => c.Pacientes).WithOne(c => c.cartaoConvenio);
            builder.HasMany(c => c.Convenios).WithOne(c => c.cartaoConvenio);

            builder.ToTable("CartaoConvenios");
        }
    }
}
