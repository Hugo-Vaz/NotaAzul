using System;

namespace NotaAzul.Models
{
    public class AlunoFilantropia : Prion.Generic.Models.Base
    {
        private Int32 _id;
        private Int32 _idAluno;
        private Aluno _aluno;
        private Int32 _anoLetivo;
        private Decimal _valorBolsa;


        public Int32 Id
        {
            get { return _id; }
            set { _id = value; this.AlterarEstadoObjeto(); }
        }

        public Int32 IdAluno
        {
            get { return _idAluno; }
            set { _idAluno = value; this.AlterarEstadoObjeto(); }
        }

        public Aluno Aluno
        {
            get
            {
                if (_aluno == null)
                {
                    _aluno = new Aluno();
                    _aluno.Id = _idAluno;
                }

                return _aluno;
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto(); return; }

                _aluno = value;
                _idAluno = _aluno.Id;
                this.AlterarEstadoObjeto();
            }
        }

        public Int32 AnoLetivo
        {
            get { return _anoLetivo; }
            set { _anoLetivo = value; this.AlterarEstadoObjeto(); }
        }

        public Decimal ValorBolsa
        {
            get { return _valorBolsa; }
            set { _valorBolsa = value; this.AlterarEstadoObjeto(); }
        }
    }
}