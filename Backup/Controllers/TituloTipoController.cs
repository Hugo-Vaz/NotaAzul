using System;
using System.Web.Mvc;
using GenericHelpers = Prion.Generic.Helpers;

namespace NotaAzul.Controllers
{
    public class TituloTipoController : BaseController
    {
        #region "Views"
        /// <summary>
        /// Retorna a view Lista, com os registros de TituloTipo
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
            ViewModels.TituloTipo.vmDados vmDados = new ViewModels.TituloTipo.vmDados();
            Business.Situacao biSituacao = new Business.Situacao();

            //carrega a lista de situações de um TituloTipo
            vmDados.Situacoes = biSituacao.CarregarSituacoesPeloTipo("Tipo Título");

            return View("Dados", vmDados);
        }

        #endregion "Views"


        #region "Requisições"

        /// <summary>
        /// Carrega um registro do tipo Prion.Generic.Models.TituloTipo através do seu Id
        /// </summary>
        /// <param name="id">id do TituloTipo</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
            Business.TituloTipo biTituloTipo = new Business.TituloTipo();
            Prion.Generic.Models.TituloTipo tituloTipo = (Prion.Generic.Models.TituloTipo)biTituloTipo.Carregar(id).Get(0);

            return Json(new { success = true, obj = tituloTipo }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Exclui um Tipo de Título pelo Id
        /// </summary>
        /// <returns>Um retorno no formato de Json</returns>
        public ActionResult Excluir()
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.TituloTipo biTituloTipo = new Business.TituloTipo();
            GenericHelpers.Retorno retorno = biTituloTipo.Excluir(Prion.Tools.Conversor.ToInt(Request.Form["id"]));

            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = (retorno.Sucesso) ? "Tipo de título excluído com sucesso" : retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Salva os dados de um TituloTipo
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult Salvar(ViewModels.TituloTipo.vmDados vModel)
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.TituloTipo biTituloTipo = new Business.TituloTipo();
            GenericHelpers.Retorno retorno = biTituloTipo.Salvar(vModel.TituloTipo);

            mensagem.EstadoObjeto = retorno.EstadoObjeto;
            mensagem.UltimoId = retorno.UltimoId;
            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Retorna uma lista no formato JSON do tipo Prion.Generic.Models.TituloTipo
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLista()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.TituloTipo biTituloTipo = new Business.TituloTipo();

            Prion.Generic.Models.Lista lista = biTituloTipo.ListaTiposTitulo(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}