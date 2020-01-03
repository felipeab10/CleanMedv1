$("input[id*='telefone1']").inputmask({
    mask: ['(99)9999-9999','(99)99999-9999'],
    keepStatic: true
});
$("input[id*='telefone2']").inputmask({
    mask: ['(99)9999-9999','(99)99999-9999' ],
    keepStatic: true
});

$(function () {
    $('.Edit').click(function () {
        var id = $(this).attr("data-Id");

        $.ajax({
            method: "GET",
            url: "/Contatos/PegarContato/" + id,
            data: { ContatoId: id },
            success: function (resposta) {
                if (resposta != null) {
                    $('#ContatoId').val(resposta.contatoId);
                    $('#nome').val(resposta.nome);
                    $('#email').val(resposta.email);
                    $('#telefone1').val(resposta.telefone1);
                    $('#telefone2').val(resposta.telefone2);


                }
            }

        });

    });

    $('.View').click(function () {
        var id = $(this).attr("data-Id");

        $.ajax({
            method: "GET",
            url: "/Contatos/PegarContato/" + id,
            data: { ContatoId: id },
            success: function (resposta) {
                if (resposta != null) {
                    $('#ContatoId').val(resposta.contatoId);
                    $('#nomes').html(resposta.nome);
                    $('#emailview').html(resposta.email);
                    $('#telefone1view').html(resposta.telefone1);
                    $('#telefone2view').html(resposta.telefone2);
                    $('#parentesco').html(resposta.parentesco);


                }
            }

        });

    });
})


function ConfirmarExclusao(id, controller, nome) {
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
               
                var url = "/" + controller + "/Delete";
                $.ajax({
                    method: "POST",
                    url: url,
                    data: { id: id },
                    success: function () {
                        location.reload();
                    }


                });
            } else {
                swal("Fique tranquilo o registro não foi apagado!");
            }
        });
}




