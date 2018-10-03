using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Prion.Data;
using Prion.Tools;
using System.Text;

namespace NotaAzul.Repository
{
    public class FormaDeAvaliacao:Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public FormaDeAvaliacao(ref DBFacade conexao)
            : base(ref conexao)
        { 
        }

        ~FormaDeAvaliacao()
        { 
        }

        /// <summary>
        /// Busca todas as formas de avaliação da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "FormaDeAvaliacao.* ";
            String join = "FROM FormaDeAvaliacao ";
            String whereDefault = "";
            String orderBy = "FormaDeAvaliacao.Id";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy, groupBy, parametro);
            List<Models.FormaDeAvaliacao> listaformasDeAvaliacao = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaformasDeAvaliacao = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaformasDeAvaliacao != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaformasDeAvaliacao);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }

        /// <summary>
        /// Retorna, através de um Id de nota, uma lista de objetos do tipo Models.FormaDeAvaliacao
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Prion.Generic.Models.Lista Buscar(Int32 id)
        {
            Prion.Tools.Request.ParametrosRequest parametro = new Prion.Tools.Request.ParametrosRequest();
            Prion.Tools.Request.Filtro f = new Request.Filtro("IdComposicaoNota", "=", id);

            parametro.Filtro.Add(f);

            return Buscar(parametro);
        }

        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de alunos
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.FormaDeAvaliacao com os registros do DataTable</returns>
        public List<Models.FormaDeAvaliacao> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }


            List<Models.FormaDeAvaliacao> lista = new List<Models.FormaDeAvaliacao>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.Aluno
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Models.FormaDeAvaliacao formaDeAvaliacao = new Models.FormaDeAvaliacao();

                formaDeAvaliacao.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                formaDeAvaliacao.IdComposicaoNota = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdComposicaoNota"].ToString());
                formaDeAvaliacao.Valor = Conversor.ToDecimal(dataTable.Rows[i][nomeBase + "Valor"].ToString());
                formaDeAvaliacao.Tipo = dataTable.Rows[i][nomeBase + "Tipo"].ToString();      
                formaDeAvaliacao.DataAvaliacao =Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataAvaliacao"].ToString());
                formaDeAvaliacao.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                formaDeAvaliacao.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(formaDeAvaliacao);
                    continue;
                }

                // carrega as demais entidades necessárias
                //formaDeAvaliacao = CarregarEntidades(Entidades, formaDeAvaliacao);

                lista.Add(formaDeAvaliacao);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }

        /// <summary>
        /// Exclui um registro de Forma de Avaliação através do Id
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

            String sql = "DELETE FROM FormaDeAvaliacao " +
                         " WHERE FormaDeAvaliacao.Id IN(" + strId + ")";

            retorno = this.Conexao.Delete(sql);
            return retorno;
        }

        /// <summary>
        /// Exclui um registro de FormaDeAvaliacao através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.FormaDeAvaliacao</param>
        /// <returns>Quantidade de registros excluídos</returns>
        protected override Int32 Excluir(Prion.Generic.Models.Base objeto)
        {
            Models.FormaDeAvaliacao oFormaDeAvaliacao = (Models.FormaDeAvaliacao)objeto;
            return Excluir(oFormaDeAvaliacao.Id);
        }

         /// <summary>
        /// Insere um registro em Forma de Avaliacao através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.FormaDeAvaliacao</param>
        /// <returns></returns>
        protected override int Inserir(Prion.Generic.Models.Base objeto)
        {
            Models.FormaDeAvaliacao oFormaDeAvaliacao = (Models.FormaDeAvaliacao)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO FormaDeAvaliacao(IdComposicaoNota, Tipo, Valor, DataAvaliacao) " +
                            "VALUES (@IdComposicaoNota, @Tipo, @Valor, @DataAvaliacao)";

            parametros.Add(this.Conexao.CriarParametro("@IdComposicaoNota", DbType.Int32, oFormaDeAvaliacao.IdComposicaoNota));
            parametros.Add(this.Conexao.CriarParametro("@Tipo", DbType.String, oFormaDeAvaliacao.Tipo));
            parametros.Add(this.Conexao.CriarParametro("@Valor", DbType.Decimal, oFormaDeAvaliacao.Valor));
            parametros.Add(this.Conexao.CriarParametro("@DataAvaliacao", DbType.DateTime, oFormaDeAvaliacao.DataAvaliacao));
            Int32 id = this.Conexao.Insert(sql, parametros);
            GravarLog(id, TipoOperacaoLog.Inserir);

            return id;
        }

         /// <summary>
        /// Altera um registro de Forma de Avaliacao através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.FormaDeAvaliacao</param>
        /// <returns></returns>
        protected override Int32 Alterar(Prion.Generic.Models.Base objeto)
        {
            Models.FormaDeAvaliacao oFormaDeAvaliacao = (Models.FormaDeAvaliacao)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE FormaDeAvaliacao SET IdComposicaoNota=@IdComposicaoNota, Tipo=@Tipo, Valor=@Valor, DataAvaliacao=@DataAvaliacao " +
                            "WHERE Id =@Id ";

            parametros.Add(this.Conexao.CriarParametro("@IdComposicaoNota", DbType.Int32, oFormaDeAvaliacao.IdComposicaoNota));
            parametros.Add(this.Conexao.CriarParametro("@Tipo", DbType.String, oFormaDeAvaliacao.Tipo));
            parametros.Add(this.Conexao.CriarParametro("@Valor", DbType.Decimal, oFormaDeAvaliacao.Valor));
            parametros.Add(this.Conexao.CriarParametro("@DataAvaliacao", DbType.DateTime, oFormaDeAvaliacao.DataAvaliacao));
            parametros.Add(this.Conexao.CriarParametro("@Id",DbType.Int32,oFormaDeAvaliacao.Id));

            Int32 id = this.Conexao.Update(sql, parametros);
            GravarLog(id, TipoOperacaoLog.Alterar);

            return id;
        }

        /// <summary>
        /// Grava o log de Aluno
        /// </summary>
        /// <param name="idRegistro">id do registro</param>
        /// <param name="tipoOperacaoLog">tipo de operação: INSERIR, ALTERAR, EXCLUIR</param>
        /// <returns>true/false </returns>
        protected Boolean GravarLog(Int32 idRegistro, TipoOperacaoLog tipoOperacaoLog)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sqlLog = " if EXISTS (SELECT Id FROM FormaDeAvaliacao WHERE Id = @Id) " +
                            " INSERT INTO FormaDeAvaliacaoLog(IdUsuarioLog, IdOperacaoLog, Id, IdComposicaoNota, Tipo, Valor, DataAvaliacao, DataCadastro)" +
                            " SELECT @IdUsuarioLog, @IdOperacaoLog, @Id, IdComposicaoNota, Tipo, Valor, DataAvaliacao, DataCadastro FROM FormaDeAvaliacao WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "FormaDeAvaliacaoLog");
        }
    }
}