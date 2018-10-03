using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace NotaAzul.Helpers
{
    public class Log
    {
        /// <summary>
        /// Grava um arquivo de log
        /// </summary>
        /// <param name="mensagem"></param>
        public static String Salvar(Exception e, String customMessage, System.Web.HttpRequestBase request = null)
        {

            try
            {

                // Path do arquivo txt
                string strFile = System.Web.HttpContext.Current.Server.MapPath("~/Log/erro-" + DateTime.Now.ToString("yyyyMMddHHmm") + ".html");

                // Se arquivo não existir...
                if (!System.IO.File.Exists(strFile))
                {
                    // criar o arquivo, 
                    System.IO.FileStream fs = System.IO.File.Create(strFile);
                    fs.Close();
                }

                StringBuilder msg = new StringBuilder();
                StringBuilder errorMsg = new StringBuilder();
                errorMsg.Append("<p>");
                errorMsg.Append(customMessage);
                errorMsg.Append("</p>");
                errorMsg.Append("<br />");
                errorMsg.Append("<p>Mensagem Técnica: ");
                errorMsg.Append(e.Message);
                errorMsg.Append("</p>");

                String url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
                String emailUsuario = Helpers.Log.EmailUsuario();

                msg.AppendLine("<p>#############################################################</p>");
                msg.Append("<p>Usuário: ");
                msg.AppendLine(emailUsuario);
                msg.Append("</p>");
                msg.Append("Data: ");
                msg.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                msg.Append("</p>");
                msg.Append("Url: ");
                msg.AppendLine(url);
                msg.Append("</p>");
                msg.AppendLine("<p>---</p>");
                msg.Append("<p>");
                msg.Append(errorMsg.ToString());
                msg.Append("</p>");
                msg.Append("<p>");
                msg.Append(e.StackTrace);
                msg.Append("</p>");
                msg.Append("<br />");

                if (request != null)
                {
                    msg.AppendLine(MontarMensagemRequest(request));
                }
                msg.AppendLine("<p>#############################################################</p>");
                System.IO.File.AppendAllText(strFile, msg.ToString(), Encoding.UTF8);

                return errorMsg.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }


        /// <summary>
        /// Se o usuário estiver logado, retorna o seu email. Caso contrário retorna vazio
        /// </summary>
        /// <returns></returns>
        public static string EmailUsuario()
        {
            Prion.Generic.Models.Usuario usuario = Prion.Generic.Helpers.Sistema.Instancia.UsuarioLogado;

            if (usuario == null)
            {
                return "";
            }

            return usuario.Email;
        }


        /// <summary>
        /// Varre os parâmetros do Request e monta uma mensagem com os valores dos parâmetros
        /// </summary>
        /// <returns></returns>
        private static string MontarMensagemRequest(System.Web.HttpRequestBase request)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("<p>Navegador: ");
            msg.Append(request.Browser.Browser);
            msg.Append(" - Versão: ");
            msg.Append(request.Browser.Version);
            msg.Append("</p>");

            foreach (String key in request.Form.Keys)
            {
                msg.Append("<p>");
                msg.Append("Parâmetro: ");
                msg.Append(key);
                msg.Append(" - Valor: ");
                msg.Append(request.Form[key]);
                msg.Append("<p/>");
            }
            return msg.ToString();
        }
    }
}