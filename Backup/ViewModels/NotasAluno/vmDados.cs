using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotaAzul.ViewModels.NotasAluno
{
    public class vmDados
    {
        private Models.MatriculaMedia _matriculaMedia;
        private List<Models.MatriculaFormaDeAvaliacao> _listaMatriculaFormasDeAvaliacao;

        public Models.MatriculaMedia MatriculaMedia
        {
            get
            {
                if (_matriculaMedia == null) { return new Models.MatriculaMedia(); }
                return _matriculaMedia;
            }
            set
            {
                _matriculaMedia = value;
            }

        }

        public List<Models.MatriculaFormaDeAvaliacao> ListaMatriculaFormasDeAvaliacao
        {
            get
            {
                if (_listaMatriculaFormasDeAvaliacao == null) { return new List<Models.MatriculaFormaDeAvaliacao>(); }
                return _listaMatriculaFormasDeAvaliacao;
            }
            set
            {
                _listaMatriculaFormasDeAvaliacao = value;
            }
        }

    }
}