using System;
using System.Collections.Generic;

namespace NotaAzul.Models
{
    public class Professor : Prion.Generic.Models.Base
    {
        private Int32 _id;
        private Int32 _idFuncionario;
        private Prion.Generic.Models.Funcionario _funcionario;
        private List<Models.Disciplina> _listaDisciplina;

        public Int32 Id
        {
            get { return _id; }
            set { _id = value; this.AlterarEstadoObjeto(); }
        }

        public Int32 IdFuncionario
        {
            get { return _idFuncionario; }
            set { _idFuncionario = value; this.AlterarEstadoObjeto(); }
        }

        public Prion.Generic.Models.Funcionario Funcionario
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

        public List<Models.Disciplina> Disciplinas
        {
            get
            {
                if (_listaDisciplina == null) { _listaDisciplina = new List<Models.Disciplina>(); }
                return _listaDisciplina;
            }
            set
            {
                if (_listaDisciplina != null) { _listaDisciplina = null; }
                _listaDisciplina = value;
            }
        }
    }
}