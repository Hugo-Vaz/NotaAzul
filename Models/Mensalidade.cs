using System;
using System.Collections.Generic;

namespace NotaAzul.Models
{
    public class Mensalidade : Prion.Generic.Models.Base
    {
        private Int32 _id;
        private Int32 _idCorporacao;
        private Int32 _idBoleto;
        private Prion.Generic.Models.Corporacao _corporacao;
        private Int32 _idSituacao;
        private Prion.Generic.Models.Situacao _situacao;
        private Int32 _idMatriculaCurso;
        private MatriculaCurso _matriculaCurso;    
        private Decimal _valor;
        private Decimal _acrescimo;
        private Decimal _desconto;
        private Boolean _isento;
        private DateTime _dataVencimento;
        private Prion.Generic.Models.Titulo _titulo;


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

        public Int32 IdBoleto
        {
            get { return _idBoleto; }
            set { _idBoleto = value; this.AlterarEstadoObjeto(); }
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

        public Int32 IdMatriculaCurso
        {
            get { return _idMatriculaCurso; }
            set { _idMatriculaCurso = value; this.AlterarEstadoObjeto(); }
        }

        public MatriculaCurso MatriculaCurso
        {
            get
            {
                if (_matriculaCurso == null)
                {
                    _matriculaCurso = new MatriculaCurso();
                    _matriculaCurso.Id = _idMatriculaCurso;
                }
                return _matriculaCurso;
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto(); return; }
                _matriculaCurso = value;
                _idMatriculaCurso = _matriculaCurso.Id;
            }
        }

        public Decimal Valor
        {
            get { return _valor; }
            set { _valor = value; this.AlterarEstadoObjeto(); }
        }

        public Decimal Acrescimo
        {
            get { return _acrescimo; }
            set { _acrescimo = value; this.AlterarEstadoObjeto(); }
        }

        public Decimal Desconto
        {
            get { return _desconto; }
            set { _desconto = value; this.AlterarEstadoObjeto(); }
        }

        public Boolean Isento
        {
            get { return _isento; }
            set { _isento = value; this.AlterarEstadoObjeto(); }
        }

        public DateTime DataVencimento
        {
            get { return _dataVencimento; }
            set { _dataVencimento = value; this.AlterarEstadoObjeto(); }
        }

        public Prion.Generic.Models.Titulo Titulo
        {
            get
            {
                if (_titulo == null) { _titulo = new Prion.Generic.Models.Titulo(); }
                return _titulo;
            }
            set
            {
                if (_titulo != null) { _titulo = null; }
                _titulo = value;
            }
        }
    }
}