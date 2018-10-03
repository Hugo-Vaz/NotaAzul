using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotaAzul.Models
{
    public class MatriculaNota : Prion.Generic.Models.Base
    {
        private Int32 _id;
        private Int32 _idMatricula;
        private Int32 _idComposicaoNota;
        private Models.ComposicaoNota _composicaoNota;
        private Decimal _valorAlcancado;
        private Decimal _valorFinal;
        private List<Models.MatriculaFormaDeAvaliacao> _listaMatriculaFormaDeAvaliacao;

        public Int32 Id
        {
            get { return _id; }
            set { _id = value; this.AlterarEstadoObjeto(); }
        }

        public Int32 IdMatricula
        {
            get { return _idMatricula; }
            set { _idMatricula = value; this.AlterarEstadoObjeto(); }
        }

        public Int32 IdComposicaoNota
        {
            get { return _idComposicaoNota; }
            set { _idComposicaoNota = value; this.AlterarEstadoObjeto(); }
        }

        public ComposicaoNota ComposicaoNota
        {
            get
            {
                if (_composicaoNota == null)
                {
                    _composicaoNota = new ComposicaoNota();
                    _composicaoNota.Id = _idComposicaoNota;
                }
                return _composicaoNota;
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto(); return; }
                _composicaoNota = value;
                _idComposicaoNota = _composicaoNota.Id;
            }
        }

        public Decimal ValorAlcancado
        {
            get { return _valorAlcancado; }
            set { _valorAlcancado = value; this.AlterarEstadoObjeto(); }
        }

        public Decimal ValorFinal
        {
            get { return _valorFinal; }
            set { _valorFinal = value; this.AlterarEstadoObjeto(); }
        }

        public List<Models.MatriculaFormaDeAvaliacao> FormasAvaliacao
        {
            get
            {
                if (_listaMatriculaFormaDeAvaliacao == null) { _listaMatriculaFormaDeAvaliacao = new List<Models.MatriculaFormaDeAvaliacao>(); }
                return _listaMatriculaFormaDeAvaliacao;
            }
            set
            {
                if (_listaMatriculaFormaDeAvaliacao != null) { _listaMatriculaFormaDeAvaliacao = null; }
                _listaMatriculaFormaDeAvaliacao = value;
            }
        }
    }
}