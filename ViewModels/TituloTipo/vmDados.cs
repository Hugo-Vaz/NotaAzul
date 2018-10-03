using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.ViewModels.TituloTipo
{
    public class vmDados
    {
        private GenericModels.TituloTipo _tituloTipo;

        public GenericModels.TituloTipo TituloTipo
        {
            get
            {
                if (_tituloTipo == null) return new GenericModels.TituloTipo();
                return _tituloTipo;
            }
            set
            {
                _tituloTipo = value;
            }
        }

        public List<GenericModels.Situacao> Situacoes { get; set; }
    }
}