using System;

namespace NotaAzul.Models
{
    public class GrupoDesconto : Prion.Generic.Models.Base
    {
        private Int32 _id;
        private Int32 _idSituacao;
        private Prion.Generic.Models.Situacao _situacao;
        private String _nome;
        private String _descricao;
        private String _tipoDesconto;
        private Decimal _valor;

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
                if (_situacao == null){
                    _situacao= new Prion.Generic.Models.Situacao();
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

        public String Nome
        {
            get { return _nome; }
            set { _nome = value; this.AlterarEstadoObjeto(); }
        }

        public String Descricao
        {
            get { return _descricao; }
            set { _descricao = value; this.AlterarEstadoObjeto(); }
        }

        public String TipoDesconto
        {
            get { return _tipoDesconto; }
            set { _tipoDesconto = value; this.AlterarEstadoObjeto(); }
        }

        public Decimal Valor
        {
            get{return _valor;}
            set { _valor = value; this.AlterarEstadoObjeto(); }
        }
    }
}