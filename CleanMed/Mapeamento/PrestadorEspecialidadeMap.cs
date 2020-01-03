using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class PrestadorEspecialidadeMap : IEntityTypeConfiguration<PrestadorEspecialidade>
    {
        public void Configure(EntityTypeBuilder<PrestadorEspecialidade> builder)
        {
            builder.HasKey(e => new { e.PrestadorId, e.EspecialidadeId  });
        }
    }
}
