using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class RecursoAgendamentoMap : IEntityTypeConfiguration<RecursoAgendamento>
    {
        public void Configure(EntityTypeBuilder<RecursoAgendamento> builder)
        {
            builder.HasKey(r => r.RecursoAgendamentoId);
            builder.Property(r => r.Descricao).IsRequired();
            builder.Property(r => r.Sigla).IsRequired();
            builder.Property(r => r.Tipo).IsRequired();
            builder.Property(r => r.Status).IsRequired();

            builder.ToTable("RecursoAgendamentos");

        }
    }
}
