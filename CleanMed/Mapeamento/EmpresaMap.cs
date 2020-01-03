using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class EmpresaMap : IEntityTypeConfiguration<Empresa>
    {
        public void Configure(EntityTypeBuilder<Empresa> builder)
        {
            builder.HasKey(e => e.EmpresaId);
            builder.Property(e => e.NomeFantasia).IsRequired();
            builder.Property(e => e.RazaoSocial).IsRequired();
            builder.HasIndex(e => e.CNPJ).IsUnique();
            builder.Property(e => e.CNPJ).IsRequired();
            builder.Property(e => e.InscricaoEstadual);
            builder.Property(e => e.InscricaoMunicipal);
            builder.Property(e => e.CNES);
            builder.Property(e => e.Numero);
            builder.Property(e => e.Telefone);
            builder.Property(e => e.Unidade).IsRequired();
            builder.Property(e => e.Status).IsRequired();
            builder.Property(e => e.CepId).IsRequired(false);
            builder.Property(e => e.Logo);
            builder.Property(e => e.DtCadastro);

            builder.HasOne(e => e.Cep);
            builder.ToTable("Empresas");

        }
    }
}
