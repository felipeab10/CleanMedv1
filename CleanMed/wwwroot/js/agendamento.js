

$('#tipoConfirmacao').formSelect({ dropdownOptions: { container: document.body } });
var elem = $('.select_dropdown');
var instances = M.FormSelect.init(elem, { dropdownOptions: { container: document.body } });
       

$("#agendar").click(function () {
    var form = $("#formAgendamento").serialize();
    if (form == '') {
        M.toast({ html: 'Selecione um horário' });
    } else {
    $.ajax({
        url: "/AgendasMedicas/Agendar",
        data: form,
        method: 'POST',
        success: function (resposta) {
            //console.log(resposta);
            if (resposta == false) {
                M.toast({ html: 'Horário já possui paciente agendado' })

            } else {
               /*
                $.ajax({
                    url: "/AgendasMedicas/horarioLivre",
                    data: { id: resposta },
                    method: 'POST',
                    success: function (modal) {
                      

                    }
                })
                */
                var teste  = JSON.stringify(resposta);
                $("#modalAgendamento").load("/AgendasMedicas/horarioLivre/" + resposta, function (resposta) {
                   $('#modalAgendamento').modal();
                  $('#modalAgendamento').modal('open');
                })
               
            }
        }
    })
    }
})
$("#confirmar").click(function () {
    var form = $("#formAgendamento").serialize();
    if (form == '') {
        M.toast({ html: 'Selecione um horário' });
    } else {
        $.ajax({
            url: "/AgendasMedicas/PacienteConfirmadoExiste",
            data: form,
            method: 'POST',
            success: function (resposta) {
                //console.log(resposta);
                if (resposta == "semagendamento") {
                    M.toast({ html: 'Horário sem agendamento' })
                } else {

                
                if (resposta == false) {
                    M.toast({ html: 'paciente já confirmado' })

                } else {
                    /*
                     $.ajax({
                         url: "/AgendasMedicas/horarioLivre",
                         data: { id: resposta },
                         method: 'POST',
                         success: function (modal) {
                           
     
                         }
                     })
                     */
                    var teste = JSON.stringify(resposta);
                    $("#modalConfirmado").load("/AgendasMedicas/ConfirmarPaciente/" + resposta, function (resposta) {
                        $('#modalConfirmado').modal();
                        $('#modalConfirmado').modal('open');
                    })

                    }
                }
            }
        })
    }
})
$("#cancelar").click(function () {
    var form = $("#formAgendamento").serialize();
    if (form == '') {
        M.toast({ html: 'Selecione um horário' });
    } else {
        $.ajax({
            url: "/AgendasMedicas/VerificaCancelamento",
            data: form,
            method: 'POST',
            success: function (resposta) {
                console.log(resposta.resposta);
                if (resposta.resposta == "Cancelado") {
                    $("#modalCancelado").load("/AgendasMedicas/ReverterPost/" + resposta.data, function (resposta) {
                       $('#modalCancelado').modal();
                        $('#modalCancelado').modal('open');
                   })
                   // M.toast({ html: 'paciente já cancelado' })
                } else {
                  
                if (resposta == "Livre") {
                    M.toast({ html: 'Horário sem agendamento' })
                }
                else
                {
                   
                    
                if (resposta == false) {
                    M.toast({ html: 'paciente já cancelado' })

                } else {
                    /*
                     $.ajax({
                         url: "/AgendasMedicas/horarioLivre",
                         data: { id: resposta },
                         method: 'POST',
                         success: function (modal) {
                           
     
                         }
                     })
                     */
                    var teste = JSON.stringify(resposta);
                    $("#modalCancelado").load("/AgendasMedicas/CancelarPost/" + resposta.data, function (resposta) {
                        $('#modalCancelado').modal();
                        $('#modalCancelado').modal('open');
                    })

                    }
                    }
                }
            }
        })
    }
})
$(function () {
    $("#DataNascimento").focusout(function () {
        var DataNascimento = document.getElementById("DataNascimento").value;
        var Nome = document.getElementById("Nome").value;
        $.ajax({
            url: '/Pacientes/PacienteExisteNome/',
            data: { Nome: Nome, DataNascimento: DataNascimento },
            success: function (resposta) {
                if ((resposta != null && resposta != true)) {
                    NProgress.start();
                    Swal.fire({
                        title: '<strong>Paciente já Cadastrado</strong>',
                        icon: 'info',
                        html:
                            'Esse CPF está cadastrado para, ' + '<b>' + resposta.nome + '</b>'
                            + '<p>' + 'Pesquise pelo codigo do paciente = ' + '<b>' + resposta.pacienteId + '</b>' + '</p>',

                        focusConfirm: false,

                        confirmButtonAriaLabel: 'Thumbs up, great!',
                        cancelButtonText:
                            'Fechar',
                        cancelButtonAriaLabel: 'Thumbs down'
                    })
                    $('.modal').modal('destroy');
                    NProgress.done();
                }
            }
        })
    })
    $("#CPF").focusout(function () {
        var CPF = document.getElementById("CPF").value;
        $.ajax({
            url: '/Pacientes/PacienteExisteCPF/',
            data: { CPF: CPF },
            success: function (resposta) {
                if ((resposta != null && resposta != true)) {
                    NProgress.start();
                    Swal.fire({
                        title: '<strong>Paciente já Cadastrado</strong>',
                        icon: 'info',
                        html:
                            'Esse CPF está cadastrado para, ' + '<b>' + resposta.nome + '</b>'
                            + '<p>' + 'Pesquise pelo codigo do paciente = ' + '<b>' + resposta.pacienteId + '</b>' + '</p>',

                        focusConfirm: false,

                        confirmButtonAriaLabel: 'Thumbs up, great!',
                        cancelButtonText:
                            'Fechar',
                        cancelButtonAriaLabel: 'Thumbs down'
                    })
                    $('.modal').modal('destroy');
                    NProgress.done();
                }
            }
        })
    })

    $(".btnAdd").click(function () {


        var nome = document.getElementById("Nome").value;

        var telefone = document.getElementById("telefone").value;

        var cpf = document.getElementById("CPF").value;

        var sexoId = document.getElementById("SexoId").value;

        var dataNascimento = document.getElementById("DataNascimento").value;
      

        $.ajax({
            url: "/Pacientes/addPacienteFromAgendamento",
            method: "POST",
            data: { Nome: nome, DataNascimento: dataNascimento, Telefone: telefone, CPF: cpf, Sexo: sexoId },
            success: function () {
                NProgress.start();
                $('.modal').modal('destroy');
                NProgress.done();
            }
        })
    })

});
$(document).ready(function () {
    //PegarEventoDodia()


    //Preenche automaticamente o plugin select2 com o resultado da pesquisa
    //var  searchItemAgendamentoId = $('#SearchItemAgendamentoId').val();
    //if (searchItemAgendamentoId != null) {
        //$('.select2ItemAgendamento').val(parseInt(searchItemAgendamentoId)).trigger('change');
 //;
   // }
    //Preenche automaticamente o plugin select2 com o resultado da pesquisa
    var SearchPrestadorId = $('#SearchPrestadorId').val();
    
    if (SearchPrestadorId != null) {
        //$('.select2Prestador').val(parseInt(SearchPrestadorId)).trigger('change');
     
        var Values = new Array();
        Values.push("1");
        Values.push("2");
        Values.push("3");

        //$(".select2Prestador").val(SearchPrestadorId).trigger('change');
    }

    $('.modal').modal();
    $("input[id*='telefone']").inputmask({
        mask: ['(99)9999-9999', '(99)9 9999-9999'],
        keepStatic: true
    });
    $("input[id*='DataNascimento']").inputmask({
        mask: ['99/99/9999'],
        keepStatic: true
    });
    $("input[id*='DtNascimento']").inputmask({
        mask: ['99/99/9999'],
        keepStatic: true
    });
    $("input[id*='DataAgenda']").inputmask({
        mask: ['99/99/9999'],
        keepStatic: true
    });
    $("input[id*='DtAgenda']").inputmask({
        mask: ['99/99/9999'],
        keepStatic: true
    });
    $("input[id*='DataLiberacao']").inputmask({
        mask: ['99/99/9999'],
        keepStatic: true
    });
    $("input[id*='HoraInicio']").inputmask({
        mask: ['99:99'],
        keepStatic: true
    });
    $("input[id*='HoraFim']").inputmask({
        mask: ['99:99'],
        keepStatic: true
    });
    $("input[id*='QtTempoMedio']").inputmask({
        mask: ['99:99:99'],
        keepStatic: true
    });
    $("input[id*='CPF']").inputmask({
        mask: ['999.999.999-99'],
        keepStatic: true
    });
    $("input[id*='Nome']").inputmask({
        regex: "[a-z-áàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ ]*",
        casing: "upper"
    });

    function format(repo) {
        //if (!repo.id) return repo.text; // optgroup
        //return repo.nome + "-" + repo.dataNascimento + " CPF " + repo.CPF;
        //função formata o layout da caixa de seleção da pesquisa, icones, e os campos que será exibido
        var markup = "<div class='select2-result-repository clearfix'>" + "<div class='control-label'><strong><i class=\"fas fa-user-injured\"></i>" + ' ' + repo.nome + "</strong></div>";
        if (repo.nome != null)
            markup += "<div style='' class='select2-result-repository__description'>" + "<i class=\"fas fa-birthday-cake\"></i>" + " " + moment(repo.dataNascimento).format('L') + " " + "<i class=\"far fa-address-card\"></i>" + " " + repo.cpf + "</div>";

        return markup;
    }





    //faz a pesquisa do prestador na index para filtrar agenda
    $('.select2Prestador').select2({
        placeholder: "",
        allowClear: true,
        minimumInputLength: 2,
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
        /*
        ajax: {
            url: '/AgendasMedicas/PesquisarPrestador',
            width: 'resolve',
            contentType: "application/json; charset=utf-8",
            data: function (params) {
                return {
                    q: params.term, // search term

                };
            },
            processResults: function (data) {
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

        */
    }).on('select2:select', function (data) {
        //função de ao selecionar um registro, salva na variavel abaixo
        var resultado = data.params.data;

      
    })
    //Pesquisa o nome do paciente para popular o plugin select2
    function format(repo) {
        //if (!repo.id) return repo.text; // optgroup
        //return repo.nome + "-" + repo.dataNascimento + " CPF " + repo.CPF;
        //função formata o layout da caixa de seleção da pesquisa, icones, e os campos que será exibido
        var markup = "<div class='select2-result-repository clearfix'>" + "<div class='control-label'><strong><i class=\"fas fa-user-injured\"></i>" + ' ' + repo.nome + "</strong></div>";
        if (repo.nome != null)
            markup += "<div style='' class='select2-result-repository__description'>" + "ID:  " + repo.pacienteId +" - " + "<i class=\"fas fa-birthday-cake\"></i>" + " " + moment(repo.dataNascimento).format('L') + " " + "<i class=\"far fa-address-card\"></i>" + " " + repo.cpf + "</div>";

        return markup;
    }
    var $selectbox = $('.select2-paciente').select2({
       
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
                return 'Sem resultados';
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
        

    
    //faz a pesquisa do Item de agendamento na index para filtrar agenda
    $('.select2ItemAgendamento').select2({
        placeholder: "",
        minimumInputLength: 2,
        allowClear: true,
       
        language: {
            inputTooShort: function () {
                return "Insira 3 ou mais caracteres";
            },
            noResults: function () {
                return 'Sem resultados ';
            },
        },
        escapeMarkup: function (markup) {
            return markup;
        },
        /*
        ajax: {
            url: '/AgendasMedicas/PesquisarItemAgendamento',
            width: 'resolve',
            contentType: "application/json; charset=utf-8",
            data: function (params) {
                return {
                    q: params.term, // search term

                };
            },
            processResults: function (data) {
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
*/

    }).on('select2:select', function (data) {
        //função de ao selecionar um registro, salva na variavel abaixo
        var resultado = data.params.data;


    })


})

function qtdAtendimento() {
    
        var horaInicio = $("#HoraInicio").val();
        //alert(horaInicio);
        var horaFim = $("#HoraFim").val();
       // alert(horaFim);
        var qQtTempoMedio = $("#QtTempoMedio").val();
        if ((horaInicio != null && horaFim != null && qQtTempoMedio != null)) {
            //alert(horaInicio);
            var Hora = moment(horaFim, 'HH:mm:ss') - moment(horaInicio, 'HH:mm:ss'); // moment(qQtTempoMedio, 'HH:mm:ss');
           // alert(Hora);
            var ToMinutos = Hora / 60000;
            //alert(ToMinutos);
            var Medio = moment(qQtTempoMedio, 'HH:mm:ss');
            var e = qQtTempoMedio.split(':');
            //alert(e[1]);
            var qtdAtendimento = ToMinutos / e[1];
            //alert(qtdAtendimento);
            if (!Number.isInteger(qtdAtendimento)) {
                alert("O resultado tem que ser número inteiro");
                $("#QtAtendimento").val(qtdAtendimento);
            } else {
                $("#QtAtendimento").val(qtdAtendimento);
            }
         
        }

    
}







//faz a pesquisa do paciente na index para filtrar agenda
function format(repo) {
    //if (!repo.id) return repo.text; // optgroup
    //return repo.nome + "-" + repo.dataNascimento + " CPF " + repo.CPF;
    //função formata o layout da caixa de seleção da pesquisa, icones, e os campos que será exibido
    var markup = "<div class='select2-result-repository clearfix'>" + "<div class='control-label'><strong><i class=\"fas fa-user-injured\"></i>" + ' ' + repo.nome + "</strong></div>";
    if (repo.nome != null)
        markup += "<div style='' class='select2-result-repository__description'>" + "<i class=\"fas fa-birthday-cake\"></i>" + " " + moment(repo.dataNascimento).format('L') + " " + "<i class=\"far fa-address-card\"></i>" + " " + repo.cpf + "</div>";

    return markup;
}

var $selectbox = $('.select2-paciente-central').select2({
    placeholder: "id,nome,cpf,data de nascimento",
    allowClear: true,
    templateResult: format,

    language: {
        inputTooShort: function () {
            return "Insira 3 ou mais caracteres";
        },
        noResults: function () {
            //sem resultados adiciona novo paciente
            return 'Nenhum registro encontrado' + '<p>Pesquisa por data: 00/00/0000</p>' + '<p>Pesquisa por CPF: 000.000.000-00 </p>';
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
       
        //o metodo abaixo faz uma busca no banco para trazer informações complementares do paciente
        $.ajax({
            url: "/AgendasMedicas/PesquisarInfoPaciente",
            data: { PacienteId: resultado.pacienteId },
            success: function (data) {
                if (data != null) {
                    NProgress.start();
                    //se tiver informações preenche os campos abaixo;

                    $("#nCartaoConvenio").val(data.numeroCarteira);
                    $("#ConvenioId").val(data.convenioId);
                    $('#ConvenioId').formSelect();
                    NProgress.done();
                } else {
                    NProgress.start();
                    //caso contrario limpa os campos para caso tenha registro anteriores
                    $("#nCartaoConvenio").val("");
                    $("#ConvenioId").val("");
                    $('#ConvenioId').formSelect();
                    NProgress.done();
                }
            }
        })

        $('label[for="Nome"]').addClass('filled active');


    })

$(function () {

    //Preenche automaticamente o campo Periodo da pesquisa
    var Periodo = $('#SearchPeriodo').val();
   
       
        $('#SearchPeriodoSelect option[value=' + Periodo + ']').attr('selected', true);
        $('#SearchPeriodoSelect').formSelect();
    

})

$(".opcaoAgendamento").on('click', function (obj) {
    //console.log(obj.currentTarget);
    alert(obj.currentTarget.value)
   
        })


function pupularCalendario() {
    var dtAgenda = document.getElementById("DtAgenda").value;
    //var prestadorId = [document.getElementById("PrestadorId").value];
    let prestadorId = $("select[name='PrestadorId']").val();
   
    //console.log(prestadorId.toString());
    if (dtAgenda != '' || prestadorId != 0) {
       // alert(dtAgenda);
        $.ajax({
            url: "/AgendasMedicas/GetEvents",
            method: "POST",
            data: { SearchDataAgenda: dtAgenda, PrestadorId: prestadorId },
            success: function (resposta) {
               
               // $('#calendar').fullCalendar('destroy');
                FetchEventAndRenderCalendar(resposta)

            }

        })
    } else {

    }

}

function PegarEventoDodia() {
    var events = [];
    var selectedEvent = null;
    $.ajax({
        type: "POST",
        url: "/AgendasMedicas/GetEvents",

        success: function (data) {
            //console.log(data);
            $.each(data, function (i, v) {
                events.push({
                    id: v.id,
                    title: "-",
                    Paciente: v.title,
                    description: v.itemAgendamento,
                    itemAgendamento: v.itemAgendamento,
                    start: moment(v.start),
                    end: v.end != null ? moment(v.end) : null,
                    color: v.color,
                    allDay: v.allDay,
                    minTime: v.minTime,
                    maxTime: v.maxTime,
                    slotDuration: v.slotDuration,
                    slotLabelInterval: v.slotLabelInterval,
                    StatusAgenda: v.statusAgenda,
                    PrestadorNome: v.prestadorNome,
                    DataAgenda: v.dataAgenda
                });
            })

            GenerateCalender(events);
        },
        error: function (error) {
            alert('failed');
        }
    })
}
var events = [];
var selectedEvent = null;
FetchEventAndRenderCalendar();
function FetchEventAndRenderCalendar(data) {
   
    events = [];
    $.each(data, function (i, v) {
        events.push({
            id: v.id,
            title: "-",
            Paciente: v.title,
            description: v.itemAgendamento,
            itemAgendamento: v.itemAgendamento,
            start: moment(v.start),
            end: v.end != null ? moment(v.end) : null,
            color: v.color,
            allDay: v.allDay,
            minTime: v.minTime,
            maxTime: v.maxTime,
            slotDuration: v.slotDuration,
            slotLabelInterval: v.slotLabelInterval,
            StatusAgenda: v.statusAgenda,
            PrestadorNome: v.prestadorNome,
            DataAgenda:v.dataAgenda
        });
    })
    GenerateCalender(events);
    GenerateCalender(events);
    /*$.ajax({
        type: "GET",
        url: "/AgendasMedicas/GetEvents",
       
        success: function (data) {
            console.log(data);
            $.each(data, function (i, v) {
                events.push({
                    id: v.id,
                    title: "-",
                    Paciente: v.title,
                    description: v.itemAgendamento,
                    itemAgendamento: v.itemAgendamento,
                    start: moment(v.start),
                    end: v.end != null ? moment(v.end) : null,
                    color: v.color,
                    allDay: v.allDay,
                    minTime: v.minTime,
                    maxTime: v.maxTime,
                    slotDuration: v.slotDuration,
                    slotLabelInterval: v.slotLabelInterval,
                    StatusAgenda: v.statusAgenda,
                    PrestadorNome: v.prestadorNome,
                });
            })

            GenerateCalender(events);
        },
        error: function (error) {
            alert('failed');
        }
    })
    */
}



function GenerateCalender(event) {
    //console.log(events[0]);
    var date = new Date();
    $('.calendar').fullCalendar('destroy');
    $('.calendar').fullCalendar({
        allDaySlot: false,
        slotEventOverlap: false,
        contentHeight: 450,
        timeFormat: 'HH:mm',
        slotLabelFormat: "HH:mm",
        defaultView: 'agenda',
        //defaultDate: new Date(),
        defaultDate: new Date(event[0].DataAgenda)   ,
        slotDuration: '00:20:00', //Define a exibição da hora na agenda se vai ser de 10 em 10 ou de 20 em 20
        slotLabelInterval: '00:20:00', //Utilizado em conjunto com o slotDuration.
        minTime: '06:00:00', //Define a hora de inicio da agenda
        maxTime: '22:00:00', //Define a hora de termino da agenda
        header: {
            left: '',
            center: '',
            right: 'basicDay,agenda'
        },
        titleFormat:'DD/MM/YYYY',
        eventLimit: true,
        eventColor: '#378006',
        events: event,
        eventRender: function (event, element) {
            element.find('.fc-title').prepend(event.Paciente != null ? '<span ><i class="fas fa-procedures"></i></span> ' + '<span style="font-size:10px;">' + event.Paciente + '</span>' + " - " + '<span ><i class="fas fa-file-medical-alt"></i></span> ' + '<span style="font-size:10px;">' + event.itemAgendamento + '</span>' + " - " + '<span ><i class="fas fa-user-md"></i></span> ' + '<span style="font-size:10px;">' + event.PrestadorNome + '</span>' : '<span ><i class="fas fa-file-medical-alt"></i></span> ' + '<span style="font-size:10px;">' + event.itemAgendamento + '</span >' + " - " + '<span ><i class="fas fa-user-md"></i></span> ' + '<span style="font-size:10px;">' + event.PrestadorNome + '</span>');
            
           
            element.qtip(
                {
                    content: "<b>Item de Agendamento: </b>" + "<p>" + event.description + "</p>" + "<p>" + "<b>Status: </b>" + "<p>" + event.StatusAgenda + "</p>" + "</p>" + "<b>Prestador: </b>" + "<p>" + event.PrestadorNome + "</p>"
                });
        },
       
        eventClick: function (event, jsEvent, view) {
            var date_start = $.fullCalendar.moment(event.start).format('YYYY-MM-DD');
            var time_start = $.fullCalendar.moment(event.start).format('HH:mm');
            var date_end = $.fullCalendar.moment(event.end).format('YYYY-MM-DD HH:mm');
            var id = event.id;

            if (event.StatusAgenda == "Livre") {
                //window.open("/AgendasMedicas/horarioLivre?id=" + id, '_blank');//"minhaJanela", "height=600,width=800");

                //Jeito 1
                /*
                $.get("/AgendasMedicas/horarioLivre", { id: id }, function (res) {
                    $('#modalAgendamentoContent').html(res);
                    $("#modalAgendamento").modal("open");
                })
                */
                $("#modalAgendamento").load("/AgendasMedicas/horarioLivre?id=" + id, function () {
                    $('#modalAgendamento').modal();
                    $('#modalAgendamento').modal('open');
                })

            }
            if (event.StatusAgenda == "Agendado") {


                $("#modalAgendado").load("/AgendasMedicas/Agendado?id=" + id, function () {
                    $('#modalAgendado').modal();
                    $('#modalAgendado').modal('open');
                })
            }
            if (event.StatusAgenda == "Confirmado") {
                // alert("Confirmado");
                $("#modalConfirmado").load("/AgendasMedicas/Confirmado?id=" + id, function () {
                    $('#modalConfirmado').modal();
                    $('#modalConfirmado').modal('open');
                })


            }
            if (event.StatusAgenda == "Cancelado") {
                // alert("Cancelado");
                $("#modalCancelado").load("/AgendasMedicas/Cancelado?id=" + id, function () {
                    $('#modalCancelado').modal();
                    $('#modalCancelado').modal('open');
                })


            }
            //$('#modal1 #horario').html(time_start);
            // $('#modal1').modal();
            //$('#modal1').modal('open'); 

        }

    });
}

/*
$('#calendar').fullCalendar({
    header: {
        left: 'prev,next today',
        center: 'title',
        right: 'month,agendaWeek,agendaDay'
    },
    defaultView:'agenda',
    firstDay: 1, //The day that each week begins (Monday=1)
    slotMinutes: 60,
  
    events:'https://localhost:44373/AgendasMedicas/GetEvents',
   
  
});

*/


