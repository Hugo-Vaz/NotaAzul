/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.ContasPagar == null) {
        NotaAzul.ContasPagar = {};
    }

    NotaAzul.ContasPagar.Pagamento = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/

        var _grid = null,
            _objTitulo = null,
            _windowFormaPagamento = null;

        /**
        * @descrição: Responsável pelo AutoComplete
        * @return: void
        **/
        var _autoCompleteBuscarTitulos = function () {
            $("#Titulo_Descricao").autocomplete({
                source: rootDir + "Titulo/ListaTituloEmAbertoPorDescricao",
                minLength: 2,
                appendTo: $("#Titulo_Descricao").parent(),
                delay: 500,
                dataType: 'json',
                focus: true,
                open: function () {
                    $(".ui-autocomplete:visible").css({ top: "45px", left: "128px" });
                },
                select: function (event, data) {
                    "Titulo_Descricao".setValue(data.item.label);
                    _objTitulo = data.item.value;
                    return false;
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
                id: "ListaTitulosAPagar",
                height: 160,
                width: 605,
                style: "margin-left:5px",
                title: { text: "Títulos" },
                type: Prion.Lista.Type.Local,
                paging: { show: false },
                // action: { onDblClick: NotaAzul.Titulo.Lista.Abrir },
                buttons: {
                    remove: { show: true }
                },
                columns: [
                    { header: "Título", nameJson: "Descricao" },
                    { header: "Valor", nameJson: "Valor", mask: "money", width: "100px" },
                    { header: "Data de Vencimento", nameJson: "DataVencimento", type: "date", width: "120px" }
                ],
                rowNumber: {
                    show: false
                }
            });

            _grid.load();
        };

        /**
        * @descricao: Carrega o conteúdo do html ContasPagar/ViewFormaPagamento em background, acelerando o futuro carregamento
        **/
        var _carregarJanelaFormaPagamento = function () {
            _windowFormaPagamento = new Prion.Window({
                url: rootDir + "ContasPagar/ViewFormaPagamento/",
                id: "popupFormaPagamento",
                height: 480,
                width: 1120,
                title: { text: "Formas de Pagamento" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.ContasPagar.Lista.Grid().load();
                    }
                },
                buttons: {
                    show: (true), // se onlyView for false, significa então que os botões deverão ser exibidos
                    buttons: [
                    {
                        text: "Salvar",
                        className: Prion.settings.ClassName.button,
                        typeButton: "save", // usado para controle interno
                        click: function (win) {
                            _objTitulo.FormasPagamento = NotaAzul.FormasPagamento.Salvar(win);
                            var totalPago = 0;
                            var quantidadeFormasPagamento = _objTitulo.FormasPagamento.length;
                            for (var i = 0; i < quantidadeFormasPagamento; i++) {
                                totalPago += _objTitulo.FormasPagamento[i].Valor;
                            }
                            _objTitulo.ValorPago = totalPago;
                            _grid.addRow(_objTitulo);
                        }
                    }
                ]
                }
            });
        };

        /**
        * @descrição: Define ação para todos os controles da tela 
        **/
        var _definirAcaoBotoes = function () {

            // evento do botão Adicionar Título 
            $("#btnPagarTitulolSalvar").click(function () {
                _objTitulo.DataOperacao = "Titulo_DataOperacao".getValue();
                _objTitulo.FormasPagamento = null;

                // verifica se o título já foi adicionado na lista
                if (!_verificarSePodeAdicionarTitulo()) {
                    window.alert("Título já selecionado");
                    return;
                }
                _windowFormaPagamento.show({
                    animate: true,
                    fnBefore: NotaAzul.FormasPagamento.Novo(_objTitulo.Valor)
                });
                "Titulo_Descricao".setValue("");
            });

        };

        /**
        * @descrição: Método que será chamado sempre após Salvar
        **/
        var _executarAposSalvar = function (retorno) {
            NotaAzul.ContasPagar.Pagamento.Novo();
            return false;
        };

        /**
        * @descrição: Método que retorna o objeto do grid
        * @return: Objeto grid
        **/
        var _getGrid = function () {
            return _grid;
        };

        /**
        * @descrição: Método que retorna o objeto de Título a ser inserido
        * @return: Objeto objTitulo
        **/
        var _getObjTitulo = function () {
            return _objTitulo;
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            NotaAzul.ContasPagar.Pagamento.AutoCompleteBuscarTitulos();
            _criarLista();
            _definirAcaoBotoes();
            _carregarJanelaFormaPagamento();
        };

        /**
        * @descrição: Reset o form, voltando os dados default
        **/
        var _novoPagamento = function () {
            Prion.ClearForm("frmPagarTitulos", true);
            _objTitulo = null;
            var dataAtual = new Date();
            _grid.clear();
            dataAtual = dataAtual.toFormat("dd/MM/yyyy HH:mm");
            "Titulo_DataOperacao".setValue(dataAtual);
        };

        /**
        * @descrição: Salva todos os dados do form
        **/
        var _salvar = function (win) {
            if (!Prion.ValidateForm("frmPagarTitulos")) {
                win.config.reloadObserver = false;
                return false;
            }

            if (win != null) { win.mask(); }

            Prion.Request({
                url: rootDir + "ContasPagar/Salvar",
                data: "titulosPagos=" + _grid.serialize(),
                success: function (retorno, registroNovo) {
                    // se for um registro existente, fecha a janela
                    if ((!registroNovo) && (win != null)) {
                        win.config.reloadObserver = true;
                        win.hide({ animate: true });
                        return;
                    }

                    NotaAzul.ContasPagar.Pagamento.ExecutarAposSalvar();
                    if (win != null) {
                        win.config.reloadObserver = true;
                        win.mask();
                    }
                },
                error: function () {
                    if (win != null) {
                        win.config.reloadObserver = false;
                        win.mask();
                    }
                }
            });

            return false;
        };

        /**
        * @descrição: Método que insere no objeto de Título a ser inserido, um novo valor
        * @return: Objeto objTitulo
        **/
        var _setObjTitulo = function (value) {
            _objTitulo = value;
        };


        /**
        *@descrição: Verifica a se o título já foi adicionado ao grid
        **/
        var _verificarSePodeAdicionarTitulo = function () {
            var length = NotaAzul.ContasPagar.Pagamento.Grid().rows().length;

            for (var i = 0; i < length; i++) {
                // verifica se o id do título selecionado é igual ao id do título do grid
                // E
                // se o título da iteração atual NÃO esta marcado como excluido
                // se cair em um desses dois casos, retorna false informando que o título não pode mais ser selecionado
                if ((_objTitulo.Id == NotaAzul.ContasPagar.Pagamento.Grid().get(i).Id) &&
                     (NotaAzul.ContasPagar.Pagamento.Grid().get(i).EstadoObjeto != EstadoObjeto.Excluido)
                   ) {
                    return false;
                }
            }

            return true;
        };        
        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            /**
            * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
            * @return: void
            **/
            Iniciar: function () {
                NotaAzul.ContasPagar.Pagamento.AutoCompleteBuscarTitulos();
                _criarLista();
                _definirAcaoBotoes();
                _carregarJanelaFormaPagamento();
            },

            /**
            * @descrição: Responsável pelo AutoComplete
            * @return: void
            **/
            AutoCompleteBuscarTitulos: function () {
                $("#Titulo_Descricao").autocomplete({
                    source: rootDir + "Titulo/ListaTituloEmAbertoPorDescricao",
                    minLength: 2,
                    appendTo: $("#Titulo_Descricao").parent(),
                    delay: 500,
                    dataType: 'json',
                    focus: true,
                    open: function () {
                        $(".ui-autocomplete:visible").css({ top: "45px", left: "128px" });
                    },
                    select: function (event, data) {
                        "Titulo_Descricao".setValue(data.item.label);
                        _objTitulo = data.item.value;
                        return false;
                    }
                });
            },


            /**
            * @descrição: Método que será chamado sempre após Salvar
            **/
            ExecutarAposSalvar: function (retorno) {
                NotaAzul.ContasPagar.Pagamento.Novo();
                return false;
            },

            /**
            * @descrição: métodos que serão executados quando for abrir para novo registro
            **/
            Novo: function () {
                _novoPagamento();
            },

            /**
            * @descrição: Método que retorna o objeto do grid
            * @return: Objeto grid
            **/
            Grid: function () {
                return _grid;
            },

            /**
            * @descrição: Método que retorna o objeto de Título a ser inserido
            * @return: Objeto objTitulo
            **/
            ObjTitulo: function () {
                return _objTitulo;
            },

            /**
            * @descrição: Método que insere no objeto de Título a ser inserido, um novo valor
            * @return: Objeto objTitulo
            **/
            SetObjTitulo: function (value) {
                _objTitulo = value;
            },

            /**
            * @descrição: Salva todos os dados do form
            **/
            Salvar: function (win) {
                if (!Prion.ValidateForm("frmPagarTitulos")) {
                    win.config.reloadObserver = false;
                    return false;
                }


                if (win != null) { win.mask(); }

                Prion.Request({
                    url: rootDir + "ContasPagar/Salvar",
                    data: "titulosPagos=" + _grid.serialize(),
                    success: function (retorno, registroNovo) {
                        // se for um registro existente, fecha a janela
                        if ((!registroNovo) && (win != null)) {
                            win.config.reloadObserver = true;
                            win.hide({ animate: true });
                            return;
                        }

                        NotaAzul.ContasPagar.Pagamento.ExecutarAposSalvar();
                        if (win != null) {
                            win.config.reloadObserver = true;
                            win.mask();
                        }
                    },
                    error: function () {
                        if (win != null) {
                            win.config.reloadObserver = false;
                            win.mask();
                        }
                    }
                });

                return false;
            }
        };

    } ();

    NotaAzul.ContasPagar.Pagamento.Iniciar();
} (window, document));

