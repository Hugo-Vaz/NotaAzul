using System;
using System.Web.Mvc;
using NotaAzul.Helpers;

/**************************************************************************************************
 * 04/04/2013
 * Arquivo controlado por Thiago Motta Zappaterra.
 * Este arquivo esta seguindo o mesmo padrão dos outros projetos.
 * Qualquer mudança por favor me consultar antes.
 *************************************************************************************************/

namespace NotaAzul.Controllers
{
    [VerifySession]
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext ctx)
        {
            this.IniciarInstanciaSistema();
            base.OnActionExecuting(ctx);
        }

        /// <summary>
        /// Retorna o objeto Singleton com as~configurações do sistema.
        /// Objeto em session
        /// </summary>
        protected Prion.Generic.Helpers.Sistema Sistema
        {
            get
            {
                this.IniciarInstanciaSistema();
                return (Prion.Generic.Helpers.Sistema)Session["Sistema"];
            }
        }

        private void IniciarInstanciaSistema()
        {
            // se a session já existir, sai fora :D
            if (Session["Sistema"] != null)
            {
                return;
            }

            // define, em uma classe Singleton, o tipo de banco de dados utilizado neste projeto
            Prion.Generic.Helpers.Sistema.Instancia.TipoBancoDados = Settings.TipoBancoDados();
            Prion.Generic.Helpers.Sistema.Instancia.UsuarioLogado = new Prion.Generic.Models.Usuario();
            Prion.Generic.Helpers.Sistema.Instancia.UsuarioLogado.IdCorporacao = Settings.IdCorporacao();
            Prion.Generic.Helpers.Sistema.Instancia.Lista = Prion.Tools.Request.ListaJavascript.Prion;

            // instância de uma conexão com o banco de dados. Utilizado para fazer a query abaixo (ConfiguracaoSistema)
            Prion.Tools.Criptografia criptografia = new Prion.Tools.Criptografia();
            String connectionString = criptografia.Decriptografar(Settings.ConnectionString());
            Prion.Data.DBFacade facade = new Prion.Data.DBFacade(Settings.Provider(), connectionString);

            // define uma lista com as configurações do sistema
            Prion.Generic.Repository.ConfiguracaoSistema repConfigSistema = new Prion.Generic.Repository.ConfiguracaoSistema(ref facade);
            Prion.Generic.Helpers.Sistema.Instancia.ConfiguracaoSistema = repConfigSistema.Buscar();

            // guarda o objeto Singleton em session
            Session["Sistema"] = Prion.Generic.Helpers.Sistema.Instancia;
        }
    }
}