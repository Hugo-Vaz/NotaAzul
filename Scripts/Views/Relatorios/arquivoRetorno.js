/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        tipoRelatorio = tipoRelatorio || "",
        booleanRadio = booleanRadio || "",
        rootDir = rootDir || "";


$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Relatorios == null) {
        NotaAzul.Relatorios = {};
    }

    NotaAzul.Relatorios.ArquivoRetorno = function () {
        /******************************************************************************************
     ** PRIVATE
     ******************************************************************************************/
        var _dataTable,
            _rows,
            _dashboard;

        /**
        * @descrição: Responsável por desenhar o controle
        **/
        var _desenharControle = function (chart) {
            //Cria o controle de busca do fornecedor
            var controleArquivo = new google.visualization.ControlWrapper({
                'controlType': 'StringFilter', //Filtro por string
                'containerId': 'ControleArquivo', //Id do elemento
                'options': {
                    'filterColumnLabel': 'Arquivo', //Coluna do datatable que será sobre a qual o filtro atuará                    
                    'ui': {
                        'labelStacking': 'vertical'//A label será aplicada acima do input
                    }
                }
            });

            //Cria o controle de busca da empresa
            var controleDocumento = new google.visualization.ControlWrapper({
                'controlType': 'StringFilter', //Filtro por string
                'containerId': 'ControleDocumento', //Id do elemento
                'options': {
                    'filterColumnLabel': 'Documento', //Coluna do datatable que será sobre a qual o filtro atuará
                    'ui': {
                        'labelStacking': 'vertical'//A label será aplicada acima do input
                    }
                }
            });


            //Cria o controle de data do contrato
            var controleOperacao = new google.visualization.ControlWrapper({
                'controlType': 'DateRangeFilter', //Filtro por Datetime
                'containerId': 'ControleOperacao', //Id do elemento
                'options': {
                    'filterColumnLabel': 'Data', //Coluna do datatable que será sobre a qual o filtro atuará                    
                    'ui': {
                        'labelStacking': 'vertical', //A label será aplicada acima do input
                        'format': {
                            'pattern': "dd/MM/yyyy"
                        }
                    }
                }
            });

            //Cria o vinculo entre o filtro e o gráfico
            _dashboard.bind(controleArquivo, chart);
            _dashboard.bind(controleDocumento, chart);
            _dashboard.bind(controleOperacao, chart);

            //O objeto _dashboard é responsável por desenhar o gráfico
            _dashboard.draw(_dataTable);
        };

        /**
        * @descrição: Responsável por desenhar o gráfico
        **/
        var _desenharGrafico = function (jsonObj) {
            _dataTable = new google.visualization.DataTable();
            _dataTable.addColumn('string', 'Arquivo');
            _dataTable.addColumn('string', 'Documento');
            _dataTable.addColumn('string', 'Operação');
            _dataTable.addColumn('date', 'Data');
            _dataTable.addColumn('number', 'Valor');
            _dataTable.addColumn('number', 'Valor Pago');


            if (jsonObj != null) {
                var len = jsonObj.length,
                    rows = [];
                for (var i = 0; i < len; i++) {
                    rows.push([
                        jsonObj[i].NomeArquivo,
                        jsonObj[i].NumeroDocumento,
                        jsonObj[i].Operacao,
                        jsonObj[i].DataOperacao.toDate("dd-mm-yyyy", "yyyy-mm-dd"),
                        { v: parseFloat(jsonObj[i].Valor), f: Prion.Mascara2(jsonObj[i].Valor, "money") },
                        { v: parseFloat(jsonObj[i].ValorPago), f: Prion.Mascara2(jsonObj[i].ValorPago, "money") }
                    ]);
                }
                console.log(rows);
                _dataTable.addRows(rows);

            } else {
                //caso a busca não retorne nada,um array composto por valores nulos,será criado com o intuito de inicializar o gráfico
                _dataTable.addRows([[null, null, null, null]]);
            }

            var chart = new google.visualization.ChartWrapper({
                'chartType': 'Table',
                'containerId': 'Grafico',
                'options': {
                    'title': 'Arquivo de Retorno',
                    /*'page': 'enable',
                    'pageSize': 20,*/
                    'width': '100%',
                    'height': '500',
                    'left': '0',
                    'alternatingRowStyle': 'true',
                    'showRowNumber': 'true'
                }
            });

            _desenharControle(chart);
        };

        /**
        * @descrição: Adiciona os eventos dos botões
        **/
        var _definirAcoesBotoes = function () {          

            Prion.Event.add("Imprimir_Relatorio", "click", function () {
                _imprimirRelatorio();
            });
        };
       

        var _imprimirRelatorio = function () {
            var mywindow = window.document.open("", "Contrato", ""),
                printContent = "Grafico".getDom().cloneNode(true);

            printContent.children[0].children[0].style.overflow = "initial";
            var $inputs = printContent.querySelectorAll("input");

            for (var i = 0, len = $inputs.length; i < len; i++) {
                $inputs[i].parentNode.innerHTML = $inputs[i].value;
            }

            var $header = "Relatorio_Impressao".getDom();
            
            var html = "<html><head><title>Relatório</title><style>table,tr,td { border:1px solid;font-size: 11px;text-align: center;page-break-inside: avoid !important; } tr{height: 10%;} table{height:initial !important;page-break-inside: avoid !important;} .texto{text-align:left !important}</style></head><body>" +
                $header.innerHTML + printContent.children[0].children[0].innerHTML + "</body></html>";

            mywindow.document.write(html);
            mywindow.print();
                 


            //mywindow.close();
        };


        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            _rows = [];          
            _dashboard = new google.visualization.Dashboard("Dashboard".getDom());
            "loading".getDom().style.display = "inline";
            _definirAcoesBotoes();

            Prion.Request({
                url: rootDir + "Relatorios/GerarArquivoRetorno",
                success: function (retorno) {
                    "loading".getDom().style.display = "none";
                    if (retorno != null && retorno.rows != null) {
                        var jsonObj = JSON.parse(retorno.rows);
                        if (jsonObj.length > 0) {
                            _desenharGrafico(jsonObj);
                            "page".getDom().style.width = "80%";
                        }
                    }
                },
                error: function () {
                    "loading".getDom().style.display = "none";
                }
            });
        };
        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            Iniciar: _iniciar
        };
    } ();
    NotaAzul.Relatorios.ArquivoRetorno.Iniciar();
} (window, document));