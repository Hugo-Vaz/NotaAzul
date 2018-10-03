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

    NotaAzul.Relatorios.Dados = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null, 
            _titulo,         
            _filtros = null;


        /**
        * @descrição: Método responsável por abrir a janela com o Relatório            
        * @return: void
        **/
        var _abrirRelatorio = function () {
            var printContents = document.getElementById("Relatorio_Impressao").innerHTML,                         
                html = "<html><head><title>"+_titulo+"</title><link rel='stylesheet' type='text/css' href='../../Content/notaAzul.css' media=print'/></head><body >" +
                        printContents +
                        "</body></html>";
                
            var w = window.open();
            w.document.write(html);
            w.print();
        };

        /**
        * @descrição: Adiciona os eventos dos botões
        **/
        var _definirAcoesBotoes = function () {
            Prion.Event.add("btnAplicarFiltro", "click", function () {
                var arrayFiltro = [];
                //A cada iteração um novo objeto será criado de acordo com o filtro,representado pelo valor do array
                for (var i = 0; i < filtros.length; i++) {
                    var nomeForm = "frmFiltro" + filtros[i];

                    //verifica se o style/diplay é "none",pois caso seja, outro filtro estará sendo usado
                    if (nomeForm.getDom().style.display == "none") {
                        continue;
                    }

                    if (!Prion.ValidateForm(nomeForm)) {
                        return false;
                    }

                    var objFiltro = Prion.FormToObject(nomeForm);
                    objFiltro["Relatorio.Tipo"] = tipoRelatorio;
                    arrayFiltro.push(objFiltro);
                }
                _titulo = _prepararTituloRelatorio(arrayFiltro);
                NotaAzul.Relatorios.Dados.GerarRelatorio(arrayFiltro, tipoRelatorio);

                return true;
            });

            Prion.Event.add("btnGerarPdf", "click", function () {
                NotaAzul.Relatorios.Dados.AbrirRelatorio();
            });
        };
              

        /**
        * @descrição: Cria todo o Html da lista
        * @return: void
        **/
        var _criarLista = function (colunas) {
            if (_grid != null) { return; }

            _grid = new Prion.Lista({
                id: "Relatorio",
                title: { text: "" },
                type: Prion.Lista.Type.Local,
                //height: 500,
                paging: { show: false },
                columns: colunas,
                rowNumber: {
                    show: false
                }
            });
        };        

        /**
        * @descrição: Cria o relatório para impressão
        * @return: void
        **/
        var _criarRelatorioImpressao = function(jsonObj){
            var $tr,$td,$th,         
                objectKeys = Object.keys(jsonObj[0]),
                $tabela ="CorpoRelatorio".getDom();

            "Titulo_Relatorio".getDom().innerText = _titulo;
            $tr = document.createElement("TR");

            for (var j = 0, len2 = objectKeys.length; j < len2; j++) {
                $th = document.createElement("TH");
                $th.innerText = objectKeys[j];
                $tr.appendChild($th);
            }

            $tabela.appendChild($tr);

            for(var i=0,len = jsonObj.length;i<len;i++){
                $tr = document.createElement("TR");
                for(var j=0,len2 = objectKeys.length;j<len2;j++){
                    $td = document.createElement("TD");
                    $td.innerText = jsonObj[i][objectKeys[j]];
                    $tr.appendChild($td);
                }

                $tabela.appendChild($tr);
            }
        };

        /**
        * @descrição: Gera o relatório
        **/
        var _gerarRelatorio = function (arrayFiltro, tipoRelatorio) {
            if (arrayFiltro === null || arrayFiltro.length === 0) {
                return false;
            }

            _grid.clear();
            
            Prion.Request({
                url: rootDir + "Relatorios/Gerar" + tipoRelatorio,
                data: "Filtro= {\"Filtros\":" + JSON.stringify(arrayFiltro) + "}",
                success: function (retorno) {
                    document.querySelectorAll("#Relatorio .listTitleH3")[0].innerText = _prepararTituloRelatorio(arrayFiltro);
                    var jsonObj = JSON.parse(retorno.rows);
                    _grid.addRow(jsonObj);
                    _criarRelatorioImpressao(jsonObj);
                    _filtros = arrayFiltro;
                }
            });

            return true;
        };        

        /**
        * @descrição: Método que retorna o objeto do grid
        * @return: Objeto grid
        **/
        var _getGrid = function () {
            return _grid;
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            _definirAcoesBotoes();            
            _verificarTipoRelatorio(tipoRelatorio);
            Prion.ClearForm("frmFiltro", true);
        };

        /**
        * @descrição: Verifica o tipo de relatório a ser gerado
        * @return: void
        **/
        var _verificarTipoRelatorio = function (tipoRelatorio) {
            switch (tipoRelatorio) {
                case "ContasPagas":
                    Prion.Loader.Carregar([
                        {
                            url: rootDir + "Scripts/Views/Relatorios/contasPagas.js",
                            fn: function () {
                                if (booleanRadio == "True") { NotaAzul.Relatorios.SelecionarFiltro.Iniciar(); }
                                var colunas = NotaAzul.Relatorios.ContasPagas.CriarColunasDoGrid();                               
                                NotaAzul.Relatorios.Dados.CriarLista(colunas);
                            }
                        }
                    ]);
                    break;

                case "ContasAPagar":
                    Prion.Loader.Carregar([
                        {
                            url: rootDir + "Scripts/Views/Relatorios/contasAPagar.js",
                            fn: function () {
                                if (booleanRadio == "True") { NotaAzul.Relatorios.SelecionarFiltro.Iniciar(); }
                                var colunas = NotaAzul.Relatorios.ContasAPagar.CriarColunasDoGrid();                                
                                NotaAzul.Relatorios.Dados.CriarLista(colunas);
                            }
                        }
                    ]);
                    break;

                case "ContasRecebidas":
                    Prion.Loader.Carregar([
                        {
                            url: rootDir + "Scripts/Views/Relatorios/contasRecebidas.js",
                            fn: function () {
                                if (booleanRadio == "True") { NotaAzul.Relatorios.SelecionarFiltro.Iniciar(); }
                                var colunas = NotaAzul.Relatorios.ContasRecebidas.CriarColunasDoGrid();                                
                                NotaAzul.Relatorios.Dados.CriarLista(colunas);
                            }
                        }
                    ]);
                    break;

                case "ContasAReceber":
                    Prion.Loader.Carregar([
                        {
                            url: rootDir + "Scripts/Views/Relatorios/contasAReceber.js",
                            fn: function () {
                                if (booleanRadio == "True") { NotaAzul.Relatorios.SelecionarFiltro.Iniciar(); }
                                var colunas = NotaAzul.Relatorios.ContasAReceber.CriarColunasDoGrid();                               
                                NotaAzul.Relatorios.Dados.CriarLista(colunas);
                            }
                        }
                    ]);
                    break;

                case "AlunosMatriculados":
                    Prion.Loader.Carregar([
                    {
                        url: rootDir + "Scripts/Views/Relatorios/alunosMatriculados.js",
                        fn: function () {
                            if (booleanRadio == "True") { NotaAzul.Relatorios.SelecionarFiltro.Iniciar(); }
                            var colunas = NotaAzul.Relatorios.AlunosMatriculados.CriarColunasDoGrid();
                            NotaAzul.Relatorios.AlunosMatriculados.Iniciar();                           
                            NotaAzul.Relatorios.Dados.CriarLista(colunas);
                        }
                    }
                ]);
                    break;


                case "AlunosBolsistas":
                    Prion.Loader.Carregar([
                    {
                        url: rootDir + "Scripts/Views/Relatorios/alunosBolsistas.js",
                        fn: function () {
                            if (booleanRadio == "True") { NotaAzul.Relatorios.SelecionarFiltro.Iniciar(); }
                            var colunas = NotaAzul.Relatorios.AlunosBolsistas.CriarColunasDoGrid();
                            NotaAzul.Relatorios.AlunosBolsistas.Iniciar();                           
                            NotaAzul.Relatorios.Dados.CriarLista(colunas);
                        }
                    }
                ]);
                    break;

                case "AlunosInadimplentes":
                    Prion.Loader.Carregar([
                    {
                        url: rootDir + "Scripts/Views/Relatorios/alunosInadimplentes.js",
                        fn: function () {
                            if (booleanRadio == "True") { NotaAzul.Relatorios.SelecionarFiltro.Iniciar(); }
                            var colunas = NotaAzul.Relatorios.AlunosInadimplentes.CriarColunasDoGrid();
                            NotaAzul.Relatorios.AlunosInadimplentes.Iniciar();                            
                            NotaAzul.Relatorios.Dados.CriarLista(colunas);
                        }
                    }
                ]);
                    break;

                case "AlunosIsentos":
                    Prion.Loader.Carregar([
                    {
                        url: rootDir + "Scripts/Views/Relatorios/alunosIsentos.js",
                        fn: function () {
                            if (booleanRadio == "True") { NotaAzul.Relatorios.SelecionarFiltro.Iniciar(); }
                            var colunas = NotaAzul.Relatorios.AlunosIsentos.CriarColunasDoGrid();
                            NotaAzul.Relatorios.AlunosIsentos.Iniciar();                           
                            NotaAzul.Relatorios.Dados.CriarLista(colunas);
                        }
                    }
                ]);
                    break;

                case "MovimentacaoFinanceira":
                    Prion.Loader.Carregar([
                    {
                        url: rootDir + "Scripts/Views/Relatorios/movimentacaoFinanceira.js",
                        fn: function () {
                            if (booleanRadio == "True") { NotaAzul.Relatorios.SelecionarFiltro.Iniciar(); }
                            var colunas = NotaAzul.Relatorios.MovimentacaoFinanceira.CriarColunasDoGrid();                            
                            NotaAzul.Relatorios.Dados.CriarLista(colunas);
                        }
                    }
                ]);
                    break;
                case "MovimentacaoFinanceiraResponsavel":
                    Prion.Loader.Carregar([
                    {
                        url: rootDir + "Scripts/Views/Relatorios/movimentacaoFinanceiraResponsavel.js",
                        fn: function () {
                            if (booleanRadio == "True") { NotaAzul.Relatorios.SelecionarFiltro.Iniciar(); }
                            var colunas = NotaAzul.Relatorios.MovimentacaoFinanceiraResponsavel.CriarColunasDoGrid();                           
                            NotaAzul.Relatorios.Dados.CriarLista(colunas);
                        }
                    }
                ]);
                    break;
                case "MensalidadesAtrasadas":
                    Prion.Loader.Carregar([
                    {
                        url: rootDir + "Scripts/Views/Relatorios/mensalidadesAtrasadas.js",
                        fn: function () {
                            var colunas = NotaAzul.Relatorios.MensalidadesAtrasadas.CriarColunasDoGrid();
                            NotaAzul.Relatorios.MensalidadesAtrasadas.Iniciar();
                            if (booleanRadio == "True") { NotaAzul.Relatorios.SelecionarFiltro.Iniciar(); }                            
                            NotaAzul.Relatorios.Dados.CriarLista(colunas);
                        }
                    }
                ]);
                    break;
                case "MensalidadesAdiantadas":
                    Prion.Loader.Carregar([
                    {
                        url: rootDir + "Scripts/Views/Relatorios/mensalidadesAdiantadas.js",
                        fn: function () {
                            var colunas = NotaAzul.Relatorios.MensalidadesAdiantadas.CriarColunasDoGrid();
                            NotaAzul.Relatorios.MensalidadesAdiantadas.Iniciar();
                            if (booleanRadio == "True") { NotaAzul.Relatorios.SelecionarFiltro.Iniciar(); }                           
                            NotaAzul.Relatorios.Dados.CriarLista(colunas);
                        }
                    }
                ]);
                    break;
                default:
                    return false;
            }
        };

        /**
        * @descrição: Torna visível o botão de Abrir o PDF em outra janela
        * @return: void
        **/
        var _prepararBotaoAbrirPdf = function (url) {
            if (url == null) { return; }
            "btnAbrirPdf".getDom().style.display = "inline";
            Prion.Event.add("btnAbrirPdf", "click", function () {
                window.open(url);
            });

            return true;
        };

        /**
        * @descrição: Cria o título do relatório a partir dos dados do filtro
        * @return: void
        **/
        var _prepararTituloRelatorio = function (arrayFiltro) {
            if (arrayFiltro == null) { return; }
            var titulo;

            switch (tipoRelatorio) {
                case "ContasPagas":
                    titulo = "Contas Pagas";
                    break;
                case "ContasAPagar":
                    titulo = "Contas à Pagar";
                    break;
                case "ContasRecebidas":
                    titulo = "Contas Recebidas";
                    break;
                case "ContasAReceber":
                    titulo = "Contas à Receber";
                    break;
                case "AlunosMatriculados":
                    titulo = "Alunos Matriculados";
                    break;
                case "AlunosBolsistas":
                    titulo = "Alunos Bolsistas";
                    break;
                case "AlunosInadimplentes":
                    titulo = "Alunos Inadimplentes";
                    break;
                case "AlunosIsentos":
                    titulo = "Alunos Isentos";
                    break;
                case "MovimentacaoFinanceira":
                    titulo = "Movimentação Financeira";
                    break;
                case "MovimentacaoFinanceiraResponsavel":
                    titulo = "Movimentação Financeira por Responsável";
                    break;
                case "MensalidadesAtrasadas":
                    titulo = "Mensalidades Atrasadas";
                    break;
                case "MensalidadesAdiantadas":
                    titulo= "Mensalidades Adiantadas";
                    break;
                default:
                   break;
            }
            for (var i = 0; i < arrayFiltro.length; i++) {
                switch (arrayFiltro[i]["Filtro.Tipo"]) {
                    case ("Periodo"):
                        titulo += arrayFiltro[i]["Filtro.DataInicial"] + "/" + arrayFiltro[i]["Filtro.DataFinal"];
                        break;
                    case ("Aluno"):
                        titulo += arrayFiltro[i]["Filtro.NomeAluno"];
                        break;
                    default:
                        break;
                }
            }
            return titulo;
        };

        /**
        * @descrição:Verifica se a criação já foi concluída
        * @return:boolean com o status
        **/
        var _verificaEstadoPdf = function (url) {

            var http = new XMLHttpRequest();
            http.open('HEAD', url, false);
            http.send();
            return http.status != 404;
        };       
        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            AbrirRelatorio: _abrirRelatorio,
            CriarLista: _criarLista,             
            GerarRelatorio: _gerarRelatorio,            
            Grid: _getGrid,            
            Iniciar: _iniciar
        };
    } ();
    NotaAzul.Relatorios.Dados.Iniciar();
} (window, document));