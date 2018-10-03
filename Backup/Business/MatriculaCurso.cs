using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.Business
{
    public class MatriculaCurso : Prion.Generic.Business.Base
    {
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public MatriculaCurso()
        {
        }


        /// <summary>
        /// Carrega um ou mais registros de MatriculaCurso
        /// </summary>
        /// <param name="ids">id do Matricula</param>
        /// <returns></returns>
        public GenericModels.Lista Carregar(Prion.Tools.Request.ParametrosRequest parametro, Int32 id)
        {
            Repository.MatriculaCurso repMatriculaCurso = new Repository.MatriculaCurso(ref this.Conexao);
            repMatriculaCurso.Entidades.Adicionar(parametro.Entidades);
            return repMatriculaCurso.Buscar(id);
        }

        /// <summary>
        /// Exclui um ou mais registros de MatriculaCurso
        /// </summary>
        /// <param name="id">id de MatriculaCurso</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] ids)
        {
            try
            {
                Repository.MatriculaCurso repMatriculaCurso = new Repository.MatriculaCurso(ref this.Conexao);
               
                Int32 quantidadeDeRegistrosExcluidos = repMatriculaCurso.Excluir(ids);
                if (quantidadeDeRegistrosExcluidos == 0)
                {
                    String msgErro = "Não foi possível desvencilhar o curso da matrícula,pois alguma mensalidade já foi quitada";
                    return new GenericHelpers.Retorno(msgErro, false); 
                }
               
            }
            catch (Exception e)
            {
                return (new GenericHelpers.Retorno(e.Message, false));
            }

            return (new GenericHelpers.Retorno());
        }

        /// <summary>
        /// Obtém um DataTable de MatriculaCurso que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaMatriculas(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.MatriculaCurso repMatriculaCurso = new Repository.MatriculaCurso(ref this.Conexao);
            repMatriculaCurso.Entidades.Adicionar(parametro.Entidades);

            return repMatriculaCurso.DataTable(parametro);
        }

        /// <summary>
        /// Salva um objeto de MatriculaCurso no banco de dados
        /// </summary>
        /// <param name="listaMatriculaCurso">objeto do tipo Models.MatriculaCurso</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(List<Models.MatriculaCurso> listaMatriculaCurso)
        {
            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            for(Int32 i=0;i<listaMatriculaCurso.Count;i++){
                this.Validar(listaMatriculaCurso[i]);
                listaMatriculaCurso[i].EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

                // mudar o estado do objeto para ALTERADO apenas se ID <> 0
                if (listaMatriculaCurso[i].Id != 0) { listaMatriculaCurso[i].EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }

                try
                {
                    Repository.MatriculaCurso rpMatriculaCurso = new Repository.MatriculaCurso(ref this.Conexao);

                    retorno = rpMatriculaCurso.Salvar(listaMatriculaCurso[i]);
                    rpMatriculaCurso = null;

                    if (!retorno.Sucesso)
                    {
                        throw new Exception(retorno.Mensagem);
                    }
                
                    // se for uma matrícula nova, atualiza o IdMatriculaCurso de todas as mensalidades
                    if (listaMatriculaCurso[i].EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo)
                    {
                        // varre todas as mensalidades e atualiza o IdMatricula
                        for (Int32 j = 0; j < listaMatriculaCurso[i].Mensalidades.Count; j++)
                        {
                            listaMatriculaCurso[i].Mensalidades[j].IdMatriculaCurso = retorno.UltimoId;
                        }
                    }
                    Business.Mensalidade biMensalidade = new Business.Mensalidade();
                    GenericHelpers.Retorno retornoMensalidade = biMensalidade.Salvar(listaMatriculaCurso[i].Mensalidades);
                    
                    retorno.Mensagem = (listaMatriculaCurso[i].EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "MatriculaCurso efetuada com sucesso" : "MatriculaCurso atualizada com sucesso";
                    this.Conexao.Commit();
                }
                catch (Exception e)
                {
                    this.Conexao.Rollback();
                    retorno = new GenericHelpers.Retorno(e.Message, false);
                }
            }
            return retorno;
        }

        /// <summary>
        /// Valida os atributos obrigatórios de uma Model.Matricula
        /// </summary>
        /// <param name="matriculaCurso">Objeto do tipo Model.Matricula</param>
        private void Validar(Models.MatriculaCurso matriculaCurso)
        {
            if (matriculaCurso == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }

            if (matriculaCurso.IdMatricula == 0)
            {
                throw new Prion.Tools.PrionException("A matrícula precisa ser informada.");
            }

            if (matriculaCurso.IdTurma == 0)
            {
                throw new Prion.Tools.PrionException("A Turma tem que ser informada.");
            }            

            if ((matriculaCurso.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (matriculaCurso.Id == 0))
            {
                throw new Prion.Tools.PrionException("O id da Matrícula não pode ser zero.");
            }
        }
    }
}