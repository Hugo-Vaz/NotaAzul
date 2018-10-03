/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";

    if (NotaAzul.ContasReceber == null) {
        NotaAzul.ContasReceber = {};
    }

    NotaAzul.ContasReceber.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowContasReceber = null,
            _winMensalidadesPagas = null,
            _permissoes = null;

        /**
        * @descricao: Carrega o conteúdo do html ContasReceber/ViewPagamento em background, acelerando o futuro carregamento
        **/
        var _carregarJanelaContasReceber = function () {
            _windowContasReceber = new Prion.Window({
                url: rootDir + "ContasReceber/ViewPagamento/",
                id: "popupPagarMensalidade",
                height: 470,
                width: 760,
                title: { text: "Mensalidades" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.ContasReceber.Lista.Grid().load();
                    }
                },
                buttons: {
                    show: (!_permissoes.onlyView), // se onlyView for false, significa então que os botões deverão ser exibidos
                    buttons: [
                        {
                            text: "Salvar",
                            className: Prion.settings.ClassName.button,
                            typeButton: "save", // usado para controle interno
                            click: function (win) {
                                NotaAzul.ContasReceber.Pagamento.Salvar(win);
                            }
                        },
                        {
                            text: "Limpar", className: Prion.settings.ClassName.buttonReset, type: "reset",
                            click: function (win) {
                                NotaAzul.ContasReceber.Pagamento.Novo();
                            }
                        }
                    ]
                }
            });
        };

        /**
        * @descrição: Carrega o conteúdo do html ContasPagar/ViewMensalidadesPagas em background, acelerando o futuro carregamento
        **/
        var _carregarJanelaMensalidadesPagas = function () {
            _winMensalidadesPagas = new Prion.Window({
                url: rootDir + "ContasReceber/ViewMensalidadesPagas/",
                id: "popupMensalidadePaga",
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.ContasReceber.Lista.Grid().load();
                    }
                },
                height: 670,
                width: 750,
                title: { text: "Mensalidades Pagas" }
            });
        };

        /**
        * @descrição: Cria todo o Html da lista
        * @return: void
        **/
        var _criarLista = function () {
            if (_grid != null) { return; }

            _grid = new Prion.Lista({
                id: "listaContasReceber",
                url: rootDir + "ContasReceber/GetLista/",
                title: { text: "Contas à Receber" },
                request: { base: "Mensalidade" }, // colocado pois o SQL não está com ALIAS para os campos da tabela ContasReceber
                buttons: {
                    update: { show: _permissoes.insert, click: NotaAzul.ContasReceber.Lista.Abrir, tooltip: "Pagar mensalidade(s)" },
                    onlyView: { show: _permissoes.onlyView }
                },
                action: { onDblClick: NotaAzul.ContasReceber.Lista.Abrir },
                columns: [
                    { header: "Aluno", nameJson: "Nome" },
                    { header: "Curso", nameJson: "Curso" },
                    { header: "Turma", nameJson: "Turma" },
                    { header: "Valor", nameJson: "Valor", mask: "money", width: '200px' },
                    { header: "Desconto", nameJson: "Desconto", mask: "money", width: '200px' },
                    { header: "Vencimento", nameJson: "DataVencimento", type: "date", width: '200px' }
                ]
            });

            // adiciona um botão à lista GrupoUsuario
            _grid.addButton({
                click: function () {
                    NotaAzul.ContasReceber.MensalidadesPagas.CarregarMensalidadesPagas();
                },
                title: "Mensalidades Pagas",
                tooltip: "Mensalidades Pagas",
                icon: { src: "Content/notaAzul/pagamentoEfetuado.png" }
            });
        };

        /**
        * @descrição: Adiciona os eventos dos botões
        **/
        var _definirAcoesBotoes = function () {
            Prion.Event.add("radioAluno", "click", function () {
                ("divAluno").getDom().style.display = "block";
                var arrayFiltro = document.querySelectorAll("#filtros > div:not(#divAluno)");
                for (var i = 0; i < arrayFiltro.length; i++) {
                    arrayFiltro[i].style.display = "none";
                }
                Prion.ClearForm("frmFiltroAluno");
            });

            Prion.Event.add("radioCursoTurma", "click", function () {
                var divCursoTurma = "divCursoTurma".getDom();
                divCursoTurma.style.display = "block";
                divCursoTurma.style.height = "110px";

                var arrayFiltro = document.querySelectorAll("#filtros > div:not(#divCursoTurma)");

                for (var i = 0; i < arrayFiltro.length; i++) {
                    arrayFiltro[i].style.display = "none";
                }

                Prion.ClearForm("frmFiltroCursoTurma");
            });

            // quando clicar no botão de buscar cursos...
            Prion.Event.add("btnBuscarCursos", "click", function () {
                _carregarCursoAnoLetivo();
            });

            // quando escolher um novo item no combobox listaCursos
            Prion.Event.add("ListaCurso", "change", function () {
                "ListaTurma".clear();
                "ListaAluno".clear();
                var itemCurso = Prion.ComboBox.Get("ListaCurso");
                _carregarTurmaAnoLetivo(itemCurso.data);
            });

            // quando escolher um novo item no combobox listaTurma
            Prion.Event.add("ListaTurma", "change", function () {
                "ListaAluno".clear();
                var itemTurma = Prion.ComboBox.Get("ListaTurma");
                _carregarAluno(itemTurma.data);
            });

            Prion.Event.add("btnAplicarFiltroAluno", "click", function () {
                _grid.load({
                    query: "{\"Filtros\":[{\"Campo\":\"Aluno.Id\",\"Operador\":\"=\",\"Valor\":\"" + "Filtro_Aluno".getValue() + "\"}]}"
                });
            });

            Prion.Event.add("btnAplicarFiltroCursoTurma", "click", function () {
                var json = "";
                //Verifica quais comboBox foram utilizados
                if (Prion.ComboBox.Get("ListaCurso").value === "") { return false; }

                else if (Prion.ComboBox.Get("ListaAluno").value !== "") {
                    //Se caiu aqui, o filtro a ser utilizado será o de aluno
                    json = "{\"Filtros\":[{\"Campo\":\"Aluno.Id\",\"Operador\":\"=\",\"Valor\":\"" + Prion.ComboBox.Get("ListaAluno").value + "\"}]}";
                }
                else if (Prion.ComboBox.Get("ListaTurma").value !== "") {
                    //Se caiu aqui,o filtro a ser utilizado será o de Turma
                    json = "{\"Filtros\":[{\"Campo\":\"Turma.Id\",\"Operador\":\"=\",\"Valor\":\"" + Prion.ComboBox.Get("ListaTurma").value + "\"}]}";
                }
                else {
                    //Se caiu aqui, o filtro a ser utilizado será o de curso
                    json = "{\"Filtros\":[{\"Campo\":\"Curso.Id\",\"Operador\":\"=\",\"Valor\":\"" + Prion.ComboBox.Get("ListaCurso").value + "\"}]}";
                }
                _grid.load({
                    query: json
                });

                return true;
            });

        };

        /**
        * @descrição: Responsável pelo AutoComplete do campo Aluno
        * @return: void
        **/
        var _autoCompleteBuscarAluno = function () {
            $("#Filtro_NomeAluno").autocomplete({
                source: rootDir + "Aluno/ListaAlunoPorNome",
                minLength: 2,
                appendTo: $("#Filtro_NomeAluno").parent(),
                delay: 500,
                dataType: 'json',
                focus: true,
                open: function () {
                    $(".ui-autocomplete:visible").css({ top: "155px", left: "139px", "max-height": "160px", "overflow-y": "auto" });

                },
                select: function (event, data) {
                    "Filtro_NomeAluno".setValue(data.item.label);
                    "Filtro_Aluno".setValue(data.item.value);
                    return false;
                }
            });
        };

        /**
        * @descrição: Carrega todos os cursos baseado no ano letivo
        * @return: void
        **/
        var _carregarCursoAnoLetivo = function () {
            var anoLetivo = "AnoLetivo".getValue();

            // validações
            if ((anoLetivo === "") || (anoLetivo.length < 4)) {
                Prion.Alert({ msg: "Informe o ano letivo." });
                return;
            }


            Prion.ComboBox.Carregar({
                url: rootDir + "CursoAnoLetivo/GetLista",
                el: "ListaCurso",
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
            if (itemCurso == null) { return false; }

            Prion.ComboBox.Carregar({
                url: rootDir + "Turma/GetLista",
                el: "ListaTurma",
                filter: "Entidades=CursoAnoLetivo&Paginar=false&CursoAnoLetivo.Id=" + itemCurso.Id,
                clear: true,
                valueDefault: true
            });

            return true;
        };

        /**
        * @descrição: Carrega todas as turmas baseado no ano letivo e no curso escolhido
        * @return: void
        **/
        var _carregarAluno = function (itemTurma) {

            if (itemTurma == null) { return false; }

            Prion.ComboBox.Carregar({
                url: rootDir + "Aluno/GetLista",
                el: "ListaAluno",
                filter: "Entidades=MatriculaCurso&Paginar=false&MatriculaCurso.IdTurma=" + itemTurma.Id,
                clear: true,
                valueDefault: true
            });
            return true;
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            _permissoes = Permissoes.CRUD("CONTAS_RECEBER");
            _definirAcoesBotoes();
            _criarLista();
            _carregarJanelaContasReceber();
            _carregarJanelaMensalidadesPagas();
            _autoCompleteBuscarAluno();
        };

        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (obj) {
            _windowContasReceber.apply({
                object: 'MensalidadeTitulo',
                url: rootDir + "ContasReceber/Carregar/" + obj.Id,
                ajax: { onlyJson: true },
                fnBeforeLoad: function () {
                    NotaAzul.ContasReceber.Pagamento.Novo();
                    NotaAzul.ContasReceber.Pagamento.GridMensalidade().clear();
                },
                success: function (retorno) {
                    document.querySelectorAll("#popupPagarMensalidadeTitulo .popupWindowTitleH3")[0].innerText = obj.Nome;
                    NotaAzul.ContasReceber.Pagamento.ObjPagamento().Nome = obj.Nome;
                    NotaAzul.ContasReceber.Pagamento.ObjPagamento().Turma = obj.Turma;
                    NotaAzul.ContasReceber.Pagamento.ObjPagamento().Turno = obj.Turno;
                    NotaAzul.ContasReceber.Pagamento.ObjPagamento().Curso = obj.Curso;
                    NotaAzul.ContasReceber.Pagamento.GridMensalidade().addRow(retorno.obj);
                    var linhasDoGridMensalidade = Array.fromList(document.querySelectorAll("#ListaMensalidadesAbertasBody tbody tr"));
                    var contador = NotaAzul.ContasReceber.Pagamento.GridMensalidade().rows().length;
                    for (var i = 0; i < contador; i++) {
                        if (NotaAzul.ContasReceber.Pagamento.GridMensalidade().get(i).Id === obj.Id) {
                            break;
                        }
                    }
                    linhasDoGridMensalidade[i].setAttribute("class", "lineSelected");
                }
            }).load().show({ animate: true });
        };

        /**
        * @descrição: Método que retorna o objeto do grid
        * @return: Objeto grid
        **/
        var _getGrid = function () {
            return _grid;
        };

        /**
        * @descrição: Método que retorna o objeto da janela de mensalidade
        * @return: Objeto grid
        **/
        var _getWindowMensalidadePaga = function () {
            return _winMensalidadesPagas;
        };
        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            Iniciar: _iniciar,
            Abrir: _abrir,
            Grid: _getGrid,
            WindowMensalidadePaga: _getWindowMensalidadePaga
        };
    } ();

    NotaAzul.ContasReceber.Lista.Iniciar();
} (window, document));


