using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class AgendamentoLogMap : IEntityTypeConfiguration<AgendamentoLog>
    {
        public void Configure(EntityTypeBuilder<AgendamentoLog> builder)
        {
            builder.HasKey(a => a.AgendamentoLogId);
            builder.Property(a => a.Dt_Acao);
            builder.Property(a => a.Acao).IsRequired();

            builder.HasOne(a => a.Paciente);
            builder.HasOne(a => a.Agendamento);
            builder.HasOne(a => a.Usuario);
            builder.ToTable("AgendamentoLogs");

        }
    }
}
