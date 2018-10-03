/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Titulo == null) {
        NotaAzul.Titulo = {};
    }

    NotaAzul.Titulo.Dados = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/

        /**
        * @descrição: Carrega os combobox desta janela
        * @return: void
        **/
        var _carregarCombobox = function () {
            NotaAzul.TituloTipo.ComboBox.Gerar({ id: "tituloTiposLista" });
        };

        /**
        * @descrição: Método que será chamado sempre após Salvar
        **/
        var _executarAposSalvar = function (retorno) {
            NotaAzul.Titulo.Dados.Novo();
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
        var _novoTitulo = function () {
            Prion.ClearForm("frmTitulo");
            _carregarCombobox();
        };

        /**
        * @descrição: Salva todos os dados do form
        **/
        var _salvar = function (win) {
            if (!Prion.ValidateForm("frmTitulo")) {
                win.config.reloadObserver = false;
                return false;
            }

            if (win != null) { win.mask(); }

            Prion.Request({
                form: "frmTitulo",
                url: rootDir + "Titulo/Salvar",
                replaceData: [
                        { type: "money", fields: ["Titulo.Valor", "Titulo.Desconto", "Titulo.Acrescimo"] }
                    ],
                success: function (retorno, registroNovo) {
                    // se for um registro existente, fecha a janela
                    if ((!registroNovo) && (win != null)) {
                        win.config.reloadObserver = true;
                        win.hide({ animate: true });
                        return;
                    }

                    NotaAzul.Titulo.Dados.ExecutarAposSalvar();
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
            ExecutarAposSalvar:_executarAposSalvar,
            Iniciar: _iniciar,
            Novo: _novoTitulo,
            Salvar: _salvar
        };
    } ();

    NotaAzul.Titulo.Dados.Iniciar();
} (window, document));

