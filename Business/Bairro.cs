using System;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;
using GenericRepository = Prion.Generic.Repository;

namespace NotaAzul.Business
{
    public class Bairro : Base
    {
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public Bairro()
        {
        }

        /// <summary>
        /// Carrega um ou mais registros de Bairro
        /// </summary>
        /// <param name="ids">id do Bairro</param>
        /// <returns></returns>
        public GenericModels.Lista Carregar(params Int32[] ids)
        {
            GenericRepository.Bairro repBairro = new GenericRepository.Bairro(ref this.Conexao);
            return repBairro.BuscarPeloId(ids);
        }

        /// <summary>
        /// Exclui um ou mais registros de Bairro
        /// </summary>
        /// <param name="id">id do Bairro</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] ids)
        {
            try
            {
                GenericRepository.Bairro repBairro = new GenericRepository.Bairro(ref this.Conexao);
                repBairro.Excluir(ids);
            }
            catch (Exception e)
            {
                return (new GenericHelpers.Retorno(e.Message, false));
            }

            return (new GenericHelpers.Retorno());
        }

        /// <summary>
        /// Obtém a lista de Bairros
        /// </summary>
        /// <returns>Lista de Bairros</returns>
        public Prion.Generic.Models.Lista Lista(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Prion.Generic.Repository.Bairro repBairro = new Prion.Generic.Repository.Bairro(ref this.Conexao);
            return repBairro.Buscar(parametro);
        }

        /// <summary>
        /// Obtém um DataTable de Bairros que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaBairros(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            GenericRepository.Bairro repBairro = new GenericRepository.Bairro(ref this.Conexao);
            repBairro.Entidades.Adicionar(parametro.Entidades);

            return repBairro.Lista(parametro);
        }

        /// <summary>
        /// Salva um objeto de Bairro no banco de dados
        /// </summary>
        /// <param name="bairro">objeto do tipo Models.Bairro</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(GenericModels.Bairro bairro)
        {
            this.Validar(bairro);

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            bairro.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

            // mudar o estado do objeto para ALTERADO apenas se ID <> 0
            if (bairro.Id != 0) { bairro.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }

            try
            {
                GenericRepository.Bairro rpBairro = new GenericRepository.Bairro(ref this.Conexao);

                retorno = rpBairro.Salvar(bairro);
                rpBairro = null;

                retorno.Mensagem = (bairro.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Bairro inserido com sucesso." : "Bairro atualizado com sucesso";
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
        /// Valida os atributos obrigatórios de uma Model.Bairro
        /// </summary>
        /// <param name="bairro">Objeto do tipo Model.Bairro</param>
        private void Validar(GenericModels.Bairro bairro)
        {
            if (bairro == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }

            if (bairro.IdCidade == 0)
            {
                throw new Prion.Tools.PrionException("É necessário informar a cidade");
            }

            if (bairro.Nome.Trim() == "")
            {
                throw new Prion.Tools.PrionException("O nome do bairro não pode ser vazio.");
            }

            if ((bairro.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (bairro.Id == 0))
            {
                throw new Prion.Tools.PrionException("O id do bairro não pode ser vazio.");
            }
        }
    }
}