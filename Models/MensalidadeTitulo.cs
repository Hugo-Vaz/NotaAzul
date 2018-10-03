using System;
using System.Collections.Generic;

namespace NotaAzul.Models
{
    public class MensalidadeTitulo : Prion.Generic.Models.Base
    {
        private Int32 _id;
        private Int32[] _mensalidadesPagas;
        private Int32 _idTitulo;
        private Int32[] _titulosPagos;
        private String _nomeCurso;
        private String _nomeAluno;
        private Decimal _valor;
        private Decimal _acrescimo;
        private Decimal _desconto;
        private Decimal _valorTotal;
        private Decimal _valorPago;
        private DateTime _dataVencimento;
        private DateTime _dataOperacao;
        private List<Prion.Generic.Models.Cartao> _listaCartao;
        private List<Prion.Generic.Models.Cheque> _listaCheque;
        private List<Prion.Generic.Models.Especie> _listaEspecie;

        public Int32 Id
        {
            get { return _id; }
            set { _id = value; this.AlterarEstadoObjeto(); }
        }

        public Int32[] MensalidadesPagas
        {
            get { return _mensalidadesPagas; }
            set { _mensalidadesPagas = value; this.AlterarEstadoObjeto(); }
        }
       
        public Int32 IdTitulo
        {
            get { return _idTitulo; }
            set { _idTitulo = value; this.AlterarEstadoObjeto(); }
        }

        public Int32[] TitulosPagos
        {
            get { return _titulosPagos; }
            set { _titulosPagos = value; this.AlterarEstadoObjeto(); }
        }

        public String NomeCurso
        {
            get { return _nomeCurso; }
            set { _nomeCurso = value; this.AlterarEstadoObjeto(); }
        }

        public String NomeAluno
        {
            get { return _nomeAluno; }
            set { _nomeAluno = value; this.AlterarEstadoObjeto(); }
        }

        public Decimal Valor
        {
            get { return _valor; }
            set { _valor = value; this.AlterarEstadoObjeto(); }
        }

        public Decimal Acrescimo
        {
            get { return _acrescimo; }
            set { _acrescimo = value; this.AlterarEstadoObjeto(); }
        }

        public Decimal Desconto
        {
            get { return _desconto; }
            set { _desconto = value; this.AlterarEstadoObjeto(); }
        }

        public Decimal ValorTotal
        {
            get { return _valorTotal; }
            set { _valorTotal = value; this.AlterarEstadoObjeto(); }
        }
        public Decimal ValorPago
        {
            get { return _valorPago; }
            set { _valorPago = value; this.AlterarEstadoObjeto(); }
        }

        public DateTime DataVencimento
        {
            get { return _dataVencimento; }
            set { _dataVencimento = value; this.AlterarEstadoObjeto(); }
        }

        public DateTime DataOperacao
        {
            get { return _dataOperacao; }
            set { _dataOperacao = value; this.AlterarEstadoObjeto(); }
        }

        public List<Prion.Generic.Models.Cartao> ListaCartao
        {
            get
            {
                if (_listaCartao == null) { _listaCartao = new List<Prion.Generic.Models.Cartao>(); }
                return _listaCartao;
            }
            set
            {
                if (_listaCartao != null) { _listaCartao = null; }
                _listaCartao = value;
            }
        }

        public List<Prion.Generic.Models.Cheque> ListaCheque
        {
            get
            {
                if (_listaCheque == null) { _listaCheque = new List<Prion.Generic.Models.Cheque>(); }
                return _listaCheque;
            }
            set
            {
                if (_listaCheque != null) { _listaCheque = null; }
                _listaCheque = value;
            }
        }

        public List<Prion.Generic.Models.Especie> ListaEspecie
        {
            get
            {
                if (_listaEspecie == null) { _listaEspecie = new List<Prion.Generic.Models.Especie>(); }
                return _listaEspecie;
            }
            set
            {
                if (_listaEspecie != null) { _listaEspecie = null; }
                _listaEspecie = value;
            }
        }
    }
}