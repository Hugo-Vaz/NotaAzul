using System;

namespace NotaAzul.Models
{
    public class CursoAnoLetivo : Prion.Generic.Models.Base
    {
        private Int32 _id;
        private Int32 _idCurso;
        private Curso _curso;
        private Int32 _idSituacao;        
        private Prion.Generic.Models.Situacao _situacao;
        private Int32 _anoLetivo;
        private Decimal _valorMatricula;
        private Decimal _valorMensalidade;
        private Int32 _quantidadeMensalidades;


        public Int32 Id
        {
            get { return _id; }
            set { _id = value; this.AlterarEstadoObjeto(); }
        }

        public Int32 IdCurso
        {
            get { return _idCurso; }
            set { _idCurso = value; this.AlterarEstadoObjeto(); }
        }

       
        public Curso Curso
        {
            get
            {
                if (_curso == null)
                {
                    _curso = new Curso();
                    _curso.Id = _idCurso;
                }

                return _curso;
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto(); return; }

                _curso = value;
                _idCurso = _curso.Id;
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

        public Int32 AnoLetivo
        {
            get { return _anoLetivo; }
            set { _anoLetivo = value; this.AlterarEstadoObjeto(); }
        }

        public Decimal ValorMatricula
        {
            get { return _valorMatricula; }
            set { _valorMatricula = value; this.AlterarEstadoObjeto(); }
        }

        public Decimal ValorMensalidade
        {
            get { return _valorMensalidade; }
            set { _valorMensalidade = value; this.AlterarEstadoObjeto(); }
        }

        public Int32 QuantidadeMensalidades
        {
            get { return _quantidadeMensalidades; }
            set { _quantidadeMensalidades = value; this.AlterarEstadoObjeto(); }
        }
    }
}