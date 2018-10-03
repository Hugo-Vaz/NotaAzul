using System;
using GenericHelpers = Prion.Generic.Helpers;

namespace NotaAzul.Business
{
    public class CursoAnoLetivo : Base
    {
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public CursoAnoLetivo()
        {
        }

        /// <summary>
        /// Carrega um registro de CursoAnoLetivo
        /// </summary>
        /// <param name="ids">id do CursoAnoLetivo</param>
        /// <returns></returns>
        public Prion.Generic.Models.Lista Carregar(Prion.Tools.Request.ParametrosRequest parametro, params Int32[] ids)
        {
            Repository.CursoAnoLetivo repCursoAnoLetivo = new Repository.CursoAnoLetivo(ref this.Conexao);
            repCursoAnoLetivo.Entidades.Adicionar(parametro.Entidades);
            return repCursoAnoLetivo.Buscar(ids);
        }

       
        /// <summary>
        /// Exclui um ou mais registros de CursoAnoLetivo
        /// </summary>
        /// <param name="id">id do CursoAnoLetivo</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] id)
        {
            try
            {
                Repository.CursoAnoLetivo repCursoAnoLetivo = new Repository.CursoAnoLetivo(ref this.Conexao);
                repCursoAnoLetivo.Excluir(id);
            }
            catch (Exception e)
            {
                return (new GenericHelpers.Retorno(e.Message, false));
            }

            return (new GenericHelpers.Retorno());
        }

        /// <summary>
        /// Obtém a lista de CursoAnoLetivo
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista Lista(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.CursoAnoLetivo repCursoAnoLetivo = new Repository.CursoAnoLetivo(ref this.Conexao);
            return repCursoAnoLetivo.Buscar(parametro);
        }

        /// <summary>
        /// Obtém um DataTable de CursoAnoLetivo que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaCursoAnoLetivo(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.CursoAnoLetivo repCursoAnoLetivo = new Repository.CursoAnoLetivo(ref this.Conexao);
            repCursoAnoLetivo.Entidades.Adicionar(parametro.Entidades);

            return repCursoAnoLetivo.DataTable(parametro);
        }

        /// <summary>
        /// Salva um objeto de CursoAnoLetivo no banco de dados
        /// </summary>
        /// <param name="cursoAnoLetivo">objeto do tipo Models.CursoAnoLetivo</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(Models.CursoAnoLetivo cursoAnoLetivo)
        {
            this.Validar(cursoAnoLetivo);

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = new GenericHelpers.Retorno("", true);

            cursoAnoLetivo.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

            // mudar o estado do objeto para ALTERADO apenas se ID <> 0
            if (cursoAnoLetivo.Id != 0) { cursoAnoLetivo.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }

            try
            {
                Repository.CursoAnoLetivo repCursoAnoLetivo = new Repository.CursoAnoLetivo(ref this.Conexao);

                retorno = repCursoAnoLetivo.Salvar(cursoAnoLetivo);
                repCursoAnoLetivo = null;

                retorno.Mensagem = (cursoAnoLetivo.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Curso inserido com sucesso para o ano letivo." : "Curso atualizado com sucesso para o ano letivo.";
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
        /// Valida os atributos obrigatórios de uma Model.CursoAnoLetivo
        /// </summary>
        /// <param name="cursoAnoLetivo">Objeto do tipo Model.CursoAnoLetivo</param>
        private void Validar(Models.CursoAnoLetivo cursoAnoLetivo)
        {
            if (cursoAnoLetivo == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }

            if (cursoAnoLetivo.IdSituacao == 0)
            {
                throw new Prion.Tools.PrionException("");
            }

            if ((cursoAnoLetivo.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (cursoAnoLetivo.Id == 0))
            {
                throw new Prion.Tools.PrionException("");
            }
        }
    }
}