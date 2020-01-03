$("input[id*='telefone1']").inputmask({
    mask: ['(99)9999-9999','(99)99999-9999'],
    keepStatic: true
});
$("input[id*='telefone2']").inputmask({
    mask: ['(99)9999-9999','(99)99999-9999' ],
    keepStatic: true
});

$("input[id*='Validade']").inputmask({
    mask: ['99/99/9999'],
    keepStatic: true
});

function dataAtualFormatada(date) {
    var data = new Date(date),
        dia = data.getDate().toString(),
        diaF = (dia.length == 1) ? '0' + dia : dia,
        mes = (data.getMonth() + 1).toString(), //+1 pois no getMonth Janeiro começa com zero.
        mesF = (mes.length == 1) ? '0' + mes : mes,
        anoF = data.getFullYear();
    return diaF + "/" + mesF + "/" + anoF;
}



$(function () {
    $('.Edit').click(function () {
        var id = $(this).attr("data-Id");

        $.ajax({
            method: "GET",
            url: "/Convenios/GetCartaoPaciente/" + id,
            data: { CartaoConvenioId: id },
            success: function (resposta) {
                if (resposta != null) {
                    $('#CartaoConvenioId').val(resposta.cartaoConvenioId);
                   // $('#validade').val(resposta.validade);
                    $('#NumeroCarteira').val(resposta.numeroCarteira);
                    $('#ConvenioId').val(resposta.convenioId);
                    $('#Validade').val(dataAtualFormatada(resposta.validade));


                }
            }

        });

    });

    $('.View').click(function () {
        var id = $(this).attr("data-Id");

        $.ajax({
            method: "GET",
            url: "/Convenios/GetCartaoPaciente/" + id,
            data: { CartaoConvenioId: id },
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




