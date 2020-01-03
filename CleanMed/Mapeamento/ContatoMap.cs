using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class ContatoMap : IEntityTypeConfiguration<Contato>
    {
        public void Configure(EntityTypeBuilder<Contato> builder)
        {
            builder.HasKey(c => c.ContatoId);
            builder.Property(c => c.Nome).HasMaxLength(160);
            builder.Property(c => c.Parentesco).HasMaxLength(160);
            builder.Property(c => c.Telefone1).HasMaxLength(160);
            builder.Property(c => c.Email).HasMaxLength(160);
            builder.Property(c => c.Telefone2).HasMaxLength(160);
           

            builder.HasOne(c => c.Paciente);
            builder.ToTable("Contatos");

        }
    }
}
