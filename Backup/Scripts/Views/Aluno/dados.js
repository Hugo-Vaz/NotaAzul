/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Aluno == null) {
        NotaAzul.Aluno = {};
    }


    NotaAzul.Aluno.Dados = function () {
        /**********************************************************************************************
        *** PRIVATE
        **********************************************************************************************/
        var _windowMatricula = null;

        /**
        * @descrição: Reseta o form, voltando os dados default
        * @return: void
        **/
        var _novoAluno = function () {
            Prion.ClearForm("frmAluno", true);
            NotaAzul.Aluno.Responsavel.Dados.Novo();
            Prion.SetTopBarraRolagem("#popupAlunoContent", 0);

            _carregarCombobox();

            // deixa a primeira aba selecionada
            $("#tabsAluno").tabs({ selected: 0 });
            //$("#tabsAluno").tabs("hide", "#tabFichaFinanceira");
            $("#tabsAluno").tabs({
                select: function (event, ui) {
                    if (ui.index == 2) {
                        NotaAzul.Aluno.FichaFinanceira.Lista.LimparGrid();
                        Prion.Request({
                            url: rootDir + "Aluno/CarregarFichaFinanceira/" + "Aluno_Id".getValue(),
                            data: { Entidades: "Aluno,MatriculaCurso,CursoAnoLetivo,Mensalidade,Turma,Curso,Situacao,AlunoResponsavel,Titulo" },
                            success: function (retorno) {
                                NotaAzul.Aluno.FichaFinanceira.Lista.CriarGrid(retorno.obj);
                            },
                            error: function () {
                                NotaAzul.Aluno.FichaFinanceira.Lista.CriarGrid(null);
                            }
                        });
                    }
                }
            });


            // limpa a lista de responsáveis
            if ((NotaAzul.Aluno.Responsavel != null) && (NotaAzul.Aluno.Responsavel.Lista)) {
                NotaAzul.Aluno.Responsavel.Lista.Grid().clear();
            }
        };


        /**
        * @descrição: Carrega os combobox desta janela
        * @return: void
        **/
        var _carregarCombobox = function () {
            NotaAzul.Estado.ComboBox.Gerar({ id: "alunoCertidaoListaEstados" });

            "alunoCertidaoListaCidades".clear();
            NotaAzul.Cidade.ComboBox.Gerar({
                id: "alunoCertidaoListaCidades",
                autoLoad: false,
                idOrigem: "alunoCertidaoListaEstados" // id do elemento de onde vai pegar os dados             
            });
        };

        /**
        * @descrição: Carrega os registros de cidade baseado no estado escolhido
        * @params: select => objeto do tipo combobox representando o responsavelListaEstados
        * @return: void
        **/
        var _carregarCidade = function (select, itemSelected) {
            if (select == null) { return; }

            var itemEstado = Prion.ComboBox.Get(select);

            Prion.ComboBox.Carregar({
                url: rootDir + "Cidade/GetLista",
                el: "alunoCertidaoListaCidades",
                filter: "Paginar=false&IdEstado=" + itemEstado.value,
                clear: true,
                valueDefault: true,
                selected: itemSelected
            });
        };

        /**
        * @descricao: Carrega o conteúdo do html Matricula/ViewDados em background, acelerando o futuro carregamento
        * @return: void
        **/
        var _carregarJanelaMatricula = function () {

            // carrega a ViewDados de Matricula
            // independe da ViewDados de Aluno
            /*_windowMatricula = new Prion.Window({
            url: rootDir + "Matricula/ViewDados/",
            id: "popupMatricula",
            modal: true,
            width: 740,
            height: 458
            });*/
        };


        /**
        * @descrição: Define a ação para todos os controles da tela
        * @return: void
        **/
        var _definirAcoesBotoes = function () {
            // evento associado ao CHANGE do combobox de ESTADOS
            Prion.Event.add("alunoCertidaoListaEstados", "change", function () {
                NotaAzul.Aluno.Dados.CarregarCidade(this);
            });

            Prion.Event.add("Aluno_DiaPagamento", "change", function () {
                console.log(this);
            });

        };

        /**
        * @descrição: Método que será chamado sempre após Salvar
        * @return: boolean
        **/
        var _executarAposSalvar = function (retorno) {

            // se o usuário quiser fazer a matrícula deste aluno, carrega a view Matricula/ViewDados passando o idAluno
            if (retorno.mensagem.EstadoObjeto == EstadoObjeto.Novo) {

                // exibe as abas
                //$("#tabsAluno").tabs("show", "#tabResponsaveis");


                /*
                if (confirm("Deseja efetuar a matrícula deste aluno?")) {
                var idAluno = retorno.mensagem.UltimoId;

                // carrega a popup com os dados para uma matricula
                Prion.Window({
                //object: 'Matricula',
                url: rootDir + "Matricula/Carregar/" + idAluno,
                el: "popupMatricula",
                ajax: { onlyJson: true },
                fnBeforeLoad: NotaAzul.Matricula.Dados.Novo
                }).show({ animate: true });

                return;
                }*/
            }

            NotaAzul.Aluno.Dados.Novo();

            return false;
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            _carregarJanelaMatricula();
            _definirAcoesBotoes();
            _carregarCombobox();
            // deixa a primeira aba selecionada e oculta a outra
            $("#tabsAluno").tabs({ selected: 0 });
            //$("#tabsAluno").tabs("hide", "#tabFichaFinanceira");
        };

        /**
        * @descrição: Salva todos os dados do form
        * @return: void
        **/
        var _salvar = function (win) {
            if (!Prion.ValidateForm("frmAluno")) {
                win.config.reloadObserver = false;
                return false;
            }

            var oldValue = "Aluno_DiaPagamento".getDom().getAttribute("oldvalue");
            var alterarDiaPagamento = false;

            if ((oldValue != null) && (oldValue !== "Aluno_DiaPagamento".getDom().value)) {
                if (window.confirm("Caso o aluno já esteja matriculado, a data de vencimento das mensalidades abertas serão alteradas.Deseja continuar?") === true) {
                    alterarDiaPagamento = true;
                }
            }

            if (win != null) { win.mask(); }


            Prion.Request({
                form: "frmAluno",
                url: rootDir + "Aluno/Salvar",
                data: "responsaveis=" + NotaAzul.Aluno.Responsavel.Lista.Grid().serialize() + "&alterarDataVencimento=" + alterarDiaPagamento,
                success: function (retorno, registroNovo) {
                    // se for um registro existente, fecha a janela
                    if ((!registroNovo) && (win != null)) {
                        win.config.reloadObserver = true;
                        win.hide({ animate: true });
                        return;
                    }

                    NotaAzul.Aluno.Dados.ExecutarAposSalvar(retorno);
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
        /**********************************************************************************************
        ** PUBLIC
        **********************************************************************************************/
        return {            
            Iniciar: _iniciar,
            CarregarCidade: _carregarCidade,
            ExecutarAposSalvar: _executarAposSalvar,
            Novo: _novoAluno,
            Salvar: _salvar
        };
    } ();
    NotaAzul.Aluno.Dados.Iniciar();
} (window, document));