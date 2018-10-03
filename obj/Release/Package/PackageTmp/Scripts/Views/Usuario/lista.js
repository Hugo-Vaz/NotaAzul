/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Usuario == null) {
        NotaAzul.Usuario = {};
    }

    NotaAzul.Usuario.Lista = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
        var _grid = null,
            _windowUsuario = null,
            _permissoes = null;

        /**
        * @descrição: Método responsável por abrir um registro
        * @params: obj => objeto que representa o registro clicado
        * @return: void
        **/
        var _abrir = function (obj) {
            _windowUsuario.apply({
                object: 'Usuario',
                url: rootDir + "Usuario/Carregar/" + obj.Id,
                ajax: { onlyJson: true },
                fnBeforeLoad: NotaAzul.Usuario.Dados.Novo,
                success: function (retorno) {

                    if ((retorno == null) || (retorno.obj == null)) {
                        return;
                    }


                    // REMOVE os atributos 'mandatory' e 'message' para os elementos abaixo
                    // desta forma a senha não é mais obrigatória, porem joga a senha para um campo hidden
                    document.getElementById("Usuario_Senha").removeAttribute("mandatory");
                    document.getElementById("Usuario_Senha").removeAttribute("message");

                    document.getElementById("Usuario_Confirmacao").removeAttribute("mandatory");
                    document.getElementById("Usuario_Confirmacao").removeAttribute("message");
                    // FIM

                    // define a senha no campo hidden
                    "Usuario_SenhaAtual".setValue(retorno.obj.Senha);


                    var listaGrupoUsuario = retorno.obj.Grupos;
                    if ((listaGrupoUsuario == null) || (listaGrupoUsuario.length === 0)) {
                        return;
                    }

                    // se o elemento 'comboboxUsuarioGrupo' não existir, nada será feito
                    Prion.ComboBox.SelecionarIndexPeloValue("comboboxUsuarioGrupo", listaGrupoUsuario[0].Id);

                    // define o nome do grupo de usuário
                    var $lblGrupoUsuario = document.getElementById("nomeGrupoUsuario");
                    if ($lblGrupoUsuario != null) {
                        $lblGrupoUsuario.style.removeProperty("display");
                        $lblGrupoUsuario.innerHTML = listaGrupoUsuario[0].Nome;
                    }
                }
            }).load().show({ animate: true });
        };

        /**
        * @descrição: Carrega o conteúdo do html Usuario/ViewDados em background, acelerando o futuro carregamento
        **/
        var _carregarJanelaUsuario = function () {

            // carrega a ViewDados de Usuário
            _windowUsuario = new Prion.Window({
                url: rootDir + "Usuario/ViewDados/",
                id: "popupUsuario",
                height: 540,
                width: 620,
                title: { text: "Usuário" },
                object: ["Usuario", "UsuarioGrupo"], // objetos que serão carregados
                fnAfterLoad: function (retorno) {

                    var possuiPermissao = false;
                    var form = document.getElementById("frmUsuario");

                    // verifica se tem permissão: 'Alterar Senha'
                    possuiPermissao = Permissoes.Possui("USUARIO", "Alterar Senha");
                    if (!possuiPermissao) {
                        document.getElementById("divUsuarioSenha1").style.display = "none";
                        document.getElementById("divUsuarioSenha2").style.display = "none";
                    }
                    // FIM


                    // verifica se tem permissão: 'Alterar Grupo de Usuário'
                    possuiPermissao = Permissoes.Possui("USUARIO", "Alterar Grupo de Usuário");
                    if (!possuiPermissao) {
                        document.getElementById("divComboBoxUsuarioGrupo").style.display = "none";
                    }
                    // FIM

                },
                observer: {
                    load: function () { // o objeto Window irá controlar a execução desta function
                        NotaAzul.Usuario.Lista.Grid().load();
                    }
                },
                buttons: {
                    show: (!_permissoes.onlyView), // se onlyView for false, significa então que os botões deverão ser exibidos
                    buttons: [
                        {
                            text: "Salvar",
                            className: Prion.settings.ClassName.button,
                            typeButton: "save", // usado para controle interno
                            click: function (win) {
                                NotaAzul.Usuario.Dados.Salvar(win);
                            }
                        },
                        {
                            text: "Limpar",
                            className: Prion.settings.ClassName.buttonReset,
                            type: "reset",
                            click: function (win) {
                                NotaAzul.Usuario.Dados.Novo();
                            }
                        }
                    ]
                }
            });
        };


        /**
        * @descrição: Cria o todo o html da lista
        * @return: void
        **/
        var _criarLista = function () {
            if (_grid != null) { return; }

            _grid = new Prion.Lista({
                id: "listaUsuarios",
                url: rootDir + "Usuario/GetLista/",
                title: { text: "Usuários" },
                filter: { Entidades: "Situacao" },
                request: { base: "Usuario" }, // colocado pois o SQL não está com ALIAS para os campos da tabela UsuarioGrupo
                buttons: {
                    insert: { show: _permissoes.insert, click: NotaAzul.Usuario.Lista.Novo },
                    update: { show: _permissoes.update, click: NotaAzul.Usuario.Lista.Abrir },
                    remove: { show: _permissoes.remove, url: rootDir + "Usuario/Excluir/" },
                    onlyView: { show: _permissoes.onlyView }
                },
                action: { onDblClick: NotaAzul.Usuario.Lista.Abrir },
                columns: [
                    { header: "Nome", nameJson: "Nome", width: "280px" },
                    { header: "Login", nameJson: "Login", width: "180px" },
                    { header: "Email", nameJson: "Email", width: "200px" },
                    { header: "Status", nameJson: "Situacao.Nome", width: "100px" },
                    { header: "Cadastrado em", nameJson: "DataCadastro", type: "date", width: "110px" }
                ]
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
            _permissoes = Permissoes.CRUD("USUARIO");

            _criarLista();
            _carregarJanelaUsuario();
        };

        /**
        * @descrição: Método responsável por abrir uma popup para inserção de novo registro
        * @return: void
        **/
        var _novo = function () {
            _windowUsuario.show({
                animate: true,
                fnBefore: NotaAzul.Usuario.Dados.Novo
            });
        };

        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {
            Abrir: _abrir,
            Grid: _getGrid,
            Iniciar: _iniciar,
            Novo: _novo
        };

    } ();

    NotaAzul.Usuario.Lista.Iniciar();
} (window, document));