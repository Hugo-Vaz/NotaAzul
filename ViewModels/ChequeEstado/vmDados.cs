using System.Collections.Generic;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.ViewModels.ChequeEstado
{
    public class vmDados
    {
        private GenericModels.ChequeEstado _chequeEstado;

        public GenericModels.ChequeEstado ChequeEstado
        {
            get 
            {
                if (_chequeEstado == null) { return new GenericModels.ChequeEstado(); }
                return _chequeEstado;
            }

            set
            {
                _chequeEstado = value;
            }

        }
        public List<GenericModels.Situacao> Situacoes { get; set; }

    }
}