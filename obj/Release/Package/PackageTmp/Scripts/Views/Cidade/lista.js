/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Cidade == null) {
        NotaAzul.Cidade = {};
    }

    NotaAzul.Cidade.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowCidade = null,
            _permissoes = null;
            
        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (obj) {
            _windowCidade.apply({
                object: 'Cidade',
                url: rootDir + "Cidade/Carregar/" + obj.Id,
                ajax: { onlyJson: true },
                fnBeforeLoad: NotaAzul.Cidade.Dados.Novo
            }).load().show({ animate: true });
        };

        /**
        * @descrição: Carrega o conteúdo do html Cidade/ViewDados em background, acelerando o futuro carregamento
        * @return: void
        **/
        var _carregarJanelaCidade = function () {
            _windowCidade = new Prion.Window({
                url: rootDir + "Cidade/ViewDados/",
                id: "popupCidade",
                height: 208,
                width: 540,
                title: { text: "Cidade" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.Cidade.Lista.Grid().load();
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
                                NotaAzul.Cidade.Dados.Salvar(win);
                            }
                        },
                        {
                            text: "Limpar",
                            className: Prion.settings.ClassName.buttonReset,
                            type: "reset",
                            click: function (win) {
                                NotaAzul.Cidade.Dados.Novo();
                            }
                        }
                    ]
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
                id: "listaCidades",
                url: rootDir + "Cidade/GetLista/",
                title: { text: "Cidades" },
                filter: { Entidades: "Estado" },
                request: { base: "Cidade" }, // colocado pois o SQL não está com ALIAS para os campos da tabela Cidade
                buttons: {
                    insert: { show: _permissoes.insert, click: NotaAzul.Cidade.Lista.Novo },
                    update: { show: _permissoes.update, click: NotaAzul.Cidade.Lista.Abrir },
                    remove: { show: _permissoes.remove, url: rootDir + "Cidade/Excluir/" },
                    onlyView: { show: _permissoes.onlyView }
                },
                action: { onDblClick: NotaAzul.Cidade.Lista.Abrir },
                columns: [
                    { header: "Nome", nameJson: "Nome" },
                    { header: "Estado", nameJson: "Estado.Nome" }
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
            _permissoes = Permissoes.CRUD("CIDADE");

            _criarLista();
            _carregarJanelaCidade();
        };

        /**
        * @descrição: Método responsável por abrir uma popup para inserção de novo registro
        * @return: void
        **/
        var _novo = function () {
            _windowCidade.show({
                animate: true,
                fnBefore: NotaAzul.Cidade.Dados.Novo
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
    NotaAzul.Cidade.Lista.Iniciar();
} (window, document));