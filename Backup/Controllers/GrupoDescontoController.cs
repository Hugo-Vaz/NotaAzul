using System;
using System.Web.Mvc;
using GenericHelpers = Prion.Generic.Helpers;

namespace NotaAzul.Controllers
{
    public class GrupoDescontoController : BaseController
    {
        #region "Views"
        /// <summary>
        /// Retorna a view Novo
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewDados()
        {
            ViewModels.GrupoDesconto.vmDados vmDados = new ViewModels.GrupoDesconto.vmDados();

            //carrega a lista de situações de um GrupoDesconto
            Business.Situacao biSituacao = new Business.Situacao();
            vmDados.Situacoes = biSituacao.CarregarSituacoesPeloTipo("Grupo de Desconto");         

            return View("Dados", vmDados);
        }

        /// <summary>
        /// Retorna a view Lista, com os registros de GrupoDesconto
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewLista()
        {
            return View("Lista");
        }

        #endregion "Views"


        #region "Requisições"

        /// <summary>
        /// Carrega um registro do tipo Models.GrupoDesconto atraves do seu Id
        /// </summary>
        /// <param name="id">id do GrupoDesconto</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);

            Business.GrupoDesconto biGrupoDesconto = new Business.GrupoDesconto();
            Models.GrupoDesconto grupoDesconto = (Models.GrupoDesconto)biGrupoDesconto.Carregar(parametros, id).Get(0);

            return Json(new { success = true, obj = grupoDesconto }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Exclui um GrupoDesconto pelo Id
        /// </summary>
        /// <param name="id">id do GrupoDesconto</param>
        /// <returns>Um retorno no formato de Json</returns>
        public ActionResult Excluir()
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.GrupoDesconto biGrupoDesconto = new Business.GrupoDesconto();
            GenericHelpers.Retorno retorno = biGrupoDesconto.Excluir(Prion.Tools.Conversor.ToInt(Request.Form["id"]));

            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = (retorno.Sucesso) ? "GrupoDesconto excluído com sucesso" : retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }


        /// <summary>
        /// Salva os dados de um GrupoDesconto
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult Salvar(ViewModels.GrupoDesconto.vmDados vModel)
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.GrupoDesconto biGrupoDesconto = new Business.GrupoDesconto();

            GenericHelpers.Retorno retorno = biGrupoDesconto.Salvar(vModel.GrupoDesconto);

            mensagem.EstadoObjeto = retorno.EstadoObjeto;
            mensagem.UltimoId = retorno.UltimoId;
            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }


        /// <summary>
        /// Retornar uma lista no formato JSON do tipo Models.GrupoDesconto
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLista()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.GrupoDesconto biGrupoDesconto = new Business.GrupoDesconto();

            Prion.Generic.Models.Lista lista = biGrupoDesconto.ListaGrupoDesconto(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}