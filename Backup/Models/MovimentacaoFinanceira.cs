using System;
using System.Collections.Generic;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.Models
{
    public class MovimentacaoFinanceira : Prion.Generic.Models.Base
    {
        private Int32 _id;             
        private Decimal _valor;        
        private List<GenericModels.Titulo> _titulos; 

        public Int32 Id
        {
            get { return _id; }
            set { _id = value; this.AlterarEstadoObjeto(); }
        }


        public Decimal Valor
        {
            get { return _valor; }
            set { _valor = value; this.AlterarEstadoObjeto(); }        }

        

        public List<GenericModels.Titulo> Titulos
        {
            get
            {
                if(_titulos==null){_titulos = new List<GenericModels.Titulo>();}
                return _titulos;
            }
            set
            {
                if(_titulos==null){_titulos = null;}
                _titulos=value;
            }
        }
    }
}