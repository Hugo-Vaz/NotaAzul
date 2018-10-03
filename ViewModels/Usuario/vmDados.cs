using System.Collections.Generic;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.ViewModels.Usuario
{
    public class vmDados
    {
        private Models.Usuario _usuario;

        public Models.Usuario Usuario
        {
            get { if (_usuario == null) { return new Models.Usuario(); } return _usuario; }
            set { _usuario = value; }
        }

        public List<GenericModels.Situacao> Situacoes { get; set; }
    }
}