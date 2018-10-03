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
    public class MatriculaNota:Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public MatriculaNota(ref DBFacade conexao)
            : base(ref conexao)
        { 
        }

        ~MatriculaNota()
        { 
        }


        /// <summary>
        /// Busca todas as Nota da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "MatriculaNota.* ";
            String join = "FROM MatriculaNota INNER JOIN ComposicaoNota ON ComposicaoNota.Id = MatriculaNota.IdComposicaoNota" +
                        " INNER JOIN ComposicaoNotaPeriodo ON ComposicaoNotaPeriodo.Id=ComposicaoNota.IdComposicaoNotaPeriodo "+
                        " INNER JOIN ProfessorDisciplina ON ProfessorDisciplina.Id = ComposicaoNotaPeriodo.IdProfessorDisciplina";
            String whereDefault = "";
            String orderBy = "MatriculaNota.Id";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy, groupBy, parametro);
            List<Models.MatriculaNota> listaMatriculaNota = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaMatriculaNota = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaMatriculaNota != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaMatriculaNota);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }

        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de Nota
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.MatriculaNota com os registros do DataTable</returns>
        public List<Models.MatriculaNota> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }


            List<Models.MatriculaNota> lista = new List<Models.MatriculaNota>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.Aluno
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Models.MatriculaNota matriculaNota = new Models.MatriculaNota();

                matriculaNota.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                matriculaNota.IdMatricula = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdMatricula"].ToString());
                matriculaNota.IdComposicaoNota = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdComposicaoNota"].ToString());
                matriculaNota.ValorAlcancado = Conversor.ToDecimal(dataTable.Rows[i][nomeBase + "ValorAlcancado"].ToString());
                matriculaNota.ValorFinal = Conversor.ToDecimal(dataTable.Rows[i][nomeBase + "ValorFinal"].ToString());
                matriculaNota.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                matriculaNota.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(matriculaNota);
                    continue;
                }
               
                lista.Add(matriculaNota);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }

        /// <summary>
        /// Insere um registro em Nota através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.FormaDeAvaliacao</param>
        /// <returns></returns>
        protected override int Inserir(Prion.Generic.Models.Base objeto)
        {
            Models.MatriculaNota oMatriculaNota = (Models.MatriculaNota)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO MatriculaNota(IdMatricula, IdComposicaoNota, ValorAlcancado, ValorFinal) " +
                            "VALUES (@IdMatricula, @IdComposicaoNota, @ValorAlcancado, @ValorFinal)";

            parametros.Add(this.Conexao.CriarParametro("@IdMatricula", DbType.Int32, oMatriculaNota.IdMatricula));
            parametros.Add(this.Conexao.CriarParametro("@IdComposicaoNota", DbType.Int32, oMatriculaNota.IdComposicaoNota));
            parametros.Add(this.Conexao.CriarParametro("@ValorAlcancado", DbType.Decimal, oMatriculaNota.ValorAlcancado));
            parametros.Add(this.Conexao.CriarParametro("@ValorFinal", DbType.Decimal, oMatriculaNota.ValorFinal));

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
            Models.MatriculaNota oMatriculaMedia = (Models.MatriculaNota)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE MatriculaNota SET IdMatricula=@IdMatricula, IdComposicaoNota=@IdComposicaoNota, ValorAlcancado=@ValorAlcancado, ValorFinal=@ValorFinal " +
                            " WHERE Id =@Id ";

            parametros.Add(this.Conexao.CriarParametro("@IdMatricula", DbType.Int32, oMatriculaMedia.IdMatricula));
            parametros.Add(this.Conexao.CriarParametro("@IdComposicaoNota", DbType.Int32, oMatriculaMedia.IdComposicaoNota));
            parametros.Add(this.Conexao.CriarParametro("@ValorAlcancado", DbType.Decimal, oMatriculaMedia.ValorAlcancado));
            parametros.Add(this.Conexao.CriarParametro("@ValorFinal", DbType.Decimal, oMatriculaMedia.ValorFinal));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, oMatriculaMedia.Id));

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
            String sqlLog = " if EXISTS (SELECT Id FROM MatriculaNota WHERE Id = @Id) " +
                            " INSERT INTO MatriculaNotaLog(IdUsuarioLog, IdOperacaoLog, Id, IdMatricula, IdComposicaoNota, ValorAlcancado, ValorFinal, DataCadastro)" +
                            " SELECT @IdUsuarioLog, @IdOperacaoLog, @Id,IdMatricula, IdComposicaoNota, ValorAlcancado, ValorFinal, DataCadastro FROM MatriculaNota WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "MatriculaNotaLog");
        }
    }
}