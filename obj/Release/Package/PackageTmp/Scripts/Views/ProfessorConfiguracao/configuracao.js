/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        tipoMedia = tipoMedia || "",
        idConfig = idConfig || "",
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.ProfessorConfiguracao == null) {
        NotaAzul.ProfessorConfiguracao = {};
    }

    NotaAzul.ProfessorConfiguracao.Configuracao = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/

        /**
        * @descrição: Carrega os combobox desta janela
        * @return: void
        **/
        var _carregarComboBox = function () {
            NotaAzul.TipoMedia.ComboBox.Gerar({ id: "listaTiposMedia" });
        };


        /**
        * @descrição: Define a ação para todos os controles da tela
        * @return: void
        **/
        var _definirAcoesBotoes = function () {

            //evento associado ao botão de salvar as configurações
            Prion.Event.add("btnSalvarConfig", "click", function () {
                NotaAzul.ProfessorConfiguracao.Configuracao.Salvar();
            });

            //evento associado ao change de listaTiposMedia
            Prion.Event.add("listaTiposMedia", "change", function () {
                var grpDom = "GrpImagens".getDom(),
                    value = this.value;

                for (var i = 0, len = grpDom.children.length; i < len; i++) {
                    grpDom.children[i].style.display = "none";
                }

                value.getDom().style.display = "inline";
            });
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            _definirAcoesBotoes();
            _carregarComboBox();
            "ProfessorConfiguracao_Id".getDom().value = idConfig;
            if (tipoMedia !== "") {
                Prion.ComboBox.SelecionarIndexPeloValue("listaTiposMedia", tipoMedia);
                tipoMedia.getDom().style.display = "inline";
            }
        };


        /**
        * @descrição: Salva todos os dados do form
        **/
        var _salvar = function (win) {
            if (!Prion.ValidateForm("frmConfigProfessor")) {
                win.config.reloadObserver = false;
                return false;
            }

            Prion.Request({
                form: "frmConfigProfessor",
                url: rootDir + "ProfessorConfiguracao/Salvar",
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
    NotaAzul.ProfessorConfiguracao.Dados.Iniciar();
} (window, document));