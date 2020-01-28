$('#tipoConfirmacao').formSelect({ dropdownOptions: { container: document.body } });
var elem = $('.select_dropdown');
var instances = M.FormSelect.init(elem, { dropdownOptions: { container: document.body } });

$("#confirmarPost").click(function () {
    
    var form = $("#myForm").serialize();
  
    $.ajax({
        url: "/AgendasMedicas/ConfirmarPaciente",
        method: "POST",
        data: form,
        success: function (resposta) {
            if (resposta == true) {
                $("#modalConfirmado").modal("close");
               
                location.reload();
            }
        }
    })
})
$("#cancelarPost").click(function () {

    var form = $("#myForm").serialize();

    $.ajax({
        url: "/AgendasMedicas/CancelarAgendamento",
        method: "POST",
        data: form,
        success: function (resposta) {
            if (resposta == true) {
                $("#modalCancelado").modal("close");

                location.reload();
            }
        }
    })
})
$("#reverterPost").click(function () {

    var form = $("#myForm").serialize();

    $.ajax({
        url: "/AgendasMedicas/ReverterCancelamento",
        method: "POST",
        data: form,
        success: function (resposta) {
            if (resposta == true) {
                $("#modalCancelado").modal("close");

                location.reload();
            }
        }
    })
})
$("#excluirPost").click(function () {

    var form = $("#myForm").serialize();

    $.ajax({
        url: "/AgendasMedicas/ExcluirAgendamentoPaciente",
        method: "POST",
        data: form,
        success: function (resposta) {
            if (resposta == true) {
                $("#modalCancelado").modal("close");

                location.reload();
            }
        }
    })
})
$('.tooltipped').tooltip();
$('select').formSelect();
//$('#selectItemAgendamentoId').formSelect();
//$('#select2-paciente').formSelect();
//$('#select2-convenio').formSelect();
$('textarea').characterCounter();

$("input[id*='DtNascimento']").inputmask({
    mask: ['99/99/9999'],
    keepStatic: true
});
$("input[id*='CartaoValidade']").inputmask({
    mask: ['99/99/9999'],
    keepStatic: true
});
$("input[id*='Telefone']").inputmask({
    mask: ['(99)9999-9999', '(99)9 9999-9999'],
    keepStatic: true
});
$("input[id*='CPF']").inputmask({
    mask: ['999.999.999-99'],
    keepStatic: true
});

$('.modal select').css('width', '100%');

function format(repo) {
    //if (!repo.id) return repo.text; // optgroup
    //return repo.nome + "-" + repo.dataNascimento + " CPF " + repo.CPF;
    //função formata o layout da caixa de seleção da pesquisa, icones, e os campos que será exibido
    var markup = "<div class='select2-result-repository clearfix'>" + "<div class='control-label'><strong><i class=\"fas fa-user-injured\"></i>" + ' ' + repo.nome + "</strong></div>";
    if (repo.nome != null)
        markup += "<div style='' class='select2-result-repository__description'>" + "<i class=\"fas fa-birthday-cake\"></i>" + " " + moment(repo.dataNascimento).format('L') + " " + "<i class=\"far fa-address-card\"></i>" + " " + repo.cpf + "</div>";

    return markup;
}
var $selectbox = $('.select2-paciente').select2({
    dropdownParent: $('#modalAgendamento'),
    minimumInputLength: 1,
    placeholder: "id,nome,cpf,data de nascimento",
    allowClear: true,
    templateResult: format,

    language: {
        inputTooShort: function () {
            return "Insira 2 ou mais caracteres";
        },
        noResults: function () {
            //sem resultados adiciona novo paciente
            return 'Sem resultados ' + '  <a class="waves-effect waves-light btn  btn-small" onclick="modalAddPaciente()">Novo paciente</a>';
        },
    },
    escapeMarkup: function (markup) {
        return markup;
    },
    ajax: {
        //faz a pesquisa do paciente via ajax na controller do agendamento
        url: '/AgendasMedicas/PesquisarPaciente',
        width: 'resolve',
        contentType: "application/json; charset=utf-8",
        data: function (params) {

            return {
                //o parametro q é a string de busca no controlador

                q: params.term, // search term

            };
        },
        processResults: function (data) {
            //dados trazidos da pesquisa
            var resultado = data.items;

            return {

                results: resultado
            };

        },
        cache: true,
        escapeMarkup: function (markup) {
            return markup;
        },
        language: "pt-BR",
        width: 'resolve',
    }
})
    .on('select2:select', function (data) {
        //função de ao selecionar um registro, salva na variavel abaixo
        var resultado = data.params.data;

        if (resultado.telefone != null) {
            //valida se o paciente possui telefone cadastrado
            $("#dropdownTelefone").val(resultado.telefone);
        }
        //o metodo abaixo faz uma busca no banco para trazer informações complementares do paciente
        $.ajax({
            url: "/AgendasMedicas/PesquisarInfoPaciente",
            data: { PacienteId: resultado.pacienteId },
            success: function (data) {
                if (data != null) {

                    //se tiver informações preenche os campos abaixo;
                    $("#DtNascimento").val(moment(data.dataNascimento).format('L'));
                    $("#Telefone").val(data.telefone);
                    $("#CPF").val(data.cpf);
                    $("#numeroCartao").val(data.numeroCarteira);
                    $("#CartaoValidade").val(moment(data.validadeCarteira).format('L'));
                    $("#select2-convenio").val(data.convenioId);
                    $('#select2-convenio').formSelect();
                   
                } else {
                   
                    //caso contrario limpa os campos para caso tenha registro anteriores
                    $("#nCartaoConvenio").val("");
                    $("#ConvenioId").val("");
                    $('#ConvenioId').formSelect();

                }
            }
        })

        $('label[for="Nome"]').addClass('filled active');


    })
    .on('select2:unselect', function () {
        $('label[for="Nome"]').removeClass('filled');
    });

