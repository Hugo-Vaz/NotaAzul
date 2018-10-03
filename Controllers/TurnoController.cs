using System.Collections.Generic;
using System.Web.Mvc;
using System;
using GenericHelpers = Prion.Generic.Helpers;

namespace NotaAzul.Controllers
{
    public class TurnoController : BaseController
    {
        #region "Views"

        /// <summary>
        /// Retorna a view Lista, com os registros de Turno
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewLista()
        {
            return View("Lista");
        }

        /// <summary>
        /// Retorna a view Dados, utilizada para Cadastrar/Alterar os registros de um objeto Turno
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewDados()
        {
            ViewModels.Turno.vmDados vm = new ViewModels.Turno.vmDados();
            Business.Situacao buSituacao = new Business.Situacao();

            vm.Situacoes = buSituacao.CarregarSituacoesPeloTipo("Turno");

            return View("Dados", vm);
        }

        #endregion "Views"


        #region "Requisições"

        /// <summary>
        /// Carrega um registro do tipo Models.Turno atráves do seu ID
        /// </summary>
        /// <param name="id">id do Turno</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
            Business.Turno biTurno = new Business.Turno();
            Models.Turno turno = (Models.Turno)biTurno.Carregar(id).Get(0);

            return Json(new {success = true, obj = turno }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Exclui um Turno pelo ID
        /// </summary>
        /// <param name="id">id do Turno</param>
        /// <returns>retorno no formato JSON</returns>
        public ActionResult Excluir()
        {
            Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
            Business.Turno biTurno = new Business.Turno();
            GenericHelpers.Retorno retorno = biTurno.Excluir(Prion.Tools.Conversor.ToInt(Request.Form["id"]));

            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = (retorno.Sucesso) ? "Turno excluído com sucesso" : retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;
            
            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Insere ou Atualiza os dados de um Turno
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult Salvar(ViewModels.Turno.vmDados vModel)
        {
            Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
            Business.Turno buTurno = new Business.Turno();
            GenericHelpers.Retorno retorno = buTurno.Salvar(vModel.Turno);

            mensagem.EstadoObjeto = retorno.EstadoObjeto;
            mensagem.UltimoId = retorno.UltimoId;
            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Retorna uma lista no formato JSON de objetos do tipo Models.Turno
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLista()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.Turno biTurno = new Business.Turno();

            Prion.Generic.Models.Lista lista = biTurno.ListaTurnos(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}