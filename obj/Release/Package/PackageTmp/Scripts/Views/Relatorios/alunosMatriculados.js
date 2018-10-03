/*jshint jquery:true */
var Prion = Prion || {},
    NotaAzul = NotaAzul || {},
    rootDir = rootDir || "";


NotaAzul.Relatorios.AlunosMatriculados = function () {
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
                    { header: "Matricula", nameJson: "NumeroMatricula", width: "150px" },
                    { header: "Aluno", nameJson: "NomeAluno", width: "250px" },
                    { header: "Curso", nameJson: "NomeCurso", width: "250px" },
                    { header: "Turma", nameJson: "NomeTurma", width: "250px" },
                    { header: "Turno", nameJson: "NomeTurno", width: "250px" }
                   ];

        return colunas;
    };

    /**
    * @descrição: Carrega todas as turmas baseado no ano letivo e no curso escolhido
    * @return: void
    **/
    var _carregarTurma = function () {
        Prion.ComboBox.Carregar({
            url: rootDir + "Turma/GetLista",
            el: "ListaTurma",
            filter: "Paginar=false",
            clear: true,
            valueDefault: true
        });
    };

    /**
    * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
    * @return: void
    **/
    var _iniciar = function () {
        _carregarTurma();
    };
    /******************************************************************************************
    ** PUBLIC
    ******************************************************************************************/
    return {
        Iniciar: _iniciar,
        CriarColunasDoGrid: _criarColunasDoGrid
    };
} ();
