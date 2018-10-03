using System;

namespace NotaAzul.Models
{
    public class ProfessorDisciplina : Prion.Generic.Models.Base
    {
        private Int32 _id;
        private Int32 _idDisciplina;
        private Disciplina _disciplina;
        private Int32 _idFuncionario;
        private Prion.Generic.Models.Funcionario _funcionario;
        private Int32 _idTurma;
        private Models.Turma _turma;
        private Int32 _idSituacao;
        private Prion.Generic.Models.Situacao _situacao;


        public Int32 Id
        {
            get { return _id; }
            set { _id = value; this.AlterarEstadoObjeto(); }
        }

        public Int32 IdDisciplina
        {
            get { return _idDisciplina; }
            set { _idDisciplina = value; this.AlterarEstadoObjeto(); }
        }

        public Disciplina Disciplina
        {
            get
            {
                if (_disciplina == null)
                {
                    _disciplina = new Disciplina();
                    _disciplina.Id = _idDisciplina;
                }

                return _disciplina;
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto(); return; }

                _disciplina = value;
                _idDisciplina = _disciplina.Id;
                this.AlterarEstadoObjeto();
            }
        }

        /// <summary>
        /// Foi chamado de IdProfessor apenas para ficar mais intuitivo, mas na verdade está classe é associada à Funcionário,
        /// pois Professor é um tipo de Funcionário
        /// </summary>
        public Int32 IdProfessor
        {
            get { return _idFuncionario; }
            set { _idFuncionario = value; this.AlterarEstadoObjeto(); }
        }

        /// <summary>
        /// Foi chamado de Professor apenas para ficar mais intuitivo, mas na verdade está classe é associada à Funcionário,
        /// pois Professor é um tipo de Funcionário
        /// </summary>
        public Prion.Generic.Models.Funcionario Professor
        {
            get
            {
                if (_funcionario == null)
                {
                    _funcionario = new Prion.Generic.Models.Funcionario();
                    _funcionario.Id = _idFuncionario;
                }

                return _funcionario;
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto(); return; }

                _funcionario = value;
                _idFuncionario = _funcionario.Id;
                this.AlterarEstadoObjeto();
            }
        }

        public Int32 IdTurma
        {
            get { return _idTurma; }
            set { _idTurma = value; this.AlterarEstadoObjeto(); }
        }

    
        public Models.Turma Turma
        {
            get
            {
                if (_turma == null)
                {
                    _turma = new Models.Turma();
                    _turma.Id = _idTurma;
                }

                return _turma;
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto(); return; }

                _turma = value;
                _idTurma = _turma.Id;
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
    }
}