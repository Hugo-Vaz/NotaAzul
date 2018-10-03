/*jshint jquery:true */
var Prion = Prion || {},
    NotaAzul = NotaAzul || {};


NotaAzul.Relatorios.ContasAPagar = function () {
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
            { header: "Numero", nameJson: "Numero" },
            { header: "Descrição", nameJson: "Descricao" },
            { header: "Tipo", nameJson: "TituloTipo" },
            { header: "Valor", nameJson: "Valor", mask: "money" },
            { header: "Acréscimo", nameJson: "Acrescimo", mask: "money" },
            { header: "Desconto", nameJson: "Desconto", mask: "money" },
            { header: "Data de Vencimento", nameJson: "DataVencimento", type: "date" }
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