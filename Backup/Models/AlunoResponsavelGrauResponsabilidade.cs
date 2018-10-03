using System;
using System.Collections.Generic;

namespace NotaAzul.Models
{
    public class AlunoResponsavelGrauResponsabilidade : Prion.Generic.Models.Base
    {
        private Int32 _id;
        private Int32 _idAlunoResponsavel;
        private AlunoResponsavel _alunoResponsavel;
        private Int32 _idAluno;
        private Aluno _aluno;
        private String _tipo;
        private List<Models.AlunoResponsavel> _listaResponsaveis;
        private List<Models.AlunoResponsavelTelefone> _listaTelefone;


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

        public String Tipo
        {
            get { return _tipo; }
            set { _tipo = value; this.AlterarEstadoObjeto();}
        }

        public List<Models.AlunoResponsavel> Responsaveis
        {
            get
            {
                if (_listaResponsaveis == null) { _listaResponsaveis = new List<Models.AlunoResponsavel>(); }
                return _listaResponsaveis;
            }
            set
            {
                if (_listaResponsaveis != null) { _listaResponsaveis = null; }
                _listaResponsaveis = value;
            }
        }

        public List<Models.AlunoResponsavelTelefone> Telefones
        {
            get
            {
                if (_listaTelefone == null) { _listaTelefone = new List<Models.AlunoResponsavelTelefone>(); }
                return _listaTelefone;
            }
            set
            {
                if (_listaTelefone != null) { _listaTelefone = null; }
                _listaTelefone = value;
            }
        }
    }
}