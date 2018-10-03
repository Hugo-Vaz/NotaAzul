/**
* @descrição: Inicia todas as configurações default para este projeto
* @return: void
**/

var _prionSettings; 

(function () {

    // inicia o objeto
    _prionSettings = {};

    // define o theme utilizado neste projeto
    _prionSettings.theme = "Cupcake";


    // configurações default para o objeto Prion.Window
    _prionSettings.Window = {
        //alwaysCentralized: true,
        modal: true,
        width: 740
    };


    // configurações default para o objeto Prion.Lista
    _prionSettings.Lista = {
        autoLoad: true,
        height: 500,
        paging: {
            totalPerPage: 40
        }
    };


    // configurações default (theme Cupcake) com o className dos elementos
    _prionSettings.ClassName = {
        button: "st-button",
        buttonReset: "st-clear",
        checkbox: "uniform",
        input: "uniform",
        select: "uniform",
        radiobutton: "uniform"
    };

    // configurações default para o JS de login
    _prionSettings.Login = {
        url: "Login/Login"
    };


    // configuração default com o id do elemento onde as mensagens das requisições serão exibidas
    _prionSettings.Mensagem = {
        id: "divMensagem"
    };


    // configuração default para moeda
    _prionSettings.Moeda = {
        simbolo: "R$"
    };


    // configuração default da barra de título da página
    // OBRIGATÓRIO
    _prionSettings.Title = {
        btnRefresh: "Clique para atualizar esta página"
    };


})();



// objeto 'objConfiguracaoSistema' obtido através do controler. Esse objeto esta em /Views/Shared/_Layout.cshtml
NotaAzul.config = {
    configuracaoSistema: objConfiguracaoSistema
};

/**
* @descrição: Retorna uma configuração de sistema através de um atributo
**/
NotaAzul.GetConfig = function (nomeAtributo) {
    if (NotaAzul.config.configuracaoSistema == null) {
        return;
    }

    var arr = NotaAzul.config.configuracaoSistema;
    var valor = "";
    var item = null;

    for (var i = 0; i < arr.length; i++) {
        valor = arr[i]["Atributo"];

        if (valor == nomeAtributo.toLowerCase()) {
            return arr[i];
        }
    }

    return item;
}



$().ready(function () {

    var main = document.getElementById("main");
    if (main == null) { return; }

    // só executa a função onresize se o elemento 'main' existir

    function ResizeMain() {
        // obtém o width do elemento com id 'main' e id 'sidebar'
        var widthMain = main.clientWidth;
        //var offsetHeightMain = main.offsetHeight;

        var widthSidebar = document.getElementById("sidebar").clientWidth;

        // define o tamanho do elemento com id 'page'
        document.getElementById("page").style.width = (widthMain - widthSidebar - 4) + "px"; // 4 é por causa das bordas e bla bla bla

        // define o tamanho do sidebar. Elemento com id 'sidebar'
        //document.getElementById("sidebar").style.height = offsetHeightMain.toString() + "px";
    }

    ResizeMain();
    window.onresize = ResizeMain;
});