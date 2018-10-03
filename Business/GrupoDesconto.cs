using System;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.Business
{
    public class GrupoDesconto : Base
    {

        /// <summary>
        /// Construtor da Classe
        /// </summary>
        public GrupoDesconto()
        {

        }

        /// <summary>
        /// Carrega um ou mais registros de GrupoDesconto
        /// </summary>
        /// <param name="ids">id de GrupoDesconto</param>
        /// <returns></returns>
        public GenericModels.Lista Carregar(Prion.Tools.Request.ParametrosRequest parametro, params Int32[] ids)
        {
            Repository.GrupoDesconto repGrupoDesconto = new Repository.GrupoDesconto(ref this.Conexao);
            repGrupoDesconto.Entidades.Adicionar(parametro.Entidades);

            return repGrupoDesconto.Buscar(ids);
        }

         /// <summary>
        /// Exclui um ou mais registros de GrupoDesconto
        /// </summary>
        /// <param name="ids">id de GrupoDesconto</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] ids)
        {
            try
            {
                Repository.GrupoDesconto repGrupoDesconto = new Repository.GrupoDesconto(ref this.Conexao);
                repGrupoDesconto.Excluir(ids);
            }
            catch (Exception e)
            {
                return (new GenericHelpers.Retorno(e.Message, false));
            }

            return (new GenericHelpers.Retorno());
        }


        /// <summary>
        /// Obtém um DataTable de GrupoDesconto que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaGrupoDesconto(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.GrupoDesconto repGrupoDesconto = new Repository.GrupoDesconto(ref this.Conexao);
            repGrupoDesconto.Entidades.Adicionar(parametro.Entidades);

            return repGrupoDesconto.DataTable(parametro);
        }


        /// <summary>
        /// Salva um objeto de GrupoDesconto no Banco de Dados
        /// </summary>
        /// <param name="grupoDesconto">objeto do tipo Models.GrupoDesconto</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(Models.GrupoDesconto grupoDesconto)
        {
            this.Validar(grupoDesconto);

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            grupoDesconto.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

            //mudar o estado do objeto para ALTERADO somente se id<>0
            if (grupoDesconto.Id != 0) { grupoDesconto.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }

            try
            {
                Repository.GrupoDesconto repGrupoDesconto = new Repository.GrupoDesconto(ref this.Conexao);
                retorno = repGrupoDesconto.Salvar(grupoDesconto);

                // verifica se houve algum erro ao salvar o GrupoDesconto
                if (!retorno.Sucesso)
                {
                    this.Conexao.Rollback();
                    return retorno;
                }

                repGrupoDesconto = null;


                retorno.Mensagem = (grupoDesconto.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Grupo de Desconto inserido com sucesso." : "Grupo de Desconto atualizado com sucesso.";
                this.Conexao.Commit();
            }
            catch (Exception e)
            {
                this.Conexao.Rollback();
                retorno = new GenericHelpers.Retorno(e.Message, false);
            }

            return retorno;
        }


        /// <summary>
        /// Valida os atributos obrigatórios de uma Model.GrupoDesconto
        /// </summary>
        /// <param name="grupoDesconto">Objeto do tipo Model.GrupoDesconto</param>
        private void Validar(Models.GrupoDesconto grupoDesconto)
        {
            if (grupoDesconto == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }

            if (grupoDesconto.IdSituacao == 0)
            {
                throw new Prion.Tools.PrionException("A Situação tem que se informada.");
            }

            if (grupoDesconto.Nome.Trim() == "")
            {
                throw new Prion.Tools.PrionException("O nome do Grupo de Desconto não pode ser vazio.");
            }

            if ((grupoDesconto.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (grupoDesconto.Id == 0))
            {
                throw new Prion.Tools.PrionException("O id do Grupo de Desconto não pode ser vazio.");
            }
        }

    }
}
    