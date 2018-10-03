using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotaAzul.ViewModels.MovimentacaoFinanceira
{
    public class vmDados
    {
        private Models.MovimentacaoFinanceira _movimentacaoFinanceira;

        public Models.MovimentacaoFinanceira MovimentacaoFinanceira
        {
            get
            {
                if (_movimentacaoFinanceira == null) { return new Models.MovimentacaoFinanceira(); }
                return _movimentacaoFinanceira;
            }
            set
            {
                _movimentacaoFinanceira = value;
            }

        }
    }
}