/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {

    if (NotaAzul.Boletim == null) {
        NotaAzul.Boletim = {};
    }

    NotaAzul.Boletim.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowBoletim = null,
            _permissoes = null;


        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (obj) {
            var nomeAluno = obj["Aluno.Nome"],
                    nomeCurso = obj.NomeCurso,
                    nomeTurma = obj.NomeTurma,
                    nomeTurno = obj.NomeTurno,
                    idMatricula = obj.Id;

            _windowBoletim.apply({
                object: 'Disciplina',
                url: rootDir + "Boletim/CarregarDisciplinasDeUmaMatricula/" + obj.Id,
                ajax: { onlyJson: true },
                filter: { Entidades: "Turma,MatriculaCurso", Paginar: false },
                success: function (retorno) {
                    if ((retorno != null) && (retorno.obj != null)) {
                        var listaDisciplinas = retorno.obj;
                        NotaAzul.Boletim.Dados.CriarCorpoBoletim(listaDisciplinas);
                    }
                }
            }).load().show({
                animate: true,
                fnBefore: NotaAzul.Boletim.Dados.Novo(nomeAluno, idMatricula, nomeCurso, nomeTurma, nomeTurno)
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
                    $(".ui-autocomplete:visible").css({ top: "127px", left: "159px" });

                },
                select: function (event, data) {
                    "Filtro_NomeAluno".setValue(data.item.label);
                    "Filtro_Aluno".setValue(data.item.value);
                    return false;
                }
            });
        };

        /**
        * @descricao: Carrega o conteúdo do html Boletim/ViewDados em background, acelerando o futuro carregamento
        * @return: void
        **/
        var _carregarJanelaBoletim = function () {

            // carrega a ViewDados de Boletim
            _windowBoletim = new Prion.Window({
                url: rootDir + "Boletim/ViewDados/",
                id: "popupBoletim",
                modal: true,
                height: 420,
                width: 800,
                title: { text: "Boletim" },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.Boletim.Lista.Grid().load();
                    }
                },
                buttons: {
                    show: (_permissoes.onlyView), // se onlyView for true, significa então que os botões deverão ser exibidos
                    buttons: [
                        {
                            text: "Imprimir",
                            className: Prion.settings.ClassName.button,
                            typeButton: "save", // usado para controle interno
                            click: function (win) {
                                NotaAzul.Boletim.Dados.Imprimir();
                            }
                        }
                    ]
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
                id: "listaAlunos",
                url: rootDir + "Matricula/GetLista/",
                filter: { Entidades: "Aluno, Curso, Turma, Turno, Situacao,MatriculaCurso", "Curso.CursoCurricular": true },
                height: 600,
                title: { text: "Alunos" },
                request: { base: "Aluno" }, // colocado pois o SQL não está com ALIAS para os campos da tabela Curso
                buttons: {
                    update: { show: _permissoes.onlyView, click: NotaAzul.Boletim.Lista.Abrir, tooltip: "Abrir Boletim" },
                    onlyView: { show: _permissoes.onlyView }
                },
                rowNumber: {
                    show: false
                },
                action: { onDblClick: NotaAzul.Boletim.Lista.Abrir },
                columns: [
                    { header: "Nome", nameJson: "Aluno.Nome", width: "150px" },
                    { header: "Cadastrado em", nameJson: "DataCadastro", type: "date", width: "110px" }
                ]
            });
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            _permissoes = Permissoes.CRUD("BOLETIM");
            _criarLista();
            _autoCompleteBuscarAluno();
            _carregarJanelaBoletim();
        };

        /**
        * @descrição: métodos que serão executados quando for abrir para novo registro
        **/
        var _novo = function () {

        };
        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            Iniciar: _iniciar,
            Abrir: _abrir,
            Novo: _novo
        };
    } ();
    NotaAzul.Boletim.Lista.Iniciar();
} (window, document));