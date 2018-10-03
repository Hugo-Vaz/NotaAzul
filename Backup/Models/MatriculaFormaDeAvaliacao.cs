using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotaAzul.Models
{
    public class MatriculaFormaDeAvaliacao: Prion.Generic.Models.Base
    {
        private Int32 _id;
        private Int32 _idMatricula;        
        private Int32 _idFormaDeAvaliacao;
        private Models.FormaDeAvaliacao _formaDeAvaliacao;
        private Decimal _valorAlcancado;

        public Int32 Id
        {
            get { return _id; }
            set { _id = value; this.AlterarEstadoObjeto(); }
        }

        public Int32 IdMatricula
        {
            get { return _idMatricula ; }
            set { _idMatricula = value; this.AlterarEstadoObjeto(); }
        }
       
        public Int32 IdFormaDeAvaliacao
        {
            get { return _idFormaDeAvaliacao; }
            set { _idFormaDeAvaliacao = value; this.AlterarEstadoObjeto(); }
        }

        public FormaDeAvaliacao FormaDeAvaliacao
        {
            get
            {
                if (_formaDeAvaliacao == null)
                {
                    _formaDeAvaliacao = new FormaDeAvaliacao();
                    _formaDeAvaliacao.Id = _idFormaDeAvaliacao;
                }
                return _formaDeAvaliacao;
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto(); return; }
                _formaDeAvaliacao = value;
                _idFormaDeAvaliacao = _formaDeAvaliacao.Id;
            }
        }

        public Decimal ValorAlcancado
        {
            get { return _valorAlcancado; }
            set { _valorAlcancado = value; this.AlterarEstadoObjeto(); }
        }
    }
}