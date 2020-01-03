
    $(document).ready(function () {
        $('.tabs').tabs();
        $('.modal').modal();
        $('.tooltipped').tooltip();
        var cpf = document.getElementById("CPF").value;
        if (cpf != "") {
            document.getElementById("CPF").disabled = false;
            document.getElementById("SemCPF").checked = true;
        } else {
            document.getElementById("CPF").disabled = true;
            document.getElementById("SemCPF").checked = false;
        }
       
       

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
               // console.log(resposta);
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
                    document.getElementById("post").disabled = true
                    NProgress.done();
                } else {
                    document.getElementById("post").disabled = false
                }
            }
        })
    }
   

});





//Validando o cpf, para saber se há algum paciente cadastrado com esse cpf
$("#CPF").focusout(function () {
    var cpf = $(this).val();
    if ($("#PacienteId").val() != null) {
        var pacienteId = document.getElementById("PacienteId").value;
        $.ajax({
            url: '/Pacientes/PacienteExisteCPF/' + $(this).val(),
            data: { CPF: $(this).val(), PacienteId: pacienteId },
            success: function (resposta) {
                //alert("teste");
                if ((resposta != null && resposta != false)) {
                    NProgress.start();
                    Swal.fire({
                        title: '<strong>Paciente já cadastrado</strong>',
                        icon: 'info',
                        html:
                            'Esse CPF está cadastrado para, ' + '<b>' + resposta.nome + '</b>',

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
                    document.getElementById("post").disabled = true
                    NProgress.done();
                } else {
                    document.getElementById("post").disabled = false
                }
            }
        })
    } else {
       
        $.ajax({
            url: '/Pacientes/PacienteExisteCPF/' + $(this).val(),
            data: { CPF: $(this).val() },
            success: function (resposta) {
                
               // console.log(resposta);
                if ((resposta != null && resposta != false)) {
                    NProgress.start();
                    Swal.fire({
                        title: '<strong>Paciente já cadastrado</strong>',
                        icon: 'info',
                        html:
                            'Esse CPF está cadastrado para, ' + '<b>' + resposta.nome + '</b>',


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
                    document.getElementById("post").disabled = true
                    NProgress.done();
                } else {
                    document.getElementById("post").disabled = false
                }

            }
        })
    }

});

function habilitaCPF() {
    var resultado = document.getElementById("SemCPF").checked;
    if (resultado == true) {
       
        document.getElementById("CPF").disabled = false;
    } else {
     
        document.getElementById("CPF").disabled = true;
    }
}

// Mantém os inputs em cache:
var inputs = $('#Nome,#DataNascimento,#Telefone,#CEPS');

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