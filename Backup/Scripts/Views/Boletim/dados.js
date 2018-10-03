/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        mediaMinima = mediaMinima || null,
        divisaoAnoLetivo = divisaoAnoLetivo || null,
        rootDir = rootDir || "";

$(window).ready(function (window, document) {

    if (NotaAzul.Boletim == null) {
        NotaAzul.Boletim = {};
    }

    NotaAzul.Boletim.Dados = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _qntPeriodosAvaliacao, _idMatricula,
            _windowBoletimImpressao = null;

        /**
        * @descrição: aplica as média ao boletim
        * @return: void  
        **/
        var _aplicarMedia = function (listaMedia) {
            for (var i = 0, len = listaMedia.length; i < len; i++) {
                //Localiza a célula correspondente a iteração
                var $td = document.querySelectorAll("#Corpo_Boletim td[iddisciplina='" + listaMedia[i].IdDisciplina + "'][periodoavaliacao='" + ((listaMedia[i].PeriodoAvaliacao) - 1) + "']")[0];
                $td.innerHTML = listaMedia[i].ValorAlcancado.toFixed(2);
                $td.style.textAlign = "center";
                $td.style.color = (parseFloat(listaMedia[i].ValorAlcancado) >= parseFloat(mediaMinima)) ? "black" : "red";
            }
        };

        /**
        * @descrição: Carrega o conteúdo do html Boletim/BoletimImpressao em background, acelerando o futuro carregamento
        **/
        var _carregarBoletimImpressao = function () {

            var height = (window.innerHeight < 900) ? (window.innerHeight - 60) : 900;

            _windowBoletimImpressao = new Prion.Window({
                url: rootDir + "Boletim/ViewBoletimImpressao/",
                id: "popupBoletimImpressao",
                height: height,
                title: { text: "Boletim" }
            });
        };

        /**
        * @descrição:carrega as médias de uma matrícula
        **/
        var _carregarMedia = function () {
            Prion.Request({
                url: rootDir + "Boletim/CarregarMediasDeUmaMatricula/" + _idMatricula,
                data: { Paginar: false },
                success: function (retorno) {
                    if ((retorno != null) && (retorno.obj != null)) {
                        _aplicarMedia(retorno.obj);
                    }
                }
            });
        };


        /**
        * @descrição:Cria o cabeçalho de acordo com a configuração do sistema
        **/
        var _criarCabecalhoBoletim = function () {
            var nomeDivisao, qntDivisao,
                domHeader = "Cabecalho_Boletim".getDom();

            if (divisaoAnoLetivo == "semestral") {
                nomeDivisao = "Semestre";
                qntDivisao = 2;
            } else if (divisaoAnoLetivo == "trimestral") {
                nomeDivisao = "Trimestre";
                qntDivisao = 3;
            } else {
                nomeDivisao = "Bimestre";
                qntDivisao = 4;
            }

            for (var i = 1; i <= qntDivisao; i++) {
                var th = document.createElement("TH");
                th.setAttribute("PeriodoAvaliacao", i);
                th.innerHTML = i + "º " + nomeDivisao;
                th.style.border = "1px solid black";
                domHeader.appendChild(th);
            }
            _qntPeriodosAvaliacao = qntDivisao;
        };

        /**
        * @descrição: cria o corpo da tabela(boletim)
        * @return: void
        **/
        var _criarCorpoBoletim = function (listaDisciplinas) {
            var $domCorpoTabela = "Corpo_Boletim".getDom();

            for (var i = 0, len = listaDisciplinas.length; i < len; i++) {
                var $tr = document.createElement("TR"),
                    $tdDisciplina = document.createElement("TD");
                $tdDisciplina.id = "disciplina" + listaDisciplinas[i].Id;
                $tdDisciplina.innerHTML = listaDisciplinas[i].Nome;
                $tdDisciplina.style.border = "1px solid black";
                $tdDisciplina.style.textAlign = "center";

                $tr.appendChild($tdDisciplina);

                for (var j = 0; j < _qntPeriodosAvaliacao; j++) {
                    var $td = document.createElement("TD");
                    $td.setAttribute("idDisciplina", listaDisciplinas[i].Id);
                    $td.setAttribute("periodoAvaliacao", j);
                    $td.style.border = "1px solid gray";

                    $tr.appendChild($td);
                }

                $domCorpoTabela.appendChild($tr);
            }
            _carregarMedia();
        };


        /**
        * @descrição:Imprime o boletim do Aluno
        **/
        var _imprimirBoletim = function () {
            var titulo = document.querySelectorAll("#popupBoletimTitulo .popupWindowTitleH3")[0].cloneNode(true),
                    domBoletim = "Tabela_Boletim".getDom().parentNode.cloneNode(true);

            "Titulo".getDom().appendChild(titulo);
            "Conteudo_Boletim".getDom().appendChild(domBoletim);

            var printContents = document.getElementById("Boletim_Impressao").innerHTML,
                mywindow = window.open("", "Boletim", ""),
                css = "<link rel='stylesheet' type='text/css' href='Content/notaAzul.css' media='print' />",
                html = "<html><head><title>" + document.querySelectorAll("#popupBoletimTitulo .popupWindowTitleH3")[0].innerHTML + "</title>" + css + "</head><body >" +
                            printContents +
                            "</body></html>";

            mywindow.document.write(html);
            mywindow.print();
            mywindow.close();

            _limparBoletimImpressao();
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            _carregarBoletimImpressao();
        };

        /**
        * @descrição:Limpa a janela de boletim impressão
        * @return:void
        **/
        var _limparBoletimImpressao = function () {
            var titulo = "Titulo".getDom(),
                corpo = "Conteudo_Boletim".getDom(),
                i,
                len;

            if (titulo == null || titulo.children.length === 0) { return false; }

            for (i = 0, len = titulo.children.length; i < len; i++) {
                titulo.removeChild(titulo.children[i]);
            }

            if (corpo == null || corpo.children.length === 0) { return false; }

            for (i = 0, len = corpo.children.length; i < len; i++) {
                corpo.removeChild(corpo.children[i]);
            }

            return true;
        };

        /**
        * @descrição:Limpa a janela de boletim
        * @return:void
        **/
        var _limparTela = function () {
            var $listaTh = document.querySelectorAll("#Cabecalho_Boletim th:not([name ='firstChild'])"),
                $rowsCorpo = document.querySelectorAll("#Corpo_Boletim tr");

            if ($listaTh == null) { return false; }

            for (var i = 0, len = $listaTh.length; i < len; i++) {
                $listaTh[i].parentNode.removeChild($listaTh[i]);
            }

            if ($rowsCorpo == null) { return false; }

            for (i = 0, len = $rowsCorpo.length; i < len; i++) {
                $rowsCorpo[i].parentNode.removeChild($rowsCorpo[i]);
            }

            return true;
        };

        /**
        * @descrição: métodos que serão executados quando for abrir para novo registro
        **/
        var _novo = function (nomeAluno, idMatricula, nomeCurso, nomeTurma, nomeTurno) {
            _idMatricula = idMatricula;
            document.querySelectorAll("#popupBoletimTitulo .popupWindowTitleH3")[0].innerHTML += " - " + nomeAluno + " (" + nomeCurso + " - " + nomeTurma + " / " + nomeTurno + ")";
            "Valor_Media".getDom().innerHTML = "Média Mínima:" + mediaMinima;
            _limparTela();
            _limparBoletimImpressao();
            _criarCabecalhoBoletim();
        };
        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            Iniciar: _iniciar,
            CriarCorpoBoletim: _criarCorpoBoletim,
            Imprimir: _imprimirBoletim,
            Novo: _novo
        };
    } ();
    NotaAzul.Boletim.Dados.Iniciar();
} (window, document));