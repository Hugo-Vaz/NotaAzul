/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.DisciplinaTurma == null) {
        NotaAzul.DisciplinaTurma = {};
    }

    NotaAzul.DisciplinaTurma.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowDisciplinaTurma = null,
            _popupWindowDisciplinaTurma = null,
            _permissoes = null;


        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (obj) {
            _windowDisciplinaTurma.apply({
                object: 'Turma',
                url: rootDir + "DisciplinaTurma/Carregar/" + obj.Id,
                ajax: { onlyJson: true },
                filter: { Entidades: "Curso,Disciplina" },
                success: function (retorno) {
                    // verifica se o objeto existe
                    if ((retorno != null) && (retorno.obj != null)) {
                        NotaAzul.DisciplinaTurma.Dados.Novo(retorno.obj.Nome, retorno.obj.IdsDisciplina);
                    }
                }
            }).load().show({ animate: true });
        };

        /**
        * @descrição: Carrega o conteúdo do html DisciplinaTurma/ViewDados em background, acelerando o futuro carregamento
        **/
        var _carregarJanelaDisciplinaTurma = function () {
            _windowDisciplinaTurma = new Prion.Window({
                url: rootDir + "DisciplinaTurma/ViewDados/",
                id: "popupDisciplinaTurma",
                height: 310,
                width: 530,
                title: { text: "Disciplinas de uma turma de um ano letivo" },
                buttons: {
                    show: (!_permissoes.onlyView), // se onlyView for false, significa então que os botões deverão ser exibidos
                    buttons: [
                                {
                                    text: "Salvar",
                                    className: Prion.settings.ClassName.button,
                                    typeButton: "save", // usado para controle interno
                                    click: function (win) {
                                        NotaAzul.DisciplinaTurma.Dados.Salvar(win);
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
                autoLoad: false,
                id: "listaDisciplinaTurma",
                title: { text: "Turmas de um ano letivo" },
                filter: { Entidades: "Situacao, CursoAnoLetivo, Disciplina" },
                request: { base: "Turma" }, // colocado pois o SQL não está com ALIAS para os campos da tabela Curso
                buttons: {
                    update: { show: _permissoes.update, click: NotaAzul.DisciplinaTurma.Lista.Abrir, tooltip: "Adicionar disciplinas à Turma" },
                    onlyView: { show: _permissoes.onlyView }
                },
                rowNumber: {
                    show: false
                },
                action: { onDblClick: NotaAzul.DisciplinaTurma.Lista.Abrir },
                columns: [
                        { header: "Turma", nameJson: "Nome", width: "150px" },
                        { header: "Curso", nameJson: "Curso.Nome", width: "150px", align: "right" },
                        { header: "Cadastrado em", nameJson: "DataCadastro", type: "date", width: "110px" }
                    ]
            });
        };

        /**
        * @descrição: Define a ação para todos os controles da tela
        * @return: void
        **/
        var _definirAcoesBotoes = function () {

            //evento associado ao botão de salvar as configurações
            Prion.Event.add("btnBuscarTurma", "click", function () {
                _grid.load(null, "DisciplinaTurma/GetLista?AnoLetivo=" + "AnoLetivo".getDom().value);
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
            _permissoes = Permissoes.CRUD("DISCIPLINA_CURSO_ANO_LETIVO");
            "AnoLetivo".setValue(Prion.Date.AnoAtual());
            _criarLista();
            _carregarJanelaDisciplinaTurma();
            _definirAcoesBotoes();
        };
        
        /**
        * @descrição: Método responsável por abrir uma popup para inserção de novo registro
        * @return: void
        **/
        var _novo = function () {
            _windowDisciplinaTurma.show({
                animate: true,
                fnBefore: NotaAzul.DisciplinaTurma.Dados.Novo
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
    NotaAzul.DisciplinaTurma.Lista.Iniciar();
} (window, document));