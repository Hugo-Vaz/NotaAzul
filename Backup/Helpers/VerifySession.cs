using System.Web.Mvc;
using System.Web.Routing;

/**************************************************************************************************
 * 17/10/2012
 * Arquivo controlado por Thiago Motta Zappaterra pois esta seguindo o mesmo padrão dos outros projetos
 * Qualquer mudança por favor me consultar antes
 *************************************************************************************************/

public class VerifySession : AuthorizeAttribute
{
    public override void OnAuthorization(AuthorizationContext filterContext)
    {
        var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
        var actionName = filterContext.ActionDescriptor.ActionName.Trim();
        var session = filterContext.HttpContext.Session["UsuarioLogado"];

        // evitar validação caso esteja no controller Login
        if (controllerName.Trim().ToLower() == "login")
        {
            return;
        }


        // se a session UsuarioLogado ainda não existir...
        if (session == null)
        {
            if (controllerName.Trim().ToLower() == "home")
            {
                // redirecioná-lo para realização do login
                var redirect = new RouteValueDictionary { { "controller", "Login" }, { "action", "Index" } };
                filterContext.Result = new RedirectToRouteResult(redirect);
            }
            else
            {
                // adiciona um atributo chamado 'redirect'. Ele será utilizado no controller Login/Index
                // Lá será verificado a existencia deste atributo, e se ele for true, a página que chamou o método saberá
                // que deverá redirecionar o usuário para a view Index
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    action = "Index",
                    controller = "Login",
                    redirect = true
                }));
            }
        }
    }
}