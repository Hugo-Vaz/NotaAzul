/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Professor == null) {
        NotaAzul.Professor = {};
    }

    NotaAzul.Professor.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowProfessor = null,
            _popupWindowProfessor = null,
            _permissoes = null;

        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (obj) {
            _windowProfessor.apply({
                object: 'Professor',
                url: rootDir + "Professor/Carregar/" + obj.Id,
                ajax: { onlyJson: true },
                filter: { Entidades: "Funcionario,Disciplina,Professor" },
                success: function (retorno) {
                    // verifica se o objeto existe
                    if ((retorno != null) && (retorno.obj != null)) {
                        NotaAzul.Professor.Dados.Novo(retorno.obj.Funcionario.Id, retorno.obj.Funcionario.Nome, retorno.obj.Disciplinas);
                    }
                }
            }).load().show({ animate: true });
        };

        /**
        * @descrição: Carrega o conteúdo do html Professor/ViewDados em background, acelerando o futuro carregamento
        **/
        var _carregarJanelaProfessor = function () {
            _windowProfessor = new Prion.Window({
                url: rootDir + "Professor/ViewDados/",
                id: "popupProfessor",
                height: 410,
                width: 530,
                title: { text: "Disciplinas de um Professor" },
                buttons: {
                    show: (!_permissoes.onlyView), // se onlyView for false, significa então que os botões deverão ser exibidos
                    buttons: [
                            {
                                text: "Salvar",
                                className: Prion.settings.ClassName.button,
                                typeButton: "save", // usado para controle interno
                                click: function (win) {
                                    NotaAzul.Professor.Dados.Salvar(win);
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
                id: "listaProfessor",
                url: rootDir + "Professor/GetLista/",
                title: { text: "Cursos de um Professor" },
                filter: { Entidades: "Situacao, Curso, Disciplina" },
                request: { base: "Funcionario" }, // colocado pois o SQL não está com ALIAS para os campos da tabela Curso
                buttons: {
                    update: { show: _permissoes.update, click: NotaAzul.Professor.Lista.Abrir, tooltip: "Adicionar disciplinas ao Professor" },
                    onlyView: { show: _permissoes.onlyView }
                },
                rowNumber: {
                    show: false
                },
                action: { onDblClick: NotaAzul.Professor.Lista.Abrir },
                columns: [
                    { header: "Nome", nameJson: "Nome", width: "150px" },
                //{ header: "Disciplinas", nameJson: "Disciplinas", width: "300px", align: "right" },
                    {header: "Cadastrado em", nameJson: "DataCadastro", type: "date", width: "110px" }
                ]
            });
        };

        /**
        * @descrição: Define a ação para todos os controles da tela
        * @return: void
        **/
        var _definirAcoesBotoes = function () {
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
            _permissoes = Permissoes.CRUD("PROFESSOR");

            _criarLista();
            _carregarJanelaProfessor();
            _definirAcoesBotoes();
        };

        /**
        * @descrição: Método responsável por abrir uma popup para inserção de novo registro
        * @return: void
        **/
        var _novo = function () {
            _windowProfessor.show({
                animate: true,
                fnBefore: NotaAzul.Professor.Dados.Novo
            });
        };

        _iniciar();
        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            Abrir: _abrir,
            Grid: _getGrid,
            Iniciar:_iniciar,
            Novo: _novo
        };
    } ();
    NotaAzul.Professor.Lista.Iniciar();   
} (window, document));