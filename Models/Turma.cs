using System;

namespace NotaAzul.Models
{
    public class Turma : Prion.Generic.Models.Base
    {
        private Int32 _id;
        private Int32 _idCorporacao;
        private Prion.Generic.Models.Corporacao _corporacao;
        private Int32 _idCursoAnoLetivo;
        private CursoAnoLetivo _cursoAnoLetivo;
        private Int32 _idSituacao;
        private Prion.Generic.Models.Situacao _situacao;
        private Int32 _idTurno;
        private Turno _turno;
        private String _nome;
        private Int32 _anoLetivo;
        private Int32 _quantidadeVagas;
        private Int32[] _idsDisciplina;


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

        public Int32 IdCursoAnoLetivo
        {
            get { return _idCursoAnoLetivo; }
            set { _idCursoAnoLetivo = value; this.AlterarEstadoObjeto(); }
        }

        public CursoAnoLetivo CursoAnoLetivo
        {
            get
            {
                if (_cursoAnoLetivo == null)
                {
                    _cursoAnoLetivo = new CursoAnoLetivo();
                    _cursoAnoLetivo.Id = _idCursoAnoLetivo;
                }

                return _cursoAnoLetivo;
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto(); return; }

                _cursoAnoLetivo = value;
                _idCursoAnoLetivo = _cursoAnoLetivo.Id;
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

        public Int32 IdTurno
        {
            get { return _idTurno; }
            set { _idTurno = value; this.AlterarEstadoObjeto(); }
        }

        public Turno Turno
        {
            get
            {
                if (_turno == null)
                {
                    _turno = new Turno();
                    _turno.Id = _idTurno;
                }

                return _turno;
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto(); return; }

                _turno = value;
                _idTurno = _turno.Id;
                this.AlterarEstadoObjeto();
            }
        }

        public String Nome
        {
            get { return _nome; }
            set { _nome = value; this.AlterarEstadoObjeto(); }
        }

        public Int32 AnoLetivo
        {
            get { return _anoLetivo; }
            set { _anoLetivo = value; this.AlterarEstadoObjeto(); }
        }

        public Int32 QuantidadeVagas
        {
            get { return _quantidadeVagas; }
            set { _quantidadeVagas = value; this.AlterarEstadoObjeto(); }
        }

        public Int32[] IdsDisciplina
        {
            get { return _idsDisciplina; }
            set { _idsDisciplina = value; this.AlterarEstadoObjeto(); }
        }

    }
}