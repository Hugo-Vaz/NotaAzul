using System;
using System.Web.Mvc;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;


namespace NotaAzul.Controllers
{
    public class ChequeController:BaseController
    {
        #region "Views"

        /// <summary>
        /// Retorna a view Novo
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewDados()
        {
            ViewModels.Cheque.vmDados vmDados = new ViewModels.Cheque.vmDados();            
            return View("Dados", vmDados);
        }


        /// <summary>
        /// Retorna a view Lista, com os registros de Cheque
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewLista()
        {
            return View("Lista");
        }


        #endregion "Views"

        #region "Requisições"

        /// <summary>
        /// Carrega um registro do tipo GenericModels.Cheque atraves do seu Id
        /// </summary>
        /// <param name="id">id do Cheque</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);

            Business.Cheque biCheque = new Business.Cheque();
            GenericModels.Cheque cheque = (GenericModels.Cheque)biCheque.Carregar(parametros, id).Get(0);

            return Json(new { success = true, obj = cheque }, JsonRequestBehavior.AllowGet);
        }
           

        /// <summary>
        /// Exclui um Cheque pelo Id
        /// </summary>
        /// <returns>Um retorno no formato de Json</returns>
        public ActionResult Excluir()
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.Cheque biCheque = new Business.Cheque();
            GenericHelpers.Retorno retorno = biCheque.Excluir(Prion.Tools.Conversor.ToInt(Request.Form["id"]));

            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = (retorno.Sucesso) ? "Cheque excluído com sucesso" : retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }


        /// <summary>
        /// Salva os dados de um Cheque
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult Salvar(ViewModels.Cheque.vmDados vModel)
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.Cheque biCheque = new Business.Cheque();


            GenericHelpers.Retorno retorno = biCheque.Salvar(vModel.Cheque);

            mensagem.EstadoObjeto = retorno.EstadoObjeto;
            mensagem.UltimoId = retorno.UltimoId;
            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }


        /// <summary>
        /// Retorna uma lista no formato JSON do tipo Models.Cheque
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLista()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.Cheque biCheque = new Business.Cheque();

            Prion.Generic.Models.Lista lista = biCheque.ListaCheques(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}