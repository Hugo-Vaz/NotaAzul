/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Segmento == null) {
        NotaAzul.Segmento = {};
    }

    NotaAzul.Segmento.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowSegmento = null,
            _permissoes = null;

        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (obj) {
            _windowSegmento.apply({
                object: 'Segmento',
                url: rootDir + "Segmento/Carregar/" + obj.Id,
                ajax: { onlyJson: true },
                fnBeforeLoad: NotaAzul.Segmento.Dados.Novo
            }).load().show({ animate: true });
        };

        /**
        * @descrição: Carrega o conteúdo do html Segmento/ViewDados em background, acelerando o futuro carregamento
        **/
        var _carregarJanelaSegmento = function () {
            _windowSegmento = new Prion.Window({
                url: rootDir + "Segmento/ViewDados/",
                id: "popupSegmento",
                height: 206,
                width: 540,
                title: { text: "Segmento" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.Segmento.Lista.Grid().load();
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
                                NotaAzul.Segmento.Dados.Salvar(win);
                            }
                        },
                        {
                            text: "Limpar",
                            className: Prion.settings.ClassName.buttonReset,
                            type: "reset",
                            click: function (win) {
                                NotaAzul.Segmento.Dados.Novo();
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
                id: "listaSegmentos",
                url: rootDir + "Segmento/GetLista/",
                title: { text: "Segmentos" },
                filter: { Entidades: "Situacao" },
                request: { base: "Segmento" }, // colocado pois o SQL não está com ALIAS para os campos da tabela Segmento
                buttons: {
                    insert: { show: _permissoes.insert, click: NotaAzul.Segmento.Lista.Novo },
                    update: { show: _permissoes.update, click: NotaAzul.Segmento.Lista.Abrir },
                    remove: { show: _permissoes.remove, url: rootDir + "Segmento/Excluir/" },
                    onlyView: { show: _permissoes.onlyView }
                },
                action: { onDblClick: NotaAzul.Segmento.Lista.Abrir },
                columns: [
                    { header: "Nome", nameJson: "Nome" },
                    { header: "Status", nameJson: "Situacao.Nome" },
                    { header: "Cadastrado em", nameJson: "DataCadastro", type: "date" }
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
            _permissoes = Permissoes.CRUD("SEGMENTO");

            _criarLista();
            _carregarJanelaSegmento();
        };

        /**
        * @descrição: Método responsável por abrir uma popup para inserção de novo registro
        * @return: void
        **/
        var _novo = function () {
            _windowSegmento.show({
                animate: true,
                fnBefore: NotaAzul.Segmento.Dados.Novo
            });
        };

        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            Abrir: _abrir,
            Grid: _grid,
            Iniciar: _iniciar,
            Novo:_novo
        };

    } ();

    NotaAzul.Segmento.Lista.Iniciar();
} (window, document));