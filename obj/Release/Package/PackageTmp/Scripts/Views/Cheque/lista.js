/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Cheque == null) {
        NotaAzul.Cheque = {};
    }

    NotaAzul.Cheque.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowCheque = null,
            _permissoes = null;


        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (obj) {
            _windowCheque.apply({
                object: 'Cheque',
                url: rootDir + "Cheque/Carregar/" + obj.Id,
                ajax: { onlyJson: true },
                fnBeforeLoad: NotaAzul.Cheque.Dados.Novo
            }).load().show({ animate: true });
        };

        /**
        * @descricao: Carrega o conteúdo do html Cheque/ViewDados em background, acelerando o futuro carregamento
        **/
        var _carregarJanelaCheque = function () {
            _windowCheque = new Prion.Window({
                url: rootDir + "Cheque/ViewDados/",
                id: "popupCheque",
                height: 380,
                width: 780,
                title: { text: "Cheque" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.Cheque.Lista.Grid().load();
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
                                NotaAzul.Cheque.Dados.Salvar(win);
                            }
                        },
                        {
                            text: "Limpar", className: Prion.settings.ClassName.buttonReset, type: "reset",
                            click: function (win) {
                                NotaAzul.Cheque.Dados.Novo();
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
                id: "listaCheques",
                url: rootDir + "Cheque/GetLista/",
                title: { text: "Cheques" },
                request: { base: "Cheque" }, // colocado pois o SQL não está com ALIAS para os campos da tabela Cheque
                buttons: {
                    insert: { show: _permissoes.insert, click: NotaAzul.Cheque.Lista.Novo },
                    update: { show: _permissoes.update, click: NotaAzul.Cheque.Lista.Abrir },
                    remove: { show: _permissoes.remove, url: rootDir + "Cheque/Excluir/" },
                    onlyView: { show: _permissoes.onlyView }
                },
                action: { onDblClick: NotaAzul.Cheque.Lista.Abrir },
                columns: [
                    { header: "Banco", nameJson: "Banco" },
                    { header: "Agência", nameJson: "Agencia", width: '200px' },
                    { header: "Valor", nameJson: "Valor", mask: "money", width: '200px' },
                    { header: "Bom Para", nameJson: "BomPara", type: "date", width: '200px' }
                ]
            });
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            _permissoes = Permissoes.CRUD("CHEQUE");

            _criarLista();
            _carregarJanelaCheque();
        };

        /**
        * @descrição: Método que retorna o objeto do grid
        * @return: Objeto grid
        **/
        var _getGrid = function () {
            return _grid;
        };

        /**
        * @descrição: Método responsável por abrir uma popup para inserção de novo registro
        * @return: void
        **/
        var _novo = function () {
            _windowCheque.show({
                animate: true,
                fnBefore: NotaAzul.Cheque.Dados.Novo
            });
        };

        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            Abrir: _abrir,
            Iniciar: _iniciar,
            Grid: _getGrid,
            Novo: _novo
        };
    } ();

    NotaAzul.Cheque.Lista.Iniciar();
} (window, document));


