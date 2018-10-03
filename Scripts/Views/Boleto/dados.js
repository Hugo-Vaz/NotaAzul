/*jshint jquery:true */
var NotaAzul = NotaAzul || {},
        Prion = Prion || {},
        Permissoes = Permissoes || {},
        EstadoObjeto = EstadoObjeto || {},
        rootDir = rootDir || "";

$(window).ready(function (window, document) {

    if (NotaAzul.Boleto == null) {
        NotaAzul.Boleto = {};
    }

    NotaAzul.Boleto.Dados = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/
       

        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
            _definirAcaoBotoes();
        };

        /**
        * @descrição: Define ação para todos os controles da tela 
        **/
        var _definirAcaoBotoes = function () {

            Prion.Event.add("Boleto_Anexo", "change", function (event) {
                var files = event.target.files;
                _anexos = new FormData();

                _anexos.append("upload", files[0]);
            });
        };

        /**
        * @descrição: Salva todos os dados do form
        **/
        var _salvarImagem = function (win) {

            if (win !== null) {
                win.mask();
            }

            var xhr = new XMLHttpRequest();
            xhr.open("POST", rootDir + "Boleto/UploadArquivoRetorno", true);
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4) { // `DONE`
                    var status = xhr.status;
                    if (status == 200) {
                        var retorno = JSON.parse(xhr.responseText);

                        win.mask();
                        win.hide({ animate: true });
                        win.config.reloadObserver = true;

                        Prion.Mensagem({
                            success: retorno.success,
                            mensagem: {
                                ClassName: retorno.mensagem.ClassName,
                                TextoMensagem: retorno.mensagem.TextoMensagem,
                                Tipo: (retorno.success) ? "Sucesso" : "Erro"
                            }
                        });
                    }
                }
            };

            xhr.send(_anexos);
        };

        /**
        * @descrição: métodos que serão executados quando for abrir para novo registro
        **/
        var _novoBoleto = function () {

        };
        /******************************************************************************************
        ** PUBLIC
        ******************************************************************************************/
        return {

            Iniciar: _iniciar,
            Novo: _novoBoleto,
            Salvar: _salvarImagem
        };
    }();
    NotaAzul.Boleto.Dados.Iniciar();
}(window, document));