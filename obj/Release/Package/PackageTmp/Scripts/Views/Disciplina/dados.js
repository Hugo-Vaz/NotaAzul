﻿/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Disciplina == null) {
        NotaAzul.Disciplina = {};
    }


    NotaAzul.Disciplina.Dados = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        /**
        * @descrição: Método que será chamado sempre após Salvar
        **/
        var _executarAposSalvar = function (retorno) {
            NotaAzul.Disciplina.Dados.Novo();
            return false;
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
        };

        /**
        * @descrição: Reseta o form, voltando os dados default
        **/
        var _novaDisciplina = function () {
            Prion.ClearForm("frmDisciplina", true);
        };

        /**
        * @descrição: Salva todos os dados do form
        **/
        var _salvar = function (win) {
            if (!Prion.ValidateForm("frmDisciplina")) {
                win.config.reloadObserver = false;
                return false;
            }

            if (win != null) { win.mask(); }

            Prion.Request({
                form: "frmDisciplina",
                url: rootDir + "Disciplina/Salvar",
                success: function (retorno, registroNovo) {
                    // se for um registro existente, fecha a janela
                    if ((!registroNovo) && (win != null)) {
                        win.config.reloadObserver = true;
                        win.hide({ animate: true });
                        return;
                    }

                    NotaAzul.Disciplina.Dados.ExecutarAposSalvar();
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
            Novo: _novaDisciplina,
            Salvar: _salvar
        };

    } ();

    NotaAzul.Disciplina.Dados.Iniciar();
} (window, document));

