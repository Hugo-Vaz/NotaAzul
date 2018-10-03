using System;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;
using GenericRepository = Prion.Generic.Repository;
using Prion.Tools;

namespace NotaAzul.Business
{
    public class Titulo: Base
    {
          /// <summary>
        /// Construtor da Classe
        /// </summary>
        public Titulo()
        { 
        }

        /// <summary>
        /// Carrega um ou mais registros do Título
        /// </summary>
        /// <param name="ids">id do Título</param>
        /// <returns></returns>
        public GenericModels.Lista Carregar(params Int32[] ids)
        {
            Prion.Generic.Repository.Titulo repTitulo = new Prion.Generic.Repository.Titulo(ref this.Conexao);
            return repTitulo.BuscarPeloId(ids);
        }

        /// <summary>
        /// Carrega todos os títulos que estão com a situação igual = 'Aberto'
        /// </summary>
        /// <param name="term">Parte da descrição de um título</param>
        /// <returns></returns>
        public GenericModels.Lista CarregarEmAberto(String term)
        {
            GenericRepository.Situacao repSituacao = new GenericRepository.Situacao(ref this.Conexao);
            GenericModels.Situacao situacaoAberta = (GenericModels.Situacao)repSituacao.BuscarPelaSituacao("Título", "Aberto");

            GenericRepository.Titulo repTitulo = new GenericRepository.Titulo(ref this.Conexao);

           
            //Busca o tipo de título correspondente a mensalidade
            Prion.Generic.Repository.TituloTipo repTituloTipo = new Prion.Generic.Repository.TituloTipo(ref this.Conexao);
            Prion.Tools.Request.ParametrosRequest parametroTipo = new Prion.Tools.Request.ParametrosRequest();
            parametroTipo.Filtro.Add(new Request.Filtro("TituloTipo.Nome", "LIKE", "Mensalidade"));
            Prion.Generic.Models.TituloTipo tituloTipo = (Prion.Generic.Models.TituloTipo)repTituloTipo.Buscar(parametroTipo).Get(0);


            Prion.Tools.Request.ParametrosRequest parametro = new Prion.Tools.Request.ParametrosRequest();
            parametro.Filtro.Add(new Request.Filtro("Descricao", "LIKE", term));
            parametro.Filtro.Add(new Request.Filtro("IdSituacao", "IN", situacaoAberta.Id));
            parametro.Filtro.Add(new Request.Filtro("IdTituloTipo", "NOT LIKE", tituloTipo.Id.ToString()));

            return repTitulo.Buscar(parametro);
        }  

        /// <summary>
        /// Exclui um ou mais registros do Título
        /// </summary>
        /// <param name="ids">id do Título</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] ids)
        {
            try
            {
                Prion.Generic.Repository.Titulo repTitulo = new Prion.Generic.Repository.Titulo(ref this.Conexao);
                repTitulo.Excluir(ids);
            }
            catch (Exception e)
            {
                return (new GenericHelpers.Retorno(e.Message, false));
            }

            return (new GenericHelpers.Retorno());
        }

        /// <summary>
        /// Obtém um DataTable dos Títulos que serão exibidos na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaTitulos(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Prion.Generic.Repository.Titulo repTitulo = new Prion.Generic.Repository.Titulo(ref this.Conexao);            
            repTitulo.Entidades.Adicionar(parametro.Entidades);

            Prion.Generic.Repository.TituloTipo repTituloTipo = new Prion.Generic.Repository.TituloTipo(ref this.Conexao);
            Prion.Tools.Request.ParametrosRequest parametroTipo = new Prion.Tools.Request.ParametrosRequest();
            parametroTipo.Filtro.Add(new Request.Filtro("TituloTipo.Nome", "LIKE", "Mensalidade"));
            Prion.Generic.Models.TituloTipo tituloTipo = (Prion.Generic.Models.TituloTipo)repTituloTipo.Buscar(parametroTipo).Get(0);

            parametro.Filtro.Add(new Request.Filtro("IdTituloTipo", "NOT LIKE", tituloTipo.Id.ToString()));

            return repTitulo.Lista(parametro);
        }

        /// <summary>
        /// Salva um objeto de Título no Banco de Dados
        /// </summary>
        /// <param name="titulo">objeto do tipo Prion.Generic.Models.Titulo</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(Prion.Generic.Models.Titulo titulo)
        {
            this.Validar(titulo);

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            titulo.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

            //mudar o estado do objeto para ALTERADO somente se id<>0
            if (titulo.Id != 0) { titulo.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }
            {
                try
                {
                    Prion.Generic.Repository.Titulo repTitulo = new Prion.Generic.Repository.Titulo(ref this.Conexao);

                    retorno = repTitulo.Salvar(titulo);
                    repTitulo = null;

                    retorno.Mensagem = (titulo.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Título inserido com sucesso." : "Título atualizado com sucesso.";
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
        /// Valida os atributos obrigatórios de uma Prion.Generic.Models.Titulo
        /// </summary>
        /// <param name="titulo">Objeto do tipo Prion.Generic.Models.Titulo</param>
        private void Validar(Prion.Generic.Models.Titulo titulo)
        {
            if (titulo == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }

            if (titulo.IdSituacao == 0)
            {
                throw new Prion.Tools.PrionException("A Situação tem que se informada.");
            }

            if (titulo.IdTituloTipo == 0)
            {
                throw new Prion.Tools.PrionException("O tipo de título tem que se informado.");
            }

            if (titulo.Descricao.Trim() == "")
            {
                throw new Prion.Tools.PrionException("A descrição do título não pode ser vazio.");
            }

            if (titulo.Valor == 0)
            {
                throw new Prion.Tools.PrionException("O valor título não pode ser vazio.");
            }

            if (Prion.Tools.IsNull.Date(titulo.DataVencimento))
            {
                throw new Prion.Tools.PrionException("A data de vencimento do título não pode ser vazia.");
            }            

            if ((titulo.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (titulo.Id == 0))
            {
                throw new Prion.Tools.PrionException("O id do Estado do título não pode ser vazio.");
            }
        }
    }
}