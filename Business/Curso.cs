using System;
using GenericHelpers = Prion.Generic.Helpers;

namespace NotaAzul.Business
{
    public class Curso : Base
    {
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public Curso()
        {
        }

        /// <summary>
        /// Carrega um registro de Curso
        /// </summary>
        /// <param name="ids">id do Curso</param>
        /// <returns></returns>
        public Prion.Generic.Models.Lista Carregar(params Int32[] ids)
        {
            Repository.Curso repCurso = new Repository.Curso(ref this.Conexao);
            return repCurso.Buscar(ids);
        }

        /// <summary>
        /// Exclui um ou mais registros de Curso
        /// </summary>
        /// <param name="id">id do Curso</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] id)
        {
            try
            {
                Repository.Curso repCurso = new Repository.Curso(ref this.Conexao);
                repCurso.Excluir(id);
            }
            catch (Exception e)
            {
                return (new GenericHelpers.Retorno(e.Message, false));
            }

            return (new GenericHelpers.Retorno());
        }

        /// <summary>
        /// Obtém a lista de Cursos
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista Lista(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Curso repCurso = new Repository.Curso(ref this.Conexao);
            return repCurso.Buscar(parametro);
        }

        /// <summary>
        /// Obtém um DataTable de Cursos que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaCursos(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Curso repCurso = new Repository.Curso(ref this.Conexao);
            repCurso.Entidades.Adicionar(parametro.Entidades);

            return repCurso.DataTable(parametro);
        }

        /// <summary>
        /// Salva um objeto de Curso no banco de dados
        /// </summary>
        /// <param name="curso">objeto do tipo Models.Curso</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(Models.Curso curso)
        {
            this.Validar(curso);

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = new GenericHelpers.Retorno("", true);

            curso.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

            // mudar o estado do objeto para ALTERADO apenas se ID <> 0
            if (curso.Id != 0) { curso.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }

            try
            {
                Repository.Curso repCurso = new Repository.Curso(ref this.Conexao);

                retorno = repCurso.Salvar(curso);
                repCurso = null;

                retorno.Mensagem = (curso.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Curso inserido com sucesso." : "Curso atualizado com sucesso";
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
        /// Valida os atributos obrigatórios de uma Model.Curso
        /// </summary>
        /// <param name="curso">Objeto do tipo Model.Curso</param>
        private void Validar(Models.Curso curso)
        {
            if (curso == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }

            if (curso.IdSituacao == 0)
            {
                throw new Prion.Tools.PrionException("A situação do curso não pode ser vazio.");
            }

            if (curso.Nome.Trim() == "")
            {
                throw new Prion.Tools.PrionException("O nome do curso não pode ser vazio.");
            }

            if ((curso.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (curso.Id == 0))
            {
                throw new Prion.Tools.PrionException("O id do curso não pode ser vazio.");
            }
        }
    }
}