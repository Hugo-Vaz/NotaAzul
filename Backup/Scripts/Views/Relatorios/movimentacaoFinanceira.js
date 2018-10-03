/*jshint jquery:true */
var Prion = Prion || {},
    NotaAzul = NotaAzul || {};


NotaAzul.Relatorios.MovimentacaoFinanceira = function () {
    "use strict";
    /******************************************************************************************
    ** PRIVATE
    ******************************************************************************************/
    /**
    * @descrição: Cria as colunas que serão utilizadas no relatório
    * @return: void
    **/
    var _criarColunasDoGrid = function () {
        var colunas = [
                    { header: "Títulos", nameJson: "Titulos", width: "450px" },
                    { header: "Valor", nameJson: "Valor" },
                    { header: "Tipo", nameJson: "Tipo" }
                   ];

        return colunas;
    };
    /******************************************************************************************
    ** PUBLIC
    ******************************************************************************************/
    return {       
        CriarColunasDoGrid: _criarColunasDoGrid
    };
} ();
