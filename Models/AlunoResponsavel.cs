using System;
using System.Collections.Generic;
namespace NotaAzul.Models
{
    public class AlunoResponsavel : Prion.Generic.Models.Base
    {
        private Int32 _id;
        private Int32 _idSituacao;
        private Prion.Generic.Models.Situacao _situacao;
        private Int32 _idImagem;
        private Prion.Generic.Models.Imagem _imagem;
        private Int32 _idAluno;
        private Models.Aluno _aluno;
        private String _nome;
        private String _cpf;
        private String _rg;
        private String _grauParentesco;
        private String _profissao;
        private String _email;
        private Boolean _financeiro;
        private Boolean _moraCom;
        private Prion.Generic.Models.Endereco _endereco;
        private List<Models.AlunoResponsavelTelefone> _telefones; 

        public Int32 Id
        {
            get { return _id; }
            set { _id = value; this.AlterarEstadoObjeto(); }
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

        public Int32 IdImagem
        {
            get { return _idImagem; }
            set { _idImagem = value; this.AlterarEstadoObjeto(); }
        }

        public Prion.Generic.Models.Imagem Imagem
        {
            get
            {
                if (_imagem == null)
                {
                    _imagem = new Prion.Generic.Models.Imagem();
                    _imagem.Id = _idImagem;
                }

                return _imagem;
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto(); return; }

                _imagem = value;
                _idImagem = _imagem.Id;
                this.AlterarEstadoObjeto();
            }
        }

        public Int32 IdAluno
        {
            get { return _idAluno; }
            set { _idAluno = value; this.AlterarEstadoObjeto(); }
        }

        public Models.Aluno Aluno
        {
            get
            {
                if (_aluno == null)
                {
                    _aluno = new Models.Aluno();
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

        public String Nome
        {
            get { return _nome; }
            set { _nome = value; this.AlterarEstadoObjeto(); }
        }

        public String GrauParentesco
        {
            get { return _grauParentesco; }
            set { _grauParentesco = value; this.AlterarEstadoObjeto(); }
        }

        public String CPF
        {
            get { return _cpf; }
            set { _cpf = value; this.AlterarEstadoObjeto(); }
        }

        public String RG
        {
            get { return _rg; }
            set { _rg = value; this.AlterarEstadoObjeto(); }
        }

        public String Profissao
        {
            get { return _profissao; }
            set { _profissao = value; this.AlterarEstadoObjeto(); }
        }

        public String Email
        {
            get { return _email; }
            set { _email = value; this.AlterarEstadoObjeto(); }
        }

        public Boolean Financeiro
        {
            get { return _financeiro; }
            set { _financeiro = value; this.AlterarEstadoObjeto(); }
        }

        public Boolean MoraCom
        {
            get { return _moraCom; }
            set { _moraCom = value; this.AlterarEstadoObjeto(); }
        }

        public Prion.Generic.Models.Endereco Endereco
        {
            get
            {
                if (_endereco == null)
                {
                    _endereco = new Prion.Generic.Models.Endereco();
                }

                return _endereco;
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto(); return; }

                _endereco = value;
                this.AlterarEstadoObjeto();
            }
        }

        public List<Models.AlunoResponsavelTelefone> Telefones
        {
            get
            {
                if (_telefones == null) { _telefones = new List<Models.AlunoResponsavelTelefone>(); }
                return _telefones;
            }
            set
            {
                if (_telefones != null) { _telefones = null; }
                _telefones = value;
            }
        }
    }
}