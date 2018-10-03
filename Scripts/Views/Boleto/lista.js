/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {

    if (NotaAzul.Boleto == null) {
        NotaAzul.Boleto = {};
    }

    NotaAzul.Boleto.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowBoleto = null,
            _windowBoletoBaixa = null,
            _windowBoletoUpdate = null,
            _windowRem = null,
            _permissoes = null;

        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrirBaixa = function (obj) {
            _windowBoletoBaixa.apply({
                object: 'Boleto',                
                ajax: { onlyJson: true },
                fnBeforeLoad: NotaAzul.Boleto.Dados.Novo
            }).load().show({ animate: true });
        };

        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrirRem = function (obj) {
            _windowRem.apply({
                object: 'Boleto',
                ajax: { onlyJson: true },
                fnBeforeLoad: NotaAzul.Boleto.DadosRem.Novo
            }).load().show({ animate: true });
        };

        var _abrirUpdate = function () {
            var obj = NotaAzul.Boleto.Lista.Grid().getSelected();

            _windowBoletoUpdate.apply({
                object: 'Boleto',
                url: rootDir + "Boleto/Carregar/" + obj[0].object.Id,
                ajax: { onlyJson: true }
            }).load().show({ animate: true });
        };

        var _carregarJanelaUpdate = function () {
            _windowBoletoUpdate = new Prion.Window({
                url: rootDir + "Boleto/ViewDadosBoleto/",
                id: "popupBoletoUpdate",
                height: 300,
                width: 640,
                title: { text: "Boleto" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.Bairro.Lista.Grid().load();
                    }
                },
                buttons: {
                    show: (!_permissoes.onlyView), // se onlyView for false, significa então que os botões deverão ser exibidos
                    buttons: [
                        {
                            text: "Salvar",
                            className: Prion.settings.ClassName.button,
                            typeButton: "save", // usado para controle interno
                            click: function (win) {
                                NotaAzul.Boleto.DadosBoleto.Salvar(win);
                            }
                        }
                    ]
                },
                success: function () {
                    // habilita o botão de insert
                    NotaAzul.Boleto.Lista.Grid().enableButton("insert");
                }
            });
        };

        /**
       * @descrição: Carrega o conteúdo do html Boleto/ViewDados em background, acelerando o futuro carregamento
       * @return: void
       **/
        var _carregarJanelaBoleto = function () {
            _windowBoletoBaixa = new Prion.Window({
                url: rootDir + "Boleto/ViewDados/",
                id: "popupBoleto",
                height: 200,
                width: 570,
                title: { text: "Ler arquivo de retorno" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.Boleto.Lista.Grid().load();
                    }
                },
                buttons: {
                    show: (!_permissoes.onlyView), // se onlyView for false, significa então que os botões deverão ser exibidos
                    buttons: [
                        {
                            text: "Salvar",
                            className: Prion.settings.ClassName.button,
                            typeButton: "save", // usado para controle interno
                            click: function (win) {
                                NotaAzul.Boleto.Dados.Salvar(win);
                            }
                        },
                        {
                            text: "Limpar", className: Prion.settings.ClassName.buttonReset, type: "reset",
                            click: function (win) {
                                NotaAzul.Boleto.Dados.Novo();
                            }
                        }
                    ]
                },
                success: function () {
                }
            });
        };

        /**
      * @descrição: Carrega o conteúdo do html Boleto/ViewDados em background, acelerando o futuro carregamento
      * @return: void
      **/
        var _carregarJanelaRem = function () {
            _windowRem = new Prion.Window({
                url: rootDir + "Boleto/ViewRemessa/",
                id: "popupRemessa",
                height: 200,
                width: 570,
                title: { text: "Gerar arquivo remessa mensal" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.Boleto.Lista.Grid().load();
                    }
                },
                buttons: {
                    show: (!_permissoes.onlyView), // se onlyView for false, significa então que os botões deverão ser exibidos
                    buttons: [
                        {
                            text: "Salvar",
                            className: Prion.settings.ClassName.button,
                            typeButton: "save", // usado para controle interno
                            click: function (win) {
                                NotaAzul.Boleto.DadosRem.Salvar(win);
                            }
                        }
                    ]
                },
                success: function () {
                }
            });
        };

        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (obj) {
            if (obj == null) {
                Prion.Alert({ msg: "Erro ao obter o objeto de Boleto." });
                return;
            }
            "loading".getDom().style.display = "block";

            $.ajax({
                type: "GET",
                url: rootDir + "Boleto/GerarBoleto?idBoleto=" + obj.Id,
                contentType: false,
                processData: false,
                data: { Entidades: "AlunoResponsavel,AlunoResponsavelEndereco,Endereco", idBoleto: obj.Id },
                success: function (result) {
                    "loading".getDom().style.display = "none";
                    if (result.success) {
                        window.open(result.enderecoBoleto);
                    }
                    else {
                        alert(result.mensagem.TextoMensagem);
                        return true;
                    }
                }
            });            
        };

        /**
       * @descrição: Método responsável por abrir um registro
       * @params: obj => objeto que representa o registro clicado
       * @return: void
       **/
        var _gerarRemessa = function (ids) {
            
            "loading".getDom().style.display = "block";
            var str="";
            for (var i = 0, len = ids.length; i < len; i++) {
                str += ids[i];
                if (i < len - 1) {
                    str += ",";
                }
            }
            $.ajax({
                type: "GET",
                url: rootDir + "Boleto/GerarArquivoRem?idsStr=" + str,
                contentType: false,
                processData: false,                
                success: function (result) {
                    "loading".getDom().style.display = "none";
                    if (result.success) {
                        window.open(result.enderecoBoleto);
                    }
                    else {
                        alert(result.mensagem.TextoMensagem);
                        return true;
                    }
                }
            });
        };
              

        var _cancelarManualmente = function (ids) {

            "loading".getDom().style.display = "block";
            var str = "";
            for (var i = 0, len = ids.length; i < len; i++) {
                str += ids[i];
                if (i < len - 1) {
                    str += ",";
                }
            }
            $.ajax({
                type: "GET",
                url: rootDir + "Boleto/CancelarBoletosManualmente?idsStr=" + str,
                contentType: false,
                processData: false,
                success: function (result) {
                    "loading".getDom().style.display = "none";
                    if (result.success) {
                        alert(result.mensagem);
                    }
                    else {
                        alert(result.mensagem.TextoMensagem);
                        return true;
                    }
                }
            });
        };

        var _quitarManualmente = function (ids) {

            "loading".getDom().style.display = "block";
            var str = "";
            for (var i = 0, len = ids.length; i < len; i++) {
                str += ids[i];
                if (i < len - 1) {
                    str += ",";
                }
            }
            $.ajax({
                type: "GET",
                url: rootDir + "Boleto/QuitarBoletosManualmente?idsStr=" + str,
                contentType: false,
                processData: false,
                success: function (result) {
                    "loading".getDom().style.display = "none";
                    if (result.success) {
                        alert(result.mensagem);
                    }
                    else {
                        alert(result.mensagem.TextoMensagem);
                        return true;
                    }
                }
            });
        };

        /**
        * @descrição: Responsável pelo AutoComplete do campo Aluno
        * @return: void
        **/
        var _autoCompleteBuscarAluno = function () {
            $("#Filtro_NomeAluno").autocomplete({
                source: rootDir + "Aluno/ListaAlunoPorNome",
                minLength: 2,
                appendTo: $("#Filtro_NomeAluno").parent(),
                delay: 500,
                dataType: 'json',
                focus: true,
                open: function () {
                    $(".ui-autocomplete:visible").css({ top: "127px", left: "159px" });

                },
                select: function (event, data) {
                    "Filtro_NomeAluno".setValue(data.item.label);
                    "Filtro_Aluno".setValue(data.item.value);
                    return false;
                }
            });
        };
        

        /**
        * @descrição: Cria o todo o html da lista
        * @return: void
        **/
        var _criarLista = function () {
            if (_grid != null) { return; }

            _grid = new Prion.Lista({
                id: "listaBoletos",
                url: rootDir + "Boleto/GetLista/",
                filter: { Entidades: "Endereco" },
                height: 600,
                title: { text: "Boletos" },
                request: { base: "Boleto" }, // colocado pois o SQL não está com ALIAS para os campos da tabela Curso
                buttons: {
                    update: { show: true, click: NotaAzul.Boleto.Lista.Abrir, tooltip: "Abrir Boleto" },
                    onlyView: { show: false }
                },
                rowNumber: {
                    show: false
                },
                action: { onDblClick: NotaAzul.Boleto.Lista.Abrir },
                columns: [
                    { header: "Aluno", nameJson: "NomeAluno", width: "150px" },
                    { header: "Número", nameJson: "NumeroDocumento", width: "150px" },
                    { header: "Valor", nameJson: "Valor",mask: "money", width: "150px" },
                    { header: "Desconto", nameJson: "Desconto", mask: "money",width: "150px" },
                    { header: "Acrescimo", nameJson: "Juros", mask: "money",width: "150px" },
                    { header: "Vencimento", nameJson: "DataVencimento", type: "date", width: "110px" },
                    { header: "Remessa?", nameJson: "RemessaGeradoStr" }
                ]
            });

            // adiciona um botão à lista de Contratos
            _grid.addButton({
                click: function () {
                    var objSelecionado = NotaAzul.Boleto.Lista.Grid().getSelected();

                    if (objSelecionado == null) {
                        window.alert("Nenhum registro foi selecionado");
                        return false;
                    }
                    var ids = [];

                    for (var i = 0, len = objSelecionado.length; i < len; i++) {
                        ids.push(objSelecionado[i].object.Id);
                    }

                    NotaAzul.Boleto.Lista.GerarArquivoRemessa(ids);
                },
                title: "Gerar Arquivo Remessa",
                tooltip: "Gera o arquivo de remessa"
            });

            // adiciona um botão à lista de Contratos
            _grid.addButton({
                click: _abrirBaixa,
                title: "Ler arquivo RET",
                tooltip: "Lê o arquivo de retorno"
            });

            // adiciona um botão à lista de Contratos
            _grid.addButton({
                click: _abrirRem,
                title: "Gerar arquivo REM mensal",
                tooltip: "Gera o arquivo de Remessa de um mês"
            });

            _grid.addButton({
                click: _abrirUpdate,
                title: "Atualizar Boleto",
                tooltip: "Atualiza os valores de um boleto"
            });


            _grid.addButton({
                click: function () {
                    var objSelecionado = NotaAzul.Boleto.Lista.Grid().getSelected();

                    if (objSelecionado == null) {
                        window.alert("Nenhum registro foi selecionado");
                        return false;
                    }
                    var ids = [];

                    for (var i = 0, len = objSelecionado.length; i < len; i++) {
                        ids.push(objSelecionado[i].object.Id);
                    }

                    NotaAzul.Boleto.Lista.QuitarManualmente(ids);
                },
                title: "Quitar Boleto",
                tooltip: "Quita manualmente os boletos selecionados"
            });

            _grid.addButton({
                click: function () {
                    var objSelecionado = NotaAzul.Boleto.Lista.Grid().getSelected();

                    if (objSelecionado == null) {
                        window.alert("Nenhum registro foi selecionado");
                        return false;
                    }
                    var ids = [];

                    for (var i = 0, len = objSelecionado.length; i < len; i++) {
                        ids.push(objSelecionado[i].object.Id);
                    }

                    NotaAzul.Boleto.Lista.CancelarManualmente(ids);
                },
                title: "Cancelar Boleto",
                tooltip: "Cancela manualmente os boletos selecionados"
            });
        };

        /**
        * @descrição: Método que retorna o objeto do grid
        * @return: Objeto grid
        **/
        var _getGrid = function () {
            return _grid;
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            _permissoes = Permissoes.CRUD("Boleto");
            _criarLista();
            _autoCompleteBuscarAluno();
            _carregarJanelaBoleto();
            _carregarJanelaRem();
            _carregarJanelaUpdate();
            Prion.Event.add("btnAplicarFiltro", "click", function () {
                _grid.load({
                    query: "{\"Filtros\":[{\"Campo\":\"Aluno.Id\",\"Operador\":\"=\",\"Valor\":\"" + "Filtro_Aluno".getValue() + "\"}]}"
                });
            });          
        };

        /**
        * @descrição: métodos que serão executados quando for abrir para novo registro
        **/
        var _novo = function () {

        };
        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            Iniciar: _iniciar,
            Abrir: _abrir,
            AbrirBoleto: _abrirUpdate,
            Grid: _getGrid,
            GerarArquivoRemessa: _gerarRemessa,
            CancelarManualmente: _cancelarManualmente,
            QuitarManualmente: _quitarManualmente,
            Novo: _novo
        };
    } ();
    NotaAzul.Boleto.Lista.Iniciar();
} (window, document));