/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function () {
    "use strict";
    if (NotaAzul.Funcionario == null) {
        NotaAzul.Funcionario = {};
    }

    NotaAzul.Funcionario.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowFuncionario = null,
            _permissoes = null;


        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (obj) {
            _windowFuncionario.apply({
                object: 'Funcionario',
                url: rootDir + "Funcionario/Carregar/" + obj.Id,
                filter: { Entidades: "Telefone" },
                ajax: { onlyJson: true },
                fnBeforeLoad: NotaAzul.Funcionario.Dados.Novo,
                success: function (retorno) {
                    var arrObjTelefone = retorno.obj.Telefones;
                    Prion.Telefone.AplicarDadosAoForm(arrObjTelefone);
                }
            }).load().show({ animate: true });
        };

        /**
        * @descricao: Carrega o conteúdo do html Funcionario/ViewDados em background, acelerando o futuro carregamento
        **/
        var _carregarJanelaFuncionario = function () {
            _windowFuncionario = new Prion.Window({
                url: rootDir + "Funcionario/ViewDados/",
                id: "popupFuncionario",
                height: 610,
                width: 740,
                title: { text: "Funcionário" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.Funcionario.Lista.Grid().load();
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
                                NotaAzul.Funcionario.Dados.Salvar(win);
                            }
                        },
                        {
                            text: "Limpar",
                            className: Prion.settings.ClassName.buttonReset,
                            type: "reset",
                            click: function (win) {
                                NotaAzul.Funcionario.Dados.Novo();
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
                id: "listaFuncionarios",
                url: rootDir + "Funcionario/GetLista/",
                title: { text: "Funcionários" },
                filter: { Entidades: "Situacao, Cargo" },
                request: { base: "Funcionario" }, // colocado pois o SQL não está com ALIAS para os campos da tabela Funcionario
                buttons: {
                    insert: { show: _permissoes.insert, click: NotaAzul.Funcionario.Lista.Novo },
                    update: { show: _permissoes.update, click: NotaAzul.Funcionario.Lista.Abrir },
                    remove: { show: _permissoes.remove, url: rootDir + "Funcionario/Excluir/" },
                    onlyView: { show: _permissoes.onlyView }
                },
                action: { onDblClick: NotaAzul.Funcionario.Lista.Abrir },
                columns: [
                    { header: "Nome", nameJson: "Nome" },
                    { header: "CPF", nameJson: "CPF", mask: "cpf", width: "100px" },
                    { header: "RG", nameJson: "RG", width: "100px" },
                    { header: "Cargo", nameJson: "Cargo.Nome" },
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
            _permissoes = Permissoes.CRUD("FUNCIONARIO");

            _criarLista();
            _carregarJanelaFuncionario();
        };
        
        /**
        * @descrição: Método responsável por abrir uma popup para inserção de novo registro
        * @return: void
        **/
        var _novo = function () {
            _windowFuncionario.show({
                animate: true,
                fnBefore: NotaAzul.Funcionario.Dados.Novo
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

    NotaAzul.Funcionario.Lista.Iniciar();
} (window, document));