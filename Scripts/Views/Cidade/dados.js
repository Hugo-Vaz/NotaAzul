/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Cidade == null) {
        NotaAzul.Cidade = {};
    }

    NotaAzul.Cidade.Dados = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/

        /**
        * @descrição: Reseta o form, voltando os dados default
        **/
        var _novaCidade = function () {
            Prion.ClearForm("frmCidade", true);

            _carregarCombobox();
        };


        /**
        * @descrição: Carrega os combobox desta janela
        * @return: void
        **/
        var _carregarCombobox = function () {
            NotaAzul.Estado.ComboBox.Gerar();
        };

        /**
        * @descrição: Método que será chamado sempre após salvar 
        **/
        var _executarAposSalvar = function (retorno) {
            NotaAzul.Cidade.Dados.Novo();
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
            if (!Prion.ValidateForm("frmCidade")) {
                win.config.reloadObserver = false;
                return false;
            }

            if (win != null) { win.mask(); }

            Prion.Request({
                form: "frmCidade",
                url: rootDir + "Cidade/Salvar",
                success: function (retorno, registroNovo) {
                    // se for um registro existente, fecha a janela
                    if ((!registroNovo) && (win != null)) {
                        win.config.reloadObserver = true;
                        win.hide({ animate: true });
                        return;
                    }

                    NotaAzul.Cidade.Dados.ExecutarAposSalvar();
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
            Novo: _novaCidade,
            Salvar: _salvar
        };
    } ();
    NotaAzul.Cidade.Dados.Iniciar();
} (window, document));