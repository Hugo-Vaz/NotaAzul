using System;

namespace NotaAzul.Models
{
    public class Usuario : Prion.Generic.Models.Usuario
    {
        public Usuario() { }

        private Prion.Generic.Models.Funcionario _funcionario;
        private Int32 _idFuncionario;

        public Usuario(Prion.Generic.Models.Usuario usuarioGenerico) 
        {
            this.Id = usuarioGenerico.Id;
            this.IdCorporacao = usuarioGenerico.IdCorporacao;
            this.IdImagem = usuarioGenerico.IdImagem;
            this.IdSituacao = usuarioGenerico.IdSituacao;
            this.Imagem = usuarioGenerico.Imagem;
            this.Login = usuarioGenerico.Login;
            this.Nome = usuarioGenerico.Nome;
            this.Senha = usuarioGenerico.Senha;
            this.Email = usuarioGenerico.Email;
            this.Perfil = usuarioGenerico.Perfil;
            this.Sexo = usuarioGenerico.Sexo;
            this.Situacao = usuarioGenerico.Situacao;
            this.Sobrenome = usuarioGenerico.Sobrenome;

            this.Grupos = usuarioGenerico.Grupos;

            this.DataCadastro = usuarioGenerico.DataCadastro;
            this.EstadoObjeto = usuarioGenerico.EstadoObjeto;
        }

        public Int32 IdFuncionario
        {
            get { return _idFuncionario; }
            set { _idFuncionario = value; this.AlterarEstadoObjeto(); }
        }

        public Prion.Generic.Models.Funcionario Funcionario
        {
            get
            {
                if (_funcionario == null)
                {
                    _funcionario = new Prion.Generic.Models.Funcionario();
                    _funcionario.Id = _idFuncionario;
                }

                return _funcionario;
            }
            set
            {
                if (value == null) { this.AlterarEstadoObjeto(); return; }

                _funcionario = value;
                _idFuncionario = _funcionario.Id;
                this.AlterarEstadoObjeto();
            }
        }
    }
}