using System;
using System.Web.Mvc;
using GenericHelpers = Prion.Generic.Helpers;

namespace NotaAzul.Controllers
{
    public class FuncionarioController : BaseController
    {
        #region "Views"
        /// <summary>
        /// Retona a view Lista, com os regitros de Funcionario
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewLista()
        {
            return View("Lista");
        }

        /// <summary>
        /// Retorna a View Novo
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewDados()
        {
            ViewModels.Funcionario.vmDados vmDados = new ViewModels.Funcionario.vmDados();
            Business.Situacao biSituacao = new Business.Situacao();

            //carrega a lista de situacoes de um Funcionario
             vmDados.Situacoes = biSituacao.CarregarSituacoesPeloTipo("Funcionário");

            return View("Dados", vmDados);
        }

        #endregion "Views"


        #region "Requisições"

        /// <summary>
        /// Carrega um registro do tipo Models.Funcionario atraves do seu Id
        /// </summary>
        /// <param name="id">id do Funcionario</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);

            Business.Funcionario biFuncionario = new Business.Funcionario();            
            Prion.Generic.Models.Funcionario funcionario = (Prion.Generic.Models.Funcionario)biFuncionario.Carregar(parametros,id).Get(0);

            return Json(new {success = true, obj = funcionario }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Exclui um Funcionario pelo Id
        /// </summary>
        /// <param name="id">id do Funcionario</param>
        /// <returns>Um retorno no formato de Json</returns>
        public ActionResult Excluir()
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.Funcionario biFuncionario = new Business.Funcionario();
            GenericHelpers.Retorno retorno = biFuncionario.Excluir(Prion.Tools.Conversor.ToInt(Request.Form["id"]));

            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = (retorno.Sucesso) ? "Funcionario excluído com Sucesso" : retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

           return Json(new {success = mensagem.Sucesso, mensagem = mensagem});
        }

        /// <summary>
        /// Salva os dados de um Funcionario
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult Salvar(ViewModels.Funcionario.vmDados vModel)
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.Funcionario biFuncionario = new Business.Funcionario();
            GenericHelpers.Retorno retorno = biFuncionario.Salvar(vModel.Funcionario);

            mensagem.EstadoObjeto = retorno.EstadoObjeto;
            mensagem.UltimoId = retorno.UltimoId;
            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem});
        }

        /// <summary>
        /// Retornar uma lista no formato JSON do tipo Models.Funcionario
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLista()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.Funcionario biFuncionario = new Business.Funcionario();

            Prion.Generic.Models.Lista lista = biFuncionario.ListaFuncionarios(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new {success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr}, JsonRequestBehavior.AllowGet);
        }
       
        #endregion
    }
}