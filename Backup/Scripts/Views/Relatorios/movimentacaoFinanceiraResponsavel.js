/*jshint jquery:true */
var Prion = Prion || {},
    NotaAzul = NotaAzul || {};


NotaAzul.Relatorios.MovimentacaoFinanceiraResponsavel = function () {
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
                    { header: "Responsável", nameJson: "ResponsavelNome", width: "250px" },
                    { header: "CPF", nameJson: "Cpf", mask: "cpf", width: "250px" },
                    { header: "Valor", nameJson: "Valor", mask: "money", width: "150px" }
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
