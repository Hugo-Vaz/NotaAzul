/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Material == null) {
        NotaAzul.Material = {};
    }

    NotaAzul.Material.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowMaterial = null,
            _permissoes = null;

        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (obj) {
            _windowMaterial.apply({
                object: 'Material',
                url: rootDir + "Material/Carregar/" + obj.Id,
                ajax: { onlyJson: true },
                fnBeforeLoad: NotaAzul.Material.Dados.Novo
            }).load().show({ animate: true });
        };

        /**
        * @descricao: Carrega o conteúdo do html Material/ViewDados em background, acelerando o futuro carregamento
        **/
        var _carregarJanelaMaterial = function () {
            _windowMaterial = new Prion.Window({
                url: rootDir + "Material/ViewDados/",
                id: "popupMaterial",
                height: 206,
                title: { text: "Material" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.Material.Lista.Grid().load();
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
                                NotaAzul.Material.Dados.Salvar(win);
                            }
                        },
                        {
                            text: "Limpar",
                            className: Prion.settings.ClassName.buttonReset,
                            type: "reset",
                            click: function (win) {
                                NotaAzul.Material.Dados.Novo();
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
                id: "listaMaterial",
                url: rootDir + "Material/GetLista/",
                title: { text: "Material" },
                filter: { Entidades: "Situacao" },
                request: { base: "Material" }, // colocado pois o SQL não está com ALIAS para os campos da tabela Material
                buttons: {
                    insert: { show: _permissoes.insert, click: NotaAzul.Material.Lista.Novo },
                    update: { show: _permissoes.update, click: NotaAzul.Material.Lista.Abrir },
                    remove: { show: _permissoes.remove, url: rootDir + "Material/Excluir/" },
                    onlyView: { show: _permissoes.onlyView }
                },
                action: { onDblClick: NotaAzul.Material.Lista.Abrir },
                columns: [
                    { header: "Nome", nameJson: "Nome" },
                    { header: "Status", nameJson: "Situacao.Nome" },
                    { header: "Cadastrado em", nameJson: "DataCadastro", type: "date" }
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
            _permissoes = Permissoes.CRUD("MATERIAL");

            _criarLista();
            _carregarJanelaMaterial();
        };

        /**
        * @descrição: Método responsável por abrir uma popup para inserção de novo registro
        * @return: void
        **/
        var _novo = function () {
            _windowMaterial.show({
                animate: true,
                fnBefore: NotaAzul.Material.Dados.Novo
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

    NotaAzul.Material.Lista.Iniciar();
} (window, document));