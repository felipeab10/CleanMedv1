
    $(document).ready(function () {
        $('.tabs').tabs();
        $('.modal').modal();
       
       

    });
$("input[id*='Nome']").inputmask({
    regex: "[a-z-áàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ ]*" ,
    casing: "upper"
});

$("input[id*='ContatoNome']").inputmask({
    regex: "[a-z-áàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ ]*",
    casing: "upper"
});
        $("input[id*='CEP']").inputmask({
        mask: ['99999-999'],
        keepStatic: true
});
         $("input[id*='DataNascimento']").inputmask({
        mask: ['99/99/9999'],
    keepStatic: true
});

$("input[id*='CPFPaciente']").inputmask({
            mask: ['999.999.999-99'],
           
             keepStatic: true
});
        $("input[id*='Telefone']").inputmask({
        mask: ['(99)9999-9999', '(99)9 9999-9999'],
    keepStatic: true
});
     
         

//Buscando CEP primeiramente no banco de dados caso não tenha registro ele busca no viacep
         $("#CEPS").focusout(function () {
        var ceps = $(this).val()
        $.ajax({
        url: '/Pacientes/LocalizarCEP/' + $(this).val(),
            data: {CEP: $(this).val() },
            success: function (resposta) {
        NProgress.start();
                if (resposta != null) {

        $("#logradouro").val(resposta.logradouro);
    $("#complemento").val(resposta.complemento);
    $("#bairro").val(resposta.bairro);
    $("#cidade").val(resposta.cidade);
    $("#uf").val(resposta.uf);
    $("#cepid").val(resposta.cepId);


$("#numero").focus();
NProgress.done();
// $('.progress').hide();
                    } else {
        $.ajax({
            url: 'https://viacep.com.br/ws/' + ceps + '/json/unicode/',
            success: function (resposta) {
                NProgress.start();
                if (resposta != null) {
                    $("#logradouro").val(resposta.logradouro);
                    $("#complemento").val(resposta.complemento);
                    $("#bairro").val(resposta.bairro);
                    $("#cidade").val(resposta.localidade);
                    $("#uf").val(resposta.uf);

                    $("#numero").focus();
                    $("#cepid").val(0);
                }
                NProgress.done();
            },
        });
}


}

});
         });




//Validando Nome + data de nascimento, na edição do paciente
$("#DataNascimento").focusout(function () {
    var DataNascimento = document.getElementById("DataNascimento").value;
    //validando se é cadastro novo ou edição de paciente
    if ($("#PacienteId").val() != null) {
        //Edição do paciente
        var pacienteId = document.getElementById("PacienteId").value;

        var Nome = document.getElementById("Nome").value;
        $.ajax({
            url: '/Pacientes/PacienteExisteNome/',
            data: { Nome: Nome, DataNascimento: DataNascimento, PacienteId: pacienteId },
            success: function (resposta) {
                if ((resposta != null && resposta != true)) {
                    NProgress.start();
                    Swal.fire({
                        title: '<strong>Paciente já Cadastrado</strong>',
                        icon: 'info',
                        html:
                            'Esse nome e data de nascimento está cadastrado para, ' + '<p>' + '<b>' + resposta.nome +'</b>' + '</p>' + '</b>' + '</b>' + 'Pesquise pelo id ' + '<b>' + resposta.pacienteId + '</b>',

                        
                       
                        focusConfirm: false,
                        
                    })
                    $('#modalAddPaciente').modal('close');
                    NProgress.done();
                }
            }
        })
    } else {
        //Novo Cadastro de paciente
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
                            'Esse nome e data de nascimento está cadastrado para, ' + '<p>' + '<b>' + resposta.nome + '</b>' + '</p>' + '</b>' + '</b>' + 'Pesquise pelo id ' + '<b>' + resposta.pacienteId + '</b>',

                       
                     
                        focusConfirm: false,
                       
                    })
                    $('#modalAddPaciente').modal('close');
                    NProgress.done();
                }
            }
        })
    }
   

});





//Validando o cpf, para saber se há algum paciente cadastrado com esse cpf
$("#CPFPaciente").focusout(function () {
    var cpf = $(this).val();
    if ($("#PacienteId").val() != null) {
        var pacienteId = document.getElementById("PacienteId").value;
        $.ajax({
            url: '/Pacientes/PacienteExisteCPF/' + $(this).val(),
            data: { CPF: $(this).val(), PacienteId: pacienteId },
            success: function (resposta) {
                if ((resposta != null && resposta != true)) {
                    NProgress.start();
                    Swal.fire({
                        title: '<strong>Paciente já cadastrado</strong>',
                        icon: 'info',
                        html:
                            'Esse CPF está cadastrado para, ' + '<b>' + '<p>' + resposta.nome + '</p>' + '</b>' + '</b>' + 'Pesquise pelo id ' + '<b>' + resposta.pacienteId + '</b>',


                        showCloseButton: true,
                        showCancelButton: true,
                        focusConfirm: false,
                        confirmButtonText:
                            '<a style="color:white;"  href="/Pacientes/Paciente?PacienteId=' + resposta.pacienteId + '">Acessar cadastro</a>',
                        confirmButtonAriaLabel: 'Thumbs up, great!',
                        cancelButtonText:
                            'Fechar',
                        cancelButtonAriaLabel: 'Thumbs down'
                    })
                    $('#modalAddPaciente').modal('close');
                    NProgress.done();
                }
            }
        })
    } else {
        $.ajax({
            url: '/Pacientes/PacienteExisteCPF/' + $(this).val(),
            data: { CPF: $(this).val() },
            success: function (resposta) {
                console.log(resposta);
                if ((resposta != null && resposta != false)) {
                   
                    NProgress.start();
                    Swal.fire({
                        title: '<strong>Paciente já cadastrado</strong>',
                        icon: 'info',
                        html:
                            'Esse CPF está cadastrado para: ' + '<b>' + '<p>' + resposta.nome + '</p>' + '</b>' + '</b>' + 'Pesquise pelo id ' + '<b>' + resposta.pacienteId+'</b>',


                        showCloseButton: true,
                        //showCancelButton: true,
                        focusConfirm: false,
                        
                        
                        cancelButtonText:
                            'Fechar',
                        cancelButtonAriaLabel: 'Thumbs down'
                    })
                    $('#modalAddPaciente').modal('close');
                    NProgress.done();
                }
            }
        })
    }

});
function addPaciente() {
   
    var form = $("#PacienteForm");
    var data = form.serialize();
    $.ajax({
        url: "/AgendasMedicas/modalAddPaciente",
        method: "POST",
        data: data,
        success: function (resposta) {

            if (resposta == true) {
               
            $('#modalAddPaciente').modal('close');
            } else {
            console.log(resposta);
            var newBody = $(resposta);
            //$('#modalAgendamento').modal();
            //$('#modalAgendamento').modal('open');
            $('#modalAddPaciente').find('#myForm').replaceWith(newBody);
            
            }
        }

    })
}

function habilitaCPF(obj) {
    var resultado = document.getElementById("SemCPF").checked;
    if (resultado == true) {
       
        document.getElementById("CPFPaciente").disabled = false;
        
    } else {
       
        document.getElementById("CPFPaciente").disabled = true;
        
    }
    //alert(resultado);
    

}