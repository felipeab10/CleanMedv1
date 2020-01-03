using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class PacienteMap : IEntityTypeConfiguration<Paciente>
    {
        public void Configure(EntityTypeBuilder<Paciente> builder)
        {
            builder.HasKey(p => p.PacienteId);
            builder.Property(p => p.Nome).IsRequired().HasMaxLength(160);
            builder.Property(p => p.DataNascimento).HasColumnType("date");
            builder.Property(p => p.RG).HasMaxLength(14);
            builder.HasIndex(p => p.CPF).IsUnique();
            builder.Property(p => p.CPF).IsRequired(false).HasMaxLength(16);
            builder.Property(p => p.Telefone).HasMaxLength(40);
            builder.Property(p => p.Email).HasMaxLength(60);
            builder.Property(p => p.Sexo).HasMaxLength(60);
            builder.Property(p => p.Estado_Civil).HasMaxLength(60);
            builder.Property(p => p.dt_cadastro);
            builder.Property(p => p.StatusId).HasColumnName("StatusId");
            builder.Property(p => p.Numero).IsRequired(false).HasMaxLength(5);
            builder.Property(p => p.CepId).IsRequired(false);
            builder.Property(p => p.SemCPF);
            builder.HasOne(p => p.Cep);
            builder.ToTable("Pacientes");
        }
    }
}
