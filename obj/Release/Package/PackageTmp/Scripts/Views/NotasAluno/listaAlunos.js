/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        formaConceito = formaConceito || "",
        divisaoAnoLetivo = divisaoAnoLetivo | "",
        rootDir = rootDir || "";

$(window).ready(function () {
    "use strict";

    if (NotaAzul.NotasAluno == null) {
        NotaAzul.NotasAluno = {};
    }

    NotaAzul.NotasAluno.ListaAlunos = function () {
        /**********************************************************************************************
        ** PRIVATE
        **********************************************************************************************/
        var _valorMin = 0,
            _valorMax,
            _grid = null,
            _idTurma = 0,
            _permissoes = null,
            _windowNotasAluno = null,
        //Capitalize o valor de divisaoAnoLetivo,criando o id do combo
            _idDivisaoAno = "Lista" + divisaoAnoLetivo[0].toUpperCase() + divisaoAnoLetivo.substring(1);

        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function () {
            var idDisciplina = Prion.ComboBox.Get("ListaDisciplina").value,
                    periodoAvaliacao = Prion.ComboBox.Get(_idDivisaoAno).value,
                    nomeAluno = NotaAzul.NotasAluno.ListaAlunos.Grid().getSelected()[0].object.Nome,
                    idMatricula = NotaAzul.NotasAluno.ListaAlunos.Grid().getSelected()[0].object.Id;

            _windowNotasAluno.apply({
                object: 'ComposicaoNota',
                url: rootDir + "NotasAluno/CarregarComposicaoNotaPeriodo/" + idDisciplina,
                ajax: { onlyJson: true },
                filter: { Entidades: "FormaAvaliacao,Nota", "ComposicaoNotaPeriodo.PeriodoDeAvaliacao": periodoAvaliacao },
                success: function (retorno) {
                    if ((retorno != null) && (retorno.obj != null)) {
                        var objFormasAvaliacao = retorno.obj;
                        Prion.Request({
                            url: rootDir + "NotasAluno/CarregarFormasDeAvaliacaoDeUmAluno/" + idMatricula,
                            data: { Entidades: "MatriculaNota", "ProfessorDisciplina.IdDisciplina": idDisciplina, "ComposicaoNotaPeriodo.PeriodoDeAvaliacao": periodoAvaliacao },
                            fnBeforeLoad: NotaAzul.NotasAluno.Dados.Novo(nomeAluno, idMatricula, objFormasAvaliacao),
                            success: function (novoRetorno) {
                                if ((novoRetorno != null) && (novoRetorno.obj != null)) {
                                    NotaAzul.NotasAluno.Dados.AplicarListaMatriculaFormasDeAvaliacao(novoRetorno.obj);
                                }
                            }
                        });
                    }
                }
            }).load().show({ animate: true });
        };

        /**
        * @descrição:Adiciona o valor máximo que uma nota possa alcançar
        * @return: void
        **/
        var _adicionarLimiteNota = function () {
            var $inpuNotaDom = document.querySelectorAll("#listaAlunosBody [nameJson='ValorAlcancado']"),
                arrNota = Array.fromList($inpuNotaDom);

            Prion.Event.add(arrNota, "blur", function () {
                if (parseFloat(this.value) > _valorMax) {
                    window.alert("O valor inserido é maior que o valor total da forma de avaliação");
                    this.value = parseInt(_valorMax);
                }
            });
            for (var i = 0, len = $inpuNotaDom.length; i < len; i++) {
                $inpuNotaDom[i].setAttribute("max", parseInt(_valorMax));
            }
        };

        /**
        * @descrição:Aplica os valores alcançados a seus alunos correspondentes
        * @return:void
        **/
        var _aplicarValoresAlcancadosAoGrid = function (listaMatriculaFormasDeAvalicao) {
            var arrDom = document.querySelectorAll("#listaAlunosBody [nameJson='ValorAlcancado']");

            for (var i = 0, tamanhoLista = listaMatriculaFormasDeAvalicao.length; i < tamanhoLista; i++) {
                //Através do tamanho do array contendo o DOM dos inputs de Valor Alcançado
                //Será comparado o id de matrícula da listaMatriculaFormasDeAvalicao com o Id de matrícula da iteração do grid
                for (var j = 0, len = arrDom.length; j < len; j++) {
                    if (listaMatriculaFormasDeAvalicao[i].IdMatricula == _grid.rows()[j].Id) {
                        arrDom[j].value = parseInt(listaMatriculaFormasDeAvalicao[i].ValorAlcancado);
                        arrDom[j].setAttribute("IdMatriculaFormaDeAvaliacao", parseInt(listaMatriculaFormasDeAvalicao[i].Id));
                        break;
                    }
                }
            }
        };

        /**
        * @descrição:Responsável por ativar o botão de criar notas/médias
        **/
        var _ativarBotaoCriarMedia = function () {
            var domListaFormasAvaliacao = "ListaFormasDeAvaliacao".getDom(),
                dataUltimaAvaliacao = JSON.parse(domListaFormasAvaliacao.options[domListaFormasAvaliacao.options.length - 1].getAttribute("json")).DataAvaliacao,
            //Cria um Date através da variável anterior
                ultimaDataFormatada = new Date(parseInt(dataUltimaAvaliacao.toString().substr(6, 4)), parseInt(dataUltimaAvaliacao.toString().substr(3, 2)), parseInt(dataUltimaAvaliacao.toString().substr(0, 2))),
                dataAtual = Prion.Date.DataAtual(),
                dataAtualFormatada = new Date(parseInt(dataAtual.toString().substr(6, 4)), parseInt(dataAtual.toString().substr(3, 2)), parseInt(dataAtual.toString().substr(0, 2)));

            if (dataAtualFormatada >= ultimaDataFormatada) {
                _grid.enableButton("update");
            } else {
                _grid.disableButton("update");
            }
        };

        /**
        * @descrição: Carrega o comboBox de Turma
        * @return: void
        **/
        var _carregarComboBoxTurma = function () {
            Prion.ComboBox.Carregar({
                url: rootDir + "Turma/GetLista",
                el: "ListaTurma",
                filter: "Entidades=CursoAnoLetivo,Funcionario&Paginar=false&CursoAnoLetivo.AnoLetivo=" + Prion.Date.AnoAtual(),
                clear: true,
                valueDefault: true
            });
        };

        /**
        * @descrição: Carrega o comboBox de Disciplina
        * @return: void
        **/
        var _carregarComboBoxDisciplina = function (elemento) {
            if (elemento == null) { return; }

            var idTurma = Prion.ComboBox.Get(elemento).value;
            _idTurma = idTurma;

            Prion.ComboBox.Carregar({
                url: rootDir + "Disciplina/GetLista",
                el: "ListaDisciplina",
                filter: "Entidades=Professor&Paginar=false&ProfessorDisciplina.IdTurma=" + idTurma,
                clear: true,
                valueDefault: true
            });
        };

        /**
        * @descrição: Carrega o comboBox de Forma de Avaliação
        * @return: void
        **/
        var _carregarComboBoxFormaAvaliacao = function (objDisciplina, objetoDivisaoAnoLetivo) {
            if (objDisciplina == null || objetoDivisaoAnoLetivo == null) { return; }

            var idDisciplina = objDisciplina.value,
                periodoDeAvaliacao = objetoDivisaoAnoLetivo.value;

            Prion.ComboBox.Carregar({
                url: rootDir + "NotasAluno/GetListaFormasDeAvaliacao",
                el: "ListaFormasDeAvaliacao",
                filter: "Entidades=MatriculaCurso,Aluno&Paginar=false&ProfessorDisciplina.IdDisciplina=" + idDisciplina + "&ComposicaoNotaPeriodo.PeriodoDeAvaliacao=" + periodoDeAvaliacao,
                clear: true,
                valueDefault: true,
                action: {
                    complete: _ativarBotaoCriarMedia
                }
            });
        };

        /**
        * @descricao: Carrega o conteúdo do html Aluno/ViewDados em background, acelerando o futuro carregamento
        * @return: void
        **/
        var _carregarJanelaNotasAluno = function () {

            // carrega a ViewDados de Aluno
            _windowNotasAluno = new Prion.Window({
                url: rootDir + "NotasAluno/ViewDados/",
                id: "popupNotas",
                modal: true,
                height: 600,
                width: 600,
                title: { text: "Notas/Média" },
                buttons: {
                    show: (!_permissoes.onlyView), // se onlyView for false, significa então que os botões deverão ser exibidos
                    buttons: [
                        {
                            text: "Gerar Média",
                            className: Prion.settings.ClassName.button,
                            typeButton: "save", // usado para controle interno
                            click: function (win) {
                                NotaAzul.NotasAluno.Dados.CalcularMedia();
                            }
                        }
                    ]
                }
            });
        };

        /**
        * @descrição: Carrega os objetos de MatriculaFormaDeAvaliacao
        * @return: void
        **/
        var _carregarMatriculasFormasDeAvaliacao = function () {
            Prion.Request({
                url: rootDir + "NotasAluno/Carregar/" + Prion.ComboBox.Get("ListaFormasDeAvaliacao").value,
                success: function (retorno) {
                    if (retorno.obj == null) {
                        console.log("Nada encontrado");
                        return;
                    }
                    _aplicarValoresAlcancadosAoGrid(retorno.obj);
                }
            });
        };


        /**
        * @descrição: Cria o todo o html da lista de mensalidades
        * @return: void
        **/
        var _criarLista = function () {
            if (_grid != null) { return; }

            // campo custom do tipo INPUT para valorAlcançado
            var columnValorAlcancado = new Prion.Lista.CustomField.Input({
                type: "number",
                value: "", // value default do campo
                attributes: { maxlength: 10, style: "width:90px", mandatory: true, message: "mensagem", nameJson: "ValorAlcancado", class: "st-forminput", min: _valorMin, max: _valorMax }
            });

            _grid = new Prion.Lista({
                id: "listaAlunos",
                url: rootDir + "NotasAluno/GetLista/",
                autoLoad: false,
                title: { text: "Alunos" },
                height: 300,
                request: { base: "Matricula" }, // colocado pois o SQL não está com ALIAS para os campos da tabela Curso                
                rowNumber: {
                    show: false
                },
                buttons: {
                    update: { show: _permissoes.update, tooltip: "Notas/Médias", click: NotaAzul.NotasAluno.ListaAlunos.Abrir }
                },
                columns: [
                        { header: "Matrícula", nameJson: "Id", visible: false },
                        { header: "Aluno", nameJson: "Nome", width: "150px" },
                        { header: "Valor Alcançado", nameJson: "ValorAlcancado", width: "130px", customField: columnValorAlcancado, IdMatriculaFormaDeAvaliacao: 0 }
                    ]
            });

        };

        /**
        * @descrição: Define a ação para todos os controles da tela
        * @return: void
        **/
        var _definirAcoesBotoes = function () {

            //Evento de change do combo de Turma,caso o valor selecionado não seja null,carrega uma lista de alunos
            Prion.Event.add("ListaTurma", "change", function () {
                if (Prion.ComboBox.Get("ListaTurma").data == null) {
                    "ListaFormasDeAvaliacao".clear();
                }
                _grid.load({
                    url: rootDir + "ComposicaoNota/GetLista/",
                    filter: {
                        Entidades: "MatriculaCurso",
                        "MatriculaCurso.IdTurma": Prion.ComboBox.Get("ListaTurma").value
                    },
                    paging: {
                        currentPage: 1,
                        totalPerPage: 40
                    },
                    afterLoad: function () { _grid.disableButton("update"); }
                });
                _carregarComboBoxDisciplina(this);
            });

            //Evento de change do combo de Disciplinas
            Prion.Event.add("ListaDisciplina", "change", function () {
                Prion.ComboBox.SelecionarIndexPeloValue("ListaBimestral", "");
                "ListaFormasDeAvaliacao".clear();
            });

            //Evento de change do combo de Disciplinas
            Prion.Event.add(_idDivisaoAno, "change", function () {
                if (Prion.ComboBox.Get(_idDivisaoAno).value === "") {
                    _grid.disableButton("update");
                }

                _carregarComboBoxFormaAvaliacao(Prion.ComboBox.Get("ListaDisciplina"), this);
                _limparMatriculaFormaAvaliacao();
            });

            //Evento de change do combo de Formas de avaliação
            Prion.Event.add("ListaFormasDeAvaliacao", "change", function () {
                //Zera  os valores e oculta o campo Valor total
                _limparMatriculaFormaAvaliacao();

                //Prepara o campo Valor Total
                "divValorTotal".getDom().style.display = "inline";
                _valorMax = Prion.ComboBox.Get("ListaFormasDeAvaliacao").data.Valor;
                "ValorTotal".getDom().innerHTML = parseInt(_valorMax);

                //Adiciona o limite do input de number
                _adicionarLimiteNota();

                //Carrega os valores
                _carregarMatriculasFormasDeAvaliacao();
            });

            Prion.Event.add("btnSalvar", "click", function () {
                NotaAzul.NotasAluno.ListaAlunos.Salvar();
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
            _permissoes = Permissoes.CRUD("NOTAS_ALUNO");
            _valorMax = (formaConceito === "base_cem") ? 100 : 10;
            if (divisaoAnoLetivo !== null || divisaoAnoLetivo !== "") {
                divisaoAnoLetivo.getDom().style.display = "inline";
            }
            _criarLista();
            _carregarComboBoxTurma();
            _carregarJanelaNotasAluno();
            _definirAcoesBotoes();
        };

        /**
        * @descrição: limpa os inputs do grid       
        **/
        var _limparMatriculaFormaAvaliacao = function () {
            var arrDom = document.querySelectorAll("#listaAlunosBody [nameJson='ValorAlcancado']");
            "divValorTotal".getDom().style.display = "none";

            for (var i = 0, len = arrDom.length; i < len; i++) {
                arrDom[i].value = "";
                arrDom[i].setAttribute("IdMatriculaFormaDeAvaliacao", 0);
            }
        };

        /**
        * @descrição: Monta o objeto que será transformado em Json
        * @returns: objeto de matriculaFormaDeAvaliacao
        **/
        var _montarObjetoMatriculaFormaAvaliacao = function () {
            var matriculaFormaAvaliacao = [],
                arrDom = document.querySelectorAll("#listaAlunosBody [nameJson='ValorAlcancado']"),
                idFormadeAvaliacao = parseInt(Prion.ComboBox.Get("ListaFormasDeAvaliacao").value);

            for (var i = 0, len = arrDom.length; i < len; i++) {
                var valorFormaAvaliacao = arrDom[i].value,
                    id = arrDom[i].getAttribute("IdMatriculaFormaDeAvaliacao"),
                    idMatricula = NotaAzul.NotasAluno.ListaAlunos.Grid().rows()[i].Id;

                if (valorFormaAvaliacao === "") { continue; }

                matriculaFormaAvaliacao.push({ "Id": id, "IdMatricula": idMatricula, "ValorAlcancado": parseFloat(valorFormaAvaliacao), "IdFormaDeAvaliacao": idFormadeAvaliacao });
            }

            return matriculaFormaAvaliacao;
        };

        /**
        * @descrição: Salva todos os dados do form
        **/
        var _salvar = function () {
            var formaAvaliacaoMatricula = _montarObjetoMatriculaFormaAvaliacao();

            Prion.Request({
                url: rootDir + "NotasAluno/Salvar",
                data: "MatriculaFormaDeAvaliacao=" + JSON.stringify(formaAvaliacaoMatricula)
            });
        };

        _iniciar();
        /**********************************************************************************************
        ** PUBLIC
        **********************************************************************************************/
        return {
            Abrir: _abrir,
            Grid: _getGrid,
            Iniciar:_iniciar,
            Salvar: _salvar
        };
    } ();
    NotaAzul.NotasAluno.ListaAlunos.Iniciar();
} (window, document));
