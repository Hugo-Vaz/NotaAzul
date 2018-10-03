using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotaAzul.Models
{
    public class MatriculaCurso : Prion.Generic.Models.Base
    {
        private Int32 _id;
        private Int32 _idMatricula;
        private Models.Matricula _matricula;
        private Int32 _idTurma;
        private Models.Turma _turma;
        private List<Models.Mensalidade> _listaMensalidades;



        public Int32 Id
        {
            get { return _id; }
            set { _id = value; this.AlterarEstadoObjeto(); }
        }

        public Int32 IdMatricula
        {
            get { return _idMatricula; }
            set { _idMatricula = value; this.AlterarEstadoObjeto(); }
        }

        public Matricula Matricula
        {
            get
            {
                if (_matricula == null)
                {
                    _matricula = new Models.Matricula();
                    _matricula.Id = _idMatricula;
                }
                return _matricula;
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto();return; }
                _matricula = value;
                _idMatricula = _matricula.Id;
                this.AlterarEstadoObjeto();
            }
        }
        public Int32 IdTurma
        {
            get { return _idTurma; }
            set { _idTurma = value; this.AlterarEstadoObjeto(); }

        }
        public Turma Turma
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
            }
        }

        public List<Models.Mensalidade> Mensalidades
        {
            get
            {
                if (_listaMensalidades == null) { _listaMensalidades = new List<Models.Mensalidade>(); }
                return _listaMensalidades;
            }
            set
            {
                if (_listaMensalidades != null) { _listaMensalidades = null; }
                _listaMensalidades = value;
            }
        }
    }
}