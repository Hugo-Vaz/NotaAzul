using System;
using System.Web.Mvc;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.Controllers
{
    public class CursoController : BaseController
    {
        #region "Views"

        /// <summary>
        /// Retorna a view Lista, com os registros de Curso
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
            ViewModels.Curso.vmDados vmDados = new ViewModels.Curso.vmDados();
            Business.Situacao biSituacao = new Business.Situacao();

            // Carrega a lista de situações de um Curso
            vmDados.Situacoes = biSituacao.CarregarSituacoesPeloTipo("Curso");

            return View("Dados", vmDados);
        }

        #endregion "Views"


        #region "Requisições"

        /// <summary>
        /// Carrega um registro do tipo Models.Curso atráves do seu ID
        /// </summary>
        /// <param name="id">id do Curso</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
            Business.Curso biCurso = new Business.Curso();
            Models.Curso curso = (Models.Curso)biCurso.Carregar(id).Get(0);

            return Json(new { success = true, obj = curso }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Exclui um Curso pelo ID
        /// </summary>
        /// <param name="id">id do Curso</param>
        /// <returns>retorno no formato JSON</returns>
        public ActionResult Excluir()
        {
            Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
            Business.Curso biCurso = new Business.Curso();
            GenericHelpers.Retorno retorno = biCurso.Excluir(Prion.Tools.Conversor.ToInt(Request.Form["id"]));

            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = (retorno.Sucesso) ? "Curso excluído com sucesso" : retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Salva os dados de um Curso
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult Salvar(ViewModels.Curso.vmDados vModel)
        {
            Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
            Business.Curso biCurso = new Business.Curso();
            GenericHelpers.Retorno retorno = biCurso.Salvar(vModel.Curso);

            mensagem.EstadoObjeto = retorno.EstadoObjeto;
            mensagem.UltimoId = retorno.UltimoId;
            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Retorna uma lista no formato JSON de objetos do tipo Models.Curso
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLista()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.Curso biCurso = new Business.Curso();

            Prion.Generic.Models.Lista lista = biCurso.ListaCursos(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
