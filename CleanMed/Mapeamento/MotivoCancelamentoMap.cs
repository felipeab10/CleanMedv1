using CleanMed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.Mapeamento
{
    public class MotivoCancelamentoMap : IEntityTypeConfiguration<MotivoCancelamento>
    {
        public void Configure(EntityTypeBuilder<MotivoCancelamento> builder)
        {
            builder.HasKey(a => a.MotivoCancelamentoId);
            builder.Property(a => a.Descricao).IsRequired();
            
            builder.ToTable("MotivoCancelamentos");
            builder.HasData(
                new MotivoCancelamento {MotivoCancelamentoId = 1,Descricao= "CANCELADO PELO PACIENTE" },
                new MotivoCancelamento { MotivoCancelamentoId = 2, Descricao = "CANCELADO PELO MÉDICO(A)" },
                new MotivoCancelamento { MotivoCancelamentoId = 3, Descricao = "PACIENTE NÃO COMPARECEU" },
                new MotivoCancelamento { MotivoCancelamentoId = 4, Descricao = "OBITO DO PACIENTE" },
                new MotivoCancelamento { MotivoCancelamentoId = 5, Descricao = "ERRO DE DIGITACAO" },
                new MotivoCancelamento { MotivoCancelamentoId = 6, Descricao = "GUIA NÃO AUTORIZADA" }


                );
        }
    }
}
