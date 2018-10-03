/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        diaPagamento = diaPagamento || "",
        listaCartao = listaCartao || {},
        listaParentesco = listaParentesco || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.ConfiguracaoSistema == null) {
        NotaAzul.ConfiguracaoSistema = {};
    }

    NotaAzul.ConfiguracaoSistema.Configuracao = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
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
                el: "listaCidades",
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
        var _carregarComboBox = function () {
            NotaAzul.Estado.ComboBox.Gerar({ id: "listaEstados" });
            NotaAzul.BandeiraCartao.ComboBox.Gerar({ id: "listaBandeiraCartao" });
            NotaAzul.GrauParentesco.ComboBox.Gerar({ id: "listaParentesco" });

            "listaBandeiraCartao".getDom().parentNode.style.marginLeft = "160px";
            "listaParentesco".getDom().parentNode.style.marginLeft = "160px";

            "listaCidades".clear();
            NotaAzul.Cidade.ComboBox.Gerar({
                id: "listaCidades",
                autoLoad: false,
                idOrigem: "listaEstados" // id do elemento de onde vai pegar os dados             
            });
        };

        /**
        * @descrição: Adiciona uma nova opção ao select
        * @param:id do Select
        * @param:id do input 
        * @return: valor= valor da opção recem inserida
        **/
        var _adicionarNovaOpcao = function (idSelect, idInput) {
            //Pega o valor da nova opção a ser inserida
            var valor = idInput.getDom().value,
                    option = document.createElement("option");

            if (valor === "" || valor == null) { return false; }

            option.value = valor.toString().toLowerCase();
            option.innerHTML = valor;

            //Adiciona a nova opção ao Select
            idSelect.getDom().add(option);
            idInput.getDom().value = "";

            return valor;
        };

        /**
        * @descrição: Remove uma  opção do select
        * @param:id do Select        
        * @return: valor= valor da opção removida
        **/
        var _removerOpcao = function (idSelect) {
            //Pega o valor da nova opção a ser inserida
            var valor = Prion.ComboBox.Get(idSelect).value;
            if (valor === "" || valor == null) { return false; }

            var option = document.querySelectorAll("#" + idSelect + " option[value=" + valor + "]")[0];
            if (option == null) { return false; }

            if (window.confirm("A opção selecionada será removida.Deseja continuar?") === false) { return; }

            option.parentNode.removeChild(option);
            Prion.ComboBox.SelecionarIndexPeloValue(idSelect, "");

            return valor;
        };

        /**
        * @descrição: Define a ação para todos os controles da tela
        * @return: void
        **/
        var _definirAcoesBotoes = function () {
            // evento associado ao CHANGE do combobox de ESTADOS
            Prion.Event.add("listaEstados", "change", function () {
                NotaAzul.Configuracao.CarregarCidade(this);
            });

            //evento associado ao botão de novo Cartão
            Prion.Event.add("addCartao", "click", function () {
                listaCartao[listaCartao.length] = _adicionarNovaOpcao("listaBandeiraCartao", "novoCartao");
            });

            //evento associado ao botão de novo Grau de parentesco
            Prion.Event.add("addGrau", "click", function () {
                listaParentesco[listaParentesco.length] = _adicionarNovaOpcao("listaParentesco", "novoGrau");
            });

            //evento associado ao botão de remover Cartão
            Prion.Event.add("removeCartao", "click", function () {
                var valor = _removerOpcao("listaBandeiraCartao");
                for (var i = 0; i < listaCartao.length; i++) {
                    if (listaCartao[i].trim() == valor) {
                        listaCartao.splice(i, 1);
                    }
                }
            });

            //evento associado ao botão de remover Grau de parentesco
            Prion.Event.add("removeGrau", "click", function () {
                var valor = _removerOpcao("listaParentesco");
                for (var i = 0; i < listaParentesco.length; i++) {
                    if (listaParentesco[i].trim() === valor) {
                        listaParentesco.splice(i, 1);
                    }
                }
            });

            //evento associado ao botão de salvar as configurações
            Prion.Event.add("btnSalvarConfig", "click", function () {
                NotaAzul.ConfiguracaoSistema.Configuracao.Salvar();
            });
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            _carregarComboBox();
            _definirAcoesBotoes();
            "DiaPagamento".setValue(diaPagamento);
        };

        /**
        * @descrição: Salva todos os dados do form
        **/
        var _salvar = function (win) {
            if (!Prion.ValidateForm("frmConfigSistema")) {
                win.config.reloadObserver = false;
                return false;
            }

            Prion.Request({
                form: "frmConfigSistema",
                url: rootDir + "ConfiguracaoSistema/Salvar",
                data: "ListaParentesco=" + listaParentesco + "&ListaCartao=" + listaCartao,
                success: function (retorno, registroNovo) {
                    // se for um registro existente, fecha a janela
                    if ((!registroNovo) && (win != null)) {
                        win.config.reloadObserver = true;
                        win.hide({ animate: true });
                        return;
                    }

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
            CarregarCidade: _carregarCidade,
            Iniciar: _iniciar,
            Salvar:_salvar
        };
    } ();
    NotaAzul.ConfiguracaoSistema.Configuracao.Iniciar();
} (window, document));