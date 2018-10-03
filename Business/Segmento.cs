using System;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.Business
{
    public class Segmento : Base
    {
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public Segmento()
        {
        }

        /// <summary>
        /// Carrega um ou mais registros de Segmento
        /// </summary>
        /// <param name="ids">id do Segmento</param>
        /// <returns></returns>
        public GenericModels.Lista Carregar(params Int32[] ids)
        {
            Repository.Segmento repSegmento = new Repository.Segmento(ref this.Conexao);
            return repSegmento.Buscar(ids);
        }

        /// <summary>
        /// Exclui um ou mais registros de Segmento
        /// </summary>
        /// <param name="id">id do Segmento</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] ids)
        {
            try
            {
                Repository.Segmento repSegmento = new Repository.Segmento(ref this.Conexao);
                repSegmento.Excluir(ids);
            }
            catch (Exception e)
            {
                return (new GenericHelpers.Retorno(e.Message, false));
            }

            return (new GenericHelpers.Retorno());
        }

        /// <summary>
        /// Obtém a lista de Segmentos
        /// </summary>
        /// <returns>Lista de Segmentos</returns>
        public Prion.Generic.Models.Lista Lista(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Segmento repSegmento = new Repository.Segmento(ref this.Conexao);
            return repSegmento.Buscar(parametro);
        }

        /// <summary>
        /// Obtém um DataTable de Segmentos que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaSegmentos(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Segmento repSegmento = new Repository.Segmento(ref this.Conexao);
            repSegmento.Entidades.Adicionar(parametro.Entidades);

            return repSegmento.DataTable(parametro);
        }

        /// <summary>
        /// Salva um objeto de Segmento no banco de dados
        /// </summary>
        /// <param name="segmento">objeto do tipo Models.Segmento</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(Models.Segmento segmento)
        {
            this.Validar(segmento);

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            segmento.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

            // mudar o estado do objeto para ALTERADO apenas se ID <> 0
            if (segmento.Id != 0) { segmento.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }

            try
            {
                Repository.Segmento rpSegmento = new Repository.Segmento(ref this.Conexao);

                retorno = rpSegmento.Salvar(segmento);
                rpSegmento = null;

                retorno.Mensagem = (segmento.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Segmento inserido com sucesso." : "Segmento atualizado com sucesso";
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
        /// Valida os atributos obrigatórios de uma Model.Segmento
        /// </summary>
        /// <param name="segmento">Objeto do tipo Model.Segmento</param>
        private void Validar(Models.Segmento segmento)
        {
            if (segmento == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }

            if (segmento.IdSituacao == 0)
            {
                throw new Prion.Tools.PrionException("A situação tem que ser informada.");
            }

            if (segmento.Nome.Trim() == "")
            {
                throw new Prion.Tools.PrionException("O nome do Segmento não pode ser vazio.");
            }

            if ((segmento.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (segmento.Id == 0))
            {
                throw new Prion.Tools.PrionException("O id do Segmento não pode ser vazio.");
            }
        }
    }
}