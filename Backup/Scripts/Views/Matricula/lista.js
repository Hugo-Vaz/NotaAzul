/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Matricula == null) {
        NotaAzul.Matricula = {};
    }

    NotaAzul.Matricula.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowMatricula = null,
            _permissoes = null;


        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (obj) {
            if (obj == null) {
                Prion.Alert({ msg: "Erro ao obter o objeto de Matrícula." });
                return;
            }

            _windowMatricula.apply({
                object: 'Matricula',
                url: rootDir + "Matricula/Carregar/" + obj.Id,
                filter: { Entidades: "MatriculaCurso,CursoAnoLetivo,Mensalidade,Turma,Curso,Aluno,AlunoFilantropia,Situacao" },
                ajax: { onlyJson: true },
                fnBeforeLoad: NotaAzul.Matricula.Dados.Novo,
                success: function (retorno) {
                    "Matricula_NomeAluno".setValue(retorno.obj.Aluno.Nome);
                    if (retorno.obj.Aluno.Filantropia.length > 0) {
                        //Se caiu aqui é porque o aluno é bolsista  
                        "Matricula_idAlunoFilantropia".setValue(retorno.obj.Aluno.Filantropia[0].Id);
                        Prion.RadioButton.SelectByValue("BolsistaValor", retorno.obj.Aluno.Filantropia[0].ValorBolsa);
                    }
                    for (var i = 0, len = retorno.obj.ListaMatriculaCurso.length; i < len; i++) {
                        NotaAzul.Matricula.Dados.CriarAbasCurso(retorno.obj.ListaMatriculaCurso[i]);
                    }
                }
            }).load().show({ animate: true });
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
                    $(".ui-autocomplete:visible").css({ top: "initial", left: "139px", "max-height": "160px", "overflow-y": "auto" });

                },
                select: function (event, data) {
                    "Filtro_NomeAluno".setValue(data.item.label);
                    "Filtro_Aluno".setValue(data.item.value);
                    return false;
                }
            });
        };

        /**
        * @descrição: Carrega o conteúdo do html Matricula/ViewDados em background, acelerando o futuro carregamento
        **/
        var _carregarJanelaMatricula = function () {
            _windowMatricula = new Prion.Window({
                url: rootDir + "Matricula/ViewDados/",
                id: "popupMatricula",
                height: 570,
                title: { text: "Matrícula" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.Matricula.Lista.Grid().load();
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
                                NotaAzul.Matricula.Dados.Salvar(win);
                            }
                        },
                        {
                            text: "Limpar",
                            className: Prion.settings.ClassName.buttonReset,
                            type: "reset",
                            click: function (win) {
                                NotaAzul.Matricula.Dados.Novo();
                            }
                        },
                        {
                            text: "Gerar Carnê",
                            className: Prion.settings.ClassName.button,
                            click: function (win) {
                                NotaAzul.Matricula.Dados.AbrirCarneDeMensalidade(Prion.FormToObject("frmMatricula"));
                            }
                        },
                        {
                            text: "Gerar Carnê Agregado",
                            className: Prion.settings.ClassName.button,
                            click: function (win) {
                                NotaAzul.Matricula.Dados.AbrirCarneDeMensalidadeAgregado(Prion.FormToObject("frmMatricula"));
                            }
                        },
                        {
                            text: "Imprimir Ficha",
                            className: Prion.settings.ClassName.button,
                            click: function (win) {
                                NotaAzul.Matricula.Dados.AbrirFichaMatricula(Prion.FormToObject("frmMatricula"));
                            }
                        }
                    ]
                },
                success: function () {


                    // gera todos os controles responsáveis por manipular o combobox de GrupoDesconto
                    NotaAzul.GrupoDesconto.ComboBox.Gerar({
                        id: "matriculaListaGrupoDesconto",
                        autoLoad: true,
                        buttons: [ // adiciona um botão extra ao combobox
                                {
                                title: "Aplicar Desconto",
                                tooltip: "Aplicar Desconto",
                                applyCssDisabled: true,
                                click: function () {
                                    var combobox = "matriculaListaGrupoDesconto".getValue();

                                    if (combobox == null) {
                                        // exibir um alert para o usuário
                                        return;
                                    }

                                    // verifica se o usuário selecionou algum item no combobox
                                    if (!combobox.itemSelected) {
                                        Prion.Alert({ msg: "Por favor, selecione primeiro um item em Grupo de Desconto." });
                                        return;
                                    }


                                    if (window.confirm("Os valores de desconto presentes na tabela serão sobrescritos. Deseja continuar?")) {
                                        NotaAzul.Matricula.Dados.AtualizarMensalidade();
                                    }
                                }
                            },
                                {
                                    title: "Remover Desconto",
                                    tooltip: "Aplicar Desconto",
                                    applyCssDisabled: true,
                                    click: function () {

                                        if (window.confirm("O desconto será removido das mensalidades ainda não pagas. Deseja continuar?")) {
                                            NotaAzul.Matricula.Dados.RemoverDesconto();
                                        }
                                    }
                                }
                        ]
                    });


                    // gera todos os controles responsáveis por manipular o combobox de TurmaAnoLetivo
                    NotaAzul.TurmaAnoLetivo.ComboBox.Gerar({
                        id: "matriculaListaTurmaAnoLetivo",
                        autoLoad: false, // indica que os registros não serão carregados automaticamente
                        load: function () {
                            // função responsável pelo carregamento dos registros
                            NotaAzul.Matricula.Dados.CarregarCursoAnoLetivo(this);
                        },
                        buttons: [ // adiciona um botão extra ao combobox
                            {
                            title: "Adicionar à Matrícula",
                            tooltip: "Adicionar à Matrícula",
                            applyCssDisabled: true,
                            click: function () {
                                NotaAzul.Matricula.Dados.CriarAbasCurso();
                            }
                        }
                        ]
                    });
                }
            });
        };

        /**
        * @descrição: Cria o todo o html da lista
        * @return: void
        **/
        var _criarLista = function () {
            if (_grid != null) { return; }

            _grid = new Prion.Lista({
                id: "listaMatriculas",
                url: rootDir + "Matricula/GetLista/",
                title: { text: "Matrículas" },
                filter: { Entidades: "Aluno, Curso, Turma, Turno, Situacao,MatriculaCurso" },
                request: { base: "Matricula" }, // colocado pois o SQL não está com ALIAS para os campos da tabela Matricula
                buttons: {
                    insert: { show: _permissoes.insert, click: NotaAzul.Matricula.Lista.Novo },
                    update: { show: _permissoes.update, click: NotaAzul.Matricula.Lista.Abrir },
                    remove: { show: _permissoes.remove, url: rootDir + "Matricula/Excluir/" },
                    onlyView: { show: _permissoes.onlyView }
                },
                action: { onDblClick: NotaAzul.Matricula.Lista.Abrir },
                columns: [
                    { header: "Aluno", nameJson: "Aluno.Nome", width: "200px" },
                    { header: "Curso", nameJson: "Cursos", width: "200px" },
                    { header: "Turma", nameJson: "Turmas", width: "200px" },
                    { header: "Turno", nameJson: "Turnos", width: "100px" },
                    { header: "Status", nameJson: "Situacao.Nome", width: "100px" },
                    { header: "Cadastrado em", nameJson: "DataCadastro", type: "date", width: "110px" }
                ]
            });
        };
        /**
        * @descrição: Adiciona os eventos dos botões
        **/
        var _definirAcoesBotoes = function () {

            Prion.Event.add("btnAplicarFiltroAluno", "click", function () {
                _grid.load({
                    query: "{\"Filtros\":[{\"Campo\":\"Aluno.Id\",\"Operador\":\"=\",\"Valor\":\"" + "Filtro_Aluno".getValue() + "\"}]}"
                });
            });
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
            _permissoes = Permissoes.CRUD("MATRICULA");

            _criarLista();
            _carregarJanelaMatricula();
            _autoCompleteBuscarAluno();
            _definirAcoesBotoes();
        };

        /**
        * @descrição: Método responsável por abrir uma popup para inserção de novo registro
        * @return: void
        **/
        var _novo = function () {
            _windowMatricula.show({
                animate: true,
                fnBefore: NotaAzul.Matricula.Dados.Novo()
            });
        };
        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            Abrir: _abrir,
            Grid: _getGrid,
            Iniciar: _iniciar,
            Novo: _novo
        };
    } ();

    NotaAzul.Matricula.Lista.Iniciar();
} (window, document));