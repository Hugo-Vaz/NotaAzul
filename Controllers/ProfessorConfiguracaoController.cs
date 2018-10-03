using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using GenericHelpers = Prion.Generic.Helpers;

namespace NotaAzul.Controllers
{
    public class ProfessorConfiguracaoController:BaseController
    {
        #region "Views"


        /// <summary>
        /// Retorna a view ConfiguracoesNotas
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewProfessorConfiguracao()
        {
            Business.ProfessorConfiguracao biConfig = new Business.ProfessorConfiguracao();
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Models.ProfessorConfiguracao configuracao = new Models.ProfessorConfiguracao();  
            Models.Usuario  usuario= (Models.Usuario)Session["UsuarioLogado"];

            Prion.Tools.Request.Filtro f = new Prion.Tools.Request.Filtro("ProfessorConfiguracao.IdUsuario", "=", usuario.Id);
            parametros.Filtro.Add(f);

            configuracao=(Models.ProfessorConfiguracao)biConfig.Carregar(parametros).Get(0);
            ViewData["Id"] = (configuracao!=null) ? configuracao.Id : 0;
            ViewData["TipoMedia"] = (configuracao != null) ? configuracao.TipoMedia : "";
            return View("ProfessorConfiguracao");
        }
        #endregion "Views"

        #region "Requisições"
        /// <summary>
        /// Salva as configurações do esquema de notas/conceitos
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult Salvar()
        {
            GenericHelpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);

            Models.ProfessorConfiguracao configuracao = new Models.ProfessorConfiguracao();
            Business.ProfessorConfiguracao biConfiguracao = new Business.ProfessorConfiguracao();

            configuracao.Usuario = (Models.Usuario)Session["UsuarioLogado"];
            configuracao.TipoMedia = Request.Form["TipoMedia"];
            configuracao.Id = Convert.ToInt32(Request.Form["ProfessorConfiguracao.Id"]);

            GenericHelpers.Retorno retorno = biConfiguracao.Salvar(configuracao);
            mensagem.EstadoObjeto = retorno.EstadoObjeto;
            mensagem.UltimoId = retorno.UltimoId;
            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        #endregion
    }
}