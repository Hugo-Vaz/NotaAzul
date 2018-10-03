/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.ContasPagar == null) {
        NotaAzul.ContasPagar = {};
    }

    NotaAzul.ContasPagar.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowContasPagar = null,
            _windowTituloPago = null,
            _permissoes = null;


        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (obj) {
            _windowTituloPago.apply({
                object: 'Titulo',
                url: rootDir + "ContasPagar/Carregar/" + obj.Id,
                ajax: { onlyJson: true },
                fnBeforeLoad: NotaAzul.ContasPagar.Dados.Novo()
            }).load().show({ animate: true });
        };

        /**
        * @descricao: Carrega o conteúdo do html ContasPagar/ViewPagamento em background, acelerando o futuro carregamento
        **/
        var _carregarJanelaContasPagar = function () {
            _windowContasPagar = new Prion.Window({
                url: rootDir + "ContasPagar/ViewPagamento/",
                id: "popupPagarTitulo",
                height: 470,
                width: 640,
                title: { text: "Títulos à Pagar" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.ContasPagar.Lista.Grid().load();
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
                                NotaAzul.ContasPagar.Pagamento.Salvar(win);
                            }
                        },
                        {
                            text: "Limpar", className: Prion.settings.ClassName.buttonReset, type: "reset",
                            click: function (win) {
                                NotaAzul.ContasPagar.Pagamento.Novo();
                            }
                        }
                    ]
                }
            });
        };

        /**
        * @descricao: Carrega o conteúdo do html ContasPagar/ViewDados em background, acelerando o futuro carregamento
        **/
        var _carregarJanelaTituloPago = function () {
            _windowTituloPago = new Prion.Window({
                url: rootDir + "ContasPagar/ViewDados/",
                id: "popupTituloPago",
                height: 430,
                width: 640,
                title: { text: "Título Pago" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.ContasPagar.Lista.Grid().load();
                    }
                },
                buttons: {
                    show: (!_permissoes.onlyView), // se onlyView for false, significa então que os botões deverão ser exibidos
                    buttons: [
                        {
                            text: "Cancelar Pagamento", className: Prion.settings.ClassName.buttonReset, type: "reset",
                            click: function (win) {
                                NotaAzul.ContasPagar.Dados.CancelarPagamento(win);
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
                id: "listaContasPagar",
                url: rootDir + "ContasPagar/GetLista/",
                title: { text: "Contas à Pagar" },
                filter: { Entidades: "Situacao" },
                request: { base: "ContasPagar" }, // colocado pois o SQL não está com ALIAS para os campos da tabela ContasPagar
                buttons: {
                    insert: { show: _permissoes.insert, click: NotaAzul.ContasPagar.Lista.Novo },
                    update: { show: _permissoes.update, click: NotaAzul.ContasPagar.Lista.Abrir, tooltip: "Visualizar título pago" },
                    remove: { show: _permissoes.remove, url: rootDir + "ContasPagar/Excluir/", tooltip: "Cancelar pagamento" },
                    onlyView: { show: _permissoes.onlyView }
                },
                action: { onDblClick: NotaAzul.ContasPagar.Lista.Abrir },
                columns: [
                    { header: "Descrição", nameJson: "Descricao" },
                    { header: "Valor", nameJson: "Valor", mask: "money", width: '200px' },
                    { header: "Desconto", nameJson: "Desconto", mask: "money", width: '200px' },
                    { header: "Vencimento", nameJson: "DataVencimento", type: "date", width: '200px' }
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
            _permissoes = Permissoes.CRUD("CONTAS_PAGAR");

            _criarLista();
            _carregarJanelaContasPagar();
            _carregarJanelaTituloPago();
        };

        /**
        * @descrição: Método responsável por abrir uma popup para inserção de novo registro
        * @return: void
        **/
        var _novo = function () {
            _windowContasPagar.show({
                animate: true,
                fnBefore: NotaAzul.ContasPagar.Pagamento.Novo
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
    NotaAzul.ContasPagar.Lista.Iniciar();
} (window, document));


