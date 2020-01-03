
$(document).ready(function () {
    $('.modal').modal();
        $('select').formSelect();
        $('textarea').characterCounter();
    $('[data-toggle="tooltip"]').tooltip()


    });
$("input[id*='Name']").inputmask({
    //regex: "[a-z-áàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ() 0-9]*" ,
    casing: "upper"
});
$("input[id*='Descricao']").inputmask({
    //regex: "[a-z-áàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ() ]*",
    casing: "upper"
});


//Pega o conselho e envia a resposta para a modal
$(function () {
    $(".btnEdit").click(function () {
        //editar nível de acessso
        var id = $(this).attr("data-SetorId");
     
        $("#modal").load("/Setores/Edit/" + id, function () {
            var elem = document.querySelector('#modal');
            var instance = M.Modal.init(elem, {
                onOpenStart: function () {
                    $('#modal').attr('style', 'z-index: 1005; display: block; opacity: 1; top: 10%; transform: scaleX(1) scaleY(1);width:500px;');
                },
               
                onCloseEnd: function () {

                    location.reload();
                }
            });
            instance.open();
        })
    });

    $(".btnAdd").click(function () {
        $("#modal").load("/Setores/Create", function () {
            var elem = document.querySelector('#modal');
            var instance = M.Modal.init(elem, {
                onOpenStart: function () {
                    $('#modal').attr('style', 'z-index: 1005; display: block; opacity: 1; top: 10%; transform: scaleX(1) scaleY(1);width:500px;');
                },
                onCloseEnd: function () {
                  
                    location.reload();
                }
            });
            instance.open();
        })
    });
})
function excluirNivelAcesso(id, controller, nome) {
    swal({
        title: "Tem certeza disso?",
        text: "Uma vez excluido, não será possivel recuperar ",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: "/NiveisAcessos/Delete/" ,
                    data: { id: id },
                    success: function () {
                        location.reload();
                    }
                })
            } else {
                swal("Fique tranquilo, registro não foi excluido");
            }
        });
}
