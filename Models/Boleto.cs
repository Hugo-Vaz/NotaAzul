using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotaAzul.Models
{
    public class Boleto : Prion.Generic.Models.Base
    {
        private Int32 _id;
        private String _statusBoleto;
        private Int32 _idCorporacao;
        private Decimal _valor;
        private Decimal _juros;
        private Decimal _desconto;
        private Int32 _convenio;
        private String _carteira;
        private String _numeroDocumento;
        private String _numeroConta;
        private String _numeroAgencia;
        private String _nossoNumero;
        public String _codigoBanco;
        private DateTime _dataEmissao;
        private DateTime _dataVencimento;
        private Decimal _valorPago;
        private DateTime _dataPagamento;
        private String _nomeAluno;
        private Boolean _remessaGerado;


        public List<Int32> IdsMensalidade
        {
            get; set;
        }

        public Int32 Id
        {
            get { return _id; }
            set { _id = value; this.AlterarEstadoObjeto(); }
        }

        public String StatusBoleto
        {
            get { return _statusBoleto; }
            set { _statusBoleto = value; this.AlterarEstadoObjeto(); }
        }

        public Int32 IdCorporacao
        {
            get { return _idCorporacao; }
            set { _idCorporacao = value; this.AlterarEstadoObjeto(); }
        }

        public Decimal Valor
        {
            get { return _valor; }
            set { _valor = value; this.AlterarEstadoObjeto(); }
        }

        public Decimal Desconto
        {
            get { return _desconto; }
            set { _desconto = value; this.AlterarEstadoObjeto(); }
        }

        public Decimal Juros
        {
            get { return _juros; }
            set { _juros = value; this.AlterarEstadoObjeto(); }
        }

        public Int32 Convenio
        {
            get { return _convenio; }
            set { _convenio = value; this.AlterarEstadoObjeto(); }
        }

        public String Carteira
        {
            get { return _carteira; }
            set { _carteira = value; this.AlterarEstadoObjeto(); }
        }

        public String NomeAluno
        {
            get { return _nomeAluno; }
            set { _nomeAluno = value; this.AlterarEstadoObjeto(); }
        }

        public String NumeroDocumento
        {
            get { return _numeroDocumento; }
            set { _numeroDocumento = value; this.AlterarEstadoObjeto(); }
        }

        public String CodigoBanco
        {
            get { return _codigoBanco; }
            set { _codigoBanco = value; this.AlterarEstadoObjeto(); }
        }

        public String NumeroAgencia
        {
            get { return _numeroAgencia; }
            set { _numeroAgencia = value; this.AlterarEstadoObjeto(); }
        }

        public String NumeroConta
        {
            get { return _numeroConta; }
            set { _numeroConta = value; this.AlterarEstadoObjeto(); }
        }

        public String NossoNumero
        {
            get { return _nossoNumero; }
            set { _nossoNumero = value; this.AlterarEstadoObjeto(); }
        }

        public DateTime DataEmissao
        {
            get { return _dataEmissao; }
            set { _dataEmissao = value; this.AlterarEstadoObjeto(); }
        }

        public DateTime DataVencimento
        {
            get { return _dataVencimento; }
            set { _dataVencimento = value; this.AlterarEstadoObjeto(); }
        }

        public Decimal ValorPago
        {
            get { return _valorPago; }
            set { _valorPago = value; this.AlterarEstadoObjeto(); }
        }

        public DateTime DataPagamento
        {
            get { return _dataPagamento; }
            set { _dataPagamento = value; this.AlterarEstadoObjeto(); }
        }

        public Boolean RemessaGerado
        {
            get { return _remessaGerado; }
            set { _remessaGerado = value; this.AlterarEstadoObjeto(); }
        }

        public String RemessaGeradoStr
        {
            get { return (_remessaGerado) ? "Sim" : "Não"; }
           
        }
    }
}