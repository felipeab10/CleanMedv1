
$(document).ready(function () {
    $('.modal').modal();
        $('select').formSelect();
        $('textarea').characterCounter();
        $('[data-toggle="tooltip"]').tooltip()

    });
$("input[id*='descricao']").inputmask({
    regex: "[a-z-áàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ() ]*" ,
    casing: "upper"
});
$("input[id*='Descricao']").inputmask({
    regex: "[a-z-áàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ() ]*",
    casing: "upper"
});


//Pega o conselho e envia a resposta para a modal
$(function () {
    $(".btnEdit").click(function () {
        var id = $(this).attr("data-TipoPrestadorId");
       
      

        $.ajax({
            url: "/TipoPrestadores/LocalizarTipoPrestador/",
            data: { id: id },
            success: function (resposta) {
                $("#Descricao").val(resposta.descricao);
                $("#TipoPrestadorId").val(resposta.tipoPrestadorId);
                $("#Id").val(resposta.tipoPrestadorId);
            }

        });
    });
})

