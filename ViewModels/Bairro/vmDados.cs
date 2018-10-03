using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.ViewModels.Bairro
{
    public class vmDados
    {
        private GenericModels.Bairro _bairro;

        public GenericModels.Bairro Bairro
        {
            get
            {
                if (_bairro == null) return new GenericModels.Bairro();
                return _bairro;
            }
            set
            {
                _bairro = value;
            }
        }
    }
}