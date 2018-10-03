using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GenericHelpers = Prion.Generic.Helpers;
using iTextSharp.text;
using System.Web;
using System.IO;

namespace NotaAzul.Controllers
{
    public class CargoController : BaseController
    {
        #region "Views"
        /// <summary>
        /// Retorna a view Lista, com os registros de Cargo
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
            ViewModels.Cargo.vmDados vmDados = new ViewModels.Cargo.vmDados();
            Business.Situacao biSituacao = new Business.Situacao();

            //carrega a lista de situacoes de um Cargo
            vmDados.Situacoes = biSituacao.CarregarSituacoesPeloTipo("Cargo");

            return View("Dados", vmDados);
        }

        #endregion "Views"


        #region "Requisições"

        /// <summary>
        /// Carrega um registro do tipo Models.Cargo atraves do seu Id
        /// </summary>
        /// <param name="id">id do Cargo</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
            Business.Cargo biCargo = new Business.Cargo();
            Prion.Generic.Models.Cargo cargo = (Prion.Generic.Models.Cargo)biCargo.Carregar(id).Get(0);

            return Json(new {success = true, obj = cargo }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Exclui um Cargo pelo Id
        /// </summary>
        /// <param name="id">id do Cargo</param>
        /// <returns>Um retorno no formato de Json</returns>
        public ActionResult Excluir()
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.Cargo biCargo = new Business.Cargo();
            GenericHelpers.Retorno retorno = biCargo.Excluir(Prion.Tools.Conversor.ToInt(Request.Form["id"]));

            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = (retorno.Sucesso) ? "Cargo excluído com Sucesso" : retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

           return Json(new {success = mensagem.Sucesso, mensagem = mensagem});
        }

        /// <summary>
        /// Salva os dados de um Cargo
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult Salvar(ViewModels.Cargo.vmDados vModel)
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.Cargo biCargo = new Business.Cargo();
            GenericHelpers.Retorno retorno = biCargo.Salvar(vModel.Cargo);

            mensagem.EstadoObjeto = retorno.EstadoObjeto;
            mensagem.UltimoId = retorno.UltimoId;
            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem});
        }

        /// <summary>
        /// Retornar uma lista no formato JSON do tipo Prion.Generic.Models.Cargo
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLista()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.Cargo biCargo = new Business.Cargo();

            Prion.Generic.Models.Lista lista = biCargo.ListaCargos(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new {success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr}, JsonRequestBehavior.AllowGet);
        }
       
        #endregion
    }
}