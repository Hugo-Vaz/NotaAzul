/*jshint jquery:true */
var Prion = Prion || {},
    NotaAzul = NotaAzul || {},
    valoresRadio = valoresRadio || "";


NotaAzul.Relatorios.SelecionarFiltro = function () {
    "use strict";
    /******************************************************************************************
    ** PRIVATE
    ******************************************************************************************/
    /**
    * @descrição: Adiciona os eventos dos botões
    **/
    var _definirAcoesBotoes = function () {
        for (var i = 0; i < valoresRadio.length; i++) {

            Prion.Event.add(valoresRadio[i], "click", function () {
                var idForm = "frmFiltro" + this.value;
                (idForm).getDom().style.display = "initial";
                //Ver forma de fazer isso sem utilizar Jquery
                $("form:not(#" + idForm + ")").css("display", "none");
                Prion.ClearForm(idForm);
            });
        }
    };

    /**
    * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
    * @return: void
    **/
    var _iniciar = function () {
        //Ver forma de fazer isso sem utilizar Jquery
        $("#SelecionarFiltros").nextAll("form").css("display", "none");
        _definirAcoesBotoes();
    };
    /******************************************************************************************
    ** PUBLIC
    ******************************************************************************************/
    return {
        Iniciar: _iniciar
    };
} ();