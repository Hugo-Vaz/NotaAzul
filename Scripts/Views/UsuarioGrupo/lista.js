/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "",
        imgPathPrion = imgPathPrion || "";


if (NotaAzul.UsuarioGrupo == null) {
    NotaAzul.UsuarioGrupo = {};
}

NotaAzul.UsuarioGrupo.Lista = function () {
    "use strict";
    /******************************************************************************************
    ** PRIVATE
    ******************************************************************************************/
    var _grid = null,
        _windowUsuarioGrupo = null,
        _permissoes = null;

    /**
    * @descrição: Método responsável por abrir um registro
    * @params: obj => objeto que representa o registro clicado
    * @return: void
    **/
    var _abrir = function (obj) {
        _windowUsuarioGrupo.apply({
            object: 'UsuarioGrupo',
            url: rootDir + "UsuarioGrupo/Carregar/" + obj.Id,
            obj: obj,
            ajax: { onlyJson: true },
            fnBeforeLoad: NotaAzul.UsuarioGrupo.Dados.Novo
        }).load().show({ animate: true });
    };

    /**
    * @descrição: Carrega o conteúdo do html UsuarioGrupo/ViewDados em background, acelerando o futuro carregamento
    **/
    var _carregarJanelaUsuarioGrupo = function () {

        // carrega a ViewDados de UsuarioGrupo
        _windowUsuarioGrupo = new Prion.Window({
            url: rootDir + "UsuarioGrupo/ViewDados/",
            id: "popupUsuarioGrupo",
            height: 262,
            width: 490,
            title: { text: "Grupo de Usuário" },
            observer: {
                load: function () { // o objeto Window irá controlar a execução desta function
                    NotaAzul.UsuarioGrupo.Lista.Grid().load();
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
                            NotaAzul.UsuarioGrupo.Dados.Salvar(win);
                        }
                    },
                    {
                        text: "Limpar",
                        className: Prion.settings.ClassName.buttonReset,
                        type: "reset",
                        click: function (win) {
                            NotaAzul.UsuarioGrupo.Dados.Novo();
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
            id: "listaUsuarioGrupo",
            url: rootDir + "UsuarioGrupo/GetLista/",
            title: { text: "Grupo de Usuários" },
            filter: { Entidades: "Situacao" },
            request: { base: "UsuarioGrupo" }, // colocado pois o SQL não está com ALIAS para os campos da tabela UsuarioGrupo
            buttons: {
                insert: { show: _permissoes.insert, click: NotaAzul.UsuarioGrupo.Lista.Novo },
                update: { show: _permissoes.update, click: NotaAzul.UsuarioGrupo.Lista.Abrir },
                remove: { show: _permissoes.remove, url: rootDir + "UsuarioGrupo/Excluir/" },
                onlyView: { show: _permissoes.onlyView }
            },
            action: { onDblClick: NotaAzul.UsuarioGrupo.Lista.Abrir },
            columns: [
                { header: "Nome", nameJson: "Nome" },
                { header: "Descrição", nameJson: "Descricao" },
                { header: "Status", nameJson: "Situacao.Nome", width: "100px" },
                { header: "Cadastrado em", nameJson: "DataCadastro", type: "date", width: "110px" }
            ]
        });


        // adiciona um botão à lista GrupoUsuario
        _grid.addButton({
            click: function () {
                NotaAzul.UsuarioGrupo.Permissoes.CarregarPermissaoGrupoUsuario();
            },
            title: "Permissões",
            tooltip: "Permissões",
            icon: { src: imgPathPrion + "/key.png" }
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
        _permissoes = Permissoes.CRUD("USUARIO_GRUPO");

        _criarLista();
        _carregarJanelaUsuarioGrupo();
    };

    /**
    * @descrição: Método responsável por abrir uma popup para inserção de novo registro
    * @return: void
    **/
    var _novo = function () {
        _windowUsuarioGrupo.show({
            animate: true,
            fnBefore: NotaAzul.UsuarioGrupo.Dados.Novo
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
NotaAzul.UsuarioGrupo.Lista.Iniciar();