using System;
using System.Web.Mvc;
using GenericHelpers = Prion.Generic.Helpers;

namespace NotaAzul.Controllers
{
    public class MovimentacaoFinanceiraController:BaseController
    {
        #region "Views"
        /// <summary>
        /// Retorna a view Lista, com os registros de MovimentacaoFinanceira
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
            ViewModels.MovimentacaoFinanceira.vmDados vmDados = new ViewModels.MovimentacaoFinanceira.vmDados();
           
            return View("Dados", vmDados);
        }

        #endregion "Views"


        #region "Requisições"

        /// <summary>
        /// Carrega um registro do tipo Models.MovimentacaoFinanceira através do seu Id
        /// </summary>
        /// <param name="id">id de MovimentacaoFinanceira</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
            Business.MovimentacaoFinanceira biMovimentacaoFinanceira = new Business.MovimentacaoFinanceira();
            Models.MovimentacaoFinanceira movimentacaoFinanceira = (Models.MovimentacaoFinanceira)biMovimentacaoFinanceira.Carregar(id).Get(0);

            return Json(new { success = true, obj = movimentacaoFinanceira }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Exclui uma MovimentacaoFinanceira pelo Id
        /// </summary>
        /// <returns>Um retorno no formato de Json</returns>
        public ActionResult Excluir()
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.MovimentacaoFinanceira biMovimentacaoFinanceira = new Business.MovimentacaoFinanceira();
            GenericHelpers.Retorno retorno = biMovimentacaoFinanceira.Excluir(Prion.Tools.Conversor.ToInt(Request.Form["id"]));

            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = (retorno.Sucesso) ? "Movimentação Financeira excluída com sucesso" : retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Salva os dados de uma MovimentacaoFinanceira
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult Salvar(ViewModels.MovimentacaoFinanceira.vmDados vModel)
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.MovimentacaoFinanceira biMovimentacaoFinanceira = new Business.MovimentacaoFinanceira();
            GenericHelpers.Retorno retorno = biMovimentacaoFinanceira.Salvar(vModel.MovimentacaoFinanceira);

            mensagem.EstadoObjeto = retorno.EstadoObjeto;
            mensagem.UltimoId = retorno.UltimoId;
            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Retorna uma lista no formato JSON do tipo Models.MovimentacaoFinanceira
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLista()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.MovimentacaoFinanceira biMovimentacaoFinanceira = new Business.MovimentacaoFinanceira();

            Prion.Generic.Models.Lista lista = biMovimentacaoFinanceira.ListaMovimentacoesFinanceira(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}