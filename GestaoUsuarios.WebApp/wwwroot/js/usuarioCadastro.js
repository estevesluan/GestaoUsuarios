var UsuarioModulo = (function () {

    var _novoCadastro = function () {

        $("#form-usuario")[0].reset();
        $("#input-id").val("0");
        $("#btn-excluir").hide();
        $("#img-foto").removeAttr("src");
        $("#input-nome").val("");
        $("#input-mae").val("");
        $("#input-pai").val("");
        $("#input-sobrenome").val("");
        $("#input-cpf").val("");
        $("#input-senha").val("");
        $("#input-senhaConfirma").val("");
        $("#input-telefone").val("");
        $("#input-email").val("");
        $("#input-parentesco").val("");
        $("#input-parentesco").val("");
        $("#input-cep").val("");
        $("#input-numero-endereco").val("");

        var campos = $("input.form-control");
        $.each(campos, function (index, campo) {
            $(campo).removeClass("is-valid");
            $(campo).removeClass("is-invalid");
        });

        _limparEndereco();

    }

    var _salvar = function (e) {

        var data = new FormData();

        data.append("id", $("#input-id").val());
        data.append("nome", $("#input-nome").val());
        data.append("sobrenome", $("#input-sobrenome").val());
        data.append("nomeMae", $("#input-mae").val());
        data.append("nomePai", $("#input-pai").val());
        data.append("nascimento", $("#input-nascimento").val());
        data.append("cpf", $("#input-cpf").cleanVal());
        data.append("senha", $("#input-senha").val());
        data.append("senhaConfirma", $("#input-senha-confirma").val());
        data.append("telefone", $("#input-telefone").cleanVal());
        data.append("email", $("#input-email").val());
        data.append("parentesco", $("#input-nome").val());
        data.append("foto", $('#input-foto')[0].files[0]);
        data.append("cep", $("#input-cep").cleanVal());
        data.append("numeroEndereco", $("#input-numero-endereco").val());

        //Validações
        var campos = [
            {
                obj: $("#input-nome"),
                valor: data.get("nome"),
                mensagem: "Preencha o nome"
            },
            {
                obj: $("#input-mae"),
                valor: data.get("nomeMae"),
                mensagem: "Preencha o nome da mãe"
            },
            {
                obj: $("#input-sobrenome"),
                valor: data.get("sobrenome"),
                mensagem: "Preencha o sobrenome"
            },
            {
                obj: $("#input-nascimento"),
                valor: data.get("nascimento"),
                mensagem: "Preencha a data de nascimento"
            },
            {
                obj: $("#input-cpf"),
                valor: data.get("cpf"),
                mensagem: "Preencha o CPF"
            },
            {
                obj: $("#input-senha"),
                valor: data.get("senha"),
                mensagem: "Preencha a senha"
            },
            {
                obj: $("#input-senha-confirma"),
                valor: data.get("senha"),
                mensagem: "Preencha a confirmação da senha"
            },
            {
                obj: $("#input-telefone"),
                valor: data.get("telefone"),
                mensagem: "Preencha o telefone"
            },
            {
                obj: $("#input-cep"),
                valor: data.get("cep"),
                mensagem: "Preencha o cep"
            }
        ];

        let validacao = true;

        $.each(campos, function (index, campo) {

            if (isNullOrEmpty(campo.valor) || campo.valor == 0) {

                campo.obj.focus();

                campo.obj.removeClass("is-valid");
                campo.obj.addClass("is-invalid");

                validacao = false;
                return;
            }

            campo.obj.removeClass("is-invalid");
            campo.obj.addClass("is-valid");
        });

        if (!validacao) {
            PNotify.alert({
                text: "Preencha os campos obrigatórios",
                type: 'notice'
            });
            return;
        }

        //Validação de datas
        var senha = data.get("senha");
        var senhaConfirma = data.get("senhaConfirma");

        if (senha !== senhaConfirma) {
            var campo = $("#input-senha");

            campo.focus();
            campo.removeClass("is-valid");
            campo.addClass("is-invalid");

            PNotify.alert({
                text: "Senhas não conferem",
                type: 'notice'
            });

            return;
        }

        //Validação - Datas
        var dateDataNascimento = createDate(data.get("nascimento"));

        if (dateDataNascimento == null || dateDataNascimento == "Invalid Date") {

            var campo = $("#input-nascimento");

            campo.focus();
            campo.removeClass("is-valid");
            campo.addClass("is-invalid");

            PNotify.alert({
                text: "Data de nascimento inválida. Preencha no formato dd/mm/aaaa",
                type: 'notice'
            });

            return;
        }

        if (dateDataNascimento > new Date()) {

            var campo = $("#input-nascimento");

            campo.focus();
            campo.removeClass("is-valid");
            campo.addClass("is-invalid");

            PNotify.alert({
                text: "A data de nascimento não pode ser maior que a data atual",
                type: 'notice'
            });

            return;
        }

        //Validação - CPF
        if (!validarCPF($("#input-cpf").cleanVal())) {

            var campo = $("#input-cpf");

            campo.focus();
            campo.removeClass("is-valid");
            campo.addClass("is-invalid");

            PNotify.alert({
                text: "Número de CPF inválido",
                type: 'notice'
            });

            return;
        }

        //Validação - E-mail
        var email = data.get("email");
        if (!isNullOrEmpty(email) && !validarEmail(email)) {

            var campo = $("#input-email");

            campo.focus();
            campo.removeClass("is-valid");
            campo.addClass("is-invalid");

            PNotify.alert({
                text: "E-mail inválido",
                type: 'notice'
            });

            return;
        }

        $.ajax({

            url: caminhoBase + "Usuario/Cadastro",
            type: "POST",
            data: data,
            processData: false,
            contentType: false

        })
            .done(function (data) {

                if (data.hasOwnProperty("erro")) {

                    if (data.erro.toLowerCase().lastIndexOf("cpf") >= 0) {
                        var campo = $("#input-cpf");

                        campo.focus();
                        campo.removeClass("is-valid");
                        campo.addClass("is-invalid");
                    }

                    PNotify.alert({
                        text: data.erro,
                        type: 'error'
                    });

                } else {

                    PNotify.alert({
                        text: "Cadastro realizado!",
                        type: 'success'
                    });
                    _novoCadastro();

                }

            })
            .fail(function () {

                PNotify.alert({
                    text: "Erro ao realizar a comunicação com o servidor",
                    type: 'error'
                });

            });

    }

    var _excluir = function () {

        var id = $("#input-id").val();

        $.ajax({
            url: caminhoBase + "Usuario/Remover/" + id,
            type: "POST",
        })
            .done(function (data) {

                if (data.hasOwnProperty("erro")) {

                    PNotify.alert({
                        text: data.erro,
                        type: 'error'
                    });

                } else {

                    PNotify.alert({
                        text: "Removido com sucesso!",
                        type: 'success'
                    });
                    _novoCadastro();

                }

            })
            .fail(function () {

                PNotify.alert({
                    text: "Erro ao realizar a comunicação com o servidor",
                    type: 'error'
                });

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

                    $("#btn-excluir").hide();

                } else {

                    $("#img-foto").attr("src", "data:image/jpg;base64, " + data.foto);
                    $("#input-nome").val(data.nome);
                    $("#input-sobrenome").val(data.sobrenome);
                    $("#input-mae").val(data.nomeMae);
                    $("#input-pai").val(data.nomePai);
                    $("#input-nascimento").val(moment.utc(data.DataNascimento).format('DD/MM/YYYY')).trigger('input');
                    $("#input-cpf").val(data.cpf).trigger('input');
                    $("#input-senha").val(data.senha);
                    $("#input-senha-confirma").val(data.senha);
                    $("#input-telefone").val(data.telefone).trigger('input');
                    $("#input-email").val(data.email);
                    $("#input-parentesco").val(data.parentesco);
                    $("#input-cep").val(data.cep).trigger('input').trigger("change");
                    $("#input-numero-endereco").val(data.numeroEndereco);

                    $("#btn-excluir").show();

                }
            })
            .fail(function () {

                PNotify.alert({
                    text: "Erro ao realizar a comunicação com o servidor",
                    type: 'error'
                });
                $("#btn-excluir").hide();

            });

    }

    var _carregarEndereco = function (e) {

        var inputCep = $(e);

        if (inputCep.cleanVal().length != 8) {
            limparEndereco();
            return;
        }

        //Exibe o loading
        inputCep.prop("disabled", true);
        $("#span-spinner-cep").show();

        $.ajax({

            url: caminhoBase + "Usuario/CarregarEndereco?cep=" + inputCep.cleanVal(),
            type: "GET"

        })
            .done(function (data) {

                if (!data.hasOwnProperty("erro")) {

                    $("#input-logradouro").val(data.logradouro);
                    $("#input-complemento").val(data.complemento);
                    $("#input-bairro").val(data.bairro);
                    $("#input-localidade").val(data.localidade);
                    $("#input-uf").val(data.uf);
                    $("#input-unidade").val(data.unidade);
                    $("#input-ibge").val(data.ibge);
                    $("#input-gia").val(data.gia);

                } else {

                    //Exibe o erro
                    PNotify.alert({
                        text: data.erro,
                        type: 'error'
                    });
                    //Limpa a tela
                    limparEndereco();

                }

                //Esconde o loading
                inputCep.prop("disabled", false);
                $("#span-spinner-cep").hide();

            })
            .fail(function () {

                //Esconde o loading
                inputCep.prop("disabled", false);
                $("#span-spinner-cep").hide();

                //Exibe o erro
                PNotify.alert({
                    text: "Erro ao realizar a comunicação com o servidor",
                    type: 'error'
                });

            });

    }

    var _atualizarFoto = function (e) {

        var input = e;
        var url = $(e).val();
        var ext = url.substring(url.lastIndexOf('.') + 1).toLowerCase();
        if (input.files && input.files[0] && (ext == "png")) {

            var reader = new FileReader();
            reader.onload = function (e) {

                $('#img-foto').attr('src', e.target.result);

            }
            reader.readAsDataURL(input.files[0]);

        }

    }

    var _limparEndereco = function () {

        $("#input-logradouro").val("");
        $("#input-complemento").val("");
        $("#input-bairro").val("");
        $("#input-localidade").val("");
        $("#input-uf").val("");
        $("#input-unidade").val("");
        $("#input-ibge").val("");
        $("#input-gia").val("");

    }

    var _inicializarMascara = function () {

        //Inicializa máscaras
        $("#input-cpf").mask('000.000.000-00');
        $("#input-cep").mask('00000-000');
        $("#input-telefone").mask('(00) 00000-0000');
        $("#input-nascimento").mask('00/00/0000');
        $("#input-numero-endereco").mask('000000');

    };

    var inicializar = function () {

        _inicializarMascara();

        //Inicializa os eventos
        $("form").on("submit", function (e) { e.preventDefault() });
        $("body").on("click", "#btn-novo", _novoCadastro);
        $("body").on("click", "#btn-salvar", _salvar);
        $("body").on("click", "#btn-excluir", _excluir);
        $('#input-cep').on("change", function (e) { _carregarEndereco(e.currentTarget) });
        $('#input-foto').on("change", function (e) { _atualizarFoto(e.currentTarget) });

        //Verifica edição de usuário
        var id = $("#input-id").val();

        if (id != null && id > 0) {
            _carregarUsuario(id);
        }

    }

    return {

        inicializar: inicializar

    }

});


$(function () {

    var modulo = new UsuarioModulo();
    modulo.inicializar();

});
