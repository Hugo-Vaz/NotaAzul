using System;
using GenericHelpers = Prion.Generic.Helpers;

namespace NotaAzul.Business
{
    public class Turma : Base
    {
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public Turma()
        {
        }

        /// <summary>
        /// Carrega um registro de Turma
        /// </summary>
        /// <param name="ids">id da Turma</param>
        /// <returns></returns>
        public Prion.Generic.Models.Lista Carregar(params Int32[] ids)
        {
            Repository.Turma repTurma = new Repository.Turma(ref this.Conexao);
            return repTurma.Buscar(ids);
        }

        /// <summary>
        /// Carrega um registro de Turma
        /// </summary>
        /// <param name="ids">id da Turma</param>
        /// <returns></returns>
        public Prion.Generic.Models.Lista Carregar(Prion.Tools.Request.ParametrosRequest parametro, params Int32[] ids)
        {
            Repository.Turma repTurma = new Repository.Turma(ref this.Conexao);
            repTurma.Entidades.Adicionar(parametro.Entidades);
            return repTurma.Buscar(ids);
        }

        /// <summary>
        /// Exclui um ou mais registros de Turma
        /// </summary>
        /// <param name="id">id da Turma</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] id)
        {
            try
            {
                Repository.Turma repTurma = new Repository.Turma(ref this.Conexao);
                repTurma.Excluir(id);
            }
            catch (Exception e)
            {
                return (new GenericHelpers.Retorno(e.Message, false));
            }

            return (new GenericHelpers.Retorno());
        }

        /// <summary>
        /// Obtém a lista de Turmas
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista Lista(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Turma repTurma = new Repository.Turma(ref this.Conexao);
            return repTurma.Buscar(parametro);
        }

        /// <summary>
        /// Obtém um DataTable de Turmas que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaTurmas(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Turma repTurma = new Repository.Turma(ref this.Conexao);
            repTurma.Entidades.Adicionar(parametro.Entidades);

            return repTurma.DataTable(parametro);
        }

        /// <summary>
        /// Salva um objeto de Turma no banco de dados
        /// </summary>
        /// <param name="turma">objeto do tipo Models.Turma</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(Models.Turma turma)
        {
            this.Validar(turma);

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = new GenericHelpers.Retorno("", true);

            turma.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

            // mudar o estado do objeto para ALTERADO apenas se ID <> 0
            if (turma.Id != 0) { turma.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }

            try
            {
                Repository.Turma repTurma = new Repository.Turma(ref this.Conexao);

                retorno = repTurma.Salvar(turma);
                repTurma = null;

                retorno.Mensagem = (turma.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Turma inserida com sucesso." : "Turma atualizada com sucesso";
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
        /// Cria o relacionamento entre Turma e suas disciplinas no banco de dados
        /// </summary>
        /// <param name="idTurma"></param>
        /// <param name="idsDisciplinas"></param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno CriarRelacionamento(Int32 idTurma, Int32[] idsDisciplinas, Int32[] estado)
        {
            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = new GenericHelpers.Retorno("", true);

            try
            {
                Repository.Turma repTurma = new Repository.Turma(ref this.Conexao);
                for (Int32 i = 0; i < idsDisciplinas.Length; i++)
                {
                    if ((GenericHelpers.Enums.EstadoObjeto)estado[i] == GenericHelpers.Enums.EstadoObjeto.Novo)
                    {
                        repTurma.CriarRelacionamento(idTurma, idsDisciplinas[i]);
                    }
                    else if ((GenericHelpers.Enums.EstadoObjeto)estado[i] == GenericHelpers.Enums.EstadoObjeto.Excluido)
                    {
                        repTurma.ExcluirRelacionamento(idTurma, idsDisciplinas[i]);
                    }
                }

                repTurma = null;
                retorno.Mensagem = "Disciplinas relacionadas a esta Turma com sucesso.";
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
        /// Valida os atributos obrigatórios de uma Model.Turma
        /// </summary>
        /// <param name="turma">Objeto do tipo Model.Turma</param>
        private void Validar(Models.Turma turma)
        {
            if (turma == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }

            if (turma.IdSituacao == 0)
            {
                throw new Prion.Tools.PrionException("A situação da turma não pode ser vazio.");
            }

            if (turma.Nome.Trim() == "")
            {
                throw new Prion.Tools.PrionException("O nome da turma não pode ser vazio.");
            }

            if ((turma.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (turma.Id == 0))
            {
                throw new Prion.Tools.PrionException("O id da turma não pode ser vazio.");
            }
        }
    }
}