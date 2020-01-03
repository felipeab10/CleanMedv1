
$(document).ready(function () {
    $('#SetorId').formSelect();
        $('select').formSelect();
        $('textarea').characterCounter();
    $('.tooltipped').tooltip();
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

        $("input[id*='CPF']").inputmask({
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




//Validando Nome + data de nascimento, para saber se há algum usuário cadastrado com esse nome e data de nascimento
$("#DataNascimento").focusout(function () {
    if ($("UsuarioId").val() != null) {
        //Editando usuário
        var DataNascimento = document.getElementById("DataNascimento").value;
        var UsuarioId = document.getElementById("UsuarioId").value;
        var Nome = document.getElementById("Nome").value;
        $.ajax({
            url: '/Usuarios/UsuarioExisteNome/',
            data: { Nome: Nome, DataNascimento: DataNascimento,UsuarioId:UsuarioId },
            success: function (resposta) {
                if ((resposta != null && resposta != true)) {
                    NProgress.start();
                    Swal.fire({
                        title: '<strong>Usuário já Cadastrado</strong>',
                        icon: 'info',
                        html:
                            'Esse nome e data de nascimento está cadastrado para, ' + '<b>' + resposta.nome + '</b>',

                        showCloseButton: true,
                        showCancelButton: true,
                        focusConfirm: false,
                        confirmButtonText:
                            '<a style="color:white;"  href="/Usuarios/Edit?UserName=' + resposta.userName + '">Acessar cadastro</a>',
                        confirmButtonAriaLabel: 'Thumbs up, great!',
                        cancelButtonText:
                            'Fechar',
                        cancelButtonAriaLabel: 'Thumbs down'
                    })

                    NProgress.done();
                }
            }
        })
    } else
    {
       //Adicionando novo Usuário
        var DataNascimento = document.getElementById("DataNascimento").value;
       
        var Nome = document.getElementById("Nome").value;
        $.ajax({
            url: '/Usuarios/UsuarioExisteNome/',
            data: { Nome: Nome, DataNascimento: DataNascimento },
            success: function (resposta) {
                if ((resposta != null && resposta != true)) {
                    NProgress.start();
                    Swal.fire({
                        title: '<strong>Usuário já Cadastrado</strong>',
                        icon: 'info',
                        html:
                            'Esse nome e data de nascimento está cadastrado para, ' + '<b>' + resposta.nome + '</b>',

                        showCloseButton: true,
                        showCancelButton: true,
                        focusConfirm: false,
                        confirmButtonText:
                            '<a style="color:white;"  href="/Usuarios/Edit?UserName=' + resposta.userName + '">Acessar cadastro</a>',
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


//Validando o cpf, para saber se há algum Usuário cadastrado com esse cpf
$("#CPF").focusout(function () {
    var cpf = $(this).val()
    if ($("#UsuarioId").val() != null) {
        var UsuarioId = document.getElementById("UsuarioId").value;
        //Editando usuário
        $.ajax({
            url: '/Usuarios/UsuarioExisteCPF/' + $(this).val(),
            data: { CPF: $(this).val(), UsuarioId: UsuarioId },
            success: function (resposta) {
                if ((resposta != null && resposta != true)) {
                    NProgress.start();

                   
                    Swal.fire({
                        title: '<strong>Usuário já cadastrado</strong>',
                        icon: 'info',
                        html:
                            'Esse CPF está cadastrado para, ' + '<b>' + resposta.nome + '</b>',


                        showCloseButton: true,
                        showCancelButton: true,
                        focusConfirm: false,
                        confirmButtonText:
                            '<a style="color:white;"  href="/Usuarios/Edit?UserName=' + resposta.userName + '">Acessar cadastro</a>',
                        confirmButtonAriaLabel: 'Thumbs up, great!',
                        cancelButtonText:
                            'Fechar',
                        cancelButtonAriaLabel: 'Thumbs down'
                    })
                    NProgress.done();
                   
                    $(document).on("submit", "form", function (e) {
                        e.preventDefault();
                        alert('it works!');
                        return false;
                    });
                }
            }
        })
    } else {
        //Adicionando novo usuário
        $.ajax({
            url: '/Usuarios/UsuarioExisteCPF/' + $(this).val(),
            data: { CPF: $(this).val() },
            success: function (resposta) {
                if ((resposta != null && resposta != true)) {
                    NProgress.start();
                    Swal.fire({
                        title: '<strong>Usuário já cadastrado</strong>',
                        icon: 'info',
                        html:
                            'Esse CPF está cadastrado para, ' + '<b>' + resposta.nome + '</b>',


                        showCloseButton: true,
                        showCancelButton: true,
                        focusConfirm: false,
                        confirmButtonText:
                            '<a style="color:white;"  href="/Usuarios/Edit?UserName=' + resposta.userName + '">Acessar cadastro</a>',
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





//Validando o UserName, para saber se há algum Usuário cadastrado com esse UserName
$("#UserName").focusout(function () {
    var UserName = $(this).val()
    if ($("#UsuarioId").val() != null) {
        var UsuarioId = document.getElementById("UsuarioId").value;
        //Editando usuário
        $.ajax({
            url: '/Usuarios/UsuarioExisteUserName/' + $(this).val(),
            data: { UserName: $(this).val(), UsuarioId: UsuarioId },
            success: function (resposta) {
                if ((resposta != null && resposta != true)) {
                    NProgress.start();
                    Swal.fire({
                        title: '<strong>Usuário já cadastrado</strong>',
                        icon: 'info',
                        html:
                            'Esse UserName está cadastrado para, ' + '<b>' + resposta.nome + '</b>',


                        showCloseButton: true,
                        showCancelButton: true,
                        focusConfirm: false,
                        confirmButtonText:
                            '<a style="color:white;"  href="/Usuarios/Edit?UserName=' + resposta.userName + '">Acessar cadastro</a>',
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
        //Adicionando novo usuário
        $.ajax({
            url: '/Usuarios/UsuarioExisteUserName/' + $(this).val(),
            data: { UserName: $(this).val() },
            success: function (resposta) {
                if ((resposta != null && resposta != true)) {
                    NProgress.start();
                    Swal.fire({
                        title: '<strong>Usuário já cadastrado</strong>',
                        icon: 'info',
                        html:
                            'Esse UserName está cadastrado para, ' + '<b>' + resposta.nome + '</b>',


                        showCloseButton: true,
                        showCancelButton: true,
                        focusConfirm: false,
                        confirmButtonText:
                            '<a style="color:white;"  href="/Usuarios/Edit?UserName=' + resposta.userName + '">Acessar cadastro</a>',
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


//Modal Generica
$(function () {
    $(".nivelAcesso").click(function () {
       //Gerencia nivel de acesso do usuario
        var id = $(this).attr("data-UsuarioId");
       
        $("#modalUsuario").load("/Usuarios/NivelAcesso?UsuarioId=" + id, function () {
           //$("#modalNivelAcesso").modal();

        })
    });

    $(".usuarioEmpresa").click(function () {
        //Gerencia empresas cadastradas para o usuário
        var id = $(this).attr("data-UsuarioId");

        $("#modalUsuario").load("/Usuarios/EmpresaUsuario?UsuarioId=" + id, function () {
            //$("#modalNivelAcesso").modal();

        })
    });
})

//Oculta a modal de usuario por empresa  e abre a modal de adicionar empresa ao usuário
function AddEmpresa(id) {

    $("#modalUsuario").load("/Usuarios/AddEmpresaUsuario?UsuarioId=" + id, function () {
        var elem = document.querySelector('#modalUsuario');
        var instance = M.Modal.init(elem, {
            onOpenStart: function () {

                $('#modalUsuario').attr('style', 'z-index: 1005; display: block; opacity: 1; top: 10%; transform: scaleX(1) scaleY(1);width:400px;');
            }
        });
        instance.open();
    })

    //})

}


//Oculta a modal de lista de permissão do usuário e abre a modal de adicionar permissão ao usuário
function AddPermissao(id) {
  
    $("#modalUsuario").load("/Usuarios/AddNivelUsuario?UsuarioId=" + id, function () {
        var elem = document.querySelector('#modalUsuario');
            var instance = M.Modal.init(elem, {
                onOpenStart: function () {
                    
                    $('#modalUsuario').attr('style','z-index: 1005; display: block; opacity: 1; top: 10%; transform: scaleX(1) scaleY(1);width:400px;');
                }
            });
            instance.open();
        })

    //})

}

//Remove a permissão do usuário
function delPermissaoUsuario(id, controller, nome, usuarioId) {
    
    Swal.fire({
        title: 'Tem certeza ao excluir ?',
        html: nome,
        text: "ao excluir não será possivel recuperar",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#F0F0F0',
        confirmButtonText: 'Sim, Excluir!'
    }).then((result) => {
        if (result.value) {
            var url = "/" + controller + "/RemoveNivelAcessoUsuario/";
            $.ajax({
                method: "POST",
                url: url,
                data: { NivelAcessoId: id, UsuarioId: usuarioId },
                success: function () {

                    location.reload();
                }
            })
        }
    })

}

//Remove uma empresa cadastrada para o usuário
function delEmpresaUsuario(id, controller, nome, usuarioId) {

    Swal.fire({
        title: 'Tem certeza ao excluir ?',
        html: nome,
        text: "ao excluir não será possivel recuperar",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#F0F0F0',
        confirmButtonText: 'Sim, Excluir!'
    }).then((result) => {
        if (result.value) {
            var url = "/" + controller + "/RemoveEmpresaUsuario/";
            $.ajax({
                method: "POST",
                url: url,
                data: { EmpresaId: id, UsuarioId: usuarioId },
                success: function () {

                    location.reload();
                }
            })
        }
    })

}

//Valida o input de confirmação de senha no formulario de edição de usuário
function ValidaConfirmarSenha(obj) {
    var senhaDigitada = document.getElementById("Senha").value;
    if (obj.value != senhaDigitada) {
        swal("Senha não confere", "confirme a senha digitada!", "error");
        $("#senhaDigitada").focus();
    } 
}