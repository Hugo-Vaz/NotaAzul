/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Titulo == null) {
        NotaAzul.Titulo = {};
    }

    NotaAzul.Titulo.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowTitulo = null,
            _permissoes = null;

        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (obj) {
            _windowTitulo.apply({
                object: "Titulo",
                url: rootDir + "Titulo/Carregar/" + obj.Id,
                ajax: { onlyJson: true },
                fnBeforeLoad: NotaAzul.Titulo.Dados.Novo
            }).load().show({ animate: true });
        };

        /**
        * @descricao: Carrega o conteúdo do html Titulo/ViewDados em background, acelerando o futuro carregamento
        **/
        var _carregarJanelaTitulo = function () {
            _windowTitulo = new Prion.Window({
                url: rootDir + "Titulo/ViewDados/",
                id: "popupTitulo",
                height: 420,
                width: 650,
                title: { text: "Título" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.Titulo.Lista.Grid().load();
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
                                NotaAzul.Titulo.Dados.Salvar(win);
                            }
                        },
                        {
                            text: "Limpar",
                            className: Prion.settings.ClassName.buttonReset,
                            type: "reset",
                            click: function (win) {
                                NotaAzul.Titulo.Dados.Novo();
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
                id: "listaTitulos",
                url: rootDir + "Titulo/GetLista/",
                title: { text: "Títulos" },
                filter: { Entidades: "Situacao" },
                request: { base: "Titulo" }, // colocado pois o SQL não está com ALIAS para os campos da tabela Titulo
                buttons: {
                    insert: { show: _permissoes.insert, click: NotaAzul.Titulo.Lista.Novo },
                    update: { show: _permissoes.update, click: NotaAzul.Titulo.Lista.Abrir },
                    remove: { show: _permissoes.remove, url: rootDir + "Titulo/Excluir/" },
                    onlyView: { show: _permissoes.onlyView }
                },
                action: { onDblClick: NotaAzul.Titulo.Lista.Abrir },
                columns: [
                    { header: "Descrição", nameJson: "Descricao" },
                    { header: "Valor", nameJson: "Valor", width: "200px", mask: "money" },
                    { header: "Data de Vencimento", nameJson: "DataVencimento", width: "100px", type: "date" },
                    { header: "Data do Pagamento", nameJson: "DataOperacao", width: "100px", type: "date" }
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
            _permissoes = Permissoes.CRUD("TITULO");

            _criarLista();
            _carregarJanelaTitulo();
        };


        /**
        * @descrição: Método responsável por abrir uma popup para inserção de novo registro
        * @return: void
        **/
        var _novo = function () {
            _windowTitulo.show({
                animate: true,
                fnBefore: NotaAzul.Titulo.Dados.Novo
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

    NotaAzul.Titulo.Lista.Iniciar();
} (window, document));