$selectbox.on('blur', function () {
    $('label[for="Nome"]').removeClass('active');
})
$selectbox.val("").trigger("change");


//Pesquisa o nome do convênio para popular o plugin select2
$('.select2-convenio').select2({
    dropdownParent: $('#modalAgendamento'),
    minimumInputLength: 2,
    placeholder: "",
    allowClear: true,
    language: {
        inputTooShort: function () {
            return "Insira 2 ou mais caracteres";
        },
        noResults: function () {
            return 'Sem resultados ';
        },
    },
    escapeMarkup: function (markup) {
        return markup;
    },
   


})






function addAgendamento() {
    var pacienteId = $('#PacienteId').val();
    var dataNascimento = $('#DtNascimento').val();
    var telefone = $('#Telefone').val();
    var CPF = $('#CPF').val();
    var agendamentoId = $('#AgendamentoId').val();
    var itemAgendamentoId = $('#selectItemAgendamentoId').val();
    var convenioId = $('#select2-convenio').val();
    var numeroCartao = $('#numeroCartao').val();
    var cartaoValidade = $('#CartaoValidade').val();
    var agendamentoId = $('#AgendamentoId').val();
    
    var data = $("#myForm").serialize();
    var modal = $("#modalAgendamento");
    var form = $('form[name="myForm"]');

      
    
    
    $.ajax({
        url: "/AgendasMedicas/agendarPaciente",
        data: data,
        //data: { PacienteId: pacienteId, DataNascimento: dataNascimento, Telefone: telefone, CPF: CPF, AgendamentoId: agendamentoId, ItemAgendamentoId: itemAgendamentoId, ConvenioId: convenioId, NumeroCartaoConvenio: numeroCartao, CartaoValidade: cartaoValidade },
        method:"POST",
        success: function (resposta) {
           
            if (resposta == true) {
                $('#modalAgendamento').modal('close');
                location.reload();
                var dtAgenda = document.getElementById("DtAgenda").value;
                if (dtAgenda != '') {
                    location.reload();
                    //$('#calendar').fullCalendar('destroy');
                   // pupularCalendario();
                } else {

               // $('#calendar').fullCalendar('destroy');
                 //   PegarEventoDodia();
                }
                //pupularCalendario();
                swal("Agendado com sucesso!", "", "success");
                console.log();
                //console.log(resposta);
                 //location.reload();

            } else {

            //console.log(resposta);
               // alert("erro");
            var newBody = $(resposta);
            //$('#modalAgendamento').modal();
            //$('#modalAgendamento').modal('open');
            $('#modalAgendamento').find('#myForm').replaceWith(newBody);
                console.log(resposta);
            } 
        }
    })
    
    
}


