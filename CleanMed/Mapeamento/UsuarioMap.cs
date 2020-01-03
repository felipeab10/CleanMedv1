using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.Property(u => u.Nome).IsRequired();
            builder.Property(u => u.DataNascimento).HasColumnType("date");
            builder.HasIndex(u => u.CPF).IsUnique();
            builder.Property(u => u.CPF).IsRequired();
            builder.Property(u => u.Status).IsRequired();
            builder.Property(u => u.Observacao).IsRequired(false);
            builder.Property(u => u.Numero).IsRequired(false);
            builder.Property(u => u.Telefone).IsRequired(false);
            builder.Property(u => u.CepId).IsRequired(false);
            builder.Property(u => u.SetorId).IsRequired(false);
            builder.Property(u => u.DtCadastro);
            
            builder.HasOne(u => u.Cep);
            builder.HasOne(u => u.Setor);

            builder.Ignore(u => u.EmailConfirmed);
            builder.Ignore(u => u.AccessFailedCount);
            builder.Ignore(u => u.LockoutEnabled);
            builder.Ignore(u => u.LockoutEnd);
            builder.Ignore(u => u.PhoneNumber);
            builder.Ignore(u => u.PhoneNumberConfirmed);
            builder.Ignore(u => u.TwoFactorEnabled);

            builder.ToTable("Usuarios");

       
           
        }
    }
}
