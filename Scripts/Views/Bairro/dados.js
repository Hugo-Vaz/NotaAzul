/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Bairro == null) {
        NotaAzul.Bairro = {};
    }

    NotaAzul.Bairro.Dados = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/

        /**
        * @descrição: Reseta o form, voltando os dados default
        **/
        var _novoBairro = function () {
            Prion.ClearForm("frmBairro", true);

            _carregarCombobox();
        };


        /**
        * @descrição: Carrega os combobox desta janela
        * @return: void
        **/
        var _carregarCombobox = function () {
            Prion.ComboBox.AlterarEstadoBotoes("bairroListaCidades", false);
            NotaAzul.Cidade.ComboBox.Gerar({ id: "bairroListaCidades" });
        };

        /**
        * @descrição: Método que será chamado sempre após salvar 
        **/
        var _executarAposSalvar = function (retorno) {
            NotaAzul.Bairro.Dados.Novo();
            return false;
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            _carregarCombobox();
        };

        /**
        * @descrição: Salva todos os dados do form
        **/
        var _salvar = function (win) {
            if (!Prion.ValidateForm("frmBairro")) {
                win.config.reloadObserver = false;
                return false;
            }

            if (win != null) { win.mask(); }

            Prion.Request({
                form: "frmBairro",
                url: rootDir + "Bairro/Salvar",
                success: function (retorno, registroNovo) {
                    // se for um registro existente, fecha a janela
                    if ((!registroNovo) && (win != null)) {
                        win.config.reloadObserver = true;
                        win.hide({ animate: true });
                        return;
                    }

                    NotaAzul.Bairro.Dados.ExecutarAposSalvar();
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
            Novo: _novoBairro,
            Salvar: _salvar
        };
    } ();
    NotaAzul.Bairro.Dados.Iniciar();
} (window, document));