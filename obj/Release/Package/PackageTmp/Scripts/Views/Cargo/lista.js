/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Cargo == null) {
        NotaAzul.Cargo = {};
    }

    NotaAzul.Cargo.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowCargo = null,
            _permissoes = null;

        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (obj) {
            _windowCargo.apply({
                object: 'Cargo',
                url: rootDir + "Cargo/Carregar/" + obj.Id,
                ajax: { onlyJson: true },
                fnBeforeLoad: NotaAzul.Cargo.Dados.Novo
            }).load().show({ animate: true });
        };

        /**
        * @descricao: Carrega o conteúdo do html Cargo/ViewDados em background, acelerando o futuro carregamento
        **/
        var _carregarJanelaCargo = function () {
            _windowCargo = new Prion.Window({
                url: rootDir + "Cargo/ViewDados/",
                id: "popupCargo",
                height: 206,
                width: 540,
                title: { text: "Cargo" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.Cargo.Lista.Grid().load();
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
                                NotaAzul.Cargo.Dados.Salvar(win);
                            }
                        },
                        {
                            text: "Limpar", className: Prion.settings.ClassName.buttonReset, type: "reset",
                            click: function (win) {
                                NotaAzul.Cargo.Dados.Novo();
                            }
                        }
                    ]
                }
            });
        };

        /**
        * @descrição: Cria todo o Html da lista
        * @return: void
        **/
        var _criarLista = function () {
            if (_grid != null) { return; }

            _grid = new Prion.Lista({
                id: "listaCargos",
                url: rootDir + "Cargo/GetLista/",
                title: { text: "Cargos" },
                filter: { Entidades: "Situacao" },
                request: { base: "Cargo" }, // colocado pois o SQL não está com ALIAS para os campos da tabela Cargo
                buttons: {
                    insert: { show: _permissoes.insert, click: NotaAzul.Cargo.Lista.Novo },
                    update: { show: _permissoes.update, click: NotaAzul.Cargo.Lista.Abrir },
                    remove: { show: _permissoes.remove, url: rootDir + "Cargo/Excluir/" },
                    onlyView: { show: _permissoes.onlyView }
                },
                action: { onDblClick: NotaAzul.Cargo.Lista.Abrir },
                columns: [
                    { header: "Nome", nameJson: "Nome" },
                    { header: "Status", nameJson: "Situacao.Nome" },
                    { header: "Cadastrado em", nameJson: "DataCadastro", type: "date" }
                ]
            });
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            _permissoes = Permissoes.CRUD("CARGO");

            _criarLista();
            _carregarJanelaCargo();
        };



        /**
        * @descrição: Método que retorna o objeto do grid
        * @return: Objeto grid
        **/
        var _getGrid = function () {
            return _grid;
        };

        /**
        * @descrição: Método responsável por abrir uma popup para inserção de novo registro
        * @return: void
        **/
        var _novo = function () {
            _windowCargo.show({
                animate: true,
                fnBefore: NotaAzul.Cargo.Dados.Novo
            });
        };

        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            Abrir: _abrir,
            Iniciar: _iniciar,
            Grid: _getGrid,
            Novo: _novo
        };
    } ();
    NotaAzul.Cargo.Lista.Iniciar();
} (window, document));


