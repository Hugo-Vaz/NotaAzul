using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.ViewModels.Cidade
{
    public class vmDados
    {
        private GenericModels.Cidade _cidade;

        public GenericModels.Cidade Cidade
        {
            get
            {
                if (_cidade == null) return new GenericModels.Cidade();
                return _cidade;
            }
            set
            {
                _cidade = value;
            }
        }
    }
}