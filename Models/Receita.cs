﻿using System;

namespace NotaAzul.Models
{
    public class Receita : Prion.Generic.Models.Base
    {
        private Int32 _id;
        private Int32 _idCorporacao;
        private Prion.Generic.Models.Corporacao _corporacao;
        private String _descricao;
        private Decimal _valor;

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

        public String Descricao
        {
            get { return _descricao; }
            set { _descricao = value; this.AlterarEstadoObjeto(); }
        }

        public Decimal Valor
        {
            get { return _valor; }
            set { _valor = value; this.AlterarEstadoObjeto(); }
        }
    }
}