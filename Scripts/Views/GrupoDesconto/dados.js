/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.GrupoDesconto == null) {
        NotaAzul.GrupoDesconto = {};
    }

    NotaAzul.GrupoDesconto.Dados = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/

        /**
        * @descrição: Adiciona os eventos dos checkbox de Tipo Desconto
        **/
        var _definirAcoesBotoes = function () {
            Prion.Event.add("GrupoDesconto_DescontoPercentual", "click", function () {
                var $dom = "GrupoDesconto_Valor".getDom();

                $dom.value = "";
                $dom.setAttribute("mask", "percent");
                Prion.Mascara($dom);
            });

            Prion.Event.add("GrupoDesconto_DescontoValorMonetario", "click", function () {
                var $dom = "GrupoDesconto_Valor".getDom();

                $dom.value = "";
                $dom.setAttribute("mask", "money");
                Prion.Mascara($dom);
            });
        };

        /**
        * @descrição: Método que será chamado sempre após salvar 
        **/
        var _executarAposSalvar = function (retorno) {
            NotaAzul.GrupoDesconto.Dados.Novo();
            return false;
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            _definirAcoesBotoes();
        };

        /**
        * @descrição: Reseta o form, voltando os dados default
        **/
        var _novoGrupoDesconto = function () {
            Prion.ClearForm("frmGrupoDesconto", true);

            // deixa o radiobutton "ValorMonetario" selecionado por default
            //Prion.RadioButton.SelectByValue("GrupoDesconto.TipoDesconto", "ValorMonetario");
        };

        /**
        * @descrição: Salva todos os dados do form
        **/
        var _salvar = function (win) {
            if (!Prion.ValidateForm("frmGrupoDesconto")) {
                win.config.reloadObserver = false;
                return false;
            }

            var dataType = "GrupoDesconto_Valor".getDom().getAttribute("mask");

            if (win != null) { win.mask(); }

            Prion.Request({
                form: "frmGrupoDesconto",
                url: rootDir + "GrupoDesconto/Salvar",
                replaceData: [
                        { type: dataType, fields: ["GrupoDesconto.Valor"] }
                    ],
                success: function (retorno, registroNovo) {
                    // se for um registro existente, fecha a janela
                    if ((!registroNovo) && (win != null)) {
                        win.config.reloadObserver = true;
                        win.hide({ animate: true });
                        return;
                    }

                    NotaAzul.GrupoDesconto.Dados.ExecutarAposSalvar();
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
            Novo: _novoGrupoDesconto,
            Salvar: _salvar
        };

    } ();

    NotaAzul.GrupoDesconto.Dados.Iniciar();
} (window, document));