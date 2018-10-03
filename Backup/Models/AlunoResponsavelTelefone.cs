using System;

namespace NotaAzul.Models
{
    public class AlunoResponsavelTelefone : Prion.Generic.Models.Telefone
    {
        private Int32 _id;
        private Int32 _idAlunoResponsavel;
        private AlunoResponsavel _alunoResponsavel;


        public Int32 Id
        {
            get { return _id; }
            set { _id = value; this.AlterarEstadoObjeto(); }
        }

        public Int32 IdAlunoResponsavel
        {
            get { return _idAlunoResponsavel; }
            set { _idAlunoResponsavel = value; this.AlterarEstadoObjeto(); }
        }

        public AlunoResponsavel AlunoResponsavel
        {
            get
            {
                if (_alunoResponsavel == null)
                {
                    _alunoResponsavel = new AlunoResponsavel();
                    _alunoResponsavel.Id = _idAlunoResponsavel;
                }

                return _alunoResponsavel;
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto(); return; }

                _alunoResponsavel = value;
                _idAlunoResponsavel = _alunoResponsavel.Id;
                this.AlterarEstadoObjeto();
            }
        }
    }
}