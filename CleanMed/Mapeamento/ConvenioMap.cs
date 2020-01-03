using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class ConvenioMap : IEntityTypeConfiguration<Convenio>
    {
        public void Configure(EntityTypeBuilder<Convenio> builder)
        {
            builder.HasKey(a => a.ConvenioId);
            builder.Property(a => a.Nome);
            builder.Property(a => a.RazaoSocial);
            builder.Property(a => a.Email);
            builder.Property(a => a.Telefone);
            builder.HasIndex(a => a.CNPJ);
            builder.Property(a => a.CNPJ).IsRequired();
            builder.Property(a => a.InscricaoEstadual);
            builder.Property(a => a.InscricaoMunicipal);
            builder.Property(a => a.RegistroAns);
            builder.Property(a => a.CepId).IsRequired(false);
            builder.Property(a => a.Numero).IsRequired(false);
            builder.Property(a => a.Logo).IsRequired(false);

            builder.HasOne(a => a.Cep);
            builder.ToTable("Convenios");
        }
    }
}
