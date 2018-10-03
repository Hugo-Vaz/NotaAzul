using System;
using System.Collections.Generic;
using Prion.Tools;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;
using GenericRepository = Prion.Generic.Repository;

namespace NotaAzul.Business
{
    public class Usuario : Base
    {
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public Usuario()
        {
        }

        /// <summary>
        /// Carrega um registro de usuário através do seu ID
        /// </summary>
        /// <param name="id">id do Usuário</param>
        /// <returns></returns>
        public GenericModels.Usuario Carregar(Int32 id)
        {
            try
            {
                GenericRepository.Usuario repUsuario = new GenericRepository.Usuario(ref this.Conexao);
                repUsuario.Entidades.Adicionar(Helpers.Entidade.UsuarioGrupo.ToString());

                GenericModels.Usuario usuarioGrupo = (GenericModels.Usuario)repUsuario.BuscarPeloId(id).Get(0);

                return usuarioGrupo;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Exclui um ou mais registros de Usuário
        /// </summary>
        /// <param name="id">um ou mais id de Usuário</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] id)
        {
            try
            {
                GenericRepository.Usuario repUsuario = new GenericRepository.Usuario(ref this.Conexao);
                Repository.Usuario repUsuarios = new Repository.Usuario(ref this.Conexao);
                
                repUsuarios.ExcluirRelacionamento(id);
                repUsuario.Excluir(id);

                return (new GenericHelpers.Retorno());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Obtém a lista de usuários
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista Lista(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            try
            {
                Repository.Usuario repUsuario = new Repository.Usuario(ref this.Conexao);
                return repUsuario.Buscar(parametro);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Obtém um DataTable de Usuários que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaUsuarios(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            try
            {
                GenericRepository.Usuario repUsuario = new GenericRepository.Usuario(ref this.Conexao);
                repUsuario.Entidades.Adicionar(parametro.Entidades);

                return repUsuario.Lista(parametro);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary> 
        /// Obtém uma lista com os menus do usuário desejado 
        /// </summary>
        /// <param name="idUsuario">id do usuário logado</param>
        public List<GenericModels.Menu> ListaMenu(Int32 idUsuario) 
        {
            try
            {//instanciando classe business de menu
                Business.Menu biMenu = new Business.Menu();

                List<GenericModels.Menu> menu = BuscarMenu(idUsuario);

                //caso usuário não possua nenhum menu, retornar um objeto novo, sem itens
                if (menu == null) return new List<GenericModels.Menu>();

                //retorna uma lista de menus já formatada
                return biMenu.Formatar(menu);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Busca um usuário através do seu login
        /// </summary>
        /// <param name="login">login do usuário a iniciar o sistema</param>
        /// <param name="senha">senha do usuário</param>
        /// <returns>Objeto do tipo Models.Usuario</returns>
        public Models.Usuario Logar(String login, String senha)
        {
            if (login.Trim() == "")
            {
                return null;
            }

            try
            {
                //instancia objetos de conexão
                Repository.Usuario rpUsuarios = new Repository.Usuario(ref this.Conexao);
                GenericModels.Usuario usuario = null;


                // define todos os módulos que deverão ser carregados no método 'BuscarPeloLogin'
                Prion.Tools.Entidade entidade = new Prion.Tools.Entidade(Helpers.Entidade.UsuarioGrupo.ToString(), Helpers.Entidade.Situacao.ToString());
                rpUsuarios.Entidades = entidade;

                usuario = rpUsuarios.BuscarPeloLogin(Helpers.Settings.IdCorporacao(), login, senha);

                // necessário dar NEW em Models.Usuario, pois Usuario é um objeto que esta sendo herdado e reimplementado
                Models.Usuario usuarioLogado = new Models.Usuario(usuario);

                return usuarioLogado;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Carrega as permissões através do id do usuário
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns>lista de permissões</returns>
        public List<GenericModels.Permissao> Permissoes(Int32 idUsuario)
        {
            try
            {
                Prion.Tools.Request.ParametrosRequest parametros = new Request.ParametrosRequest();
                parametros.Paginar = false;
                parametros.Filtro.Add(new Request.Filtro("Usuario.Id", "=", idUsuario.ToString()));

                Repository.Usuario repUsuario = new Repository.Usuario(ref this.Conexao);
                return repUsuario.BuscarPermissoes(parametros);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Salva um objeto de usuário no banco de dados
        /// </summary>
        /// <param name="usuario">objeto do tipo Models.Usuario</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(Models.Usuario usuario)
        {
            this.Validar(usuario);

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            usuario.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

            // mudar o estado do objeto para ALTERADO apenas se ID <> 0
            if (usuario.Id != 0) { usuario.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }
          
            try
            {
                Repository.Usuario rpUsuarios = new Repository.Usuario(ref this.Conexao);

                retorno = rpUsuarios.Salvar(usuario);

                // se houve sucesso E for um usuário já existente...
                if ((retorno.Sucesso) && (usuario.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado))
                {
                    // ... exclui o grupo de usuário ao qual este usuário pertence e depois insere novamente
                    rpUsuarios.ExcluirGrupoUsuario(usuario.Id);                   
                }

                // insere a lista de grupos de usuários
                for (Int32 i = 0; i < usuario.Grupos.Count; i++)
                {
                    rpUsuarios.InserirGrupoUsuario(usuario.Id, usuario.Grupos[i].Id);
                }

                if (usuario.IdFuncionario != 0 && usuario.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo)
                {
                    rpUsuarios.CriarRelacionamento(usuario.IdFuncionario, retorno.UltimoId);
                }
                
                rpUsuarios = null;

                retorno.Mensagem = (usuario.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Usuário inserido com sucesso." : "Usuário atualizado com sucesso";
                this.Conexao.Commit();
                return retorno;
            }
            catch (Exception e)
            {
                this.Conexao.Rollback();
                throw e;
            }
        }

        /// <summary> Obtém uma lista de menus relacionados ao id de usuário passado </summary>
        /// <param name="idUsuario"></param>
        /// <returns> Uma lista contendo todos os menus de determinado usuário </returns>
        private List<GenericModels.Menu> BuscarMenu(Int32 idUsuario)
        {
            try
            {//instancia objetos de conexão
                Repository.Usuario rpUsuarios = new Repository.Usuario(ref this.Conexao);

                //retorna lista de menu
                return rpUsuarios.BuscarMenu(idUsuario);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void Validar(Models.Usuario usuario)
        {
            if (usuario == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }

            if (usuario.Login.ToString().Trim() == "")
            {
                throw new Prion.Tools.PrionException("Login do usuário não pode ser vazio.");
            }

            if (usuario.Nome.ToString().Trim() == "")
            {
                throw new Prion.Tools.PrionException("Nome do usuário não pode ser vazio.");
            }

            if (usuario.Senha.ToString().Trim() == "")
            {
                throw new Prion.Tools.PrionException("Senha do usuário não pode ser vazio.");
            }

            if ((usuario.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (usuario.Id == 0))
            {
                throw new Prion.Tools.PrionException("O id do usuário não pode ser vazio.");
            }
        }
    }
}