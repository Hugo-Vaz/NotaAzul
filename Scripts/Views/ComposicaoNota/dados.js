/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "",
        divisaoAnoLetivo = divisaoAnoLetivo || "",
        formaConceito = formaConceito || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.ComposicaoNota == null) {
        NotaAzul.ComposicaoNota = {};
    }

    NotaAzul.ComposicaoNota.Dados = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _tabs = null,
            _composicaoNota = {};

        /**
        * @descrição: Adiciona evento onBlur para o campo Valor
        * @return: void
        **/
        var _adicionarEventoParaFormaDeAvaliacaoValor = function (idAba, indice) {
            var valorDom = (idAba + "_Valor" + indice).getDom();

            Prion.Event.add(valorDom, "change", function () {
                var valorTotalDOM = (idAba + "_ValorTotal").getDom(),
                    valor = 0,
                    arrayValor = document.querySelectorAll("#" + idAba + " [name='valor']");

                for (var i = 0; i < arrayValor.length; i++) {
                    valor += parseFloat(arrayValor[i].value);
                }

                //Verifica qual é a forma de conceito salva nas configurações, se o valor do campo ou se a soma de um novo valor com o valor total ultrapassa o estipulado
                // a variável 'formaConceito' esta em Dados.cshtml
                if (
                    ((formaConceito.toLowerCase() === "base_cem") && (valorDom.value < 100)) && (valor < 100) ||
                    ((formaConceito.toLowerCase() === "base_dez") && (valorDom.value < 10)) && (valor < 10)
                   ) {
                    _clonarFormadeAvaliacao(idAba, indice + 1);
                }

                //caso caia aqui o valor estipulado foi estrapolado e exibe um confirm para o usuário
                else if (
                    (formaConceito.toLowerCase() === "base_cem") && ((valorDom.value > 100) || (valor > 100)) ||
                    (formaConceito.toLowerCase() === "base_dez") && ((valorDom.value > 10) || (valor > 10))
                   ) {
                    if (window.confirm("O valor inserido extrapola o total estipulado. Deseja continuar?") === false) {
                        valorDom.value = "";
                        return false;
                    }
                }
                //Adiciona o valor total das formas de avaliação na parte mais baixa da janela

                valorTotalDOM.innerHTML = valor;

                return true;
            });
        };

        /**
        * @descrição: Aplica os objeto de Formas de avaliação
        **/
        var _aplicarListaFormasDeAvaliacao = function (listaFormasAvaliacao, idNotaDom) {
            var len,
                i;

            for (i = 0, len = listaFormasAvaliacao.length; i < len; i++) {
                if (i !== 0) {
                    _clonarFormadeAvaliacao(idNotaDom, i);
                }
                //Seleciona o DOM da forma de avaliação correspondente a iteração atual
                var $formaAvaliacaoDom = document.getElementById(idNotaDom + "_FormaAvaliacao" + i);

                //Aplica o valor do input valor correspondente
                document.getElementById(idNotaDom + "_Valor" + i).value = listaFormasAvaliacao[i].Valor;
                document.getElementById(idNotaDom + "_FormaAvaliacao_Id" + i).value = listaFormasAvaliacao[i].Id;

                var data = Prion.Format(listaFormasAvaliacao[i].DataAvaliacao, "dd/mm/aaaa"),
                    dataFormatada = (data.toString().substring(6, 10) + "-" + data.toString().substring(3, 5) + "-" + data.toString().substring(0, 2));

                document.getElementById(idNotaDom + "_DataAvaliacao" + i).value = dataFormatada;

                //Seleciona o valor do ComboBox
                var selectDom = document.getElementById(idNotaDom + "_ListaFormaDeAvaliacao" + i);
                Prion.ComboBox.SelecionarIndexPeloValue(selectDom, listaFormasAvaliacao[i].Tipo);
            }

            var $arrayValor = document.querySelectorAll("#" + idNotaDom + " [name='valor']"),
                valor = 0;

            for (i = 0, len = $arrayValor.length; i < len; i++) {
                valor += parseFloat($arrayValor[i].value);
            }

            //Aplica o valor total
            (idNotaDom + "_ValorTotal").getDom().innerHTML = valor;
        };

        /**
        * @descrição: Aplica o objeto de
        **/
        var _aplicarObjetoDeComposicaoNota = function (objComposicaoNota) {
            _composicaoNota = objComposicaoNota;
            for (var i = 0, len = objComposicaoNota.ListaNotas.length; i < len; i++) {
                if (i !== 0) {
                    _criarAba();
                }

                var idTabDom = "Nota" + (i + 1),
                        tabDom = (idTabDom).getDom();

                "ComposicaoNota_Id".getDom().value = objComposicaoNota.Id;

                tabDom.querySelectorAll("#Id_Nota" + (i + 1))[0].value = objComposicaoNota.ListaNotas[i].Id;
                tabDom.querySelectorAll("[name='Peso']")[0].value = objComposicaoNota.ListaNotas[i].Peso;
                _aplicarListaFormasDeAvaliacao(objComposicaoNota.ListaNotas[i].ListaFormasDeAvaliacao, idTabDom);
            }
        };

        /**
        * @descrição: Efetua o carregamento de uma composição de nota através do bimestre
        * @return: um objeto de composição de nota
        **/
        var _carregarPorBimestre = function (periodo) {

            Prion.Request({
                url: rootDir + "ComposicaoNota/CarregarPorBimestre",
                data: { Entidades: "Funcionario,Disciplina,Professor,FormaAvaliacao,Nota", "ComposicaoNotaPeriodo.IdProfessorDisciplina": "ProfessorDisciplina_Id".getValue(), "ComposicaoNotaPeriodo.PeriodoDeAvaliacao": periodo.value },
                success: function (retorno) {
                    _limparTabs();
                    _limparJanelaComposicaoNota();
                    "Id_Nota1".getDom().value = 0;
                    "Nota1_FormaAvaliacao_Id0".getDom().value = 0;
                    "ComposicaoNota_Id".getDom().value = 0;
                    document.getElementsByName("Peso")[0].value = 1;

                    if ((retorno != null) && (retorno.obj != null)) {
                        NotaAzul.ComposicaoNota.Dados.AplicarObjetoDeComposicaoNota(retorno.obj);
                    }
                }
            });

        };

        /**
        * @descrição: Cria uma Nova Aba
        * @return: void
        **/
        var _criarAba = function () {
            //seleciona o botão de adicionar uma tab,e o exclui do documento
            var elemento = "addTab".getDom();
            elemento.parentNode.removeChild(elemento);

            var tabs = "ulTabs".getDom(),
                titulo = "Nota " + ("ulTabs".getDom().children.length + 1),
                idTab = "Nota" + ("ulTabs".getDom().children.length + 1);

            //seleciona e clona o conteúdo da primeira tab
            var contentTab = "Nota1".getDom().innerHTML;

            _tabs.Create({
                id: idTab,
                title: titulo,
                content: contentTab,
                style: "height:306px !important; width: 520px !important;"
            });

            //reinsere o botão addTab
            tabs.appendChild(elemento);

            //Remove qualquer forma de avaliação extra
            var queryFormaAvaliacao = "#" + idTab + " [name='divFormaAvaliacao']",
                arrayFormaAvaliacao = document.querySelectorAll(queryFormaAvaliacao);

            for (var i = arrayFormaAvaliacao.length - 1; i > 0; i--) {
                _excluirFormasDeAvaliacao(arrayFormaAvaliacao[i]);
            }

            var domNovaAba = idTab.getDom();

            //recria o onchange do select
            domNovaAba.querySelectorAll("#Nota1_ListaFormaDeAvaliacao0")[0].id = idTab + "_ListaFormaDeAvaliacao0";
            $.uniform.restore("#tabsNotas [name = 'listaFormaDeAvaliacao'");
            $("#tabsNotas  [name = 'listaFormaDeAvaliacao'").uniform();


            //Altera o Id e zera o valores da nova aba
            domNovaAba.querySelectorAll("#Nota1_DivFormaAvaliacao")[0].id = idTab + "_DivFormaAvaliacao";

            domNovaAba.querySelectorAll("#Id_Nota1")[0].id = "Id_" + idTab;
            ("Id_" + idTab).getDom().value = 0;

            domNovaAba.querySelectorAll("#Nota1_ValorTotal")[0].id = idTab + "_ValorTotal";
            (idTab + "_ValorTotal").getDom().innerHTML = 0;

            domNovaAba.querySelectorAll("#Nota1_FormaAvaliacao_Id0")[0].id = idTab + "_FormaAvaliacao_Id0";
            (idTab + "_FormaAvaliacao_Id0").getDom().value = 0;

            domNovaAba.querySelectorAll("#Nota1_Valor0")[0].id = idTab + "_Valor0";
            (idTab + "_Valor0").getDom().value = 0;

            domNovaAba.querySelectorAll("#Nota1_DataAvaliacao0")[0].id = idTab + "_DataAvaliacao0";
            (idTab + "_DataAvaliacao0").getDom().value = 0;

            domNovaAba.querySelectorAll("[name='Peso']")[0].value = 1;
            //Adiciona o evento onChange
            _adicionarEventoParaFormaDeAvaliacaoValor(idTab, 0);
            idTab.getDom().setAttribute("data", "TabNota");

            return ("ulTabs".getDom().children.length - 1);
        };

        /**
        * @descrição: Clona o html de forma de avaliação
        * @return: void
        **/
        var _clonarFormadeAvaliacao = function (idTab, indice) {
            var parent = (idTab + "_DivFormaAvaliacao").getDom();

            var divClonada = "Nota1_FormaAvaliacao0".getDom().cloneNode(true);
            divClonada.id = (idTab + "_FormaAvaliacao" + indice);

            var arrayCampos = divClonada.querySelectorAll("[mudar='true']");

            for (var i = 0; i < arrayCampos.length; i++) {
                var complementoId = arrayCampos[i].id.slice(6, arrayCampos[i].id.length - 1);
                arrayCampos[i].id = idTab + "_" + complementoId + indice;
                if (arrayCampos[i].tagName.toLowerCase() == "input") {
                    arrayCampos[i].value = "";
                }
                if (arrayCampos[i].tagName.toLowerCase() == "select") {
                    var selectId = arrayCampos[i].id;
                }
            }

            parent.appendChild(divClonada);
            $.uniform.restore("#tabsNotas [name = 'listaFormaDeAvaliacao'");
            $("#tabsNotas [name = 'listaFormaDeAvaliacao'").uniform();
            (idTab + "_FormaAvaliacao_Id" + indice).getDom().value = 0;
            _adicionarEventoParaFormaDeAvaliacaoValor(idTab, indice);
        };


        /**
        * @descrição: Define a ação para todos os controles da tela
        * @return: void
        **/
        var _definirAcoesBotoes = function () {
            // quando clicar no botão de buscar cursos...
            Prion.Event.add("addTab", "click", function () {
                var id = _criarAba();
                _tabs.Show(id);
            });

            //quando o valor do combo mudar, um novo carregamento será efetuado
            Prion.Event.add("listaBimestre", "change", function () {
                _carregarPorBimestre("listaBimestre".getValue());
            });

            Prion.Event.add("listaTrimestre", "change", function () {
                _carregarPorBimestre("listaTrimestre".getValue());
            });

        };

        /**
        * @descrição: Método responsável por apagar uma nota
        **/
        var _deletarNota = function (objeto) {
            if (window.confirm("Esta Nota e suas respectivas formas de avaliação serão apagadas.Deseja continuar") === false) { return false; }

            var idNota = ("Id_" + objeto.parentElement.id).getDom().value,
                    $notaDom = (objeto.parentElement.id).getDom(),
                    $abaDom = document.querySelectorAll("#tabsNotas [data='" + $notaDom.id + "']")[0];

            if (idNota !== 0) {
                $notaDom.style.display = "none";
                $abaDom.parentElement.style.display = "none";
                return false;
            }

            $notaDom.parentElement.removeChild($notaDom);
            $abaDom.parentElement.parentElement.removeChild($abaDom.parentElement);

            return true;
        };

        /**
        * @descrição: Exclui uma forma de avaliação
        **/
        var _excluirFormasDeAvaliacao = function (elemento) {
            elemento.parentNode.removeChild(elemento);
        };

        /**
        * @descrição: Método que será chamado sempre após Salvar
        **/
        var _executarAposSalvar = function (retorno) {

        };

        /**
        * @descrição:limpa a janela
        **/
        var _limparJanelaComposicaoNota = function () {
            "Nota1_Peso".getDom().value = 1;
            "Nota1_Valor0".getDom().value = "";
            "Nota1_ValorTotal".getDom().innerHTML = 0;

            //Remove as formas de avaliação extras
            var arrayFormasAvaliacao = document.querySelectorAll("#Nota1 [name='divFormaAvaliacao']");
            for (var i = arrayFormasAvaliacao.length - 1; i > 0; i--) {
                _excluirFormasDeAvaliacao(arrayFormasAvaliacao[i]);
            }
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            if (_tabs == null) {
                _tabs = new Prion.Tabs({
                    id: "tabsNotas",
                    selected: 0
                });
            }
            _definirAcoesBotoes();
            _adicionarEventoParaFormaDeAvaliacaoValor("Nota1", 0);
            _tabs.Show(0);
        };

        /**
        * @descrição: Limpa as abas e todo o seu conteúdo
        **/
        var _limparTabs = function () {
            // elimina as linhas da lista
            var $domElememt = document.querySelectorAll("#ulTabs li"),
                $parent = "ulTabs".getDom(),
                i;

            for (i = ($domElememt.length - 1); i >= 1; i--) {
                $parent.removeChild($domElememt[i]);
            }

            // elimina as div correspondentes
            var $element = document.querySelectorAll("#tabsNotas>div"),
                $parentElement = "tabsNotas".getDom();
            for (i = $element.length - 1; i >= 1; i--) {
                $parentElement.removeChild($element[i]);
            }
        };

        /**
        * @descrição: Monta o objeto que será transformado em Json
        * @returns: objeto de composição de nota
        **/
        var _montarObjetoComposicaoNota = function () {
            var composicaoNota = {},
                idComposicaoNota = "ComposicaoNota_Id".getValue();

            composicaoNota.Id = (idComposicaoNota === 0) ? 0 : idComposicaoNota;
            composicaoNota.IdProfessorDisciplina = "ProfessorDisciplina_Id".getValue();
            composicaoNota.FormaDivisao = divisaoAnoLetivo; // esta em Dados.cshtml
            composicaoNota.PeriodoAvaliacao = "listaBimestre".getValue().value;

            //Monta o array com os objetos de nota
            var arrayDomNotas = document.querySelectorAll("#tabsNotas [data='TabNota']"),
                arrayNotas = [];

            for (var i = 0, len = arrayDomNotas.length; i < len; i++) {
                var nota = {},
                    idNota = arrayDomNotas[i].querySelectorAll("[name='Nota.Id']")[0].value;

                nota.Id = (idNota === "") ? 0 : idNota;
                nota.Peso = arrayDomNotas[i].querySelectorAll(" [name='Peso']")[0].value;
                nota.EstadoObjeto = ((arrayDomNotas[i].id).getDom().style.display === "none") ? EstadoObjeto.Excluido : EstadoObjeto.Novo;

                var arrayDomFormasAvaliacao = arrayDomNotas[i].querySelectorAll(" [name='divFormaAvaliacao']"),
                    arrayFormasAvaliacao = [];

                //Monta o array com objetos de formas de avaliação
                for (var j = 0, len2 = arrayDomFormasAvaliacao.length; j < len2; j++) {
                    var formaAvaliacao = {},
                        indice = i + 1, //O indice sempre será incrementado por um,pois o Id de Nota é iniciado em 1 e não em 0
                        idFormaAvaliacao = ("Nota" + indice + "_FormaAvaliacao_Id" + j).getDom().value;

                    formaAvaliacao.Id = (idFormaAvaliacao === "") ? 0 : idFormaAvaliacao;

                    var selectForma = ("Nota" + indice + "_ListaFormaDeAvaliacao" + j).getDom();
                    formaAvaliacao.formaAvaliacao = selectForma.options[selectForma.selectedIndex].value;

                    formaAvaliacao.Valor = ("Nota" + indice + "_Valor" + j).getDom().value;
                    formaAvaliacao.DataAvaliacao = ("Nota" + indice + "_DataAvaliacao" + j).getDom().value;

                    arrayFormasAvaliacao.push(formaAvaliacao);
                }

                nota.FormasAvaliacao = arrayFormasAvaliacao;
                arrayNotas.push(nota);
            }

            composicaoNota.Notas = arrayNotas;

            return composicaoNota;
        };

        /**
        * @descrição: métodos que serão executados quando for abrir para novo registro
        **/
        var _novo = function (nomeDisciplina, idDisciplina, idTurma) {
            //altera o título
            document.querySelectorAll("#popupComposicaoNotaTitulo .popupWindowTitleH3")[0].innerHTML = nomeDisciplina;
            //Seta os ids que não mudam por composição
            "ProfessorDisciplina_Id".setValue(idDisciplina);
            "ProfessorDisciplina_IdTurma".setValue(idTurma);

            //Deixa o primeiro bimestre como selecionado
            Prion.ComboBox.SelecionarIndexPeloValue("listaBimestre", 1);

            if (divisaoAnoLetivo != null || divisaoAnoLetivo !== "") {
                divisaoAnoLetivo.getDom().style.display = "inherit";
            }

            _limparTabs();
            _limparJanelaComposicaoNota();
        };

        /**
        * @descrição: Salva todos os dados do form
        **/
        var _salvar = function (win) {
            if (!Prion.ValidateForm("frmComposicaoNota")) {
                win.config.reloadObserver = false;
                return false;
            }

            if (win != null) { win.mask(); }

            var composicaoNota = _montarObjetoComposicaoNota();

            Prion.Request({
                url: rootDir + "ComposicaoNota/Salvar",
                data: "ComposicaoNota=" + JSON.stringify(composicaoNota),
                success: function (retorno, registroNovo) {
                    win.config.reloadObserver = true;
                    win.hide({ animate: true });
                    return;
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
            Iniciar: _iniciar,
            AplicarObjetoDeComposicaoNota: _aplicarObjetoDeComposicaoNota,
            DeletarNota: _deletarNota,
            ExecutarAposSalvar: _executarAposSalvar,
            Novo: _novo,
            Salvar: _salvar,
            TestarArray: _montarObjetoComposicaoNota
        };
    } ();
    NotaAzul.ComposicaoNota.Dados.Iniciar();
} (window, document));