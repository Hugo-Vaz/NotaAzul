/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Aluno.Responsavel == null) {
        NotaAzul.Aluno.Responsavel = {};
    }

    NotaAzul.Aluno.Responsavel.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null;

        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @params: indiceObjeto => inteiro que representa o índice deste objeto na lista de Responsáveis
        * @return: void
        **/
        var _abrir = function (obj, indiceObjeto) {
            var runDependence = {
                IdCidade: {
                    type: "select", // indica o tipo do campo
                    getField: "IdBairro", // indica qual campo extra deve ser recuperado do objeto
                    // @param itemSelected => object contendo o atributo 'value'. Este atributo terá o valor que será setado em IdCidade
                    before: function (itemSelected) {

                        var cfgAction = {
                            action: {
                                complete: function () {
                                    NotaAzul.Aluno.Responsavel.Dados.CarregarBairro("responsavelListaCidades", itemSelected);
                                }
                            }
                        };

                        // passa como parâmetro o id do elemento 'responsavelListaEstados' e um objeto com o IdCidade
                        // o elemento 'responsavelListaEstados' será utilizado para obter o estado selecionado
                        // o objeto 'itemSelected' será utililizado para selecionar o item após o load das cidades
                        NotaAzul.Aluno.Responsavel.Dados.CarregarCidade("responsavelListaEstados", itemSelected, cfgAction);
                    }
                }
            };

            Prion.ObjectToForm("frmAlunoResponsavel", "", obj);
            Prion.RadioButton.SelectByValue("MoraCom", obj.MoraCom);
            Prion.RadioButton.SelectByValue("Financeiro", obj.Financeiro);
            Prion.ObjectToForm("frmResponsavelEndereco", "", obj.Endereco, runDependence);
            Prion.Telefone.AplicarDadosAoForm(obj.Telefones);

            // define o objeto de responsável
            NotaAzul.Aluno.Responsavel.Dados.Responsavel({
                obj: obj, // objeto selecionado na lista
                indice: indiceObjeto // índice deste objeto na lista de Responsáveis
            });
        };

        /**
        * @descrição: Cria todo o Html da lista
        * @return: void
        **/
        var _criarLista = function () {
            if (_grid != null) { return; }

            _grid = new Prion.Lista({
                id: "listaAlunoResponsaveis",
                height: 160,
                width: 686,
                style: "margin-left:5px",
                title: { text: "Responsáveis" },
                type: Prion.Lista.Type.Local,
                filter: { Entidades: "Telefone" },
                paging: { show: false },
                buttons: {
                    update: { show: true, click: NotaAzul.Aluno.Responsavel.Lista.Abrir },
                    remove: { show: true, url: rootDir + "Aluno/ResponsavelExcluir/" }
                },
                action: { onDblClick: NotaAzul.Aluno.Responsavel.Lista.Abrir },
                columns: [
                    { header: "Responsável", nameJson: "Nome" },
                    { header: "Grau de Parentesco", nameJson: "GrauParentesco", width: "100px" },
                    { header: "CPF", nameJson: "CPF", mask: "cpf", width: "100px" }
                ],
                rowNumber: {
                    show: false
                }
            });

            _grid.load();
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
            _criarLista();
        };

        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            Iniciar: _criarLista,
            Abrir: _abrir,
            Grid: _getGrid
        };
    } ();

    NotaAzul.Aluno.Responsavel.Lista.Iniciar();
} (window, document));