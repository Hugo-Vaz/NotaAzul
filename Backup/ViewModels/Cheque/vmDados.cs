using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GenericModels = Prion.Generic.Models;


namespace NotaAzul.ViewModels.Cheque
{
    public class vmDados
    {
        private GenericModels.Cheque _cheque;
        private Models.AlunoResponsavel _responsavel;

        public GenericModels.Cheque Cheque
        {
            get
            {
                if (_cheque == null) { return new GenericModels.Cheque(); }
                return _cheque;
            }
            set
            {
                _cheque = value;
            }
        }

        public Models.AlunoResponsavel Responsavel
        {
            get
            {
                if (_cheque == null) { return new Models.AlunoResponsavel(); }
                return _responsavel;
            }
            set
            {
                _responsavel = value;
            }
        }
    }
}