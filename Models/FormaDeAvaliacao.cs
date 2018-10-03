using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotaAzul.Models
{
    public class FormaDeAvaliacao : Prion.Generic.Models.Base
    {
        private Int32 _id;
        private Int32 _idComposicaoNota;
        private String _tipo;
        private Decimal _valor;
        private DateTime _dataAvaliacao;
        public Int32 Id
        {
            get { return _id; }
            set { _id = value; this.AlterarEstadoObjeto(); }
        }

        public Int32 IdComposicaoNota
        {
            get { return _idComposicaoNota; }
            set { _idComposicaoNota = value; this.AlterarEstadoObjeto(); }
        }

        public String Tipo
        {
            get { return _tipo; }
            set { _tipo = value; this.AlterarEstadoObjeto(); }
        }

        public Decimal Valor
        {
            get { return _valor; }
            set { _valor = value; this.AlterarEstadoObjeto(); }
        }

        public DateTime DataAvaliacao
        {
            get { return _dataAvaliacao; }
            set { _dataAvaliacao = value; this.AlterarEstadoObjeto(); }
        }
    }
}