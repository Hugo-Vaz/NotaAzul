using GenericModels = Prion.Generic.Models;
using System.Collections.Generic;

namespace NotaAzul.ViewModels.ContasPagar
{
    public class vmDados
    {
        private GenericModels.Titulo _tituloPago;

        public GenericModels.Titulo TituloPago
        {
            get
            {
                if (_tituloPago == null) { return new GenericModels.Titulo(); }
                return _tituloPago;
            }

            set
            {
                _tituloPago = value;
            }
        }
    }
}