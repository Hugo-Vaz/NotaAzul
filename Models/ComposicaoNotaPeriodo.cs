using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotaAzul.Models
{
    public class ComposicaoNotaPeriodo:Prion.Generic.Models.Base
    {
        private Int32 _id;
        private Int32 _idProfessorDisciplina;
        private Int32 _periodoDeAvaliacao;
        private String _formaDivisaoAnoLetivo;
        private List<Models.ComposicaoNota> _notas;
        private Models.ProfessorDisciplina _professor;

        public Int32 Id
        {
            get { return _id; }
            set { _id = value; this.AlterarEstadoObjeto(); }
        }

        public Int32 IdProfessorDisciplina
        {
            get { return _idProfessorDisciplina; }
            set { _idProfessorDisciplina = value; this.AlterarEstadoObjeto(); }
        }

        public Int32 PeriodoDeAvaliacao
        {
            get { return _periodoDeAvaliacao; }
            set { _periodoDeAvaliacao = value; this.AlterarEstadoObjeto(); }
        }

        public String FormaDivisaoAnoLetivo
        {
            get { return _formaDivisaoAnoLetivo; }
            set { _formaDivisaoAnoLetivo = value; this.AlterarEstadoObjeto(); }
        }

        public List<Models.ComposicaoNota> ListaNotas
        {
            get
            {
                if (_notas == null) { _notas = new List<Models.ComposicaoNota>(); }
                return _notas;
            }
            set
            {
                if (_notas != null) { _notas = null; }
                _notas = value;
            }
        }


        public Models.ProfessorDisciplina Professor
        {
            get
            {
                if (_professor == null)
                {
                    _professor = new Models.ProfessorDisciplina();
                    _professor.Id = _idProfessorDisciplina;
                }
                return _professor;
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto(); return; }
                _professor = value;
                _idProfessorDisciplina = _professor.Id;
                //_cidadeRegistroNascimento.Estado = new Prion.Generic.Models.Estado();
                this.AlterarEstadoObjeto();
            }
        }
    }
}