using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.ViewModels.Funcionario
{
    public class vmDados
    {
        private Prion.Generic.Models.Funcionario _funcionario;

        public Prion.Generic.Models.Funcionario Funcionario
        {
            get
            {
                if (_funcionario == null) return new Prion.Generic.Models.Funcionario();
                return _funcionario;
            }
            set
            {
                _funcionario = value;
            }
        }

        public List<GenericModels.Situacao> Situacoes { get; set; }
    }
}