using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.ViewModels.Corporacao
{
    public class vmDados
    {
        private GenericModels.Corporacao _corporacao;

        public GenericModels.Corporacao Corporacao
        {
            get
            {
                if (_corporacao == null) return new GenericModels.Corporacao();
                return _corporacao;
            }
            set
            {
                _corporacao = value;
            }
        }
    }
}