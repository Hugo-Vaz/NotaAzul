/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Funcionario == null) {
        NotaAzul.Funcionario = {};
    }

    NotaAzul.Funcionario.Dados = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/

        var _arrTelefone = [];

        /**
        * @descrição: Carrega os combobox desta janela
        * @return: void
        **/
        var _carregarCombobox = function () {
            NotaAzul.Cargo.ComboBox.Gerar({ filter: "Situacao.Nome=Ativo" });
        };

        /**
        * @descrição: Método que será chamado sempre após Salvar
        **/
        var _executarAposSalvar = function (retorno) {
            NotaAzul.Funcionario.Dados.Novo();
            return false;
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            _carregarCombobox();
            _arrTelefone = null;
            Prion.Telefone.Iniciar();
        };

        /**
        * @descrição: Reseta o form, voltando os dados default
        **/
        var _novoFuncionario = function () {
            Prion.ClearForm("frmFuncionario", true);
            Prion.Telefone.RemoverFormulariosTelefone();
            _arrTelefone = null;
        };

        /**
        * @descrição: Salva todos os dados do form
        **/
        var _salvar = function (win) {
            if (!Prion.ValidateForm("frmFuncionario")) {
                win.config.reloadObserver = false;
                return false;
            }

            if (win != null) { win.mask(); }

            _salvarTelefone();

            var paramReplace = [
                    { de: "TipoTelefone{0}", para: "TipoTelefone" },
                    { de: "Preferencial{0}", para: "Preferencial" }
                ];
            var dataTelefone = Prion.ArrayToJson(_arrTelefone, "Telefone", "Funcionario.Telefones[{0}]", paramReplace);

            Prion.Request({
                form: "frmFuncionario",
                url: rootDir + "Funcionario/Salvar",
                data: dataTelefone,
                success: function (retorno, registroNovo) {
                    // se for um registro existente, fecha a janela
                    if ((!registroNovo) && (win != null)) {
                        win.config.reloadObserver = true;
                        win.hide({ animate: true });
                        return;
                    }

                    NotaAzul.Funcionario.Dados.ExecutarAposSalvar();
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

        /**
        * @descrição: Salva em um array os objetos de telefone
        **/
        var _salvarTelefone = function () {
            if (_arrTelefone == null) {
                _arrTelefone = [];
            }
            var objTelefone = {};

            for (var i = 0; i < "divTelefone".getDom().children.length - 1; i++) {
                objTelefone = Prion.FormToObject("formTelefone" + i);
                if (objTelefone.EstadoObjeto == "2") {
                    //Nada precisa ser executado
                }
                else if (("idTelefone" + i).getValue() !== "0" || ("idTelefone" + i).getValue() !== 0) {
                    objTelefone.EstadoObjeto = EstadoObjeto.Alterado;
                }
                else {
                    objTelefone.EstadoObjeto = EstadoObjeto.Novo;
                    objTelefone.Id = '0';
                }
                _arrTelefone.push({ "Telefone": objTelefone });
            }
        };

        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            ExecutarAposSalvar: _executarAposSalvar,
            Iniciar: _iniciar,
            Novo: _novoFuncionario,
            Salvar: _salvar
        };
    } ();
    NotaAzul.Funcionario.Dados.Iniciar();
} (window, document));

