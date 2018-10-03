/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Aluno.Responsavel == null) {
        NotaAzul.Aluno.Responsavel = {};
    }

    NotaAzul.Aluno.Responsavel.Dados = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/

        // array com os responsáveis
        var _arrResponsavel = [],
            _arrTelefone = [];

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
                el: "responsavelListaCidades",
                filter: "Paginar=false&IdEstado=" + itemEstado.value,
                clear: true,
                valueDefault: true,
                selected: itemSelected
            });
        };

        /**
        * @descrição: Carrega os combobox desta janela
        * @return: void
        **/
        var _carregarCombobox = function () {
            "responsavelListaCidades".clear();
            "responsavelListaBairros".clear();

            NotaAzul.GrauParentesco.ComboBox.Gerar({ id: "responsavelGrauParentesco" });
            NotaAzul.Estado.ComboBox.Gerar({ id: "responsavelListaEstados" });

            NotaAzul.Cidade.ComboBox.Gerar({
                id: "responsavelListaCidades",
                autoLoad: false,
                idOrigem: "responsavelListaEstados", // id do elemento de onde vai pegar os dados
                observer: {
                    // o que será feito antes de carregar os registros de cidade
                    beforeLoad: function () {
                        "responsavelListaBairros".clear();
                    },

                    // o que será feito após carregar os registros de cidade
                    afterLoad: function () {
                        var idOrigem = "responsavelListaCidades";
                        var idDestino = "responsavelListaBairros";

                        NotaAzul.Bairro.ComboBox.Load(idOrigem, idDestino);
                    }
                }
            });

            NotaAzul.Bairro.ComboBox.Gerar({
                id: "responsavelListaBairros",
                autoLoad: false,
                idOrigem: "responsavelListaCidades" //id do elemento de onde vai pegar os dados
            });
        };

        /**
        * @descrição: Carrega os registros de bairro baseado na cidade escolhida
        * @params: select => objeto do tipo combobox representando o responsavelListaCidades
        * @return: void
        **/
        var _carregarBairro = function (select) {
            if (select == null) { return; }

            var itemCidade = Prion.ComboBox.Get(select);

            Prion.ComboBox.Carregar({
                url: rootDir + "Bairro/GetLista",
                el: "responsavelListaBairros",
                filter: "Paginar=false&IdCidade=" + itemCidade.value,
                clear: true,
                valueDefault: true
            });
        };

        /**
        * @descrição: Define ação para todos os controles da tela 
        **/
        var _definirAcaoBotoes = function () {

            // evento do botão Salvar da aba Responsável
            $("#btnAlunoResponsavelSalvar").click(function () {
                if (!Prion.ValidateForm("frmAlunoResponsavel") || !Prion.ValidateForm("frmResponsavelEndereco")) {
                    return false;
                }
                return NotaAzul.Aluno.Responsavel.Dados.Salvar(this);
            });

            // evento do botão Cancelar da aba Responsável
            $("#btnAlunoResponsavelCancelar").click(function () {
                Prion.Telefone.RemoverFormulariosTelefone();
                NotaAzul.Aluno.Responsavel.Dados.Novo(this);

                return false;
            });

            // evento associado ao CHANGE do combobox de ESTADOS
            Prion.Event.add("responsavelListaEstados", "change", function () {
                NotaAzul.Aluno.Responsavel.Dados.CarregarCidade(this);
            });

            // evento associado ao CHANGE do combobox de CIDADES
            Prion.Event.add("responsavelListaCidades", "change", function () {
                NotaAzul.Aluno.Responsavel.Dados.CarregarBairro(this);
            });

        };

        /**
        * @descrição: Método que será chamado sempre após salvar
        * @return: boolean 
        **/
        var _executarAposSalvar = function (retorno) {
            NotaAzul.Aluno.Responsavel.Dados.Novo();
            return false;
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            _definirAcaoBotoes();
            Prion.Telefone.Iniciar();
            _carregarCombobox();
        };

        /**
        * @descrição: Reseta o form, voltando os dados default
        **/
        var _novoResponsavel = function () {
            Prion.ClearForm("frmAlunoResponsavel");
            Prion.ClearForm("frmResponsavelEndereco");
            NotaAzul.Aluno.Responsavel.Dados.objResponsavel = null;
            Prion.RadioButton.SelectByValue("MoraCom", "0");
            Prion.RadioButton.SelectByValue("Financeiro", "0");
            Prion.Telefone.RemoverFormulariosTelefone();
            _arrTelefone = null;
        };

        /**
        * @descrição: recebe o objeto do responsável que esta sendo editado no momento
        * @return: void
        **/
        var _responsavel = function (obj) {
            this.objResponsavel = obj;
        };


        /**
        * @descrição: Adiciona os dados do form no grid de responsáveis
        * @return: boolean
        **/
        var _salvar = function (el) {
            if (!Prion.ValidateForm("frmAlunoResponsavel")) { return false; }

            // obtém os dados do responsável
            var objForm = Prion.FormToObject("frmAlunoResponsavel");
            objForm.Endereco = Prion.FormToObject("frmResponsavelEndereco");

            NotaAzul.Aluno.Responsavel.Dados.SalvarTelefone();
            objForm.Telefones = _arrTelefone;

            // verifica se o 'this.objResponsavel' existe
            if (this.objResponsavel != null) {

                // se existir, significa que o Responsável esta sendo alterado
                // atribui então os dados do form à este objeto e marca como alterado
                this.objResponsavel.obj = objForm;
                this.objResponsavel.obj.EstadoObjeto = EstadoObjeto.Alterado;
                this.objResponsavel.obj.Endereco.EstadoObjeto = EstadoObjeto.Alterado;

                // atualiza o objeto de responsável de acordo com o seu índice na lista de objetos
                NotaAzul.Aluno.Responsavel.Lista.Grid().modifyRow(this.objResponsavel.indice, this.objResponsavel.obj);

                this.Novo();
                return false;
            }

            // diz que o objeto de responsável é um objeto novo
            objForm.Id = 0;
            objForm.EstadoObjeto = EstadoObjeto.Novo;
            objForm.IdSituacao = parseInt(objForm.IdSituacao,10);

            objForm.Endereco.Id = 0;
            objForm.Endereco.EstadoObjeto = EstadoObjeto.Novo;
            objForm.Endereco.IdSituacao = parseInt(objForm.Endereco.IdSituacao, 10);
            objForm.Endereco.IdEstado = parseInt(objForm.Endereco.IdEstado, 10);
            objForm.Endereco.IdCidade = parseInt(objForm.Endereco.IdCidade, 10);
            objForm.Endereco.IdBairro = parseInt(objForm.Endereco.IdBairro, 10);

            for (var i = 0; i < objForm.Telefones.length; i++) {
                objForm.Telefones[i].EstadoObjeto = EstadoObjeto.Novo;
            }

            // adiciona na lista o objeto de responsável
            NotaAzul.Aluno.Responsavel.Lista.Grid().addRow(objForm);

            this.Novo();
            return false;
        };

        /**
        * @descrição: Cria um objeto com os dados do telefone inseridos no formulário
        **/
        var _salvarTelefone = function () {
            if (_arrTelefone == null) {
                _arrTelefone = [];
            }

            for (var i = 0; i < "divTelefone".getDom().children.length - 1; i++) {
                var objTelefone = Prion.FormToObject("formTelefone" + i);

                //Verifica se já existe um objeto responsável
                if (NotaAzul.Aluno.Responsavel.Dados.objResponsavel != null) {
                    if (objTelefone.EstadoObjeto == "2") {/*Nada precisa ser executado*/}
                    else if (NotaAzul.Aluno.Responsavel.Dados.objResponsavel.obj != null && _arrTelefone.length >= NotaAzul.Aluno.Responsavel.Dados.objResponsavel.obj.Telefones.length) {
                        objTelefone.EstadoObjeto = EstadoObjeto.Novo;
                        objTelefone.Id = "0";
                    } else {
                        objTelefone.EstadoObjeto = EstadoObjeto.Alterado;
                    }
                } else {
                    //Caso não exista seu id será "setado" como 0, se tornando responsabilidade do BD inserir seu valor
                    objTelefone.Id = "0";
                }

                _arrTelefone.push(objTelefone);
            }
        };

        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            Iniciar: _iniciar,
            CarregarBairro: _carregarBairro,
            CarregarCidade: _carregarCidade,
            ExecutarAposSalvar: _executarAposSalvar,
            Novo: _novoResponsavel,
            Responsavel: _responsavel,
            Salvar: _salvar,
            SalvarTelefone: _salvarTelefone
        };
    } ();

    NotaAzul.Aluno.Responsavel.Dados.Iniciar();
} (window, document));