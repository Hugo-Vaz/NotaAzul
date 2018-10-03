using System;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.Business
{
    public class Material : Base
    {
          /// <summary>
        /// Construtor da Classe
        /// </summary>
        public Material()
        { 
        }

        /// <summary>
        /// Carrega um ou mais registros de Material
        /// </summary>
        /// <param name="ids">id de Material</param>
        /// <returns></returns>
        public GenericModels.Lista Carregar(params Int32[] ids)
        {
            Repository.Material repMaterial = new Repository.Material(ref this.Conexao);
            return repMaterial.Buscar(ids);
        }

        /// <summary>
        /// Exclui um ou mais registros do Material
        /// </summary>
        /// <param name="ids">id de Material</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] ids)
        {
            try
            {
                Repository.Material repMaterial = new Repository.Material(ref this.Conexao);
                repMaterial.Excluir(ids);
            }
            catch (Exception e)
            {
                return (new GenericHelpers.Retorno(e.Message, false));
            }

            return (new GenericHelpers.Retorno());
        }

        /// <summary>
        /// Obtém um DataTable de Material que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaMateriais(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Material repMaterial = new Repository.Material(ref this.Conexao);
            repMaterial.Entidades.Adicionar(parametro.Entidades);

            return repMaterial.DataTable(parametro);
        }

        /// <summary>
        /// Salva um objeto de Material no Banco de Dados
        /// </summary>
        /// <param name="Material">objeto do tipo Models.Material</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(Models.Material material)
        {
            this.Validar(material);

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            material.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

            //mudar o estado do objeto para ALTERADO somente se id<>0
            if (material.Id != 0) { material.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }
            {
                try
                {
                    Repository.Material repMaterial = new Repository.Material(ref this.Conexao);

                    retorno = repMaterial.Salvar(material);
                    repMaterial = null;

                    retorno.Mensagem = (material.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Material inserido com sucesso." : "Material atualizado com sucesso.";
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
        /// Valida os atributos obrigatórios de uma Models.Material
        /// </summary>
        /// <param name="material">Objeto do tipo Models.Material</param>
        private void Validar(Models.Material material)
        {
            if (material == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }

            if (material.IdSituacao == 0)
            {
                throw new Prion.Tools.PrionException("A Situação tem que se informada.");
            }

            if (material.Nome.Trim() == "")
            {
                throw new Prion.Tools.PrionException("O nome do Material não pode ser vazio.");
            }

            if ((material.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (material.Id == 0))
            {
                throw new Prion.Tools.PrionException("O id do Material não pode ser vazio.");
            }
        }
    }
}