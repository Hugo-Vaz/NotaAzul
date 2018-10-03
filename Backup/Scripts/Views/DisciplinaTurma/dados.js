/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function () {
    "use strinct";
    if (NotaAzul.DisciplinaTurma == null) {
        NotaAzul.DisciplinaTurma = {};
    }

    NotaAzul.DisciplinaTurma.Dados = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        /**
        * @descrição: Carrega os registros de disciplina
        **/
        var _carregarDisciplinas = function (win) {
            Prion.Request({
                url: rootDir + "Disciplina/GetLista",
                success: function (retorno) {
                    _gerarCheckBox(JSON.parse(retorno.rows));
                },
                error: function () {
                    if (win != null) {
                        win.config.reloadObserver = false;
                        win.mask();
                    }
                }
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
                    selecionarTodosDOM = document.getElementById("selecionarTodos");

                if (selecionarTodosDOM.checked === true) { boolean = true; }

                for (var i = 0, len = $checkBoxes.length; i < len; i++) {
                    $checkBoxes[i].checked = boolean;
                    _verificarEstadoCheckbox($checkBoxes[i]);
                }
            });

            //Evento associado a mudança de estado dos CheckBox
            var arrarCheck = Array.fromList($checkBoxes);
            Prion.Event.add(arrarCheck, "click", function () {
                _verificarEstadoCheckbox(this);
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
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            _carregarDisciplinas();
            //_definirAcoesBotoes();
        };

        /**
        * @descrição: métodos que serão executados quando for abrir para novo registro
        **/
        var _novo = function (nomeTurma, idsDisciplina) {
            _definirAcoesBotoes();
            "Turma_Nome".getDom().innerHTML = nomeTurma;
            var $checkBoxes = document.getElementsByName("disciplina");

            for (var i = 0, len = $checkBoxes.length; i < len; i++) {
                $checkBoxes[i].checked = false;
                $checkBoxes[i].removeAttribute("Estado");

                for (var j = 0, len2 = idsDisciplina.length; j < len2; j++) {
                    if ($checkBoxes[i].value == idsDisciplina[j]) {
                        $checkBoxes[i].checked = true;
                        $checkBoxes[i].setAttribute("Estado", EstadoObjeto.Consultado);
                    }
                }
            }
        };

        /**
        * @descrição: Salva todos os dados do form
        **/
        var _salvar = function (win) {
            if (!Prion.ValidateForm("frmDisciplinaTurma")) {
                win.config.reloadObserver = false;
                return false;
            }

            if (win != null) { win.mask(); }

            //Seleciona o DOM de todos os checkboxes
            var $checkBoxes = document.getElementsByName("disciplina");

            if ($checkBoxes == null || $checkBoxes.length === 0) { return false; }

            //Monta um array com os Ids dos checkbox selecionados
            var arrayIds = [];
            for (var i = 0, len = $checkBoxes.length; i < len; i++) {
                var estado = $checkBoxes[i].getAttribute("Estado");
                if (($checkBoxes[i].checked === true) || (estado != null)) {
                    arrayIds.push({ "Id": $checkBoxes[i].value, "Estado": estado });
                }
            }

            Prion.Request({
                form: "frmDisciplinaTurma",
                url: rootDir + "DisciplinaTurma/Salvar",
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

            if ((estado == null) && ($checkBox.checked === true)) {
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

    NotaAzul.DisciplinaTurma.Dados.Iniciar();
} (window, document));