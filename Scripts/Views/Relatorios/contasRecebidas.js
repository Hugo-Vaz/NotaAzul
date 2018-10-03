/*jshint jquery:true */
var Prion = Prion || {},
    NotaAzul = NotaAzul || {};


NotaAzul.Relatorios.ContasRecebidas = function () {
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
                    { header: "Aluno", nameJson: "AlunoNome", width: "250px" },
                    { header: "Descrição", nameJson: "Descricao", width: "250px" },
                    { header: "Tipo", nameJson: "TituloTipo", width: "250px" },
                    { header: "Valor", nameJson: "Valor", mask: "money", width: "150px" },
                    { header: "Acréscimo", nameJson: "Acrescimo", mask: "money", width: "150px" },
                    { header: "Desconto", nameJson: "Desconto", mask: "money", width: "150px" },
                    { header: "Valor Pago", nameJson: "ValorPago", mask: "money", width: "150px" },
                    { header: "Data de Vencimento", nameJson: "DataVencimento", type: "date", width: "150px" },
                    { header: "Data de Pagamento", nameJson: "DataOperacao", type: "date", width: "150px" }
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
