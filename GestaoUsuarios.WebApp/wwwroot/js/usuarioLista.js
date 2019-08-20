var UsuarioListaModulo = (function () {
    var _numeroPagina;
    var _paginacao;

    var _novaPesquisa = function(){

        _numeroPagina = 1;
        _carregarUsuarios();

    }

    var _editarUsuario = function (e) {
        var id = $(e).attr("data-id-usuario");
        window.location.href = caminhoBase + "Usuario/Cadastro/" + id;
    }

    var _detalhesUsuario = function (e) {
        var id = $(e).attr("data-id-usuario");
        window.location.href = caminhoBase + "Usuario/Detalhes/" + id;
    }

    var _carregarUsuarios = function () {

        //lista de usuarios
        var lista = $("#div-lista-usuarios");
        //Texto da pesquisa
        var pesquisa = $("#input-pesquisa-usuario").val();
        //Número de usuarios por página
        var usuariosPorPagina = $("#select-usuarios-por-pagina option:selected").val();

        //Exibe o loading no botão
        var botao = $("#btn-pesquisar-usuario");
        botao.prop("disabled", true);
        botao.find(".spinner-border").show();

        $.ajax({
            url: caminhoBase + "Usuario/ListaUsuariosPagina/",
            data: { pesquisa: pesquisa, numeroPagina: _numeroPagina, numeroItensPorPagina: usuariosPorPagina },
            type: "GET"
        })
            .done(function (data) {

                if (data.usuarios !== undefined && data.usuarios !== null && data.usuarios.length > 0) {
                    //limpar a lista de usuarios
                    lista.empty();
                    //Atualizar a paginação com os novos usuarios
                    _paginacao.twbsPagination('destroy');
                    _paginacao.twbsPagination({
                        startPage: _numeroPagina,
                        totalPages: data.total,
                        first: 'Início',
                        prev: '<',
                        next: '>',
                        last: 'Fim'
                    }).on('page', function (evt, page) {
                        _numeroPagina = page;
                        _carregarUsuarios();
                    });
                    //Adicionar ao html todos usuarios retornados
                    $.each(data.usuarios, (index, valor) => {

                        //Cria elementos da pagina para adicionar ao html
                        var img = $(document.createElement("img"));
                        var divUsuario = $(document.createElement("div"));
                        var divCard = $(document.createElement("div"));
                        var divCardBody = $(document.createElement("div"));
                        var pCard = $(document.createElement("p"));
                        var divCardBtn = $(document.createElement("div"));
                        var divCardBtnGroup = $(document.createElement("div"));

                        var btnDetalhes = $(document.createElement("button"));
                        var btnEditar = $(document.createElement("button"));

                        if (valor.foto == null || valor.foto == undefined) {
                            img.attr("src", "/images/img-usuario.png");
                        } else {
                            img.attr("src", "data:image/jpg;base64, " + valor.foto);
                        }

                        img.addClass("card-img-top");
                        divUsuario.addClass("col-md-4");
                        divCard.addClass("card mb-4 shadow-sm");
                        divCardBody.addClass("card-body");
                        pCard.html(valor.nome + " " + valor.sobrenome).addClass("card-text");
                        divCardBtn.addClass("d-flex justify-content-between align-items-center");
                        divCardBtnGroup.addClass("btn-group");

                        btnDetalhes.html("Detalhes").attr("type", "button").attr("data-id-usuario", valor.id).addClass("btn btn-sm btn-outline-secondary btn-detalhes-usuario");
                        btnEditar.html("Editar").attr("type", "button").attr("data-id-usuario", valor.id).addClass("btn btn-sm btn-outline-secondary btn-editar-usuario");

                        divCardBtnGroup.append(btnEditar);
                        divCardBtnGroup.append(btnDetalhes);
                        divCardBtn.append(divCardBtnGroup);
                        divCardBody.append(pCard);
                        divCardBody.append(divCardBtnGroup);
                        divCard.append(img);
                        divCard.append(divCardBody);
                        divUsuario.append(divCard);

                        //Adicionar o usuario na lista da página
                        lista.append(divUsuario);
                    });
                }
                //Esconde o loading do botão
                botao.prop("disabled", false);
                botao.find(".spinner-border").hide();
            })
            .fail(function () {
                //Esconde o loading do botão
                botao.prop("disabled", false);
                botao.find(".spinner-border").hide();
            });

    }

    var _iniciarHub = function () {

        //Configura conexao com o hub de usuários
        var connection = new signalR.HubConnectionBuilder()
            .withUrl("/usuarioHub")
            .configureLogging(signalR.LogLevel.Information)
            .build();

        connection.start().then(function () { });

        connection.on("AtualizarLista", function () {
            _carregarUsuarios();
        });

    }

    var inicializar = function () {

        //iniciar contagem
        _numerooPagina = 1
        _paginacao = $('#ul-paginacao-usuarios');

        //Eventos
        $("body").on("click", "#btn-pesquisar-usuario", _novaPesquisa);
        $("body").on("click", ".btn-editar-usuario", function (e) { _editarUsuario(e.currentTarget) });
        $("body").on("click", ".btn-detalhes-usuario", function (e) { _detalhesUsuario(e.currentTarget) });
        $('#select-usuarios-por-pagina').on("change", _novaPesquisa );

        //iniciar hub
        _iniciarHub();

        //iniciar página
        _novaPesquisa();

    }

    return {
        inicializar: inicializar
    }

});

$(function () {

    var modulo = new UsuarioListaModulo();
    modulo.inicializar();

});