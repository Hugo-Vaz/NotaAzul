using System;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;
using System.Collections.Generic;

namespace NotaAzul.Business
{
    public class Aluno : Base
    {
         /// <summary>
        /// Construtor da Classe
        /// </summary>
        public Aluno()
        { 
        }


        /// <summary>
        /// Carrega um ou mais registros de Aluno
        /// </summary>
        /// <param name="ids">id de Aluno</param>
        /// <returns></returns>
        public GenericModels.Lista Carregar(Prion.Tools.Request.ParametrosRequest parametro, params Int32[] ids)
        {
            Repository.Aluno repAluno = new Repository.Aluno(ref this.Conexao);
            repAluno.Entidades.Adicionar(parametro.Entidades);

            return repAluno.Buscar(ids);
        }

        /// <summary>
        /// Carrega um ou mais registros de Aluno
        /// </summary>
        /// <param name="ids">nome de Aluno</param>
        /// <returns></returns>
        public GenericModels.Lista Carregar(String nomeAluno)
        {
            Repository.Aluno repAluno = new Repository.Aluno(ref this.Conexao);
            return repAluno.Buscar(nomeAluno);
        }

        /// <summary>
        /// Carrega um ou mais registros de AlunoResponsavel
        /// </summary>
        /// <param name="nomeResponsavel">nome do AlunoResponsavel</param>
        /// <returns></returns>
        public GenericModels.Lista CarregarResponsaveis(Int32[]ids)
        {
            Repository.AlunoResponsavel repResponsavel = new Repository.AlunoResponsavel(ref this.Conexao);

            return repResponsavel.Buscar(ids);
        }

        /// <summary>
        /// Carrega um ou mais registros de AlunoResponsavel
        /// </summary>
        /// <param name="nomeResponsavel">nome do AlunoResponsavel</param>
        /// <returns></returns>
        public GenericModels.Lista CarregarResponsaveis(String nomeResponsavel)
        {
            // carrega a situação ATIVO do AlunoResponsavel
            Business.Situacao biSituacao = new Business.Situacao();
            GenericModels.Situacao situacao = biSituacao.CarregarSituacoesPelaSituacao("AlunoResponsavel", "Ativo");

            if (situacao == null)
            {
                throw new Prion.Tools.PrionException("Erro ao carregar a situação do Responsável.");
            }

            // carrega uma lista de AlunoResponsavel que estão ATIVOS (baseado no IdSituacao) e que contenha a string "nomeResponsavel" em seu nome
            Repository.AlunoResponsavel repResponsavel = new Repository.AlunoResponsavel(ref this.Conexao);

            Prion.Tools.Request.ParametrosRequest parametro = new Prion.Tools.Request.ParametrosRequest();
            parametro.Filtro.Add(new Prion.Tools.Request.Filtro("IdSituacao", "=", situacao.Id));
            parametro.Filtro.Add(new Prion.Tools.Request.Filtro("Nome", "LIKE", "%" + nomeResponsavel + "%"));

            return repResponsavel.Buscar(parametro);
        }

        /// <summary>
        /// Carrega os registros necessários para se criar uma ficha financeira
        /// </summary>
        /// <param name="idAluno">id do Aluno</param>
        /// <returns></returns>
        public Models.Matricula CarregarFichaFinanceira(Prion.Tools.Request.ParametrosRequest parametro, Int32 idAluno)
        {
            Models.Matricula matricula = new Models.Matricula();

            Repository.Matricula repMatricula = new Repository.Matricula(ref this.Conexao);
            repMatricula.Entidades.Adicionar(parametro.Entidades);

            Prion.Tools.Request.Filtro f = new Prion.Tools.Request.Filtro("IdAluno", "=", idAluno);
            parametro.Filtro.Add(f);

            matricula = (Models.Matricula)repMatricula.Buscar(parametro).Get(0);
            return matricula;
        }

        /// <summary>
        /// Exclui um ou mais registros de Aluno
        /// </summary>
        /// <param name="ids">id de Aluno</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] ids)
        {
            GenericHelpers.Retorno retorno = new GenericHelpers.Retorno(); 

            try
            {
               
                Repository.Aluno repAluno = new Repository.Aluno(ref this.Conexao);
                List<String> alunosMatriculados = repAluno.BuscarAlunosMatriculados(ids);

                if (alunosMatriculados.Count > 0)
                {
                    retorno.Mensagem = "Os seguintes alunos não foram excluídos pois já foram matriculados,logo foram apenas inativados";
                    for (Int32 i = 0; i < alunosMatriculados.Count; i++)
                    {
                        retorno.Mensagem += "</br>" + alunosMatriculados[i];
                    }
                }

                else
                {
                    retorno.Mensagem = "Aluno excluído com sucesso";
                }
                repAluno.Excluir(ids);
            }
            catch (Exception e)
            {
                return (new GenericHelpers.Retorno(e.Message, false));
            }

            return (retorno);
        }


        /// <summary>
        /// Obtém um DataTable de Alunos que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaAlunos(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Aluno repAluno = new Repository.Aluno(ref this.Conexao);
            repAluno.Entidades.Adicionar(parametro.Entidades);

            return repAluno.DataTable(parametro);
        }


        /// <summary>
        /// Salva um objeto de Aluno no Banco de Dados
        /// </summary>
        /// <param name="aluno">objeto do tipo Models.Aluno</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(Models.Aluno aluno)
        {
            this.Validar(aluno);

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            aluno.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

            //mudar o estado do objeto para ALTERADO somente se id<>0
            if (aluno.Id != 0) { aluno.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }

            try
            {
                Repository.Aluno repAluno = new Repository.Aluno(ref this.Conexao);
                
                FormatarCampos(ref aluno);
                retorno = repAluno.Salvar(aluno);

                // verifica se houve algum erro ao salvar o aluno
                if (!retorno.Sucesso)
                {
                    this.Conexao.Rollback();
                    return retorno;
                }


                repAluno = null;


                retorno.Mensagem = (aluno.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Aluno inserido com sucesso." : "Aluno atualizado com sucesso.";
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
        /// 
        /// </summary>
        /// <param name="aluno"></param>
        private void FormatarCampos(ref Models.Aluno aluno)
        {
            // remove a formatação do cpf caso o cpf tenha sido preenchido.
            if (aluno.Cpf != null)
            {
                aluno.Cpf = aluno.Cpf.Replace(".", "").Replace("-", "");
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aluno"></param>
        public Models.AlunoResponsavel CarregarResponsavelBoleto(Int32 idBoleto)
        {
            Repository.AlunoResponsavel repResp = new Repository.AlunoResponsavel(ref this.Conexao);
            Models.AlunoResponsavel responsavel = repResp.BuscarResponsavelPorBoleto(idBoleto);

            return responsavel;
        }

        /// <summary>
        /// Valida os atributos obrigatórios de uma Model.Aluno
        /// </summary>
        /// <param name="aluno">Objeto do tipo Model.Aluno</param>
        private void Validar(Models.Aluno aluno)
        {
            if (aluno == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }

            if (aluno.IdSituacao == 0)
            {
                throw new Prion.Tools.PrionException("A Situação tem que se informada.");
            }

            if (aluno.Nome.Trim() == "")
            {
                throw new Prion.Tools.PrionException("O nome do Aluno não pode ser vazio.");
            }

            if ((aluno.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (aluno.Id == 0))
            {
                throw new Prion.Tools.PrionException("O id do Aluno não pode ser vazio.");
            }
        }
    }
}