//Função para confirmar o paciente
function ConfirmarPaciente(id) {
   // alert("Paciente confirmado");
    if ($('#ObservacaoAgendamento').val() != "") {
        var obs = $('#ObservacaoAgendamento').val();
       // alert(obs);
    }
   
    swal({
        title: "Tem Certeza?",
        text: "que deseja confirmar esse paciente?",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: "/AgendasMedicas/ConfirmarPaciente",
                    method:"POST",
                    data: { id: id,ObservacaoAgendamento:obs },
                    success: function () {
                        $('#modalAgendado').modal('close');
                        var dtAgenda = document.getElementById("DtAgenda").value;
                        if (dtAgenda != '') {
                            $('#calendar').fullCalendar('destroy');
                            pupularCalendario();
                        } else {

                            $('#calendar').fullCalendar('destroy');
                            PegarEventoDodia();
                        }
                        swal("Confirmado com sucesso!", "", "success");
                    }
                })
            } else {
                swal("paciente não foi confirmado");
            }
        });
}

//Excluir o agendamento do paciente
function ExcluirAgendamentoPaciente(id) {
   // alert("Agendamento do paciente será excluido");
   

    swal({
        title: "Tem Certeza?",
        text: "que deseja excluir o agendamento desse paciente?",
        icon: "error",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: "/AgendasMedicas/ExcluirAgendamentoPaciente",
                    method: "POST",
                    data: { id: id },
                    success: function () {
                        $('#modalCancelado').modal('close');
                        $('#modalAgendado').modal('close');
                      

                        var dtAgenda = document.getElementById("DtAgenda").value;
                        if (dtAgenda != '') {
                            $('#calendar').fullCalendar('destroy');
                            pupularCalendario();
                        } else {

                            $('#calendar').fullCalendar('destroy');
                            PegarEventoDodia();
                        }
                        swal("Agendamento excluido com sucesso!", "", "success");
                    }
                })
            } else {
                swal("Agendamento não excluido");
            }
        });
}

//Cancelando o agendamento do paciente
function CancelarAgendamentoPaciente(id) {
    // alert("Agendamento do paciente será excluido");


    swal({
        title: "Tem Certeza?",
        text: "que deseja cancelar o agendamento desse paciente?",
        icon: "error",
        buttons: true,
        dangerMode: true,
        
    })
        .then((willDelete) => {
            if (willDelete) {
                $("#modalCancelamento").load("/AgendasMedicas/Cancelamento?id=" + id, function () {
                    $('#modalConfirmado').modal('close');
                    $('#modalCancelamento').modal();
                    $('#modalCancelamento').modal('open');
                })
                //$('#modalConfirmado').modal('close');
               /* $.ajax({
                    url: "/AgendasMedicas/ExcluirAgendamentoPaciente",
                    method: "POST",
                    data: { id: id },
                    success: function () {
                        location.reload();
                    }
                })
                */
            } else {
                swal("Agendamento não foi cancelado");
            }
        });
}

//Revertendo o cancelamento do agendamento para o status de agendado
function reverterCancelamento(id) {
    // alert("Agendamento do paciente será excluido");


    swal({
        title: "Tem certeza disso?",
        text: "que deseja reverter o agendamento desse paciente?",
        icon: "info",
        buttons: true,
        dangerMode: true,

    })
        .then((willRevert) => {
            if (willRevert) {
                $.ajax({
                    url: "/AgendasMedicas/ReverterCancelamento",
                    method: "POST",
                    data: { id: id },
                    success: function () {
                        $('#modalCancelado').modal('close');
                        var dtAgenda = document.getElementById("DtAgenda").value;
                        if (dtAgenda != '') {
                            $('#calendar').fullCalendar('destroy');
                            pupularCalendario();
                        } else {

                            $('#calendar').fullCalendar('destroy');
                            PegarEventoDodia();
                        }
                        swal("Agendamento Revertido com sucesso!", "", "success");
                    }

                })
            } else {
                swal("O cancelamento não foi revertido");
            }
        });
}
function modalAddPaciente() {
   
    $('#modalAddPaciente').load('/AgendasMedicas/modalAddPaciente', function () {

        $('#modalAddPaciente').modal();
        $('#modalAddPaciente').modal('open');


    })
}

function motivoCancelamento(id) {
    var form = $('#motivoCancelamentoForm');
    var data = form.serialize();
    $.ajax({
        url: "/AgendasMedicas/Cancelamento?id="+ id,
        method: "POST",
        data: data,
        success: function (resposta) {
            console.log(resposta);
            if (resposta == true) {
                $('#modalCancelamento').modal('close');
                var dtAgenda = document.getElementById("DtAgenda").value;
                if (dtAgenda != '') {
                    $('#calendar').fullCalendar('destroy');
                    pupularCalendario();
                } else {

                    $('#calendar').fullCalendar('destroy');
                    PegarEventoDodia();
                }
                swal("Cancelado com sucesso!", "", "success");
            }
        }

    })

}