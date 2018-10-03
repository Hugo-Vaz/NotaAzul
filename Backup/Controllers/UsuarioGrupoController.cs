using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;
using System.Text;

namespace NotaAzul.Controllers
{
    public class UsuarioGrupoController : BaseController
    {
        #region "Views"

        /// <summary>
        /// Retorna a view Dados, utilizada para Cadastrar/Alterar os registros de um objeto UsuarioGrupo
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewDados()
        {
            ViewModels.UsuarioGrupo.vmDados vm = new ViewModels.UsuarioGrupo.vmDados();
            Business.Situacao buSituacao = new Business.Situacao();
            Business.UsuarioGrupo buUsuarioGurpo = new Business.UsuarioGrupo();

            // Carrega a lista de situações de um UsuárioGrupo
            vm.Situacoes = buSituacao.CarregarSituacoesPeloTipo("UsuárioGrupo");

            return View("Dados", vm);
        }

        /// <summary>
        /// Retorna a view Lista, com os registros de UsuarioGrupo
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewLista()
        {
            return View("Lista");
        }

        /// <summary>
        /// Retorna a view Permissoes, utilizada para definir as permissoes de um Grupo de Usuário
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewPermissoes()
        {
            return View("Permissoes");
        }

        #endregion "Views"


        #region "Requisições"

        /// <summary>
        /// Carrega um registro do tipo Models.UsuarioGrupo atráves do seu ID
        /// </summary>
        /// <param name="id">id do UsuárioGrupo</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
            Business.UsuarioGrupo biUsuarioGrupo = new Business.UsuarioGrupo();
            GenericModels.UsuarioGrupo modelUsuarioGrupo = biUsuarioGrupo.Carregar(id);

            return Json(new { success = true, obj = modelUsuarioGrupo }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Exclui um UsuárioGrupo pelo ID
        /// </summary>
        /// <param name="id">id do UsuárioGrupo</param>
        /// <returns>retorno no formato JSON</returns>
        public ActionResult Excluir()
        {
            Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
            Business.UsuarioGrupo biUsuarioGrupo = new Business.UsuarioGrupo();
            GenericHelpers.Retorno retorno = biUsuarioGrupo.Excluir(Prion.Tools.Conversor.ToInt(Request.Form["id"]));

            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = (retorno.Sucesso) ? "Grupo de Usuário excluído com sucesso" : retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Carrega todas as permissões visíveis para o Grupo de Usuário informado
        /// </summary>
        /// <param name="id">id do Grupo de Usuário</param>
        /// <returns>Objeto no formado JSON com todas as permissões</returns>
        public ActionResult PermissoesVisiveis(Int32 id)
        {
            Business.UsuarioGrupo biUsuarioGrupo = new Business.UsuarioGrupo();
            Prion.Generic.Models.Lista listaPermissoesVisiveis = biUsuarioGrupo.CarregarPermissoesVisiveis(id);
            String json = Prion.Tools.Conversor.ToJson(listaPermissoesVisiveis.DataTable);

            return Json(new { success = true, modulos = json }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Carrega todas as permissões deste grupo de usuário
        /// </summary>
        /// <param name="id">id do Grupo de Usuário</param>
        /// <returns>Objeto no formato JSON com todas as permissões do grupo de usuário</returns>
        public ActionResult PermissoesGrupoUsuario(Int32 id)
        {
            Business.UsuarioGrupo biUsuarioGrupo = new Business.UsuarioGrupo();
            Prion.Generic.Models.Lista listaPermissoes = biUsuarioGrupo.CarregarPermissoes(id);
            String json = Prion.Tools.Conversor.ToJson(listaPermissoes.DataTable);

            return Json(new { success = true, totalRegistros = listaPermissoes.Count, permissoesGrupoUsuario = json }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Insere ou Atualiza os dados de um grupo de usuário
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult Salvar(ViewModels.UsuarioGrupo.vmDados vModel)
        {
            Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
            Business.UsuarioGrupo biUsuarioGrupo = new Business.UsuarioGrupo();
            GenericHelpers.Retorno retorno = biUsuarioGrupo.Salvar(vModel.UsuarioGrupo);

            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Salva todas as permissões selecionadas na interface para aquele Grupo de Usuário
        /// </summary>
        /// <returns></returns>
        public ActionResult SalvarPermissoes()
        {
            Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
            String permissoes = Request.Form["permissoes"];

            Int32[] listaPermissoes = Prion.Tools.Conversor.ToInt(permissoes);
            Int32 idUsuarioGrupo = Prion.Tools.Conversor.ToInt32(Request.Form["idUsuarioGrupo"]);

            Business.UsuarioGrupo biUsuarioGrupo = new Business.UsuarioGrupo();
            GenericHelpers.Retorno retorno = biUsuarioGrupo.SalvarPermissoes(idUsuarioGrupo, listaPermissoes);

            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

         /// <summary>
        /// Retorna uma lista no formato JSON de objetos do tipo Models.UsuarioGrupo
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLista()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.UsuarioGrupo biUsuarioGrupo = new Business.UsuarioGrupo();

            Prion.Generic.Models.Lista lista = biUsuarioGrupo.ListaUsuarioGrupo(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        #endregion "Requisições"
    }
}