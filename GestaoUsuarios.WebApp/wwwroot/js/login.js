var LoginModulo = (function () {

    var _login = function () {

        var data = {
            cpf: $("#input-cpf").cleanVal(),
            senha: $("#input-senha").val()
        };

        //Validações
        var campos = [
            {
                obj: $("#input-cpf"),
                valor: data.cpf,
                mensagem: "Preencha o CPF"
            },
            {
                obj: $("#input-senha"),
                valor: data.senha,
                mensagem: "Preencha a senha"
            }
        ];


        let validacao = true;

        $.each(campos, function (index, campo) {

            if (isNullOrEmpty(campo.valor) || campo.valor == 0) {

                campo.obj.focus();

                PNotify.alert({
                    text: campo.mensagem,
                    type: 'notice'
                });

                validacao = false;
                return;
            }

        });

        if (!validacao) {
            return;
        }

        //Exibe o loading no botão
        var botao = $("#btn-login");
        botao.prop("disabled", true);
        botao.find(".spinner-grow").show();


        $.ajax({
            url: caminhoBase + "Login/Login",
            data: data,
            type: "POST"
        })
            .done(function (data) {

                if (data.hasOwnProperty("erro")) {

                    PNotify.alert({
                        text: data.erro,
                        type: 'error'
                    });

                    botao.prop("disabled", false);
                    botao.find(".spinner-grow").hide();

                } else {

                    window.location.href = caminhoBase + "Home/Index";

                }
            })
            .fail(function () {

                PNotify.alert({
                    text: "Erro ao realizar a comunicação com o servidor",
                    type: 'error'
                });

                botao.prop("disabled", false);
                botao.find(".spinner-grow").hide();

            });

    }

    var _inicializarMascara = function () {

        //Inicializa máscaras
        $("#input-cpf").mask('000.000.000-00');

    };

    var inicializar = function () {

        //Eventos
        $("form").on("submit", function (e) { e.preventDefault() });
        $("body").on("click", "#btn-login", _login);

        //mascara
        _inicializarMascara();

    }

    return {

        inicializar: inicializar

    }

});

$(function () {

    var modulo = new LoginModulo();
    modulo.inicializar();
    
});
