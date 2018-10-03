/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";


$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Bairro == null) {
        NotaAzul.Bairro = {};
    }

    NotaAzul.Bairro.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowBairro = null,
            _permissoes = null;

        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (obj) {
            _windowBairro.apply({
                object: 'Bairro',
                url: rootDir + "Bairro/Carregar/" + obj.Id,
                ajax: { onlyJson: true },
                fnBeforeLoad: NotaAzul.Bairro.Dados.Novo
            }).load().show({ animate: true });
        };

        /**
        * @descrição: Carrega o conteúdo do html Bairro/ViewDados em background, acelerando o futuro carregamento
        * @return: void
        **/
        var _carregarJanelaBairro = function () {
            _windowBairro = new Prion.Window({
                url: rootDir + "Bairro/ViewDados/",
                id: "popupBairro",
                height: 208,
                width: 540,
                title: { text: "Bairro" },
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
                                NotaAzul.Bairro.Dados.Salvar(win);
                            }
                        },
                        {
                            text: "Limpar", className: Prion.settings.ClassName.buttonReset, type: "reset",
                            click: function (win) {
                                NotaAzul.Bairro.Dados.Novo();
                            }
                        }
                    ]
                },
                success: function () {
                    // habilita o botão de insert
                    NotaAzul.Bairro.Lista.Grid().enableButton("insert");
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
                id: "listaBairros",
                url: rootDir + "Bairro/GetLista/",
                title: { text: "Bairros" },
                filter: { Entidades: "Cidade" },
                request: { base: "Bairro" }, // colocado pois o SQL não está com ALIAS para os campos da tabela Bairro
                buttons: {
                    insert: { show: _permissoes.insert, click: NotaAzul.Bairro.Lista.Novo, disabled: true },
                    update: { show: _permissoes.update, click: NotaAzul.Bairro.Lista.Abrir, disabled: true },
                    remove: { show: _permissoes.remove, url: rootDir + "Bairro/Excluir/" },
                    onlyView: { show: _permissoes.onlyView }
                },
                action: { onDblClick: NotaAzul.Bairro.Lista.Abrir },
                columns: [
                    { header: "Nome", nameJson: "Nome" },
                    { header: "Cidade", nameJson: "Cidade.Nome" }
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
            _permissoes = Permissoes.CRUD("BAIRRO");

            _criarLista();
            _carregarJanelaBairro();
        };

        /**
        * @descrição: Método responsável por abrir uma popup para inserção de novo registro
        * @return: void
        **/
        var _novo = function () {
            _windowBairro.show({
                animate: true,
                fnBefore: NotaAzul.Bairro.Dados.Novo
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
    NotaAzul.Bairro.Lista.Iniciar();
} (window, document));