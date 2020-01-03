
    $(document).ready(function () {
    

    });

        $("input[id*='Telefone']").inputmask({
        mask: ['(99)9999-9999', '(99)9 9999-9999'],
    keepStatic: true
        });
$("input[id*='CEP']").inputmask({
    mask: ['99999-999'],
    keepStatic: true
});
     
function CarregarImagem(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $("#arquivo").attr('src', e.target.result).width(100).height(100);
            $("#arquivo").show();
        }
    }
    reader.readAsDataURL(input.files[0]);
}   

//Buscando CEP primeiramente no banco de dados caso não tenha registro ele busca no viacep
         $("#CEPS").click(function () {
             var ceps = $("#CEP").val()
        $.ajax({
            url: '/Pacientes/LocalizarCEP/' + ceps,
            data: {CEP: ceps },
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

function validarSenha(obj) {
    var Senha = document.getElementById("Senha").value;
    if (Senha == "") {
        alert("Campo senha vazio ou nulo");
    }
    if (obj.value != Senha) {
        alert("Senha digitada não confere");
    }
}

