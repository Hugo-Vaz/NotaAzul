using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.ViewModels.Disciplina
{
    public class vmDados
    {
        private Models.Disciplina _disciplina;

        public Models.Disciplina Disciplina
        {
            get
            {
                if (_disciplina == null) return new Models.Disciplina();
                return _disciplina;
            }
            set
            {
                _disciplina = value;
            }
        }

        public List<GenericModels.Situacao> Situacoes { get; set; } 
    }
}