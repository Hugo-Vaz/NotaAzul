/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$().ready(function (window, document) {
    "use strict";
    if (NotaAzul.Cargo == null) {
        NotaAzul.Cargo = {};
    }

    NotaAzul.Cargo.Dados = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/

        /**
        * @descrição: Método que será chamado sempre após Salvar
        **/
        var _executarAposSalvar = function (retorno) {
            NotaAzul.Cargo.Dados.Novo();
            return false;
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
        };
        /**
        * @descrição: Reset o form, voltando os dados default
        **/
        var _novoCargo = function () {
            Prion.ClearForm("frmCargo", true);
        };

        /**
        * @descrição: Salva todos os dados do form
        **/
        var _salvar = function (win) {
            if (!Prion.ValidateForm("frmCargo")) {
                win.config.reloadObserver = false;
                return false;
            }

            if (win != null) { win.mask(); }

            Prion.Request({
                form: "frmCargo",
                url: rootDir + "Cargo/Salvar",
                success: function (retorno, registroNovo) {
                    // se for um registro existente, fecha a janela
                    if ((!registroNovo) && (win != null)) {
                        win.config.reloadObserver = true;
                        win.hide({ animate: true });
                        return;
                    }

                    NotaAzul.Cargo.Dados.ExecutarAposSalvar();
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

        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            ExecutarAposSalvar: _executarAposSalvar,
            Iniciar: _iniciar,
            Novo: _novoCargo,
            Salvar: _salvar
        };
    } ();
    NotaAzul.Cargo.Dados.Iniciar();
});

