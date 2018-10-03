using System;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.Business
{
    public class Cheque : Base
    {
        /// <summary>
        /// Construtor da Classe
        /// </summary>
        public Cheque()
        {
        }

        /// <summary>
        /// Carrega um ou mais registros de Cheque
        /// </summary>
        /// <param name="ids">id de Cheque</param>
        /// <returns></returns>
        public GenericModels.Lista Carregar(Prion.Tools.Request.ParametrosRequest parametro, params Int32[] ids)
        {
            //GenericRepository.Cheque repCheque = new GenericRepository.Cheque(ref this.Conexao);
            Repository.Cheque repCheque = new Repository.Cheque(ref this.Conexao);
            repCheque.Entidades.Adicionar(parametro.Entidades);

            return repCheque.Buscar(ids);
        }

         /// <summary>
        /// Exclui um ou mais registros de Cheque
        /// </summary>
        /// <param name="ids">id de Cheque</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] ids)
        {
            try
            {
                //GenericRepository.Cheque repCheque = new GenericRepository.Cheque(ref this.Conexao);
                Repository.Cheque repCheque = new Repository.Cheque(ref this.Conexao);
                repCheque.Excluir(ids);
            }
            catch (Exception e)
            {
                return (new GenericHelpers.Retorno(e.Message, false));
            }

            return (new GenericHelpers.Retorno());
        }

        /// <summary>
        /// Obtém um DataTable de Cheques que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaCheques(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
           //GenericRepository.Cheque repCheque = new GenericRepository.Cheque(ref this.Conexao);
            Repository.Cheque repCheque = new Repository.Cheque(ref this.Conexao);
            repCheque.Entidades.Adicionar(parametro.Entidades);

            return repCheque.DataTable(parametro);
        }

        /// <summary>
        /// Salva um objeto de Cheque no Banco de Dados
        /// </summary>
        /// <param name="cheque">objeto do tipo Prion.Generic.Models.Cheque</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(GenericModels.Cheque cheque)
        {
            this.Validar(cheque);

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            cheque.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

            //mudar o estado do objeto para ALTERADO somente se id<>0
            if (cheque.Id != 0) { cheque.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }
            {
                try
                {
                    //GenericRepository.Cheque repCheque = new GenericRepository.Cheque(ref this.Conexao);
                    Repository.Cheque repCheque = new Repository.Cheque(ref this.Conexao);
                    retorno = repCheque.Salvar(cheque);
                    repCheque = null;

                    retorno.Mensagem = (cheque.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Cheque inserido com sucesso." : "Cheque atualizado com sucesso.";
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

        /// <summary>
        /// Valida os atributos obrigatórios de uma Prion.Generic.Model.Cheque
        /// </summary>
        /// <param name="cheque">Objeto do tipo Prion.Generic.Model.Cheque</param>
        private void Validar(GenericModels.Cheque cheque)
        {
            if (cheque == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }

            if (cheque.Banco.Trim() == "")
            {
                throw new Prion.Tools.PrionException("O banco do cheque precisa ser informado.");
            }

            if (cheque.Agencia.Trim() == "")
            {
                throw new Prion.Tools.PrionException("A agência do cheque precisa ser informada.");
            }

            if (cheque.ContaCorrente.Trim() == "")
            {
                throw new Prion.Tools.PrionException("A conta corrente do cheque precisa ser informada.");
            }

            if (cheque.Numero.Trim() == "")
            {
                throw new Prion.Tools.PrionException("O número do cheque precisa ser informado.");
            }

            if (cheque.Valor == 0)
            {
                throw new Prion.Tools.PrionException("O valor do cheque precisa ser informado.");
            }

            if ((cheque.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (cheque.Id == 0))
            {
                throw new Prion.Tools.PrionException("O id do cheque precisa ser informado.");
            }
        }
    }
}
