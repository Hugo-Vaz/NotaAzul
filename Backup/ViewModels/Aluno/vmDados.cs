using System;
using System.Collections.Generic;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.ViewModels.Aluno
{
    public class vmDados
    {
        private Models.Aluno _aluno;
        private Models.AlunoResponsavel _responsavel;

        public Models.Aluno Aluno
        {
            get
            {
                if (_aluno == null) return new Models.Aluno();
                return _aluno;
            }
            set
            {
                _aluno = value;
            }
        }

        public Models.AlunoResponsavel Responsavel
        {
            get
            {
                if (_responsavel == null) return new Models.AlunoResponsavel();
                return _responsavel;
            }
            set
            {
                _responsavel = value;
            }
        }

        public List<GenericModels.Situacao> Situacoes { get; set; }

        public List<GenericModels.Situacao> SituacoesResponsavel { get; set; }

        public Int32 SituacaoEndereco { get; set; }
    }
}
