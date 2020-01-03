using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class ConvenioAgendaMedicaMap : IEntityTypeConfiguration<ConvenioAgendaMedica>
    {
        public void Configure(EntityTypeBuilder<ConvenioAgendaMedica> builder)
        {
            builder.HasKey(c => new {c.ConvenioId,c.AgendaMedicaId });
            builder.ToTable("ConveniosAgendasMedica");
        }
    }
}
