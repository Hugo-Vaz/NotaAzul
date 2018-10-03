using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Prion.Data;
using Prion.Tools;
using System.Text;

namespace NotaAzul.Repository
{
    public class Disciplina : Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public Disciplina(ref DBFacade conexao)
            : base(ref conexao)
        {
        }


        ~Disciplina()
        {
        }


        /// <summary>
        /// Busca todas os Disicplina da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "Disciplina.* ";
            String join = "FROM Disciplina";
            String whereDefault = "";
            String orderBy = "Disciplina.Nome";
            String groupBy = "";

            if (Entidades.Carregar(Helpers.Entidade.Professor.ToString()))
            {
                campos += ", ProfessorDisciplina.IdTurma as IdTurma";
                join+=" INNER JOIN ProfessorDisciplina ON Disciplina.Id = ProfessorDisciplina.IdDisciplina";
            }

            if (Entidades.Carregar(Helpers.Entidade.Turma.ToString()))
            {
                join += " INNER JOIN DisciplinaTurma ON Disciplina.Id = DisciplinaTurma.IdDisciplina";
            }

            if (Entidades.Carregar(Helpers.Entidade.MatriculaCurso.ToString()))
            {
                join += " INNER JOIN MatriculaCurso ON MatriculaCurso.IdTurma = DisciplinaTurma.IdTurma "+
                    " INNER JOIN Matricula on MatriculaCurso.IdMatricula = Matricula.Id";
            }

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy,groupBy, parametro);
            List<Models.Disciplina> listaDisciplina = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaDisciplina = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaDisciplina != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaDisciplina);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }


        /// <summary>
        /// Retorna, através de um (ou vários) ID, uma lista de objetos do tipo Models.Disciplina
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Prion.Generic.Models.Lista Buscar(params Int32[] ids)
        {
            Prion.Tools.Request.ParametrosRequest parametro = new Prion.Tools.Request.ParametrosRequest();
            Prion.Tools.Request.Filtro f = new Request.Filtro("Id", "IN", Conversor.ToString(",", ids));

            parametro.Filtro.Add(f);

            return Buscar(parametro);
        }

        /// <summary>
        /// Cria o relacionamento entre um professor e uma disciplina
        /// </summary>
        /// <param name="idDisciplina"></param>
        /// <returns></returns>
        public void CriarRelacionamentoProfessorDisciplina(Models.ProfessorDisciplina professorDisciplina)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO ProfessorDisciplina(IdDisciplina, IdFuncionario, IdTurma ,IdSituacao) VALUES(@IdDisciplina, @IdFuncionario,@IdTurma, @IdSituacao)";

            parametros.Add(this.Conexao.CriarParametro("@IdDisciplina", DbType.Int32, professorDisciplina.IdDisciplina));
            parametros.Add(this.Conexao.CriarParametro("@IdFuncionario", DbType.Int32, professorDisciplina.IdProfessor));
            parametros.Add(this.Conexao.CriarParametro("@IdTurma", DbType.Int32, professorDisciplina.IdTurma));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, professorDisciplina.IdSituacao));

            Int32 id = this.Conexao.Insert(sql, parametros);

            GravarLogRelacionamento(id, TipoOperacaoLog.Inserir);
          

        }

        /// <summary>
        /// Apaga o relacionamento entre um professor e uma disciplina
        /// </summary>
        /// <param name="idDisciplina"></param>
        /// <returns></returns>
        public void ExcluirRelacionamentoProfessorDisciplina(Models.ProfessorDisciplina professorDisciplina)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "DELETE FROM ProfessorDisciplina WHERE (IdDisciplina=@IdDisciplina AND IdFuncionario=@IdFuncionario AND IdTurma=@IdTurma)";

            parametros.Add(this.Conexao.CriarParametro("@IdDisciplina", DbType.Int32, professorDisciplina.IdDisciplina));
            parametros.Add(this.Conexao.CriarParametro("@IdFuncionario", DbType.Int32, professorDisciplina.IdProfessor));
            parametros.Add(this.Conexao.CriarParametro("@IdTurma", DbType.Int32, professorDisciplina.IdTurma));

            Int32 id = this.Conexao.Delete(sql, parametros);

            GravarLogRelacionamento(id, TipoOperacaoLog.Excluir);


        }

        /// <summary>
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros sem WHERE
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista DataTable(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder campos = new StringBuilder(), join = new StringBuilder();
            campos.Append("Disciplina.* ");
            join.Append(" FROM Disciplina");

            if (Entidades.Carregar(Helpers.Entidade.Situacao.ToString()))
            {
                campos.Append(", " + Prion.Generic.Helpers.MapeamentoTabela.Situacao());
                join.Append(" INNER JOIN Situacao ON Disciplina.IdSituacao = Situacao.Id ");
            }

            if (Entidades.Carregar(Helpers.Entidade.Professor.ToString()))
            {
                join.Append(" INNER JOIN ProfessorDisciplina ON Disciplina.Id = ProfessorDisciplina.IdDisciplina ");
            }

            if (Entidades.Carregar(Helpers.Entidade.Funcionario.ToString()))
            {
                campos.Append(" ,ProfessorDisciplina.Id as IdProfessorDisciplina,ProfessorDisciplina.IdTurma as IdTurma ");
                join.Append(" INNER JOIN FuncionarioUsuario ON ProfessorDisciplina.IdFuncionario = FuncionarioUsuario.IdFuncionario ");
                join.Append(" INNER JOIN Usuario ON Usuario.Id = FuncionarioUsuario.IdUsuario ");
            }

            if (Entidades.Carregar(Helpers.Entidade.Turma.ToString()))
            {
                join.Append(" INNER JOIN DisciplinaTurma ON Disciplina.Id = DisciplinaTurma.IdDisciplina ");
            }

            String whereDefault = "";
            String orderBy = "Disciplina.Nome";
            String groupBy = "";

            return this.Select("SELECT " + campos.ToString(), join.ToString(), whereDefault, orderBy,groupBy, parametro);
        }


        /// <summary>
        /// Insere um registro em Disciplina através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Disciplina</param>
        /// <returns></returns>
        protected override Int32 Inserir(Prion.Generic.Models.Base objeto)
        {
            Models.Disciplina oDisciplina = (Models.Disciplina)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO Disciplina(IdCorporacao, IdSituacao, Nome) VALUES(@IdCorporacao, @IdSituacao, @Nome)";

            parametros.Add(this.Conexao.CriarParametro("@IdCorporacao", DbType.Int32, UsuarioLogado.IdCorporacao));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, oDisciplina.IdSituacao));
            parametros.Add(this.Conexao.CriarParametro("@Nome", DbType.String, StringHelper.OnlyOneSpace(oDisciplina.Nome), 100));
            
            Int32 id = this.Conexao.Insert(sql, parametros);

            GravarLog(id, TipoOperacaoLog.Inserir);

            return id;
        }


        /// <summary>
        /// Altera um registro de Disciplina através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Disciplina</param>
        /// <returns></returns>
        protected override Int32 Alterar(Prion.Generic.Models.Base objeto)
        {
            Models.Disciplina oDisciplina = (Models.Disciplina)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE Disciplina SET IdSituacao=@IdSituacao, Nome=@Nome WHERE Id=@Id";

            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, oDisciplina.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, oDisciplina.IdSituacao));
            parametros.Add(this.Conexao.CriarParametro("@Nome", DbType.String, StringHelper.OnlyOneSpace(oDisciplina.Nome), 100));

            Int32 linhasAfetadas = this.Conexao.Update(sql, parametros);

            GravarLog(oDisciplina.Id, TipoOperacaoLog.Alterar);

            return linhasAfetadas;
        }


        /// <summary>
        /// Exclui um registro de Disciplina através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Disciplina</param>
        /// <returns>Quantidade de registros excluídos</returns>
        protected override Int32 Excluir(Prion.Generic.Models.Base objeto)
        {
            Models.Disciplina oDisciplina = (Models.Disciplina)objeto;
            return Excluir(oDisciplina.Id);
        }


        /// <summary>
        /// Exclui um registro de Disciplina através do Id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>Quantidade de registros excluídos</returns>
        public Int32 Excluir(params Int32[] ids)
        {
            // se o parâmetro for null, retorna 0, informando que não excluiu nenhum registro
            if (ids == null) { return 0; }

            for (Int32 indice = 0; indice < ids.Length; indice++)
            {
                GravarLog(ids[indice], TipoOperacaoLog.Excluir);
            }

            Int32 retorno = -1;
            String strId = Conversor.ToString(",", ids);
            String sql = "DELETE FROM Disciplina WHERE Id IN(" + strId + ")";

            retorno = this.Conexao.Delete(sql);
            return retorno;
        }


        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de Disciplina
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.Disciplina com os registros do DataTable</returns>
        public List<Models.Disciplina> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }


            List<Models.Disciplina> lista = new List<Models.Disciplina>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.Disciplina
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Models.Disciplina disciplina = new Models.Disciplina();

                disciplina.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                disciplina.IdCorporacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdCorporacao"].ToString());
                disciplina.IdSituacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdSituacao"].ToString());
                disciplina.Nome = dataTable.Rows[i][nomeBase + "Nome"].ToString();
                disciplina.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                disciplina.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                //Utilizado apenas para carregar as disciplinas de um professor(Tela Professor)
                if (Entidades.Carregar(Helpers.Entidade.Professor.ToString()))
                {
                    disciplina.IdTurma = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdTurma"].ToString());
                }
                
                
                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(disciplina);
                    continue;
                }

                // carrega as demais entidades necessárias
                disciplina = CarregarEntidades(Entidades, disciplina);

                lista.Add(disciplina);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }


        /// <summary>
        /// Carrega as demais entidades relacionadas a este objeto
        /// </summary>
        /// <param name="modulos">lista de módulos que serão utilizados para carregar as demais entidades</param>
        /// <param name="objTurno">Objeto do tipo Models.Base que será convertido em Models.Disciplina</param>
        protected Models.Disciplina CarregarEntidades(Prion.Tools.Entidade modulos, Prion.Generic.Models.Base objDisciplina)
        {
            Boolean modoCompleto = Entidades.Carregar(Helpers.Entidade.Tudo.ToString());
            Models.Disciplina disciplina = (Models.Disciplina)objDisciplina;


            // verifica se possui o módulo COMPLETO ou se possui o módulo CORPORACAO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Corporacao.ToString())))
            {
                Prion.Generic.Repository.Corporacao repCorporacao = new Prion.Generic.Repository.Corporacao(ref this._conexao);
                repCorporacao.Entidades = modulos;

                // carrega um objeto do tipo Models.Corporacao
                disciplina.Corporacao = repCorporacao.BuscarPeloId(disciplina.IdCorporacao);
            }

            // verifica se possui o módulo COMPLETO ou se possui o módulo SITUACAO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Situacao.ToString())))
            {
                Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this._conexao);
                repSituacao.Entidades = modulos;

                // carrega um objeto do tipo Models.Situacao, representando a situação do CursoAnoLetivo
                disciplina.Situacao = (Prion.Generic.Models.Situacao)repSituacao.BuscarPeloId(disciplina.IdSituacao).Get(0);
            }

            // verifica se possui o módulo COMPLETO ou se possui o módulo FUNCIONARIO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Funcionario.ToString())))
            {
                Prion.Generic.Repository.Funcionario repFuncionario = new Prion.Generic.Repository.Funcionario(ref this._conexao);
                repFuncionario.Entidades = modulos;

                // carrega uma lista de objetos do tipo Models.Funcionario, para retornar um lista de Professores
                //THIAGO_ACERTAR disciplina.Professores = (Prion.Generic.Models.Funcionario)repFuncionario;
            }


            // retorna o objeto de Disciplina com as entidades que foram carregadas
            return disciplina;
        }


        /// <summary>
        /// Grava o log de Disciplina
        /// </summary>
        /// <param name="idRegistro">id do registro</param>
        /// <param name="tipoOperacaoLog">tipo de operação: INSERIR, ALTERAR, EXCLUIR</param>
        /// <returns>true/false </returns>
        protected Boolean GravarLog(Int32 idRegistro, TipoOperacaoLog tipoOperacaoLog)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sqlLog = " if EXISTS (SELECT Id FROM Disciplina WHERE Id = @Id) " +
                            " INSERT INTO DisciplinaLog(IdUsuarioLog, IdOperacaoLog, Id, IdCorporacao, IdSituacao, Nome, DataCadastro)" +
                            " SELECT @IdUsuarioLog, @IdOperacaoLog, @Id, IdCorporacao, IdSituacao, Nome, DataCadastro FROM Disciplina WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "DisciplinaLog");
        }

        /// <summary>
        /// Grava o log de ProfessorDisciplina
        /// </summary>
        /// <param name="idRegistro">id do registro</param>
        /// <param name="tipoOperacaoLog">tipo de operação: INSERIR, ALTERAR, EXCLUIR</param>
        /// <returns>true/false </returns>
        protected Boolean GravarLogRelacionamento(Int32 idRegistro, TipoOperacaoLog tipoOperacaoLog)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sqlLog = " if EXISTS (SELECT Id FROM ProfessorDisciplina WHERE Id = @Id) " +
                            " INSERT INTO ProfessorDisciplinaLog(IdUsuarioLog, IdOperacaoLog, Id, IdDisciplina, IdFuncionario,IdTurma, IdSituacao, DataCadastro)" +
                            " SELECT @IdUsuarioLog, @IdOperacaoLog, @Id, IdDisciplina, IdFuncionario,IdTurma, IdSituacao, DataCadastro FROM ProfessorDisciplina WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "ProfessorDisciplinaLog");
        }
    }
}