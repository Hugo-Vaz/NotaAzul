using System.Collections.Generic;
using System.Web.Mvc;
using System;
using GenericHelpers = Prion.Generic.Helpers;

namespace NotaAzul.Controllers
{
    public class TurmaController : BaseController
    {
        #region "Views"

        /// <summary>
        /// Retorna a view Lista, com os registros de Turma
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
            ViewModels.Turma.vmDados vmDados = new ViewModels.Turma.vmDados();
            Business.Situacao buSituacao = new Business.Situacao();

            vmDados.Situacoes = buSituacao.CarregarSituacoesPeloTipo("Turma");
            return View("Dados", vmDados);
        }

        #endregion "Views"


        #region "Requisições"

        /// <summary>
        /// Carrega um registro do tipo Models.Turma atráves do seu ID
        /// </summary>
        /// <param name="id">id do Turma</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
            Business.Turma biTurma = new Business.Turma();
            Models.Turma turma = (Models.Turma)biTurma.Carregar(id).Get(0);

            return Json(new { success = true, obj = turma }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Exclui uma Turma pelo ID
        /// </summary>
        /// <param name="id">id da Turma</param>
        /// <returns>retorno no formato JSON</returns>
        public ActionResult Excluir()
        {
            Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
            Business.Turma biTurma = new Business.Turma();
            GenericHelpers.Retorno retorno = biTurma.Excluir(Prion.Tools.Conversor.ToInt(Request.Form["id"]));

            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = (retorno.Sucesso) ? "Turma excluída com sucesso" : retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Salva os dados de uma Turma
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult Salvar(ViewModels.Turma.vmDados vModel)
        {
            Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
            Business.Turma biTurma = new Business.Turma();
            GenericHelpers.Retorno retorno = biTurma.Salvar(vModel.Turma);

            mensagem.EstadoObjeto = retorno.EstadoObjeto;
            mensagem.UltimoId = retorno.UltimoId;
            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Retorna uma lista no formato JSON de objetos do tipo Models.Turma
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLista()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.Turma biTurma = new Business.Turma();

            Prion.Generic.Models.Lista lista = biTurma.ListaTurmas(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
