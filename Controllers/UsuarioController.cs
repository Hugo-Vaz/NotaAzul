using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.Controllers
{
    public class UsuarioController : BaseController
    {
        #region "Menu"
        /// <summary>
        /// Monta, através do Usuário Logado, um menu com todos os links que o mesmo tem acesso
        /// </summary>
        /// <returns></returns>
        public ActionResult MontaMenu() 
        {
            Models.Usuario usuario = (Models.Usuario)Session["UsuarioLogado"];
            Business.Usuario biUsuario = new Business.Usuario();
            biUsuario.ListaMenu(usuario.Id);

            return View();
        }
        #endregion


        #region "Views"

        /// <summary>
        /// Retorna a view Dados, utilizada para Cadastrar/Alterar os registros de um objeto Usuario
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewDados()
        {
            ViewModels.Usuario.vmDados vmDados = new ViewModels.Usuario.vmDados();

            // Carrega a lista de situações de um Usuário
            Business.Situacao buSituacao = new Business.Situacao();
            vmDados.Situacoes = buSituacao.CarregarSituacoesPeloTipo("Usuário");

            return View("Dados", vmDados);
        }

        /// <summary>
        /// Retorna a view Lista, com os registros de Usuario
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewLista()
        {
            return View("Lista");
        }

        /// <summary>
        /// Retorna a view MeusDados com os dados do usuário logado
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewMeusDados()
        {
            ViewModels.Usuario.vmDados vmDados = new ViewModels.Usuario.vmDados();
            vmDados.Usuario = (Models.Usuario)Session["UsuarioLogado"];

            return View("Dados", vmDados);
        }

        #endregion


        #region "Requisições"

        /// <summary>
        /// Carrega um registro do tipo Models.Usuario atráves do seu ID
        /// </summary>
        /// <param name="id">id do Usuário</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
            Business.Usuario biUsuario = new Business.Usuario();
            GenericModels.Usuario modelUsuario = biUsuario.Carregar(id);

            return Json(new { success = true, obj = modelUsuario }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Exclui um Usuário pelo ID
        /// </summary>
        /// <param name="id">id do Usuário</param>
        /// <returns>retorno no formato JSON</returns>
        public ActionResult Excluir()
        {
            Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
            Business.Usuario biUsuario = new Business.Usuario();
            GenericHelpers.Retorno retorno = biUsuario.Excluir(Prion.Tools.Conversor.ToInt(Request.Form["id"]));

            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = (retorno.Sucesso) ? "Usuário excluído com sucesso" : retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Salva os dados de um usuário
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult Salvar(ViewModels.Usuario.vmDados vModel)
        {
            Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
            Business.Usuario biUsuario = new Business.Usuario();
            GenericHelpers.Retorno retorno = biUsuario.Salvar(vModel.Usuario);

            mensagem.EstadoObjeto = retorno.EstadoObjeto;
            mensagem.UltimoId = retorno.UltimoId;
            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Retorna uma lista no formato JSON de objetos do tipo Models.Usuario
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLista()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);

            Business.Usuario buUsuario = new Business.Usuario();
            Prion.Generic.Models.Lista lista = buUsuario.ListaUsuarios(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        #endregion "Requisições"

    }
}