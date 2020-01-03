using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanMed.ViewModels
{
    public class AgendasMedicasViewModel
    {
        //Tebela Agendamento
        public int AgendaMedicaId { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataAgenda { get; set; }
        public int? RecursoAgendamentoId { get; set; }
        public int? PrestadorId { get; set; }
        [DisplayFormat(DataFormatString = "{0:T}", ApplyFormatInEditMode = true)]
        public DateTime HoraInicio { get; set; }
        [DisplayFormat(DataFormatString = "{0:T}", ApplyFormatInEditMode = true)]
        public DateTime HoraFim { get; set; }
        public string StatusAgendamento { get; set; }
        public DateTime DataLiberacao { get; set; }
        public int QtAtendimento { get; set; }
        public int QtEncaixe { get; set; }
        public DateTime DtCadastro { get; set; } = DateTime.Now;
        public string ObservacaoAgendamento { get; set; }
        public string ThemeColor { get; set; }
        public string UsuarioNome { get; set; }
        public string UsuarioId { get; set; }
        public int? EmpresaId { get; set; }
        public TimeSpan QtTempoMedio { get; set; }
        public bool Bloqueado { get; set; } = false;
        public string TipoAgenda { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int ItemAgendamentoId { get; set; }
        public string NomeItemAgendamento { get; set; }
        public string Color { get; set; } = "#4caf50";
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int ConvenioId { get; set; }
        //Tabela Paciente
        [Required(ErrorMessage = "Campo Obrigatório")]
        //[DataType(DataType.Currency,ErrorMessage ="valor invalido")]
        public int PacienteId { get; set; }
        public string NmPaciente { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Telefone { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string CPF { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Campo Obrigatório")]
        //[DataType(DataType.Date,ErrorMessage ="Campo Obrigatório")]
        public DateTime DataNascimento { get; set; }

        //tabela Agendamentos
        public int AgendamentoId { get; set; }
        [DisplayFormat(DataFormatString = "{0:T}", ApplyFormatInEditMode = true)]
        public DateTime HoraAgenda { get; set; }
        //tabela Prestador
        public string NomePrestador { get; set; }
        //tabela setores
        public int? SetorId { get; set; }
        public string SetorNome { get; set; }
        //Tabela Recurso
        public string RecursoNome { get; set; }
        //Tabela CartaoConvenio
        public string ConvenioNome { get; set; }
        public string NumeroCartaoConvenio { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CartaoValidade { get; set; }
        //Tabela MotivoCancelamento
        public int MotivoCancelamentoId { get; set; }
        public string MotivoDescricao { get; set; }
    }
}
