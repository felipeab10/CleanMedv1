
$(document).ready(function () {
    $('.tooltipped').tooltip();
   
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

        $("input[id*='CPF']").inputmask({
            mask: ['999.999.999-99'],
           
             keepStatic: true
});
        $("input[id*='Telefone']").inputmask({
        mask: ['(99)9999-9999', '(99)9 9999-9999'],
    keepStatic: true
});
        function InputToUpper(obj) {
            if (obj.value != "") {
        obj.value = obj.value.toUpperCase();
}
}

         

//Buscando CEP primeiramente no banco de dados caso não tenha registro ele busca no viacep
         $("#CEPS").click(function () {
             var ceps = $("#CEP").val()
        $.ajax({
            url: '/Pacientes/LocalizarCEP/' + ceps,
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




//Validando Nome + data de nascimento, para saber se há algum prestador cadastrado com esse nome e data de nascimento
$("#DataNascimento").focusout(function () {
   //Validando se é um novo cadastro de prestador ou edição
    if ($("#prestadorId").val() != null) {
        //Edição de cadastro de prestador
        var DataNascimento = document.getElementById("DataNascimento").value;
        var Nome = document.getElementById("Nome").value;
        var prestadorId = document.getElementById("prestadorId").value;
        $.ajax({
            url: '/Prestadores/PrestadorExisteNome/',
            data: { Nome: Nome, DataNascimento: DataNascimento, PrestadorId: prestadorId },
            success: function (resposta) {
                if ((resposta != null && resposta != true)) {
                    NProgress.start();
                    Swal.fire({
                        title: '<strong>Prestador já Cadastrado</strong>',
                        icon: 'info',
                        html:
                            'Esse nome e data de nascimento está cadastrado para, ' + '<b>' + resposta.nome + '</b>',

                        showCloseButton: true,
                        showCancelButton: true,
                        focusConfirm: false,
                        confirmButtonText:
                            '<a style="color:white;"  href="/Prestadores/Prestador?PrestadorId=' + resposta.prestadorId + '">Acessar cadastro</a>',
                        confirmButtonAriaLabel: 'Thumbs up, great!',
                        cancelButtonText:
                            'Fechar',
                        cancelButtonAriaLabel: 'Thumbs down'
                    })
                    document.getElementById("post").disabled = true;
                    NProgress.done();
                } else {
                    document.getElementById("post").disabled = false;
                }
            }
        })
    } else {
        //Novo cadastro de prestador
        var DataNascimento = document.getElementById("DataNascimento").value;
        var Nome = document.getElementById("Nome").value;
        $.ajax({
            url: '/Prestadores/PrestadorExisteNome/',
            data: { Nome: Nome, DataNascimento: DataNascimento },
            success: function (resposta) {
                if ((resposta != null && resposta != true)) {
                    NProgress.start();
                    Swal.fire({
                        title: '<strong>Prestador já Cadastrado</strong>',
                        icon: 'info',
                        html:
                            'Esse nome e data de nascimento está cadastrado para, ' + '<b>' + resposta.nome + '</b>',

                        showCloseButton: true,
                        showCancelButton: true,
                        focusConfirm: false,
                        confirmButtonText:
                            '<a style="color:white;"  href="/Prestadores/Prestador?PrestadorId=' + resposta.prestadorId + '">Acessar cadastro</a>',
                        confirmButtonAriaLabel: 'Thumbs up, great!',
                        cancelButtonText:
                            'Fechar',
                        cancelButtonAriaLabel: 'Thumbs down'
                    })
                    document.getElementById("post").disabled = true;
                    NProgress.done();
                } else {
                    document.getElementById("post").disabled = false;
                }
            }
        })
    }

});


//Validando o cpf, para saber se há algum prestador cadastrado com esse cpf
$("#CPF").focusout(function () {
    var cpf = $(this).val();
    if ($("#prestadorId").val() != null) {
        //Editando prestador
        var prestadorId = document.getElementById("prestadorId").value;
        $.ajax({
            url: '/Prestadores/PrestadorExisteCPF/' + $(this).val(),
            data: { CPF: $(this).val(), PrestadorId: prestadorId },
            success: function (resposta) {
                if ((resposta != null && resposta != true)) {
                    NProgress.start();
                    Swal.fire({
                        title: '<strong>Prestador já cadastrado</strong>',
                        icon: 'info',
                        html:
                            'Esse CPF está cadastrado para, ' + '<b>' + resposta.nome + '</b>',


                        showCloseButton: true,
                        showCancelButton: true,
                        focusConfirm: false,
                        confirmButtonText:
                            '<a style="color:white;"  href="/Prestadores/Prestador?PrestadorId=' + resposta.prestadorId + '">Acessar cadastro</a>',
                        confirmButtonAriaLabel: 'Thumbs up, great!',
                        cancelButtonText:
                            'Fechar',
                        cancelButtonAriaLabel: 'Thumbs down'
                    })
                    document.getElementById("post").disabled = true;
                    NProgress.done();
                } else {
                    document.getElementById("post").disabled = false;
                }
            }
        })
    } else {
        $.ajax({
            url: '/Prestadores/PrestadorExisteCPF/' + $(this).val(),
            data: { CPF: $(this).val()},
            success: function (resposta) {
                if ((resposta != null && resposta != true)) {
                    NProgress.start();
                    Swal.fire({
                        title: '<strong>Prestador já cadastrado</strong>',
                        icon: 'info',
                        html:
                            'Esse CPF está cadastrado para, ' + '<b>' + resposta.nome + '</b>',


                        showCloseButton: true,
                        showCancelButton: true,
                        focusConfirm: false,
                        confirmButtonText:
                            '<a style="color:white;"  href="/Prestadores/Prestador?PrestadorId=' + resposta.prestadorId + '">Acessar cadastro</a>',
                        confirmButtonAriaLabel: 'Thumbs up, great!',
                        cancelButtonText:
                            'Fechar',
                        cancelButtonAriaLabel: 'Thumbs down'
                    })
                    document.getElementById("post").disabled = true;
                    NProgress.done();
                } else {
                    document.getElementById("post").disabled = false;
                }
            }
        })
    }

});

//Validando o numero crm e o conselho, para saber se há algum prestador cadastrado com esse CRM
$("#NumeroCRM").focusout(function () {
    var NumeroCRM = $(this).val();
    var conselhoId = $("#conselhoId option:selected").val();
    
    $.ajax({
        url: '/Prestadores/PrestadorExisteNumeroCrm/' + $(this).val(),
        data: { NumeroCrm: $(this).val(), ConselhoId: conselhoId },
        success: function (resposta) {
            if ((resposta != null && resposta != true)) {
                NProgress.start();
                Swal.fire({
                    title: '<strong>Prestador já cadastrado</strong>',
                    icon: 'info',
                    html:
                        'Esse CPF está cadastrado para, ' + '<b>' + resposta.nome + '</b>',


                    showCloseButton: true,
                    showCancelButton: true,
                    focusConfirm: false,
                    confirmButtonText:
                        '<a style="color:white;"  href="/Prestadores/Prestador?PrestadorId=' + resposta.prestadorId + '">Acessar cadastro</a>',
                    confirmButtonAriaLabel: 'Thumbs up, great!',
                    cancelButtonText:
                        'Fechar',
                    cancelButtonAriaLabel: 'Thumbs down'
                })
                document.getElementById("post").disabled = true;
                NProgress.done();
            } else {
                document.getElementById("post").disabled = false;
            }
        }
    })

});
function delEspecialidadePrestador(id, controller, prestadorId) {
    swal({
        title: "Tem certeza disso?",
        text: "Uma vez deletado, não será possivel recupera-lo",
        icon: "warning",
        buttons: {
            cancel: {
                text: "Cancelar",
                value: null,
                visible: true,
                className: "waves-effect waves-light btn-small",
                closeModal: true,
            },
            confirm: {
                text: "Excluir",
                value: true,
                visible: true,
                className: "waves-effect waves-light btn-small red",
                closeModal: true
            }
        },
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
               
                var url = "/" + controller + "/DesassociarEspecialidade/";
                $.ajax({
                    method: "POST",
                    url: url,
                    data: { EspecialidadeId: id, PrestadorId: prestadorId },
                    success: function () {
                        location.reload();
                    }


                });
            } else {
                swal("Fique tranquilo o registro não foi apagado!");
            }
        });
}
$(function () {
    var inputs = $('#Nome,#DataNascimento,#CPF,#conselhoId,#NumeroCRM,#TipoPrestadorId,#CEP');

    // Chama a função de verificação quando as entradas forem modificadas
    // Usei o 'keyup', mas 'change' ou 'keydown' são também eventos úteis aqui
    inputs.on('keyup', verificarInputs);

    function verificarInputs() {
        var preenchidos = true;  // assumir que estão preenchidos
        inputs.each(function () {
            // verificar um a um e passar a false se algum falhar
            // no lugar do if pode-se usar alguma função de validação, regex ou outros
            if (!this.value) {
                preenchidos = false;
                // parar o loop, evitando que mais inputs sejam verificados sem necessidade
                return false;
            }
        });
        // Habilite, ou não, o <button>, dependendo da variável:
        $('button').prop('disabled', !preenchidos); // 
    }
})