using System;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.Business
{
    public class MovimentacaoFinanceira:Base
    {
          /// <summary>
        /// Construtor da Classe
        /// </summary>
        public MovimentacaoFinanceira()
        { 
        }

        /// <summary>
        /// Carrega um ou mais registros do MovimentacaoFinanceira
        /// </summary>
        /// <param name="ids">id do Cheque Estado</param>
        /// <returns></returns>
        public GenericModels.Lista Carregar(params Int32[] ids)
        {
            Repository.MovimentacaoFinanceira repMovimentacaoFinanceira = new Repository.MovimentacaoFinanceira(ref this.Conexao);
            return repMovimentacaoFinanceira.Buscar(ids);
        }

        /// <summary>
        /// Exclui um ou mais registros de MovimentacaoFinanceira
        /// </summary>
        /// <param name="ids">id de MovimentacaoFinanceira</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] ids)
        {
            try
            {
                Repository.MovimentacaoFinanceira repMovimentacaoFinanceira = new Repository.MovimentacaoFinanceira(ref this.Conexao);
                repMovimentacaoFinanceira.Excluir(ids);
            }
            catch (Exception e)
            {
                return (new GenericHelpers.Retorno(e.Message, false));
            }

            return (new GenericHelpers.Retorno());
        }

        /// <summary>
        /// Obtém um DataTable de MovimentacaoFinanceira que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaMovimentacoesFinanceira(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.MovimentacaoFinanceira repMovimentacaoFinanceira = new Repository.MovimentacaoFinanceira(ref this.Conexao);
            repMovimentacaoFinanceira.Entidades.Adicionar(parametro.Entidades);

            return repMovimentacaoFinanceira.DataTable(parametro);
        }

        /// <summary>
        /// Salva um objeto de MovimentacaoFinanceira no Banco de Dados
        /// </summary>
        /// <param name="movimentacaoFinanceira">objeto do tipo Models.MovimentacaoFinanceira</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(Models.MovimentacaoFinanceira movimentacaoFinanceira)
        {
            this.Validar(movimentacaoFinanceira);
            
            this.Conexao.UsarTransacao = true;

            GenericHelpers.Retorno retorno = null;
            movimentacaoFinanceira.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;
            Repository.MovimentacaoFinanceira repMovimentacaoFinanceira;
            Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this.Conexao);
            GenericModels.Situacao situacaoConcluida = repSituacao.BuscarPelaSituacao("Genérico", "Concluído");

            //mudar o estado do objeto para ALTERADO somente se id<>0
            if (movimentacaoFinanceira.Id != 0) { movimentacaoFinanceira.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }

            try
            {
                repMovimentacaoFinanceira = new Repository.MovimentacaoFinanceira(ref this.Conexao);
                retorno = repMovimentacaoFinanceira.Salvar(movimentacaoFinanceira);


                foreach (GenericModels.Titulo titulo in movimentacaoFinanceira.Titulos)
                {
                    // se o titulo estiver marcado como excluido, não cria o relacionamento com a movimentação financeira
                    if (titulo.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Excluido)
                    {
                        continue;
                    }

                    //Cria o relacionamento entre um Título e uma movimentação Financeira
                    repMovimentacaoFinanceira.CriarRelacionamento(retorno.UltimoId, titulo.Id,situacaoConcluida.Id);
                }


                repMovimentacaoFinanceira = null;

                retorno.Mensagem = (movimentacaoFinanceira.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Movimentações Financeira inserida com sucesso." : "Movimentações Financeira atualizada com sucesso.";

                this.Conexao.Commit();
            }
            catch (Exception e)
            {
                this.Conexao.Rollback();
                retorno = new GenericHelpers.Retorno(e.Message, false);
            }
            finally
            {
                repMovimentacaoFinanceira = null;
            }

            return retorno;
        }

        /// <summary>
        /// Valida os atributos obrigatórios de uma Models.MovimentacaoFinanceira
        /// </summary>
        /// <param name="ChequeEstado">Objeto do tipo Models.MovimentacaoFinanceira</param>
        private void Validar(Models.MovimentacaoFinanceira movimentacaoFinanceira)
        {
            if (movimentacaoFinanceira == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }

            if (movimentacaoFinanceira.Valor == 0)
            {
                throw new Prion.Tools.PrionException("O valor da Movimentação Financeira não pode ser vazio.");
            }
                    
            if ((movimentacaoFinanceira.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (movimentacaoFinanceira.Id == 0))
            {
                throw new Prion.Tools.PrionException("O id da Movimentação Financeira  não pode ser vazio.");
            }
        }
    }
}