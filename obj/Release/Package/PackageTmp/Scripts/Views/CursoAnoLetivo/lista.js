/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.CursoAnoLetivo == null) {
        NotaAzul.CursoAnoLetivo = {};
    }

    NotaAzul.CursoAnoLetivo.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowCursoAnoLetivo = null,
            _permissoes = null;

        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (obj) {
            _windowCursoAnoLetivo.apply({
                object: 'CursoAnoLetivo',
                url: rootDir + "CursoAnoLetivo/Carregar/" + obj.Id,
                ajax: { onlyJson: true },
                fnBeforeLoad: NotaAzul.CursoAnoLetivo.Dados.Novo,
                success: function (retorno) {
                    // verifica se o objeto existe
                    if ((retorno == null) || (retorno.obj == null)) {
                        return;
                    }

                    var idCurso = retorno.obj.IdCurso;
                    Prion.ComboBox.SelecionarIndexPeloValue("comboboxCurso", idCurso);
                }
            }).load().show({ animate: true });
        };

        /**
        * @descrição: Carrega o conteúdo do html CursoAnoLetivo/ViewDados em background, acelerando o futuro carregamento
        **/
        var _carregarJanelaCursoAnoLetivo = function () {
            _windowCursoAnoLetivo = new Prion.Window({
                url: rootDir + "CursoAnoLetivo/ViewDados/",
                id: "popupCursoAnoLetivo",
                height: 370,
                width: 600,
                title: { text: "Cursos de um ano letivo" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.CursoAnoLetivo.Lista.Grid().load();
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
                                NotaAzul.CursoAnoLetivo.Dados.Salvar(win);
                            }
                        },
                        {
                            text: "Limpar",
                            className: Prion.settings.ClassName.buttonReset,
                            type: "reset",
                            click: function (win) {
                                NotaAzul.CursoAnoLetivo.Dados.Novo();
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
                id: "listaCursosAnoLetivo",
                url: rootDir + "CursoAnoLetivo/GetLista/",
                title: { text: "Cursos de um ano letivo" },
                filter: { Entidades: "Situacao, Curso" },
                request: { base: "CursoAnoLetivo" }, // colocado pois o SQL não está com ALIAS para os campos da tabela Curso
                buttons: {
                    insert: { show: _permissoes.insert, click: NotaAzul.CursoAnoLetivo.Lista.Novo },
                    update: { show: _permissoes.update, click: NotaAzul.CursoAnoLetivo.Lista.Abrir },
                    remove: { show: _permissoes.remove, url: rootDir + "Curso/Excluir/" },
                    onlyView: { show: _permissoes.onlyView }
                },
                action: { onDblClick: NotaAzul.CursoAnoLetivo.Lista.Abrir },
                columns: [
                    { header: "Ano Letivo", nameJson: "AnoLetivo", width: "80px" },
                    { header: "Curso", nameJson: "Curso.Nome", width: "200px" },
                    { header: "Valor da Matrícula", nameJson: "ValorMatricula", width: "120px", type: "float", currency: "R$", align: "right" },
                    { header: "Valor da Mensalidade", nameJson: "ValorMensalidade", width: "140px", type: "float", currency: "R$", align: "right" },
                    { header: "Qtd Mensalidades", nameJson: "QuantidadeMensalidades", width: "120px", align: "right" },
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
            _permissoes = Permissoes.CRUD("CURSO_ANO_LETIVO");

            _criarLista();
            _carregarJanelaCursoAnoLetivo();
        };

        /**
        * @descrição: Método responsável por abrir uma popup para inserção de novo registro
        * @return: void
        **/
        var _novo = function () {
            _windowCursoAnoLetivo.show({
                animate: true,
                fnBefore: NotaAzul.CursoAnoLetivo.Dados.Novo
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

    NotaAzul.CursoAnoLetivo.Lista.Iniciar();
} (window, document));