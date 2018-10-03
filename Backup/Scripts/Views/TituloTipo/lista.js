/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.TituloTipo == null) {
        NotaAzul.TituloTipo = {};
    }

    NotaAzul.TituloTipo.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowTituloTipo = null,
            _permissoes = null;

        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (obj) {
            _windowTituloTipo.apply({
                object: 'TituloTipo',
                url: rootDir + "TituloTipo/Carregar/" + obj.Id,
                ajax: { onlyJson: true },
                fnBeforeLoad: NotaAzul.TituloTipo.Dados.Novo
            }).load().show({ animate: true });
        };

        /**
        * @descricao: Carrega o conteúdo do html TituloTipo/ViewDados em background, acelerando o futuro carregamento
        **/
        var _carregarJanelaTituloTipo = function () {
            _windowTituloTipo = new Prion.Window({
                url: rootDir + "TituloTipo/ViewDados/",
                id: "popupTituloTipo",
                height: 280,
                width: 540,
                title: { text: "Tipos de Título" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.TituloTipo.Lista.Grid().load();
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
                                NotaAzul.TituloTipo.Dados.Salvar(win);
                            }
                        },
                        {
                            text: "Limpar",
                            className: Prion.settings.ClassName.buttonReset,
                            type: "reset",
                            click: function (win) {
                                NotaAzul.TituloTipo.Dados.Novo();
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
                id: "listaTiposTitulo",
                url: rootDir + "TituloTipo/GetLista/",
                title: { text: "Tipos de Título" },
                filter: { Entidades: "Situacao" },
                request: { base: "TituloTipo" }, // colocado pois o SQL não está com ALIAS para os campos da tabela TituloTipo
                buttons: {
                    insert: { show: _permissoes.insert, click: NotaAzul.TituloTipo.Lista.Novo },
                    update: { show: _permissoes.update, click: NotaAzul.TituloTipo.Lista.Abrir },
                    remove: { show: _permissoes.remove, url: rootDir + "TituloTipo/Excluir/" },
                    onlyView: { show: _permissoes.onlyView }
                },
                action: { onDblClick: NotaAzul.TituloTipo.Lista.Abrir },
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
            _permissoes = Permissoes.CRUD("TIPO_TITULO");

            _criarLista();
            _carregarJanelaTituloTipo();
        };

        /**
        * @descrição: Método responsável por abrir uma popup para inserção de novo registro
        * @return: void
        **/
        var _novo = function () {
            _windowTituloTipo.show({
                animate: true,
                fnBefore: NotaAzul.TituloTipo.Dados.Novo
            });
        };
        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            Abrir: _abrir,
            Grid: _getGrid,
            Iniciar: _iniciar,
            Novo: _novo
        };
    } ();

    NotaAzul.TituloTipo.Lista.Iniciar();
} (window, document));


