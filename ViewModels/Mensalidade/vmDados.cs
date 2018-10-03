using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.ViewModels.Mensalidade
{
    public class vmDados
    {
        private Models.Mensalidade _mensalidade;

        public Models.Mensalidade Mensalidade
        {
            get
            {
                if (_mensalidade == null) return new Models.Mensalidade();
                return _mensalidade;
            }
            set
            {
                _mensalidade = value;
            }
        }

        public List<GenericModels.Situacao> Situacoes { get; set; }
    }
}