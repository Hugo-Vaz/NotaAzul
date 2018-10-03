using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.ViewModels.Curso
{
    public class vmDados
    {
        private Models.Curso _curso;

        public Models.Curso Curso
        {
            get
            {
                if (_curso == null) return new Models.Curso();
                return _curso;
            }
            set
            {
                _curso = value;
            }
        }

        public List<GenericModels.Situacao> Situacoes { get; set; }
    }
}