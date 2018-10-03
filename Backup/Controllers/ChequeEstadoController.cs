using System;
using System.Web.Mvc;
using GenericHelpers = Prion.Generic.Helpers;

namespace NotaAzul.Controllers
{
    public class ChequeEstadoController : BaseController
    { 
        #region "Views"
        /// <summary>
        /// Retorna a view Lista, com os registros do Estado do Cheque
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
            ViewModels.ChequeEstado.vmDados vmDados = new ViewModels.ChequeEstado.vmDados();
            Business.Situacao biSituacao = new Business.Situacao();

            //carrega a lista de situações de um Estado do Cheque
            vmDados.Situacoes = biSituacao.CarregarSituacoesPeloTipo("Estados de um Cheque");

            return View("Dados", vmDados);
        }

        #endregion "Views"


        #region "Requisições"

        /// <summary>
        /// Carrega um registro do tipo Prion.Generic.Models.ChequeEstado através do seu Id
        /// </summary>
        /// <param name="id">id do Estado do Cheque</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
            Business.ChequeEstado biChequeEstado = new Business.ChequeEstado();
            Prion.Generic.Models.ChequeEstado chequeEstado = (Prion.Generic.Models.ChequeEstado)biChequeEstado.Carregar(id).Get(0);

            return Json(new { success = true, obj = chequeEstado }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Exclui um Estado do Cheque pelo Id
        /// </summary>
        /// <returns>Um retorno no formato de Json</returns>
        public ActionResult Excluir()
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.ChequeEstado biChequeEstado = new Business.ChequeEstado();
            GenericHelpers.Retorno retorno = biChequeEstado.Excluir(Prion.Tools.Conversor.ToInt(Request.Form["id"]));

            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = (retorno.Sucesso) ? "Estado do Cheque excluído com sucesso" : retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Salva os dados de um Estado do Cheque
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult Salvar(ViewModels.ChequeEstado.vmDados vModel)
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.ChequeEstado biChequeEstado = new Business.ChequeEstado();
            GenericHelpers.Retorno retorno = biChequeEstado.Salvar(vModel.ChequeEstado);

            mensagem.EstadoObjeto = retorno.EstadoObjeto;
            mensagem.UltimoId = retorno.UltimoId;
            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Retorna uma lista no formato JSON do tipo Prion.Generic.Models.ChequeEstado
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLista()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.ChequeEstado biChequeEstado = new Business.ChequeEstado();

            Prion.Generic.Models.Lista lista = biChequeEstado.ListaChequesEstado(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
