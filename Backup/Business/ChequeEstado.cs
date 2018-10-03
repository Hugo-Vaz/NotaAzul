using System;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.Business
{
    public class ChequeEstado : Base
    {
          /// <summary>
        /// Construtor da Classe
        /// </summary>
        public ChequeEstado()
        { 
        }

        /// <summary>
        /// Carrega um ou mais registros do Estado do Cheque
        /// </summary>
        /// <param name="ids">id do Cheque Estado</param>
        /// <returns></returns>
        public GenericModels.Lista Carregar(params Int32[] ids)
        {
            Prion.Generic.Repository.ChequeEstado repChequeEstado = new Prion.Generic.Repository.ChequeEstado(ref this.Conexao);
            return repChequeEstado.BuscarPeloId(ids);
        }

        /// <summary>
        /// Exclui um ou mais registros do Cheque Estado
        /// </summary>
        /// <param name="ids">id do Estado do Cheque</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] ids)
        {
            try
            {
                Prion.Generic.Repository.ChequeEstado repChequeEstado = new Prion.Generic.Repository.ChequeEstado(ref this.Conexao);
                repChequeEstado.Excluir(ids);
            }
            catch (Exception e)
            {
                return (new GenericHelpers.Retorno(e.Message, false));
            }

            return (new GenericHelpers.Retorno());
        }

        /// <summary>
        /// Obtém um DataTable dos Estados dos Cheques que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaChequesEstado(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Prion.Generic.Repository.ChequeEstado repChequeEstado = new Prion.Generic.Repository.ChequeEstado(ref this.Conexao);
            repChequeEstado.Entidades.Adicionar(parametro.Entidades);

            return repChequeEstado.Lista(parametro);
        }

        /// <summary>
        /// Salva um objeto de ChequeEstado no Banco de Dados
        /// </summary>
        /// <param name="chequeEstado">objeto do tipo Prion.Generic.Models.ChequeEstado</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(Prion.Generic.Models.ChequeEstado chequeEstado)
        {
            this.Validar(chequeEstado);

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            chequeEstado.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

            //mudar o estado do objeto para ALTERADO somente se id<>0
            if (chequeEstado.Id != 0) { chequeEstado.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }
            {
                try
                {
                    Prion.Generic.Repository.ChequeEstado repChequeEstado = new Prion.Generic.Repository.ChequeEstado(ref this.Conexao);

                    retorno = repChequeEstado.Salvar(chequeEstado);
                    repChequeEstado = null;

                    retorno.Mensagem = (chequeEstado.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Estado do Cheque inserido com sucesso." : "Estado do Cheque atualizado com sucesso.";
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
        /// Valida os atributos obrigatórios de uma Prion.Generic.Models.ChequeEstado
        /// </summary>
        /// <param name="ChequeEstado">Objeto do tipo Prion.Generic.Models.ChequeEstado</param>
        private void Validar(Prion.Generic.Models.ChequeEstado chequeEstado)
        {
            if (chequeEstado == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }

            if (chequeEstado.IdSituacao == 0)
            {
                throw new Prion.Tools.PrionException("A Situação tem que se informada.");
            }

            if (chequeEstado.Nome.Trim() == "")
            {
                throw new Prion.Tools.PrionException("O nome do Estado do Cheque não pode ser vazio.");
            }

            if ((chequeEstado.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (chequeEstado.Id == 0))
            {
                throw new Prion.Tools.PrionException("O id do Estado do Cheque não pode ser vazio.");
            }
        }
    }
}