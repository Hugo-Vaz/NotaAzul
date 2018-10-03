using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.ViewModels.Cargo
{
    public class vmDados
    {
        private Prion.Generic.Models.Cargo _cargo;

        public Prion.Generic.Models.Cargo Cargo
        {
            get
            {
                if (_cargo == null) return new Prion.Generic.Models.Cargo();
                return _cargo;
            }
            set
            {
                _cargo = value;
            }
        }

        public List<GenericModels.Situacao> Situacoes { get; set; }
    }
}