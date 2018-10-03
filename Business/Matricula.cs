using System;
using System.Collections.Generic;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.Business
{
    public class Matricula : Base
    {
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public Matricula()
        {
        }

        /// <summary>
        /// Carrega um ou mais registros de Matricula
        /// </summary>
        /// <param name="ids">id do Matricula</param>
        /// <returns></returns>
        public GenericModels.Lista Carregar(Prion.Tools.Request.ParametrosRequest parametro, params Int32[] ids)
        {
            Repository.Matricula repMatricula = new Repository.Matricula(ref this.Conexao);

            if (parametro!=null) { repMatricula.Entidades.Adicionar(parametro.Entidades); }
            return repMatricula.Buscar(ids);
        }

        public bool ChecarPossibilidadeDeMatricula(Int32 anoLetivo,Int32 idAluno)
        {
            Repository.Matricula repMatricula = new Repository.Matricula(ref this.Conexao);

            return repMatricula.ChecarSeExisteMatriculaParaAnoLetivo(anoLetivo, idAluno);
        }

        /// <summary>
        /// Exclui um ou mais registros de Matricula
        /// </summary>
        /// <param name="id">id da Matricula</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] ids)
        {
            GenericHelpers.Retorno retorno = new GenericHelpers.Retorno();
            this.Conexao.UsarTransacao = true;
            try
            {
                Repository.Matricula repMatricula = new Repository.Matricula(ref this.Conexao);
                String mensagem = null;

                for (Int32 i = 0; i < ids.Length; i++)
                {

                    repMatricula.Excluir(ids[i])
;                   // o Javascript irá executar um String.format para colocar o nome do aluno na posição {0}
                    mensagem = "A matrícula foi marcada como excluída, as mensalidades que ainda não foram quitadas, foram marcadas como CANCELADAS";
                    retorno = new GenericHelpers.Retorno(mensagem, false); 
                    
                }

                this.Conexao.Commit();
            }
            catch (Exception e)
            {
                this.Conexao.Rollback();
                return (new GenericHelpers.Retorno(e.Message, false));
            }
            
            return retorno;
        }
        
        /// <summary>
        /// Obtém a lista de Matriculas
        /// </summary>
        /// <returns>Lista de Matriculas</returns>
        public Prion.Generic.Models.Lista Lista(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Matricula repMatricula = new Repository.Matricula(ref this.Conexao);
            return repMatricula.Buscar(parametro);
        }

        /// <summary>
        /// Obtém um DataTable de Matriculas que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaMatriculas(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Matricula repMatricula = new Repository.Matricula(ref this.Conexao);
            repMatricula.Entidades.Adicionar(parametro.Entidades);
            
            return repMatricula.DataTable(parametro);
        }
      
        /// <summary>
        /// Salva um objeto de Matricula no banco de dados
        /// </summary>
        /// <param name="matricula">objeto do tipo Models.Matricula</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(Models.Matricula matricula, List<Models.Boleto> boletos)
        {
            this.Validar(matricula);

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            matricula.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

            // mudar o estado do objeto para ALTERADO apenas se ID <> 0
            if (matricula.Id != 0) { matricula.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }

            try
            {
                Repository.Matricula rpMatricula = new Repository.Matricula(ref this.Conexao);
                matricula.ListaBoletos = boletos;
                retorno = rpMatricula.Salvar(matricula);
                rpMatricula = null;

                if (!retorno.Sucesso) 
                {
                    this.Conexao.Rollback();
                    return retorno; 
                }
                
                retorno.Mensagem = (matricula.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Aluno matriculado com sucesso" : "Matrícula atualizada com sucesso";
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
        /// Valida os atributos obrigatórios de uma Model.Matricula
        /// </summary>
        /// <param name="matricula">Objeto do tipo Model.Matricula</param>
        private void Validar(Models.Matricula matricula)
        {
            if (matricula == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }        

            if (matricula.IdSituacao == 0)
            {
                throw new Prion.Tools.PrionException("A situação tem que ser informada.");
            }

            if (matricula.IdAluno == 0)
            {
                throw new Prion.Tools.PrionException("O aluno tem que ser informado.");
            }

            if ((matricula.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (matricula.Id == 0))
            {
                throw new Prion.Tools.PrionException("O id da Matrícula não pode ser zero.");
            }
        }
        
    }
}