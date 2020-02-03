using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class TipoSetorMap : IEntityTypeConfiguration<TipoSetor>
    {
        public void Configure(EntityTypeBuilder<TipoSetor> builder)
        {
            builder.HasKey(t => t.TipoSetorId);
            builder.Property(t => t.Descricao).IsRequired();

            builder.ToTable("TipoSetores");
            builder.HasData(
               new TipoSetor { TipoSetorId = 1, Descricao = "Ambulatório" },
               new TipoSetor { TipoSetorId = 2, Descricao = "Administrativo" },
               new TipoSetor { TipoSetorId = 3, Descricao = "Exame" },
               new TipoSetor { TipoSetorId = 4, Descricao = "Externo" }


               );
        }
    }
}
