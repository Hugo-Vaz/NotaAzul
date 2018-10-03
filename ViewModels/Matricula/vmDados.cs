using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.ViewModels.Matricula
{
    public class vmDados
    {
        private Models.Matricula _matricula;

        public Models.Matricula Matricula
        {
            get
            {
                if (_matricula == null) return new Models.Matricula();
                return _matricula;
            }
            set
            {
                _matricula = value;
            }
        }

        public List<GenericModels.Situacao> Situacoes { get; set; }

        public Prion.Generic.Models.Corporacao Corporacao { get; set; }
    }
}