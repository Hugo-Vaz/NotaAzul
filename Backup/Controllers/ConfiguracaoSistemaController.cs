using System;
using System.Web.Mvc;
using GenericHelpers = Prion.Generic.Helpers;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using GenericTools = Prion.Tools;

namespace NotaAzul.Controllers
{
    public class ConfiguracaoSistemaController:BaseController
    {
        #region "Views"


        /// <summary>
        /// Retorna a view ConfiguracaoDoSistema
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewConfiguracaoDoSistema()
        {
            ViewData["CidadeDefault"] = Sistema.GetConfiguracaoSistema("cidade:id_cidade_default");
            ViewData["EstadoDefault"] = Sistema.GetConfiguracaoSistema("estado:id_uf_default");            
            ViewData["DiaPagamentoDefault"] = Sistema.GetConfiguracaoSistema("mensalidade:dia_default_pagamento");
            ViewData["ListaBandeira"] = Sistema.GetConfiguracaoSistema("cartao_bandeira:lista");
            ViewData["TrocaMensalidade"] = Sistema.GetConfiguracaoSistema("mensalidade:dia_default_troca");

            return View("ConfiguracaoDoSistema");
        }
        #endregion "Views"

        #region "Requisições"
        /// <summary>
        /// Salva as configurações do sistema
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult Salvar()
        {
            GenericHelpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            List<Prion.Generic.Models.ConfiguracaoSistema> configuracoes = new List<Prion.Generic.Models.ConfiguracaoSistema>();
            
            Business.ConfiguracaoSistema biConfig = new Business.ConfiguracaoSistema();
            Prion.Generic.Models.ConfiguracaoSistema config = new Prion.Generic.Models.ConfiguracaoSistema();
            config.Atributo = "estado:id_uf_default";
            config.Valor = Request.Form["IdEstado"];
            config.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado;
            configuracoes.Add(config);

            config = new Prion.Generic.Models.ConfiguracaoSistema();
            config.Atributo = "cidade:id_cidade_default";
            config.Valor = Request.Form["IdCidade"];
            config.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado;
            configuracoes.Add(config);

            config = new Prion.Generic.Models.ConfiguracaoSistema();
            config.Atributo = "mensalidade:dia_default_pagamento";
            config.Valor = Request.Form["TrocaDiaPagamento"];
            config.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado;
            configuracoes.Add(config);

            config = new Prion.Generic.Models.ConfiguracaoSistema();
            config.Atributo = "mensalidade:dia_default_pagamento";
            config.Valor = Request.Form["DiaPagamento"];
            config.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado;
            configuracoes.Add(config);

            config = new Prion.Generic.Models.ConfiguracaoSistema();
            config.Atributo = "grau_parentesco:lista";
            config.Valor = Request.Form["ListaParentesco"];
            config.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado;
            configuracoes.Add(config);

            config = new Prion.Generic.Models.ConfiguracaoSistema();
            config.Atributo = "operadora_cartão:lista";
            config.Valor = Request.Form["listaCartao"];
            config.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado;
            configuracoes.Add(config);    

            GenericHelpers.Retorno retorno = biConfig.Salvar(configuracoes);

            mensagem.EstadoObjeto = retorno.EstadoObjeto;
            mensagem.UltimoId = retorno.UltimoId;
            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        #endregion
    }
}