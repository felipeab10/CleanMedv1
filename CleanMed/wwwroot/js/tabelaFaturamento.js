
$(document).ready(function () {
    $('.modal').modal();
    $('select').formSelect({ dropdownOptions: { container: document.body } });
        $('textarea').characterCounter();
    $('.tooltipped').tooltip();
   

    });
$("input[id*='Name']").inputmask({
    //regex: "[a-z-áàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ() 0-9]*" ,
    casing: "upper"
});
$("input[id*='Descricao']").inputmask({
    //regex: "[a-z-áàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ() ]*",
    casing: "upper"
});


function descricaoTabelaFaturamentoADD(obj) {
    var valor = obj.value;
    if (valor != null || valor != "undefined" || valor != "") {
     
        document.getElementById("post").disabled = false;
    } else {
        Alert("sem valor");
        document.getElementById("post").disabled = true;
    }
}
function descricaoTabelaFaturamentoEdit(obj) {
    var valor = obj.value;
    if (valor == null || valor == "undefined" || valor == "") {

        document.getElementById("post").disabled = true;
    } else {
       
        document.getElementById("post").disabled = false;
    }
}

//Pega o conselho e envia a resposta para a modal
$(function () {
    $(".btnEdit").click(function () {
        //editar nível de acessso
        var ConvenioId = $(this).attr("data-ConvenioId");
        var id = $(this).attr("data-tabelaFaturamentoId");
       
        $("#tabelaFaturamentoAdd").load("/TabelaFaturamentos/Edit?id=" + id + "&" + "ConvenioId=" + ConvenioId, function () {
            var elem = document.querySelector('#modal');
            var instance = M.Modal.init(elem, {
                onOpenStart: function () {
                    $('#tabelaFaturamentoAdd').attr('style', 'z-index: 1005; display: block; opacity: 1; top: 10%; transform: scaleX(1) scaleY(1);width:100px !important;');
                },
               
                onCloseEnd: function () {

                    location.reload();
                }
            });
           // instance.open();
        })
    });

    $(".btnAdd").click(function () {
        var id = $(this).attr("data-ConvenioId");
       
        $("#tabelaFaturamentoAdd").load("/TabelaFaturamentos/Create/" + id, function () {
            var elem = document.querySelector('#tabelaFaturamentoAdd');
            var instance = M.Modal.init(elem, {
                onOpenStart: function () {
                    $('#tabelaFaturamentoAdd').attr('style', 'z-index: 1005; display: block; opacity: 1; top: 10%; transform: scaleX(1) scaleY(1);width:500px;');
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
