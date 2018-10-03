/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        tipoMedia = tipoMedia || "",
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.NotasAluno == null) {
        NotaAzul.NotasAluno = {};
    }

    NotaAzul.NotasAluno.Dados = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _tabs = null,
            _objNotasMedia = {};

        /**
        * @descrição: Aplica os objetos de MatriculaFormaDeAvaliação
        **/
        var _aplicarListaMatriculaFormasDeAvaliacao = function (obj) {
            var domIdsFormasAvaliacao = document.querySelectorAll("#tabsNotas [name='FormaAvaliacao.Id']"),
                    indice,
                    listaMatriculasFormasAvaliacao = obj.ListaMatriculaFormasDeAvaliacao,
                    matriculaMedia = obj.MatriculaMedia;

            for (indice = 0; indice < listaMatriculasFormasAvaliacao.length; indice++) {
                for (var j = 0; j < domIdsFormasAvaliacao.length; j++) {
                    if (listaMatriculasFormasAvaliacao[indice].IdFormaDeAvaliacao === domIdsFormasAvaliacao[j].value) {
                        var parent = domIdsFormasAvaliacao[j].parentNode;
                        parent.querySelectorAll("[name='valorAlcancado']")[0].innerHTML = listaMatriculasFormasAvaliacao[indice].ValorAlcancado;
                        continue;
                    }
                }
            }
            var qntAbas = "ulTabs".getDom().children.length;

            if (matriculaMedia.Notas.length === 0) {
                //caso o objeto MatriculaMedia seja vazio, soma os valores alcançados nas avaliações,e os seta em suas respectivas notas
                for (indice = 1; indice <= qntAbas; indice++) {
                    var domValorAlcancado = document.querySelectorAll("#Nota" + indice + " [name='valorAlcancado']"),
                        totalAlcancado = 0;

                    for (var k = 0; k < domValorAlcancado.length; k++) {
                        totalAlcancado += parseFloat(domValorAlcancado[k].innerHTML);
                    }
                    ("Nota" + indice + "_ValorTotalAlcancado").getDom().innerHTML = totalAlcancado;
                    ("Nota" + indice + "_ValorTotalFinal").getDom().value = totalAlcancado;
                }

                return;
            }
            var len = matriculaMedia.Notas.length;
            for (indice = 0; indice < len; indice++) {
                for (var l = 1; l <= qntAbas; l++) {
                    if (("Id_Nota" + l).getValue() === matriculaMedia.Notas[indice].IdComposicaoNota) {
                        ("Id_MatriculaNota" + l).setValue(matriculaMedia.Notas[indice].Id);
                        ("Nota" + l + "_ValorTotalAlcancado").getDom().innerHTML = matriculaMedia.Notas[indice].ValorAlcancado;
                        ("Nota" + l + "_ValorTotalFinal").getDom().value = matriculaMedia.Notas[indice].ValorFinal;

                        "Media".getDom().innerHTML = matriculaMedia.ValorAlcancado;
                        "Media_Id".setValue(matriculaMedia.Id);
                    }
                }
            }
        };

        /**
        * @descrição: Aplica o objeto de
        **/
        var _aplicarObjetoDeComposicaoNota = function (objComposicaoNota) {
            for (var i = 0; i < objComposicaoNota.ListaNotas.length; i++) {

                if (i !== 0) {
                    _criarAba();
                }

                var idTabDom = "Nota" + (i + 1),
                        tabDom = (idTabDom).getDom();

                "ComposicaoNotaPeriodo_Id".getDom().value = objComposicaoNota.Id;

                tabDom.querySelectorAll("#Id_Nota" + (i + 1))[0].value = objComposicaoNota.ListaNotas[i].Id;
                tabDom.querySelectorAll("#Peso_Nota" + (i + 1))[0].innerHTML = objComposicaoNota.ListaNotas[i].Peso;
                _aplicarListaFormasDeAvaliacao(objComposicaoNota.ListaNotas[i].ListaFormasDeAvaliacao, idTabDom);
            }
        };

        /**
        * @descrição: Aplica os objetos de Formas de avaliação
        **/
        var _aplicarListaFormasDeAvaliacao = function (listaFormasAvaliacao, idNotaDom) {
            for (var i = 0; i < listaFormasAvaliacao.length; i++) {
                if (i !== 0) {
                    _clonarFormadeAvaliacao(idNotaDom, i);
                }
                //Seleciona o DOM da forma de avaliação correspondente a iteração atual
                var formaAvaliacaoDom = document.getElementById(idNotaDom + "_FormaAvaliacao" + i);

                //Aplica o valor do input valor correspondente
                document.getElementById(idNotaDom + "_FormaAvaliacao_ValorTotal" + i).innerHTML = "/" + listaFormasAvaliacao[i].Valor;
                document.getElementById(idNotaDom + "_FormaAvaliacao_Id" + i).value = listaFormasAvaliacao[i].Id;
                document.getElementById(idNotaDom + "_FormaAvaliacao_DataAvaliacao" + i).innerHTML = Prion.Format(listaFormasAvaliacao[i].DataAvaliacao, "dd/mm/aaaa");
                document.getElementById(idNotaDom + "_FormaAvaliacao_Nome" + i).innerHTML = listaFormasAvaliacao[i].Tipo;
            }
        };


        /**
        * @descrição: Calcula a média de acordo com as configurações
        **/
        var _calcularTipoMedia = function () {
            switch (tipoMedia) {
                case "aritmetica_simples":
                    return _mediaAritmeticaSimples();

                case "aritmetica_ponderada":
                    return _mediaAritmeticaPonderada();

                case "geometrica":
                    return _mediaGeometrica();

                case "harmonica":
                    return _mediaHarmonica();

                default:
                    return false;
            }
        };

        /**
        * @Descrição: responsável por calcular a média de acordo com as configurações
        **/
        var _calcularMedia = function () {
            _objNotasMedia = _montarObjMatriculaMedia();
            console.log(JSON.stringify(_objNotasMedia));
            _objNotasMedia.ValorAlcancado = _calcularTipoMedia();
            "Media".getDom().innerHTML = _objNotasMedia.ValorAlcancado.toFixed(2);
            if (window.confirm("Deseja salvar a Média?") === true) {
                NotaAzul.NotasAluno.Dados.Salvar();
                _objNotasMedia = null;
            }
        };

        /**
        * @descrição: Cria uma Nova Aba
        * @return: void
        **/
        var _criarAba = function () {

            var tabs = "ulTabs".getDom(),
                titulo = "Nota " + ("ulTabs".getDom().children.length + 1),
                idTab = "Nota" + ("ulTabs".getDom().children.length + 1);

            //seleciona e clona o conteúdo da primeira tab
            var contentTab = "Nota1".getDom().innerHTML;

            _tabs.Create({
                id: idTab,
                title: titulo,
                content: contentTab,
                style: "height: 355px !important; width: 520px !important;margin-top: 20px;"
            });

            //Remove qualquer forma de avaliação extra
            var queryFormaAvaliacao = "#" + idTab + " [name='divFormaAvaliacao']",
                arrayFormaAvaliacao = document.querySelectorAll(queryFormaAvaliacao);

            for (var i = arrayFormaAvaliacao.length - 1; i > 0; i--) {
                _excluirFormasDeAvaliacao(arrayFormaAvaliacao[i]);
            }

            var domNovaAba = idTab.getDom();


            //Altera o Id e zera o valores da nova aba
            domNovaAba.querySelectorAll("#Nota1_DivFormaAvaliacao")[0].id = idTab + "_DivFormaAvaliacao";

            domNovaAba.querySelectorAll("#Id_Nota1")[0].id = "Id_" + idTab;
            ("Id_" + idTab).getDom().value = 0;
            domNovaAba.querySelectorAll("#Id_MatriculaNota1")[0].id = "Id_Matricula" + idTab;
            ("Id_" + idTab).getDom().value = 0;

            domNovaAba.querySelectorAll("#Nota1_FormaAvaliacao_Id0")[0].id = idTab + "_FormaAvaliacao_Id0";
            (idTab + "_FormaAvaliacao_Id0").getDom().value = 0;

            domNovaAba.querySelectorAll("#Peso_Nota1")[0].id = "Peso_" + idTab;
            domNovaAba.querySelectorAll("#Nota1_FormaAvaliacao_Nome0")[0].id = idTab + "_FormaAvaliacao_Nome0";
            domNovaAba.querySelectorAll("#Nota1_FormaAvaliacao_ValorAlcancado0")[0].id = idTab + "_FormaAvaliacao_ValorAlcancado0";
            domNovaAba.querySelectorAll("#Nota1_FormaAvaliacao_ValorTotal0")[0].id = idTab + "_FormaAvaliacao_ValorTotal0";
            domNovaAba.querySelectorAll("#Nota1_FormaAvaliacao_DataAvaliacao0")[0].id = idTab + "_FormaAvaliacao_DataAvaliacao0";
            domNovaAba.querySelectorAll("#Nota1_ValorTotalAlcancado")[0].id = idTab + "_ValorTotalAlcancado";

            domNovaAba.querySelectorAll("#Nota1_ValorTotalFinal")[0].id = idTab + "_ValorTotalFinal";
            (idTab + "_ValorTotalFinal").getDom().value = ""
            ;
            //Adiciona o evento onChange           
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
                arrayCampos[i].innerHTML = "";
            }

            parent.appendChild(divClonada);

            (idTab + "_FormaAvaliacao_Id" + indice).getDom().value = 0;
        };


        /**
        * @descrição: Define a ação para todos os controles da tela
        * @return: void
        **/
        var _definirAcoesBotoes = function () {

        };

        /**
        * @descrição: Método que será chamado sempre após Salvar
        **/
        var _executarAposSalvar = function (retorno) {

        };

        /**
        * @descrição: Exclui uma forma de avaliação
        **/
        var _excluirFormasDeAvaliacao = function (elemento) {
            elemento.parentNode.removeChild(elemento);
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
            _tabs.Show(0);
        };

        /**
        * @descrição:limpa a janela
        **/
        var _limparJanelaComposicaoNota = function () {

            //Remove as formas de avaliação extras
            var arrayFormasAvaliacao = document.querySelectorAll("#Nota1 [name='divFormaAvaliacao']");
            for (var i = arrayFormasAvaliacao.length - 1; i > 0; i--) {
                _excluirFormasDeAvaliacao(arrayFormasAvaliacao[i]);
            }
        };

        /**
        * @descrição: Limpa as abas e todo o seu conteúdo
        **/
        var _limparTabs = function () {
            // elimina as linhas da lista
            var domElememt = document.querySelectorAll("#ulTabs li"),
                parent = "ulTabs".getDom(),
                i;
            for (i = (domElememt.length - 1); i >= 1; i--) {
                parent.removeChild(domElememt[i]);
            }

            // elimina as div correspondentes
            var element = document.querySelectorAll("#tabsNotas>div"),
                parentElement = "tabsNotas".getDom();
            for (i = element.length - 1; i >= 1; i--) {
                parentElement.removeChild(element[i]);
            }
        };

        /**
        * @descrição: Calcula a média aritimética simples
        * @returns: valor da média
        **/
        var _mediaAritmeticaSimples = function () {
            var qntNotas = _objNotasMedia.ListaNotas.length,
                total = 0;

            for (var i = 0; i < qntNotas; i++) {
                total += _objNotasMedia.ListaNotas[i].ValorFinal;
            }

            return (total / qntNotas);
        };

        /**
        * @descrição: Calcula a média aritimética ponderada
        * @returns: valor da média
        **/
        var _mediaAritmeticaPonderada = function () {
            var qntNotas = _objNotasMedia.ListaNotas.length,
                total = 0,
                pesoFinal = 0;

            for (var i = 0; i < qntNotas; i++) {
                total += (_objNotasMedia.ListaNotas[i].ValorFinal * _objNotasMedia.ListaNotas[i].Peso);
                pesoFinal += _objNotasMedia.ListaNotas[i].Peso;
            }

            return (total / pesoFinal);
        };

        /**
        * @descrição: Calcula a média geométrica 
        * @returns: valor da média
        **/
        var _mediaGeometrica = function () {
            var qntNotas = _objNotasMedia.ListaNotas.length,
                total = 0,
                pesoFinal = 0;

            for (var i = 0; i < qntNotas; i++) {
                total = total * _objNotasMedia.ListaNotas[i].ValorFinal;
            }

            //Para extrair raiz cujo expoente não seja dois,esse é o melhor método
            return Math.pow(total, 1 / qntNotas);
        };

        /**
        * @descrição: Calcula a média harmônica 
        * @returns: valor da média
        **/
        var _mediaHarmonica = function () {
            var qntNotas = _objNotasMedia.ListaNotas.length,
                total = 0;

            for (var i = 0; i < qntNotas; i++) {
                total = total + (1 / _objNotasMedia.ListaNotas[i].ValorFinal);
            }

            //Para extrair raiz cujo expoente não seja dois,esse é o melhor método
            return (qntNotas / total);
        };

        /**
        * @descrição: Monta o objeto que será transformado em Json
        * @returns: objeto de composição de nota
        **/
        var _montarObjMatriculaMedia = function () {
            var matriculaMedia = {},
                idComposicaoNota = "ComposicaoNotaPeriodo_Id".getValue(),
                qntNotas = "ulTabs".getDom().children.length,
                idMatricula = "Matricula_Id".getValue(),
                idMatriculaMedia = "Media_Id".getValue();

            matriculaMedia.Id = (idMatriculaMedia === "") ? 0 : idMatriculaMedia;
            matriculaMedia.IdComposicaoNota = (idComposicaoNota === 0) ? 0 : idComposicaoNota;
            matriculaMedia.IdMatricula = idMatricula;
            matriculaMedia.ValorAlcancado = 0;
            matriculaMedia.ListaNotas = [];

            var arrayNotas = [];

            for (var i = 1; i <= qntNotas; i++) {
                var notaMatricula = {},
                    idNota = ("Id_Nota" + i).getValue(),
                    idMatriculaNota = ("Id_MatriculaNota" + i).getValue();

                notaMatricula.Id = (idMatriculaNota === "") ? 0 : idMatriculaNota;
                notaMatricula.IdComposicaoNota = (idNota === "") ? 0 : idNota;
                notaMatricula.IdMatricula = idMatricula;
                notaMatricula.Peso = parseFloat(("Peso_Nota" + i).getDom().innerHTML);
                notaMatricula.ValorAlcancado = parseFloat(("Nota" + i + "_ValorTotalAlcancado").getDom().innerHTML);
                notaMatricula.ValorFinal = parseFloat(("Nota" + i + "_ValorTotalFinal").getDom().value);

                arrayNotas.push(notaMatricula);
            }

            matriculaMedia.ListaNotas = arrayNotas;

            return matriculaMedia;
        };

        /**
        * @descrição: métodos que serão executados quando for abrir para novo registro
        **/
        var _novo = function (nomeAluno, idMatricula, objComposicaoNota) {
            //altera o título
            document.querySelectorAll("#popupNotasTitulo .popupWindowTitleH3")[0].innerHTML = nomeAluno;
            "Matricula_Id".setValue(idMatricula);
            "Media".getDom().innerHTML = "";
            "Nota1_ValorTotalFinal".setValue("");
            "Nota1_ValorTotalAlcancado".getDom().innerHTML = "";

            _limparTabs();
            _limparJanelaComposicaoNota();

            _aplicarObjetoDeComposicaoNota(objComposicaoNota);
        };
        
        /**
        * @descrição: Salva todos os dados do form
        **/
        var _salvar = function (win) {

            Prion.Request({
                url: rootDir + "NotasAluno/SalvarMedia",
                data: "NotasMedia=" + JSON.stringify(_objNotasMedia),
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

        _iniciar();
        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            AplicarListaMatriculaFormasDeAvaliacao: _aplicarListaMatriculaFormasDeAvaliacao,
            CalcularMedia: _calcularMedia,
            ExecutarAposSalvar: _executarAposSalvar,
            Iniciar: _iniciar,
            Novo: _novo,
            Salvar: _salvar
        };
    } ();
    NotaAzul.NotasAluno.Dados.Iniciar();
} (window, document));