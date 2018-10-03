/**
* @autor: Thiago Motta Zappaterra
* @email: tmottaz@gmail.com
* @descrição: Classe responsável por criação de lista
* =================================
* @param: configUser (object): objeto com parâmetros de configuração da lista
* @param: config2 (object, opcional): objeto com parâmetros extras
*
* @versão: 1.55 - data: 16/12/2013
**/

(function () {
    "use strict";


    var dcTime = 250,    // doubleclick time
        dcDelay = 100,   // no clicks after doubleclick
        dcAt = 0,        // time of doubleclick
        savEvent = null, // save Event for handling doClick().
        savEvtTime = 0,  // save time of click event.
        savTO = null;    // handle of click setTimeOut

    function hadDoubleClick() {
        var d = new Date(),
            now = d.getTime();
        //console.log("Checking DC (" + now + " - " + dcAt);

        if ((now - dcAt) < dcDelay) {
            //console.log("*hadDC*");
            return true;
        }

        //console.log(" OK ");
        return false;
    };


    function handleWisely(which, config, fn) {
        //console.log(which + " fired...");

        switch (which) {
            case "click":

                // If we've just had a doubleclick then ignore it
                if (hadDoubleClick()) {
                    return false;
                }

                // Otherwise set timer to act.  It may be preempted by a doubleclick.
                var savEvent = which,
                    callback = fn,
                    objConfig = config;

                var d = new Date();
                savEvtTime = d.getTime();
                savTO = setTimeout(function () {
                    doClick(savEvent, callback, objConfig);
                }, dcTime);

                break;
            case "dblclick":
                doDoubleClick(which, fn, config);
                break;
            default:
        }
    };

    function doClick(which, callback, config) {
        // preempt if DC occurred after original click.
        if (savEvtTime - dcAt <= 0) {
            //console.log("ignore Click");
            return false;
        }

        //console.log("Handle Click.  ");
        if (callback != null) {
            callback.call(config);
        }
    };

    function doDoubleClick(which, fn, config) {
        var d = new Date();
        dcAt = d.getTime();

        if (savTO != null) {
            clearTimeout(savTO); // Clear pending Click  
            savTO = null;
        }

        //console.log("Handle DoubleClick at " + dcAt);
        if (fn != null) {
            fn.call(config);
        }
    };




    Prion.Lista = function (configUser/**, config2*/) {
        "use strict";

        var _load = null, // método que será utilizado para a paginação. NÃO REMOVER!!!!
            _mask = null, // objeto em cache da máscara. NÃO REMOVER!!!!
            _divMaskMsg = null, // objeto em cache da mensagem da máscara. NÃO REMOVER!!!!
            _checkboxHeader = null, // objeto em cache do checkbox do header. NÃO REMOVER!!!!
            _configLoad = null, // obtém em cache da configuração atual de um load. NÃO REMOVER!!!!
            _sortAtual = null, // guarda em cache o sort atual. NÃO REMOVER!!!!
            _thead = null, // guarda em cache o thead da tabela. NÃO REMOVER!!!!
            _tbody = null, // guarda em cache o tbody da tabela. NÃO REMOVER!!!!
            _buttonUpdate = null, // guarda em cache o botão utilizado para alterar um registro. NÃO REMOVER!!!!
            _buttonRemove = null, // guarda em cache o botão utilizado para remover as linhas. NÃO REMOVER!!!!
            _buttonsPaging = { first: null, previous: null, next: null, last: null }; // guarda em cache os botões da paginação. NÃO REMOVER

        var _configDefault = {
            autoLoad: true, // indica que os registros já serão carregados automaticamente assim que a lista for criada. ATENÇÃO: este parâmetro tem um funcionamento diferente do requisicao.autoLoad
            url: "", // url utilizado na requisição Ajax
            id: "", // id do elemento para onde a requisição será jogada
            width: "100%", // width default da table. Pode ser inteiro (que irá representar PX) ou string (que irá representar %. Formato: 100% por exemplo)
            height: 500, // height default da table. Pode ser inteiro (que irá representar PX) ou string (que irá representar %. Formato: 100% por exemplo)
            popup: false, // por default a lista não será gerada em uma popup
            action: {
                onDblClick: null // function que será executada quando o usuário clicar 2x em uma linha do body
            },
            style: "", // style que será aplicado à lista
            type: Prion.Lista.Type.Remote, // indica que a vai trabalhar remotamente, ou seja, faz requisições ao servidor
            useTheme: false,
            collapsed: null,
            buttons: {
                insert: {
                    show: false, // indica que existirá um botão INSERT
                    click: null, // function que será executada quando o usuário clicar no botão
                    //fnBefore: null, // function que será executada antes de exibir o registro
                    //fnAfter: null // function que será executada após clicar em INSERT
                    img: {
                        src: imgPathPrion + "/new.png"
                    },
                    tooltip: "Novo"
                },
                update: {
                    show: false, // indica que existirá um botão de UPDATE
                    click: null, // function que será executada quando o usuário clicar no botão
                    //fnBefore: null, // function que será executada antes de exibir o registro
                    //fnAfter: null // function que será executada após clicar em UPDATE
                    img: {
                        src: imgPathPrion + "/edit.png"
                    },
                    tooltip: "Alterar"
                },
                remove: {
                    click: null, // function que será executada quando o usuário clicar no botão
                    confirm: true, // indica que pedirá confirmação de exclusão para o usuário
                    confirmMessage: "Tem certeza que deseja excluir o(s) registro(s) selecionado(s)?", // mensagem de confirmação de exclusão
                    show: false, // indica que existirá um botão de DELETE (excluir registros)
                    //fnBefore: null, // function que será executada antes de excluir os registros
                    //fnAfter: null, // function que será executada após excluir os registros
                    url: "", // url chamada para excluir os registros
                    fields: ["Id"], // array de campos que serão utilizados para excluir um registro
                    img: {
                        src: imgPathPrion + "/delete.png"
                    },
                    tooltip: "Excluir"
                },
                close: {
                    img: rootDir + "Content/cupcake/images/icons/shortcut/close.png"
                }
            },
            checkbox: {
                show: true, // indica se irá exibir um checkbox para cada linha
                classLineSelected: "" // indica o class que será atribuído (caso exista) quando a linha for selecionada.
            },
            rowNumber: {
                show: true, // indica se irá exibir uma coluna com o número da linha
                className: "" // className que será aplicado ao rowNumber
            },
            mask: {
                imgLoad: rootDir + "Content/prion/loading.gif",
                show: true, // indica se irá ou não exibir o máscara de carregando
                text: "Aguarde, carregando...", // mensagem que será exibida enquanto os registros são carregados
                className: "maskList" // className que será aplicado à máscara
            },
            paging: {
                show: true, // indica se irá exibir a paginação.
                totalPerPage: 20, // total de registros por página
                currentPage: 1, // página atual
                textPage: "Página",
                textOf: "de",
                showDescription: true,
                description: "Exibindo {0} de {1} / {2} registros", // Exibindo 1 de 20 / 200
                nothing: "Nenhum registro para exibir",
                sort: {}, // objeto que representa a ordenação
                showOptions: true, // indica se irá exibe um combobox com novos valores por página
                options: [20, 50, 100, 200] // lista com novos valores para busca
            },
            request: {
                ajax: true, // indica se é feita requisição no servidor
                autoLoad: false, // indica que a lista será atualizada automaticamente baseado no parâmetro time
                base: "", // base do registro obtido
                returnType: Prion.Lista.ReturnType.String, // se for STRING, indica que será realizado um parseJson no resultado vindo da requisição
                time: (60 * 1000) // 60 segundos * 1000 milisegundos. Configuração time em milisegundos
            },
            search: {
                show: false, // indica se irá ou não exibir um painel para realizar busca nos registros
                width: "200px" // indica o tamanho default do textbox
            },
            table: {
                //className: "tablesorter",
                //classLineHover: "", // indica o nome do class que será atribuído quando o usuário passar o mouse em cima de uma determinada linha
                createFooter: false,
                columns: new Prion.Lista.Column(),
                header: new Prion.Lista.Header(),
                body: new Prion.Lista.Body(),
                footer: new Prion.Lista.Footer()
            },
            title: {
                show: true, // indica se irá ou não exibir o título do grid
                text: "", // título do grid
                height: 42,
                className: "title" // className que será aplicado ao titulo
            },
            zebrado: {
                show: true, // indica se irá fazer o efeito zebrado nas linhas
                classLineOdd: "", // class da linha par
                classLineEven: "" // class da linha ímpar
            }
        };


        // configurações default que deverão ser aplicadas à um botão desabilitado
        var _configButtonsDisabled = {
            className: "buttonDisabled"
        };




        var configUserFinal = {};


        // aplica as configurações default deste projeto, apenas se elas existirem
        if ((Prion.settings != null) && (Prion.settings.Lista)) {
            configUserFinal = Prion.Apply(_configDefault, Prion.settings.Lista);
        }


        // atribui as configurações do usuário ao objeto de configuração default
        configUserFinal = Prion.Apply(configUserFinal, configUser);


        // se config2 for != null
        // config2 é uma configuração extra, que pode ser passado depois do objeto Prion.Lista já ter sido criado
        if (arguments[1] != null) {
            configUserFinal = Prion.Apply(configUserFinal, arguments[1]);
        }


        configUserFinal.table.header.config.id = configUserFinal.id;
        configUserFinal.table.header.config.columns = configUserFinal.columns;
        configUserFinal.table.body.config.id = configUserFinal.id;
        configUserFinal.table.footer.config.id = configUserFinal.id;
        configUserFinal.table.footer.config.columns = configUserFinal.columns;





        /**
        * @descrição: Acerta os botões da paginação, habilitando/desabilitando de acordo com o número de registros/páginas
        * @return: void
        **/
        var _acertarBotoesPaginacao = function (retorno) {
            // verifica se o objeto existe
            if (retorno == null) { return; }

            // não existe registro, desabilita todos os botões menos o de refresh
            if (retorno.total == 0) {
                // desabilitar os botões
            }
        };


        /**
        * @descrição: acerta os elementos da paginação, caso ela exista
        * @param: retorno => objeto vindo da requisição AJAX
        **/
        var _acertarPaginacao = function (retorno) {

            // verifica se a paginação existe...
            if (!_configDefault.paging.show) {
                return;
            }

            // se não existir registro no tbody...
            if (_tbody.children.length == 0) {
                // ...reinicia a paginação
                if (retorno == null) { retorno = new Object(); }

                retorno.page = 1;
                retorno.total = 0;
            }

            // validação necessária
            if (retorno == null) {
                return;
            }


            // pega o elemento do número da página
            var numberPage = document.querySelectorAll("#" + _configDefault.id + " input[name='NumberPage']");

            if ((numberPage != null) && (numberPage.length >= 1)) {
                numberPage[0].value = retorno.page;
            }


            // pega o elemento que exibe o total de páginas
            var totalPerPage = document.getElementById(_configDefault.id + "_TotalPerPage");

            if (totalPerPage != null) {
                var t = (Math.ceil(retorno.total / _configDefault.paging.totalPerPage));
                totalPerPage.innerText = _configDefault.paging.textOf + " " + ((t == 0) ? "1" : t);
            }


            // pega o elemento que exibe a descrição
            if (_configDefault.paging.showDescription) {
                var description = document.getElementById(_configDefault.id + "_Description");

                if (description == null) {
                    return;
                }

                // desabilita os botões
                _acertarBotoesPaginacao(retorno);

                // verifica se existe algum registro
                if (retorno.total == 0) {
                    // exibe mensagem de que não existe nenhum registro para exibir
                    description.innerText = _configDefault.paging.nothing;
                    return;
                }

                var totalR;
                var p = (_configDefault.paging.currentPage == 1) ? 1 : (((_configDefault.paging.currentPage - 1) * _configDefault.paging.totalPerPage) + 1);

                if (retorno.rows == null) {
                    totalR = 0;
                } else if (_configDefault.paging.currentPage == 1) {
                    totalR = (retorno.rows.length);
                } else if (_configDefault.paging.currentPage == _configDefault.paging.totalPages) {
                    totalR = retorno.total;
                } else if (_configDefault.paging.currentPage > 1) {
                    totalR = (_configDefault.paging.currentPage - 1) * ((p - 1) + _configDefault.paging.totalPerPage);
                }


                description.innerText = _configDefault.paging.description.format(p, totalR, retorno.total); // "Exibindo 21 de 40 / 200 registros"
            }

        };

        /**
        * @descrição: adiciona uma linha na tabela
        * @param: lineNumber => número da linha que será exibido no HTML
        * @param: lineNumberDataId => número da linha que será atribuido ao atributo 'data-id' do TR
        * @param: row => array com todos os valores que serão adicionados
        * @param: setWidth => se for true, indica se deverá adicionar o width da coluna no html
        * @param: rowOdd => se for true, indica que é uma linha PAR (utilizado para fazer o zebrado)
        **/
        var _addRow = function (lineNumber, lineNumberDataId, row, setWidth, rowOdd) {

            var tr = document.createElement("tr");
            tr.setAttribute("dataId", lineNumberDataId);


            // verifica se é para exibir o número atual da linha
            if (_configDefault.rowNumber.show) {

                var divNumber = document.createElement("div");
                divNumber.style.width = "30px";
                divNumber.innerHTML = lineNumber;

                var td = document.createElement("td");
                td.style.cssText = "text-align:center; ";
                td.appendChild(divNumber);

                tr.appendChild(td);
            }
            // FIM


            // verifica se é para criar um checkbox
            if (_configDefault.checkbox.show) {

                var divCb = document.createElement("div");
                divCb.style.width = "30px";

                var td = document.createElement("td");
                td.style.cssText = "text-align:center; ";

                // cria o checkbox da linha
                var cb = document.createElement("input");
                cb.type = "checkbox";
                cb.checked = false;
                cb.name = "cb_" + _configDefault.id;

                if (Prion.settings.ClassName != null) {
                    if (_configDefault.useTheme) {
                        cb.className = Prion.settings.ClassName.checkbox;
                    }
                }

                // adiciona o evento de click para o checkbox
                Prion.Event.add(cb, "click", function () {

                    // obtém o total de checkbox
                    var allCB = document.querySelectorAll("#" + _configDefault.id + " tbody input[name='" + this.name + "']");
                    var totalChecked = 0;

                    // obtém o total de checkbox selecionados (checked=true)
                    for (var i = 0; i < allCB.length; i++) {
                        if (allCB[i].checked) { totalChecked += 1; }
                    }

                    // se o total selecionado for igual ao total de checkbox, define que o checkbox do header e do footer também serão marcados
                    var marcarHeaderFooter = (totalChecked == allCB.length);


                    // muda o estado também do checkbox do header
                    //var c = document.querySelectorAll("#" + _configDefault.id + " table thead input[type=checkbox]");
                    //if ((c[0] != null) && (c.length >= 1)) { c[0].checked = marcarHeaderFooter; }
                    _checkboxHeader.checked = marcarHeaderFooter;

                    // muda o estado também do checkbox do footer
                    if (_configDefault.table.createFooter) {
                        var c = document.querySelectorAll("#" + _configDefault.id + " table tfoot input[type=checkbox]");

                        if ((c[0] != null) && (c.length >= 1)) {
                            c[0].checked = marcarHeaderFooter;
                        }
                    }


                    // adiciona um evento para o click no tr
                    Prion.Log({ msg: "Prion.Lista.js => CLICK, selected", type: "info", from: showLog.Lista });

                    // obtém o TR deste checkbox
                    var tr = this.parentElement.parentElement.parentElement;
                    if (this.checked) {
                        Prion.removeClass(tr, "trHover");
                        Prion.addClass(tr, "lineSelected");
                    }
                    else {
                        Prion.removeClass(tr, "lineSelected");
                        Prion.addClass(tr, "trHover");
                    }


                    // modifica o estado do botão
                    _alterStateButton(_buttonRemove, totalChecked >= 1);
                });


                divCb.appendChild(cb);

                td.setAttribute("active-click", false);
                td.appendChild(divCb);

                tr.appendChild(td);
            }
            // FIM


            var header = document.querySelectorAll("#" + _configDefault.id + " table thead tr th");

            // obtém todas as colunas do header da tabela e cria a coluna
            for (var i = 0; i < header.length; i++) {

                // obtém o atributo 'namejson' que esta na TH da TR
                var nameJson = header[i].getAttribute("namejson");

                if (nameJson == null) { continue; }

                // cria um TD para o TR
                var td = _createCellBody(
                    _configDefault.table.header.get(nameJson), // obtém o objeto header relacionado ao nameJson
                    row,
                    rowOdd,
                    header[i],
                    setWidth
                );

                tr.appendChild(td);
            }
            // FIM DO FOR

            return tr;
        };


        /**
        * @descrição: ajusta o tamanho das colunas do header
        **/
        /*var _adjustWidthHeader = function () {
        // TEMP
        return;
    
        if ((_thead == null) || (0 == _thead.children.length) || (0 == _thead.children[0].length)) {
        return;
        }
    
        var tds = _thead.children[0].children;
        var i = -1;
    
        if (configUserFinal.rowNumber.show) {
        i += 1;
        tds[i].style.width = "30px";
        }
    
        if (configUserFinal.checkbox.show) {
        i += 1;
        tds[i].style.width = "30px";
        }
    
        i += 1;
        for (; i < tds.length; i++) {
        tds[i].style.width = tds[i].clientWidth.toString() + "px";
        tds[i].children[1].style.width = tds[i].clientWidth.toString() + "px";
        }
        };*/


        /**
        * @descrição: Calcula o height interno da tabela (apenas a parte do BODY)
        * @return: objeto com height e width
        **/
        var _calculateSizeInternal = function (configsTable) {
            var id = _configDefault.id,
                selector = document.querySelectorAll("#" + id + " div.title"),
                heightDiv = ((selector == null) || (selector.length == 0)) ? 0 : selector[0].parentElement.clientHeight, // height da div
                widthDiv = configsTable.title.width, // comentado em 24/09/2012 - TESTE ((selector == null) || (selector.length == 0)) ? 0 : selector[0].parentElement.clientWidth; // width da div
                height = (heightDiv + 1) - configsTable.title.height - configsTable.header.height;

            if (configsTable.paging != null) {
                height = height - configsTable.paging.height;
            }

            return {
                height: height,
                width: widthDiv
            };
        };


        /**
        * @descrição: Limpa todos os registros da tabela
        * @return: void
        **/
        var _clear = function () {
            var rows = _tbody.children;

            if ((rows == null) || (rows.length == 0)) {
                return;
            }

            // verifica se existe algum registro no tbody. Se existir, exclui todos para adicionar as novas linhas
            for (var i = rows.length; i > 0; i--) {
                _tbody.deleteRow(i - 1);
            }

            // gera um TR vazio
            _gerarTRVazio();

            // zera as linhas
            configUserFinal.table.body.rows = [];

            // desmarca o checkbox do header
            _checkboxHeader.checked = false;
            _checkboxHeader.disabled = true;
            Prion.addClass(_checkboxHeader, "cbDisabled");


            // desmarca o checkbox do footer
            // ??? UÉ, CADÊ ???


            // desabilita o botão de exclusão
            _alterStateButton(_buttonRemove, false);
        };


        /**
        * @descrição: cria os botões de ação dos registros da lista
        * @return: elements
        **/
        var _createButtons = function (elDivTitle, configTable) {

            // verifica se os botões INSERT, UPDATE E REMOVE são false
            if ((!configTable.buttons.insert.show) && (!configTable.buttons.update.show) && (!configTable.buttons.remove.show)) {
                return null;
            }

            // se um dos dois botões forem exibidos, cria a div de botões

            var divBotoes = document.createElement("div");
            divBotoes.className = "shortcuts-icons";


            // só cria o botão de REMOVE caso o usuário tenha informado que quer...
            if (configTable.buttons.remove.show) {

                if (Prion.Theme != null) {

                    // obtém a função de deletar que o usuário informou
                    var deleteUsuario = configTable.buttons.remove.click;


                    // função que será executada quando clicar no delete
                    var deleteClick = function () {
                        // obtém todos os registros selecionados
                        var selecteds = _getSelected();

                        // verifica se possui algum registro selecionado.
                        if ((selecteds == null) || (0 == selecteds.length)) {
                            return;
                        }

                        var arrSelecteds = [],
                            count = selecteds.length;

                        for (var j = 0; j < count; j++) {
                            arrSelecteds.push(selecteds[j].object);
                        }

                        var urlRemove = configTable.buttons.remove.url.trim();

                        // se não for para pedir confirmação, OK será sempre TRUE
                        var ok = !configTable.buttons.remove.confirm;

                        // se for para pedir confirmação...
                        if (configTable.buttons.remove.confirm) {
                            // pede confirmação
                            ok = confirm(configTable.buttons.remove.confirmMessage);
                        }

                        // se NÃO estiver OK, sai fora...
                        // só não será OK se estiver configurado para pedir confirmação e o usuário tiver cancelado
                        if (!ok) {
                            return;
                        }


                        // verifica se foi informado a URL da ação e se é uma lista remota
                        // Se não tiver sido informada, as linhas serão excluidas somente do lado do cliente
                        if ((urlRemove != "") && (configTable.type == Prion.Lista.Type.Remote)) {

                            // se chegou aqui é porque OK é true ou remove.confirm é false
                            var o = new Array(),
                                o2 = "";

                            for (var j = 0; j < configTable.buttons.remove.fields.length; j++) {
                                var at = configTable.buttons.remove.fields[j];

                                if (selecteds.length == 0) {
                                    continue;
                                }

                                o2 += at + "=";

                                // monta uma string com todos os fields selecionados
                                for (var i = 0; i < selecteds.length; i++) {
                                    var value = selecteds[i].object[configTable.buttons.remove.fields[j]];
                                    o2 += value;

                                    if (i < (selecteds.length - 1)) { o2 += ","; }
                                }
                            }

                            // exibe a máscara e muda o seu texto
                            _showMask("Excluindo, aguarde...");

                            // faz requisição para excluir os registros selecionados
                            Prion.Request({
                                url: urlRemove,
                                data: o2,
                                success: function () {
                                    _load();
                                    _hideMask();

                                    if (deleteUsuario != null) {
                                        deleteUsuario.call(this, arrSelecteds);
                                    }
                                },
                                error: function () {
                                    _hideMask();
                                }
                            });

                        } else {
                            // exclui apenas as linhas selecionadas (no cliente), sem fazer requisição no servidor (ou seja, não exclui do banco de dados, por exemplo)

                            // exibe a máscara e muda o seu texto
                            _showMask("Excluindo, aguarde...");
                            _deleteRow(selecteds);
                            _hideMask();

                            if (deleteUsuario != null) {
                                deleteUsuario.call(this, arrSelecteds);
                            }
                        }
                    };


                    var desabilitar = ((configUserFinal.buttons.remove.disabled != null) && (configUserFinal.buttons.remove.disabled == false));


                    _buttonRemove = Prion.Theme[Prion.settings.theme].Button.Create({
                        title: configUserFinal.buttons.remove.tooltip,
                        className: "buttons " + _configButtonsDisabled.className,
                        icon: { src: configUserFinal.buttons.remove.img.src },
                        tooltip: configUserFinal.buttons.remove.tooltip || null,
                        owner: divBotoes,
                        click: deleteClick,
                        attributes: {
                            disabled: true // sempre TRUE
                        }
                    });

                    if (desabilitar) {
                        _alterStateButton(_buttonRemove, !desabilitar);
                    }

                    configUserFinal.buttons.remove.dom = _buttonRemove;
                    configUserFinal.buttons.remove.id = _buttonRemove.id;
                }
            }


            // só cria o botão de UPDATE caso o usuário tenha informado que quer...
            if (configTable.buttons.update.show) {

                if (Prion.Theme != null) {
                    var updateClick = null;

                    if (configUserFinal.buttons.update.click != null) {
                        updateClick = function () {
                            Prion.Log({ msg: "", from: showLog.Lista });

                            // obtém todos os registros selecionados
                            var oSelecionado = _getSelected();

                            if (oSelecionado == null) {
                                alert("Selecione um registro");
                                return;
                            }

                            configUserFinal.buttons.update.click.call(this, oSelecionado[0].object);
                        }
                    }

                    var desabilitar = ((configUserFinal.buttons.update.disabled != null) && (configUserFinal.buttons.update.disabled == false));

                    _buttonUpdate = Prion.Theme[Prion.settings.theme].Button.Create({
                        title: configUserFinal.buttons.update.tooltip,
                        className: "buttons",
                        icon: { src: configUserFinal.buttons.update.img.src },
                        tooltip: configUserFinal.buttons.update.tooltip || null,
                        owner: divBotoes,
                        click: updateClick,
                        attributes: {
                            disabled: desabilitar
                        }
                    });

                    if (desabilitar) {
                        _alterStateButton(_buttonUpdate, !desabilitar);
                    }

                    configUserFinal.buttons.update.dom = _buttonUpdate;
                    configUserFinal.buttons.update.id = _buttonUpdate.id;
                }
            }


            // só cria o botão de INSERT caso o usuário tenha informado que quer...
            if (configTable.buttons.insert.show) {

                if (Prion.Theme != null) {
                    var novoClick = null;

                    if (configUserFinal.buttons.insert.click != null) {
                        novoClick = configUserFinal.buttons.insert.click;
                    }

                    var desabilitar = ((configUserFinal.buttons.insert.disabled != null) && (configUserFinal.buttons.insert.disabled == false));

                    var btnInsert = Prion.Theme[Prion.settings.theme].Button.Create({
                        title: "Novo",
                        className: "buttons",
                        icon: { src: configUserFinal.buttons.insert.img.src },
                        tooltip: configUserFinal.buttons.insert.tooltip || null,
                        owner: divBotoes,
                        click: novoClick,
                        attributes: {
                            disabled: desabilitar
                        }
                    });

                    if (desabilitar) {
                        _alterStateButton(btnInsert, !desabilitar);
                    }

                    configUserFinal.buttons.insert.dom = btnInsert;
                    configUserFinal.buttons.insert.id = btnInsert.id;

                }
            }


            // retorna a divBotoes que será appendChild em divTitle
            return divBotoes;
        };


        /**
        * @descrição: cria as colunas na tabela através de uma lista de objetos
        * @param: columns => objeto ou array de objetos com as configurações de cada coluna
        **/
        var _addColumn = function (columns) {

            // se o objeto não existir, sai fora...
            if (isNull(columns)) {
                return;
            }

            var arr = Array();

            // verifica se é um objeto
            if (!isArray(columns)) {
                // se entrou aqui é porque é um único objeto
                arr.push(columns);
            } else {
                arr = columns;
            }

            // verifica se possui algum item no array
            if (arr.length == 0) {
                return;
            }

            // ao chegar aqui, mesmo sendo um único objeto, ele já se transformou em array de objetos de configuração
            for (var i = 0; i < arr.length; i++) {

                // adiciona a coluna ao array de colunas
                configUserFinal.table.header.config.columns.push(arr[i]);

                // cria todo o html relacionado a coluna da iteração atual
                var th = _createColumn(
                    arr[i], // column
                    {aplicarFator: false, fator: 1, tamWidth: 60 },
                    true //indica que o width do header deverá ser modificado a cada nova coluna adicionada
                );

                _thead.children[0].appendChild(th);


                // se o tbody ainda não existir, vai para a próxima iteração
                if (_tbody == null) {
                    continue;
                }

                var tr = null,
                    row = null,
                    countColumns = configUserFinal.table.header.config.columns.length,
                    objHeader = configUserFinal.table.header.config.columns[countColumns - 1];

                // vai criar a coluna em cada linha do tbody
                for (var i = 0; i < _tbody.children.length; i++) {
                    tr = _tbody.children[i];

                    var td = _createCellBody(
                        objHeader,
                        null
                    );

                    tr.appendChild(td);
                }
            }
        };


        /**
        * @descrição: Aplica o theme (se o objeto Prion.Theme existir) à todos os elementos do tipo INPUT
        **/
        var _applyTheme = function () {

            if (Prion.Theme == null) {
                return;
            }

            // aplica apenas nos elementos do body
            Prion.Theme[Prion.settings.theme].Iniciar(configUserFinal.id + "Body");
        };


        /**
        * @descrição: cria uma celula no body da tabela
        * @usado por: _addRow, addColumn
        * @return: void
        **/
        var _createCellBody = function (objHeader, row, rowOdd, headerHtml, setWidth) {

            var css = (rowOdd) ? _configDefault.zebrado.classLineOdd : _configDefault.zebrado.classLineEven;

            // cria o TD e seta o text
            var td = document.createElement("td"),
                valueHTML = (_configDefault.request.returnType == Prion.Lista.ReturnType.String) ? (row[objHeader.nameJson]) : (_getValueRow(row, objHeader.nameJson, 0));


            // define o conteúdo da DIV
            var div = document.createElement("div");


            // se o campo NÃO for customizado (CustomField)...
            if (objHeader.customField == null) {

                // aplica uma formatação à coluna
                // se o atributo TYPE for != NULL e != STRING...
                if (objHeader.type != null) {
                    console.log(objHeader);
                    if (objHeader.type != "string") {
                        var f = (objHeader.format != null) ? objHeader.format : null;
                        valueHTML = _formatColumn(valueHTML, objHeader.type, f);

                        if ((objHeader.type == "float") || (objHeader.type == "decimal")) {
                            if ((objHeader.currency != null) && (objHeader.currency != "")) {
                                valueHTML = objHeader.currency.trim() + " " + valueHTML;
                            }
                        }
                        else if (objHeader.type == "boolean") {
                            console.log(valueHTML);
                        }
                    } else {
                        // SE CAIU AQUI É PORQUE type=string
                        var m = (objHeader.mask != null) ? objHeader.mask : null;
                        if (m != null) {
                            valueHTML = _applyMask(valueHTML, m);
                        }
                        else if (objHeader.type == "boolean") {
                            console.log(valueHTML);
                        }
                    }
                } else {
                    // SE CAIU AQUI É PORQUE type=null

                    var m = (objHeader.mask != null) ? objHeader.mask : null;
                    if (m != null) {
                        valueHTML = _applyMask(valueHTML, m);
                    }

                    if ((objHeader.substring != null) && (parseInt(objHeader.substring, 10) > 0)) {
                        valueHTML = valueHTML.substring(0, parseInt(objHeader.substring, 10));
                    }

                    else if (objHeader.type == "boolean") {
                        console.log(valueHTML);
                    }
                }


                // verifica se o valueHTML é null
                div.innerHTML = ((valueHTML == "undefined") || (valueHTML == null)) ? "" : valueHTML;


                // verifica se o atributo 'align' foi informado
                if (objHeader.align != null) {
                    if (objHeader.align == "right") {
                        div.style.cssText += "text-align:right";
                    }
                }
            } else {
                // obtém o elemento customizado
                var configCustomField = objHeader.customField.config,
                    newCustomField = objHeader.customField.create(configCustomField);

                valueHTML = ((typeof valueHTML == "undefined") || (valueHTML == null)) ? "" : valueHTML;

                // se for do tipo MONEY...
                if ((configCustomField.attributes.mask != null) && (configCustomField.attributes.mask.toLowerCase() == "money")) {
                    valueHTML = parseFloat(valueHTML).format(2, ",", ".");
                }

                if (newCustomField.type.toLowerCase() == "checkbox") {
                    newCustomField.checked = valueHTML || false;
                } else {
                    newCustomField.value = valueHTML;
                }

                // aplica a máscara ao elemento, apenas se este elemento possuir o atributo 'mask'
                Prion.Mascara(newCustomField);

                // adiciona o elemento customizado
                div.appendChild(newCustomField);
            }


            div.className = "textTD";
            var width = objHeader.width;

            // div responsável por fazer os pontilhados
            var divMother = document.createElement("DIV");
            divMother.style.cssText = "overflow:hidden; width:" + width;
            divMother.appendChild(div);
            // FIM

            td.appendChild(divMother);

            if (setWidth) {
                var width = (headerHtml.clientWidth == 0) ? objHeader.width : headerHtml.clientWidth.toString() + "px";

                //td.style.width = width;
                td.innerWidth = width;
            }

            // só vai adicionar a função de selecionar linha se o objeto atual não for um customField
            if (objHeader.customField == null) {

                // função responsável por selecionar uma linha. Altera o className
                var fnSelecionarLinha = function () {
                    //console.log("SELECIONAR LINHA");
                    var lineTr = (arguments[0] == null ) ? this : arguments[0];
                    // obtém todos os elementos do tipo input do elemento clicado (a própria linha)
                    var elementos = lineTr.getElementsByTagName("input");

                    // verifica se retornou algum registro
                    if ((elementos == null) || (elementos.length == 0)) { return; }

                    var nameCheckbox = "cb_" + configUserFinal.id;
                    var cbSelecionado = false;

                    for (var i = 0; i < elementos.length; i++) {
                        if (elementos[i].name == nameCheckbox) {
                            elementos[i].checked = !elementos[i].checked;
                            cbSelecionado = elementos[i].checked;
                            break;
                        }
                    }

                    // modifica o estado do botão de remover os registros
                    var totalCbSelecionados = $("#" + configUserFinal.id + "Body input[type=checkbox]:checked").length;
                    _alterStateButton(_buttonRemove, totalCbSelecionados >= 1);

                    Prion.removeClass(lineTr, "trHover");

                    // se existir o className 'lineSelected', remove, caso contrário adiciona
                    if (Prion.hasClass(lineTr, "lineSelected")) {
                        Prion.removeClass(lineTr, "lineSelected");
                    } else {
                        Prion.addClass(lineTr, "lineSelected");
                    }
                };


                Prion.Event.add(td, "click", function (evt) {
                    var line = this;

                    handleWisely("click", { line: line, evt: evt }, function () {
                        Prion.Log({ msg: "Prion.Lista.js => click na linha", type: "info", from: showLog.Lista });

                        // this => obj com os atributos: line, evt
                        var lineTr = this.line.parentElement;
                        fnSelecionarLinha.call(lineTr);
                    });
                });



                if (_configDefault.action.onDblClick != null) {

                    Prion.Event.add(td, "dblclick", function (evt) {
                        var line = this;

                        handleWisely("dblclick", { line: line, evt: evt }, function () {

                            // this => obj com os atributos: line, evt
                            var lineTr = this.line.parentElement;
                            fnSelecionarLinha(lineTr);

                            Prion.Log({ msg: "Prion.Lista.js => dblclick na linha", type: "info", from: showLog.Lista });

                            // verifica se existe o atributo
                            var ac = this.evt.srcElement.getAttribute("active-click");

                            if ((ac != null) || (ac == "false")) {
                                return;
                            }

                            var id = this.line.parentElement.getAttribute("dataid"),
                                i = (parseInt(id, 10) - 1),
                                o = _configDefault.table.body.rows[i];

                            // chama a ação onDblClick passando o objeto obtido como parâmetro
                            _configDefault.action.onDblClick.call(this.line, o, i);
                        });
                    });
                }
            }

            // retorna o TD criado
            return td;
        };



        /**
        * @descrição: cria todo o html relacionado a coluna passada por parâmetro
        * @usado por: _addColumn, _createHeader
        * @return: void
        **/
        var _createColumn = function (column, objFatorSomatorio, modificarWidthHeader) {

            // se a coluna não for visível, sai fora
            if ((column.visible != null) && (!column.visible)) { return; }

            var th = document.createElement("th"),
                widthColumn = 0;

            if ((column.class != null) && (column.class != "")) {
                th.className = configUserFinal.table.header.config.class;
            }

            // verifica se possui a coluna o atributo sort e se ele é true. 
            // Se for, aplica o css e faz as devidas sacanagens na célula
            if ((column.sort == null) || (column.sort)) {
                th.className += " headerSort";

                Prion.Event.add(th, "click", function () {

                    // verifica se possui algum item no tbody
                    if ((_tbody.children == null) || (_tbody.children.length == 0)) {
                        return;
                    }
                    // FIM...


                    // verifica se a primeira linha é uma linha VAZIA
                    var attrTRVazio = _tbody.children[0].getAttribute("linetype");

                    if ((attrTRVazio != null) && (attrTRVazio.trim().toLowerCase() == "clean")) {
                        return;
                    }
                    // FIM...


                    Prion.Log({ msg: "Prion.Lista.js => vai fazer um SORT", type: "info", from: showLog.Lista });

                    var nameJson = this.getAttribute("nameJson");
                    var oHeader = _configDefault.table.header.get(nameJson);
                    var o = new Object();
                    o.name = nameJson;

                    if (o.name.indexOf(".") < 0) {
                        if (_configDefault.request.base != "") {
                            if (_configDefault.request.returnType == Prion.Lista.ReturnType.String) {
                                o.name = _configDefault.request.base + "." + o.name;
                            }
                        }
                    }

                    // verifica a ordenação informada
                    if (_sortAtual == null) {
                        o.dir = "DESC";
                    } else if (_sortAtual.name == o.name) {
                        o.dir = (_sortAtual.dir == "ASC") ? "DESC" : "ASC";
                    } else {
                        o.dir = "ASC";
                    }

                    // remove o class 'arrowDown' e 'arrowUp' de todos os elementos
                    $("#" + _configDefault.id + " div.divTable thead th div").each(function () {
                        $(this).removeClass("arrowDown");
                        $(this).removeClass("arrowUp");
                    });

                    // guardo em cache o sort atual
                    _sortAtual = o;
                    if (o.dir == "ASC") {
                        Prion.removeClass(this.children[0], "arrowUp");
                        Prion.addClass(this.children[0], "arrowDown");
                    } else {
                        Prion.removeClass(this.children[0], "arrowDown");
                        Prion.addClass(this.children[0], "arrowUp");
                    }

                    this.children[0].style.display = "block";

                    _configDefault.paging.sort = o;
                    _load({ paging: _configDefault.paging });
                });
            }

            if ((column.width != null) && (column.width != "")) {
                // define o tamanho da coluna
                th.style.width = column.width;
            } else {
                // define o tamanho da coluna
                if (objFatorSomatorio.aplicarFator) {
                    th.style.width = (configUserFinal.table.body.config.width + (parseInt(configUserFinal.table.body.config.width, 10) * objFatorSomatorio.fator)) + "px";
                } else {
                    th.style.width = parseInt(configUserFinal.table.body.config.width, 10) + "px";
                }
            }

            // guarda o tamanho da coluna como string mesmo!
            column.width = th.style.width;


            if ((column.height != null) && (column.height != "")) {
                th.style.height = column.height;
            } else {
                th.style.height = configUserFinal.table.body.config.height + "px";
            }


            if ((column.header != null) && (column.header != "")) {
                // cria a DIV com a imagem para SORT
                var divSort = document.createElement("div");
                divSort.className = "sort arrowDown";
                divSort.innerHTML = "";
                th.appendChild(divSort);

                var divTamanhoCertim = document.createElement("div"); // ahaha, palavrinha chave rs
                divTamanhoCertim.style.width = th.style.width; // já possui o px
                th.appendChild(divTamanhoCertim);

                // vai adicionar esta DIV dentro da div 'divTamanhoCertim'
                var divValorzim = document.createElement("div");
                divValorzim.className = "text";
                divValorzim.innerHTML = column.header;

                divTamanhoCertim.appendChild(divValorzim);


                th.appendChild(divTamanhoCertim);
            }

            // definir como ASC a direção inicial da coluna
            column.dir = "ASC";

            th.setAttribute("namejson", column.nameJson);


            // define o novo tamanho da tableHeader e tableBody
            if (modificarWidthHeader) {
                _thead.parentNode.style.width = (parseInt(_thead.parentNode.style.width, 10) + parseInt(column.width, 10)) + "px";
                _tbody.parentNode.style.width = (parseInt(_thead.parentNode.style.width, 10) + parseInt(column.width, 10)) + "px";
            }

            // retorna a coluna criada
            return th;
        };


        /**
        * @descrição: cria as linhas da tabela através de um json
        * @param: rows => objeto no formato JSON
        **/
        var _createRows = function (rows, retorno) {

            _showMask();

            var valoresLocal = (configUser.type == Prion.Lista.Type.Local),
                totalRows = 0,
                widthInicial = 0;

            widthInicial += (configUserFinal.rowNumber.show) ? 30 : 0;
            widthInicial += (configUserFinal.checkbox.show) ? 30 : 0;

            var objFatorSomatorio = _getFatorSomatorio(widthInicial);


            // define o width do tbody
            // comentado em 06/11/2012
            //_tbody.parentElement.style.width = objFatorSomatorio.tamWidth.toString() + "px";


            // armazena o objeto rows, que contém todas as linhas e colunas
            if (_configDefault.table.body.rows == null) {
                _configDefault.table.body.rows = new Array();
            }


            if ((rows != null) && (rows.constructor == Object)) {
                var rowsTemp = new Array();
                rowsTemp.push(rows);
                rows = rowsTemp;
            }


            var possuiTRVazio = false;

            // verifica se a primeira linha é uma linha VAZIA
            if ((_tbody.children != null) && (_tbody.children.length > 0)) {
                var attrTRVazio = _tbody.children[0].getAttribute("linetype");

                if ((attrTRVazio != null) && (attrTRVazio.trim().toLowerCase() == "clean")) {
                    possuiTRVazio = true;
                }
            }

            if ((rows == null) || (rows.length == 0)) {
                _hideMask();

                // @versão: 1.39
                // só vai adicionar o TR vazio caso ainda não exista
                if (!possuiTRVazio) {
                    _gerarTRVazio();
                }
                // FIM DA VERSÃO 1.39


                // associa uma function ao scrollbar da div
                document.getElementById(_configDefault.id + "Body").onscroll = function () {
                    document.getElementById(_configDefault.id + "Header").style.left = (parseFloat(document.getElementById(_configDefault.id + "Body").scrollLeft) * -1) + "px";
                };


                // desmarca o checkbox do header
                _checkboxHeader.checked = false;
                _checkboxHeader.disabled = true;
                Prion.addClass(_checkboxHeader, "cbDisabled");

                return;
            }


            // se possuir o TR clean, deleta ele do tbody
            if (possuiTRVazio) {
                _tbody.deleteRow(0);
            }


            // adiciona o object ao array
            for (var i = 0; i < rows.length; i++) {
                _configDefault.table.body.rows.push(rows[i]);
            }

            // obtém o total de registros da lista
            totalRows = _configDefault.table.body.rows.length;


            // habilita o checkbox do header
            _checkboxHeader.disabled = false;
            Prion.removeClass(_checkboxHeader, "cbDisabled");


            // adiciona as TR
            var tr = null,
                contagemInicial = 0;

            if (retorno != null) {
                var totalPorPagina = configUserFinal.paging.totalPerPage,
                    paginaAtual = retorno.page;

                contagemInicial = (paginaAtual == 1) ? 0 : ((paginaAtual - 1) * totalPorPagina);
            }

            for (var i = 0; i < rows.length; i++) {
                // (i+1) => número da página
                // rows[1] => objeto com os dados que serão adicionados na linha
                // (i==0) => se i == 0, define o width de cada TD. Essa comparação é feita pois não há necessidade de definir o width para todas as TR
                // (i%2) => se i % 2, for true, significa que é uma linha par
                tr = _addRow(
                    contagemInicial + (i + 1), // lineNumber
                    (i + 1), // comentado em 04/10/2013 => motivo: Lista.Type.Local estava ficando com o primeiro dataid=2 ao invés de ficar dataid=1 (valoresLocal) ? (totalRows + i) : (i + 1), // lineNumberDataId
                    rows[i], // row
                    false, //(i == 0), // setWidth
                    (i % 2) // rowOdd
                );

                _tbody.appendChild(tr);
            }


            // associa uma function ao scrollbar da div
            document.getElementById(_configDefault.id + "Body").onscroll = function () {
                document.getElementById(_configDefault.id + "Header").style.left = (parseFloat(document.getElementById(_configDefault.id + "Body").scrollLeft) * -1) + "px";
            };


            _hideMask();
        };


        /**
        * @descrição: cria os campos de busca
        * @return: this
        **/
        var _createSearch = function (owner, config) {

            // verifica se é para criar a barra de busca
            if (!config.search.show) {
                return;
            }


            var divSearch = document.createElement("div");
            divSearch.className = "search";

            // monta o HTML com os campos visíveis na lista
            var htmlListaCampos = _montarPopupListaCampos();

            // cria o botão para exibir os CAMPOS da lista
            var btnSearch = Prion.Theme[Prion.settings.theme].Button.Create({
                title: "Campos",
                className: "buttons",
                owner: divSearch,
                click: function () {
                    var dom = (config.id + "ListaCampos").getDom();
                    if (dom == null) {
                        return;
                    }

                    var css = dom.style.cssText;
                    if (css.trim() == "") {
                        dom.style.cssText = "opacity:1; filter:Alpha(opacity=100)";
                    } else {
                        dom.style.cssText = "";
                    }
                }
            });

            divSearch.appendChild(htmlListaCampos);
            divSearch.appendChild(btnSearch);

            var inputSearch = document.createElement("input");
            inputSearch.id = config.id + "Search";
            inputSearch.type = "text";
            inputSearch.style.cssText = "width: " + config.search.width + "; margin-left: 6px;";

            divSearch.appendChild(inputSearch);


            // cria o botão de LOCALIZAR
            var btnSearch = Prion.Theme[Prion.settings.theme].Button.Create({
                title: "Localizar",
                className: "buttons",
                owner: divSearch,
                click: function () {
                    var textSearch = (config.id + "Search").getValue();
                    var columns = _getColumns(true, "nameJson", true, true);

                    _showMask();

                    Prion.Request({
                        url: config.url,
                        data: "fields=[" + columns + "]&query=" + textSearch,
                        success: function () {
                            //_load();
                            _hideMask();
                        },
                        error: function () {
                            _hideMask();
                        }
                    });
                }
            });

            divSearch.appendChild(btnSearch);


            owner.appendChild(divSearch);
        };


        /**
        * @descrição: cria o thead da tabela
        * @param: table => HTML object de uma table
        * @return: thead referente aos headers da table
        **/
        var _createHeader = function (table) {

            _thead = null;
            _thead = document.createElement("thead");
            _thead.height = configUserFinal.table.header.config.heightDefaultCell + "px";

            /*if (configUserFinal.table.header.config.class != "") { _thead.className = configTable.table.header.config.class; } */

            // cria o TR que será adicionado ao thead
            var tr = document.createElement("tr"),
                totalWidth = 0;

            // verifica se é para exibir o número atual da linha
            if (configUserFinal.rowNumber.show) {

                var divNumber = document.createElement("div");
                divNumber.style.width = "30px";

                var th = document.createElement("th");
                th.style.width = "30px";
                th.appendChild(divNumber);

                tr.appendChild(th);

                totalWidth += 30;
            }
            // FIM

            // verifica se é para criar um checkbox
            if (configUserFinal.checkbox.show) {

                var divCheckbox = document.createElement("div");
                divCheckbox.style.width = "30px";

                var th = document.createElement("th");
                th.style.cssText = "width:30px; text-align:center;";

                var cb = document.createElement("input");
                cb.type = "checkbox";
                cb.checked = false;
                cb.id = Prion.GenerateId();

                if (Prion.settings.ClassName != null) {
                    if (configUserFinal.useTheme) {
                        cb.className = Prion.settings.ClassName.checkbox;
                    }
                }

                // adiciona o onclick para o checkbox do header
                Prion.Event.add(cb, "click", function () {
                    Prion.CheckBox.Toogle("cb_" + _configDefault.id, this.checked);

                    // muda o estado também do checkbox do footer
                    //var c = document.querySelectorAll("#" + _configDefault.id + " table tfoot input[type=checkbox]");
                    //if ((c[0] != null) && (c.length >= 1)) { c[0].checked = this.checked; }

                    // obtém o estado do checkbox do header e armazena na variável em cache (_checkboxHeader)
                    _checkboxHeader.checked = this.checked;


                    // modifica o estado do botão de remover os registros, apenas se existir algum registro no tbody
                    if (_tbody.children.length > 0) {
                        var marcar = (_tbody.children.length >= 2);

                        if (_tbody.children.length == 1) {
                            var attrTRVazio = _tbody.children[0].getAttribute("linetype");
                            marcar = true;

                            // se possuiir o atributo linetype='clean', significa que a primeira linha é vazia
                            if ((attrTRVazio != null) && (attrTRVazio.trim().toLowerCase() == "clean")) {
                                marcar = false;
                            }
                        }

                        if (marcar) {
                            _alterStateButton(_buttonRemove, _checkboxHeader.checked);
                        }
                    }


                    // obtém todos os TR do body
                    var tr = _tbody.children; //document.querySelectorAll("#" + _configDefault.id + " table tbody tr");
                    if ((tr == null) || (tr.length == 0)) {
                        return;
                    }

                    var attr = "";

                    // modifica o class de cada linha de acordo com o estado do checkbox do header
                    for (var i = 0; i < tr.length; i++) {
                        attr = tr[i].getAttribute("linetype");

                        if ((attr != null) && (attr == "clean")) {
                            continue;
                        }

                        if (this.checked) { Prion.addClass(tr[i], "lineSelected"); }
                        else { Prion.removeClass(tr[i], "lineSelected"); }

                        Prion.removeClass(tr[i], "trHover");
                    }
                });

                // armazena o objeto em cache, para aceletar futuras ações
                _checkboxHeader = cb;

                divCheckbox.appendChild(cb);
                th.appendChild(divCheckbox);
                tr.appendChild(th);

                totalWidth += 30;
            }

            // obtém o fator que será aplicado em cada coluna
            // passa o totalWidth, que é o total acumulado até agora (através da exibição do rowNumber e do checkbox)
            var objFatorSomatorio = _getFatorSomatorio(totalWidth);



            // cria um TH para cada coluna definida...
            for (var i = 0; i < configUserFinal.columns.length; i++) {
                var th = _createColumn(
                    configUserFinal.columns[i],
                    objFatorSomatorio,
                    false // indica que não deverá modificar o width do header
                );

                if (th == null) {
                    Prion.Log({ msg: "th indefinido", type: "error" });
                    continue;
                }

                tr.appendChild(th);
            }

            // adiciona o TR ao header da tabela
            _thead.appendChild(tr);

            // retorna um objeto com os atributos:
            // * thead: elemento HTML com o thead da table, representando todas as colunas da tabela
            // * tamWidth (int): representa o width que a table terá
            return {
                thead: _thead,
                tamWidth: objFatorSomatorio.tamWidth
            };
        };


        /**
        * @descrição: cria o tfoot da tabela
        **/
        var _createFooter = function (table, configTable) {

            if (!configTable.table.footer.createFooter) { return; }

            var tfoot = document.createElement("tfoot");

            if (configTable.table.footer.config.class != "") {
                /*//COMENTADO  tfoot.className = configTable.table.footer.config.class;*/
            }

            var tr = document.createElement("tr"),
                td = null,
                totalWidth = 0;

            // verifica se é para exibir o número atual da linha
            if (_configDefault.rowNumber.show) {

                td = document.createElement("td");
                tr.appendChild(td);

                totalWidth += 30;
            }
            // FIM

            // verifica se é para criar um checkbox
            if (configTable.checkbox.show) {

                // verifica se foi definido para criar o checkbox no footer
                if (configTable.table.footer.config.checkbox.show) {
                    td = document.createElement("td");
                    td.style.cssText = "text-align:center;";

                    var cb = document.createElement("input");
                    cb.type = "checkbox";
                    cb.checked = false;
                    //cb.name = "cb_" + _configDefault.id;

                    if (Prion.settings.ClassName != null) {
                        if (configUserFinal.useTheme) {
                            cb.className = Prion.settings.ClassName.checkbox;
                        }
                    }

                    // adiciona o onclick para o checkbox
                    Prion.Event.add(cb, "click", function () {
                        Prion.CheckBox.Toogle("cb_" + _configDefault.id, this.checked);

                        // muda o estado também do checkbox do header
                        //var c = document.querySelectorAll("#" + _configDefault.id + " table thead input[type=checkbox]");
                        //if ((c[0] != null) && (c.length >= 1)) { c[0].checked = this.checked; }
                        _checkboxHeader.checked = this.checked;
                    });

                    td.appendChild(cb);
                    tr.appendChild(td);
                }

                totalWidth += 30;
            }

            for (var i = 0; i < configTable.columns.length; i++) {

                // se a coluna não for visível, vai para a próxima iteração...
                if ((configTable.columns[i].visible != null) && (!configTable.columns[i].visible)) { continue; }

                // cria um TD vazio
                td = document.createElement("td");
                tr.appendChild(td);
            }

            tfoot.appendChild(tr);

            return tfoot;
        };



        /**
        * @descrição: cria os elementos referentes à paginação
        **/
        var _createPaging = function (paging, width) {

            // verifica se é para criar a páginação...
            if (!paging.show) {
                return null;
            }

            var divGlobal = document.createElement("div");
            divGlobal.className = "paging";
            divGlobal.style.width = (width + 2) + "px";
            //divGlobal.style.width = (typeof _configDefault.width == "number") ? (_configDefault.width + 2) + "px" : "100%";


            var divDentro = document.createElement("div");
            divDentro.className = "pagingInternal";


            // cria o botão <<
            var div1 = document.createElement("div");
            div1.innerHTML = "";
            div1.className = "pagingFirst pagingButton";
            div1.style.cssText += "float:left;";
            divDentro.appendChild(div1);

            _buttonsPaging.first = div1;


            // adiciona um evento onKeyPress para o botão <<
            Prion.Event.add(div1, "click", function () {
                Prion.Log({ msg: "Prion.Lista.js => objeto de paginação: " + _configDefault.paging, type: "info", from: showLog.Lista });

                _configDefault.paging.currentPage = 1;
                _load({ paging: _configDefault.paging });
            });


            // cria o botão <
            var div2 = document.createElement("div");
            div2.innerHTML = "";
            div2.className = "pagingPrevious pagingButton";
            div2.style.cssText += "float:left;";
            divDentro.appendChild(div2);

            _buttonsPaging.previous = div2;

            // adiciona um evento onKeyPress para o botão <
            Prion.Event.add(div2, "click", function () {
                Prion.Log({ msg: "Prion.Lista.js => objeto de paginação: " + _configDefault.paging, type: "info", from: showLog.Lista });

                _configDefault.paging.currentPage = (_configDefault.paging.currentPage == 1) ? 1 : (_configDefault.paging.currentPage - 1);
                _load({ paging: _configDefault.paging });
            });

            // cria a span 'Página'
            var spanPage = document.createElement("span");
            spanPage.innerText = _configDefault.paging.textPage;
            spanPage.style.cssText += "float:left;";
            divDentro.appendChild(spanPage);


            // cria o textbox
            var txtPagina = document.createElement("input");
            txtPagina.name = "NumberPage";
            txtPagina.type = "text";
            txtPagina.size = 2;
            txtPagina.value = "1";
            txtPagina.style.cssText += "float:left;";
            divDentro.appendChild(txtPagina);

            // adiciona um evento onKeyPress para o input
            Prion.Event.add(txtPagina, "keypress", function (evt) {
                if (evt.keyCode == 13) {
                    Prion.Log({ msg: "Prion.Lista.js => objeto de paginação: " + _configDefault.paging, type: "info", from: showLog.Lista });

                    // obtém a página informada na caixa de texto
                    var pagina = (evt.target.value == "") ? 1 : parseInt(evt.target.value, 10);

                    // verifica o número total de páginas disponíveis
                    pagina = (pagina > _configDefault.paging.totalPages) ? _configDefault.paging.totalPages : pagina;

                    // define para qual página o usuário será redirecionado
                    _configDefault.paging.currentPage = pagina;
                    _load({ paging: _configDefault.paging });
                }

                var keypress = (window.event) ? evt.keyCode : evt.which;
                //if (32 == keypress) { return; }

                if ((keypress != 13) && (keypress != 8) && (keypress != 0) && (keypress != 8) && (keypress != 0) && ((keypress < 48) || (keypress > 57))) {
                    (window.event) ? evt.returnValue = false : evt.preventDefault();
                }
            });


            // cria a label 'de XX'
            var spanTotalPagina = document.createElement("span");
            spanTotalPagina.id = _configDefault.id + "_TotalPerPage";
            spanTotalPagina.innerHTML = _configDefault.paging.textOf + " 1";
            spanTotalPagina.style.cssText += "float:left;";
            divDentro.appendChild(spanTotalPagina);

            // cria o botão >
            var div3 = document.createElement("div");
            div3.innerHTML = "";
            div3.className = "pagingNext pagingButton";
            div3.style.cssText += "float:left;";
            divDentro.appendChild(div3);

            _buttonsPaging.next = div3;

            // adiciona um evento onKeyPress para o botão >
            Prion.Event.add(div3, "click", function () {
                Prion.Log({ msg: "Prion.Lista.js => objeto de paginação: " + _configDefault.paging, type: "info", from: showLog.Lista });

                // obtém o total de páginas disponíveis
                var paginaFinal = _configDefault.paging.totalPages;

                // verifica se a página atual + 1 (próxima), irá exceder o total de páginas disponíveis. Se for, leva o usuário para a página final, caso contrário leva o usuário para a próxima página
                var proximaPagina = ((_configDefault.paging.currentPage + 1) > paginaFinal) ? paginaFinal : (_configDefault.paging.currentPage + 1);

                // define para qual página o usuário será redirecionado
                _configDefault.paging.currentPage = proximaPagina;
                _load({ paging: _configDefault.paging });
            });

            // cria o botão >>
            var div4 = document.createElement("div");
            div4.innerHTML = "";
            div4.className = "pagingLast pagingButton";
            div4.style.cssText += "float:left;";
            divDentro.appendChild(div4);

            _buttonsPaging.last = div4;

            // adiciona um evento onKeyPress para o botão <<
            Prion.Event.add(div4, "click", function () {
                Prion.Log({ msg: "Prion.Lista.js => objeto de paginação: " + _configDefault.paging, type: "info", from: showLog.Lista });

                // define para qual página o usuário será redirecionado
                _configDefault.paging.currentPage = _configDefault.paging.totalPages;
                _load({ paging: _configDefault.paging });
            });


            // cria o botão REFRESH
            var divRefresh = document.createElement("div");
            divRefresh.innerHTML = "";
            divRefresh.className = "pagingRefresh pagingButton";
            divRefresh.style.cssText += "float:left;";
            divDentro.appendChild(divRefresh);

            // adiciona um evento onKeyPress para o botão REFRESH
            Prion.Event.add(divRefresh, "click", function () {
                Prion.Log({ msg: "Prion.Lista.js => objeto de paginação: " + _configDefault.paging, type: "info", from: showLog.Lista });
                _load({ paging: _configDefault.paging });
            });


            // verifica se é para criar o combobox de total de registros
            if (_configDefault.paging.showOptions) {
                var existe = false, select = document.createElement("select");
                select.id = "";
                select.style.cssText = "float:left; height:26px;";

                // verifica se o valor default existe na lista de options que serão adicionados
                for (var j = 0; j < _configDefault.paging.options.length; j++) {
                    if (_configDefault.paging.options[j] == _configDefault.paging.totalPerPage) {
                        existe = true;
                        break;
                    }
                }

                // se ainda não existe, adiciona o totalPerPage (valor default)
                if (!existe) {
                    // adiciona no array o item que não existe
                    _configDefault.paging.options.push(_configDefault.paging.totalPerPage);
                }

                _configDefault.paging.options.sort(Prion.DynamicSort(null, "ASC", "integer"));

                var arrOptionsComboBox = Array();

                for (var j = 0; j < _configDefault.paging.options.length; j++) {
                    arrOptionsComboBox.push({
                        value: _configDefault.paging.options[j],
                        text: _configDefault.paging.options[j],
                        selected: (_configDefault.paging.options[j] == _configDefault.paging.totalPerPage) // deixa o item selecionado
                    });
                }

                // ordena o array
                arrOptionsComboBox.sort();
                Prion.ComboBox.Add({ el: select, itens: arrOptionsComboBox });

                // ordena os dados
                //Prion.ComboBox.Sort(select);

                Prion.Event.add(select, "change", function () {
                    var vSelecionado = Prion.ComboBox.Get(this);
                    if (vSelecionado.value == "") { return; }

                    _configDefault.paging.totalPerPage = parseInt(vSelecionado.value, 10);
                    _configLoad.paging.totalPerPage = parseInt(vSelecionado.value, 10);
                    _load();
                });

                divDentro.appendChild(select);
            }


            if (_configDefault.paging.showDescription) {
                var spanTotal = document.createElement("span");
                spanTotal.id = _configDefault.id + "_Description";
                spanTotal.style.cssText += "float:right;";
                spanTotal.innerHTML = _configDefault.paging.description.format(1, 1, 1); // "Exibindo 21 de 40 / 200 registros"
                divDentro.appendChild(spanTotal);
            }


            // retorna a divGlobal que possui todos os elementos referente à paginação
            divGlobal.appendChild(divDentro);

            return divGlobal;
        };


        /**
        * @descrição: cria todo o HTML da lista
        **/
        var _init = function (configTable) {

            // verifica se foi informado as colunas do grid
            if ((configTable.columns != null) && (configTable.columns.length == 0)) {
                Prion.Log({ msg: "Prion.Lista.js => não foi informado nenhuma coluna para esta lista.", type: "info", from: showLog.Lista });
                return;
            }


            // obtém o elemento
            var content = document.getElementById(configTable.id);
            if (content == null) {
                Prion.Log({ msg: "Prion.Lista.js => elemento " + configTable.id + " não encontrado.", type: "error", from: showLog.Lista });
                return;
            }

            // pega o width e height do content
            var width = (typeof configTable.width == "number") ? (configTable.width) : ($(content).parent().width());
            var height = (typeof configTable.height == "number") ? (configTable.height) : (configTable.height);


            content.style.width = width.toString() + "px";
            content.style.height = height.toString() + "px";


            var divTitle = null,
                h3 = null;

            // verifica se é para exibir o titulo do grid
            if (configTable.title.show) {
                divTitle = document.createElement("div");
                divTitle.className = configTable.title.className;

                var divH3 = document.createElement("div");
                divH3.style.float = "left";

                h3 = document.createElement("h3");
                h3.className = "listTitleH3";
                h3.innerHTML = configTable.title.text;
                divH3.appendChild(h3);
                divTitle.appendChild(divH3);


                // cria os botões de INSERT, UPDATE E REMOVE, se for o caso...
                var buttonsTitle = _createButtons(divTitle, configTable);

                // se existir algum botão, adiciona na barra de títulos
                if (buttonsTitle != null) {
                    divTitle.appendChild(buttonsTitle);
                }


                // chama o método responsável por criar a caixa de busca
                // passa o elemento owner
                _createSearch(divTitle, configTable);

                content.appendChild(divTitle);


                // aplica o tooltip de cada botão
                if (Prion.Theme != null) {
                    // apenas para os botões que não estiverem desabilitados
                    Prion.Theme[Prion.settings.theme].Tooltip.Apply("#" + configUserFinal.id + " div.shortcuts-icons a.tips[disabled=false]");
                }
            }



            var divGlobal = document.createElement("div");
            divGlobal.id = configUserFinal.id + "Content";
            divGlobal.style.cssText = "width: calc(100% + 2px); overflow: hidden;";


            // cria a máscara
            var divMask = document.createElement("div");
            divMask.className = configTable.mask.className;
            divMask.style.display = "none";
            divGlobal.appendChild(divMask);


            // cria a div com o texto de aguarde
            var divMsgAguarde = document.createElement("div");
            divMsgAguarde.className = "maskListMsg";
            divMsgAguarde.style.cssText = "left:34% !important; top:54% !important; display:none; position:absolute; z-index:2002;";

            var imgAguarde = document.createElement("img");
            imgAguarde.src = configTable.mask.imgLoad;

            var labelAguarde = document.createElement("label");
            labelAguarde.innerHTML = configTable.mask.text;

            divMsgAguarde.appendChild(imgAguarde);
            divMsgAguarde.appendChild(labelAguarde);

            divGlobal.appendChild(divMsgAguarde);
            // FIM


            var divTable = document.createElement("div");
            divTable.className = "divTable";

            // CRIA UMA TABELA PARA OS HEADERS
            var divHeader = document.createElement("div");
            divHeader.className = "header";
            divHeader.style.cssText = "overflow:hidden;";

            var tableHeader = document.createElement("table");
            tableHeader.id = configTable.id + "Header";
            tableHeader.style.cssText = "position:relative;";

            var objThead = _createHeader(tableHeader);
            tableHeader.appendChild(objThead.thead);
            divHeader.appendChild(tableHeader);
            // FIM



            // se for um popup, diminui 2px no tamanho, caso contrário não diminui nada!
            var widthSubtrair = 2; // 2px é igual a bordar da lista
            var heightBodySubtrair = 0; // PQ DISSO???? PQ PRECISOU, se foda rs


            // CRIA UMA TABELA PARA O BODY
            var divBody = document.createElement("div");
            divBody.id = configTable.id + "Body";
            divBody.style.cssText = "overflow:auto;";

            var tableBody = document.createElement("table");

            // cria o tbody e armazena em cache
            _tbody = document.createElement("tbody");
            tableBody.appendChild(_tbody);
            divBody.appendChild(tableBody);
            // FIM



            var tfooter = _createFooter(tableBody, configTable);
            if (tfooter != null) {
                table.appendChild(tfooter);
            }
            // FIM da criação da tabela

            divTable.appendChild(divHeader);
            divTable.appendChild(divBody);


            // cria os elemntos referente à paginação
            var divPaging = _createPaging(configTable.paging, width);


            // adiciona à table à div content
            divGlobal.appendChild(divTable);

            // adiciona a paginação
            if (divPaging != null) {
                divGlobal.appendChild(divPaging);
            }


            content.style.cssText += configTable.style;


            var widthInt = parseInt(content.style.width.replace("px", ""), 10);
            //var widthShowzaoRs = objThead.thead.clientWidth;

            // gera um objeto com os width e height dos elementos: HEADER, BODY E PAGING
            var configsSizeTable = {
                title: { width: (divTitle.clientWidth || widthInt) - widthSubtrair, height: (divTitle.clientHeight || configTable.title.height) },
                header: { width: (divHeader.clientWidth || widthInt) - widthSubtrair, height: (divHeader.clientHeight) },
                body: { width: (divBody.clientWidth || widthInt) - widthSubtrair, height: (divBody.clientHeight || configTable.height) }
            };

            if (divPaging != null) {
                configsSizeTable.paging = { width: (divPaging.clientWidth || widthInt) - widthSubtrair, height: divPaging.clientHeight };
            }


            // VAI CALCULAR O TAMANHO DO HEIGHT/WIDTH INTERNO (tamanho do BODY)
            var tamanhoInterno = _calculateSizeInternal(configsSizeTable);


            // DEFININDO OS TAMANHOS
            divTitle.style.width = width.toString() + "px";
            divTitle.style.height = configTable.title.height.toString() + "px";

            divMask.style.width = width.toString() + "px";
            divMask.style.height = (tamanhoInterno.height + configsSizeTable.title.height - heightBodySubtrair).toString() + "px";

            divTable.style.width = width.toString() + "px";
            divHeader.style.width = width.toString() + "px";

            divBody.style.height = (tamanhoInterno.height - heightBodySubtrair).toString() + "px";


            divGlobal.style.height = "calc(100% + 40px);";

            content.appendChild(divGlobal);


            // se existir o H3 e collapsed
            if ((h3 != null) && (configTable.collapsed != null)) {
                if (configTable.collapsed) {
                    divGlobal.style.display = "none";
                }

                // como existe o collapsed, muda o cursor do elemento H3
                h3.style.cssText += "cursor:pointer;";

                $(h3).click(function () {
                    // se estiver TRUE, lista esta diminuida, vamos aumentar \o/
                    if (configTable.collapsed) {
                        $("#" + configTable.id + "Content").slideDown(500);
                    } else {
                        $("#" + configTable.id + "Content").slideUp(500);
                    }

                    configTable.collapsed = !configTable.collapsed;
                });
            }

            //_adjustWidthHeader();
            // FIM DO INIT
        };



        /**
        * @descrição: exclui uma linha da tabela.
        * @param: o => pode ser um id (índice da linha) 
        *              pode ser um objeto (que conterá o índice da linha)
        *              pode ser uma string (que conterá o índice da linha separado por vírgula
        **/
        var _deleteRow = function (o) {

            // obtém o tbody
            if ((_tbody == null) || (_tbody.rows.length == 0)) { return; }

            switch (typeof o) {

                case "number":
                    {
                        // exclui a linha pelo índice
                        _tbody.deleteRow(o);

                        break;
                    }

                case "string":
                    {
                        // exclui uma linha através de um ou mais ID (pode ser separados por virgulinha rs :D
                        var ids = o.split(",");

                        // verifica se possui algum registro
                        if (ids.length == 0) { return; }

                        // faz de tras pra frente
                        for (var i = ids.length - 1; i >= 0; i--) {

                            if (ids[i].trim() == "") { continue; }

                            // o ids[i] não pode ser maior do que o total de linhas do body
                            if (parseInt(ids[i], 10) >= _tbody.rows.length) { continue; }

                            // delete a linha
                            _tbody.deleteRow(ids[i]);
                        }

                        break;
                    }

                case "object":
                    {
                        // exclui uma linha através do objeto
                        for (var i = o.length - 1; i >= 0; i--) {

                            // verifica se existe o atributo
                            if (o[i].rowNumber == null) { continue; }

                            // delete a linha
                            _tbody.deleteRow(o[i].rowNumber);

                            // se a lista for local, marca o objeto como excluido
                            if (_configDefault.type == Prion.Lista.Type.Local) {
                                _configDefault.table.body.rows[o[i].rowNumber].EstadoObjeto = EstadoObjeto.Excluido;
                            } else {
                                // se entrou aqui é porque a lista é remota, então neste caso apenas deleta o objeto do array
                                _configDefault.table.body.rows.splice(o[i].rowNumber, 1);
                            }
                        }

                        _alterStateButton(_buttonRemove, false);

                        break;
                    }
            }


            // desmarca o checkbox do header
            if (_configDefault.checkbox.show) {
                _checkboxHeader.checked = false;
            }

            // verifica se a coluna que exibe o número da linha esta ativa para esta lista
            if (_configDefault.rowNumber.show) {
                // atualiza a label que exibe o número da linha
                var count = 0;

                for (var i = 0; i < _tbody.rows.length; i++) {
                    count += 1;
                    _tbody.rows[i].children[0].children[0].textContent = count.toString();
                }
            }
        };


        /**
        * @descrição: Método que desabilita os buttons da lista. Botão de incluir, deletar e alterar
        * @return: void
        **/
        var _disableButtons = function () {
            if (!_configDefault.buttons.insert.show) {
                var d = document.getElementById(_configDefault.id + "BtnNew");
                if (d != null) { }
            }

            /*if (!_configDefault.buttons.update.show) {
            _alterStateButton(_buttonUpdate, false);
            }*/

            /*if (!_configDefault.buttons.remove.show) {
            _alterStateButton(_buttonRemove, false);
            }*/
        };


        /**
        * @descrição: Altera o estado do botão de exclusão dos registros
        * @paran: botao (dom HTML) => botão que sofrerá a modificação
        **/
        var _alterStateButton = function (botao, habilitar) {

            // verifica se o botão existe
            if (botao == null) {
                return;
            }

            var existePrionTheme = (Prion.Theme != null);

            if (habilitar) {
                // remove o atributo 'disabled' e remove o css de botão desabilitado
                botao.setAttribute("disabled", false);
                Prion.removeClass(botao, _configButtonsDisabled.className);

                if (existePrionTheme) {
                    var data = botao.getAttribute("data");
                    if (data == null) { return; }

                    data = JSON.parse(data);

                    Prion.Theme[Prion.settings.theme].Tooltip.Apply(botao);
                    Prion.Theme[Prion.settings.theme].Tooltip.Add(botao, data["original-title"]);
                }

                return;
            }

            // se chegou aqui é porque é para desabilitar o botão

            // marca o botão como disabled e define os css de botão desabilitado
            botao.setAttribute("disabled", true);
            Prion.addClass(botao, _configButtonsDisabled.className);

            if (existePrionTheme) {
                Prion.Theme[Prion.settings.theme].Tooltip.Remove(botao);
            }
        };


        /**
        * @descrição: Aplica um máscara a uma string
        **/
        var _applyMask = function (value, mask) {
            return Prion.Mascara2(value, mask);
        };


        /**
        * @descrição: Método que desabilita os buttons da lista. Botão de incluir, deletar e alterar
        * @return: void
        **/
        var _enableButtons = function () {
            if (_configDefault.buttons.insert.show) {
                var d = document.getElementById(_configDefault.id + "BtnNew");
                if (d != null) { }
            }

            if (_configDefault.buttons.update.show) {
                _alterStateButton(_buttonUpdate, true);
            }

            if (_configDefault.buttons.remove.show) {
                _alterStateButton(_buttonRemove, true);
            }
        };


        /**
        * @descrição: Formata uma string (value) de acordo com o seu tipo (type)
        **/
        var _formatColumn = function (value, type/**, f**/) {

            // verifica se o value é NULL
            if ((value == null) || (value.trim() == "")) { return ""; }

            type = type.trim();

            switch (type) {
                case "date":
                case "datetime":
                case "time":
                    {
                        var v = null;

                        // verifica o tipo do atributo value
                        if (typeof value == "string") {
                            // verifica se possui a string Date
                            if (value.indexOf("Date") < 0) {
                                v = value.replaceAll("/", "-");
                                v = v.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3");
                                v = new Date(v);
                            }
                        }

                        // verifica se o segundo argumento (f) foi informado
                        var a = (arguments[2] == null) ? null : arguments[2];

                        // se o argumento não foi informado, irá pegar a formatação padrão dependendo do seu type
                        if (a == null) {
                            switch (type) {
                                case "date": { a = "dd/MM/yyyy"; break; }
                                case "datetime": { a = "dd/MM/yyyy HH:mm:ss"; break; }
                                case "time": { a = "HH:mm:ss"; break; }
                            }
                        }

                        var d = ((v != null) && (isDate(v))) ? v : Date.initWithJSON(value);
                        return d.toFormat(a);
                    }

                case "float":
                case "decimal":
                    {

                        //var v1 = value;
                        //v1 = v1.replace(".","");
                        //v1 = v1.replace(",",".");
                        value = parseFloat(value);
                        value = value.format(2, ",", ".");

                        break;
                    }
            }

            return value;
        };



        /**
        * @descrição: monta o HTML com os campos visíveis na lista
        **/
        var _montarPopupListaCampos = function () {
            // obtém os campos da mesma forma e na mesma ordem que são exibidos no header
            var campos = _getColumns(true, "header", false, false);

            if (campos == "") {
                return null;
            }

            var arrCampos = campos.split(","),
                html = document.createElement("div");

            html.id = configUserFinal.id + "ListaCampos";
            html.className = "listFieldsSearch";

            var ul = document.createElement("ul");

            for (var i = 0; i < arrCampos.length; i++) {
                var li = document.createElement("li");

                var cb = document.createElement("input");
                cb.type = "checkbox";
                cb.checked = true;

                var span = document.createElement("span");
                span.innerText = arrCampos[i];

                li.appendChild(cb);
                li.appendChild(span);
                ul.appendChild(li);
            }

            html.appendChild(ul);
            return html;
        };


        /**
        * @descrição: cria um TR VAZIO para poder aparecer a barra de rolagem
        * @return: void
        **/
        var _gerarTRVazio = function () {
            var tr = document.createElement("tr");
            tr.style.cssText = "border:none;";
            tr.setAttribute("linetype", "clean");

            // adiciona o TD à TR
            var td = document.createElement("td");
            td.style.cssText = "border:none;";
            tr.appendChild(td);

            _tbody.appendChild(tr);
            _tbody.parentElement.style.width = document.getElementById(_configDefault.id + "Header").clientWidth + "px";
        };



        var _getColumns = function (onlyVisible, property, ordenarResultado, addAspas) {
            if (onlyVisible) {
                var colunas = configUserFinal.table.header.config.columns;
                var strColunas = "";

                if (ordenarResultado) {
                    colunas.sort(Prion.DynamicSort(property, "ASC", "string"));
                }

                for (var i = 0; i < colunas.length; i++) {
                    if (addAspas) {
                        strColunas += "\"" + colunas[i][property] + "\"";
                    } else {
                        strColunas += colunas[i][property];
                    }

                    if (i < (colunas.length - 1)) {
                        strColunas += ",";
                    }
                }

                return strColunas;
            }
        };


        /**
        * @descrição: Obtém o fator somatório que será aplicado em cada coluna da lista
        * @params: widthInicial (int): total do width, já pode vir com o tamanho das colunas de numeração e do checkbox
        **/
        var _getFatorSomatorio = function (widthInicial) {
            var w = configUserFinal.width;

            // verifica se o width da tabela é por PERCENTAGEM
            if (typeof configUserFinal.width == "string") {
                if (configUserFinal.width.indexOf("%") >= 0) {
                    // obtém o objeto
                    var t = document.getElementById(configUserFinal.id);

                    if (t == null) {
                        Prion.Log({ msg: "Elemento " + configUserFinal.id + " não existe", from: showLog.Lista });
                        return 0;
                    }

                    // obtém o width real do objeto
                    w = t.offsetWidth;

                    if (0 == w) {
                        w = t.parentElement.offsetWidth;
                    }
                }
            }

            w = parseInt(w);


            // tamanho da lista menos o tamanho da coluna com o número da linha e menos o tamanho da coluna com o checkbox
            //var restanteWidth = w - widthInicial;
            //var somatorioWidth = 0;
            //var widthColunas = 0;

            var somatorioColunas = widthInicial; // já inicial com o width (coluna de numeração + checkbox, por exemplo)
            var tamanhoUltrapassouWidth = false;


            // obtém o somatório dos width de cada coluna
            for (var i = 0; i < configUserFinal.columns.length; i++) {

                // verifica se o tamanho da coluna foi informado
                if ((configUserFinal.columns[i].width != null) && (configUserFinal.columns[i].width != "")) {
                    if (typeof configUserFinal.columns[i].width == "number") {
                        somatorioColunas += configUserFinal.columns[i].width;
                    } else {
                        somatorioColunas += parseInt(configUserFinal.columns[i].width.replace("px", ""), 10);
                    }
                } else {
                    // pega o tamanho default da coluna
                    somatorioColunas += configUserFinal.table.body.config.width;
                }
            }


            // somatorioColunas -> total da colunas sem contabilizar a coluna NÚMERO LINHA e a coluna CHECKBOX
            // restanteWidth -> tamanho da janela diminuindo o tamanho da coluna NÚMERO LINHA e o tamanho da coluna CHECKBOX
            tamanhoUltrapassouWidth = (somatorioColunas > w);

            /*
            // obtém o somatório dos width de cada coluna
            if (!tamanhoUltrapassouWidth) {
            for (var i = 0; i < configUserFinal.columns.length; i++) {
            // verifica se o tamanho da coluna foi informado
            if ((configUserFinal.columns[i].width != null) && (confconfigUserFinaligTable.columns[i].width != "")) {
            restanteWidth -= parseInt(configUserFinal.columns[i].width.replace("px", ""), 10);
            //somatorioWidth += parseInt(configUserFinal.columns[i].width.replace("px", ""), 10);
            widthColunas += parseInt(configUserFinal.columns[i].width.replace("px", ""), 10);
            } else {
            restanteWidth -= configUserFinal.table.body.config.width;
            somatorioWidth += configUserFinal.table.body.config.width;
            widthColunas += configUserFinal.table.body.config.width;
            }
            }
            // FIM somatório...
            }*/

            if (tamanhoUltrapassouWidth) {
                // retorna um objeto com os atributos: 
                // * fator (neste caso 1, pois o tamanho das colunas ultrapassou o tamanho da lista) 
                // * tamWidth (tamanho do width, pois ultrapassou o tamanho da lista)
                return { fator: 1, tamWidth: somatorioColunas, aplicarFator: false };
            } else {
                // retorna um objeto com os atributos: 
                // - fator (neste caso é maior do que 1, pois o tamanho das colunas é menor do que o tamanho da lista
                // - tamWidth (neste caso, o tamWidth é o tamanho default do lista)
                return { fator: (w / somatorioColunas), tamWidth: w, aplicarFator: true };
            }
        };


        /**
        * @descrição: Retorna todas as linhas checked da lista
        **/
        var _getSelected = function () {
            // obtém apenas os elementos checkbox 
            var linesBody = document.querySelectorAll("#" + _configDefault.id + " table tbody tr input[name=cb_" + _configDefault.id + "]"); //input[type=checkbox]");

            // retorna null caso não tenha encontrado nenhum elemento
            if (linesBody == null) { return null; }

            var selecionados = new Array();
            for (var i = 0; i < linesBody.length; i++) {

                // verifica se o item da iteração atual esta checked
                if (linesBody[i].checked) {

                    // insere no array o objeto da iteração atual
                    var o = new Object();
                    o.rowNumber = i;
                    o.object = _configDefault.table.body.rows[i];

                    selecionados.push(o);
                }
            }

            return (selecionados.length == 0) ? null : selecionados;
        };


        /**
        * @descrição: Função recursiva que obtém o valor de uma propriedade, varrendo todo os seus objetos
        * @param: obj (object): objeto 
        * @param: nome (string): nome do campo no seguinte formato: (Nome ou Usuario.Nome, por exemplo)
        * @parma: nivel (integer): nível utilizado no campo nome
        * @return: valor
        **/
        var _getValueRow = function (obj, nome, nivel) {

            if (nome.indexOf(".") < 0) { return obj[nome]; }

            var a = nome.split("."),
                o = obj[a[nivel]];

            if (o == null) {
                return "";
            }

            if (typeof o == "object") {
                return _getValueRow(o, nome, (nivel + 1));
            }

            return o;
        };


        /**
        * @descrição: Método que faz o carregamento dos registros
        * @return: void
        **/
        var _load = function (config/**, url*/) {
            // verifica se o usuário informou a configuração do load (paginação, order by, bla bla bla)
            var configAtualFoiInformada = (config != null);

            // se a configuração atual não foi informada, obtém a do cache
            if (!configAtualFoiInformada) {
                config = _configLoad;
            }

            // verifica os atributos de paging
            if (config == null) {
                config = {};
                config.paging = {};
                config.paging.currentPage = 1;
                config.paging.totalPerPage = _configDefault.paging.totalPerPage;
            } else if (config.paging == null) {
                //config = _configLoad;
                config = Prion.Apply(_configLoad, config);
            }

            // mesmo o parâmetro paging ter sido informado, o usuário quis ser malandro e não informou os atributos necessários.
            // eu sou mais malandro que o usuário, então sendo assim, verifico...
            if (config.paging.currentPage == null || config.paging.currentPage == 0) {
                config.paging.currentPage = 1;
            }

            if (config.paging.totalPerPage == null || config.paging.totalPerPage == 0) {
                config.paging.totalPerPage = _configDefault.paging.totalPerPage;
            }


            var filter = null;

            // verifica se o usuário iniciou a lista com algum filtro
            if ((configUserFinal.filter != null) && (configUserFinal.filter != "")) {
                filter = configUserFinal.filter;

                if (typeof configUserFinal.filter == "string") {
                    filter = Prion.StringToObject(configUserFinal.filter);
                }

                config.filter = filter;
            }


            var url = (arguments[1] != null) ? arguments[1] : _configDefault.url;

            // cria a variável data e atribui a ela apenas os atributos utilizados para a paginação
            // apenas se config.paging.show = true
            var data = {};


            // se tiver paginação, cria os parâmetros
            if (configUserFinal.paging.show) {
                data.totalPerPage = config.paging.totalPerPage;
                data.currentPage = config.paging.currentPage;
            }


            // verifica se existe o SORT
            if (config.paging.sort != null) {
                data.sort = config.paging.sort;
            } else {
                data.sort = _sortAtual;
            }


            // armazena em cache a configuração atual utilizada para este load;
            _configLoad = config;

            // define o tipo de retorno da lista
            data.returnType = _configDefault.request.returnType;

            // cria um objeto 'configFilter' para garantir que apenas o atributo 'filter' e/ou 'query' sejam jogados para dentro do objeto 'data'

            // aplica o filtro ao objeto data. O objeto data é passado na requisição
            data = Prion.Apply(data, config.filter);
            var configQuery = {
                query: config.query
            };
            data = Prion.Apply(data, configQuery);

            _disableButtons();
            _acertarPaginacao();

            // se for uma lista local, sai fora...
            if (_configDefault.type == Prion.Lista.Type.Local) {
                return;
            }

            _showMask();


            var leftScroll = document.getElementById(_configDefault.id + "Body").scrollLeft;

            $.ajax({
                type: "POST",
                url: url,
                data: data,
                success: function (retorno) {

                    try {

                        // verifica se é para redirecionar para outra página
                        if ((retorno.isRedirect != null) && (retorno.isRedirect)) {
                            // redireciona para a URL
                            window.location.href = retorno.redirectTo;
                            return;
                        }

                        // obtém o total de registros obtidos na consulta
                        var totalRegistros = (retorno.total == null) ? 0 : retorno.total;

                        // total de registros por página
                        _configDefault.paging.totalPerPage = _configDefault.paging.totalPerPage;

                        // página atual
                        _configDefault.paging.currentPage = (retorno.page == null) ? 1 : retorno.page;

                        // total de páginas que o grid terá
                        _configDefault.paging.totalPages = (Math.ceil(totalRegistros / _configDefault.paging.totalPerPage));

                        // desmarca o checkbox do header
                        _checkboxHeader.checked = false;

                        // verifica se é para realizar um parseJson na requisição. Irá gerar com isso objetos baseados na string
                        if (_configDefault.request.returnType == Prion.Lista.ReturnType.String) {
                            retorno.rows = $.parseJSON(retorno.rows);
                        }

                        _clear();
                        _createRows(retorno.rows, retorno);
                        _acertarPaginacao(retorno);
                        _alterStateButton(_buttonRemove, false);
                        _alterStateButton(_buttonUpdate, (retorno.rows.length > 0));
                        _applyTheme();

                        // define o left scroll
                        document.getElementById(_configDefault.id + "Body").scrollLeft = leftScroll;
                        
                        // se existir o atributo 'afterLoad' e se ele for do tipo 'function'...
                        if ((config.afterLoad != null) && (typeof config.afterLoad == "function")) {
                            // ... chama a função definida 
                            config.afterLoad.call(this);
                        }

                        _hideMask();
                    } catch (err) {
                        // se caiu aqui é porque deu algum erro na requisição

                        _hideMask();

                        // exibe um log no console do browser
                        Prion.Log({
                            type: "error",
                            msg: err
                        });

                        // exibe uma mensagem de erro para o usuário
                        Prion.Mensagem({
                            mensagem: { TextoMensagem: "Erro ao carrega a lista. Por favor, tente novamente.", ClassName: "errorbox" },
                            hide: false // indica que a mensagem não sumirá, a não ser que o usuário clique no botão de fechar
                        });
                    }
                },
                error: function (retorno) {
                    _hideMask();
                }
            });
        };


        /**
        * @descrição: atualiza os dados de um objeto e do seu HTML, baseado no indice do array
        **/
        var _modifyRow = function (indice, obj) {
            configUserFinal.table.body.rows[indice] = obj;

            var columnsHeader = configUserFinal.table.header.config.columns,
                indiceTdBody = 0;

            if (configUserFinal.rowNumber.show) {
                indiceTdBody += 1;
            }

            if (configUserFinal.checkbox.show) {
                indiceTdBody += 1;
            }

            // obtém todas as colunas do header da tabela e cria a coluna
            for (var i = 0; i < columnsHeader.length; i++) {

                // obtém o atributo 'nameJson' que esta na TH da TR
                var nameJson = columnsHeader[i].nameJson;

                if (nameJson == null) {
                    continue;
                }

                // ATUALIZA O CONTEÚDO DO TD
                _tbody.children[indice].children[(i + indiceTdBody)].children[0].children[0].innerHTML = obj[nameJson];
            }
        };


        /**
        * @descrição: retorna um JSON de todos os registros da lista
        * @return: string no formato JSON com todos os registros da lista
        **/
        var _serialize = function () {

            if ((configUserFinal.table.body.rows == null) || (0 == configUserFinal.table.body.rows.length)) {
                return "[]";
            }

            var str = "[",
                temp = "",
                element = null;

            for (var i = 0; i < configUserFinal.table.body.rows.length; i++) {
                // obtém o objeto atual
                var o = configUserFinal.table.body.rows[i],
                    objHeader = null;

                for (var p in o) {
                    objHeader = configUserFinal.table.header.get(p);

                    // se não for um customField, vai para a próxima iteração
                    if ((objHeader == null) || (objHeader.customField == null)) {
                        continue;
                    }

                    // se chegou aqui é porque é uma coluna customizada (a famosa CustomField, rs)
                    // obtém todos os elementos do tipo INPUT desta linha (children[i])
                    var childrens = _tbody.children[i].getElementsByTagName("input"),
                        nameJsonInput = "";

                    for (var j = 0; j < childrens.length; j++) {
                        nameJsonInput = childrens[j].getAttribute("namejson");

                        if (nameJsonInput == null) {
                            continue;
                        }

                        // verifica se o atributo 'nameson' da coluna atual (que é um input) é o mesmo 'namejson' do header atual
                        // se for, sai fora
                        if (nameJsonInput.toLowerCase() == objHeader.customField.config.attributes.namejson.toLowerCase()) {
                            element = childrens[j];
                            break;
                        }
                    }

                    // verifica se encontrou o elemento
                    if (element == null) {
                        continue;
                    }

                    switch (objHeader.customField.config.type.toLowerCase()) {
                        case "text":
                            {
                                // verifica se possui o atributo 'mask'
                                var attr = element.getAttribute("mask");

                                // se não existir o atributo, pega o seu value rapidão, sem chorumelas
                                if (attr == null) {
                                    o[p] = element.value;
                                    break;
                                }

                                // verifica se o atributo é do tipo money
                                if (attr.toLowerCase() == "money") {
                                    // há, é do tipo money sim, faz então macetes no seu value
                                    // remove o currency (R$, U$)
                                    // troca o '.' por ''
                                    // troca a ',' por '.'
                                    o[p] = parseFloat(element.value.replace("R$", "").replace(".", "").replace(",", "."));
                                }

                                break;
                            }
                        case "checkbox":
                            {
                                o[p] = element.checked;
                                break;
                            }
                    }
                }

                temp = JSON.stringify(o);

                if (i < (configUserFinal.table.body.rows.length - 1)) {
                    temp += ",";
                }

                str += temp;
            }

            str += "]";

            return str;
        };


        /**
        * @descrição: Método responsável por exibir a máscara
        **/
        var _showMask = function (/**message*/) {

            // se não for para exibir a máscara, sai fora...
            if (!_configDefault.mask.show) { return; }

            // se o objeto já existir, da um block
            if (_mask != null) {
                _mask.style.display = "block";

                _divMaskMsg.getElementsByTagName("label")[0].textContent = (arguments[0] == null) ? _configDefault.mask.text : arguments[0];
                _divMaskMsg.style.display = "block";
                return;
            }

            // se chegou até aqui, é porque o objeto mask ainda não existe;
            var divMask = document.querySelectorAll("#" + _configDefault.id + " div." + _configDefault.mask.className),
                divMaskMsg = document.querySelectorAll("#" + _configDefault.id + " div.maskListMsg");


            if ((divMask != null) && (divMask.length > 0)) {
                _mask = divMask[0];
                _divMaskMsg = divMaskMsg[0];
            }


            if (_mask != null) {
                _mask.style.display = "block";
                _divMaskMsg.style.display = "block";
            }
        };


        var _hideMask = function () {
            if (_mask == null) { return; }
            _mask.style.display = "none";
            _divMaskMsg.style.display = "none";
        };



        // cria o HTML
        _init(configUserFinal);

        // se autoLoad for true, carrega os registros
        if (configUserFinal.autoLoad) {
            _load();
        }


        return {
            /**
            * @descrição: configuração do grid
            **/
            config: configUserFinal,

            /**
            * @descrição: Adiciona um botão na lista
            * @params: config (object). Parâmetro de configuração que será aplicado ao botão
            * @exemplo:
            * {
            *   title: "Novo",
            *   className: "buttons",
            *   icon: { src: imgPathPrion + "/new.png" },
            *   owner: OWNER (dono do botão),
            *   click: novoClick
            * }
            *
            * @return: this
            **/
            addButton: function (config) {
                if (Prion.Theme == null) {
                    return;
                }

                // define o owner deste botão como sendo a div que tem o class = 'shortcuts-icons'
                config.owner = document.querySelectorAll("#" + _configDefault.id + " div.shortcuts-icons");

                if ((config.owner == null) || (config.owner.length == 0)) {
                    config.owner = null;
                }
                else {
                    config.owner = config.owner[0];
                }

                config.className = "buttons";
                config.type = "into";
                Prion.Theme[Prion.settings.theme].Button.Create(config);

                return this;
            },


            /**
            * @descrição: adiciona uma coluna à tabela
            * @return: this
            **/
            addColumn: function (column) {
                _addColumn(column);
                return this;
            },


            /**
            * @descrição: adiciona uma linha à tabela
            * @return: this
            **/
            addRow: function (row) {
                _createRows(row);
                return this;
            },


            /**
            * @descrição: aplica os novos parâmetros de configuração do usuário aos parâmetros de configuração da lista
            * @return: objeto lista
            **/
            apply: function (arg0) {
                var c = (this.config || _configDefault);
                this.config = Prion.Apply(c, arg0);

                return this;
            },

            /**
            * @descrição: apaga todos os registros da lista
            * @return: void
            **/
            clear: function () {
                _clear();
                return this;
            },

            /**
            * @descrição: exclui uma linha da tabela.
            * @param: o => pode ser um id (índice da linha) 
            *              pode ser um objeto (que conterá o índice da linha)
            *              pode ser uma string (que conterá o índice das linhas separado por vírgula
            * @param: msg => mensagem de confirmação. Se não for informado, usa a mensagem default
            **/
            deleteRow: function (o, msg) {
                // verifica se precisa pedir confirmação para exclusão do registro selecionado
                if (!configUserFinal.buttons.remove.confirm) {
                    _deleteRow(o);
                    return;
                }

                // se caiu aqui é porque deve pedir confirmação antes de excluir o registro selecionado

                msg = msg || configUserFinal.buttons.remove.confirmMessage;

                // pede confirmação do usuário para exclusão dos registros
                if (!confirm(msg)) { return; }

                _deleteRow(o);
            },

            /**
            * @descrição: exclui todas as linhas selecionadsa (checkbox = true)
            * @param: config => object => config.msg = mensagem de confirmação
            *                             config.ajax (opcional, default true) = true/false. 
            *                           . Se for true, será feita uma requisição AJAX e o deleteRow só será executado se o retorno da requisição for true.
            *                           . Se for false, o deleteRow será chamado direto config.url = url que será chamada
            **/
            deleteSelected: function (config) {
                // pede confirmação do usuário para exclusão dos registros
                if (!confirm(config.msg)) { return; }

                var ajax = ((config.ajax == null) || (config.ajax));
                if (!ajax) {
                    _deleteRow(this.getSelected());
                    return;
                }

                _deleteRow(this.getSelected());
            },


            /**
            * @descrição: Desabilita um botão
            **/
            disableButton: function (botao) {
                var configBotao = this.config.buttons[botao];
                if (configBotao == null) { return; }

                _alterStateButton(configBotao.dom, false);
            },


            /**
            * @descrição: Habilita um botão
            **/
            enableButton: function (botao) {
                var configBotao = this.config.buttons[botao];
                if (configBotao == null) { return; }

                _alterStateButton(configBotao.dom, true);
            },

            /**
            * @descrição: retorna o primeiro objeto selecionado
            * @param: idRow (integer, opcional): se indicado, retornará a linha indicada
            **/
            get: function (/**idRow*/) {

                var indice = 0;

                // verifica se o parâmetro idRow foi informado
                if (typeof arguments[0] != "undefined") {
                    indice = parseInt(arguments[0], 10);
                }

                return (configUserFinal.table.body.rows[indice]);
            },


            /**
            * @descrição: retorna todas as colunas do grid. Se o parâmetro 'onlyVisible' for true, irá retornar apenas as colunas visíveis
            * @param: onlyVisible (boolean, opcional): se indicado, retornará apenas as colunas visíveis 
            **/
            getColumns: function (/**onlyVisible**/) {
                var onlyVisible = arguments[0] || false;
                var ordenarResultado = false;
                var columns = _getColumns(onlyVisible, "nameJson", ordenarResultado, false);

                return columns;
            },


            getHeaderColumns: function (/**onlyVisible**/) {
                var onlyVisible = arguments[0] || false;
                var ordenarResultado = true;
                var columns = _getColumns(onlyVisible, "header", ordenarResultado, true);

                return columns;
            },


            /**
            * @descrição: retorna um array de objetos que conterá todos os registros selecionados (registros que o checkbox esta true)
            **/
            getSelected: function () {
                return _getSelected();
            },


            /**
            * @descrição: por enquanto ainda dependente de jQuery para fazer a requisição AJAX
            * @params: config (object): config.parseJson (boolean): indica se irá ou não fazer um parseJson no retorno da requisição
            *                           config.paging (object): objeto de configuração da paginação
            *                           config.afterLoad (function): função que será executada após um load completo
            **/
            load: function (config/**, url*/) {
                if (_load != null) {
                    _load.call(this, config, arguments[1]);
                }
            },


            /**
            * @descrição: modifica os dados de uma linha
            * @return: this
            **/
            modifyRow: function (indice, obj) {
                _modifyRow(indice, obj);
                return this;
            },


            /**
            * @descrição: Retorna as linhas da tabela
            * @returna: um array com todas as linhas da tabela (da página atual, caso seja uma tabela com paginação)
            **/
            rows: function () {
                if (configUserFinal == null) { return null; }
                if (configUserFinal.table == null) { return null; }
                if (configUserFinal.table.body == null) { return null; }

                return (configUserFinal.table.body.rows || new Array());
            },


            /**
            * @descrição: serializa todas as colunas e linhas da tabela
            * @return: tabela serializada
            **/
            serialize: function () {
                return _serialize();
            },


            /**
            * @descrição: define a visibilidade de uma coluna, representada pelo atributo 
            * @params: column (pode ser um int, que irá representar o índice, ou uma string, que irá representar o header)
            **/
            setColumnVisible: function (visible, column) {
                this.config.table.header.setVisible(visible, column);
            },


            /**
            * @descrição: modifica o title do header
            **/
            setTitle: function (title) {
                alert("rs, em breve!!!");
            },


            /**
            * @descrição: modifica o titulo de uma coluna (pode ser baseado no indice ou no header atual)
            * @params: column (pode ser um int, que irá representar o índice, ou uma string, que irá representar o header)
            **/
            setTitleHeader: function (newTitle, column) {
                var indiceColuna = column;

                if (typeof column == "string") {

                }
            },


            /**
            * @descricao: Define o value de uma coluna/linha
            * @params: column => nameJson de uma coluna OU o índice de uma coluna
            * @params: row => indice de uma linha
            * @params: value => valor que a coluna assumirá
            * @return: void
            **/
            setValue: function (column, row, value) {
                console.log("rs, em breve!!");
            },


            /**
            * @descrição: Exibe a lista
            * @return: void
            **/
            show: function () {
                var el = document.getElementById(configUserFinal.id);
                if (el != null) { el.style.display = "block"; }
            },


            /**
            * @descrição: exibe o header
            **/
            showHeader: function () {
                this.config.table.header.show();
            },


            /**
            * @descrição: oculta o header
            **/
            hideHeader: function () {
                this.config.table.header.hide();
            },


            /**
            * @descrição: exibe o footer
            **/
            showFooter: function () {
                this.config.table.footer.show();
            },


            /**
            * @descrição: oculta o footer
            **/
            hideFooter: function () {
                this.config.table.footer.hide();
            },


            /**
            * @descrição: oculta a lista
            **/
            hide: function () {
                var el = document.getElementById(configUserFinal.id);
                if (el != null) { el.style.display = "none"; }
            }
        };
    };



    Prion.Lista.Header = function (configUser) {

        var _configDefault = {
            id: "", // id do elemento PAI (neste caso, é uma TABLE)
            class: "myClassHeader", // class do header
            visible: true, // indica que o header será exibido
            widthDefaultCell: 100, // width default da celula da table
            heightDefaultCell: 40, // height default da celula da table
            columns: {}
        };

        // atribui as configurações do usuário ao objeto de configuração default
        configUser = Prion.Apply(_configDefault, configUser);

        return {

            config: configUser,

            /**
            * @descrição: Obtém o objeto a partir do nameJson
            **/
            get: function (nameJson) {
                if ((_configDefault.columns == null) || (_configDefault.columns.length == 0)) {
                    return null;
                }

                for (var i = 0; i < _configDefault.columns.length; i++) {
                    if (_configDefault.columns[i].nameJson == nameJson) {
                        return _configDefault.columns[i];
                    }
                }

                return null;
            },

            /**
            * @descrição: define a visibilidade do header, ou de uma coluna, caso o segundo argumento (column) tenha sido informado
            * @params: column (pode ser um int, que irá representar o índice, ou uma string, que irá representar o header)
            **/
            setVisible: function (visible/**, column**/) {
                if (arguments[1] == null) {
                    this.config.visible = visible;
                    if (visible) { this.show(); } else { this.hide(); }
                } else {
                    // verifica se o argumento é do tipo int
                    if (typeof arguments[1] == "integer") {
                        // oculta a coluna atráves do seu indice
                    } else if (typeof arguments[1] == "string") {
                        // oculta a coluna através do seu header
                    }
                }
            },

            /**
            * @descrição: modifica o title de um header
            * @params: column (pode ser um int, que irá representar o índice, ou uma string, que irá representar o header)
            **/
            setTitle: function (title, column) {

            },

            /**
            * @descrição: exibe o header
            **/
            show: function () {
                this.config.vibile = true;
                //document.querySelectorAll("#" + this.config.id + " table thead")[0].style.display = "block";
                _thead.style.display = "block";
            },

            /**
            * @descrição: oculta o header
            **/
            hide: function () {
                this.config.vibile = false;
                //document.querySelectorAll("#" + this.config.id + " table thead")[0].style.display = "none";
                _thead.style.display = "none";
            }
        };
    };



    Prion.Lista.Body = function (configUser) {

        var _configDefault = {
            width: 100, // width default da celula da table
            height: 40, // height default da celula da table
            class: "myClassBody", // class do body
            rows: {}
        };

        // atribui as configurações do usuário ao objeto de configuração default
        configUser = Prion.Apply(_configDefault, configUser);

        return {
            config: configUser
        };
    };



    /**
    * @descrição: Objeto que representa o footer da TABLE
    **/
    Prion.Lista.Footer = function (configUser) {

        var _configDefault = {
            id: "", // id do elemento PAI (neste caso, é uma TABLE)
            class: "myClassFooter", // class do footer
            visible: true, // indica que o footer será exibido
            height: 40, // height default do footer
            checkbox: { show: false },
            columns: {}
        };

        // atribui as configurações do usuário ao objeto de configuração default
        configUser = Prion.Apply(_configDefault, configUser);

        return {

            config: configUser,

            /**
            * @descrição: define a visibilidade do footer
            **/
            setVisible: function (visible) {
                if (visible) { this.show(); } else { this.hide(); }
            },

            /**
            * @descrição: exibe o footer
            **/
            show: function () {
                this.config.vibile = true;
                document.querySelectorAll("#" + this.config.id + " table tfoot")[0].style.display = "block";
            },

            /**
            * @descrição: oculta o footer
            **/
            hide: function () {
                this.config.vibile = false;
                document.querySelectorAll("#" + this.config.id + " table tfoot")[0].style.display = "none";
            }
        };
    };


    /**
    * @descrição: Objeto que representa uma TD de uma TABLE
    **/
    Prion.Lista.Column = function (configUser) {

        var _configDefault = {
            visible: true, // indica que o header será exibido
            width: 100, // width default da TD da table
            height: 40 // height default da TD da table
        };

        return {

            /**
            * @descrição: adiciona uma coluna
            **/
            add: function () {

            }
        };
    };


    /**
    * @descrição: Objeto que representa uma TR (com várias TD, representados pelo objeto _configDefault.columns) de uma TABLE
    **/
    Prion.Lista.Row = function (configUser) {

        var _configDefault = {
            visible: true, // indica que a linha será exibida
            height: 40, // height default da TR da table
            columns: {} // array de objetos do tipo Prion.Lista.Column
        };

        return {

        };
    };


    /**
    * @descrição: Objeto que indica o tipo de retorno de uma requisição. String ou Object
    *             Se for string, será feita uma requisição utilizando DataTable e será aplicado
    *               um parseJson no retorno desta requisição. Se for object, será feita uma requisição usando
    *               usando lista de objetos, e nada precisa ser feito no retorno desta requisição.
    **/
    Prion.Lista.ReturnType = {
        String: 0,
        Object: 1
    };

    Prion.Lista.Type = {
        Local: 0,
        Remote: 1
    };


    /**
    * @descrição: Objeto CustomField para a lista (elementos do tipo input 
    **/
    Prion.Lista.CustomField = {};
    Prion.Lista.CustomField.Input = function (config) {

        var _configElement = config;
        var _input = null;

        // cria o elemento
        var _init = function (config) {

            var input = document.createElement("INPUT");
            input.type = config.type;
            //input.id = Prion.GenerateId();
            //input.name = config.name;

            switch (config.type.toLowerCase()) {
                case "checkbox":
                    {
                        input.checked = config.value || false;
                        break;
                    }
                case "text":
                    {
                        input.value = "";
                        break;
                    }
            }

            // verifica se existe o objeto Prion.className
            if (Prion.settings.ClassName != null) {
                Prion.addClass(input, Prion.settings.ClassName.checkbox);
            }

            // adiciona o atributos apenas se existir
            if (config.attributes != null) {
                for (var p in config.attributes) {
                    input.setAttribute(p, config.attributes[p]);

                    // verifica se o atributo atual é 'mask'
                    if (p.toLowerCase() == "mask") {
                        // se for, aplica o objeto de máscara, apenas se o método existir
                        if ((Prion != null) && (Prion.Mascara != null)) {
                            Prion.Mascara(input);
                        }
                    }
                }
            }

            // retorna o elemento com os seus atributos definidos
            return input;
        };

        // inicia o objeto, criando o elemento
        _input = _init(config);

        return {
            /**
            * @descrição: retorna a configuração do elemento HTML
            **/
            config: _configElement,

            /**
            * @descrição: retorna um elemento já criado
            **/
            html: _input,

            /**
            * @descrição: cria um elemento baseado em outro utilizando as suas configurações
            **/
            create: function (config) {
                var newInput = _init(config);
                return newInput;
            }
        }
    };

} ());