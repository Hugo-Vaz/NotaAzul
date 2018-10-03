/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Disciplina == null) {
        NotaAzul.Disciplina = {};
    }

    NotaAzul.Disciplina.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowDisciplina = null,
            _permissoes = null;


        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (obj) {
            _windowDisciplina.apply({
                object: 'Disciplina',
                url: rootDir + "Disciplina/Carregar/" + obj.Id,
                ajax: { onlyJson: true },
                fnBeforeLoad: NotaAzul.Disciplina.Dados.Novo
            }).load().show({ animate: true });
        };

        /**
        * @descricao: Carrega o conteúdo do html Disciplina/ViewDados em background, acelerando o futuro carregamento
        **/
        var _carregarJanelaDisciplina = function () {
            _windowDisciplina = new Prion.Window({
                url: rootDir + "Disciplina/ViewDados/",
                id: "popupDisciplina",
                height: 250,
                width: 560,
                title: { text: "Disciplina" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.Disciplina.Lista.Grid().load();
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
                                NotaAzul.Disciplina.Dados.Salvar(win);
                            }
                        },
                        {
                            text: "Limpar",
                            className: Prion.settings.ClassName.buttonReset,
                            type: "reset",
                            click: function (win) {
                                NotaAzul.Disciplina.Dados.Novo();
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
                id: "listaDisciplinas",
                url: rootDir + "Disciplina/GetLista/",
                title: { text: "Disciplinas" },
                filter: { Entidades: "Situacao" },
                request: { base: "Disciplina" }, // colocado pois o SQL não está com ALIAS para os campos da tabela Disciplina
                buttons: {
                    insert: { show: _permissoes.insert, click: NotaAzul.Disciplina.Lista.Novo },
                    update: { show: _permissoes.update, click: NotaAzul.Disciplina.Lista.Abrir },
                    remove: { show: _permissoes.remove, url: rootDir + "Disciplina/Excluir/" },
                    onlyView: { show: _permissoes.onlyView }
                },
                action: { onDblClick: NotaAzul.Disciplina.Lista.Abrir },
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
            _permissoes = Permissoes.CRUD("DISCIPLINA");

            _criarLista();
            _carregarJanelaDisciplina();
        };

        /**
        * @descrição: Método responsável por abrir uma popup para inserção de novo registro
        * @return: void
        **/
        var _novo = function () {
            _windowDisciplina.show({
                animate: true,
                fnBefore: NotaAzul.Disciplina.Dados.Novo
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
    NotaAzul.Disciplina.Lista.Iniciar();
} (window, document));


