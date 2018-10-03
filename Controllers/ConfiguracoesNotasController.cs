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
    public class ConfiguracoesNotasController:BaseController
    {
        #region "Views"


        /// <summary>
        /// Retorna a view ConfiguracoesNotas
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewConfiguracoesNotas()
        {
            ViewData["FormaDeConceito"] = Sistema.GetConfiguracaoSistema("notas:forma_conceito");
            ViewData["DivisaoAnoLetivo"] = Sistema.GetConfiguracaoSistema("notas:divisao_ano_letivo");
            ViewData["MediaMinima"] = Sistema.GetConfiguracaoSistema("notas:media_minima");
            return View("ConfiguracoesNotas");
        }
        #endregion "Views"

        #region "Requisições"
        /// <summary>
        /// Salva as configurações do esquema de notas/conceitos
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult Salvar()
        {
            GenericHelpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            List<Prion.Generic.Models.ConfiguracaoSistema> configuracoes = new List<Prion.Generic.Models.ConfiguracaoSistema>();

            Business.ConfiguracoesNotas biConfig = new Business.ConfiguracoesNotas();
            Prion.Generic.Models.ConfiguracaoSistema config = new Prion.Generic.Models.ConfiguracaoSistema();

            config.Atributo = "notas:divisao_ano_letivo";
            config.Valor = Request.Form["FormaDeDivisaoAnoLetivo"];
            config.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado;
            configuracoes.Add(config);

            config = new Prion.Generic.Models.ConfiguracaoSistema();
            config.Atributo = "notas:forma_conceito";
            config.Valor = Request.Form["FormaDeConceito"];
            config.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado;
            configuracoes.Add(config);


            config = new Prion.Generic.Models.ConfiguracaoSistema();
            config.Atributo = "notas:media_minima";
            config.Valor = Request.Form["MediaMinima"];
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