using System;
using System.Collections.Generic;

namespace NotaAzul.Models
{
    public class Aluno : Prion.Generic.Models.Base
    {
        private Int32 _id;
        private Int32 _idCorporacao;
        private Prion.Generic.Models.Corporacao _corporacao;
        private Int32 _idSituacao;
        private Prion.Generic.Models.Situacao _situacao;
        private Int32 _idImagem;
        private Prion.Generic.Models.Imagem _imagem;
        private Int32 _idCidadeRegistroNascimento;
        private Prion.Generic.Models.Cidade _cidadeRegistroNascimento;
        private Int32 _idEstadoRegistroNascimento;
        private Prion.Generic.Models.Estado _estadoRegistroNascimento;
        private String _nome;
        private String _sexo;
        private String _grupoSanguineo;
        private String _cpf;
        private String _rg;
        private int _diaPagamento;
        private DateTime _dataNascimento;
        private String _nacionalidade;
        private String _religiao;
        private String _corRaca;
        private String _registroDeNascimento;
        private String _numeroOrdem;
        private String _folhas;
        private String _numeroLivro;
        private DateTime _dataRegistroNascimento;
        private String _nis;
        private String _observacaoSaude;
        private String _observacaoMedicacao;
        private List<Models.AlunoResponsavel> _listaResponsavel;
        private List<Models.AlunoFilantropia> _listaFilantropia;
        private List<Models.AlunoResponsavelGrauResponsabilidade> _listaGrauResponsabilidade;

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

        public Int32 IdCidadeRegistroNascimento
        {
            get { return _idCidadeRegistroNascimento; }
            set { _idCidadeRegistroNascimento = value; this.AlterarEstadoObjeto(); }
        }

        public Prion.Generic.Models.Cidade CidadeRegistroNascimento
        {
            get 
            {
                if (_cidadeRegistroNascimento == null)
                {
                    _cidadeRegistroNascimento = new Prion.Generic.Models.Cidade();
                    _cidadeRegistroNascimento.Id = _idCidadeRegistroNascimento;                    
                }
                return _cidadeRegistroNascimento; 
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto(); return; }
                _cidadeRegistroNascimento=value;
                _idCidadeRegistroNascimento = _cidadeRegistroNascimento.Id;
                //_cidadeRegistroNascimento.Estado = new Prion.Generic.Models.Estado();
                this.AlterarEstadoObjeto();
            }
        }

        public Int32 IdEstadoRegistroNascimento
        {
            get { return _idEstadoRegistroNascimento; }
            set { _idEstadoRegistroNascimento = value; this.AlterarEstadoObjeto(); }
        }

        public Prion.Generic.Models.Estado EstadoRegistroNascimento{
            get
            {
                if (_estadoRegistroNascimento == null)
                {
                    _estadoRegistroNascimento = new Prion.Generic.Models.Estado();
                    _estadoRegistroNascimento.Id = _idEstadoRegistroNascimento;                    
                }
                return _estadoRegistroNascimento;
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto(); return; }
                _estadoRegistroNascimento = value;
                _idEstadoRegistroNascimento = _estadoRegistroNascimento.Id;
                //_estadoRegistroNascimento.Cidades = null;
                this.AlterarEstadoObjeto();
            }
        }
        
        public String Nome
        {
            get { return _nome; }
            set { _nome = value; this.AlterarEstadoObjeto(); }
        }

        public String Sexo
        {
            get { return _sexo; }
            set { _sexo = value; this.AlterarEstadoObjeto(); }
        }

        public String GrupoSanguineo
        {
            get { return _grupoSanguineo; }
            set { _grupoSanguineo = value; this.AlterarEstadoObjeto(); }
        }

        public String Cpf
        {
            get { return _cpf; }
            set { _cpf = value; this.AlterarEstadoObjeto(); }
        }

        public String Rg
        {
            get { return _rg;  }
            set { _rg = value; this.AlterarEstadoObjeto(); }
        }

        public Int32 DiaPagamento
        {
            get { return _diaPagamento; }
            set { _diaPagamento = value; this.AlterarEstadoObjeto(); }
        }

        public DateTime DataNascimento
        {
            get { return _dataNascimento; }
            set { _dataNascimento = value; this.AlterarEstadoObjeto(); }
        }

        public String Nacionalidade
        {
            get { return _nacionalidade; }
            set { _nacionalidade = value; this.AlterarEstadoObjeto(); }
        }

        public String Religiao
        {
            get { return _religiao; }
            set { _religiao = value; this.AlterarEstadoObjeto(); }
        }

        public String CorRaca
        {
            get { return _corRaca; }
            set { _corRaca = value; this.AlterarEstadoObjeto(); }
        }

        public String RegistroDeNascimento
        {
            get { return _registroDeNascimento; }
            set { _registroDeNascimento = value; this.AlterarEstadoObjeto(); }
        }

        public String NumeroOrdem
        {
            get { return _numeroOrdem; }
            set { _numeroOrdem = value; this.AlterarEstadoObjeto(); }
        }

        public String Folhas
        {
            get { return _folhas; }
            set { _folhas = value; this.AlterarEstadoObjeto(); }
        }

        public String NumeroLivro
        {
            get { return _numeroLivro; }
            set { _numeroLivro = value; this.AlterarEstadoObjeto(); }
        }

        public DateTime DataRegistroNascimento
        {
            get { return _dataRegistroNascimento; }
            set { _dataRegistroNascimento = value; this.AlterarEstadoObjeto(); }
        }

        public String NIS
        {
            get { return _nis; }
            set { _nis = value; this.AlterarEstadoObjeto(); }
        }

        public String ObservacaoSaude
        {
            get { return _observacaoSaude; }
            set { _observacaoSaude = value; this.AlterarEstadoObjeto(); }
        }


        public String ObservacaoMedicacao
        {
            get { return _observacaoMedicacao; }
            set { _observacaoMedicacao = value; this.AlterarEstadoObjeto(); }
        }


        public List<Models.AlunoResponsavel> Responsaveis
        {
            get
            {
                if (_listaResponsavel == null) { _listaResponsavel = new List<Models.AlunoResponsavel>(); }
                return _listaResponsavel;
            }
            set
            {
                if (_listaResponsavel != null) { _listaResponsavel = null; }
                _listaResponsavel = value;
            }
        }

        public List<Models.AlunoFilantropia> Filantropia
        {
            get
            {
                if (_listaFilantropia == null) { _listaFilantropia = new List<Models.AlunoFilantropia>(); }
                return _listaFilantropia;
            }
            set
            {
                if (_listaFilantropia != null) { _listaFilantropia = null; }
                _listaFilantropia = value;
            }
        }

        public List<Models.AlunoResponsavelGrauResponsabilidade> GrauResponsabilidades
        {
            get
            {
                if (_listaGrauResponsabilidade == null) { _listaGrauResponsabilidade = new List<Models.AlunoResponsavelGrauResponsabilidade>(); }
                return _listaGrauResponsabilidade;
            }
            set
            {
                if (_listaGrauResponsabilidade != null) { _listaGrauResponsabilidade = null; }
                _listaGrauResponsabilidade = value;
            }
        }
    }
}