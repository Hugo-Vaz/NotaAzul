/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Aluno == null) {
        NotaAzul.Aluno = {};
    }

    NotaAzul.Aluno.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowAluno = null,
            _popupWindowAluno = null,
            _permissoes = null;

        /**
        * @descrição: Método responsável por abrir um registro
        * @params: objAluno => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (objAluno) {
            _windowAluno.apply({
                object: 'Aluno',
                url: rootDir + "Aluno/Carregar/" + objAluno.Id,
                filter: { Entidades: "AlunoResponsavel, Endereco, Telefone" },
                ajax: { onlyJson: true },
                fnBeforeLoad: function () {
                    NotaAzul.Aluno.Dados.Novo();
                    NotaAzul.Aluno.Responsavel.Lista.Grid().clear();
                    $("#tabsAluno").tabs("show", "#tabFichaFinanceira");
                },
                success: function (retorno) {
                    var listaResponsaveis = retorno.obj.Responsaveis;
                    NotaAzul.Aluno.Responsavel.Lista.Grid().addRow(listaResponsaveis);
                    "Aluno_DiaPagamento".getDom().setAttribute("oldValue", retorno.obj.DiaPagamento);
                }
            }).load().show({ animate: true });
        };

        /**
       * @descrição: Responsável pelo AutoComplete do campo Aluno
       * @return: void
       **/
        var _autoCompleteBuscarAluno = function () {
            $("#Filtro_NomeAluno").autocomplete({
                source: rootDir + "Aluno/ListaAlunoPorNome",
                minLength: 2,
                appendTo: $("#Filtro_NomeAluno").parent(),
                delay: 500,
                dataType: 'json',
                focus: true,
                open: function () {
                    $(".ui-autocomplete:visible").css({ top: "initial", left: "139px", "max-height": "160px", "overflow-y": "auto" });

                },
                select: function (event, data) {
                    "Filtro_NomeAluno".setValue(data.item.label);
                    "Filtro_Aluno".setValue(data.item.value);
                    return false;
                }
            });
        };

        /**
        * @descricao: Carrega o conteúdo do html Aluno/ViewDados em background, acelerando o futuro carregamento
        * @return: void
        **/
        var _carregarJanelaAluno = function () {

            // carrega a ViewDados de Aluno
            _windowAluno = new Prion.Window({
                url: rootDir + "Aluno/ViewDados/",
                id: "popupAluno",
                modal: true,
                height: 620,
                width: 800,
                title: { text: "Aluno" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.Aluno.Lista.Grid().load();
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
                                NotaAzul.Aluno.Dados.Salvar(win);
                            }
                        },
                        {
                            text: "Limpar", className: Prion.settings.ClassName.buttonReset, type: "reset",
                            click: function (win) {
                                NotaAzul.Aluno.Dados.Novo();
                            }
                        }
                    ]
                }
            });
        };


        /**
        * @descrição: Cria todo o Html da lista
        * @params: autoLoad (boolean): indica se irá ou não carregar os registros automaticamente
        * @return: void
        **/
        var _criarLista = function (config) {
            if (_grid != null) { return; }

            _grid = new Prion.Lista({
                id: config.id,
                url: rootDir + "Aluno/GetLista/",
                autoLoad: config.autoLoad,
                height: 500,
                title: { text: "Alunos" },
                filter: { Entidades: "Situacao" },
                request: { base: "Aluno" }, // colocado pois o SQL não está com ALIAS para os campos da tabela Aluno
                buttons: {
                    insert: { show: _permissoes.insert, click: NotaAzul.Aluno.Lista.Novo/* v2.0, depends: "NotaAzul.Aluno.Dados"*/ },
                    update: { show: _permissoes.update, click: NotaAzul.Aluno.Lista.Abrir/* v2.0, depends: "NotaAzul.Aluno.Dados"*/ },
                    remove: { show: _permissoes.remove, url: rootDir + "Aluno/Excluir/" },
                    onlyView: { show: _permissoes.onlyView }
                },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.Bairro.Lista.Grid().load();
                    }
                },
                action: { onDblClick: NotaAzul.Aluno.Lista.Abrir },
                columns: [
                    { header: "Nome", nameJson: "Nome", width: "300px" },
                    { header: "Sexo", nameJson: "Sexo", width: "60px" },
                    { header: "Grupo Sanguíneo", nameJson: "GrupoSanguineo", width: "110px" },
                    { header: "CPF", nameJson: "CPF", mask: "cpf" },
                    { header: "RG", nameJson: "RG" },
                    { header: "Data de Nascimento", nameJson: "DataNascimento", type: "date", width: "120px" },
                    { header: "Nacionalidade", nameJson: "Nacionalidade", width: "160px" },
                    { header: "Regilião", nameJson: "Religiao", width: "160px" },
                    { header: "Cor / Raça", nameJson: "CorRaca", width: "160px" },
                    { header: "Status", nameJson: "Situacao.Nome", width: "80px" },
                    { header: "Cadastrado em", nameJson: "DataCadastro", type: "date" }
                ]
            }, config);
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
        * @params: arg0 (boolean, opcional): Se for informado, carrega apenas a lista, porém não dará um load
        * @return: void
        **/
        var _iniciar = function (/**arg0*/) {
            _permissoes = Permissoes.CRUD("ALUNO");

            var config = (arguments[0] == null) ? { autoLoad: true, id: "listaAlunos"} : arguments[0];

            // cria a lista baseada em configurações informadas pelo usuário
            _criarLista(config);
            _autoCompleteBuscarAluno();
            _carregarJanelaAluno();

            Prion.Event.add("btnAplicarFiltroAluno", "click", function () {
                _grid.load({
                    query: "{\"Filtros\":[{\"Campo\":\"Aluno.Id\",\"Operador\":\"=\",\"Valor\":\"" + "Filtro_Aluno".getValue() + "\"}]}"
                });
            });
        };

        /**
        * @descrição: Método responsável por abrir uma popup para inserção de novo registro
        * @return: void
        **/
        var _novo = function () {
            _windowAluno.show({
                animate: true,
                fnBefore: function () {
                    NotaAzul.Aluno.Dados.Novo();
                    $("#tabsAluno").tabs("hide", "#tabFichaFinanceira");
                }
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

} (window, document));