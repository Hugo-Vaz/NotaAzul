using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.ViewModels.Segmento
{
    public class vmDados
    {
        private Models.Segmento _segmento;

        public Models.Segmento Segmento
        {
            get
            {
                if (_segmento == null) return new Models.Segmento();
                return _segmento;
            }
            set
            {
                _segmento = value;
            }
        }

        public List<GenericModels.Situacao> Situacoes { get; set; }
    }
}