using System;
using System.Web.Mvc;
using Prion.Tools;

using CommonHelpers = NotaAzul.Helpers;
using CommonModels = NotaAzul.Models;
using GenericData = Prion.Data;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;
using GenericRepository = Prion.Generic.Repository;
using NotaAzul.Helpers;

/**************************************************************************************************
 * 17/10/2012
 * Arquivo controlado por Thiago Motta Zappaterra pois esta seguindo o mesmo padrão dos outros projetos
 * Qualquer mudança por favor me consultar antes
 *************************************************************************************************/

namespace NotaAzul.Controllers
{
    public class LoginController : BaseController
    {
        /// <summary>
        /// Retorna a VIEW da página Index.cshtml
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // define tudo que será mostrado no HTML por default
            ViewBag.BuildSistema = Helpers.Settings.BuildSistema();
            ViewBag.NomeProjeto = Sistema.GetConfiguracaoSistema("admin_html:title");

            // o parâmetro 'redirect' foi adicionado na classe Helpers\VerifySession
            Object redirect = Request.Params.Get("redirect");
            if ((redirect != null) && (redirect.ToString().Trim().ToLower() == "true"))
            {
                // retorna um JSON que irá redirecionar o usuário para ação Index do LoginController
                return Json(new
                {
                    redirectTo = Url.Action("Index", "Login"), // método, controller
                    isRedirect = true
                }, JsonRequestBehavior.AllowGet);
            }

            return View();
        }

        /// <summary>
        /// Efetua o login do usuário no sistema
        /// </summary>
        /// <param name="logon">Objeto do tipo vmLogin que possui Login e Senha que o usuário digitou na página</param>
        /// <param name="returnUrl">Url para onde o usuário será redirecionado</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(NotaAzul.ViewModels.Login.vmLogin logon, string returnUrl)
        {
            // caso a view não tenha sido preenchida devidamente, retornar erro para interface
            if (!ModelState.IsValid)
            {
                return View(logon);
            }


            Criptografia criptografia = new Criptografia();

            // captura os dados do web.config
            string connectionString = criptografia.Decriptografar(Settings.ConnectionString());

            // instância da conexão com o banco de dados
            GenericData.DBFacade facade = new GenericData.DBFacade(Settings.Provider(), connectionString);

            // instância repositório de usuário
            GenericRepository.Usuario rpUsuarios = new GenericRepository.Usuario(ref facade);

            // define que também quer carregar o objeto Models.Situacao, Models.SituacaoTipo
            Prion.Tools.Entidade entidade = new Prion.Tools.Entidade(CommonHelpers.Entidade.Corporacao.ToString(), CommonHelpers.Entidade.Situacao.ToString(), CommonHelpers.Entidade.SituacaoTipo.ToString());
            rpUsuarios.Entidades = entidade;

            String login, senha;
            login = Prion.Tools.Conversor.Base64Decode(logon.Login).Trim();
            senha = Prion.Tools.Conversor.Base64Decode(logon.Senha).Trim();

            // realiza a busca do usuário
            Business.Usuario biUsuario = new Business.Usuario();
            CommonModels.Usuario usuarioLogado = biUsuario.Logar(login, senha);

            // verifica se objeto de usuário retornado coincide com os dados passados na interface
            if ((usuarioLogado == null) || (usuarioLogado.Login != login))
            {
                GenericHelpers.Sistema.Instancia.UsuarioLogado = null;

                Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
                mensagem.TextoMensagem = "O login ou senha informados estão incorretos. Verifique e tente novamente.";
                mensagem.Sucesso = false;

                return Json(new { success = false, mensagem = mensagem });
            }


            // verifica se o estado do usuário é 'Inativo'
            if (usuarioLogado.Situacao.Nome.ToLower().Trim() == GenericHelpers.Situacao.Generico.Inativo.ToString().ToLower().Trim())
            {
                GenericHelpers.Sistema.Instancia.UsuarioLogado = null;

                Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
                mensagem.TextoMensagem = "Usuário inativo!";
                mensagem.Sucesso = false;

                return Json(new { success = false, mensagem = mensagem });
            }



            // obtém os menus do usuário logado
            usuarioLogado.Menus = biUsuario.ListaMenu(usuarioLogado.Id);


            // verifica se o Usuário possui algum menu ativo
            if (usuarioLogado.Menus.Count == 0)
            {
                GenericHelpers.Sistema.Instancia.UsuarioLogado = null;

                Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
                mensagem.TextoMensagem = "Usuário sem menu ativo.";
                mensagem.Sucesso = false;

                return Json(new { success = false, mensagem = mensagem });
            }

            //obtém as permissões do usuário logado
            usuarioLogado.Permissoes = biUsuario.Permissoes(usuarioLogado.Id);

            //obtém usuário que acabou de logar e armazena em sessão
            Session.Add("UsuarioLogado", usuarioLogado);

            //armazena, em uma classe Singleton, o objeto de usuário logado. Esta classe é utilizada em repositórios Generic
            GenericHelpers.Sistema.Instancia.UsuarioLogado = usuarioLogado;

            //define, em uma classe Singleton, o tipo de lista utilizado neste projeto
            GenericHelpers.Sistema.Instancia.Lista = Prion.Tools.Request.ListaJavascript.Prion;

            //define a lista de configurações do sistema
            Prion.Generic.Repository.ConfiguracaoSistema repConfigSistema = new Prion.Generic.Repository.ConfiguracaoSistema(ref facade);
            GenericHelpers.Sistema.Instancia.ConfiguracaoSistema = repConfigSistema.Buscar();

            //caso login tenha sido requisitado de alguma outra ação, redirecionar usuário para esta após autenticação
            if ((returnUrl != null) || (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\")))
            {
                return Redirect(returnUrl);
            }

            // retorna um JSON que irá redirecionar o usuário para ação Index do HomeController
            return Json(new
            {
                redirectTo = Url.Action("Index", "Home"),
                isRedirect = true
            }, JsonRequestBehavior.AllowGet);
        }
    }
}