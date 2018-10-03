/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$().ready(function (window, document) {
    "use strict";
    if (NotaAzul.Corporacao == null) {
        NotaAzul.Corporacao = {};
    }

    NotaAzul.Corporacao.Dados = function () {
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
        * @descrição: Método que será chamado sempre após salvar 
        **/
        var _executarAposSalvar = function (retorno) {
            NotaAzul.Corporacao.Dados.Novo();
            return false;
        };


        /**
        * @descrição: Reseta o form, voltando os dados default
        **/
        var _novaCorporacao = function () {
            Prion.ClearForm("frmCorporacao", true);
        };

        /**
        * @descrição: Salva todos os dados do form
        **/
        var _salvar = function (win) {
            if (!Prion.ValidateForm("frmCorporacao")) {
                win.config.reloadObserver = false;
                return false;
            }

            if (win != null) { win.mask(); }

            Prion.Request({
                form: "frmCorporacao",
                url: rootDir + "Corporacao/Salvar",
                success: function (retorno, registroNovo) {
                    // se for um registro existente, fecha a janela
                    if ((!registroNovo) && (win != null)) {
                        win.config.reloadObserver = true;
                        win.hide({ animate: true });
                        return;
                    }

                    NotaAzul.Corporacao.Dados.ExecutarAposSalvar();
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
            Iniciar: _iniciar,
            ExecutarAposSalvar: _executarAposSalvar,
            Novo: _novaCorporacao,
            Salvar: _salvar
        };
    } ();
    NotaAzul.Corporacao.Dados.Iniciar();
} (window, document));