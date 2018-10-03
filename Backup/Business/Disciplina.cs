using System;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.Business
{
    public class Disciplina : Base
    {
         /// <summary>
        /// Construtor da Classe
        /// </summary>
        public Disciplina()
        { 
        }

        /// <summary>
        /// Carrega um ou mais registros de Disciplina
        /// </summary>
        /// <param name="ids">id de Disciplina</param>
        /// <returns></returns>
        public GenericModels.Lista Carregar(params Int32[] ids)
        {
            Repository.Disciplina repDisciplina = new Repository.Disciplina(ref this.Conexao);
            return repDisciplina.Buscar(ids);
        }

        /// <summary>
        /// Carrega um ou mais registros de Disciplina
        /// </summary>
        /// <param name="ids">id de Disciplina</param>
        /// <returns></returns>
        public GenericModels.Lista Carregar(Prion.Tools.Request.ParametrosRequest parametro)
        {
            Repository.Disciplina repDisciplina = new Repository.Disciplina(ref this.Conexao);
            repDisciplina.Entidades.Adicionar(parametro.Entidades);
            return repDisciplina.Buscar(parametro);
        }

        /// <summary>
        /// Exclui um ou mais registros de Disciplina
        /// </summary>
        /// <param name="ids">id de Disciplina</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] ids)
        {
            try
            {
                Repository.Disciplina repDisciplina = new Repository.Disciplina(ref this.Conexao);
                repDisciplina.Excluir(ids);
            }
            catch (Exception e)
            {
                return (new GenericHelpers.Retorno(e.Message, false));
            }

            return (new GenericHelpers.Retorno());
        }

        /// <summary>
        /// Obtém um DataTable de Disciplinas que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaDisciplinas(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Disciplina repDisciplina = new Repository.Disciplina(ref this.Conexao);
            repDisciplina.Entidades.Adicionar(parametro.Entidades);

            return repDisciplina.DataTable(parametro);
        }

        /// <summary>
        /// Salva um objeto de Disciplina no Banco de Dados
        /// </summary>
        /// <param name="disciplina">objeto do tipo Models.Disciplina</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(Models.Disciplina disciplina)
        {
            this.Validar(disciplina);

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            disciplina.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

            //mudar o estado do objeto para ALTERADO somente se id<>0
            if (disciplina.Id != 0) { disciplina.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }
            {
                try
                {
                    Repository.Disciplina repDisciplina = new Repository.Disciplina(ref this.Conexao);

                    retorno = repDisciplina.Salvar(disciplina);
                    repDisciplina = null;

                    retorno.Mensagem = (disciplina.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Disciplina inserida com sucesso." : "Disciplina atualizada com sucesso.";
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
        /// Valida os atributos obrigatórios de uma Model.Disciplina
        /// </summary>
        /// <param name="disciplina">Objeto do tipo Model.Disciplina</param>
        private void Validar(Models.Disciplina disciplina)
        {
            if (disciplina == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }

            if (disciplina.IdSituacao == 0)
            {
                throw new Prion.Tools.PrionException("A Situação tem que se informada.");
            }

            if (disciplina.Nome.Trim() == "")
            {
                throw new Prion.Tools.PrionException("O nome da Disciplina não pode ser vazio.");
            }

            if ((disciplina.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (disciplina.Id == 0))
            {
                throw new Prion.Tools.PrionException("O id da Disciplina não pode ser vazio.");
            }
        }
    }
}