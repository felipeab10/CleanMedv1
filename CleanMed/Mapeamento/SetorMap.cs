using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class SetorMap : IEntityTypeConfiguration<Setor>
    {
        public void Configure(EntityTypeBuilder<Setor> builder)
        {
            builder.HasKey(s => s.SetorId);
            builder.Property(s => s.Descricao).IsRequired().HasMaxLength(160);
            builder.Property(s => s.Status);
            builder.Property(s => s.TipoSetor);

            builder.ToTable("Setores");
        }

    }
}
