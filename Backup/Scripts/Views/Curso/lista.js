/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Curso == null) {
        NotaAzul.Curso = {};
    }

    NotaAzul.Curso.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowCurso = null,
            _permissoes = null;


        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (obj) {
            _windowCurso.apply({
                object: 'Curso',
                url: rootDir + "Curso/Carregar/" + obj.Id,
                ajax: { onlyJson: true },
                fnBeforeLoad: NotaAzul.Curso.Dados.Novo
            }).load().show({ animate: true });
        };

        /**
        * @descrição: Carrega o conteúdo do html Curso/ViewDados em background, acelerando o futuro carregamento
        **/
        var _carregarJanelaCurso = function () {
            _windowCurso = new Prion.Window({
                url: rootDir + "Curso/ViewDados/",
                id: "popupCurso",
                height: 360,
                width: 540,
                title: { text: "Curso" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.Curso.Lista.Grid().load();
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
                                NotaAzul.Curso.Dados.Salvar(win);
                            }
                        },
                        {
                            text: "Limpar",
                            className: Prion.settings.ClassName.buttonReset,
                            type: "reset",
                            click: function (win) {
                                NotaAzul.Curso.Dados.Novo();
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
                id: "listaCursos",
                url: rootDir + "Curso/GetLista/",
                title: { text: "Cursos" },
                filter: { Entidades: "Situacao, Segmento" },
                request: { base: "Curso" }, // colocado pois o SQL não está com ALIAS para os campos da tabela Curso
                buttons: {
                    insert: { show: _permissoes.insert, click: NotaAzul.Curso.Lista.Novo },
                    update: { show: _permissoes.update, click: NotaAzul.Curso.Lista.Abrir },
                    remove: { show: _permissoes.remove, url: rootDir + "Curso/Excluir/" },
                    onlyView: { show: _permissoes.onlyView }
                },
                action: { onDblClick: NotaAzul.Curso.Lista.Abrir },
                columns: [
                    { header: "Nome", nameJson: "Nome" },
                    { header: "Segmento", nameJson: "Segmento.Nome" },
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
            _permissoes = Permissoes.CRUD("CURSO");

            _criarLista();
            _carregarJanelaCurso();
        };

        /**
        * @descrição: Método responsável por abrir uma popup para inserção de novo registro
        * @return: void
        **/
        var _novo = function () {
            _windowCurso.show({
                animate: true,
                fnBefore: NotaAzul.Curso.Dados.Novo
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

    NotaAzul.Curso.Lista.Iniciar();
} (window, document));