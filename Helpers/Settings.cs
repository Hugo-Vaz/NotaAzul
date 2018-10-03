using System;

namespace NotaAzul.Helpers
{
    static class Settings
    {
        private static System.Configuration.AppSettingsReader _appReader = null;

        public static string LocalSistema()
        {
            return AppReader().GetValue("LocalSistema", typeof(string)).ToString();
        }

        public static string EnderecoDocumentos()
        {
            return AppReader().GetValue("EnderecoDocumentos", typeof(string)).ToString();
        }

        public static string EnderecoPhantomJs()
        {
            return AppReader().GetValue("EnderecoPhantomJs", typeof(string)).ToString();
        }

        public static string UrlDocumentos()
        {
            return AppReader().GetValue("UrlDocumentos", typeof(string)).ToString();
        }

        public static string Disco()
        {
            return AppReader().GetValue("DiscoPhantom", typeof(string)).ToString();
        }

        public static string BuildSistema()
        {
            //Helpers.Settings.LocalSistema = AppReader().GetValue("LocalSistema", typeof(string)).ToString();
            //Helpers.Settings.EnderecoDocumentos = AppReader().GetValue("EnderecoDocumentos", typeof(string)).ToString();
            //Helpers.Settings.UrlDocumentos = AppReader().GetValue("UrlDocumentos", typeof(string)).ToString();
            //Helpers.Settings.EnderecoPhantomJs = AppReader().GetValue("EnderecoPhantomJs", typeof(string)).ToString();

            // se o sistema estiver em desenvolvimento, o BuildSistema será composto da Data/Hora atual do sistema
            // evita o cache local
            if (Helpers.Settings.LocalSistema().ToLower().Trim() == "desenvolvimento")
            {
                return String.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
            }
            else
            {
                // se o LocalSistema for produção, pega o valor contindo no Atributo 'admin_sistema:build'
                String data = Prion.Generic.Helpers.Sistema.Instancia.GetConfiguracaoSistema("admin_sistema:build");
                return data;
            }
        }

        public static string ConnectionString()
        {
            return AppReader().GetValue("connectionString", typeof(string)).ToString();
        }

        public static Int32 IdCorporacao()
        {
            return Prion.Tools.Conversor.ToInt32(AppReader().GetValue("IdCorporacao", typeof(string)).ToString());
        }

        public static string Provider()
        {
            return AppReader().GetValue("provider", typeof(string)).ToString();
        }

        public static string EnderecoSistema()
        {
            return AppReader().GetValue("EnderecoSistema", typeof(string)).ToString();
        }

        public static Int32 CedenteConvenio()
        {
            return Prion.Tools.Conversor.ToInt32(AppReader().GetValue("Boleto_Convenio", typeof(string)).ToString());
        }

        public static string CedenteAgencia()
        {
            return AppReader().GetValue("Boleto_Agencia", typeof(string)).ToString();
        }

        public static string CedenteConta()
        {
            return AppReader().GetValue("Boleto_Conta", typeof(string)).ToString();
        }

        public static string CedenteDigitoVerificador()
        {
            return AppReader().GetValue("Boleto_Digito_Agencia", typeof(string)).ToString();
        }

        /// <summary>
        /// retorna o tipo do banco de dados utilizado neste projeto
        /// </summary>
        public static Prion.Tools.Enums.TipoBancoDados TipoBancoDados()
        {
            return Prion.Tools.Enums.TipoBancoDados.SQLServer;
        }

        /// <summary>
        /// retorna uma instância do objeto AppSettingsReader, que é responsável por ler uma chave do web.config
        /// </summary>
        public static System.Configuration.AppSettingsReader AppReader()
        {
            if (_appReader == null)
            {
                _appReader = new System.Configuration.AppSettingsReader();
            }

            return _appReader;
        }
    }
}