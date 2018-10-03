using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.ViewModels.Material
{
    public class vmDados
    {
         private Models.Material _material;

        public Models.Material Material
        {
            get
            {
                if (_material == null) return new Models.Material();
                return _material;
            }
            set
            {
                _material = value;
            }
        }

        public List<GenericModels.Situacao> Situacoes { get; set; } 
    }
}