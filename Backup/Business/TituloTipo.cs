using System;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;
using Prion.Tools;

namespace NotaAzul.Business
{
    public class TituloTipo:Base
    {
           /// <summary>
        /// Construtor da Classe
        /// </summary>
        public TituloTipo()
        { 
        }

        /// <summary>
        /// Carrega um ou mais registros de Tipos de Título
        /// </summary>
        /// <param name="ids">id do Tipo Título</param>
        /// <returns></returns>
        public GenericModels.Lista Carregar(params Int32[] ids)
        {
            Prion.Generic.Repository.TituloTipo repTituloTipo = new Prion.Generic.Repository.TituloTipo(ref this.Conexao);
            return repTituloTipo.BuscarPeloId(ids);
        }

        /// <summary>
        /// Exclui um ou mais registros do Tipos de Título
        /// </summary>
        /// <param name="ids">id de TituloTipo</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] ids)
        {
            try
            {
                Prion.Generic.Repository.TituloTipo repTituloTipo = new Prion.Generic.Repository.TituloTipo(ref this.Conexao);
                repTituloTipo.Excluir(ids);
            }
            catch (Exception e)
            {
                return (new GenericHelpers.Retorno(e.Message, false));
            }

            return (new GenericHelpers.Retorno());
        }

        /// <summary>
        /// Obtém um DataTable de TituloTipo que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaTiposTitulo(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Prion.Generic.Repository.TituloTipo repTituloTipo = new Prion.Generic.Repository.TituloTipo(ref this.Conexao);
            repTituloTipo.Entidades.Adicionar(parametro.Entidades);
            Prion.Tools.Request.Filtro f = new Request.Filtro("TituloTipo.Visivel", "LIKE", 1.ToString());
            parametro.Filtro.Add(f);
            return repTituloTipo.Lista(parametro);
        }

        /// <summary>
        /// Salva um objeto de TituloTipo no Banco de Dados
        /// </summary>
        /// <param name="tituloTipo">objeto do tipo Prion.Generic.Models.TituloTipo</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(Prion.Generic.Models.TituloTipo tituloTipo)
        {
            this.Validar(tituloTipo);

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            tituloTipo.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

            //mudar o estado do objeto para ALTERADO somente se id<>0
            if (tituloTipo.Id != 0) { tituloTipo.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }
            {
                try
                {
                    Prion.Generic.Repository.TituloTipo repTituloTipo = new Prion.Generic.Repository.TituloTipo(ref this.Conexao);

                    retorno = repTituloTipo.Salvar(tituloTipo);
                    repTituloTipo = null;

                    retorno.Mensagem = (tituloTipo.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Tipo de Título inserido com sucesso." : "Tipo de Título atualizado com sucesso.";
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
        /// Valida os atributos obrigatórios de uma Prion.Generic.Models.TituloTipo
        /// </summary>
        /// <param name="TituloTipo">Objeto do tipo Prion.Generic.Models.TituloTipo</param>
        private void Validar(Prion.Generic.Models.TituloTipo tituloTipo)
        {
            if (tituloTipo == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }

            if (tituloTipo.IdSituacao == 0)
            {
                throw new Prion.Tools.PrionException("A Situação tem que se informada.");
            }

            if (tituloTipo.Nome.Trim() == "")
            {
                throw new Prion.Tools.PrionException("O nome do Tipo de Título não pode ser vazio.");
            }

            if ((tituloTipo.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (tituloTipo.Id == 0))
            {
                throw new Prion.Tools.PrionException("O id do Tipo de Título não pode ser vazio.");
            }
        }
    }
}