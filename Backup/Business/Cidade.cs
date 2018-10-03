using System;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;
using GenericRepository = Prion.Generic.Repository;

namespace NotaAzul.Business
{
    public class Cidade : Base
    {
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public Cidade()
        {
        }

        /// <summary>
        /// Carrega um ou mais registros de Cidade
        /// </summary>
        /// <param name="ids">id da Cidade</param>
        /// <returns></returns>
        public GenericModels.Lista Carregar(params Int32[] ids)
        {
            GenericRepository.Cidade repCidade = new GenericRepository.Cidade(ref this.Conexao);
            return repCidade.BuscarPeloId(ids);
        }

        /// <summary>
        /// Exclui um ou mais registros de Cidade
        /// </summary>
        /// <param name="id">id da Cidade</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] ids)
        {
            try
            {
                GenericRepository.Cidade repCidade = new GenericRepository.Cidade(ref this.Conexao);
                repCidade.Excluir(ids);
            }
            catch (Exception e)
            {
                return (new GenericHelpers.Retorno(e.Message, false));
            }

            return (new GenericHelpers.Retorno());
        }

        /// <summary>
        /// Obtém a lista de Cidades
        /// </summary>
        /// <returns>Lista de Cidades</returns>
        public Prion.Generic.Models.Lista Lista(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Prion.Generic.Repository.Cidade repCidade = new Prion.Generic.Repository.Cidade(ref this.Conexao);
            return repCidade.Buscar(parametro);
        }

        /// <summary>
        /// Obtém um DataTable de Cidades que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaCidades(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            GenericRepository.Cidade repCidade = new GenericRepository.Cidade(ref this.Conexao);
            repCidade.Entidades.Adicionar(parametro.Entidades);

            return repCidade.Lista(parametro);
        }

        /// <summary>
        /// Salva um objeto de Cidade no banco de dados
        /// </summary>
        /// <param name="cidade">objeto do tipo Models.Cidade</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(GenericModels.Cidade cidade)
        {
            this.Validar(cidade);

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            cidade.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

            // mudar o estado do objeto para ALTERADO apenas se ID <> 0
            if (cidade.Id != 0) { cidade.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }

            try
            {
                GenericRepository.Cidade rpCidade = new GenericRepository.Cidade(ref this.Conexao);

                retorno = rpCidade.Salvar(cidade);
                rpCidade = null;

                retorno.Mensagem = (cidade.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Cidade inserida com sucesso." : "Cidade atualizada com sucesso";
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
        /// Valida os atributos obrigatórios de uma Model.Cidade
        /// </summary>
        /// <param name="cidade">Objeto do tipo Model.Cidade</param>
        private void Validar(GenericModels.Cidade cidade)
        {
            if (cidade == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }

            if (cidade.IdEstado == 0)
            {
                throw new Prion.Tools.PrionException("É necessário informar o estado.");
            }

            if (cidade.Nome.Trim() == "")
            {
                throw new Prion.Tools.PrionException("O nome da Cidade não pode ser vazio.");
            }

            if ((cidade.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (cidade.Id == 0))
            {
                throw new Prion.Tools.PrionException("O id do Cidade não pode ser vazio.");
            }
        }
    }
}