/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$().ready(function (window, document) {
    "use strict";
    if (NotaAzul.ContasReceber == null) {
        NotaAzul.ContasReceber = {};
    }

    NotaAzul.ContasReceber.Pagamento = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/

        var _gridMensalidadesAbertas = null,
            _gridPagamento = null,
            _windowFormaPagamento = null,
            _windowRecibo = null,
            _objPagamento = null;


        /**
        * @descrição: Método responsável por abrir a janela com o Recibo do pagamento
        * @params: obj
        * @return: void
        **/
        var _abrirRecibo = function () {
            var obj = NotaAzul.ContasReceber.Pagamento.ObjPagamento();

            if (obj == null) {
                Prion.Alert({ msg: "Erro ao obter do Pagamento." });
                return;
            }

            _windowRecibo.apply({
                object: 'MensalidadeTitulo',
                ajax: { onlyJson: true },
                fnAfterLoad: NotaAzul.ContasReceber.Pagamento.GerarRecibo(obj)
            }).load();
        };

        /**
        * @descricao: Carrega o conteúdo do html ContasPagar/ViewFormaPagamento em background, acelerando o futuro carregamento
        **/
        var _carregarJanelaFormaPagamento = function () {
            _windowFormaPagamento = new Prion.Window({
                url: rootDir + "ContasReceber/ViewFormaPagamento/",
                id: "popupFormaPagamento",
                height: 510,
                width: 1120,
                title: { text: "Formas de Pagamento" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.ContasReceber.Lista.Grid().load();
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
                            _objPagamento.FormasPagamento = NotaAzul.FormasPagamento.Salvar(win);
                            var totalPago = 0;
                            var quantidadeFormasPagamento = _objPagamento.FormasPagamento.length;
                            for (var i = 0; i < quantidadeFormasPagamento; i++) {
                                totalPago += _objPagamento.FormasPagamento[i].Valor;
                            }
                            _objPagamento.ValorPago = totalPago;
                            _gridPagamento.addRow(_objPagamento);
                        }
                    }
                ]
                }
            });
        };

        /**
        * @descrição: Carrega o conteúdo do html ContasReceber/Recibo em background, acelerando o futuro carregamento
        **/
        var _carregarRecibo = function () {
            if (_windowRecibo != null) { return; }

            var height = (window.innerHeight < 900) ? (window.innerHeight - 60) : 900;

            _windowRecibo = new Prion.Window({
                url: rootDir + "ContasReceber/ViewRecibo",
                id: "popupRecibo",
                height: height,
                title: { text: "Recibo" }
            });
        };

        /**
        * @descrição: Cria todo o Html da lista de mensalidades abertas
        * @return: void
        **/
        var _criarListaMensalidadesAbertas = function () {
            if (_gridMensalidadesAbertas != null) { return; }

            _gridMensalidadesAbertas = new Prion.Lista({
                id: "ListaMensalidadesAbertas",
                width: 725,
                style: "margin-left:5px",
                title: { text: "Mensalidades" },
                type: Prion.Lista.Type.Local,
                filter: { Entidades: "Titulo" },
                paging: { show: false, totalPerPage: 6 },
                columns: [
                    { header: "Curso", nameJson: "NomeCurso" },
                    { header: "Valor", nameJson: "Valor", mask: "money", width: "100px" },
                    { header: "Acréscimo", nameJson: "Acrescimo", mask: "money", width: "100px" },
                    { header: "Desconto", nameJson: "Desconto", mask: "money", width: "100px" },
                    { header: "Data de Vencimento", nameJson: "DataVencimento", type: "date", width: "120px" }
                ],
                rowNumber: {
                    show: false
                }
            });

            _gridMensalidadesAbertas.load();
        };

        /**
        * @descrição: Cria todo o Html da lista de mensalidades abertas
        * @return: void
        **/
        var _criarListaPagamento = function () {
            if (_gridPagamento != null) { return; }

            _gridPagamento = new Prion.Lista({
                id: "ListaTitulosAPagar",
                height: 150,
                width: 605,
                style: "margin-left:5px",
                title: { text: "Pagamento" },
                type: Prion.Lista.Type.Local,
                paging: { show: false },
                buttons: {
                    remove: { show: true }
                },
                columns: [
                    { header: "Quantidade de Mensalidades pagas", nameJson: "TotalMensalidades" },
                    { header: "Valor Total", nameJson: "ValorTotal", mask: "money", width: "100px" },
                    { header: "Valor Pago", nameJson: "ValorPago", mask: "money", width: "100px" }
                ],
                rowNumber: {
                    show: false
                }
            });

            _gridPagamento.load();
        };


        /**
        * @descrição: Define ação para todos os controles da tela 
        **/
        var _definirAcaoBotoes = function () {

            // evento do botão Adicionar Título 
            $("#btnPagarMensalidade").click(function () {
                _objPagamento.DataOperacao = "DataOperacao".getValue();
                _objPagamento.FormasPagamento = null;
                _objPagamento.Mensalidades = NotaAzul.ContasReceber.Pagamento.GridMensalidade().getSelected();
                _objPagamento.ValorPago = 0;

                var quantidadeDeMensalidades = _objPagamento.Mensalidades.length,
                    dataVencimento, dataOperacao;
                _objPagamento.TotalMensalidades = quantidadeDeMensalidades;

                var valor = 0;

                for (var i = 0; i < quantidadeDeMensalidades; i++) {
                    dataOperacao = new Date(parseInt(_objPagamento.DataOperacao.substring(6, 10), 10), parseInt(_objPagamento.DataOperacao.substring(3, 5), 10) - 1, parseInt(_objPagamento.DataOperacao.substring(0, 2), 10));
                    dataVencimento = new Date(parseInt(_objPagamento.Mensalidades[i].object.DataVencimento.substr(6, 13), 10));

                    if (dataVencimento.getDay() == 0) {
                        //caso seja domingo será adicionado 24 horas ao vencimento ou seja o vencimento passará para segunda feira
                        dataVencimento.setHours(24);
                    } else if (dataVencimento.getDay() == 1) {
                        //caso seja sábado será adicionado 48 horas ao vencimento ou seja o vencimento passará para segunda feira
                        dataVencimento.setHours(48);
                    }
                    //Caso a data de vencimento seja maior que a data de operação, o valor será incluso com desconto
                    valor += (dataOperacao <= dataVencimento)
                        ? _objPagamento.Mensalidades[i].object.Valor + _objPagamento.Mensalidades[i].object.Acrescimo - _objPagamento.Mensalidades[i].object.Desconto
                        : _objPagamento.Mensalidades[i].object.Valor + _objPagamento.Mensalidades[i].object.Acrescimo;
                }

                _objPagamento.ValorTotal = valor;

                _windowFormaPagamento.show({
                    animate: true,
                    fnBefore: NotaAzul.FormasPagamento.Novo(valor)
                });

            });

        };

        /**
        * @descrição: Método que retorna o objeto do grid
        * @return: Objeto grid
        **/
        var _getGridMensalidade = function () {
            return _gridMensalidadesAbertas;
        };

        /**
        * @descrição: Método que retorna o objeto do grid
        * @return: Objeto grid
        **/
        var _getGridPagamento = function () {
            return _gridPagamento;
        };

        /**
        * @descrição: Método que retorna o objeto do pagamento
        * @return: _objPagamento
        **/
        var _getObjPagamento = function () {
            return _objPagamento;
        };

        /**
        * @descrição: Gera o Recibo de um pagamento 
        * @params: objPagamento => objeto com os dados correspondentes ao pagamento que acabou de ser efetuado
        * @return: void
        **/
        var _gerarRecibo = function (objPagamento) {
            "Nome_Aluno".setValue(objPagamento.Nome);
            "Nome_Turno".setValue(objPagamento.Turno);
            "Nome_Curso".setValue(objPagamento.Curso);
            "Recibo_ValorTotal".setValue(objPagamento.ValorTotal);
            "Recibo_ValorPago".setValue(objPagamento.ValorPago);
            "Data_Pagamento".setValue(objPagamento.DataOperacao);

            var quantidadeMensalidades = objPagamento.Mensalidades.length;
            var vencimentos = "";
            for (var i = 0; i < quantidadeMensalidades; i++) {
                vencimentos += Prion.Format(objPagamento.Mensalidades[i].object.DataVencimento, "dd/mm/aaaa");
                vencimentos += (i == quantidadeMensalidades - 1) ? "" : " , ";
            }
            "Vencimento_Mensalidade".setValue(vencimentos);
            var divReciboClonado = "Conteudo_Recibo".getDom().cloneNode(true);
            "Recibo_Pagamento".getDom().appendChild(divReciboClonado);

            _imprimirRecibo();
        };

        /**
        * @descrição: Método utilizado para imprimir apenas os carnês de Mensalidade
        **/
        var _imprimirRecibo = function () {
            var printContents = document.getElementById("Recibo_Pagamento").innerHTML;
            var mywindow = window.open("", "CarneMensalidade", "");
            var css = "<link rel='stylesheet' type='text/css' href='Content/notaAzul.css' media='print' />";

            var html = "<html><head><title>Carnê de Mensalidades</title>" + css + "</head><body >" +
                        printContents +
                        "</body></html>";

            mywindow.document.write(html);
            mywindow.print();
            mywindow.close();
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            _criarListaMensalidadesAbertas();
            _criarListaPagamento();
            _definirAcaoBotoes();
            _carregarJanelaFormaPagamento();
        };

        /**
        * @descrição: Reset o form, voltando os dados default
        **/
        var _novoPagamento = function () {
            NotaAzul.ContasReceber.Pagamento.GridPagamento().clear();
            var dataAtual = new Date();
            dataAtual = dataAtual.toFormat("dd/MM/yyyy HH:mm");
            "DataOperacao".setValue(dataAtual);
            _objPagamento = {};
            _carregarRecibo();

            //Remove o height default do grid
            ("ListaMensalidadesAbertas").getDom().removeAttribute("style", "height");

        };

        /**
        * @descrição: Salva todos os dados do form
        **/
        var _salvar = function (win) {

            if (win != null) { win.mask(); }
            if (NotaAzul.ContasReceber.Pagamento.GridPagamento().rows().length < 1) {
                window.alert("Nenhuma mensalidade está sendo paga");
                win.config.reloadObserver = false;
                win.mask();
                return false;
            }
            Prion.Request({
                url: rootDir + "ContasReceber/Salvar",
                data: "MensalidadesPagas=" + _gridPagamento.serialize(),
                success: function (retorno, registroNovo) {
                    win.config.reloadObserver = true;
                    win.hide({ animate: true });
                    NotaAzul.ContasReceber.Pagamento.AbrirRecibo();
                    return;
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
        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            Iniciar: _iniciar,
            AbrirRecibo: _abrirRecibo,
            Novo: _novoPagamento,
            GridMensalidade: _getGridMensalidade,
            GridPagamento: _getGridPagamento,
            GerarRecibo: _gerarRecibo,
            ImprimirRecibo: _imprimirRecibo,
            ObjPagamento: _getObjPagamento,
            Salvar: _salvar
        };
    } ();

    NotaAzul.ContasReceber.Pagamento.Iniciar();
} (window, document));
