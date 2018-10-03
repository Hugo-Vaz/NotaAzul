/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.CursoAnoLetivo == null) {
        NotaAzul.Curso = {};
    }

    NotaAzul.CursoAnoLetivo.Dados = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        /**
        * @descrição: Carrega os combobox desta janela
        * @return: void
        **/
        var _carregarCombobox = function () {
            "CursoAnoLetivo_AnoLetivo".setValue(Prion.Date.AnoAtual());
            NotaAzul.Curso.ComboBox.Gerar({ filter: "Situacao.Nome=Ativo" });
        };

        /**
        * @descrição: Método que será chamado sempre após salvar 
        **/
        var _executarAposSalvar = function (retorno) {
            NotaAzul.CursoAnoLetivo.Dados.Novo();
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
        * @descrição: Reseta o form, voltando os dados default
        * @return: void
        **/
        var _novoCursoAnoLetivo = function () {
            Prion.ClearForm("frmCursoAnoLetivo", true);

            _carregarCombobox();
        };

        /**
        * @descrição: Salva todos os dados do form
        **/
        var _salvar = function (win) {
            if (!Prion.ValidateForm("frmCursoAnoLetivo")) {
                win.config.reloadObserver = false;
                return false;
            }

            if (win != null) { win.mask(); }

            Prion.Request({
                form: "frmCursoAnoLetivo",
                url: rootDir + "CursoAnoLetivo/Salvar",
                replaceData: [
                        { type: "money", fields: ["CursoAnoLetivo.ValorMatricula", "CursoAnoLetivo.ValorMensalidade"] }
                    ],
                success: function (retorno, registroNovo) {
                    // se for um registro existente, fecha a janela
                    if ((!registroNovo) && (win != null)) {
                        win.config.reloadObserver = true;
                        win.hide({ animate: true });
                        return;
                    }

                    NotaAzul.CursoAnoLetivo.Dados.ExecutarAposSalvar();
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
            Novo: _novoCursoAnoLetivo,
            Salvar: _salvar
        };

    } ();

    NotaAzul.CursoAnoLetivo.Dados.Iniciar();
} (window, document));