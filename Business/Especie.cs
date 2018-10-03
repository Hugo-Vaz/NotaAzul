using System;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;
using GenericRepository = Prion.Generic.Repository;


namespace NotaAzul.Business
{
    public class Especie:Base
    {
         /// <summary>
        /// Construtor da Classe
        /// </summary>
        public Especie()
        { 
        }

        /// <summary>
        /// Carrega um ou mais registros de Especie
        /// </summary>
        /// <param name="ids">id da Especie</param>
        /// <returns></returns>
        public GenericModels.Lista Carregar(params Int32[] ids)
        {
            GenericRepository.Especie repEspecie = new GenericRepository.Especie(ref this.Conexao);
            return repEspecie.Buscar(ids);
        }

        /// <summary>
        /// Exclui um ou mais registros do Especie
        /// </summary>
        /// <param name="ids">id da Especie</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] ids)
        {
            try
            {
                GenericRepository.Especie repEspecie = new GenericRepository.Especie(ref this.Conexao);
                repEspecie.Excluir(ids);
            }
            catch (Exception e)
            {
                return (new GenericHelpers.Retorno(e.Message, false));
            }

            return (new GenericHelpers.Retorno());
        }

        /// <summary>
        /// Obtém um DataTable das Espécies que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaEspecies(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            GenericRepository.Especie repEspecie = new GenericRepository.Especie(ref this.Conexao);
            repEspecie.Entidades.Adicionar(parametro.Entidades);

            return repEspecie.DataTable(parametro);
        }

        /// <summary>
        /// Salva um objeto de Especie no Banco de Dados
        /// </summary>
        /// <param name="especie">objeto do tipo Models.Especie</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(GenericModels.Especie especie)
        {
            this.Validar(especie);

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            especie.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

            //mudar o estado do objeto para ALTERADO somente se id<>0
            if (especie.Id != 0) { especie.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }
            {
                try
                {
                    GenericRepository.Especie repEspecie = new GenericRepository.Especie(ref this.Conexao);

                    retorno = repEspecie.Salvar(especie);
                    repEspecie = null;

                    retorno.Mensagem = (especie.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Especie inserida com sucesso." : "Especie atualizada com sucesso.";
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
        /// Valida os atributos obrigatórios de uma Models.Especie
        /// </summary>
        /// <param name="especie">Objeto do tipo Models.Especie</param>
        private void Validar(GenericModels.Especie especie)
        {
            if (especie == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }

            if (especie.Valor == 0)
            {
                throw new Prion.Tools.PrionException("O valor tem que ser informada.");
            }
            

            if ((especie.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (especie.Id == 0))
            {
                throw new Prion.Tools.PrionException("O id da Especie não pode ser vazio.");
            }
        }
    }
}