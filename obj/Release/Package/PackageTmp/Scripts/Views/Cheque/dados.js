/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Cheque == null) {
        NotaAzul.Cheque = {};
    }

    NotaAzul.Cheque.Dados = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        /**
        * @descrição: Responsável pelo AutoComplete do campo AlunoResponsavel
        * @return: void
        **/
        var _autoCompleteBuscarResponsavel = function () {
            $("#Cheque_NomeAlunoResponsavel").autocomplete({
                source: rootDir + "Aluno/ListaResponsavelPorNome",
                minLength: 1,
                appendTo: $("#Cheque_NomeAlunoResponsavel").parent(),
                delay: 500,
                dataType: 'json',
                focus: true,
                open: function () {
                    $(".ui-autocomplete:visible").css({ top: "", left: "128px" });

                },
                select: function (event, data) {
                    "Cheque_NomeAlunoResponsavel".setValue(data.item.label);
                    "Cheque_IdAlunoResponsavel".setValue(data.item.value);
                    return false;
                }
            });
        };

        /**
        * @descrição: Reset o form, voltando os dados default
        **/
        var _novoCheque = function () {
            Prion.ClearForm("frmCheque", true);
            Prion.RadioButton.SelectByValue("Cheque.Proprio", "true");
            _carregarCombobox();
        };

        /**
        * @descrição: Carrega os combobox desta janela
        * @return: void
        **/
        var _carregarCombobox = function () {
            NotaAzul.Alinea.ComboBox.Gerar({ id: "chequeAlineaLista" });

        };

        /**
        * @descrição: Método que será chamado sempre após Salvar
        **/
        var _executarAposSalvar = function (retorno) {
            NotaAzul.Cheque.Dados.Novo();
            return false;
        };


        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            NotaAzul.Cheque.Dados.AutoCompleteBuscarResponsavel();
        };

        /**
        * @descrição: Salva todos os dados do form
        **/
        var _salvar = function (win) {
            if (!Prion.ValidateForm("frmCheque")) {
                win.config.reloadObserver = false;
                return false;
            }

            if (win != null) { win.mask(); }

            Prion.Request({
                form: "frmCheque",
                url: rootDir + "Cheque/Salvar",
                replaceData: [
                        { type: "money", fields: ["Cheque.Valor"] }
                    ],
                success: function (retorno, registroNovo) {
                    // se for um registro existente, fecha a janela
                    if ((!registroNovo) && (win != null)) {
                        win.config.reloadObserver = true;
                        win.hide({ animate: true });
                        return;
                    }

                    NotaAzul.Cheque.Dados.ExecutarAposSalvar();
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
            AutoCompleteBuscarResponsavel: _autoCompleteBuscarResponsavel,
            ExecutarAposSalvar:_executarAposSalvar,
            Iniciar: _iniciar,
            Novo: _novoCheque,
            Salvar: _salvar
        };
    } ();

    NotaAzul.Cheque.Dados.Iniciar();
} (window, document));

