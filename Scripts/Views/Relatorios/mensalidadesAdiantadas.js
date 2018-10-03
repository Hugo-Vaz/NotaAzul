/*jshint jquery:true */
var Prion = Prion || {},
    NotaAzul = NotaAzul || {},
    rootDir = rootDir || "";


NotaAzul.Relatorios.MensalidadesAdiantadas = function () {
    "use strict";
    /******************************************************************************************
    ** PRIVATE
    ******************************************************************************************/
    /**
    * @descrição: Cria as colunas que serão utilizadas no relatório
    * @return: void
    **/
    var _criarColunasDoGrid = function () {
        var colunas = [
                    { header: "Matrícula", nameJson: "NumeroMatricula", width: "150px" },
                    { header: "Aluno", nameJson: "NomeAluno", width: "250px" },
                    { header: "Curso", nameJson: "NomeCurso", width: "250px" },
                    { header: "Turma", nameJson: "NomeTurma", width: "250px" },
                    { header: "Referente à", nameJson: "DataVencimento", type: "date", width: "250px" },
                    { header: "Valor", nameJson: "Valor", mask: "money", width: "250px" }
                   ];

        return colunas;
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
                $(".ui-autocomplete:visible").css({ top: "155px", left: "139px" });

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
    * @descrição: Define ação para todos os controles da tela 
    **/
    var _definirAcaoBotoes = function () {
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
    };

    /**
    * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
    * @return: void
    **/
    var _iniciar = function () {
        _definirAcaoBotoes();
        _autoCompleteBuscarAluno();
    };
    /******************************************************************************************
    ** PUBLIC
    ******************************************************************************************/
    return {
        Iniciar: _iniciar,
        CriarColunasDoGrid: _criarColunasDoGrid
    };
} ();
