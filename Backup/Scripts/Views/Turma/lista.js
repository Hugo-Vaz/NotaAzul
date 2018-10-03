/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function () {
    "use strict";
    if (NotaAzul.Turma == null) {
        NotaAzul.Turma = {};
    }

    NotaAzul.Turma.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowTurma = null,
            _permissoes = null;


        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (obj) {
            _windowTurma.apply({
                object: 'Turma',
                url: rootDir + "Turma/Carregar/" + obj.Id,
                ajax: { onlyJson: true },
                fnBeforeLoad: NotaAzul.Turma.Dados.Novo
            }).load().show({ animate: true });
        };

        /**
        * @descrição: Carrega o conteúdo do html Turma/ViewDados em background, acelerando o futuro carregamento
        **/
        var _carregarJanelaTurma = function () {
            _windowTurma = new Prion.Window({
                url: rootDir + "Turma/ViewDados/",
                id: "popupTurma",
                height: 440,
                width: 680,
                title: { text: "Turma" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.Turma.Lista.Grid().load();
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
                                NotaAzul.Turma.Dados.Salvar(win);
                            }
                        },
                        {
                            text: "Limpar",
                            className: Prion.settings.ClassName.buttonReset,
                            type: "reset",
                            click: function (win) {
                                NotaAzul.Turma.Dados.Novo();
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
                id: "listaTurmas",
                url: rootDir + "Turma/GetLista/",
                title: { text: "Turmas" },
                filter: { Entidades: "Situacao, CursoAnoLetivo, Turno" },
                request: { base: "Turma" }, // colocado pois o SQL não está com ALIAS para os campos da tabela Turma
                buttons: {
                    insert: { show: _permissoes.insert, click: NotaAzul.Turma.Lista.Novo },
                    update: { show: _permissoes.update, click: NotaAzul.Turma.Lista.Abrir },
                    remove: { show: _permissoes.remove, url: rootDir + "Turma/Excluir/" },
                    onlyView: { show: _permissoes.onlyView }
                },
                action: { onDblClick: NotaAzul.Turma.Lista.Abrir },
                columns: [
                    { header: "Nome", nameJson: "Nome", width: "200px" },
                    { header: "Curso", nameJson: "Curso.Nome", width: "200px" },
                    { header: "Turno", nameJson: "Turno.Nome", width: "120px" },
                    { header: "Vagas", nameJson: "QuantidadeVagas" },
                    { header: "Status", nameJson: "Situacao.Nome", width: "100px" },
                    { header: "Cadastrado em", nameJson: "DataCadastro", type: "date", width: "110px" }
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
            _permissoes = Permissoes.CRUD("TURMA");

            _criarLista();
            _carregarJanelaTurma();
        };
        
        /**
        * @descrição: Método responsável por abrir uma popup para inserção de novo registro
        * @return: void
        **/
        var _novo = function () {
            _windowTurma.show({
                animate: true,
                fnBefore: NotaAzul.Turma.Dados.Novo
            });
        };
        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            Abrir: _abrir,
            Grid: _getGrid,
            Iniciar: _iniciar,
            Novo:_novo
        };

    } ();

    NotaAzul.Turma.Lista.Iniciar();
} (window, document));