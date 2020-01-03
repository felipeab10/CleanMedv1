
    $(document).ready(function () {
        $('.tabs').tabs();
        $('.modal').modal();
        //$("#arquivo").hide();

    });
$("input[id*='RazaoSocial']").inputmask({
    //regex: "[a-z-áàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ/- ]*" ,
    casing: "upper"
});

$("input[id*='Nome']").inputmask({
    //regex: "[a-z-áàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ/- ]*",
    casing: "upper"
});
        


function CarregarImagem(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $("#arquivo").attr('src', e.target.result).width(250).height(130);
            $("#arquivo").show();
        }
    }
    reader.readAsDataURL(input.files[0]);
}

$("input[id*='CEP']").inputmask({
    mask: ['99999-999'],
    keepStatic: true
});
$("input[id*='CNPJ']").inputmask({
    mask: ['99.999.999/9999-99'],
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
                            'Esse nome e data de nascimento está cadastrado para, ' + '<b>' + resposta.nome + '</b>',

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
                            'Esse nome e data de nascimento está cadastrado para, ' + '<b>' + resposta.nome + '</b>',

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
                    NProgress.done();
                }
            }
        })
    }
   

});





//Validando o cpf, para saber se há algum paciente cadastrado com esse cpf
$("#CNPJ").focusout(function () {
    var CNPJ = $(this).val();
    if ($("#EmpresaId").val() != null) {
        //Editando empresa
        var empresaId = document.getElementById("EmpresaId").value;
        $.ajax({
            url: '/Empresas/EmpresaExisteCNPJ/' + $(this).val(),
            data: { CNPJ: $(this).val(), EmpresaId: empresaId },
            success: function (resposta) {
                if ((resposta != null && resposta != true)) {
                    NProgress.start();
                    Swal.fire({
                        title: '<strong>Empresa já cadastrado</strong>',
                        icon: 'info',
                        html:
                            'Esse CNPJ está cadastrado para, ' + '<b>' + resposta.NomeFantasia + '</b>',


                        showCloseButton: true,
                        showCancelButton: true,
                        focusConfirm: false,
                        confirmButtonText:
                            '<a style="color:white;"  href="/Empresas/Edit?EmpresaId=' + resposta.empresaId + '">Acessar cadastro</a>',
                        confirmButtonAriaLabel: 'Thumbs up, great!',
                        cancelButtonText:
                            'Fechar',
                        cancelButtonAriaLabel: 'Thumbs down'
                    })
                    NProgress.done();
                }
            }
        })
    } else {
        //Cadastrando nova empresa
        $.ajax({
            url: '/Empresas/EmpresaExisteCNPJ/',
            data: { CNPJ: $(this).val()},
            success: function (resposta) {
                if ((resposta != null && resposta != true)) {
                    NProgress.start();
                    Swal.fire({
                        title: '<strong>Empresa já cadastrado</strong>',
                        icon: 'info',
                        html:
                            'Esse CNPJ está cadastrado para, ' + '<p>' + '<b>' + resposta.nomeFantasia + '</b>' + '</p>',


                        showCloseButton: true,
                        showCancelButton: true,
                        focusConfirm: false,
                        confirmButtonText:
                            '<a style="color:white;"  href="/Empresas/Edit/' + resposta.empresaId + '">Acessar cadastro</a>',
                        confirmButtonAriaLabel: 'Thumbs up, great!',
                        cancelButtonText:
                            'Fechar',
                        cancelButtonAriaLabel: 'Thumbs down'
                    })
                    NProgress.done();
                }
            }
        })
    }

});
