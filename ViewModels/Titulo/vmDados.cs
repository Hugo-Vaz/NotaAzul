using System;
using System.Collections.Generic;
using GenericModel = Prion.Generic.Models;

namespace NotaAzul.ViewModels.Titulo
{
    public class vmDados
    {
        private GenericModel.Titulo _titulo;

        public GenericModel.Titulo Titulo
        {
            get
            {
                if (_titulo == null) { return new GenericModel.Titulo(); }
                return _titulo;
            }
            set
            {
                _titulo = value; 
            }
        }
        public GenericModel.Situacao Situacao { get; set; }       
    }
}