using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotaAzul.Models
{
    public class MatriculaMedia : Prion.Generic.Models.Base
    {
        private Int32 _id;
        private Int32 _idMatricula;
        private Int32 _idComposicaoNotaPeriodo;
        private Int32 _idDisciplina;
        private Int32 _periodoAvaliacao;
        private Models.ComposicaoNotaPeriodo _composicaoNotaPeriodo;
        private Decimal _valorAlcancado;
        private List<Models.MatriculaNota> _listaMatriculaNota;
        
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

        public Int32 IdComposicaoNotaPeriodo
        {
            get { return _idComposicaoNotaPeriodo; }
            set { _idComposicaoNotaPeriodo = value; this.AlterarEstadoObjeto(); }
        }

        public Int32 IdDisciplina
        {
            get { return _idDisciplina; }
            set { _idDisciplina = value; this.AlterarEstadoObjeto(); }
        }

        public Int32 PeriodoAvaliacao
        {
            get { return _periodoAvaliacao; }
            set { _periodoAvaliacao = value; this.AlterarEstadoObjeto(); }
        }

        public ComposicaoNotaPeriodo ComposicaoNotaPeriodo
        {
            get
            {
                if (_composicaoNotaPeriodo == null)
                {
                    _composicaoNotaPeriodo = new ComposicaoNotaPeriodo();
                    _composicaoNotaPeriodo.Id = _idComposicaoNotaPeriodo;
                }
                return _composicaoNotaPeriodo;
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto(); return; }
                _composicaoNotaPeriodo = value;
                _idComposicaoNotaPeriodo = _composicaoNotaPeriodo.Id;
            }
        }

        public Decimal ValorAlcancado
        {
            get { return _valorAlcancado; }
            set { _valorAlcancado = value; this.AlterarEstadoObjeto(); }
        }

        public List<Models.MatriculaNota> Notas
        {
            get
            {
                if (_listaMatriculaNota == null) { _listaMatriculaNota = new List<Models.MatriculaNota>(); }
                return _listaMatriculaNota;
            }
            set
            {
                if (_listaMatriculaNota != null) { _listaMatriculaNota = null; }
                _listaMatriculaNota = value;
            }
        }
    }
}