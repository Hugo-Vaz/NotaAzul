using System;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;
using System.Collections.Generic;


namespace NotaAzul.Business
{
    public class NotasAluno:Base
    {
         /// <summary>
        /// Construtor da Classe
        /// </summary>
        public NotasAluno()
        {
        }

        /// <summary>
        /// Carrega um ou mais registros de MatriculaFormaDeAvaliacao
        /// </summary>
        /// <param name="ids">id de Forma de avaliação</param>
        /// <returns></returns>
        public GenericModels.Lista Carregar(Prion.Tools.Request.ParametrosRequest parametro,Int32 idMatricula)
        {
            Repository.NotasAluno repNotasAluno = new Repository.NotasAluno(ref this.Conexao);
            repNotasAluno.Entidades.Adicionar(parametro.Entidades);

            Prion.Tools.Request.Filtro f = new Prion.Tools.Request.Filtro("MatriculaFormaDeAvaliacao.IdMatricula", "=", idMatricula);
            parametro.Filtro.Add(f);

            return repNotasAluno.Buscar(parametro);
        }

        /// <summary>
        /// Carrega um ou mais registros de MatriculaMedia
        /// </summary>
        /// <param name="ids">id Matricula</param>
        /// <returns></returns>
        public GenericModels.Lista CarregarMedia(Prion.Tools.Request.ParametrosRequest parametro, Int32 idMatricula)
        {
            Repository.MatriculaMedia repMatriculaMedia = new Repository.MatriculaMedia(ref this.Conexao);
            repMatriculaMedia.Entidades.Adicionar(parametro.Entidades);

            Prion.Tools.Request.Filtro f = new Prion.Tools.Request.Filtro("MatriculaMedia.IdMatricula", "=", idMatricula);
            if (parametro.Filtro.Count != 0)
            {
                parametro.Filtro[2] = f;
            }
            else
            {
                parametro.Filtro.Add(f);
            }

            return repMatriculaMedia.Buscar(parametro);
        }

        /// <summary>
        /// Obtém um DataTable de NotasAluno que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaAlunos(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.NotasAluno repNotasAluno = new Repository.NotasAluno(ref this.Conexao);
            repNotasAluno.Entidades.Adicionar(parametro.Entidades);

            return repNotasAluno.DataTable(parametro);
        }

        /// <summary>
        /// Obtém um DataTable de NotasAluno que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaFormasDeAvaliacao(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.NotasAluno repNotasAluno = new Repository.NotasAluno(ref this.Conexao);
            repNotasAluno.Entidades.Adicionar(parametro.Entidades);

            return repNotasAluno.DataTableFormaAvaliacao(parametro);
        }

        /// <summary>
        /// Salva um objeto de MatriculaFormaDeAvaliacao no Banco de Dados
        /// </summary>
        /// <param name="listaMatriculaFormaDeAvaliacao">objeto do tipo Models.MatriculaFormaDeAvaliacao</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(List<Models.MatriculaFormaDeAvaliacao> listaMatriculaFormaDeAvaliacao)
        {

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;           

            try
            {
                foreach (Models.MatriculaFormaDeAvaliacao matriculaFormaDeAvalicao in listaMatriculaFormaDeAvaliacao)
                {
                    matriculaFormaDeAvalicao.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

                    //mudar o estado do objeto para ALTERADO somente se id<>0
                    if (matriculaFormaDeAvalicao.Id != 0) { matriculaFormaDeAvalicao.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }
                    Repository.NotasAluno repMatriculaFormaDeAvaliacao = new Repository.NotasAluno(ref this.Conexao);

                    retorno = repMatriculaFormaDeAvaliacao.Salvar(matriculaFormaDeAvalicao);

                    // verifica se houve algum erro ao salvar o aluno
                    if (!retorno.Sucesso)
                    {
                        this.Conexao.Rollback();
                        return retorno;
                    }


                    repMatriculaFormaDeAvaliacao = null;
                    retorno.Mensagem = (matriculaFormaDeAvalicao.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Valores das formas de avaliação inseridas com sucesso." : "Valores das formas de avaliação atualizadas com sucesso.";
                
                }
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
        /// Salva um objeto de MatriculaMedia no Banco de Dados
        /// </summary>
        /// <param name="MatriculaMedia">objeto do tipo Models.MatriculaFormaDeAvaliacao</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno SalvarMedia(Models.MatriculaMedia matriculaMedia)
        {

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            try
            {                        
                Repository.MatriculaMedia repMatriculaMedia = new Repository.MatriculaMedia(ref this.Conexao);

                retorno = repMatriculaMedia.Salvar(matriculaMedia);

                // verifica se houve algum erro ao salvar a média
                if (!retorno.Sucesso)
                {
                    this.Conexao.Rollback();
                    return retorno;
                }


                repMatriculaMedia = null;
                retorno.Mensagem =  "Média salva com sucesso";

                
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
}