using System;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;


namespace NotaAzul.Business
{
    public class ProfessorConfiguracao: Base
    {
         /// <summary>
        /// Construtor da Classe
        /// </summary>
        public ProfessorConfiguracao()
        { 
        }


        /// <summary>
        /// Carrega um ou mais registros de ProfessorConfiguracao
        /// </summary>
        /// <param name="ids">id de Usuario</param>
        /// <returns></returns>
        public GenericModels.Lista Carregar(Prion.Tools.Request.ParametrosRequest parametro)
        {
            Repository.ProfessorConfiguracao repProfessorConfiguracao = new Repository.ProfessorConfiguracao(ref this.Conexao);
            repProfessorConfiguracao.Entidades.Adicionar(parametro.Entidades);

            return repProfessorConfiguracao.Buscar(parametro);
        }

        /// <summary>
        /// Salva um objeto de ProfessorConfiguracao no Banco de Dados
        /// </summary>
        /// <param name="professorConfiguracao">objeto do tipo Models.ProfessorConfiguracao</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(Models.ProfessorConfiguracao professorConfiguracao)
        {
            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            professorConfiguracao.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

            //mudar o estado do objeto para ALTERADO somente se id<>0
            if (professorConfiguracao.Id != 0) { professorConfiguracao.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }

            try
            {
                Repository.ProfessorConfiguracao repProfessorConfiguracao = new Repository.ProfessorConfiguracao(ref this.Conexao);
                
                retorno = repProfessorConfiguracao.Salvar(professorConfiguracao);

                // verifica se houve algum erro ao salvar o ProfessorConfiguracao
                if (!retorno.Sucesso)
                {
                    this.Conexao.Rollback();
                    return retorno;
                }


                repProfessorConfiguracao = null;


                retorno.Mensagem = (professorConfiguracao.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Configuração inserida com sucesso." : "Configuração atualizada com sucesso.";
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