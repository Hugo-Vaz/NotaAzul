/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.FormasPagamento == null) {
        NotaAzul.FormasPagamento = {};
    }

    NotaAzul.FormasPagamento = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _objBoleto = null,
            _objCartao = null,
            _objCheque = null;

        /**
        * @descrição: Atualiza o valor pago
        **/
        var _atualizarValorPago = function (obj) {
            var valor = 0;
            for (var i = 0; i < obj.length; i++) {
                valor = obj[i].Valor;
            }

            "Valor_Pago".setValue("Valor_Pago".getValue().toFloat() - valor);
        };

        /**
        * @descrição: Carrega os combobox desta janela
        * @return: void
        **/
        var _carregarCombobox = function () {
            "cartaoBandeiraLista".clear();
            NotaAzul.BandeiraCartao.ComboBox.Gerar({ id: "cartaoBandeiraLista" });
        };

        /**
        * @descrição: Compara o valor total com o valor pago
        * @return: boolean
        **/
        var _comparaValorPagoComValorTotal = function (valor) {
            //O valor total do título é comparado com o valor pago + o valor sendo adicionado no momento
            //Caso o valor seja o menor ou igual,retorna true
            if (("Valor_Pago".getValue().toFloat() + parseFloat(valor)) <= "Valor_Total".getValue().toFloat()) {
                return true;
            }

            if ("Valor_Pago".getValue().toFloat() === 0) {
                return window.confirm("Esse valor é superior ao total da conta, deseja continuar?");
            }

            //caso contrário um confirm perguntará ao usuário o que fazer
            return window.confirm("O valor total já foi alcançado, deseja continuar?");
        };

        /**
        * @descrição: Cria todo o Html da lista
        * @return: void
        **/
        var _criarLista = function () {
            if (_grid != null) { return; }

            _grid = new Prion.Lista({
                id: "ListaFormasDePagamento",
                height: 275,
                width: 300,
                style: "margin-right: 6px;position: relative;float: right;",
                title: { text: "Pagamento" },
                type: Prion.Lista.Type.Local,
                paging: { show: false },
                buttons: {
                    remove: { show: true, click: NotaAzul.FormasPagamento.AtualizarValorPago, filter: { Entidades: "Cartao,Cheque,Especie"} }
                },
                columns: [
                    { header: "Tipo", nameJson: "Tipo", width: "115px" },
                    { header: "Valor", nameJson: "Valor", mask: "money", width: "150px" }

                ],
                rowNumber: {
                    show: false
                }
            });

            _grid.load();
        };

        /**
        * @descrição: Define ação para todos os controles da tela 
        **/
        var _definirAcaoBotoes = function () {

            // evento do botão Salvar da aba Responsável
            $("#btnAdicionarCartao").click(function () {
                NotaAzul.FormasPagamento.SalvarCartao();
            });

            $("#btnAdicionarCheque").click(function () {
                NotaAzul.FormasPagamento.SalvarCheque();
            });

            $("#btnAdicionarBoleto").click(function () {
                NotaAzul.FormasPagamento.SalvarEspecie();
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
            _definirAcaoBotoes();
            _criarLista();
        };



        /**
        * @descrição: Reset o form, voltando os dados default
        **/
        var _novoPagamento = function (valor) {
            Prion.ClearForm("frmCartao");
            Prion.ClearForm("frmBoleto");
            Prion.ClearForm("frmCheque");
            NotaAzul.FormasPagamento.Grid().clear();
            _carregarCombobox();
            NotaAzul.Cheque.Dados.Novo();
            $("#tabsFormasPagamento").tabs({ selected: 0 });
            "Valor_Total".setValue(valor);
            "Valor_Pago".setValue(0);
        };

        /**
        * @descrição: Salva todos os dados do form
        **/
        var _salvar = function (win) {
            if ("Valor_Pago".getValue().toFloat() < "Valor_Total".getValue().toFloat()) {
                if (window.confirm("O valor total da conta a ser paga, ainda não foi atingido.Deseja efetuar o pagamento desta forma?") === false) {
                    return;
                }
            }

            win.hide({ animate: true });
            return _grid.rows();
        };

        /**
        * @descrição: Salva um objeto de cartão no grid
        **/
        var _salvarCartao = function () {
            if (!Prion.ValidateForm("frmCartao")) {
                return false;
            }

            _objCartao = Prion.FormToObject("frmCartao");
            _objCartao.Tipo = "Cartão";
            _objCartao.Valor = (_objCartao["Cartao.Valor"].substr(3)).toFloat();

            if (!_comparaValorPagoComValorTotal(_objCartao.Valor)) {
                return false;
            }

            "Valor_Pago".setValue(_objCartao.Valor + ("Valor_Pago".getValue().toFloat()));
            _grid.addRow(_objCartao);
            Prion.ClearForm("frmCartao");
        };

        /**
        * @descrição: Salva um objeto de cheque no grid
        **/
        var _salvarCheque = function () {
            if (!Prion.ValidateForm("frmCheque")) {
                return false;
            }
            _objCheque = Prion.FormToObject("frmCheque");
            _objCheque.Tipo = "Cheque";
            _objCheque.Valor = (_objCheque["Cheque.Valor"].substr(3)).toFloat();

            if (!_comparaValorPagoComValorTotal(_objCheque.Valor)) {
                return false;
            }

            "Valor_Pago".setValue(_objCheque.Valor + ("Valor_Pago".getValue().toFloat()));
            _grid.addRow(_objCheque);
            Prion.ClearForm("frmCheque");
        };

        /**
        * @descrição: Salva um objeto de espécie no grid
        **/
        var _salvarEspecie = function () {
            if (!Prion.ValidateForm("frmEspecie")) {
                return false;
            }

            _objBoleto = Prion.FormToObject("frmEspecie");
            _objBoleto.Tipo = "Espécie";
            _objBoleto.Valor = (_objBoleto["Especie.Valor"].substr(3)).toFloat();

            if (!_comparaValorPagoComValorTotal(_objBoleto.Valor)) {
                return false;
            }

            "Valor_Pago".setValue(_objBoleto.Valor + "Valor_Pago".getValue().toFloat());
            _grid.addRow(_objBoleto);
            Prion.ClearForm("frmEspecie");
        };
        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            AtualizarValorPago: _atualizarValorPago,
            Grid: _getGrid,
            Iniciar: _iniciar,
            Novo: _novoPagamento,
            Salvar: _salvar,
            SalvarCartao: _salvarCartao,
            SalvarCheque: _salvarCheque,
            SalvarEspecie: _salvarEspecie
        };

    } ();
    NotaAzul.FormasPagamento.Iniciar();
} (window, document));

