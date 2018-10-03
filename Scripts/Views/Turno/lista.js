/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Turno == null) {
        NotaAzul.Turno = {};
    }

    NotaAzul.Turno.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowTurno = null,
            _permissoes = null;

        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (obj) {
            _windowTurno.apply({
                object: 'Turno',
                url: rootDir + "Turno/Carregar/" + obj.Id,
                ajax: { onlyJson: true },
                fnBeforeLoad: NotaAzul.Turno.Dados.Novo
            }).load().show({ animate: true });
        };

        /**
        * @descrição: Carrega o conteúdo do html Turno/ViewDados em background, acelerando o futuro carregamento
        **/
        var _carregarJanelaTurno = function () {
            _windowTurno = new Prion.Window({
                url: rootDir + "Turno/ViewDados/",
                id: "popupTurno",
                width: 450,
                height: 206,
                title: { text: "Turno" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.Turno.Lista.Grid().load();
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
                                NotaAzul.Turno.Dados.Salvar(win);
                            }
                        },
                        {
                            text: "Limpar",
                            className: Prion.settings.ClassName.buttonReset,
                            type: "reset",
                            click: function (win) {
                                NotaAzul.Turno.Dados.Novo();
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
                id: "listaTurnos",
                url: rootDir + "Turno/GetLista/",
                title: { text: "Turnos" },
                filter: { Entidades: "Situacao" },
                request: { base: "Turno" }, // colocado pois o SQL não está com ALIAS para os campos da tabela UsuarioGrupo
                buttons: {
                    insert: { show: _permissoes.insert, click: NotaAzul.Turno.Lista.Novo },
                    update: { show: _permissoes.update, click: NotaAzul.Turno.Lista.Abrir },
                    remove: { show: _permissoes.remove, url: rootDir + "Turno/Excluir/" },
                    onlyView: { show: _permissoes.onlyView }
                },
                action: { onDblClick: NotaAzul.Turno.Lista.Abrir },
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
            _permissoes = Permissoes.CRUD("TURNO");

            _criarLista();
            _carregarJanelaTurno();
        };

        /**
        * @descrição: Método responsável por abrir uma popup para inserção de novo registro
        * @return: void
        **/
        var _novo = function () {
            _windowTurno.show({
                animate: true,
                fnBefore: NotaAzul.Turno.Dados.Novo
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
    NotaAzul.Turno.Lista.Iniciar();
} (window, document));