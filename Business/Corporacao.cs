using System;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;
using GenericRepository = Prion.Generic.Repository;

namespace NotaAzul.Business
{
    public class Corporacao : Base
    {
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public Corporacao()
        {
        }

        /// <summary>
        /// Carrega um ou mais registros de Corporacao
        /// </summary>
        /// <param name="ids">id da Corporacao</param>
        /// <returns></returns>
        public GenericModels.Corporacao Carregar(Int32 id)
        {
            GenericRepository.Corporacao repCorporacao = new GenericRepository.Corporacao(ref this.Conexao);
            return repCorporacao.BuscarPeloId(id);
        }


        /// <summary>
        /// Salva um objeto de Corporacao no banco de dados
        /// </summary>
        /// <param name="corporacao">objeto do tipo Models.Corporacao</param>
        /// <returns>true/false indicando se salvou com sucesso</returns>
        public GenericHelpers.Retorno Salvar(GenericModels.Corporacao corporacao)
        {
            this.Validar(corporacao);

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            // mudar o estado do objeto para ALTERADO apenas se ID <> 0
            if (corporacao.Id != 0) { corporacao.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }

            try
            {
                GenericRepository.Corporacao rpCorporacao = new GenericRepository.Corporacao(ref this.Conexao);

                retorno = rpCorporacao.Salvar(corporacao);
                rpCorporacao = null;

                retorno.Mensagem = "Corporação atualizada com sucesso";
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
        /// Valida os atributos obrigatórios de uma Model.Corporacao
        /// </summary>
        /// <param name="corporacao">Objeto do tipo Model.Corporacao</param>
        private void Validar(GenericModels.Corporacao corporacao)
        {
            if (corporacao == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }

            if (corporacao.Nome.Trim() == "")
            {
                throw new Prion.Tools.PrionException("O nome da Corporação não pode ser vazio.");
            }

            if ((corporacao.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (corporacao.Id == 0))
            {
                throw new Prion.Tools.PrionException("O id da Corporação não pode ser vazio.");
            }
        }
    }
}