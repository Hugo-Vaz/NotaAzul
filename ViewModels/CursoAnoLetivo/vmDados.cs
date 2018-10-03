using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.ViewModels.CursoAnoLetivo
{
    public class vmDados
    {
        private Models.CursoAnoLetivo _cursoAnoLetivo;

        public Models.CursoAnoLetivo CursoAnoLetivo
        {
            get
            {
                if (_cursoAnoLetivo == null) return new Models.CursoAnoLetivo();
                return _cursoAnoLetivo;
            }
            set
            {
                _cursoAnoLetivo = value;
            }
        }

        public List<GenericModels.Situacao> Situacoes { get; set; }
    }
}