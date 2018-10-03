using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotaAzul.Models
{
    public class ComposicaoNota:Prion.Generic.Models.Base
    {
        private Int32 _id;
        private Int32 _idComposicaoNotaPeriodo;
        private Decimal _peso;
        private List<Models.FormaDeAvaliacao> _formasDeAvaliacao;

        public Int32 Id
        {
            get{return _id;}
            set{_id = value; this.AlterarEstadoObjeto();}
        }

        public Int32 IdComposicaoNotaPeriodo
        {
            get { return _idComposicaoNotaPeriodo; }
            set { _idComposicaoNotaPeriodo = value; this.AlterarEstadoObjeto(); }
        }

        public Decimal Peso
        {
            get { return _peso; }
            set { _peso = value; this.AlterarEstadoObjeto(); }
        }

        public List<Models.FormaDeAvaliacao> ListaFormasDeAvaliacao
        {
            get
            {
                if (_formasDeAvaliacao == null) { _formasDeAvaliacao = new List<Models.FormaDeAvaliacao>(); }
                return _formasDeAvaliacao;
            }
            set
            {
                if (_formasDeAvaliacao != null) { _formasDeAvaliacao = null; }
                _formasDeAvaliacao = value;
            }
        }


    }
}