using System;
using System.Collections.Generic;

namespace NotaAzul.Models
{
    public class Matricula : Prion.Generic.Models.Base
    {
        private Int32 _id;        
        private Int32 _idAluno;
        private Aluno _aluno;
        private Int32 _idSituacao;
        private Int32 _idAlunoFilantropia;
        private Prion.Generic.Models.Situacao _situacao;
        private Models.AlunoFilantropia _filantropia;
        private List<Models.Observacao> _listaObservacao;
        private List<Models.MatriculaCurso> _listaMatriculaCurso;
        private List<Models.Boleto> _listaBoletos;
        private String _numeroMatricula;

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

        public Int32 IdAlunoFilantropia
        {
            get { return _idAlunoFilantropia; }
            set { _idAlunoFilantropia = value; this.AlterarEstadoObjeto(); }
        }

        public AlunoFilantropia AlunoFilantropia
        {
            get
            {
                if (_filantropia == null)
                {
                    _filantropia = new AlunoFilantropia();
                    _filantropia.Id = _idAlunoFilantropia;
                }

                return _filantropia;
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto(); return; }

                _filantropia = value;
                _idAlunoFilantropia = _filantropia.Id;
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

        
        public List<Models.Observacao> Observacoes
        {
            get
            {
                if (_listaObservacao == null) { _listaObservacao = new List<Models.Observacao>(); }
                return _listaObservacao;
            }
            set
            {
                if (_listaObservacao != null) { _listaObservacao = null; }
                _listaObservacao = value;
            }
        }

        public List<Models.MatriculaCurso> ListaMatriculaCurso
        {
            get
            {
                if (_listaMatriculaCurso == null) { _listaMatriculaCurso = new List<Models.MatriculaCurso>(); }
                return _listaMatriculaCurso;
            }
            set
            {
                if (_listaMatriculaCurso != null) { _listaMatriculaCurso = null; }
                _listaMatriculaCurso = value;
            }
        }

        public List<Models.Boleto> ListaBoletos
        {
            get
            {
                if (_listaBoletos == null) { _listaBoletos = new List<Models.Boleto>(); }
                return _listaBoletos;
            }
            set
            {
                if (_listaBoletos != null) { _listaBoletos = null; }
                _listaBoletos = value;
            }
        }

        public String NumeroMatricula
        {
            get { return _numeroMatricula; }
            set { this._numeroMatricula = value; this.AlterarEstadoObjeto(); }
        }

    }
}