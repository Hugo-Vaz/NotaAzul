using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.ViewModels.Turno
{
    public class vmDados
    {
        private Models.Turno _turno;

        public Models.Turno Turno
        {
            get { if (_turno == null) { return new Models.Turno(); }  return _turno; }
            set { _turno = value; }
        }

        public List<GenericModels.Situacao> Situacoes { get; set; }
    }
}