
$(document).ready(function () {
    $('.modal').modal();
        $('select').formSelect();
        $('textarea').characterCounter();
        $('[data-toggle="tooltip"]').tooltip()

    });
$("input[id*='descricao']").inputmask({
    regex: "[a-z-áàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ ]*" ,
    casing: "upper"
});
$("input[id*='Descricao']").inputmask({
    regex: "[a-z-áàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ ]*",
    casing: "upper"
});


//Pega o conselho e envia a resposta para a modal
$(function () {
    $(".btnEdit").click(function () {
        var id = $(this).attr("data-ConselhoId");
      
      

        $.ajax({
            url: "/Conselhos/LocalizarConselho",
            data: { id: id },
            success: function (resposta) {
                $("#Descricao").val(resposta.descricao);
                $("#ConselhoId").val(resposta.conselhoId);
                $("#Id").val(resposta.conselhoId);
            }

        });
    });
})

