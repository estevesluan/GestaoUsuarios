var UsuarioDetalhesModulo = (function () {

    var _carregarEndereco = function (cep, numero) {

        $.ajax({
            url: caminhoBase + "Usuario/CarregarEndereco?cep=" + cep,
            type: "GET"
        })
            .done(function (data) {

                if (!data.hasOwnProperty("erro")) {

                    var endereco = "";

                    if (!isNullOrEmpty(data.logradouro)) {
                        endereco += data.logradouro + " ";
                    }

                    if (!isNullOrEmpty(data.complemento)) {
                        endereco += data.complemento + ", ";
                    }

                    endereco += numero + ", ";

                    if (!isNullOrEmpty(data.bairro)) {
                        endereco += data.bairro + ", ";
                    }

                    if (!isNullOrEmpty(data.localidade)) {
                        endereco += data.localidade + " - ";
                    }

                    if (!isNullOrEmpty(data.uf)) {
                        endereco += data.uf + " - ";
                    }

                    if (!isNullOrEmpty(data.unidade)) {
                        endereco += data.unidade;
                    }

                    if (!isNullOrEmpty(data.ibge)) {
                        endereco += "IBGE: " + data.ibge + " ";
                    }

                    if (!isNullOrEmpty(data.gia)) {
                        endereco += "GIA: " + data.gia + " ";
                    }

                    $("#span-endereco").text(endereco);

                } else {

                    $("#span-endereco").text("Endereço não encontrado");

                }

            })
            .fail(function () {

                $("#span-endereco").text("Endereço não encontrado");

            });

    }

    var _carregarUsuario = function (id) {

        $.ajax({

            url: caminhoBase + "Usuario/Dados/" + id,
            type: "GET"

        })
            .done(function (data) {

                if (data.hasOwnProperty("erro")) {

                    PNotify.alert({
                        text: data.erro,
                        type: 'error'
                    });

                } else {

                    if (data.foto == null || data.foto == undefined) {
                        $("#img-foto").attr("src", "/images/img-usuario.png");
                    } else {
                        $("#img-foto").attr("src", "data:image/jpg;base64, " + data.foto);
                    }
                   
                    $("#span-nome").html(data.nome + " " + data.sobrenome);
                    $("#span-mae").html(data.nomeMae);
                    $("#span-pai").html(data.nomePai);
                    $("#span-nascimento").html(moment.utc(data.DataNascimento).format('DD/MM/YYYY')).trigger('input');
                    $("#span-cpf").html(data.cpf).trigger('input');
                    $("#span-telefone").html(data.telefone).trigger('input');
                    $("#span-email").html(data.email);
                    $("#span-parentesco").html(data.parentesco);
                    $("#span-cep").html(data.cep).trigger('input');

                    $("#a-cadastro").attr("href", caminhoBase + "Usuario/Cadastro/" + data.id);

                    _carregarEndereco(data.cep, data.numeroEndereco);

                }

            })
            .fail(function () {

                PNotify.alert({
                    text: "Erro ao realizar a comunicação com o servidor",
                    type: 'error'
                });

            });

    }

    var _inicializarMascara = function () {

        //Inicializa máscaras
        $("#span-nascimento").mask('00/00/0000');
        $("#span-cpf").mask('000.000.000-00');
        $("#span-cep").mask('00000-000');
        $("#span-telefone").mask('(00) 00000-0000');

    };

    var inicializar = function () {

        //Carregar dados do usuário
        var id = $("#input-id").val();

        if (id != null && id > 0) {
            _carregarUsuario(id);
        }

        _inicializarMascara();

    }

    return {
        inicializar: inicializar
    }

});

$(function () {

    var modulo = new UsuarioDetalhesModulo();
    modulo.inicializar();
    
});
