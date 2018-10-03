/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$().ready(function (window, document) {
    "use strict";
    if (NotaAzul.ContasPagar == null) {
        NotaAzul.ContasPagar = {};
    }

    NotaAzul.ContasPagar.Dados = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
        };

        /**
        * @descrição: Método que será chamado sempre após Salvar
        **/
        var _cancelarPagamento = function (win) {

            if (window.confirm("Deseja cancelar esse pagamento?") === false) {
                return;
            }
            Prion.Request({
                url: rootDir + "ContasPagar/Excluir",
                data: { id: "TituloPago_Id".getValue() },
                success: function () {
                    win.config.reloadObserver = true;
                    win.hide({ animate: true });
                    return;
                }
            });
        };

        /**
        * @descrição: Reset o form, voltando os dados default
        **/
        var _novoContasPagar = function () {
            Prion.ClearForm("frmContasPagar", true);
        };
        
        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            Iniciar: _iniciar,
            CancelarPagamento: _cancelarPagamento,
            Novo: _novoContasPagar
        };

    } ();

    NotaAzul.ContasPagar.Dados.Iniciar();
} (window, document));

