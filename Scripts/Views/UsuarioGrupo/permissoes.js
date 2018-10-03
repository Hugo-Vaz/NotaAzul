/**
* @autor: Thiago Motta Zappaterra
* @email: tmottaz@gmail.com
* @descrição: Classe responsável por criação da popup de permissões, além de 
*             toda requisição relacionada a permissões de grupos de usuários
**/
/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";


if (NotaAzul.UsuarioGrupo == null) {
    NotaAzul.UsuarioGrupo = {};
}

NotaAzul.UsuarioGrupo.Permissoes = function () {
    "use strict";
    /******************************************************************************************
    ** PRIVATE
    ******************************************************************************************/
    var _todosSelecionados = false,
        _winPermissoes = null;

    /**
    * @descrição: Carrega o conteúdo do html UsuarioGrupo/ViewPermissoes em background, acelerando o futuro carregamento
    **/
    var _carregarJanelaPermissoes = function () {

        // carrega a ViewPermissoes de UsuarioGrupo
        _winPermissoes = new Prion.Window({
            url: rootDir + "UsuarioGrupo/ViewPermissoes/",
            id: Prion.GenerateId(),
            height: 500,
            width: 600,
            title: { text: "Permissões para o Grupo de Usuário:" },
            buttons: {
                show: true,
                buttons: [
                    {
                        text: "Marcar todos",
                        className: "button-green",
                        click: function (win) {
                            NotaAzul.UsuarioGrupo.Permissoes.SelecionarTodos(this);
                        }
                    },
                    {
                        text: "Salvar",
                        className: Prion.settings.ClassName.button,
                        typeButton: "save", // usado para controle interno
                        click: function (win) {
                            NotaAzul.UsuarioGrupo.Permissoes.SalvarPermissao(win);
                        }
                    },
                    {
                        text: "Cancelar",
                        className: Prion.settings.ClassName.buttonReset,
                        type: "reset",
                        click: function (win) {
                            win.hide({ animate: true });
                        }
                    }
                ]
            }
        });
    };

    /**
    * @descrição: Carrega todas as permissões já definidas para o grupo de usuário selecionado
    * @return: void
    **/
    var _carregarPermissoes = function (grupoUsuario) {

        // abre a janela da view de permissões e carrega todas as permissões do Grupo de Usuário selecionado
        _winPermissoes.apply({
            url: rootDir + "UsuarioGrupo/PermissoesGrupoUsuario/" + grupoUsuario.Id,
            title: { text: "Permissões para o Grupo de Usuário: " + grupoUsuario.Nome },
            ajax: { onlyJson: true },
            success: function (retorno) {
                // desmarca todos os checkbox
                $("#listaPermissoes input[type=checkbox]").each(function () { this.checked = false; });

                if ((retorno == null) || (retorno.permissoesGrupoUsuario == null)) { return; }

                var arr = JSON.parse(retorno.permissoesGrupoUsuario).groupBy("IdUsuarioGrupo", "IdUsuarioGrupo");
                if ((arr[0] == null) || (arr[0].values.length === 0)) { return; }

                // validação. Se todos estiverem marcados, muda o estado do botão 'Selecionar Todos'
                if ($("#listaPermissoes input[type=checkbox]").length === arr.length) {

                    var text = "Desmarcar todos";
                    // TMZ - VER
                    // Obter os botões da janela e mudar o text
                }

                $("#listaPermissoes input[type=checkbox]").each(function () {
                    if (this.value == "on") { return; }

                    for (var i = 0; i < arr[0].values.length; i++) {
                        if (this.value == arr[0].values[i].IdPermissao.toString()) {
                            this.checked = true;
                        }
                    }
                });


                // esse bloco abaixo irá verificar se todos os filhos de um módulo estão marcados
                // se todos tiverem marcados, marca o checkbox do módulo

                // obtém a lista de H3 contidos em #listaPermissoes
                $("#listaPermissoes h3").each(function () {

                    // obtém o checkbox do header
                    var cb = $(this).find("input")[0],
                            count = 0,
                            total = $(this).next().children("input").length;

                    // varre todos os children do tipo input
                    $(this).next().children("input").each(function () {
                        if (this.checked) { count += 1; }
                        if (count === total) { cb.checked = true; }
                    });
                });
                // FIM da verificação
            }
        }).load().show({ animate: true });
    };

    /**
    * @descrição: Carrega todas as permissões já definidas para o Grupo de Usuário selecionado e marca os respectivos checkbox
    * @return: void
    **/
    var _carregarPermissaoGrupoUsuario = function () {

        // obtém um objeto do PRIMEIRO registro selecionado
        var grupoUsuario = NotaAzul.UsuarioGrupo.Lista.Grid().getSelected();
        grupoUsuario = (grupoUsuario == null) ? null : grupoUsuario[0].object;

        // exibe mensagem caso o usuário não tenha selecionado nenhum registro
        if (grupoUsuario == null) {
            Prion.Alert({ msg: "Selecione primeiro um registro." });
            return;
        }


        "idUsuarioGrupo".setValue(grupoUsuario.Id);


        // carrega APENAS as permissões visíveis para o grupo de usuário informado
        Prion.Request({
            url: rootDir + "UsuarioGrupo/PermissoesVisiveis/" + grupoUsuario.Id,
            success: function (retorno) {
                if ((retorno == null) || (retorno.modulos == null)) {
                    return;
                }

                // monta uma lista com as permissões visíveis para este grupo de usuário
                NotaAzul.UsuarioGrupo.Permissoes.MontarListaPermissoesVisiveis(retorno.modulos, grupoUsuario);
            }
        });
    };

    /**
    * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
    * @return: void
    **/
    var _iniciar = function () {
        _carregarJanelaPermissoes();
    };

    /**
    * @descrição: Monta uma lista com todos os módulos e permissões visíveis para o grupo de usuário informado
    * @return: void
    **/
    var _montarListaPermissoesVisiveis = function (modulos, grupoUsuario) {
        var arr = JSON.parse(modulos).groupBy("Modulo.Descricao", "Modulo.Id,Modulo.Descricao"),
                $div = document.getElementById("listaPermissoes");
        $div.style.cssText += "margin:14px;";
        $div.textContent = "";

        // se não houver nenhum registro na lista, sai fora...
        if (arr.length === 0) {
            return;
        }

        // Necessário ser assim por causa dos eventos existentes aqui dentro
        var fnCriarCheckbox = function (objAtual) {
            // cria um elemento A que conterá o nome do módulo
            var aHref = document.createElement("a");
            //aHref.href = "#";
            aHref.className = "cursor";
            aHref.innerText = objAtual["Modulo.Descricao"];
            aHref.rel = "id_" + objAtual["Modulo.Id"];
            aHref.style.cssText += "top:-2px; position:relative;";

            Prion.Event.add(aHref, "click", function () {
                var n = this.rel.replace("id_", ""),
                            cb = this.previousElementSibling;
                cb.checked = !cb.checked;

                // se o checkbox do header for false, muda o valor da variável _todosSelecionados também para false
                if (!cb.checked) {
                    _todosSelecionados = false;

                    var button = _winPermissoes.buttons(0);
                    button.html.value = "Selecionar todos";
                }

                $("#listaPermissoes input[name='modulo_" + n + "']").each(function () {
                    this.checked = cb.checked;
                });
            });

            // cria um CHECKBOX que entrará antes do elemento A (nome do módulo)
            var cb = document.createElement("input");
            cb.className = "cursor";
            cb.type = "checkbox";
            cb.name = "id_" + objAtual["Modulo.Id"];
            Prion.Event.add(cb, "click", function () {
                var n = cb.name.replace("id_", "");

                // se o checkbox do header for false, muda o valor da variável _todosSelecionados também para false
                if (!cb.checked) {
                    _todosSelecionados = false;

                    var button = _winPermissoes.buttons(0);
                    button.html.value = "Selecionar todos";
                }

                $("#listaPermissoes input[name='modulo_" + n + "']").each(function () {
                    this.checked = cb.checked;
                });
            });

            var h3 = document.createElement("h3");
            h3.appendChild(cb);
            h3.appendChild(aHref);

            // se estiver no segundo H3, adiciona um padding-top
            if (i >= 1) {
                h3.style.cssText += "padding-top:10px;";
            }

            var $divPermissoes = document.createElement("div");
            $divPermissoes.style.cssText += "padding-top:6px; margin-left:24px; position:relative;";

            // armazena dentro da DIV todos os checkbox e descrição de cada permissão do módulo atual
            for (var j = 0; j < objAtual.values.length; j++) {

                // cria um CHECKBOX para cada permissão
                var $input = document.createElement("input");
                $input.id = Prion.GenerateId();
                $input.className = "cursor";
                $input.type = "checkbox";
                $input.name = "modulo_" + objAtual.values[j]["Modulo.Id"];
                $input.value = objAtual.values[j]["Permissao.Id"];

                if (objAtual.values[j]["Menu.Id"] > 0) {
                    $input.setAttribute("idmenu", objAtual.values[j]["Menu.Id"]);
                }

                $divPermissoes.appendChild($input);

                // cria um LABEL para cada permissão
                var $label = document.createElement("label");
                $label.setAttribute("for", $input.id);
                $label.innerText = objAtual.values[j]["Permissao.Descricao"];
                $label.className = "cursor";

                $divPermissoes.appendChild($label);

                // cria um BR
                var $br = document.createElement("br");
                $br.style.cssText = "line-height: 20px;";

                $divPermissoes.appendChild($br);
            }

            $div.appendChild(h3);
            $div.appendChild($divPermissoes);
        };

        // monta a treeview
        for (var i = 0; i < arr.length; i++) {

            if (arr[i].values.length === 0) { continue; }

            // função auto executável.
            // Necessário ser assim por causa dos eventos existentes aqui dentro
            fnCriarCheckbox.call(this, arr[i]);
        }


        this.CarregarPermissoes(grupoUsuario);
    };

    /**
    * @descrição: Salva todas as permissões selecionadas
    * @params: window (object): Objeto representando a popup
    * @return: void
    **/
    var _salvarPermissao = function (win) {

        if (win != null) { win.mask(); }

        var listaPermissoes = "";

        $("#listaPermissoes input[type=checkbox]").each(function () {
            // por causa checkbox ao lado do nome do módulo
            if (this.value === "on") { return; }

            // verifica se o checkbox atual esta DESMARCADO
            if (!this.checked) { return; }

            // adiciona o value do checkbox à listaPermissoes
            listaPermissoes += this.value + ",";
        });


        // remove o último caractere (",")
        if (listaPermissoes !== "") {
            listaPermissoes = listaPermissoes.substring(0, listaPermissoes.length - 1);
        }

        var idUsuarioGrupo = "idUsuarioGrupo".getValue();

        Prion.Request({
            url: rootDir + "UsuarioGrupo/SalvarPermissoes",
            data: "idUsuarioGrupo=" + idUsuarioGrupo + "&permissoes=" + listaPermissoes,
            success: function (retorno) {
                if (win != null) { win.mask(); }
            },
            error: function () {
                if (win != null) { win.mask(); }
            }
        });
    };


    /**
    * @descrição: Marca/Desmarca todos os checkbox da lista e muda o label do botão (faz um toogle, rs)
    * @return: void
    **/
    var _selecionarTodos = function (botao) {
        _todosSelecionados = !_todosSelecionados;

        // muda a descrição do botão
        botao.value = (_todosSelecionados) ? "Desmarcar todos" : "Selecionar todos";

        $("#listaPermissoes input[type=checkbox]").each(function () { this.checked = _todosSelecionados; });
    };

    /******************************************************************************************
    ** PUBLIC
    ******************************************************************************************/
    return {
        CarregarPermissoes: _carregarPermissoes,
        CarregarPermissaoGrupoUsuario: _carregarPermissaoGrupoUsuario,
        Iniciar: _iniciar,
        MontarListaPermissoesVisiveis: _montarListaPermissoesVisiveis,
        SalvarPermissao: _salvarPermissao,
        SelecionarTodos: _selecionarTodos
    };
} ();
NotaAzul.UsuarioGrupo.Permissoes.Iniciar();