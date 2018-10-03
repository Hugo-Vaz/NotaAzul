/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        EstadoObjeto = EstadoObjeto || {},
        md5 = md5 || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {
    "use strict";
    if (NotaAzul.Usuario == null) {
        NotaAzul.Usuario = {};
    }

    NotaAzul.Usuario.Dados = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/

        /**
        * @descrição: Carrega os combobox desta janela
        * @return: void
        **/
        var _carregarCombobox = function () {
            NotaAzul.UsuarioGrupo.ComboBox.Gerar();
            NotaAzul.Funcionario.ComboBox.Gerar();

            Prion.ComboBox.Carregar({
                url: rootDir + "Funcionario/GetLista",
                el: "listaProfessores",
                filter: "Paginar=false&Cargo.Nome=Professora&Entidades=Cargo",
                clear: true,
                valueDefault: true
            });
        };

        /**
        * @descrição: Método que será chamado sempre após salvar 
        **/
        var _executarAposSalvar = function (retorno) {
            NotaAzul.Usuario.Dados.Novo();
            return false;
        };

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            _carregarCombobox();
        };

        /**
        * @descrição: Reseta o form, voltando os dados default
        **/
        var _novoUsuario = function () {
            Prion.ClearForm("frmUsuario", true);

            // ADICIONA os atributos 'mandatory' e 'message' para os elementos abaixo
            document.getElementById("Usuario_Senha").setAttribute("mandatory", true);
            document.getElementById("Usuario_Senha").setAttribute("message", "A senha não pode ser vazia");

            document.getElementById("Usuario_Confirmacao").setAttribute("mandatory", true);
            document.getElementById("Usuario_Confirmacao").setAttribute("message", "A senha precisa ser confirmada");
            // FIM


            _carregarCombobox();
        };

        /**
        * @descrição: Salva todos os dados do form
        **/
        var _salvar = function (win) {
            if (!Prion.ValidateForm("frmUsuario")) {
                win.config.reloadObserver = false;
                return false;
            }

            if (win != null) { win.mask(); }

            var gerarSenha = true;

            // verifica se é um registro existente
            if ("Usuario_Id".getValue() !== "") {
                // se o usuário não tiver informado a senha
                if ("Usuario_Senha".getValue().trim() === "") {
                    // usa a senha obtida pelo request
                    "Usuario_Senha".setValue("Usuario_SenhaAtual".getValue());
                    gerarSenha = false;
                }
            }

            if (gerarSenha) {
                var login = "Usuario_Login".getValue();
                var senha = "Usuario_Senha".getValue();

                // deixa apenas UM espaço
                login = login.replace(/\s{2,}/g, ' ');
                "Usuario_Senha".setValue(md5(login + senha));
            }


            Prion.Request({
                form: "frmUsuario",
                url: rootDir + "Usuario/Salvar",
                success: function (retorno, registroNovo) {
                    // se for um registro existente, fecha a janela
                    if ((!registroNovo) && (win != null)) {
                        win.config.reloadObserver = true;
                        win.hide({ animate: true });
                        return;
                    }

                    NotaAzul.Usuario.Dados.ExecutarAposSalvar();
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
            ExecutarAposSalvar: _executarAposSalvar,
            Iniciar: _iniciar,
            Novo: _novoUsuario,
            Salvar: _salvar
        };
    } ();

    NotaAzul.Usuario.Dados.Iniciar();
} (window, document));