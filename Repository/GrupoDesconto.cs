using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Prion.Data;
using Prion.Tools;

namespace NotaAzul.Repository
{
    public class GrupoDesconto:Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>

        public GrupoDesconto(ref DBFacade conexao)
            :base(ref conexao)
        {
        }

        ~GrupoDesconto()
        {
        }


        /// <summary>
        /// Busca todas os GrupoDesconto da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            String campos = "GrupoDesconto.* ";
            String join = "FROM GrupoDesconto ";
            String whereDefault = "";
            String orderBy = "GrupoDesconto.Nome";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy,groupBy, parametro);
            List<Models.GrupoDesconto> listaGrupoDesconto = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaGrupoDesconto= this.DataTableToObject(lista.DataTable);

            if (listaGrupoDesconto != null)
            {
                listaBase = ListTo.CollectionToList <Prion.Generic.Models.Base> (listaGrupoDesconto);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }

        /// <summary>
        /// Retorna, através de um (ou vários) ID, uma lista de objetos do tipo Models.GrupoDesconto
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
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros sem WHERE
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista DataTable(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder campos = new StringBuilder(), join = new StringBuilder();
            campos.Append("GrupoDesconto.* ");
            join.Append(" FROM GrupoDesconto");

            if (Entidades.Carregar(Helpers.Entidade.Situacao.ToString()))
            {
                campos.Append(", " + Prion.Generic.Helpers.MapeamentoTabela.Situacao());
                join.Append(" INNER JOIN Situacao ON GrupoDesconto.IdSituacao = Situacao.Id ");
            }

            String whereDefault = "";
            String orderBy = "GrupoDesconto.Nome";
            String groupBy = "";

            return this.Select("SELECT " + campos.ToString(), join.ToString(), whereDefault, orderBy,groupBy, parametro);
        }

        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de GrupoDesconto
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.GrupoDesconto com os registros do DataTable</returns>
        public List<Models.GrupoDesconto> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }


            List<Models.GrupoDesconto> lista = new List<Models.GrupoDesconto>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.GrupoDesconto
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Models.GrupoDesconto grupoDesconto= new Models.GrupoDesconto();

                grupoDesconto.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                grupoDesconto.IdSituacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdSituacao"].ToString());
                grupoDesconto.Nome = dataTable.Rows[i][nomeBase + "Nome"].ToString();
                grupoDesconto.Descricao = dataTable.Rows[i][nomeBase + "Descricao"].ToString();
                grupoDesconto.Valor = Conversor.ToDecimal(dataTable.Rows[i][nomeBase + "Valor"].ToString());
                grupoDesconto.TipoDesconto = dataTable.Rows[i][nomeBase + "TipoDesconto"].ToString();
                grupoDesconto.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                grupoDesconto.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(grupoDesconto);
                    continue;
                }

                // carrega as demais entidades necessárias
                grupoDesconto = CarregarEntidades(Entidades, grupoDesconto);

                lista.Add(grupoDesconto);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }

        /// <summary>
        /// Exclui um registro de GrupoDesconto através do Id
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
            String sql = "DELETE FROM GrupoDesconto WHERE Id IN (" + strId + ")";

            retorno = this.Conexao.Delete(sql);

            return retorno;
        }

        /// <summary>
        /// Insere um registro em GrupoDesconto através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.GrupoDesconto</param>
        /// <returns></returns>
        protected override int Inserir(Prion.Generic.Models.Base objeto)
        {
            Models.GrupoDesconto oGrupoDesconto = (Models.GrupoDesconto)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO GrupoDesconto(IdSituacao, Nome, Descricao, Valor,TipoDesconto) " +
                            "VALUES (@IdSituacao, @Nome, @Descricao, @Valor,@TipoDesconto)";

            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, oGrupoDesconto.IdSituacao));
            parametros.Add(this.Conexao.CriarParametro("@Nome", DbType.String, StringHelper.OnlyOneSpace(oGrupoDesconto.Nome), 50));
            parametros.Add(this.Conexao.CriarParametro("@Descricao", DbType.String, oGrupoDesconto.Descricao, 255));
            parametros.Add(this.Conexao.CriarParametro("@Valor", DbType.Decimal, oGrupoDesconto.Valor));
            parametros.Add(this.Conexao.CriarParametro("@TipoDesconto", DbType.String, oGrupoDesconto.TipoDesconto, 50));

            Int32 id = this.Conexao.Insert(sql, parametros);
            GravarLog(id, TipoOperacaoLog.Inserir);

            return id;
        }

        /// <summary>
        /// Altera um registro de GrupoDesconto através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.GrupoDesconto</param>
        /// <returns></returns>
        protected override Int32 Alterar(Prion.Generic.Models.Base objeto)
        {
            Models.GrupoDesconto oGrupoDesconto = (Models.GrupoDesconto)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE GrupoDesconto SET IdSituacao=@IdSituacao, Nome=@Nome, Descricao=@Descricao, Valor=@Valor,TipoDesconto=@TipoDesconto " +
                        "WHERE Id=@Id";

            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, oGrupoDesconto.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, oGrupoDesconto.IdSituacao));
            parametros.Add(this.Conexao.CriarParametro("@Nome", DbType.String, StringHelper.OnlyOneSpace(oGrupoDesconto.Nome), 50));
            parametros.Add(this.Conexao.CriarParametro("@Descricao", DbType.String, oGrupoDesconto.Descricao, 255));
            parametros.Add(this.Conexao.CriarParametro("@Valor", DbType.Decimal, oGrupoDesconto.Valor));
            parametros.Add(this.Conexao.CriarParametro("@TipoDesconto", DbType.String, oGrupoDesconto.TipoDesconto, 50));
            Int32 linhasAfetadas = this.Conexao.Update(sql, parametros);

            GravarLog(oGrupoDesconto.Id, TipoOperacaoLog.Alterar);

            return linhasAfetadas;
        }

        /// <summary>
        /// Exclui um registro de GrupoDesconto através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.GrupoDesconto</param>
        /// <returns>Quantidade de registros excluídos</returns>
        protected override Int32 Excluir(Prion.Generic.Models.Base objeto)
        {
            Models.GrupoDesconto oGrupoDesconto = (Models.GrupoDesconto)objeto;
            return Excluir(oGrupoDesconto.Id);
        }

        /// <summary>
        /// Carrega as demais entidades relacionadas a este objeto
        /// </summary>
        /// <param name="modulos">lista de módulos que serão utilizados para carregar as demais entidades</param>
        /// <param name="objTurno">Objeto do tipo Models.Base que será convertido em Models.GrupoDesconto</param>
        protected Models.GrupoDesconto CarregarEntidades(Prion.Tools.Entidade modulos, Prion.Generic.Models.Base objGrupoDesconto)
        {
            Boolean modoCompleto = Entidades.Carregar(Helpers.Entidade.Tudo.ToString());
            Models.GrupoDesconto grupoDesconto = (Models.GrupoDesconto)objGrupoDesconto;


            // verifica se possui o módulo COMPLETO ou se possui o módulo SITUACAO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Situacao.ToString())))
            {
                Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this._conexao);
                repSituacao.Entidades = modulos;

                // carrega um objeto do tipo Models.Situacao, representando a situação do GrupoDesconto
                grupoDesconto.Situacao = (Prion.Generic.Models.Situacao)repSituacao.BuscarPeloId(grupoDesconto.IdSituacao).Get(0);
            }

            return grupoDesconto;
        }

        /// <summary>
        /// Grava o log de GrupoDesconto
        /// </summary>
        /// <param name="idRegistro">id do registro</param>
        /// <param name="tipoOperacaoLog">tipo de operação: INSERIR, ALTERAR, EXCLUIR</param>
        /// <returns>true/false </returns>
        protected Boolean GravarLog(Int32 idRegistro, TipoOperacaoLog tipoOperacaoLog)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sqlLog = " if EXISTS (SELECT Id FROM GrupoDesconto WHERE Id = @Id) " +
                            " INSERT INTO GrupoDescontoLog(IdUsuarioLog, IdOperacaoLog, Id, IdSituacao, " +
                                "Nome, Descricao, Valor,TipoDesconto, DataCadastro)" +
                            " SELECT @IdUsuarioLog, @IdOperacaoLog, @Id, IdSituacao," +
                                "Nome, Descricao, Valor,TipoDesconto, DataCadastro FROM GrupoDesconto WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "GrupoDescontoLog");
        }

    }
}