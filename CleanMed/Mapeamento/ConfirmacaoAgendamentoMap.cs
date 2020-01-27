using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class ConfirmacaoAgendamentoMap : IEntityTypeConfiguration<ConfirmacaoAgendamento>
    {
        public void Configure(EntityTypeBuilder<ConfirmacaoAgendamento> builder)
        {
            builder.HasKey(c => c.ConfirmacaoAgendamentoId);
            builder.Property(c => c.TipoConfirmacao).IsRequired();
            builder.Property(c => c.Nomecontato);
            builder.Property(c => c.ObservacaoConfirmacao);

            builder.HasOne(c=> c.Agendamento);
            builder.ToTable("ConfirmacaoAgendamentos");
        }
    }
}
