using System.Collections.Generic;
using GenericModels = Prion.Generic.Models;


namespace NotaAzul.ViewModels.GrupoDesconto
{
    public class vmDados
    {
        private Models.GrupoDesconto _grupoDesconto;

        public Models.GrupoDesconto GrupoDesconto
        {
            get
            {
                if (_grupoDesconto == null) return new Models.GrupoDesconto();
                return _grupoDesconto;
            }
            set
            {
                _grupoDesconto = value;
            }
        }

        public List<GenericModels.Situacao> Situacoes { get; set; }
    }
}