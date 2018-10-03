using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.ViewModels.UsuarioGrupo
{
    public class vmDados
    {
        private GenericModels.UsuarioGrupo _usuarioGrupo;

        public GenericModels.UsuarioGrupo UsuarioGrupo
        {
            get { if (_usuarioGrupo == null) { return new GenericModels.UsuarioGrupo(); } return _usuarioGrupo; } 
            set { _usuarioGrupo = value; }
        }

        public List<GenericModels.Situacao> Situacoes { get; set; }
    }
}