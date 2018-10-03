using System;

namespace NotaAzul.Models 
{
    public class ReceitaTitulo : Prion.Generic.Models.Base
    {
        private Int32 _idReceita;
        private Receita _receita;
        private Int32 _idTitulo;
        private Prion.Generic.Models.Titulo _titulo;

        public Int32 IdReceita
        {
            get { return _idReceita; }
            set { _idReceita = value; this.AlterarEstadoObjeto(); }
        }

        public Receita Receita
        {
            get
            {
                if (_receita == null)
                {
                    _receita = new Receita();
                    _receita.Id = _idReceita;
                }

                return _receita;
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto(); return; }

                _receita = value;
                _idReceita = _receita.Id;
                this.AlterarEstadoObjeto();
            }
        }

        public Int32 IdTitulo
        {
            get { return _idTitulo; }
            set { _idTitulo = value; this.AlterarEstadoObjeto(); }
        }

        public Prion.Generic.Models.Titulo Titulo
        {
            get
            {
                if (_titulo == null)
                {
                    _titulo = new Prion.Generic.Models.Titulo();
                    _titulo.Id = _idTitulo;
                }

                return _titulo;
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto(); return; }

                _titulo = value;
                _idTitulo = _titulo.Id;
                this.AlterarEstadoObjeto();
            }
        }
    }
}