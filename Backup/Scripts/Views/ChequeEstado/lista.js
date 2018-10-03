/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.ChequeEstado == null) {
        NotaAzul.ChequeEstado = {};
    }

    NotaAzul.ChequeEstado.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowChequeEstado = null,
            _permissoes = null;


        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (obj) {
            _windowChequeEstado.apply({
                object: 'ChequeEstado',
                url: rootDir + "ChequeEstado/Carregar/" + obj.Id,
                ajax: { onlyJson: true },
                fnBeforeLoad: NotaAzul.ChequeEstado.Dados.Novo
            }).load().show({ animate: true });
        };
        /**
        * @descricao: Carrega o conteúdo do html ChequeEstado/ViewDados em background, acelerando o futuro carregamento
        **/
        var _carregarJanelaChequeEstado = function () {
            _windowChequeEstado = new Prion.Window({
                url: rootDir + "ChequeEstado/ViewDados/",
                id: "popupChequeEstado",
                height: 300,
                width: 540,
                title: { text: "Estado de um Cheque" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.ChequeEstado.Lista.Grid().load();
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
                                NotaAzul.ChequeEstado.Dados.Salvar(win);
                            }
                        },
                        {
                            text: "Limpar", className: Prion.settings.ClassName.buttonReset, type: "reset",
                            click: function (win) {
                                NotaAzul.ChequeEstado.Dados.Novo();
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
                id: "listaEstadosCheque",
                url: rootDir + "ChequeEstado/GetLista/",
                title: { text: "Estados de um Cheque" },
                filter: { Entidades: "Situacao" },
                request: { base: "ChequeEstado" }, // colocado pois o SQL não está com ALIAS para os campos da tabela Cheque
                buttons: {
                    insert: { show: _permissoes.insert, click: NotaAzul.ChequeEstado.Lista.Novo },
                    update: { show: _permissoes.update, click: NotaAzul.ChequeEstado.Lista.Abrir },
                    remove: { show: _permissoes.remove, url: rootDir + "ChequeEstado/Excluir/" },
                    onlyView: { show: _permissoes.onlyView }
                },
                action: { onDblClick: NotaAzul.ChequeEstado.Lista.Abrir },
                columns: [
                    { header: "Nome", nameJson: "Nome" },
                    { header: "Descrição", nameJson: "Descricao", width: '200px' }
                ]
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
            _permissoes = Permissoes.CRUD("CHEQUE_ESTADO");

            _criarLista();
            _carregarJanelaChequeEstado();
        };
        /**
        * @descrição: Método responsável por abrir uma popup para inserção de novo registro
        * @return: void
        **/
        var _novo = function () {
            _windowChequeEstado.show({
                animate: true,
                fnBefore: NotaAzul.ChequeEstado.Dados.Novo
            });
        };
        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            Iniciar: _iniciar,
            Abrir: _abrir,
            Grid: _getGrid,
            Novo: _novo
        };
    } ();

    NotaAzul.ChequeEstado.Lista.Iniciar();
} (window, document));


