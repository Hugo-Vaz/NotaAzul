/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Turma == null) {
        NotaAzul.Turma = {};
    }

    NotaAzul.Turma.Dados = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/

        /**
        * @descrição: Carrega os combobox desta janela
        * @return: void
        **/
        var _carregarCombobox = function () {
            NotaAzul.CursoAnoLetivo.ComboBox.Gerar();
            NotaAzul.Turno.ComboBox.Gerar();
        };

        /**
        * @descrição: Carrega todos os cursos baseado no ano letivo
        * @return: void
        **/
        var _carregarCursoAnoLetivo = function () {
            var anoLetivo = "Turma_AnoLetivo".getValue();

            _limparCamposInformacoesCurso();

            // validações
            if ((anoLetivo === "") || (anoLetivo.length < 4)) {
                Prion.Alert({ msg: "Informe o ano letivo." });
                return;
            }

            Prion.ComboBox.Carregar({
                url: rootDir + "CursoAnoLetivo/GetLista",
                el: "comboboxCursoAnoLetivo",
                filter: "Entidades=Curso&Paginar=false&AnoLetivo=" + anoLetivo,
                clear: true,
                valueDefault: true,
                valueJson: "Id",
                textJson: "Curso.Nome"
            });
        };


        /**
        * @descrição: Carrega todas as informações do curso selecionado
        * @return: void
        **/
        var _carregarInformacoesCurso = function (/**fnAfterLoad*/) {

            // obtem o item selecionado no combobox comboboxCursoAnoLetivo
            var itemCurso = "comboboxCursoAnoLetivo".getValue();
            var anoLetivo = "Turma_AnoLetivo".getValue();

            // limpa todas as informações do curso
            _limparCamposInformacoesCurso();


            // validações 
            if ((anoLetivo === "") || (anoLetivo.length < 4)) {
                Prion.Alert({ msg: "Informe o ano letivo." });
                return;
            }

            // verifica se foi selecionado algum curso
            if (itemCurso.itemSelected === "") {
                Prion.Alert({ msg: "Selecione um curso" });
                return;
            }


            // exibe informações sobre o curso selecionado
            var cursoAnoletivo = itemCurso.data;
            "CursoAnoLetivo_ValorMatricula".setValue("R$ " + parseFloat(cursoAnoletivo.ValorMatricula).format(2, ",", "."));
            "CursoAnoLetivo_ValorMensalidade".setValue("R$ " + parseFloat(cursoAnoletivo.ValorMensalidade).format(2, ",", "."));
            "CursoAnoLetivo_QtdMensalidade".setValue(cursoAnoletivo.QuantidadeMensalidades);
        };


        /**
        * @descrição: Define ação para todos os controles da tela 
        **/
        var _definirAcaoBotoes = function () {

            // quando clicar no botão de buscar cursos...
            Prion.Event.add("btnBuscarCursoAnoLetivo", "click", function () {
                NotaAzul.Turma.Dados.CarregarCursoAnoLetivo();
            });


            // quando selecionar um item no combobox comboboxCursoAnoLetivo
            Prion.Event.add("comboboxCursoAnoLetivo", "change", function () {
                NotaAzul.Turma.Dados.CarregarInformacoesCurso();
            });

        };

        /**
        * @descrição: Método que será chamado sempre após salvar 
        **/
        var _executarAposSalvar = function (retorno) {
            NotaAzul.Turma.Dados.Novo();
            return false;
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            _definirAcaoBotoes();
            _limparCamposInformacoesCurso();
            _carregarCombobox();
        };

        /**
        * @descrição: Limpa todos os campos de curso antes de buscar as turmas
        **/
        var _limparCamposInformacoesCurso = function () {
            // limpa os campos com informações sobre o curso selecionado
            "CursoAnoLetivo_ValorMatricula".setValue("R$ 0.00");
            "CursoAnoLetivo_ValorMensalidade".setValue("R$ 0.00");
            "CursoAnoLetivo_QtdMensalidade".setValue("0");
        };

        /**
        * @descrição: Reseta o form, voltando os dados default
        **/
        var _novaTurma = function () {
            Prion.ClearForm("frmTurma", true);

            // define o ano tual como sendo o ano letivo
            "Turma_AnoLetivo".setValue(Prion.Date.AnoAtual());

            // carrega a lista de curso para o ano letivo informado
            NotaAzul.Turma.Dados.CarregarCursoAnoLetivo();

            _carregarCombobox();
        };

        /**
        * @descrição: Salva todos os dados do form
        **/
        var _salvar = function (win) {
            if (!Prion.ValidateForm("frmTurma")) {
                win.config.reloadObserver = false;
                return false;
            }

            if (win != null) { win.mask(); }

            Prion.Request({
                form: "frmTurma",
                url: rootDir + "Turma/Salvar",
                success: function (retorno, registroNovo) {
                    // se for um registro existente, fecha a janela
                    if ((!registroNovo) && (win != null)) {
                        win.config.reloadObserver = true;
                        win.hide({ animate: true });
                        return;
                    }

                    NotaAzul.Turma.Dados.ExecutarAposSalvar();
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
        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            CarregarCursoAnoLetivo: _carregarCursoAnoLetivo,
            CarregarInformacoesCurso:_carregarInformacoesCurso,
            ExecutarAposSalvar: _executarAposSalvar,
            Iniciar: _iniciar,
            Novo: _novaTurma,
            Salvar: _salvar
        };

    } ();

    NotaAzul.Turma.Dados.Iniciar();
} (window, document));