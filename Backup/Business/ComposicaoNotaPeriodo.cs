using System;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.Business
{
    public class ComposicaoNotaPeriodo:Base
    {
         /// <summary>
        /// Construtor da Classe
        /// </summary>
        public ComposicaoNotaPeriodo()
        {
        }

        /// <summary>
        /// Carrega um ou mais registros de ComposicaoNota
        /// </summary>
        /// <param name="ids">id de ComposicaoNota</param>
        /// <returns></returns>
        public GenericModels.Lista Carregar(Prion.Tools.Request.ParametrosRequest parametro)
        {
            Repository.ComposicaoNotaPeriodo repComposicaoNota = new Repository.ComposicaoNotaPeriodo(ref this.Conexao);
            repComposicaoNota.Entidades.Adicionar(parametro.Entidades);

            return repComposicaoNota.Buscar(parametro);
        }

        /// <summary>
        /// Exclui um ou mais registros de ComposicaoNota
        /// </summary>
        /// <param name="ids">id de ComposicaoNota</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] ids)
        {
            try
            {
                Repository.ComposicaoNotaPeriodo repComposicaoNota = new Repository.ComposicaoNotaPeriodo(ref this.Conexao);
                repComposicaoNota.Excluir(ids);
            }
            catch (Exception e)
            {
                return (new GenericHelpers.Retorno(e.Message, false));
            }

            return (new GenericHelpers.Retorno());
        }

        /// <summary>
        /// Salva um objeto de ComposicaoNota no Banco de Dados
        /// </summary>
        /// <param name="composicaoNotaPeriodo">objeto do tipo Models.ComposicaoNotaPeriodo</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(Models.ComposicaoNotaPeriodo composicaoNotaPeriodo)
        {          

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            composicaoNotaPeriodo.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

            //mudar o estado do objeto para ALTERADO somente se id<>0
            if (composicaoNotaPeriodo.Id != 0) { composicaoNotaPeriodo.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }

            try
            {
                Repository.ComposicaoNotaPeriodo repComposicaoNota = new Repository.ComposicaoNotaPeriodo(ref this.Conexao);
                
                retorno = repComposicaoNota.Salvar(composicaoNotaPeriodo);

                // verifica se houve algum erro ao salvar o aluno
                if (!retorno.Sucesso)
                {
                    this.Conexao.Rollback();
                    return retorno;
                }


                repComposicaoNota = null;


                retorno.Mensagem = (composicaoNotaPeriodo.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Composição da Nota inserida com sucesso." : "Composição da Nota atualizada com sucesso.";
                this.Conexao.Commit();
            }
            catch (Exception e)
            {
                this.Conexao.Rollback();
                retorno = new GenericHelpers.Retorno(e.Message, false);
            }

            return retorno;
        }

    }
}