/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";

    if (NotaAzul.Matricula == null) {
        NotaAzul.Matricula = {};
    }

    NotaAzul.Matricula.Dados = function () {
        /**********************************************************************************************
        ** PRIVATE
        **********************************************************************************************/
        var _windowFicha = null,
            _windowCarneMensalidade = null,
            _listaGridMensalidades = null,
            _tabs = null,
            TOTAL_MENSALIDADES_POR_PAGINA = 5;

        /**
        * @descrição: Método responsável por abrir a ficha de Matrícula
        * @params: obj => objeto que representa a matrícula atual
        * @return: void
        **/
        var _abrirFichaMatricula = function (obj) {
            if (obj == null) {
                Prion.Alert({ msg: "Erro ao obter o objeto de Matrícula." });
                return;
            }

            _windowFicha.apply({
                object: 'Matricula',
                url: rootDir + "Matricula/Carregar/" + obj["Matricula.Id"],
                filter: { Entidades: "Aluno, AlunoResponsavel, Endereco, Telefone, Curso, MatriculaCurso, CursoAnoLetivo, Turma, Turno" },
                ajax: { onlyJson: true },
                success: function (retorno) {
                    NotaAzul.Matricula.Dados.PreencherFichaDeMatricula(retorno.obj);
                }
            }).load().show({ animate: true });
        };

        /**
        * @descrição: Método responsável por abrir a janela com o carnê de Mensalidades
        * @params: obj => objeto que representa a matrícula atual
        * @return: void
        **/
        var _abrirCarneDeMensalidade = function (obj) {
            if (obj == null) {
                Prion.Alert({ msg: "Erro ao obter o objeto de Matrícula." });
                return;
            }

            _windowCarneMensalidade.apply({
                object: 'Matricula',
                url: rootDir + "Matricula/Carregar/" + obj["Matricula.Id"],
                filter: { Entidades: "Aluno, AlunoResponsavel,Telefone, Curso, MatriculaCurso, CursoAnoLetivo, Turma, Turno, Mensalidade" },
                ajax: { onlyJson: true },
                success: function (retorno) {
                    NotaAzul.Matricula.Dados.GerarCarneDeMensalidades(retorno.obj);
                }
            }).load();
        };


        /**
        * @descrição: Método responsável por abrir a janela com o carnê de Mensalidades Agregado
        * @params: obj => objeto que representa a matrícula atual
        * @return: void
        **/
        var _abrirCarneDeMensalidadeAgregado = function (obj) {
            if (obj == null) {
                Prion.Alert({ msg: "Erro ao obter o objeto de Matrícula." });
                return;
            }

            _windowCarneMensalidade.apply({
                object: 'Matricula',
                url: rootDir + "Matricula/Carregar/" + obj["Matricula.Id"],
                filter: { Entidades: "Aluno, AlunoResponsavel, Telefone, Curso, MatriculaCurso, CursoAnoLetivo, Turma, Turno, Mensalidade" },
                ajax: { onlyJson: true },
                success: function (retorno) {
                    NotaAzul.Matricula.Dados.GerarCarneDeMensalidadesAgregado(retorno.obj);
                }
            }).load();
        };

        /**
        * @descrição: Responsável pelo AutoComplete do campo Aluno
        * @return: void
        **/
        var _autoCompleteBuscarAluno = function () {
            $("#Matricula_NomeAluno").autocomplete({
                source: rootDir + "Aluno/ListaAlunoPorNome",
                minLength: 2,
                appendTo: $("#Matricula_NomeAluno").parent(),
                delay: 500,
                dataType: 'json',
                focus: true,
                open: function () {
                    $(".ui-autocomplete:visible").css({ top: "110px", left: "118px", "max-height": "160px", "overflow-y": "auto" });
                },
                select: function (event, data) {
                    "Matricula_NomeAluno".setValue(data.item.label);
                    "Matricula_IdAluno".setValue(data.item.value);
                    return false;
                }
            });
        };

        /**
        * @descrição: Atualiza as mensalidades de acordo com o desconto do GrupoDesconto selecionado
        * @return: void
        **/
        var _atualizarMensalidade = function () {
            var objGrupoDesconto = "matriculaListaGrupoDesconto".getValue(),
                    idLista = document.getElementsByClassName("div selected")[0].firstChild.id,
                    $arrInputDesconto = document.querySelectorAll("#" + idLista + "Body [namejson='Desconto']"),
                    valorMensalidade = document.querySelectorAll("#" + idLista + "Body [namejson='Valor']"),
                    situacaoMensalidade = document.querySelectorAll("#" + idLista + "Body tr"),
                    idCursoAbaSelecionada = document.getElementsByClassName("selected")[0].id.replace("tab", ""),
                    i,
                    len,
                    indiceTab,
                    valorDesconto;

            valorMensalidade = parseFloat(valorMensalidade[0].value.substring(3).replace(/\,/g, '.'));

            for (i = 0, len = NotaAzul.Matricula.Dados.ListaMensalidades().length; i < len; i++) {
                if (NotaAzul.Matricula.Dados.ListaMensalidades()[i].idCurso == idCursoAbaSelecionada) {
                    indiceTab = i;
                    break;
                }
            }

            for (i = 0, len = NotaAzul.Matricula.Dados.ListaMensalidades()[indiceTab].grid.rows().length; i < len; i++) {
                if (NotaAzul.Matricula.Dados.ListaMensalidades()[indiceTab].grid.rows()[i].Situacao.toLowerCase() !== "quitada") {
                    valorDesconto = NotaAzul.Matricula.Dados.ListaMensalidades()[indiceTab].grid.rows()[i].Desconto;
                    NotaAzul.Matricula.Dados.ListaMensalidades()[indiceTab].grid.rows()[i].Desconto = (objGrupoDesconto.data.TipoDesconto.toLowerCase() === "valormonetario") ? valorDesconto + parseFloat(objGrupoDesconto.data.Valor)
                        : valorDesconto + parseFloat(valorMensalidade * objGrupoDesconto.data.Valor / 100);
                }
            }

            for (i = 0, len = $arrInputDesconto.length; i < len; i++) {
                valorDesconto = parseFloat($arrInputDesconto[i].value.substring(3).replace(/\,/g, '.'));
                //Apenas as mensalidades que não foram quitadas podem ser alteradas
                if (situacaoMensalidade[i].lastChild.lastChild.lastChild.innerText.toLowerCase() !== "quitada") {

                    //Verifica se o tipo de desconto é monetário,a fim de decidir qual cálculo será realizado
                    //Expressão regular responsável por trocar o ponto por vírgula                        
                    $arrInputDesconto[i].value = (objGrupoDesconto.data.TipoDesconto.toLowerCase() === "valormonetario") ? "R$ " + (valorDesconto + parseFloat(objGrupoDesconto.data.Valor)).toFixed(2).replace(/\./g, ',')
                        : "R$ " + (valorDesconto + parseFloat(valorMensalidade * objGrupoDesconto.data.Valor / 100)).toFixed(2).replace(/\./g, ',');
                }
            }

        };

        /**
        * @descrição: Carrega um popup com uma lista de alunos
        * @return: void
        **/
        var _buscarAluno = function () {
            NotaAzul.Aluno.Lista.Grid().show();
        };


        /**
        * @descrição: Carrega arquivos javascript
        **/
        var _carregarArquivos = function () {
            Prion.Loader.Carregar([
                {
                    url: rootDir + "Scripts/Views/Aluno/lista.js",
                    fn: function () {
                        var config = {
                            id: "listaAlunos",
                            popup: true,
                            width: "700px",
                            action: { onDblClick: NotaAzul.Matricula.Dados.ObterAluno }
                        };

                        NotaAzul.Aluno.Lista.Iniciar(config);
                    }
                }
            ]);
        };


        /**
        * @descrição: Carrega o conteúdo do html Matricula/Ficha em background, acelerando o futuro carregamento
        **/
        var _carregarFichaMatricula = function () {

            var height = (window.innerHeight < 900) ? (window.innerHeight - 60) : 900;

            _windowFicha = new Prion.Window({
                url: rootDir + "Matricula/ViewFicha",
                id: "popupFicha",
                height: height,
                title: { text: "Ficha de Matrícula" },
                buttons: {
                    buttons: [{
                        text: "Imprimir",
                        className: Prion.settings.ClassName.button,
                        click: function (win) {
                            NotaAzul.Matricula.Dados.ImprimirFichaDeMatricula();
                        }
                    }]
                }
            });
        };

        /**
        * @descrição: Carrega o conteúdo do html Matricula/Ficha em background, acelerando o futuro carregamento
        **/
        var _carregarCarneDeMensalidades = function () {

            var height = (window.innerHeight < 900) ? (window.innerHeight - 60) : 900;

            _windowCarneMensalidade = new Prion.Window({
                url: rootDir + "Matricula/ViewCarneMensalidade",
                id: "popupCarneMensalidade",
                height: height,
                title: { text: "Carnê de Mensalidades" },
                buttons: {
                    buttons: [{
                        text: "Imprimir",
                        className: Prion.settings.ClassName.button,
                        click: function (win) {
                            NotaAzul.Matricula.Dados.ImprimirCarneDeMensalidade();
                        }
                    }]
                }
            });
        };

        /**
        * @descrição: Carrega todos os cursos baseado no ano letivo
        * @return: void
        **/
        var _carregarCursoAnoLetivo = function () {
            var anoLetivo = "Matricula_CursoAnoLetivo_AnoLetivo".getValue();

            // validações
            if ((anoLetivo === "") || (anoLetivo.length < 4)) {
                Prion.Alert({ msg: "Informe o ano letivo." });
                return;
            }

            _limparCamposInformacoesCurso();

            Prion.ComboBox.Carregar({
                url: rootDir + "CursoAnoLetivo/GetLista",
                el: "matriculaListaCursoAnoLetivo",
                filter: "Entidades=Curso&Paginar=false&AnoLetivo=" + anoLetivo,
                clear: true,
                valueDefault: true,
                textJson: "Curso.Nome"
            });
        };


        /**
        * @descrição: Carrega todas as turmas baseado no ano letivo e no curso escolhido
        * @return: void
        **/
        var _carregarTurmaAnoLetivo = function (itemCurso) {
            Prion.ComboBox.Carregar({
                url: rootDir + "Turma/GetLista",
                el: "matriculaListaTurmaAnoLetivo",
                filter: "Entidades=CursoAnoLetivo&Paginar=false&CursoAnoLetivo.Id=" + itemCurso.Id,
                clear: true,
                valueDefault: true
            });
        };

        /**
        * @descrição: Cria abas de acordo com o(s) curso(s) escolhido(s)
        * @return: void
        **/
        var _criarAbasCurso = function (/*objMatriculaCurso*/) {

            var objMatriculaCurso = arguments[0],
                    objGrupoDesconto = "matriculaListaGrupoDesconto".getValue(),
                    turmaSelecionada,
                    idTurma,
                    idCurso,
                    idMatriculaCurso,
                    idTab,
                    idListaMensalidades,
                    nomeTab;

            if (objMatriculaCurso != null) {
                turmaSelecionada = objMatriculaCurso.Turma;
                idTurma = objMatriculaCurso.Turma.Id;
                idCurso = objMatriculaCurso.Turma.CursoAnoLetivo.Curso.Id;
                idMatriculaCurso = objMatriculaCurso.Id;
                idTab = "tab" + idCurso;
                idListaMensalidades = "listaMensalidade" + idCurso;
                nomeTab = objMatriculaCurso.Turma.CursoAnoLetivo.Curso.Nome;

            } else {
                // obtém o objeto de turma, caso a função não o receba como parâmetro
                var cursoSelecionado = "matriculaListaCursoAnoLetivo".getValue();

                turmaSelecionada = "matriculaListaTurmaAnoLetivo".getValue();
                idTurma = turmaSelecionada.data.Id;
                idCurso = cursoSelecionado.data["Curso.Id"];
                idMatriculaCurso = 0;
                idTab = "tab" + idCurso;
                idListaMensalidades = "listaMensalidade" + idCurso;
                nomeTab = cursoSelecionado.data["Curso.Nome"];
            }


            // Verifica se já existe uma tab com esse id
            if (idTab.getDom() != null) {
                window.alert("Você já selecionou esse curso");
                return;
            }

            // verifica se o objeto de abas já foi criado
            if (_tabs == null) {
                _tabs = new Prion.Tabs({
                    id: "tabsMensalidades",
                    selected: 0
                });
            }



            // cria uma aba para o curso selecionado
            _tabs.Create({
                id: idTab,
                content: "<div id='" + idListaMensalidades + "' class='list'></div>",
                title: nomeTab,
                style: "height:306px !important; width: 699px !important;",
                action: {
                    afterCreate: function () {
                        // após criar a aba, executa a function para criar a lista de mensalidades
                        if (objMatriculaCurso != null) {
                            NotaAzul.Matricula.Dados.CriarListaMensalidades(idListaMensalidades, idCurso, idTurma, idMatriculaCurso, objGrupoDesconto, objMatriculaCurso);
                        } else {
                            NotaAzul.Matricula.Dados.CriarListaMensalidades(idListaMensalidades, idCurso, idTurma, idMatriculaCurso, objGrupoDesconto.data);
                        }
                    }
                }
            });

        };


        /**
        * @descrição: Cria o grid de mensalidades
        * @return: void
        **/
        var _criarListaMensalidades = function (idLista, idCurso, idTurma, idMatriculaCurso, objGrupoDesconto/*,objCursoAnoLetivo*/) {
            var objCursoAnoLetivo = arguments[5],
                    flagEstadoMatricula = false;

            if (objCursoAnoLetivo == null) {
                objCursoAnoLetivo = Prion.ComboBox.Get("matriculaListaCursoAnoLetivo").data;
                flagEstadoMatricula = true;
            }

            _criarHtmlListaMensalidades(idLista, idCurso, idTurma, idMatriculaCurso);
            _gerarMensalidades(objCursoAnoLetivo, idLista, flagEstadoMatricula, objGrupoDesconto);
        };

        /**
        * @descrição: Cria o todo o html da lista de mensalidades
        * @return: void
        **/
        var _criarHtmlListaMensalidades = function (idLista, idCurso, idTurma, idMatriculaCurso) {
            if (_listaGridMensalidades == null) {
                _listaGridMensalidades = [];
            }


            // campo custom do tipo INPUT para valor da mensalidade
            var columnValorMensalidade = new Prion.Lista.CustomField.Input({
                type: "text",
                value: "", // value default do campo
                attributes: { maxlength: 10, style: "width:90px", mask: "money", mandatory: true, message: "mensagem", namejson: "Valor" }
            });

            // campo custom do tipo INPUT para valor da mensalidade
            var columnAcrescimo = new Prion.Lista.CustomField.Input({
                type: "text",
                value: "", // value default do campo
                attributes: { maxlength: 10, style: "width:90px", mask: "money", namejson: "Acrescimo" }
            });

            // campo custom do tipo INPUT para valor da mensalidade
            var columnDesconto = new Prion.Lista.CustomField.Input({
                type: "text",
                value: "", // value default do campo
                attributes: { maxlength: 10, style: "width:90px", mask: "money", namejson: "Desconto" }
            });

            // campo custom do tipo CHECKBOX para indicar se o aluno esta isento desta mensalidade
            var columnIsento = new Prion.Lista.CustomField.Input({
                type: "checkbox",
                value: false, // value default do campo
                attributes: { namejson: "Isento" }
            });



            var gridMensalidades = new Prion.Lista({
                id: idLista,
                autoLoad: false,
                width: 680,
                style: "margin:14px; position:relative;",
                paging: { show: false },
                rowNumber: { show: false },
                filter: { Entidades: "Situacao" },
                title: { text: "Mensalidades" },
                type: Prion.Lista.Type.Local,
                buttons: {
                    remove: { show: true }
                },
                columns: [
                        { header: "Mês", nameJson: "Mes", visible: false },
                        { header: "Estado do Objeto", nameJson: "EstadoObjeto", visible: false },
                        { header: "Mês", nameJson: "MesStr", width: "110px" },
                        { header: "Valor da Mensalidade", nameJson: "Valor", width: "130px", customField: columnValorMensalidade },
                        { header: "Desconto", nameJson: "Desconto", width: "100px", customField: columnDesconto },
                        { header: "Acréscimo", nameJson: "Acrescimo", width: "100px", customField: columnAcrescimo },
                        { header: "Isento", nameJson: "Isento", width: "40px", customField: columnIsento },
                        { header: "Situação", nameJson: "Situacao", width: "100px" }
                    ]
            });



            _listaGridMensalidades.push({
                id: idLista,
                idCurso: idCurso,
                idTurma: idTurma,
                idMatriculaCurso: idMatriculaCurso,
                grid: gridMensalidades
            });
        };


        /**
        * @descrição: Define ação para todos os controles da tela 
        **/
        var _definirAcaoBotoes = function () {
            // quando clicar no botão de buscar cursos...
            Prion.Event.add("btnBuscarCursos", "click", function () {
                NotaAzul.Matricula.Dados.CarregarCursoAnoLetivo();
            });

            // quando clicar no botão de buscar aluno...
            Prion.Event.add("btnBuscarAluno", "click", function () {
                NotaAzul.Matricula.Dados.BuscarAluno();
            });

            // quando escolher um novo item no combobox listaCursos
            Prion.Event.add("matriculaListaCursoAnoLetivo", "change", function () {
                NotaAzul.Matricula.Dados.ExibirDadosCursoAnoLetivo();
            });
        };



        /**
        * @descrição: Exibe os valores do combbox de Curso Ano Letivo      
        **/
        var _exibirDadosCursoAnoLetivo = function () {
            var itemCurso = Prion.ComboBox.Get("matriculaListaCursoAnoLetivo"),
                    anoLetivo = "Matricula_CursoAnoLetivo_AnoLetivo".getValue();

            _limparCamposInformacoesCurso();

            // validações
            if ((anoLetivo === "") || (anoLetivo.length < 4)) {
                Prion.Alert({ msg: "Informe o ano letivo." });
                return;
            }

            if (itemCurso.value === "") {
                Prion.Alert({ msg: "Selecione um curso" });
                return;
            }

            var cursoAnoletivo = itemCurso.data;
            "CursoAnoLetivo_ValorMatricula".setValue(cursoAnoletivo.ValorMatricula);
            "CursoAnoLetivo_ValorMensalidade".setValue(cursoAnoletivo.ValorMensalidade);
            "CursoAnoLetivo_QtdMensalidade".setValue(cursoAnoletivo.QuantidadeMensalidades);

            this.CarregarTurmaAnoLetivo(cursoAnoletivo);
        };


        /**
        * @descrição: Método que será chamado sempre após salvar 
        **/
        var _executarAposSalvar = function (retorno) {
            NotaAzul.Matricula.Dados.Novo();
            return false;
        };


        /**
        * @descrição: Gera os carnês de mensalidade
        * @params: objMatricula => objeto com os dados correspondentes a matrícula selecionada
        * @return: void
        **/
        var _gerarCarneDeMensalidades = function (objMatricula) {
            if (objMatricula == null) { return; }

            _limparCarneDeMatriculas();
            var diaVencimento = objMatricula.Aluno.DiaPagamento.toString();

            for (var i = 0, len = objMatricula.ListaMatriculaCurso.length; i < len; i++) {
                for (var k = 0, quantidadeMensalidades = objMatricula.ListaMatriculaCurso[i].Mensalidades.length; k < quantidadeMensalidades; k++) {
                    var $domCarneMensalidade = "carneMensalidade".getDom();

                    if ($domCarneMensalidade == null) { return; }
                    var indice = $domCarneMensalidade.children.length;

                    if (indice == 1) {
                        "Capa_ValorMensalidadeDesconto".setValue(objMatricula.ListaMatriculaCurso[i].Mensalidades[k].Valor - objMatricula.ListaMatriculaCurso[i].Mensalidades[k].Desconto);
                        "Capa_ValorMensalidadeAcrescimo".setValue(objMatricula.ListaMatriculaCurso[i].Mensalidades[k].Valor + objMatricula.ListaMatriculaCurso[i].Mensalidades[k].Acrescimo);
                        "Capa_DiaVencimento1".setValue(diaVencimento);
                        "Capa_DiaVencimento2".setValue(diaVencimento);
                        "NomeAlunoA".setValue(objMatricula.Aluno.Nome);
                        "NomeAlunoB".setValue(objMatricula.Aluno.Nome);
                        "TurnoAlunoA".setValue(objMatricula.ListaMatriculaCurso[i].Turma.Turno.Nome);
                        "TurnoAlunoB".setValue(objMatricula.ListaMatriculaCurso[i].Turma.Turno.Nome);
                        "AnoEscolarAlunoA".setValue(objMatricula.ListaMatriculaCurso[i].Turma.CursoAnoLetivo.Curso.Nome);
                        "AnoEscolarAlunoB".setValue(objMatricula.ListaMatriculaCurso[i].Turma.CursoAnoLetivo.Curso.Nome);
                        "MensalidadeAlunoA".setValue(objMatricula.ListaMatriculaCurso[i].Mensalidades[k].Valor - objMatricula.ListaMatriculaCurso[i].Mensalidades[k].Desconto);
                        "MensalidadeAlunoB".setValue(objMatricula.ListaMatriculaCurso[i].Mensalidades[k].Valor - objMatricula.ListaMatriculaCurso[i].Mensalidades[k].Desconto);
                        "MensalidadeMultaA".setValue(objMatricula.ListaMatriculaCurso[i].Mensalidades[k].Valor + objMatricula.ListaMatriculaCurso[i].Mensalidades[k].Acrescimo);
                        "MensalidadeMultaB".setValue(objMatricula.ListaMatriculaCurso[i].Mensalidades[k].Valor + objMatricula.ListaMatriculaCurso[i].Mensalidades[k].Acrescimo);
                        "VencimentoMensalidadeA".setValue(Prion.Format(objMatricula.ListaMatriculaCurso[i].Mensalidades[k].DataVencimento, "dd/mm/aaaa"));
                        "VencimentoMensalidadeB".setValue(Prion.Format(objMatricula.ListaMatriculaCurso[i].Mensalidades[k].DataVencimento, "dd/mm/aaaa"));
                        k++;
                    }


                    var $divFolhaCarneClonado = "Folhas".getDom().cloneNode(true);
                    $divFolhaCarneClonado.id = "Folhas" + indice;

                    var $quebraDePagina = document.createElement("DIV");
                    $quebraDePagina.setAttribute("class", "QuebraDeImpressao");

                    var $arrLabel = $divFolhaCarneClonado.getElementsByClassName("Campo");

                    for (var j = 0, len2 = $arrLabel.length - 1; j < len2; j++) {
                        $arrLabel[j].id += indice;
                    }

                    $domCarneMensalidade.appendChild($divFolhaCarneClonado);
                    if ($domCarneMensalidade.children.length % TOTAL_MENSALIDADES_POR_PAGINA === 0) {
                        $domCarneMensalidade.appendChild($quebraDePagina);
                    }

                    ("NomeAlunoA" + indice).setValue(objMatricula.Aluno.Nome);
                    ("NomeAlunoB" + indice).setValue(objMatricula.Aluno.Nome);
                    ("TurnoAlunoA" + indice).setValue(objMatricula.ListaMatriculaCurso[i].Turma.Turno.Nome);
                    ("TurnoAlunoB" + indice).setValue(objMatricula.ListaMatriculaCurso[i].Turma.Turno.Nome);
                    ("AnoEscolarAlunoA" + indice).setValue(objMatricula.ListaMatriculaCurso[i].Turma.CursoAnoLetivo.Curso.Nome);
                    ("AnoEscolarAlunoB" + indice).setValue(objMatricula.ListaMatriculaCurso[i].Turma.CursoAnoLetivo.Curso.Nome);
                    ("MensalidadeAlunoA" + indice).setValue(objMatricula.ListaMatriculaCurso[i].Mensalidades[k].Valor - objMatricula.ListaMatriculaCurso[i].Mensalidades[k].Desconto);
                    ("MensalidadeAlunoB" + indice).setValue(objMatricula.ListaMatriculaCurso[i].Mensalidades[k].Valor - objMatricula.ListaMatriculaCurso[i].Mensalidades[k].Desconto);
                    ("MensalidadeMultaA" + indice).setValue(objMatricula.ListaMatriculaCurso[i].Mensalidades[k].Valor + objMatricula.ListaMatriculaCurso[i].Mensalidades[k].Acrescimo);
                    ("MensalidadeMultaB" + indice).setValue(objMatricula.ListaMatriculaCurso[i].Mensalidades[k].Valor + objMatricula.ListaMatriculaCurso[i].Mensalidades[k].Acrescimo);
                    ("VencimentoMensalidadeA" + indice).setValue(Prion.Format(objMatricula.ListaMatriculaCurso[i].Mensalidades[k].DataVencimento, "dd/mm/aaaa"));
                    ("VencimentoMensalidadeB" + indice).setValue(Prion.Format(objMatricula.ListaMatriculaCurso[i].Mensalidades[k].DataVencimento, "dd/mm/aaaa"));
                }
            }

            NotaAzul.Matricula.Dados.ImprimirCarneDeMensalidade();
        };

        /**
        * @descrição: Gera os carnês de mensalidade,somando o valor da mensalidade de todos os cursos 
        * @params: objMatricula => objeto com os dados correspondentes a matrícula selecionada
        * @return: void
        **/
        var _gerarCarneDeMensalidadesAgregado = function (objMatricula) {
            if (objMatricula == null) {
                return;
            }

            _limparCarneDeMatriculas();

            var objPrincipal = objMatricula.ListaMatriculaCurso[0],
                    valorMensalidadeDesconto = 0,
                    valorMensalidadeAcrescimo = 0,
                    quebraDePagina = document.createElement("DIV"),
                    i,
                    j,
                    len,
                    quantidadeCursos = objMatricula.ListaMatriculaCurso.length,
                    diaVencimento = objMatricula.Aluno.DiaPagamento.toString(),
                    numeroMensalidades = objPrincipal.Mensalidades.length;

            quebraDePagina.setAttribute("class", "QuebraDeImpressao");
            quebraDePagina.style.pageBreakBefore = "always";

            for (i = 1; i < quantidadeCursos; i++) {
                if (objPrincipal.Mensalidades.length < objMatricula.ListaMatriculaCurso[i].Mensalidades.length) {
                    objPrincipal = objMatricula.ListaMatriculaCurso[i];
                }
            }

            for (i = 0; i < numeroMensalidades; i++) {
                valorMensalidadeDesconto = 0;
                valorMensalidadeAcrescimo = 0;

                var nomeTurnos = "",
                        nomeCursos = "",
                        $domCarneMensalidade = "carneMensalidade".getDom();

                if ($domCarneMensalidade == null) {
                    return;
                }

                var indice = $domCarneMensalidade.children.length;

                for (j = 0; j < quantidadeCursos; j++) {
                    valorMensalidadeDesconto += objMatricula.ListaMatriculaCurso[j].Mensalidades[i].Valor - objMatricula.ListaMatriculaCurso[j].Mensalidades[i].Desconto;
                    valorMensalidadeAcrescimo += objMatricula.ListaMatriculaCurso[j].Mensalidades[i].Valor + objMatricula.ListaMatriculaCurso[j].Mensalidades[i].Acrescimo;
                    nomeCursos += objMatricula.ListaMatriculaCurso[j].Turma.CursoAnoLetivo.Curso.Nome;
                    nomeTurnos += objMatricula.ListaMatriculaCurso[j].Turma.Turno.Nome;
                    if (j != quantidadeCursos - 1) {
                        nomeCursos += ",";
                        nomeTurnos += ",";
                    }
                }

                if (indice == 1) {
                    "Capa_ValorMensalidadeDesconto".setValue(valorMensalidadeDesconto);
                    "Capa_ValorMensalidadeAcrescimo".setValue(valorMensalidadeAcrescimo);

                    "Capa_DiaVencimento1".setValue(diaVencimento);
                    "Capa_DiaVencimento2".setValue(diaVencimento);
                    "NomeAlunoA".setValue(objMatricula.Aluno.Nome);
                    "NomeAlunoB".setValue(objMatricula.Aluno.Nome);
                    "TurnoAlunoA".setValue(nomeTurnos);
                    "TurnoAlunoB".setValue(nomeTurnos);
                    "AnoEscolarAlunoA".setValue(nomeCursos);
                    "AnoEscolarAlunoB".setValue(nomeCursos);
                    "MensalidadeAlunoA".setValue(valorMensalidadeDesconto);
                    "MensalidadeAlunoB".setValue(valorMensalidadeDesconto);
                    "MensalidadeMultaA".setValue(valorMensalidadeAcrescimo);
                    "MensalidadeMultaB".setValue(valorMensalidadeAcrescimo);
                    "VencimentoMensalidadeA".setValue(Prion.Format(objPrincipal.Mensalidades[i].DataVencimento, "dd/mm/aaaa"));
                    "VencimentoMensalidadeB".setValue(Prion.Format(objPrincipal.Mensalidades[i].DataVencimento, "dd/mm/aaaa"));
                    i++;
                    valorMensalidadeDesconto = 0;
                    valorMensalidadeAcrescimo = 0;
                    nomeTurnos = "";
                    nomeCursos = "";

                    for (j = 0; j < quantidadeCursos; j++) {
                        valorMensalidadeDesconto += objMatricula.ListaMatriculaCurso[j].Mensalidades[i].Valor - objMatricula.ListaMatriculaCurso[j].Mensalidades[i].Desconto;
                        valorMensalidadeAcrescimo += objMatricula.ListaMatriculaCurso[j].Mensalidades[i].Valor + objMatricula.ListaMatriculaCurso[j].Mensalidades[i].Acrescimo;
                        nomeCursos += objMatricula.ListaMatriculaCurso[j].Turma.CursoAnoLetivo.Curso.Nome;
                        nomeTurnos += objMatricula.ListaMatriculaCurso[j].Turma.Turno.Nome;

                        if (j != quantidadeCursos - 1) {
                            nomeCursos += ",";
                            nomeTurnos += ",";
                        }
                    }
                }

                var $divFolhaCarneClonado = "Folhas".getDom().cloneNode(true);
                $divFolhaCarneClonado.id = "Folhas" + indice;

                var arrLabel = $divFolhaCarneClonado.getElementsByClassName("Campo");
                for (j = 0, len = arrLabel.length - 1; j < len; j++) {
                    arrLabel[j].id += indice;
                }

                if ($domCarneMensalidade.children.length % TOTAL_MENSALIDADES_POR_PAGINA === 0) { $domCarneMensalidade.appendChild(quebraDePagina); }
                $domCarneMensalidade.appendChild($divFolhaCarneClonado);
                ("NomeAlunoA" + indice).setValue(objMatricula.Aluno.Nome);
                ("NomeAlunoB" + indice).setValue(objMatricula.Aluno.Nome);
                ("TurnoAlunoA" + indice).setValue(nomeTurnos);
                ("TurnoAlunoB" + indice).setValue(nomeTurnos);
                ("AnoEscolarAlunoA" + indice).setValue(nomeCursos);
                ("AnoEscolarAlunoB" + indice).setValue(nomeCursos);
                ("MensalidadeAlunoA" + indice).setValue(valorMensalidadeDesconto);
                ("MensalidadeAlunoB" + indice).setValue(valorMensalidadeDesconto);
                ("MensalidadeMultaA" + indice).setValue(valorMensalidadeAcrescimo);
                ("MensalidadeMultaB" + indice).setValue(valorMensalidadeAcrescimo);
                ("VencimentoMensalidadeA" + indice).setValue(Prion.Format(objPrincipal.Mensalidades[i].DataVencimento, "dd/mm/aaaa"));
                ("VencimentoMensalidadeB" + indice).setValue(Prion.Format(objPrincipal.Mensalidades[i].DataVencimento, "dd/mm/aaaa"));

            }
            NotaAzul.Matricula.Dados.ImprimirCarneDeMensalidade();
        };

        /**
        * @descrição: Gerar a lista das mensalidades baseado na quantidade de mensalidades de um curso
        **/
        var _gerarMensalidades = function (itemCurso, idLista, ehMatriculaNova, objGrupoDesconto) {

            if (_listaGridMensalidades == null) {
                // FAZER: exibir mensagem para debug
                return;
            }

            // verifica se o objeto itemCurso foi informado
            if (itemCurso == null) {
                // exibir um alert para o usuário
                return;
            }


            /*  // verifica se existe alguma mensalidade
            if (itemCurso.Mensalidades.length == 0) {
            return;
            }*/
            var bolsa = parseInt(Prion.RadioButton.GetValueSelected("BolsistaValor"),10),
                isento = (bolsa == 100) ? true : false,
                qtdMensalidades = (ehMatriculaNova) ? itemCurso.QuantidadeMensalidades
                : itemCurso.Mensalidades.length,
                valorMensalidade = (ehMatriculaNova) ? itemCurso.ValorMensalidade : itemCurso.Turma.CursoAnoLetivo.ValorMensalidade,
                valorDesconto = (ehMatriculaNova && objGrupoDesconto != null) ? (
                (objGrupoDesconto.TipoDesconto.toLowerCase() == "percentual") ? (valorMensalidade * objGrupoDesconto.Valor / 100)
                    : objGrupoDesconto.Valor)
                : 0;

            valorDesconto = (ehMatriculaNova && bolsa == 50) ? parseFloat(valorDesconto) + ((valorMensalidade * bolsa) / 100) : valorDesconto;

            var indiceGridMensalidades = _listaGridMensalidades.length - 1,
                gridMensalidades = _listaGridMensalidades[indiceGridMensalidades],
                rowsMensalidades = [];

            for (var i = 0; i < qtdMensalidades; i++) {
                var nomeMes = Prion.Date.GetNomeMes(i);
                gridMensalidades.EstadoObjeto = (ehMatriculaNova) ? EstadoObjeto.Novo : EstadoObjeto.Alterado;

                rowsMensalidades.push({
                    Id: (ehMatriculaNova) ? 0 : itemCurso.Mensalidades[i].Id,
                    MesStr: nomeMes,
                    Mes: (i + 1),
                    EstadoObjeto: (ehMatriculaNova) ? EstadoObjeto.Novo : itemCurso.Mensalidades[i].EstadoObjeto,
                    Valor: valorMensalidade,
                    Acrescimo: (ehMatriculaNova) ? 0 : itemCurso.Mensalidades[i].Acrescimo,
                    Desconto: (ehMatriculaNova) ? valorDesconto : itemCurso.Mensalidades[i].Desconto + valorDesconto,
                    Isento: (ehMatriculaNova) ? isento : itemCurso.Mensalidades[i].Isento,
                    Situacao: (ehMatriculaNova) ? "Aberta" : itemCurso.Mensalidades[i].Situacao.Nome
                });
            }


            // adiciona as linhas ao grid
            gridMensalidades.grid.addRow(rowsMensalidades);

            // atualiza os dados
            _listaGridMensalidades[indiceGridMensalidades] = gridMensalidades;

        };


        /**
        * @descrição: Método que rtorna a lista de grids de Mensalidades
        **/
        var _getListaMensalidades = function () {
            return _listaGridMensalidades;
        };

        /**
        * @descrição: Método utilizado para imprimir apenas os carnês de Mensalidade
        **/
        var _imprimirCarneDeMensalidade = function () {
            var printContents = document.getElementById("CarneCompleto").innerHTML,
                    mywindow = window.open("", "CarneMensalidade", ""),
                    css = "<link rel='stylesheet' type='text/css' href='Content/notaAzul.css' media='print' />",
                    html = "<html><head><title>Carnê de Mensalidades</title>" + css + "</head><body style='width:1000px;padding: 10px;margin: 0% 0% 0% 10%;'>" +
                            printContents +
                            "</body></html>";

            mywindow.document.write(html);
            mywindow.print();
            mywindow.close();
        };


        /**
        * @descrição: Método utilizado para imprimir apenas a Ficha de Matrícula 
        **/
        var _imprimirFichaDeMatricula = function () {
            var printContents = document.getElementById("FichaDeMatricula").innerHTML,
                    mywindow = window.open("", "FichaDeMatricula", ""),
                    css = "<link rel='stylesheet' type='text/css' href='Content/notaAzul.css' media='print' />",
                    html = "<html><head><title>Ficha de Matrícula</title>" + css + "</head><body style='width:740px;border: 1px solid black;padding: 10px'>" + printContents + "</body></html>";

            mywindow.document.write(html);
            mywindow.print();
            mywindow.close();
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            _definirAcaoBotoes();
            //_criarHtmlListaMensalidades();
            //_carregarArquivos();
            _carregarFichaMatricula();
            _carregarCarneDeMensalidades();
            _novaMatricula();
            NotaAzul.Matricula.Dados.AutoCompleteBuscarAluno();
        };

        /**
        * @descrição: Limpa todos os campos de curso antes de buscar as turmas
        **/
        var _limparCamposInformacoesCurso = function () {
            "CursoAnoLetivo_ValorMatricula".setValue(0);
            "CursoAnoLetivo_ValorMensalidade".setValue(0);
            "CursoAnoLetivo_QtdMensalidade".setValue(0);

            // limpa o combobox das turmas
            "matriculaListaTurmaAnoLetivo".setValue("");

            // desabilita o botão relacionado ao combobox das turmas

            //_gridMensalidades.clean();
        };

        /**
        * @descrição: remove todas as divs do Carne de Mensalidades
        **/
        var _limparCarneDeMatriculas = function () {
            var $domElememt = document.getElementById("carneMensalidade");

            while ($domElememt.children.length != 1) {
                $domElememt.removeChild($domElememt.lastChild);
            }

        };

        /**
        * @descrição: Limpa todos os valores da Ficha de Matricula
        **/
        var _limparFichaDeMatricula = function () {
            var arrLabel = Array.fromList(document.querySelectorAll("#conteudoFicha .Campo"));

            for (var i = 0; i < arrLabel.length - 1; i++) {
                arrLabel[i].innerText = "";
            }
        };

        /**
        * @descrição: Limpa as abas de Mensalidade e todo o seu conteúdo
        **/
        var _limparTabsMensalidade = function () {
            // elimina as linhas da lista
            var $domElememt = document.getElementById("ulTabs");

            while ($domElememt.hasChildNodes()) {
                $domElememt.removeChild($domElememt.lastChild);
            }

            // elimina as div correspondentes
            var element = document.getElementById("tabsMensalidades").getElementsByTagName("div");

            for (var i = element.length - 1; i >= 0; i--) {
                element[i].parentNode.removeChild(element[i]);
            }
        };

        /**
        * @descrição: Reseta o form, voltando os dados default
        **/
        var _novaMatricula = function () {
            Prion.ClearForm("frmMatricula", true);

            // limpa o array de mensalidades
            _listaGridMensalidades = [];

            // limpa todas as informações relacionadas ao curso que estão definidas em labels
            _limparCamposInformacoesCurso();
            _limparTabsMensalidade();

            // define valores default para esses campos
            "Matricula_DataCadastro".setValue(Prion.Date.DataAtual());
            "Matricula_CursoAnoLetivo_AnoLetivo".setValue(Prion.Date.AnoAtual());
            Prion.RadioButton.SelectByValue("BolsistaValor", 0);

            _limparFichaDeMatricula();
            NotaAzul.Matricula.Dados.CarregarCursoAnoLetivo();
        };

        /**
        * @descrição: Obtém o objeto de aluno selecionado na lista
        * @params: objAluno => objeto que representa o registro clicado
        * @return: void
        **/
        var _obterAluno = function (objAluno) {
            console.log(objAluno);
        };

        /**
        * @descrição: Preenche a ficha de matrícula
        * @params: objMatricula => objeto com os dados correspondentes a matrícula selecionada
        * @return: void
        **/
        var _preencherFichaDeMatricula = function (objMatricula) {
            if (objMatricula == null) { return; }

            var i = 0;

            "Matricula_Numero".setValue(objMatricula.NumeroMatricula);
            "Matricula_CadastroData".setValue(Prion.Format(objMatricula.DataCadastro, "dd/mm/aaaa"));

            //Referente as informações de Aluno
            "Aluno_Nome".setValue(objMatricula.Aluno.Nome);
            "Aluno_DataNascimento".setValue(Prion.Format(objMatricula.Aluno.DataNascimento, "dd/mm/aaaa"));
            "Aluno_GrupoSanguineo".setValue(objMatricula.Aluno.GrupoSanguineo);
            "Aluno_Religiao".setValue(objMatricula.Aluno.Religiao);
            "Aluno_RegistroNascimento".setValue(objMatricula.Aluno.RegistroDeNascimento);
            "Aluno_EstadoRegistroNascimento".setValue(objMatricula.Aluno.EstadoRegistroNascimento.UF);
            "Aluno_CidadeRegistroNascimento".setValue(objMatricula.Aluno.CidadeRegistroNascimento.Nome);
            "Aluno_Nacionalidade".setValue(objMatricula.Aluno.Nacionalidade);
            "Aluno_NumeroOrdem".setValue(objMatricula.Aluno.NumeroOrdem);
            "Aluno_Fls".setValue(objMatricula.Aluno.Folhas);
            "Aluno_LivroNumero".setValue(objMatricula.Aluno.NumeroLivro);
            "Aluno_DataRegistroNascimento".setValue(Prion.Format(objMatricula.Aluno.DataRegistroNascimento, "dd/mm/aaaa"));

            if (objMatricula.Aluno.ObservacaoSaude !== null && objMatricula.Aluno.ObservacaoSaude !== "") {
                "saude_Flag".setValue("Sim");
                "Aluno_observacaoSaude".setValue(objMatricula.Aluno.ObservacaoSaude);
            } else {
                "saude_Flag".setValue("Não");
            }

            if (objMatricula.Aluno.ObservacaoMedicacao !== null && objMatricula.Aluno.ObservacaoMedicacao !== "") {
                "medicacao_Flag".setValue("Sim");
                "Aluno_observacaoMedicacao".setValue(objMatricula.Aluno.ObservacaoMedicacao);
            } else {
                "medicacao_Flag".setValue("Não");
            }

            //Referente as informações dos responsáveis
            if (objMatricula.Aluno.Responsaveis != null) {
                var quantidadeResponsaveis = objMatricula.Aluno.Responsaveis.length;

                for (i = 0; i < quantidadeResponsaveis; i++) {
                    if (objMatricula.Aluno.Responsaveis[i].GrauParentesco.toLowerCase() == "pai") {

                        "Pai_Nome".setValue(objMatricula.Aluno.Responsaveis[i].Nome);
                        "Pai_Cpf".setValue(objMatricula.Aluno.Responsaveis[i].CPF);
                        "Pai_Rg".setValue(objMatricula.Aluno.Responsaveis[i].RG);
                        "Pai_Profissao".setValue(objMatricula.Aluno.Responsaveis[i].Profissao);

                    } else if (objMatricula.Aluno.Responsaveis[i].GrauParentesco.toLowerCase() == "mãe") {

                        "Mae_Nome".setValue(objMatricula.Aluno.Responsaveis[i].Nome);
                        "Mae_Cpf".setValue(objMatricula.Aluno.Responsaveis[i].CPF);
                        "Mae_Rg".setValue(objMatricula.Aluno.Responsaveis[i].RG);
                        "Mae_Profissao".setValue(objMatricula.Aluno.Responsaveis[i].Profissao);

                    }
                    if (objMatricula.Aluno.Responsaveis[i].MoraCom === true) {

                        "Responsavel_Endereco".setValue(objMatricula.Aluno.Responsaveis[i].Endereco.DadosEndereco);
                        "Responsavel_Bairro".setValue(objMatricula.Aluno.Responsaveis[i].Endereco.Bairro.Nome);
                        "Responsavel_Cidade".setValue(objMatricula.Aluno.Responsaveis[i].Endereco.Cidade.Nome);
                        "Responsavel_Estado".setValue(objMatricula.Aluno.Responsaveis[i].Endereco.Estado.UF);
                        "Responsavel_Cep".setValue(objMatricula.Aluno.Responsaveis[i].Endereco.Cep);

                        var telefone = '';

                        for (var j = 0, quantidadeTelefones = objMatricula.Aluno.Responsaveis[i].Telefones.length - 1; j <= quantidadeTelefones; j++) {
                            telefone += objMatricula.Aluno.Responsaveis[i].Telefones[j].Numero;

                            if (j != objMatricula.Aluno.Responsaveis[i].Telefones.length - 1) {
                                telefone += " ,";
                            }
                        }

                        "Responsavel_Telefone".setValue(telefone);

                    }
                }
            }

            // dados referente as informações de Turma
            var quantidadeMatriculas = objMatricula.ListaMatriculaCurso.length;
            for (i = 0; i < quantidadeMatriculas; i++) {
                ("anoEscolar_Matricula" + i).setValue(objMatricula.ListaMatriculaCurso[i].Turma.CursoAnoLetivo.Curso.Nome);
                ("anoLetivo_Matricula" + i).setValue(objMatricula.ListaMatriculaCurso[i].Turma.CursoAnoLetivo.AnoLetivo);
                ("turno_Matricula" + i).setValue(objMatricula.ListaMatriculaCurso[i].Turma.Turno.Nome);
            }
        };

        /**
        * @descrição: Remove o desconto das mensalidades ainda não quitadas
        * @return: void
        **/
        var _removerDesconto = function () {
            var idLista = document.getElementsByClassName("div selected")[0].firstChild.id,
                    arrInputDesconto = document.querySelectorAll("#" + idLista + "Body [namejson='Desconto']"),
                    situacaoMensalidade = document.querySelectorAll("#" + idLista + "Body tr"),
                    idCursoAbaSelecionada = document.getElementsByClassName("selected")[0].id.replace("tab", ""),
                    i, len, indiceTab;

            for (i = 0, len = NotaAzul.Matricula.Dados.ListaMensalidades().length; i < len; i++) {
                if (NotaAzul.Matricula.Dados.ListaMensalidades()[i].idCurso === idCursoAbaSelecionada) {
                    indiceTab = i;
                    break;
                }
            }

            for (i = 0, len = NotaAzul.Matricula.Dados.ListaMensalidades()[indiceTab].grid.rows().length; i < len; i++) {
                if (NotaAzul.Matricula.Dados.ListaMensalidades()[indiceTab].grid.rows()[i].Situacao.toLowerCase() !== "quitada") {
                    NotaAzul.Matricula.Dados.ListaMensalidades()[indiceTab].grid.rows()[i].Desconto = 0;
                }
            }

            for (i = 0; i < arrInputDesconto.length; i++) {
                //Apenas as mensalidades que não foram quitadas podem ser alteradas
                if (situacaoMensalidade[i].lastChild.lastChild.lastChild.innerText.toLowerCase() != "quitada") {
                    //Verifica se o tipo de desconto é monetário,a fim de decidir qual cálculo será realizado
                    arrInputDesconto[i].value = "R$ 0,00";
                }
            }

        };

        /**
        * @descrição: Salva todos os dados do form
        **/
        var _salvar = function (win) {
            if (!Prion.ValidateForm("frmMatricula")) {
                win.config.reloadObserver = false;
                return false;
            }

            if (win != null) { win.mask(); }

            // alterar o Json de acordo com o modelo da Controller
            var nomeAluno = "Matricula_IdAluno".getValue();
            var anoLetivo = "Matricula_CursoAnoLetivo_AnoLetivo".getValue();
            var idMatricula = ("Matricula_Id".getValue() === "") ? 0 : "Matricula_Id".getValue();
            var jsonCursos = '{"Id":"' + idMatricula + '","IdAluno":"' + nomeAluno + '","AnoLetivo":"' + anoLetivo + '", "MatriculaCurso":[';

            for (var i = 0; i < _listaGridMensalidades.length; i++) {
                jsonCursos += '{"Id":"' + _listaGridMensalidades[i].idMatriculaCurso + '","EstadoObjeto":"' + _listaGridMensalidades[i].EstadoObjeto + '","IdTurma":"' + _listaGridMensalidades[i].idTurma + '","Mensalidades":' + _listaGridMensalidades[i].grid.serialize() + '}';

                if (i < (_listaGridMensalidades.length - 1)) {
                    jsonCursos += ",";
                }
            }
            jsonCursos += "]}";

            Prion.Request({
                form: "frmMatricula",
                url: rootDir + "Matricula/Salvar",
                data: "MatriculaCurso=" + jsonCursos,
                success: function (retorno, registroNovo) {
                    // se for um registro existente, fecha a janela
                    if ((!registroNovo) && (win != null)) {
                        win.config.reloadObserver = true;
                        win.hide({ animate: true });
                        return;
                    }

                    NotaAzul.Matricula.Dados.ExecutarAposSalvar();
                    if (win != null) {
                        win.config.reloadObserver = true;
                        win.mask();
                    }
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

        /**
        * @descrição: Salva todos os dados do form
        **/
        var _gerarBoleto = function (win) {
            if (!Prion.ValidateForm("frmMatricula")) {
                win.config.reloadObserver = false;
                return false;
            }

            if (win != null) { win.mask(); }

            // alterar o Json de acordo com o modelo da Controller
            var nomeAluno = "Matricula_IdAluno".getValue();
            var anoLetivo = "Matricula_CursoAnoLetivo_AnoLetivo".getValue();
            var idMatricula = ("Matricula_Id".getValue() === "") ? 0 : "Matricula_Id".getValue();
            var jsonCursos = '{"Id":"' + idMatricula + '","IdAluno":"' + nomeAluno + '","AnoLetivo":"' + anoLetivo + '", "MatriculaCurso":[';

            for (var i = 0; i < _listaGridMensalidades.length; i++) {
                jsonCursos += '{"Id":"' + _listaGridMensalidades[i].idMatriculaCurso + '","EstadoObjeto":"' + _listaGridMensalidades[i].EstadoObjeto + '","IdTurma":"' + _listaGridMensalidades[i].idTurma + '","Mensalidades":' + _listaGridMensalidades[i].grid.serialize() + '}';

                if (i < (_listaGridMensalidades.length - 1)) {
                    jsonCursos += ",";
                }
            }
            jsonCursos += "]}";

            Prion.Request({
                form: "frmMatricula",
                url: rootDir + "Matricula/GerarBoletos",
                data: "MatriculaCurso=" + jsonCursos,
                success: function (retorno, registroNovo) {
                    // se for um registro existente, fecha a janela
                    if ((!registroNovo) && (win != null)) {
                        win.config.reloadObserver = true;
                        win.hide({ animate: true });
                        return;
                    }

                    NotaAzul.Matricula.Dados.ExecutarAposSalvar();
                    if (win != null) {
                        win.config.reloadObserver = true;
                        win.mask();
                    }
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

        /**
        * @descrição: Salva todos os dados do form
        **/
        var _imprimirBoletos = function (win) {
            if (!Prion.ValidateForm("frmMatricula")) {
                win.config.reloadObserver = false;
                return false;
            }

            if (win != null) { win.mask(); }

            // alterar o Json de acordo com o modelo da Controller
            var aluno = "Matricula_IdAluno".getValue();
            

            Prion.Request({
                url: rootDir + "Boleto/ImprimirBoletoPorAluno",
                data: "IdAluno=" + aluno,
                success: function (retorno) {
                    if (retorno.success) {
                        var arr = retorno.enderecos;
                        for (var i = 0, len = arr.length; i < len; i++) {
                            var w = window.open(arr[i], "W" + i);
                            w.print();
                        }
                    }

                    win.hide({ animate: true });    
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

        /**********************************************************************************************
        ** PUBLIC
        **********************************************************************************************/
        return {
            AbrirFichaMatricula: _abrirFichaMatricula,
            AbrirCarneDeMensalidade: _abrirCarneDeMensalidade,
            AbrirCarneDeMensalidadeAgregado: _abrirCarneDeMensalidadeAgregado,
            AutoCompleteBuscarAluno: _autoCompleteBuscarAluno,
            AtualizarMensalidade: _atualizarMensalidade,
            BuscarAluno: _buscarAluno,
            CarregarCursoAnoLetivo: _carregarCursoAnoLetivo,
            CarregarTurmaAnoLetivo: _carregarTurmaAnoLetivo,
            CriarAbasCurso: _criarAbasCurso,
            CriarListaMensalidades: _criarListaMensalidades,
            ExibirDadosCursoAnoLetivo: _exibirDadosCursoAnoLetivo,
            ExecutarAposSalvar: _executarAposSalvar,
            GerarCarneDeMensalidades: _gerarCarneDeMensalidades,
            GerarCarneDeMensalidadesAgregado: _gerarCarneDeMensalidadesAgregado,
            GerarBoleto: _gerarBoleto,
            ImprimirCarneDeMensalidade: _imprimirCarneDeMensalidade,
            ImprimirFichaDeMatricula: _imprimirFichaDeMatricula,
            ImprimirBoletos: _imprimirBoletos,
            Iniciar: _iniciar,
            ListaMensalidades: _getListaMensalidades,
            Novo: _novaMatricula,
            ObterAluno: _obterAluno,
            PreencherFichaDeMatricula: _preencherFichaDeMatricula,
            RemoverDesconto: _removerDesconto,
            Salvar: _salvar            
        };

    } ();

    NotaAzul.Matricula.Dados.Iniciar();
} (window, document));
