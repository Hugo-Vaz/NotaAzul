"use strict";
Prion.Telefone = {

    /* /**
    * @descrição: Define o evento a ser utilizado pelo Botão remover telefone
    **/
    AcaoBotaoRemover: function (/*indice*/) {

        var indice = arguments[0];

        if (indice != null) {
            var arrButtonsRemover = Array.fromList(document.querySelectorAll("#formTelefone" + indice + " [name=removerTelefone]"));
        } else {
            // obtém todos os botões com o name = 'removerTelefone'
            var arrButtonsRemover = Array.fromList(document.getElementsByName("removerTelefone"));
        }

        Prion.Event.add(arrButtonsRemover, "click", function () {
            if (confirm("Tem certeza que deseja apagar este telefone?") == false) {
                return;
            }

            var divTelefone = this.parentNode.parentNode,
                indiceTelefone = divTelefone.getAttribute("indicetelefone");

            if (indiceTelefone == 0) {
                // marca o telefone como excluído
                var estadoObjetoAtual = document.getElementById("telefoneEstado" + indiceTelefone);
                estadoObjetoAtual.value = EstadoObjeto.Excluido;
            }

            // se for o primeiro telefone, limpa o html e mantém o formulário
            if (indiceTelefone == 0) {
                Prion.ClearForm("formTelefone0");
                Prion.RadioButton.SelectByValue("Preferencial0", "true")
                return;
            }

            // remove a div de telefone
            var form = ("formTelefone" + indiceTelefone).getDom();
            if (form != null && ("Telefone_Numero" + indice).getValue() != "") {
                form.style.display = "none";
            } else {
                form.parentNode.removeChild(form);
            }
        });
    },

    /**
    * @descrição: Aplica ao formulario os dados do objeto
    * @parametros:um array de objetos de telefone   
    **/
    AplicarDadosAoForm: function (arrObjTelefone) {
        if (arrObjTelefone != null && arrObjTelefone.length != 0) {
            // adiciona o primeiro objeto de telefone ao formulário já existente
            Prion.ObjectToForm("formTelefone0", "", arrObjTelefone[0]);
            Prion.RadioButton.SelectByValue("TipoTelefone0", arrObjTelefone[0].TipoTelefone);
            Prion.RadioButton.SelectByValue("Preferencial0", (arrObjTelefone[0].Preferencial) ? "true" : "false");

            // varre a lista de telefones para criar os campos HTML
            for (var i = 1; i < arrObjTelefone.length; i++) {
                Prion.Telefone.ClonarHtmlTelefone(arrObjTelefone[i]);
            }

            return;
        }

        "idTelefone0".setValue(0);
        "telefoneEstado0".setValue(0);
    },

    /**
    * @descrição: Cria todo o html relacionado aos campos de telefone
    **/
    ClonarHtmlTelefone: function (/*objTelefone*/) {
        var domDivTelefone = "divTelefone".getDom();

        if (domDivTelefone == null) {
            return;
        }

        var indice = domDivTelefone.children.length - 1,
            objTelefone = arguments[0],
            divTelefoneClonado = "formTelefone0".getDom().cloneNode(true);

        divTelefoneClonado.id = "formTelefone" + indice;
        divTelefoneClonado.setAttribute("indicetelefone", indice);

        //Realiza as alterações nas propriedades "name" e "id" de acordo com o seu type
        var inputTelefone = divTelefoneClonado.getElementsByTagName("input");

        for (var i = 0; i < inputTelefone.length; i++) {
            if (inputTelefone[i].type == "hidden") {
                var id = (inputTelefone[i].id.slice(0, -1)) + indice;
                inputTelefone[i].id = id;
            } else {
                inputTelefone[i].id += indice;
            }

            if (inputTelefone[i].type == "radio") {
                var nameRadio = (inputTelefone[i].name.slice(0, -1)) + indice;
                inputTelefone[i].name = nameRadio;
                inputTelefone[i].checked = "false";

                var elementRadio = inputTelefone[i];
                var labelRadio = inputTelefone[i].parentNode;

                while (labelRadio.hasChildNodes() && labelRadio.tagName != "LABEL") {
                    labelRadio = labelRadio.parentNode;
                }

                labelRadio.removeChild(labelRadio.firstElementChild);
                labelRadio.appendChild(elementRadio);
            } else if (inputTelefone[i].type != "button") {
                inputTelefone[i].value = (objTelefone != null) ? objTelefone[inputTelefone[i].name] : "";
            }
        }

        domDivTelefone.appendChild(divTelefoneClonado);
        $("#formTelefone" + indice + " .uniform").uniform();

        //Seleciona o RadioButton de acordo com o valor do objeto
        if (objTelefone != null) {
            Prion.RadioButton.SelectByValue("TipoTelefone" + indice, objTelefone["TipoTelefone"]);
            Prion.RadioButton.SelectByValue("Preferencial" + indice, (objTelefone["Preferencial"]) ? "true" : "false");
        }

        // evento associado ao CLICK dos radioButtons de TelefonePreferencial
        var arrInputPrincipal = Array.fromList(document.querySelectorAll("#formTelefone" + indice + " [telefonepreferencial]"));

        Prion.Event.add(arrInputPrincipal, "click", function () {
            var nome = this.name;
            if (this.value.toLowerCase() == "false") {
                return;
            }

            Prion.each("#divTelefone [telefonepreferencial]", function (item) {
                if (nome != item.name) {
                    Prion.RadioButton.SelectByValue(item.name, "false");
                }
            });
        });

        Prion.Telefone.AcaoBotaoRemover(indice);
    },

    /**
    * @descrição: Define as ações dos botões de telefone
    **/
    DefinirAcaoBotoes: function () {
        //evento do botão adicionar novo telefone
        Prion.Event.add("btnNovoTelefone", "click", function () {
            //O valor do índice corresponde a quantidade de filhas de diVTelefone sem contar a divTitulo
            var indice = "divTelefone".getDom().children.length - 2;

            //Corresponde ao último formulário de telefone inserido
            var form = "formTelefone" + indice;

            if (("Telefone_DDD").getValue() != "" && ("Telefone_Numero").getValue() != "" && indice == 0 ||
            ("Telefone_DDD").getValue() != "" && ("Telefone_Numero").getValue() != "" && form.getDom().style.display == "none" ||
            ("Telefone_DDD" + indice).getValue() != "" && ("Telefone_Numero" + indice).getValue() != "") {

                Prion.Telefone.ClonarHtmlTelefone();
                return;
            }

            alert("Por favor, preencha o formulário atual antes de adicionar outro telefone.");
        });

        //evento do botão remover telefone
        Prion.Telefone.AcaoBotaoRemover();
    },


    /**
    * @descrição: Função chamada ao carregar  o script
    **/
    Iniciar: function () {
        Prion.RadioButton.SelectByValue("Preferencial0", "true");
        Prion.RadioButton.SelectByValue("TipoTelefone0", "Residencial");
        Prion.Telefone.DefinirAcaoBotoes();
    },

    /**
    * @descrição: Remove todos os formulários de telefone
    **/
    RemoverFormulariosTelefone: function () {
        // pega todos os elementos que tenham o name="formTelefone"
        var frmTelefone = document.getElementsByName("formTelefone");

        if (frmTelefone == null) {
            return;
        }

        // remove todos as DIV de telefone, mantendo apenas a primeira
        // importante remover de trás pra frente
        for (var i = frmTelefone.length - 1; i > 0; --i) {
            frmTelefone[i].parentNode.removeChild(frmTelefone[i]);
        }

        Prion.ClearForm("#formTelefone0");
        Prion.RadioButton.SelectByValue("Preferencial0", "true");
        Prion.RadioButton.SelectByValue("TipoTelefone0", "Residencial");
    }
};