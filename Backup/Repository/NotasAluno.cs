using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Prion.Data;
using System.Data;
using Prion.Tools;
using System.Data.Common;

namespace NotaAzul.Repository
{
    public class NotasAluno:Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public NotasAluno(ref DBFacade conexao)
            : base(ref conexao)
        { 
        }

        ~NotasAluno()
        { 
        }

        /// <summary>
        /// Busca todas as Nota da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "MatriculaFormaDeAvaliacao.* ";
            String join = "FROM MatriculaFormaDeAvaliacao INNER JOIN FormaDeAvaliacao ON FormaDeAvaliacao.Id = MatriculaFormaDeAvaliacao.IdFormaAvaliacao "+
                        "INNER JOIN ComposicaoNota ON ComposicaoNota.Id = FormaDeAvaliacao.IdComposicaoNota "+
                        "INNER JOIN ComposicaoNotaPeriodo ON ComposicaoNotaPeriodo .Id =ComposicaoNota.IdComposicaoNotaPeriodo  "+
                        "INNER JOIN ProfessorDisciplina ON ProfessorDisciplina.Id = ComposicaoNotaPeriodo.IdProfessorDisciplina";
            String whereDefault = "";
            String orderBy = "MatriculaFormaDeAvaliacao.Id";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy, groupBy, parametro);
            List<Models.MatriculaFormaDeAvaliacao> listaMatriculaFormaDeAvaliacao = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaMatriculaFormaDeAvaliacao = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaMatriculaFormaDeAvaliacao != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaMatriculaFormaDeAvaliacao);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }

        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de Nota
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.MatriculaFormaDeAvaliacao com os registros do DataTable</returns>
        public List<Models.MatriculaFormaDeAvaliacao> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }


            List<Models.MatriculaFormaDeAvaliacao> lista = new List<Models.MatriculaFormaDeAvaliacao>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.Aluno
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Models.MatriculaFormaDeAvaliacao matriculaFormaDeAvaliacao = new Models.MatriculaFormaDeAvaliacao();

                matriculaFormaDeAvaliacao.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                matriculaFormaDeAvaliacao.IdMatricula = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdMatricula"].ToString());
                matriculaFormaDeAvaliacao.IdFormaDeAvaliacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdFormaAvaliacao"].ToString());
                matriculaFormaDeAvaliacao.ValorAlcancado = Conversor.ToDecimal(dataTable.Rows[i][nomeBase + "ValorAlcancado"].ToString());
                matriculaFormaDeAvaliacao.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                matriculaFormaDeAvaliacao.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(matriculaFormaDeAvaliacao);
                    continue;
                }               

                lista.Add(matriculaFormaDeAvaliacao);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }

        /// <summary>
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros sem WHERE
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista DataTable(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder campos = new StringBuilder(), join = new StringBuilder();
            campos.Append("Matricula.Id , Aluno.Nome ");
            join.Append(" FROM Aluno INNER JOIN Matricula  ON Matricula.IdAluno = Aluno.Id " +
                        " INNER JOIN MatriculaCurso ON Matricula.Id = MatriculaCurso.IdMatricula ");
            

            String whereDefault = "";
            String where = this.MontarWhere(parametro, whereDefault);
            String orderBy = "Aluno.Nome";
            String groupBy = "";

            return this.Select("SELECT " + campos.ToString(), join.ToString(), where, orderBy, groupBy, parametro);
        }

        /// <summary>
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros sem WHERE
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista DataTableFormaAvaliacao(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder campos = new StringBuilder(), join = new StringBuilder();
            campos.Append("FormaDeAvaliacao.* ,  FormaDeAvaliacao.Tipo + '-' + CONVERT(varchar(10), FormaDeAvaliacao.DataAvaliacao , 103) as Nome ");
            join.Append(" FROM FormaDeAvaliacao INNER JOIN ComposicaoNota ON ComposicaoNota.Id = FormaDeAvaliacao.IdComposicaoNota  " +
                        " INNER JOIN ComposicaoNotaPeriodo ON ComposicaoNotaPeriodo .Id =ComposicaoNota.IdComposicaoNotaPeriodo " +
                        " INNER JOIN ProfessorDisciplina ON ProfessorDisciplina.Id = ComposicaoNotaPeriodo.IdProfessorDisciplina ");
          

            String whereDefault = "";
            String where = this.MontarWhere(parametro, whereDefault);
            String orderBy = "FormaDeAvaliacao.Id";
            String groupBy = "";

            return this.Select("SELECT " + campos.ToString(), join.ToString(), where, orderBy, groupBy, parametro);
        }

        /// <summary>
        /// Insere um registro em Nota através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.FormaDeAvaliacao</param>
        /// <returns></returns>
        protected override int Inserir(Prion.Generic.Models.Base objeto)
        {
            Models.MatriculaFormaDeAvaliacao oMatriculaFormaDeAvaliacao = (Models.MatriculaFormaDeAvaliacao)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO MatriculaFormaDeAvaliacao(IdMatricula, IdFormaAvaliacao, ValorAlcancado) " +
                            "VALUES (@IdMatricula, @IdFormaAvaliacao, @ValorAlcancado)";

            parametros.Add(this.Conexao.CriarParametro("@IdMatricula", DbType.Int32, oMatriculaFormaDeAvaliacao.IdMatricula));
            parametros.Add(this.Conexao.CriarParametro("@IdFormaAvaliacao", DbType.Int32, oMatriculaFormaDeAvaliacao.IdFormaDeAvaliacao));
            parametros.Add(this.Conexao.CriarParametro("@ValorAlcancado", DbType.Decimal, oMatriculaFormaDeAvaliacao.ValorAlcancado));

            Int32 id = this.Conexao.Insert(sql, parametros);
            GravarLog(id, TipoOperacaoLog.Inserir);

            return id;
        }

        /// <summary>
        /// Altera um registro de Nota através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Nota</param>
        /// <returns></returns>
        protected override Int32 Alterar(Prion.Generic.Models.Base objeto)
        {
            Models.MatriculaFormaDeAvaliacao oMatriculaFormaDeAvaliacao = (Models.MatriculaFormaDeAvaliacao)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE MatriculaFormaDeAvaliacao SET IdMatricula=@IdMatricula, IdFormaAvaliacao=@IdFormaAvaliacao, ValorAlcancado=@ValorAlcancado " +
                            " WHERE Id =@Id ";

            parametros.Add(this.Conexao.CriarParametro("@IdMatricula", DbType.Int32, oMatriculaFormaDeAvaliacao.IdMatricula));
            parametros.Add(this.Conexao.CriarParametro("@IdFormaAvaliacao", DbType.Int32, oMatriculaFormaDeAvaliacao.IdFormaDeAvaliacao));
            parametros.Add(this.Conexao.CriarParametro("@ValorAlcancado", DbType.Decimal, oMatriculaFormaDeAvaliacao.ValorAlcancado));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, oMatriculaFormaDeAvaliacao.Id));

            Int32 id = this.Conexao.Update(sql, parametros);
            GravarLog(id, TipoOperacaoLog.Alterar);

            return id;
        }

        /// <summary>
        /// Grava o log de Nota
        /// </summary>
        /// <param name="idRegistro">id do registro</param>
        /// <param name="tipoOperacaoLog">tipo de operação: INSERIR, ALTERAR, EXCLUIR</param>
        /// <returns>true/false </returns>
        protected Boolean GravarLog(Int32 idRegistro, TipoOperacaoLog tipoOperacaoLog)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sqlLog = " if EXISTS (SELECT Id FROM MatriculaFormaDeAvaliacao WHERE Id = @Id) " +
                            " INSERT INTO MatriculaFormaDeAvaliacaoLog(IdUsuarioLog, IdOperacaoLog, Id, IdMatricula, IdFormaAvaliacao, ValorAlcancado, DataCadastro)" +
                            " SELECT @IdUsuarioLog, @IdOperacaoLog, @Id,IdMatricula, IdFormaAvaliacao, ValorAlcancado, DataCadastro FROM MatriculaFormaDeAvaliacao WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "MatriculaFormaDeAvaliacaoLog");
        }
    }
}