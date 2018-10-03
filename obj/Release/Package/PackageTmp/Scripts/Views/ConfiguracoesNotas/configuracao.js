/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        mediaMinima= mediaMinima || 0,
        divisaoAnoLetivo = divisaoAnoLetivo || 0,
        formaDeConceito = formaDeConceito || 0 ,
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.ConfiguracoesNotas == null) {
        NotaAzul.ConfiguracoesNotas = {};
    }

    NotaAzul.ConfiguracoesNotas.Configuracao = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/

        /**
        * @descrição: Carrega os combobox desta janela
        * @return: void
        **/
        var _carregarComboBox = function () {
            NotaAzul.FormaNota.ComboBox.Gerar({ id: "listaFormaDeConceito" });
            NotaAzul.DivisaoAnoLetivo.ComboBox.Gerar({ id: "listaFormaDeDivisaoAnoLetivo" });
        };


        /**
        * @descrição: Define a ação para todos os controles da tela
        * @return: void
        **/
        var _definirAcoesBotoes = function () {

            //evento associado ao botão de salvar as configurações
            Prion.Event.add("btnSalvarConfig", "click", function () {
                NotaAzul.ConfiguracoesNotas.Configuracao.Salvar();
            });
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            _definirAcoesBotoes();
            _carregarComboBox();
            Prion.ComboBox.SelecionarIndexPeloValue("listaFormaDeConceito", formaDeConceito);
            Prion.ComboBox.SelecionarIndexPeloValue("listaFormaDeDivisaoAnoLetivo", divisaoAnoLetivo);
            "MediaMinima".setValue(mediaMinima);
        };

        /**
        * @descrição: Salva todos os dados do form
        **/
        var _salvar = function (win) {
            if (!Prion.ValidateForm("frmConfigNota")) {
                win.config.reloadObserver = false;
                return false;
            }

            Prion.Request({
                form: "frmConfigNota",
                url: rootDir + "ConfiguracoesNotas/Salvar",
                success: function (retorno, registroNovo) {
                    // se for um registro existente, fecha a janela
                    if ((!registroNovo) && (win != null)) {
                        win.config.reloadObserver = true;
                        win.hide({ animate: true });
                        return;
                    }

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
            Salvar: _salvar
        };
    } ();

    NotaAzul.ConfiguracoesNotas.Configuracao.Iniciar();
} (window, document));