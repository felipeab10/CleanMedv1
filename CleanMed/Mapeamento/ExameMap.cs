using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class ExameMap : IEntityTypeConfiguration<Exame>
    {
        public void Configure(EntityTypeBuilder<Exame> builder)
        {
            builder.HasKey(e => e.ExameId);
            builder.Property(e => e.Descricao).IsRequired();
            builder.Property(e => e.DiaEntrega).IsRequired(false);
            builder.Property(e => e.Mnemonico);
            builder.Property(e => e.Sexo);
            builder.Property(e => e.Status);
            
            builder.Property(e => e.PreparoExame).HasColumnType("text");

            builder.HasOne(e => e.Especialidade);
            builder.HasOne(e => e.Setor);

            builder.ToTable("Exames");
        }
    }
}
