using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.Controllers
{
    public class CorporacaoController : BaseController
    {
        #region "Views"

        /// <summary>
        /// Retorna a view Novo
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewDados()
        {
            ViewModels.Corporacao.vmDados vmDados = new ViewModels.Corporacao.vmDados();
            return View("Dados", vmDados);
        }

        #endregion "Views"


        #region "Requisições"

        /// <summary>
        /// Carrega um registro do tipo Models.Corporacao atráves do seu ID
        /// </summary>
        /// <param name="id">id da Corporacao</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
            Business.Corporacao biCorporacao = new Business.Corporacao();
            GenericModels.Corporacao corporacao = biCorporacao.Carregar(id);

            return Json(new { success = true, obj = corporacao }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Salva os dados de uma Corporação
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult Salvar(ViewModels.Corporacao.vmDados vModel)
        {
            Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
            Business.Corporacao biCorporacao = new Business.Corporacao();
            GenericHelpers.Retorno retorno = biCorporacao.Salvar(vModel.Corporacao);

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
