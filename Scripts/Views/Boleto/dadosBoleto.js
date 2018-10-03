/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {

    if (NotaAzul.Boleto == null) {
        NotaAzul.Boleto = {};
    }

    NotaAzul.Boleto.DadosBoleto = function () {
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
        * @descrição: Salva todos os dados do form
        **/
        var _salvar = function (win) {
            if (!Prion.ValidateForm("frmBoleto")) {
                win.config.reloadObserver = false;
                return false;
            }

            if (win != null) { win.mask(); }

            var valor = "Boleto_Valor".getDom().value.split(' ')[1].replace(',', '.'),
                desconto = "Boleto_Desconto".getDom().value.split(' ')[1].replace(',', '.'),
                replicar = "Replicar_Boletos".getDom().checked,
                id = "Boleto_Id".getDom().value;
            var url = "idBoleto=" + id + "&valor=" + valor + "&desconto=" + desconto + "&replicar=" + replicar

            $.ajax({
                type: "GET",
                url: rootDir + "Boleto/Salvar?" + url,
                contentType: false,
                processData: false,
               // data: { idBoleto: "Boleto_Id".getDom().value, valor: valor, desconto: desconto, replicar: replicar },
                success: function (result) {
                    if (result.success) {
                        win.config.reloadObserver = true;
                        win.hide({ animate: true });
                        return;

                    }
                    else {
                        if (win != null) {
                            win.config.reloadObserver = false;
                            win.mask();
                        }
                    }
                }
            });

            return false;
        };

        /**
        * @descrição: métodos que serão executados quando for abrir para novo registro
        **/
        var _novoBoleto = function () {

        };
        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {

            Iniciar: _iniciar,
            Novo: _novoBoleto,
            Salvar: _salvar
        };
    }();
    NotaAzul.Boleto.DadosBoleto.Iniciar();
}(window, document));