using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotaAzul.Models
{
    public class ProfessorConfiguracao:Prion.Generic.Models.Base
    {
        private Int32 _id;
        private Int32 _idUsuario;
        private Models.Usuario _usuario;
        private String _tipoMedia;

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

        public Models.Usuario Usuario
        {
            get
            {
                if (_usuario == null)
                {
                    _usuario = new Models.Usuario();
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

        public String TipoMedia
        {
            get { return _tipoMedia; }
            set { _tipoMedia = value; this.AlterarEstadoObjeto(); }
        }
    }
}