using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class SobreUsuarioMap : IEntityTypeConfiguration<SobreUsuario>
    {
        public void Configure(EntityTypeBuilder<SobreUsuario> builder)
        {
            builder.HasKey(s => s.SobreUsuarioId);
            builder.Property(s => s.Formacao);
            builder.Property(s => s.Foto);

            builder.HasOne(s => s.Usuario);
            builder.ToTable("SobreUsuarios");
        }
    }
}
