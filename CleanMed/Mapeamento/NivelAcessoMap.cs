using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class NivelAcessoMap : IEntityTypeConfiguration<NiveisAcesso>
    {
        public void Configure(EntityTypeBuilder<NiveisAcesso> builder)
        {
            builder.Property(n => n.Descricao).IsRequired();
            builder.ToTable("NiveisAcessos");
        }
    }
}
