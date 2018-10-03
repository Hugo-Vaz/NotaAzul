using NotaAzul.Helpers;

namespace NotaAzul.Business
{
    public class Base : Prion.Generic.Business.Base
    {
        public Base() : base()
        {
            base.Iniciar(Settings.Provider(), Settings.ConnectionString(), true);
        }  
    }
}