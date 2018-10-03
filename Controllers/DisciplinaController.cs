using System;
using System.Web.Mvc;
using GenericHelpers = Prion.Generic.Helpers;

namespace NotaAzul.Controllers
{
    public class DisciplinaController : BaseController
    {
        #region "Views"
        /// <summary>
        /// Retorna a view Lista, com os registros de Disciplina
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
            ViewModels.Disciplina.vmDados vmDados = new ViewModels.Disciplina.vmDados();
            Business.Situacao biSituacao = new Business.Situacao();

            //carrega a lista de situacoes de um Disciplina
            vmDados.Situacoes = biSituacao.CarregarSituacoesPeloTipo("Disciplina");

            return View("Dados", vmDados);
        }

        #endregion "Views"


        #region "Requisições"

        /// <summary>
        /// Carrega um registro do tipo Models.Disciplina atraves do seu Id
        /// </summary>
        /// <param name="id">id da Disciplina</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
            Business.Disciplina biDisciplina = new Business.Disciplina();
            Models.Disciplina disciplina = (Models.Disciplina)biDisciplina.Carregar(id).Get(0);

            return Json(new { success = true, obj = disciplina }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Exclui uma Disciplina pelo Id
        /// </summary>
        /// <param name="id">id da Disciplina</param>
        /// <returns>Um retorno no formato de Json</returns>
        public ActionResult Excluir()
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.Disciplina biDisciplina = new Business.Disciplina();
            GenericHelpers.Retorno retorno = biDisciplina.Excluir(Prion.Tools.Conversor.ToInt(Request.Form["id"]));

            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = (retorno.Sucesso) ? "Disciplina excluída com Sucesso" : retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Salva os dados de uma Disciplina
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult Salvar(ViewModels.Disciplina.vmDados vModel)
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.Disciplina biDisciplina = new Business.Disciplina();
            GenericHelpers.Retorno retorno = biDisciplina.Salvar(vModel.Disciplina);

            mensagem.EstadoObjeto = retorno.EstadoObjeto;
            mensagem.UltimoId = retorno.UltimoId;
            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Retornar uma lista no formato JSON do tipo Models.Disciplina
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLista()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.Disciplina biDisciplina = new Business.Disciplina();

            Prion.Generic.Models.Lista lista = biDisciplina.ListaDisciplinas(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
