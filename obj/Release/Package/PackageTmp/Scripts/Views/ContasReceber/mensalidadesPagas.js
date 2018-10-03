/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.ContasReceber == null) {
        NotaAzul.ContasReceber = {};
    }

    NotaAzul.ContasReceber.MensalidadesPagas = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _permissoes = null,
            _windowRecibo = null;

        /**
        * @descrição: Carrega o conteúdo do html ContasReceber/Recibo em background, acelerando o futuro carregamento
        **/
        var _carregarRecibo = function () {

            var height = (window.innerHeight < 900) ? (window.innerHeight - 60) : 900;

            _windowRecibo = new Prion.Window({
                url: rootDir + "ContasReceber/ViewRecibo",
                id: "popupRecibo",
                height: height,
                title: { text: "Recibo" }
            });
        };

        /**
        * @descrição: Cria todo o Html da lista
        * @return: void
        **/
        var _criarLista = function () {
            if (_grid != null) { return; }

            _grid = new Prion.Lista({
                id: "listaMensalidadesPagas",
                url: rootDir + "ContasReceber/GetListaMensalidadesPagas/",
                width: 690,
                height: 300,
                request: { base: "Mensalidade" },
                buttons: {
                    update: { show: _permissoes.update, click: NotaAzul.ContasReceber.MensalidadesPagas.AbrirRecibo, tooltip: "Abrir recibo" },
                    remove: { show: _permissoes.remove, url: rootDir + "ContasReceber/Excluir/", tooltip: "Cancelar pagamento" },
                    onlyView: { show: _permissoes.onlyView }
                },
                action: { onDblClick: NotaAzul.ContasReceber.MensalidadesPagas.AbrirRecibo },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.ContasReceber.MensalidadesPagas.Grid().load();
                    }
                },

                columns: [
                    { header: "Id", nameJson: "Id", visible: false },
                    { header: "Aluno", nameJson: "Nome", width: '200px' },
                    { header: "Curso", nameJson: "Curso", width: '150px' },
                    { header: "Valor", nameJson: "Valor", mask: "money", width: '150px' },
                    { header: "Vencimento", nameJson: "Vencimento", type: "date", width: '150px' }
                ]
            });
        };

        /**
        * @descrição: Responsável pelo AutoComplete do campo Aluno
        * @return: void
        **/
        var _autoCompleteBuscarAluno = function () {
            $("#Filtro_Nome").autocomplete({
                source: rootDir + "Aluno/ListaAlunoPorNome",
                minLength: 2,
                appendTo: $("#Filtro_Nome").parent(),
                delay: 500,
                dataType: 'json',
                focus: true,
                open: function () {
                    $(".ui-autocomplete:visible").css({ top: "45px", left: "145px", "max-height": "160px", "overflow-y": "auto" });

                },
                select: function (event, data) {
                    "Filtro_Nome".setValue(data.item.label);
                    "Filtro".setValue(data.item.value);
                    return false;
                }
            });
        };

        /**
        * @descrição: Adiciona os eventos dos botões
        **/
        var _definirAcoesBotoes = function () {
            Prion.Event.add("btnAplicarFiltro", "click", function () {
                if (!Prion.ValidateForm("frmFiltro")) { return false; }
                _grid.load({
                    query: "{\"Filtros\":[{\"Campo\":\"Aluno.Id\",\"Operador\":\"=\",\"Valor\":\"" + "Filtro".getValue() + "\"}]}"
                });
                return true;
            });          
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            _permissoes = Permissoes.CRUD("CONTAS_RECEBER");
            _autoCompleteBuscarAluno();
            _definirAcoesBotoes();
            _carregarRecibo();
        };


        /**
        * @descrição: Método responsável por abrir a janela com o Recibo do pagamento
        * @params: obj
        * @return: void
        **/
        var _abrirRecibo = function (obj) {
            _carregarRecibo();
            _windowRecibo.apply({
                url: rootDir + "ContasReceber/CarregarPagamento/" + obj.Id,
                object: 'MensalidadeTitulo',
                ajax: { onlyJson: true },
                success: function (retorno) {
                    var objPagamento = JSON.parse(retorno.obj)[0];
                    NotaAzul.ContasReceber.MensalidadesPagas.GerarRecibo(objPagamento);
                }
            }).load();
        };


        /**
        * @descrição: Carrega todas as permissões já definidas para o grupo de usuário selecionado
        * @return: void
        **/
        var _carregarMensalidadesPagas = function () {
            NotaAzul.ContasReceber.Lista.WindowMensalidadePaga().apply({
                ajax: { onlyJson: true },
                fnBeforeLoad: function () {
                    _criarLista();
                }
            }).load().show({ animate: true });
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
            "Vencimento_Mensalidade".setValue(objPagamento.DataVencimento.toString().substr(0, 10));
            "Data_Pagamento".setValue(objPagamento.DataOperacao.toString().substr(0, 10));

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
        * @descrição: Método que retorna o objeto do grid
        * @return: Objeto grid
        **/
        var _getGrid = function () {
            return _grid;
        };

        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            Iniciar: _iniciar,
            AbrirRecibo: _abrirRecibo,
            CarregarMensalidadesPagas: _carregarMensalidadesPagas,
            GerarRecibo: _gerarRecibo,
            ImprimirRecibo: _imprimirRecibo,
            Grid: _getGrid
        };
    } ();
    NotaAzul.ContasReceber.MensalidadesPagas.Iniciar();
} (window, document));
   