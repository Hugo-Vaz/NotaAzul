using System.Web.Mvc;
using System.Collections.Generic;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;

/**************************************************************************************************
 * 17/10/2012
 * Arquivo controlado por Thiago Motta Zappaterra pois esta seguindo o mesmo padrão dos outros projetos
 * Qualquer mudança por favor me consultar antes
 *************************************************************************************************/

namespace NotaAzul.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            //caso usuário não esteja em sessão, ir para página de login
            if ((Models.Usuario)Session["UsuarioLogado"] == null)
            {
                return Redirect("/Login/Index/");
            }

            // define tudo que será mostrado no HTML por default
            ViewBag.BuildSistema = Helpers.Settings.BuildSistema();
            ViewBag.NomeProjeto = Sistema.GetConfiguracaoSistema("admin_html:title");
            ViewBag.ConfiguracaoSistema = Sistema.ConfiguracaoSistema;
            //ViewBag.UsuarioLogado = (Models.Usuario)Session["UsuarioLogado"];

            return View();
        }

        /// <summary>
        /// Efetua o logoff do usuário no sistema.
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            GenericHelpers.Sistema.Instancia.UsuarioLogado = null;

            // limpa toda a session
            Session.Clear();

            return RedirectToAction("Index", "Home"); // método, controller
        }

        /// <summary>
        /// retorna um JSON com todas as permissões do UsuarioLogado (objeto em session)
        /// </summary>
        /// <returns></returns>
        public ActionResult Permissoes()
        {
            //caso usuário não esteja em sessão, ir para página de login
            if ((Models.Usuario)Session["UsuarioLogado"] == null)
            {
                return Redirect("/Login/Index/");
            }

            Models.Usuario usuarioLogado = (Models.Usuario)Session["UsuarioLogado"];
            return Json(new { success = true, permissoes = usuarioLogado.Permissoes }, JsonRequestBehavior.AllowGet);
        }

        /// <summary> Obtém o menu do usuário logado em formato HTML </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult MenuHTML(List<GenericModels.Menu> menu)
        {
            Models.Usuario usuario = (Models.Usuario)Session["UsuarioLogado"];

            Business.Menu biMenu = new Business.Menu();
            return Content(biMenu.ToHTML(usuario.Menus));
        }
    }
}