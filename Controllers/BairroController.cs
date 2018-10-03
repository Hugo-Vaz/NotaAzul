using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.Controllers
{
    public class BairroController : BaseController
    {
        #region "Views"

        /// <summary>
        /// Retorna a view Lista, com os registros de Bairro
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewLista()
        {
            return View("Lista");
        }

        /// <summary>
        /// Retorna a view Novo
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewDados()
        {
            ViewModels.Bairro.vmDados vmDados = new ViewModels.Bairro.vmDados();
            return View("Dados", vmDados);
        }

        #endregion "Views"


        #region "Requisições"

        /// <summary>
        /// Carrega um registro do tipo Models.Bairro atráves do seu ID
        /// </summary>
        /// <param name="id">id do Bairro</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
            Business.Bairro biBairro = new Business.Bairro();
            GenericModels.Bairro bairro = (GenericModels.Bairro)biBairro.Carregar(id).Get(0);

            return Json(new { success = true, obj = bairro }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Exclui um Bairro pelo ID
        /// </summary>
        /// <param name="id">id do Bairro</param>
        /// <returns>retorno no formato JSON</returns>
        public ActionResult Excluir()
        {
            Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
            Business.Bairro biBairro = new Business.Bairro();
            GenericHelpers.Retorno retorno = biBairro.Excluir(Prion.Tools.Conversor.ToInt(Request.Form["id"]));

            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = (retorno.Sucesso) ? "Bairro excluído com sucesso" : retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Salva os dados de um Bairro
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult Salvar(ViewModels.Bairro.vmDados vModel)
        {
            Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
            Business.Bairro biBairro = new Business.Bairro();
            GenericHelpers.Retorno retorno = biBairro.Salvar(vModel.Bairro);

            mensagem.EstadoObjeto = retorno.EstadoObjeto;
            mensagem.UltimoId = retorno.UltimoId;
            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Retorna uma lista no formato JSON de objetos do tipo Models.Bairro
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLista()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.Bairro biBairro = new Business.Bairro();

            Prion.Generic.Models.Lista lista = biBairro.ListaBairros(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
