using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GenericHelpers = Prion.Generic.Helpers;

namespace NotaAzul.Controllers
{
    public class MaterialController : BaseController
    {
        #region "Views"
        /// <summary>
        /// Retorna a view Lista, com os registros de Material
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
            ViewModels.Material.vmDados vmDados = new ViewModels.Material.vmDados();
            Business.Situacao biSituacao = new Business.Situacao();

            //carrega a lista de situacoes de um Material
            vmDados.Situacoes = biSituacao.CarregarSituacoesPeloTipo("Material");

            return View("Dados", vmDados);
        }

        #endregion "Views"


        #region "Requisições"

        /// <summary>
        /// Carrega um registro do tipo Models.Material atraves do seu Id
        /// </summary>
        /// <param name="id">id do Material</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
            Business.Material biMaterial = new Business.Material();
            Models.Material material = (Models.Material)biMaterial.Carregar(id).Get(0);

            return Json(new { success = true, obj = material }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Exclui um Material pelo Id
        /// </summary>
        /// <param name="id">id do Material</param>
        /// <returns>Um retorno no formato de Json</returns>
        public ActionResult Excluir()
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.Material biMaterial = new Business.Material();
            GenericHelpers.Retorno retorno = biMaterial.Excluir(Prion.Tools.Conversor.ToInt(Request.Form["id"]));

            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = (retorno.Sucesso) ? "Material excluído com Sucesso" : retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Salva os dados de um Material
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult Salvar(ViewModels.Material.vmDados vModel)
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.Material biMaterial = new Business.Material();
            GenericHelpers.Retorno retorno = biMaterial.Salvar(vModel.Material);

            mensagem.EstadoObjeto = retorno.EstadoObjeto;
            mensagem.UltimoId = retorno.UltimoId;
            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Retornar uma lista no formato JSON do tipo Models.Material
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLista()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.Material biMaterial = new Business.Material();

            Prion.Generic.Models.Lista lista = biMaterial.ListaMateriais(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
