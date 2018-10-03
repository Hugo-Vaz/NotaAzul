/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.ComposicaoNota == null) {
        NotaAzul.ComposicaoNota = {};
    }

    NotaAzul.ComposicaoNota.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowComposicaoNota = null,
            _permissoes = null;


        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (obj) {
            var nomeDisciplina = obj.Nome,
                idDisciplina = obj.IdProfessorDisciplina,
                idTurma = obj.IdTurma;

            _windowComposicaoNota.apply({
                object: 'ComposicaoNota',
                url: rootDir + "ComposicaoNota/Carregar/" + obj.IdProfessorDisciplina,
                ajax: { onlyJson: true },
                filter: { Entidades: "Funcionario,Disciplina,Professor,FormaAvaliacao,Nota" },
                success: function (retorno) {
                    NotaAzul.ComposicaoNota.Dados.Novo(nomeDisciplina, idDisciplina, idTurma);
                    // verifica se o objeto existe
                    if ((retorno != null) && (retorno.obj != null)) {
                        NotaAzul.ComposicaoNota.Dados.AplicarObjetoDeComposicaoNota(retorno.obj);
                    }
                }
            }).load().show({ animate: true });
        };

        /**
        * @descrição: Carrega o conteúdo do html ComposicaoNota/ViewDados em background, acelerando o futuro carregamento
        **/
        var _carregarJanelaComposicaoNota = function () {
            _windowComposicaoNota = new Prion.Window({
                url: rootDir + "ComposicaoNota/ViewDados/",
                id: "popupComposicaoNota",
                height: 600,
                width: 600,
                title: { text: "Disciplinas" },
                buttons: {
                    show: (!_permissoes.onlyView), // se onlyView for false, significa então que os botões deverão ser exibidos
                    buttons: [
                            {
                                text: "Salvar",
                                className: Prion.settings.ClassName.button,
                                typeButton: "save", // usado para controle interno
                                click: function (win) {
                                    NotaAzul.ComposicaoNota.Dados.Salvar(win);
                                }
                            }
                        ]
                }
            });
        };

        /**
        * @descrição: Carrega todas as turmas
        * @return: void
        **/
        var _carregarTurma = function () {
            Prion.ComboBox.Carregar({
                url: rootDir + "Turma/GetLista",
                el: "listaTurma",
                filter: "Entidades=CursoAnoLetivo&Paginar=false&CursoAnoLetivo.AnoLetivo=" + Prion.Date.AnoAtual(),
                clear: true,
                valueDefault: true
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
                id: "listaDisciplinas",
                url: rootDir + "ComposicaoNota/GetLista/",
                height: 300,
                title: { text: "Lista de disciplinas lecionadas" },
                request: { base: "Disciplina" }, // colocado pois o SQL não está com ALIAS para os campos da tabela Curso
                buttons: {
                    update: { show: _permissoes.update, click: NotaAzul.ComposicaoNota.Lista.Abrir, tooltip: "Adicionar Composição da Nota" },
                    onlyView: { show: _permissoes.onlyView }
                },
                rowNumber: {
                    show: false
                },
                action: { onDblClick: NotaAzul.ComposicaoNota.Lista.Abrir },
                columns: [
                    { header: "Nome", nameJson: "Nome", width: "150px" },
                    { header: "Cadastrado em", nameJson: "DataCadastro", type: "date", width: "110px" }
                ]
            });
        };

        /**
        * @descrição: Define a ação para todos os controles da tela
        * @return: void
        **/
        var _definirAcoesBotoes = function () {
            Prion.Event.add("btnAplicarFiltro", "click", function () {
                _grid.load({
                    url: rootDir + "ComposicaoNota/GetLista/",
                    filter: { Entidades: "Funcionario,Disciplina,Professor", "ProfessorDisciplina.IdTurma": Prion.ComboBox.Get("listaTurma").value },
                    paging: {
                        currentPage: 1,
                        totalPerPage: 40
                    }
                });
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
            _permissoes = Permissoes.CRUD("NOTAS_DISCIPLINA");
            _carregarTurma();
            _criarLista();
            _carregarJanelaComposicaoNota();
            _definirAcoesBotoes();
        };

        /**
        * @descrição: Método responsável por abrir uma popup para inserção de novo registro
        * @return: void
        **/
        var _novo = function () {
            _windowComposicaoNota.show({
                animate: true,
                fnBefore: NotaAzul.ComposicaoNota.Dados.Novo
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
    NotaAzul.ComposicaoNota.Lista.Iniciar();
} (window, document));