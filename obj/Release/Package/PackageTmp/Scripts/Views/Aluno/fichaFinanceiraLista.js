/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";

    if (NotaAzul.Aluno.FichaFinanceira == null) {
        NotaAzul.Aluno.FichaFinanceira = {};
    }

    NotaAzul.Aluno.FichaFinanceira.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _listaGrid = [];

        /**
        * @descrição: Cria todo o Html da lista
        * @param: idLista = uma string que representará  o id do grid
        * @param: anoLetivo e nomeCurso = strings que serão utilizados para formar  o título do grid
        * @return: void
        **/
        var _criarLista = function (idLista, tituloLista) {

            var grid = new Prion.Lista({
                id: idLista,
                width: 750,
                style: "margin-left:5px",
                title: { text: tituloLista },
                type: Prion.Lista.Type.Local,
                //filter: { Entidades: "" },
                collapsed: true,
                paging: { show: false },
                rowNumber: { show: false },
                //checkbox: { show:false},
                columns: [
                    { header: "Mês", nameJson: "Mes", visible: false },
                    { header: "Mês", nameJson: "MesStr", width: "90px" },
                    { header: "Valor", nameJson: "Valor", width: "70px", mask: "money" },
                    { header: "Desconto", nameJson: "Desconto", width: "70px", mask: "money" },
                    { header: "Acréscimo", nameJson: "Acrescimo", width: "70px", mask: "money" },
                    { header: "Total", nameJson: "Total", width: "70px", mask: "money" },
                    { header: "Valor Pago", nameJson: "ValorPago", width: "100px", mask: "money" },
                    { header: "Data do Pagamento", nameJson: "DataPagamento", type: "date", width: "120px" },
                    { header: "Saldo", nameJson: "Saldo", width: "70px", mask: "money" }
                ]
            });

            _listaGrid.push({
                id: idLista,
                grid: grid
            });
        };

        /**
        * @descrição: Cria os grids de acordo com o número de cursos em que o aluno está matriculado
        * @param: Objeto de Aluno
        * @return: void
        **/
        var _criarGrid = function (objMatricula) {
            var divContent = "fichaFinanceira".getDom(),
                    i,
                    j;

            if (divContent == null) {
                return;
            }

            // se o aluno ainda não estiver matriculado, exibe a div com uma informação para o usuário
            if (objMatricula == null) {
                var divMensagem = document.createElement("DIV"),
                        mensagem = document.createElement("H1");

                mensagem.style.marginLeft = "20px";
                mensagem.style.marginTop = "15px";
                mensagem.style.marginBottom = "15px";
                mensagem.innerText = "Esse aluno ainda não foi matriculado";
                divMensagem.appendChild(mensagem);
                divContent.appendChild(divMensagem);

                return;
            }

            var arrayObj = [];

            for (i = 0; i < objMatricula.ListaMatriculaCurso.length; i++) {
                var idLista = "listaFichaFinanceira" + i,
                        divGrid = document.createElement("DIV"),
                        tituloLista = "";

                divGrid.setAttribute("id", idLista);
                divGrid.setAttribute("class", "list");
                divGrid.style.position = "relative";

                divContent.appendChild(divGrid);

                tituloLista = objMatricula.ListaMatriculaCurso[i].Turma.CursoAnoLetivo.AnoLetivo + "/" + objMatricula.ListaMatriculaCurso[i].Turma.CursoAnoLetivo.Curso.Nome;
                _criarLista(idLista, tituloLista);

                //Remove o atributo height,para que o collapsible funcione efetivamente
                ("listaFichaFinanceira" + i).getDom().removeAttribute("style", "height");

                for (j = 0; j < objMatricula.ListaMatriculaCurso[i].Mensalidades.length; j++) {
                    var nomeMes = Prion.Date.GetNomeMes(j),
                            objMensalidade = objMatricula.ListaMatriculaCurso[i].Mensalidades[j],
                            valor = objMensalidade.Titulo.Valor,
                            acrescimo = objMensalidade.Titulo.Acrescimo,
                            desconto = objMensalidade.Titulo.Desconto;

                    var valorPago = (objMensalidade.Situacao.Nome == "Quitada") ? objMensalidade.Titulo.ValorPago
                                    : 0;

                    //Verifica se a dataPagamento está preenchida, o valor da comparação corresponde a data 01/01/1900(default)
                    var dataPagamento = (objMensalidade.Titulo.DataOperacao != "/Date(-2208981600000)/") ? objMensalidade.Titulo.DataOperacao
                            : null,
                            saldo = (valorPago > 0) ? valorPago - (valor + acrescimo)
                            : 0,
                            total = valor + acrescimo;

                    // verifica se existe data de pagamento
                    // se existir data de pagamento, verifica se ela é menor que a data de vencimento
                    if (dataPagamento != null && (Prion.Format(dataPagamento, "timestamp") <= Prion.Format(objMensalidade.Titulo.DataVencimento, "timestamp"))) {
                        saldo -= desconto;
                        total -= desconto;
                    }


                    // adiciona no array um objeto referente a linha da iteração atual
                    arrayObj.push({
                        MesStr: nomeMes,
                        Mes: (j + 1),
                        Valor: valor,
                        Acrescimo: acrescimo,
                        Desconto: desconto,
                        Total: total,
                        ValorPago: valorPago,
                        DataPagamento: dataPagamento,
                        Saldo: saldo
                    });
                }

                _listaGrid[i].grid.addRow(arrayObj);
                arrayObj = [];
            }

            //Muda a cor da linha de acordo com o valor do saldo
            var tamanhoGridCursos = NotaAzul.Aluno.FichaFinanceira.Lista.Grid().length;

            for (i = 0; i < tamanhoGridCursos; i++) {
                var tamanhoGridMensalidades = NotaAzul.Aluno.FichaFinanceira.Lista.Grid()[i].grid.rows().length;

                for (j = 0; j < tamanhoGridMensalidades; j++) {
                    // verifica se o saldo da linha atual é negativo...
                    if (NotaAzul.Aluno.FichaFinanceira.Lista.Grid()[i].grid.rows()[j].Saldo < 0) {
                        // se for, muda a cor da label para VERMELHO
                        document.querySelector("#" + NotaAzul.Aluno.FichaFinanceira.Lista.Grid()[i].id + "Body table tr[dataid='" + NotaAzul.Aluno.FichaFinanceira.Lista.Grid()[i].grid.rows()[j].Mes + "'] td:last-child").style.color = "red";
                    }
                }
            }
        };

        /**
        * @descrição: Métodod de inicialização default
        **/
        var _iniciar = function () {

        };

        /**
        * @descrição: Retorno o Grid
        **/
        var _getGrid = function () {
            return _listaGrid;
        };

        /**
        * @descrição: Método que limpa a tab grid
        **/
        var _limparGrid = function () {
            var $divContent = document.getElementById("fichaFinanceira");

            while ($divContent.firstChild) {
                $divContent.removeChild($divContent.firstChild);
            }

            _listaGrid = [];
        };
        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            Iniciar: _iniciar,
            CriarGrid: _criarGrid,
            Grid: _getGrid,
            LimparGrid: _limparGrid
        };
    } ();
    NotaAzul.Aluno.FichaFinanceira.Lista.Iniciar();
} (window, document));
