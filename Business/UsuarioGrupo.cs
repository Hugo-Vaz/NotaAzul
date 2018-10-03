using System;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;
using GenericRepository = Prion.Generic.Repository;

namespace NotaAzul.Business
{
    public class UsuarioGrupo : Base
    {
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public UsuarioGrupo()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id do UsuarioGrupo</param>
        /// <returns></returns>
        public GenericModels.UsuarioGrupo Carregar(Int32 id)
        {
            try
            {
                GenericRepository.UsuarioGrupo repUsuarioGrupo = new GenericRepository.UsuarioGrupo(ref this.Conexao);
                GenericModels.UsuarioGrupo usuarioGrupoGeneric = (GenericModels.UsuarioGrupo)repUsuarioGrupo.BuscarPeloId(id).Get(0);

                return usuarioGrupoGeneric;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Retorna um objeto do tipo Models.Lista contento um dataTable com todos os módulos ATIVOS e as suas permissões
        /// </summary>
        /// <param name="idUsuarioGrupo">id do Grupo de Usuário</param>
        /// <returns></returns>
        public Prion.Generic.Models.Lista CarregarPermissoesVisiveis(Int32 idUsuarioGrupo)
        {
            try
            {
                GenericRepository.Modulo repModulos = new GenericRepository.Modulo(ref this.Conexao);
                return repModulos.PermissoesVisiveis(idUsuarioGrupo);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Retorna um objeto do tipo Models.Lista contendo um datatable com todos os módulos definidos para este Grupo de Usuário
        /// </summary>
        /// <param name="idUsuarioGrupo">id do Grupo de Usuário</param>
        /// <returns>Objeto do tipo Models.Lista contento um datatable com todos os módulos definidos para o Grupo de Usuário</returns>
        public Prion.Generic.Models.Lista CarregarPermissoes(Int32 idUsuarioGrupo)
        {
            try
            {
                GenericRepository.UsuarioGrupo repUsuarioGrupo = new GenericRepository.UsuarioGrupo(ref this.Conexao);
                return repUsuarioGrupo.BuscarPermissoes(idUsuarioGrupo);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Exclui um ou mais registros de Grupos de Usuário
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] id)
        {
            try
            {
                GenericRepository.UsuarioGrupo repUsuarioGrupo = new GenericRepository.UsuarioGrupo(ref this.Conexao);
                repUsuarioGrupo.Excluir(id);
                return (new GenericHelpers.Retorno());
            }
            catch (Exception e)
            {
                return (new GenericHelpers.Retorno(e.Message, false));
            }
        }

        /// <summary>
        /// Obtém a lista de Grupos de Usuários
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista Lista(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            try
            {
                GenericRepository.UsuarioGrupo repUsuarioGrupo = new GenericRepository.UsuarioGrupo(ref this.Conexao);
                repUsuarioGrupo.Entidades.Adicionar(Helpers.Entidade.Situacao.ToString());

                return repUsuarioGrupo.Buscar(parametro);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Obtém um DataTable de Grupos de Usuários que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaUsuarioGrupo(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            try
            {
                GenericRepository.UsuarioGrupo repUsuarioGrupo = new GenericRepository.UsuarioGrupo(ref this.Conexao);
                repUsuarioGrupo.Entidades.Adicionar(parametro.Entidades);

                return repUsuarioGrupo.Lista(parametro);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Salva um objeto de UsuarioGrupo no banco de dados
        /// </summary>
        /// <param name="usuarioGrupo">objeto do tipo Models.UsuarioGrupo</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(GenericModels.UsuarioGrupo usuarioGrupo)
        {
            this.Validar(usuarioGrupo);

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            usuarioGrupo.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

            // mudar o estado do objeto para ALTERADO apenas se ID <> 0
            if (usuarioGrupo.Id != 0) { usuarioGrupo.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }

            try
            {
                GenericRepository.UsuarioGrupo rpUsuarioGrupo = new GenericRepository.UsuarioGrupo(ref this.Conexao);

                retorno = rpUsuarioGrupo.Salvar(usuarioGrupo);

                // se houve sucesso e o objeto for NOVO...
                if ((retorno.Sucesso) && (usuarioGrupo.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo))
                {
                    // ... insere para este Grupo de Usuário todas as permissões VISIVEIS
                    usuarioGrupo.Id = retorno.UltimoId;
                    rpUsuarioGrupo.InserirPermissoesVisiveis(usuarioGrupo.Id);
                }


                rpUsuarioGrupo = null;

                retorno.Mensagem = (usuarioGrupo.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Grupo de Usuário inserido com sucesso." : "Grupo de Usuário atualizado com sucesso";
                this.Conexao.Commit();
                return retorno;
            }
            catch (Exception e)
            {
                this.Conexao.Rollback();
                throw e;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idUsuarioGrupo"></param>
        /// <param name="listaPermissoes"></param>
        /// <param name="listaPermissoesIncluir"></param>
        /// <returns></returns>
        public GenericHelpers.Retorno SalvarPermissoes(Int32 idUsuarioGrupo, Int32[] listaPermissoes)
        {
            GenericRepository.UsuarioGrupo repUsuarioGrupo = new GenericRepository.UsuarioGrupo(ref this.Conexao);

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = new GenericHelpers.Retorno();

            try
            {
                repUsuarioGrupo.SalvarPermissoes(idUsuarioGrupo, listaPermissoes);

                retorno.Sucesso = true;
                retorno.Mensagem = "Permissões atualizadas com sucesso";
                this.Conexao.Commit();
                return retorno;
            }
            catch (Exception e)
            {
                this.Conexao.Rollback();
                throw e;
            }
        }

        /// <summary>
        /// Valida os atributos obrigatórios de uma Model.UsuarioGrupo
        /// </summary>
        /// <param name="usuarioGrupo">Objeto do tipo Model.UsuarioGrupo</param>
        private void Validar(GenericModels.UsuarioGrupo usuarioGrupo)
        {
            if (usuarioGrupo == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }

            if (usuarioGrupo.IdSituacao == 0)
            {
                throw new Prion.Tools.PrionException("A situação do grupo não pode ser vazio.");
            }

            if (usuarioGrupo.Nome.Trim() == "")
            {
                throw new Prion.Tools.PrionException("O nome do grupo não pode ser vazio.");
            }

            if (usuarioGrupo.Descricao.Trim() == "")
            {
                throw new Prion.Tools.PrionException("A descrição do grupo não pode ser vazio.");
            }

            if ((usuarioGrupo.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (usuarioGrupo.Id == 0))
            {
                throw new Prion.Tools.PrionException("O id do grupo não pode ser vazio.");
            }
        }
    }
}