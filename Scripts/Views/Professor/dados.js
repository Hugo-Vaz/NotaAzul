/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strinct";
    if (NotaAzul.Professor == null) {
        NotaAzul.Professor = {};
    }

    NotaAzul.Professor.Dados = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _arrayObjDisciplinas = null,
            _idTurma = null,
            _listaDeDisciplinas = [];
        /**
        * @descrição: Marca os checkbox de acordo com o array de objetos
        **/
        var _aplicarObjetosDisciplina = function () {
            if (_arrayObjDisciplinas == null) { return false; }

            var $checkBoxes = document.getElementsByName("disciplina");

            for (var i = 0, len = $checkBoxes.length; i < len; i++) {
                $checkBoxes[i].checked = false;
                $checkBoxes[i].removeAttribute("Estado");
                for (var j = 0, quantidadeDisciplinas = _arrayObjDisciplinas.length; j < quantidadeDisciplinas; j++) {
                    if ($checkBoxes[i].value == _arrayObjDisciplinas[j].Id && _arrayObjDisciplinas[j].IdTurma === _idTurma) {
                        $checkBoxes[i].checked = true;
                        $checkBoxes[i].setAttribute("Estado", EstadoObjeto.Consultado);
                    }
                }
            }
            return true;
        };

        /**
        * @descrição: Carrega os registros de disciplina
        **/
        var _carregarDisciplinas = function () {
            Prion.Request({
                url: rootDir + "Disciplina/GetLista",
                data: { Entidades: "Turma", "DisciplinaTurma.IdTurma": _idTurma },
                success: function (retorno) {
                    _listaDeDisciplinas.push({ "IdTurma": _idTurma, "Disciplinas": JSON.parse(retorno.rows) });
                    _gerarCheckBox(JSON.parse(retorno.rows));
                },
                error: function (win) {
                    if (win != null) {
                        win.config.reloadObserver = false;
                        win.mask();
                    }
                }
            });
        };

        /**
        * @descrição: Carrega todas as turmas
        * @return: void
        **/
        var _carregarTurma = function () {
            Prion.ComboBox.Carregar({
                url: rootDir + "Turma/GetLista",
                el: "listaTurma",
                filter: "Entidades=CursoAnoLetivo&Paginar=false&CursoAnoLetivo.AnoLetivo=" + Prion.Date.AnoAtual(),
                clear: true,
                valueDefault: true
            });
        };

        /**
        * @descrição: Define a ação para todos os controles da tela
        * @return: void
        **/
        var _definirAcoesBotoes = function () {
            var $checkBoxes = document.getElementsByName("disciplina");
            //evento associado ao checkbox de selecionar todos
            Prion.Event.add("selecionarTodos", "click", function () {
                var boolean = false,
                    $selecionarTodosDOM = document.getElementById("selecionarTodos");

                if ($selecionarTodosDOM.checked === true) { boolean = true; }

                for (var i = 0, len = $checkBoxes.length; i < len; i++) {
                    $checkBoxes[i].checked = boolean;
                    _verificarEstadoCheckbox($checkBoxes[i]);
                }
            });

            //evento associado ao change do combobox
            Prion.Event.add("listaTurma", "change", function () {
                _idTurma = Prion.ComboBox.Get("listaTurma").value;
                _removerCheckBox();
                if (_listaDeDisciplinas.length > 0) {
                    for (var i = 0, len = _listaDeDisciplinas.length; i < len; i++) {
                        if (_listaDeDisciplinas[i].IdTurma == _idTurma) {
                            _gerarCheckBox(_listaDeDisciplinas[i].Disciplinas);
                            return;
                        }
                    }
                }
                _carregarDisciplinas();
            });

        };

        /**
        * @descrição: Método que será chamado sempre após Salvar
        **/
        var _executarAposSalvar = function (retorno) {

        };

        /**
        * @descrição: Gera os checkbox com os valores de disciplinas
        **/
        var _gerarCheckBox = function (disciplinas) {
            var $checkGroup = document.getElementById("checkBoxGrp"),
                $divPai = document.createElement("div");

            $divPai.style.position = "relative";
            $divPai.style.float = "left";
            $divPai.style.marginRight = "40px";

            for (var i = 0; i < disciplinas.length; i++) {
                var $checkBox = document.createElement('input'),
                    $label = document.createElement('label'),
                    $div = document.createElement("div");

                $checkBox.type = 'checkbox';
                $checkBox.value = disciplinas[i].Id;
                $checkBox.name = "disciplina";
                $label.innerHTML = disciplinas[i].Nome;

                $div.appendChild($checkBox);
                $div.appendChild($label);

                $divPai.appendChild($div);

                if (i % 5 === 0 && i !== 0) {
                    $checkGroup.appendChild($divPai);
                    $divPai = document.createElement("div");
                    $divPai.style.position = "relative";
                    $divPai.style.float = "left";
                    $divPai.style.marginRight = "40px";
                }

            }
            $checkGroup.appendChild($divPai);

            var $checkBoxes = document.getElementsByName("disciplina");

            //Evento associado a mudança de estado dos CheckBox
            var arrarCheck = Array.fromList($checkBoxes);
            Prion.Event.add(arrarCheck, "click", function () {
                _verificarEstadoCheckbox(this);
            });

            _aplicarObjetosDisciplina();
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            _definirAcoesBotoes();
        };

        /**
        * @descrição: métodos que serão executados quando for abrir para novo registro
        **/
        var _novo = function (idProfessor, nomeProfessor, disciplinas) {
            _removerCheckBox();
            _carregarTurma();
            _listaDeDisciplinas = [];

            "Funcionario_Nome".getDom().innerHTML = nomeProfessor;
            "Funcionario_Id".getDom().value = idProfessor;
            _arrayObjDisciplinas = disciplinas;
        };

        /**
        * @descrição: remove os checkbox da janela
        **/
        var _removerCheckBox = function () {
            var $dom = "checkBoxGrp".getDom();

            while ($dom.hasChildNodes()) {
                $dom.removeChild($dom.lastChild);
            }
        };

        /**
        * @descrição: Salva todos os dados do form
        **/
        var _salvar = function (win) {
            if (!Prion.ValidateForm("frmProfessor")) {
                win.config.reloadObserver = false;
                return false;
            }

            if (win != null) { win.mask(); }

            //Seleciona o DOM de todos os checkboxes
            var $checkBoxes = document.getElementsByName("disciplina");

            if ($checkBoxes == null || $checkBoxes.length === 0) { return false; }

            //Monta um array com os Ids dos checkbox selecionados
            var arrayIds = [];
            for (var i = 0; i < $checkBoxes.length; i++) {
                var estado = $checkBoxes[i].getAttribute("Estado");
                if (($checkBoxes[i].checked === true) || (estado !== null)) {
                    arrayIds.push({ "Id": $checkBoxes[i].value, "Estado": estado });
                }
            }

            Prion.Request({
                form: "frmProfessor",
                url: rootDir + "Professor/Salvar",
                data: "Ids={\"Ids\":" + JSON.stringify(arrayIds) + "}",
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

        /**
        * @descrição: Verifica o estado do checkbox
        **/
        var _verificarEstadoCheckbox = function ($checkBox) {
            var estado = $checkBox.getAttribute("Estado");

            if ((estado === null) && ($checkBox.checked === true)) {
                $checkBox.setAttribute("Estado", EstadoObjeto.Novo);
            }
            else if ((estado === EstadoObjeto.Novo) && ($checkBox.checked === false)) {
                $checkBox.removeAttribute("Estado");
            }
            else if ((estado === EstadoObjeto.Excluido) && ($checkBox.checked === true)) {
                $checkBox.setAttribute("Estado", EstadoObjeto.Consultado);
            }
            else if ((estado === EstadoObjeto.Consultado) && ($checkBox.checked === false)) {
                $checkBox.setAttribute("Estado", EstadoObjeto.Excluido);
            }
        };

        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            ExecutarAposSalvar: _executarAposSalvar,
            Iniciar: _iniciar,
            Novo: _novo,
            Salvar: _salvar
        };
    } ();
    NotaAzul.Professor.Dados.Iniciar();
} (window, document));