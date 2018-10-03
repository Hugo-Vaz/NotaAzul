/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.GrupoDesconto == null) {
        NotaAzul.GrupoDesconto = {};
    }

    NotaAzul.GrupoDesconto.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowGrupoDesconto = null,
            _permissoes = null;


        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (obj) {
            _windowGrupoDesconto.apply({
                object: 'GrupoDesconto',
                url: rootDir + "GrupoDesconto/Carregar/" + obj.Id,
                ajax: { onlyJson: true },
                fnBeforeLoad: NotaAzul.GrupoDesconto.Dados.Novo
            }).load().show({ animate: true });
        };

        /**
        * @descrição: Carrega o conteúdo do html GrupoDesconto/ViewDados em background, acelerando o futuro carregamento
        **/
        var _carregarJanelaGrupoDesconto = function () {
            _windowGrupoDesconto = new Prion.Window({
                url: rootDir + "GrupoDesconto/ViewDados/",
                id: "popupGrupoDesconto",
                height: 305,
                width: 670,
                title: { text: "Grupos de Desconto" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.GrupoDesconto.Lista.Grid().load();
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
                                NotaAzul.GrupoDesconto.Dados.Salvar(win);
                            }
                        },
                        {
                            text: "Limpar",
                            className: Prion.settings.ClassName.buttonReset,
                            type: "reset",
                            click: function (win) {
                                NotaAzul.GrupoDesconto.Dados.Novo();
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
                id: "listaGrupoDesconto",
                url: rootDir + "GrupoDesconto/GetLista/",
                title: { text: "Grupos de Desconto" },
                filter: { Entidades: "Situacao, CursoAnoLetivo, Turno" },
                request: { base: "GrupoDesconto" },
                buttons: {
                    insert: { show: _permissoes.insert, click: NotaAzul.GrupoDesconto.Lista.Novo },
                    update: { show: _permissoes.update, click: NotaAzul.GrupoDesconto.Lista.Abrir },
                    remove: { show: _permissoes.remove, url: rootDir + "GrupoDesconto/Excluir/" },
                    onlyView: { show: _permissoes.onlyView }
                },
                action: { onDblClick: NotaAzul.GrupoDesconto.Lista.Abrir },
                columns: [
                    { header: "Nome", nameJson: "Nome", width: "160px" },
                    { header: "Descrição", nameJson: "Descricao", width: "400px" },
                    { header: "Valor", nameJson: "Valor", width: "100px" },
                    { header: "Tipo de Desconto", nameJson: "TipoDesconto", width: "120px" }
                ]
            });
        };

        /**
        * @descrição: Método que retorna o objeto do grid
        * @return: Objeto grid
        **/
        var _getrGrid = function () {
            return _grid;
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            _permissoes = Permissoes.CRUD("GRUPO_DESCONTO");

            _criarLista();
            _carregarJanelaGrupoDesconto();
        };

        /**
        * @descrição: Método responsável por abrir uma popup para inserção de novo registro
        * @return: void
        **/
        var _novo = function () {
            _windowGrupoDesconto.show({
                animate: true,
                fnBefore: NotaAzul.GrupoDesconto.Dados.Novo
            });
        };
        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            Abrir: _abrir,
            Grid: _getrGrid,
            Iniciar: _iniciar,
            Novo: _novo
        };
    } ();
    NotaAzul.GrupoDesconto.Lista.Iniciar();
} (window, document));