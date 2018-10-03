using System;
using System.Collections.Generic;

namespace NotaAzul.Models
{
    public class Curso : Prion.Generic.Models.Base
    {
        private Int32 _id;
        private Int32 _idCorporacao;
        private Prion.Generic.Models.Corporacao _corporacao;
        private Int32 _idSegmento;
        private Segmento _segmento;
        private Int32 _idSituacao;
        private Prion.Generic.Models.Situacao _situacao;
        private String _nome;
        private List<Models.Turma> _listaTurma;
        private List<Models.Material> _listaMaterial;
        private Boolean _cursoCurricular;

        public Int32 Id
        {
            get { return _id; }
            set { _id = value; this.AlterarEstadoObjeto(); }
        }

        public Int32 IdCorporacao
        {
            get { return _idCorporacao; }
            set { _idCorporacao = value; this.AlterarEstadoObjeto(); }
        }

        public Prion.Generic.Models.Corporacao Corporacao
        {
            get
            {
                if (_corporacao == null)
                {
                    _corporacao = new Prion.Generic.Models.Corporacao();
                    _corporacao.Id = _idCorporacao;
                }

                return _corporacao;
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto(); return; }

                _corporacao = value;
                _idCorporacao = _corporacao.Id;
                this.AlterarEstadoObjeto();
            }
        }

        public Int32 IdSegmento
        {
            get { return _idSegmento; }
            set { _idSegmento = value; this.AlterarEstadoObjeto(); }
        }

        public Segmento Segmento
        {
            get
            {
                if (_segmento == null)
                {
                    _segmento = new Segmento();
                    _segmento.Id = _idSegmento;
                }

                return _segmento;
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto(); return; }

                _segmento = value;
                _idSegmento = _segmento.Id;
                this.AlterarEstadoObjeto();
            }
        }

        public Int32 IdSituacao
        {
            get { return _idSituacao; }
            set { _idSituacao = value; this.AlterarEstadoObjeto(); }
        }

        public Prion.Generic.Models.Situacao Situacao
        {
            get
            {
                if (_situacao == null)
                {
                    _situacao = new Prion.Generic.Models.Situacao();
                    _situacao.Id = _idSituacao;
                }

                return _situacao;
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto(); return; }

                _situacao = value;
                _idSituacao = _situacao.Id;
                this.AlterarEstadoObjeto();
            }
        }

        public String Nome
        {
            get { return _nome; }
            set { _nome = value; this.AlterarEstadoObjeto(); }
        }

        public List<Models.Turma> Turmas
        {
            get
            {
                if (_listaTurma == null) { _listaTurma = new List<Models.Turma>(); }
                return _listaTurma;
            }
            set
            {
                if (_listaTurma != null) { _listaTurma = null; }
                _listaTurma = value;
            }
        }

        public List<Models.Material> Materiais
        {
            get
            {
                if (_listaMaterial == null) { _listaMaterial = new List<Models.Material>(); }
                return _listaMaterial;
            }
            set
            {
                if (_listaMaterial != null) { _listaMaterial = null; }
                _listaMaterial = value;
            }
        }

        public Boolean CursoCurricular
        {
            get { return _cursoCurricular; }
            set { _cursoCurricular = value; this.AlterarEstadoObjeto(); }
        }
    }
}