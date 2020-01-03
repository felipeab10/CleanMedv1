using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class ItemAgendaMedicaMap : IEntityTypeConfiguration<ItemAgendaMedica>
    {
        public void Configure(EntityTypeBuilder<ItemAgendaMedica> builder)
        {
            builder.HasKey(i => new { i.ItemAgendamentoId,i.AgendaMedicaId });
            builder.ToTable("ItensAgendasMedica");
        }
    }
}
