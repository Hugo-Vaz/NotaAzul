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

    NotaAzul.Boleto.DadosRem = function () {
        /******************************************************************************************
        ** PRIVATE
        ******************************************************************************************/


        /**
        * @descrição: Inicia o objeto chamando todos os métodos que irão preparar a tela
        * @return: void
        **/
        var _iniciar = function () {
        };        

        /**
        * @descrição: Salva todos os dados do form
        **/
        var _salvar = function (win) {

            if (win !== null) {
                win.mask();
            }


            var mes = "Filtro_Mes".getDom().value;

            $.ajax({
                type: "GET",
                url: rootDir + "Boleto/GerarArquivoMes?Mes=" + mes,
                contentType: false,
                processData: false,
                success: function (result) {
                    win.mask();
                    if (result.success) {
                        window.open("http://pontodepartida-001-site1.itempurl.com/Boleto/Download?fileName=" + result.nomeArquivo);
                    }
                    else {
                        Prion.Mensagem({
                            success: result.success,
                            mensagem: {
                                ClassName: result.mensagem.ClassName,
                                TextoMensagem: result.mensagem.TextoMensagem,
                                Tipo: (result.success) ? "Sucesso" : "Erro"
                            }
                        });
                        return true;
                    }
                }
            });           
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
            Salvar: _salvar
        };
    }();
    NotaAzul.Boleto.DadosRem.Iniciar();
}(window, document));