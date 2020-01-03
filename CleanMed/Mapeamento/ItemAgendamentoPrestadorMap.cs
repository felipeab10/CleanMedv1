using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class ItemAgendamentoPrestadorMap : IEntityTypeConfiguration<ItemAgendamentoPrestador>
    {
        public void Configure(EntityTypeBuilder<ItemAgendamentoPrestador> builder)
        {
            builder.HasKey(i => new { i.PrestadorId,i.ItemAgendamentoId });
        }
    }
}
