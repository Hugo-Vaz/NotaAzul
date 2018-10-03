using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.ViewModels.Turma
{
    public class vmDados
    {
        private Models.Turma _turma;

        public Models.Turma Turma
        {
            get
            {
                if (_turma == null) return new Models.Turma();
                return _turma;
            }
            set
            {
                _turma = value;
            }
        }

        public List<GenericModels.Situacao> Situacoes { get; set; }
    }
}