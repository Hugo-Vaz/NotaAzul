using System;

namespace NotaAzul.Models
{
    public class Observacao : Prion.Generic.Models.Base
    {
        private Int32 _id;
        private Int32 _idUsuario;
        private Usuario _usuario;
        private Int32 _idSituacao;
        private Prion.Generic.Models.Situacao _situacao;
        private String _descricao;

        public Int32 Id
        {
            get { return _id; }
            set { _id = value; this.AlterarEstadoObjeto(); }
        }

        public Int32 IdUsuario
        {
            get { return _idUsuario; }
            set { _idUsuario = value; this.AlterarEstadoObjeto(); }
        }

        public Usuario Usuario
        {
            get
            {
                if (_usuario == null)
                {
                    _usuario = new Usuario();
                    _usuario.Id = _idUsuario;
                }

                return _usuario;
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto(); return; }

                _usuario = value;
                _idUsuario = _usuario.Id;
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

        public String Descricao
        {
            get { return _descricao; }
            set { _descricao = value; this.AlterarEstadoObjeto(); }
        }
    }
}