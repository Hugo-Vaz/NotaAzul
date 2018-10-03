using System;
using System.Collections.Generic;

namespace NotaAzul.Models
{
    public class Disciplina : Prion.Generic.Models.Base
    {
        private Int32 _id;
        private Int32 _idCorporacao;
        private Int32 _idTurma;//Criado para facilitar o carregamento das disciplinas de um professor em determinada turma
        private Prion.Generic.Models.Corporacao _corporacao;
        private Int32 _idSituacao;
        private Prion.Generic.Models.Situacao _situacao;
        private String _nome;
        private List<Prion.Generic.Models.Funcionario> _listaProfessores;
        private List<Models.Observacao> _listaObservacao;


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

        public Int32 IdTurma
        {
            get { return _idTurma; }
            set { _idTurma = value; this.AlterarEstadoObjeto(); }
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

        public String Nome
        {
            get { return _nome; }
            set { _nome = value; this.AlterarEstadoObjeto(); }
        }

        public List<Prion.Generic.Models.Funcionario> Professores
        {
            get
            {
                if (_listaProfessores == null) { _listaProfessores = new List<Prion.Generic.Models.Funcionario>(); }
                return _listaProfessores;
            }
            set
            {
                if (_listaProfessores != null) { _listaProfessores = null; }
                _listaProfessores = value;
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
    }
}