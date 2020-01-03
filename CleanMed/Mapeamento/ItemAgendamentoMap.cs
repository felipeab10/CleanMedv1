using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class ItemAgendamentoMap : IEntityTypeConfiguration<ItemAgendamento>
    {
        public void Configure(EntityTypeBuilder<ItemAgendamento> builder)
        {
            builder.HasKey(i => i.ItemAgendamentoId);
            builder.Property(i => i.Tipo).IsRequired();
            builder.Property(i => i.Status);
            builder.Property(i => i.Descricao).IsRequired();
            builder.Property(i => i.ExameId).IsRequired(false);
            builder.Property(i => i.RecursoAgendamentoId).IsRequired(false);
           
            builder.HasOne(i => i.Exame);
            builder.HasOne(i => i.RecursoAgendamento);
            builder.ToTable("ItemAgendamentos");
        }
    }
}
