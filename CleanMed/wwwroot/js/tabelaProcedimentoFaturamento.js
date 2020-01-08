
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
        var TabelaFatuProcedimentoId = $(this).attr("data-TabelaFatuProcedimentoId");
       
        $("#tabelaProcedimentoAdd").load("/TabelaFatuProcedimentos/Edit?id=" + TabelaFatuProcedimentoId + "&" + "ConvenioId=" + ConvenioId, function () {
            var elem = document.querySelector('#modal');
            var instance = M.Modal.init(elem, {
                onOpenStart: function () {
                    $('#tabelaProcedimentoAdd').attr('style', 'z-index: 1005; display: block; opacity: 1; top: 10%; transform: scaleX(1) scaleY(1);width:100px !important;');
                },
               
                onCloseEnd: function () {

                    location.reload();
                }
            });
           // instance.open();
        })
    });

    $(".btnAdd").click(function () {
        var ConvenioId = $(this).attr("data-ConvenioId");
        var TabelaFaturamentoId = $(this).attr("data-TabelaFaturamentoId");

        $("#tabelaProcedimentoAdd").load("/TabelaFatuProcedimentos/Create?TabelaFaturamentoId=" + TabelaFaturamentoId + "&" + "ConvenioId=" + ConvenioId, function () {
            var elem = document.querySelector('#tabelaFaturamentoAdd');
            var instance = M.Modal.init(elem, {
                onOpenStart: function () {
                    $('#tabelaProcedimentoAdd').attr('style', 'z-index: 1005; display: block; opacity: 1; top: 10%; transform: scaleX(1) scaleY(1);width:500px;');
                },
                onCloseEnd: function () {
                  
                    location.reload();
                }
            });
           // instance.open();
        })
    });
})
$('.select2-procedimento').select2({
    //dropdownParent: $('#modalAgendamento'),
    minimumInputLength: 2,
    placeholder: "",
    allowClear: true,
    language: {
        inputTooShort: function () {
            return "Insira 2 ou mais caracteres";
        },
        noResults: function () {
            return 'Sem resultados ';
        },
    },
    escapeMarkup: function (markup) {
        return markup;
    },



})
