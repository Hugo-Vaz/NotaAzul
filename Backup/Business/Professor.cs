using System;
using GenericHelpers = Prion.Generic.Helpers;
using GenericRepository = Prion.Generic.Repository;
using GenericModel = Prion.Generic.Models;
using System.Collections.Generic;

namespace NotaAzul.Business
{
    public class Professor: Base
    {
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public Professor()
        {
        }

        /// <summary>
        /// Carrega um registro de Professor
        /// </summary>
        /// <param name="ids">id do Professor</param>
        /// <returns></returns>
        public Models.Professor Carregar(Prion.Tools.Request.ParametrosRequest parametro, Int32 id)
        {
            Models.Professor professor = new Models.Professor();
            GenericRepository.Funcionario repFuncionario = new GenericRepository.Funcionario(ref this.Conexao);
            Repository.Disciplina repDisciplina = new Repository.Disciplina(ref this.Conexao);

            repFuncionario.Entidades.Adicionar(parametro.Entidades);
            repDisciplina.Entidades.Adicionar(parametro.Entidades);
            professor.Funcionario = (GenericModel.Funcionario)repFuncionario.BuscarPeloId(id).Get(0);
            
            Prion.Tools.Request.Filtro f = new Prion.Tools.Request.Filtro("ProfessorDisciplina.IdFuncionario", "=", id);
            parametro.Filtro.Add(f);

            professor.Disciplinas = Prion.Tools.ListTo.CollectionToList<Models.Disciplina>(repDisciplina.Buscar(parametro).ListaObjetos);
            return professor;
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
        public GenericHelpers.Retorno Salvar(List<Models.ProfessorDisciplina> listaProfessorDisciplina)
        {
            
            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = new GenericHelpers.Retorno("", true);
            Business.Situacao biSituacao = new Business.Situacao();
            GenericModel.Situacao situacaoProfessor = biSituacao.CarregarSituacoesPelaSituacao("Professor", "Ativo");
            foreach (Models.ProfessorDisciplina professorDisciplina in listaProfessorDisciplina)
            {
                professorDisciplina.IdSituacao = situacaoProfessor.Id;

                try
                {
                    Repository.Disciplina repDisciplina = new Repository.Disciplina(ref this.Conexao);

                    if (professorDisciplina.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo)
                    {
                        repDisciplina.CriarRelacionamentoProfessorDisciplina(professorDisciplina);
                    }
                    else if (professorDisciplina.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Excluido)
                    {
                        repDisciplina.ExcluirRelacionamentoProfessorDisciplina(professorDisciplina);
                    }
                    
                    repDisciplina = null;

                    retorno.Mensagem = "Disciplinas relacionadas ao professor com suceso.";
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
       
    }
}