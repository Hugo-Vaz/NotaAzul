using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Prion.Data;
using Prion.Tools;
using System.Text;

namespace NotaAzul.Repository
{
    public class MovimentacaoFinanceira : Prion.Generic.Repository.Base
    {
         /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public MovimentacaoFinanceira(ref DBFacade conexao)
            : base(ref conexao)
        {
        }


        ~MovimentacaoFinanceira()
        {
        }


        /// <summary>
        /// Busca todas as MovimentacoesFinanceiras da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "MovimentacaoFinanceira.* ";
            String join = "FROM MovimentacaoFinanceira ";
            String whereDefault = "";
            String orderBy = "MovimentacaoFinanceira.Id";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy,groupBy, parametro);
            List<Models.MovimentacaoFinanceira> listaMovimentacaoFinanceira = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaMovimentacaoFinanceira = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaMovimentacaoFinanceira != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaMovimentacaoFinanceira);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }

        /// <summary>
        /// Busca todas as MovimentacoesFinanceiras da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista BuscarMovimentacoesConcluidasDeUmTitulo(Int32 idSituacao,Int32 idTitulo)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "MovimentacaoFinanceira.* ";
            String join = "FROM MovimentacaoFinanceira INNER JOIN MovimentacaoFinanceiraTitulo ON MovimentacaoFinanceira.Id = MovimentacaoFinanceiraTitulo.IdMovimentacaoFinanceira ";
            String whereDefault = "";
            String orderBy = "MovimentacaoFinanceira.Id";
            String groupBy = "";

            //Busca as movimentações de acordo com o relacionamento entre movimentação financeira e título
            Prion.Tools.Request.ParametrosRequest parametro = new Prion.Tools.Request.ParametrosRequest();
            Prion.Tools.Request.Filtro f = new Request.Filtro("IdSituacao", "IN",  idSituacao.ToString());
            parametro.Filtro.Add(f);

            f = new Request.Filtro("IdTitulo", "IN", idTitulo.ToString());
            parametro.Filtro.Add(f);


            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy,groupBy, parametro);
            List<Models.MovimentacaoFinanceira> listaMovimentacaoFinanceira = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaMovimentacaoFinanceira = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaMovimentacaoFinanceira != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaMovimentacaoFinanceira);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }

        /// <summary>
        /// Retorna, através de um (ou vários) ID, uma lista de objetos do tipo Models.MovimentacaoFinanceira
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
        /// Insere um registro em MovimentacaoFinanceira através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.MovimentacaoFinanceira</param>
        /// <returns></returns>
        protected override Int32 Inserir(Prion.Generic.Models.Base objeto)
        {
            Models.MovimentacaoFinanceira oMovimentacaoFinanceira = (Models.MovimentacaoFinanceira)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO MovimentacaoFinanceira( Valor) VALUES( @Valor)";

            parametros.Add(this.Conexao.CriarParametro("@Valor", DbType.Decimal, oMovimentacaoFinanceira.Valor));
            

            Int32 id = this.Conexao.Insert(sql, parametros);

            GravarLog(id, TipoOperacaoLog.Inserir);

            return id;
        }


        /// <summary>
        /// Altera um registro de MovimentacaoFinanceira através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.MovimentacaoFinanceira</param>
        /// <returns></returns>
        protected override Int32 Alterar(Prion.Generic.Models.Base objeto)
        {
            Models.MovimentacaoFinanceira oMovimentacaoFinanceira = (Models.MovimentacaoFinanceira)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE MovimentacaoFinanceira SET Valor=@Valor WHERE Id=@Id";

            parametros.Add(this.Conexao.CriarParametro("@Id",DbType.Int32,oMovimentacaoFinanceira.Id));
            parametros.Add(this.Conexao.CriarParametro("@Valor", DbType.Decimal, oMovimentacaoFinanceira.Valor));
           ;

            Int32 linhasAfetadas = this.Conexao.Update(sql, parametros);

            GravarLog(oMovimentacaoFinanceira.Id, TipoOperacaoLog.Alterar);

            return linhasAfetadas;
        }


        /// <summary>
        /// Exclui um registro de MovimentacaoFinanceira através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.MovimentacaoFinanceira</param>
        /// <returns>Quantidade de registros excluídos</returns>
        protected override Int32 Excluir(Prion.Generic.Models.Base objeto)
        {
            Models.MovimentacaoFinanceira oMovimentacaoFinanceira = (Models.MovimentacaoFinanceira)objeto;
            return Excluir(oMovimentacaoFinanceira.Id);
        }


        /// <summary>
        /// Exclui um registro de MovimentacaoFinanceira através do Id
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
            String sql = "DELETE FROM MovimentacaoFinanceira WHERE Id IN(" + strId + ")";

            retorno = this.Conexao.Delete(sql);
            return retorno;
        }


        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de MovimentacaoFinanceira
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.MovimentacaoFinanceira com os registros do DataTable</returns>
        public List<Models.MovimentacaoFinanceira> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }


            List<Models.MovimentacaoFinanceira> lista = new List<Models.MovimentacaoFinanceira>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.MovimentacaoFinanceira
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Models.MovimentacaoFinanceira movimentacaoFinanceira = new Models.MovimentacaoFinanceira();
                movimentacaoFinanceira.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());                
                movimentacaoFinanceira.Valor = Conversor.ToDecimal(dataTable.Rows[i][nomeBase + "Valor"].ToString());               
                movimentacaoFinanceira.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                movimentacaoFinanceira.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(movimentacaoFinanceira);
                    continue;
                }

                // carrega as demais entidades necessárias
                movimentacaoFinanceira = CarregarEntidades(Entidades, movimentacaoFinanceira);

                lista.Add(movimentacaoFinanceira);
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
            campos.Append("MovimentacaoFinanceira.* ");
            join.Append(" FROM MovimentacaoFinanceira");
                       

            String whereDefault = "";
            String where = this.MontarWhere(parametro, whereDefault);
            String orderBy = "MovimentacaoFinanceira.DataCadastro";
            String groupBy = "";

            return this.Select("SELECT " + campos.ToString(), join.ToString(), where, orderBy,groupBy, parametro);
        }

        /// <summary>
        /// Carrega as demais entidades relacionadas a este objeto
        /// </summary>
        /// <param name="modulos">lista de módulos que serão utilizados para carregar as demais entidades</param>
        /// <param name="objTurno">Objeto do tipo Models.Base que será convertido em Models.MovimentacaoFinanceira</param>
        protected Models.MovimentacaoFinanceira CarregarEntidades(Prion.Tools.Entidade modulos, Prion.Generic.Models.Base objMovimentacaoFinanceira)
        {
            Boolean modoCompleto = Entidades.Carregar(Helpers.Entidade.Tudo.ToString());
            Models.MovimentacaoFinanceira movimentacaoFinanceira = (Models.MovimentacaoFinanceira)objMovimentacaoFinanceira;

                 

            // retorna o objeto de MovimentacaoFinanceira com as entidades que foram carregadas
            return movimentacaoFinanceira;
        }

        /// <summary>
        /// Cria o relacionamento entre Movimentação Financeira e Título
        /// </summary>
        /// <param name="IdMovimentacaoFinanceira"></param>
        /// <param name="idTitulo"></param>
        /// <param name="IdSituacao"></param>
        /// <returns>id da chave primária</returns>
        public Int32 CriarRelacionamento(Int32 idMovimentacaoFinanceira, Int32 idTitulo,Int32 idSituacao)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO MovimentacaoFinanceiraTitulo(IdMovimentacaoFinanceira, IdTitulo,IdSituacao) " +
                        "VALUES(@IdMovimentacaoFinanceira, @IdTitulo,@IdSituacao)";

            parametros.Add(this.Conexao.CriarParametro("@IdMovimentacaoFinanceira", DbType.Int32, idMovimentacaoFinanceira));
            parametros.Add(this.Conexao.CriarParametro("@IdTitulo", DbType.Int32, idTitulo));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, idSituacao ));

            Int32 id = this.Conexao.Insert(sql, parametros);
            return id;
        }

        /// <summary>
        /// Atualiza a situação do relacionamento entre Movimentação Financeira e Título
        /// </summary>
        /// <param name="IdMovimentacaoFinanceira"></param>
        /// <param name="idTitulo"></param>
        /// <param name="IdSituacao"></param>       
        /// <returns>id da chave primária</returns>
        public Int32 AtualizarRelacionamento(Int32 idMovimentacaoFinanceira, Int32 idTitulo, Int32 idSituacao)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE MovimentacaoFinanceiraTitulo SET IdSituacao=@IdSituacao " +
                        "WHERE IdMovimentacaoFinanceira=@IdMovimentacaoFinanceira AND IdTitulo=@IdTitulo";

            parametros.Add(this.Conexao.CriarParametro("@IdMovimentacaoFinanceira", DbType.Int32, idMovimentacaoFinanceira));
            parametros.Add(this.Conexao.CriarParametro("@IdTitulo", DbType.Int32, idTitulo));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, idSituacao));

            Int32 id = this.Conexao.Update(sql, parametros);
            return id;
        }

        /// <summary>
        /// Cria o relacionamento entre Cartão e Título
        /// </summary>
        /// <param name="idCartao"></param>
        /// <param name="idTitulo"></param>
        /// <param name="IdSituacao"></param>   
        /// <returns>id da chave primária</returns>
        public Int32 CriarRelacionamentoCartao(Int32 idCartao, Int32 idTitulo, Int32 idSituacao)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO TituloCartao(IdCartao, IdTitulo,IdSituacao) " +
                        "VALUES(@IdCartao, @IdTitulo,@IdSituacao)";

            parametros.Add(this.Conexao.CriarParametro("@IdCartao", DbType.Int32, idCartao));
            parametros.Add(this.Conexao.CriarParametro("@IdTitulo", DbType.Int32, idTitulo));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, idSituacao));

            Int32 id = this.Conexao.Insert(sql, parametros);
            return id;
        }
        
        /// <summary>
        /// Carrega os ids de cartão de determinado Título
        /// </summary>       
        /// <param name="idTitulo"></param>
        /// <param name="IdSituacao"></param>   
        /// <returns>ids dos cartões utilizados para efetuar o pagamento de um título</returns>
        public Int32[] CarregarRelacionamentoCartao(Int32 idTitulo, Int32 idSituacao)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "TituloCartao.* ";
            String join = "FROM TituloCartao ";
            String whereDefault = "";
            String orderBy = "TituloCartao.IdCartao";
            String groupBy = "";

            //Busca as movimentações de acordo com o relacionamento entre movimentação financeira e título
            Prion.Tools.Request.ParametrosRequest parametro = new Prion.Tools.Request.ParametrosRequest();
            Prion.Tools.Request.Filtro f = new Request.Filtro("IdSituacao", "IN", idSituacao.ToString());
            parametro.Filtro.Add(f);

            f = new Request.Filtro("IdTitulo", "IN", idTitulo.ToString());
            parametro.Filtro.Add(f);

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy,groupBy, parametro);
            Int32 qntResultados = lista.DataTable.Rows.Count;
            Int32[] ids = new Int32[qntResultados];

            for (Int32 i = 0; i < qntResultados; i++)
            {
                ids[i] = Conversor.ToInt32(lista.DataTable.Rows[i]["IdCartao"].ToString());
            }

            return ids;

        }

        /// <summary>
        /// Atualiza a situação do relacionamento entre Cartão e Título
        /// </summary>
        /// <param name="idCartao"></param>
        /// <param name="idTitulo"></param>
        /// <param name="IdSituacao"></param>   
        /// <returns>id da chave primária</returns>
        public Int32 AtualizarRelacionamentoCartao(Int32[] idTitulo, Int32 idSituacao)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE TituloCartao SET IdSituacao=@IdSituacao "+
                "WHERE IdTitulo IN (" +Conversor.ToString(",",idTitulo)+ ")";

            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, idSituacao));

            Int32 id = this.Conexao.Update(sql, parametros);
            return id;
        }

        /// <summary>
        /// Cria o relacionamento entre Espécie e Título
        /// </summary>
        /// <param name="idEspecie"></param>
        /// <param name="idTitulo"></param>
        /// <param name="IdSituacao"></param>  
        /// <returns>id da chave primária</returns>
        public Int32 CriarRelacionamentoEspecie(Int32 idEspecie, Int32 idTitulo, Int32 idSituacao)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO TituloEspecie(IdEspecie, IdTitulo,IdSituacao) " +
                        "VALUES(@IdEspecie, @IdTitulo,@IdSituacao)";

            parametros.Add(this.Conexao.CriarParametro("@IdEspecie", DbType.Int32, idEspecie));
            parametros.Add(this.Conexao.CriarParametro("@IdTitulo", DbType.Int32, idTitulo));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, idSituacao));

            Int32 id = this.Conexao.Insert(sql, parametros);
            return id;
        }

        /// <summary>
        /// Atualiza a situação do relacionamento entre Espécie e Título
        /// </summary>
        /// <param name="idEspecie"></param>
        /// <param name="idTitulo"></param>
        /// <param name="IdSituacao"></param>  
        /// <returns>id da chave primária</returns>
        public Int32 AtualizarRelacionamentoEspecie(Int32[] idTitulo, Int32 idSituacao)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE TituloEspecie SET IdSituacao=@IdSituacao " +
                "WHERE IdTitulo IN (" + Conversor.ToString(",", idTitulo) + ")";            

            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, idSituacao));

            Int32 id = this.Conexao.Update(sql, parametros);
            return id;
        }

        /// <summary>
        /// Carrega os ids de espécie de determinado Título
        /// </summary>       
        /// <param name="idTitulo"></param>
        /// <param name="IdSituacao"></param>   
        /// <returns>ids das espécies utilizados para efetuar o pagamento de um título</returns>
        public Int32[] CarregarRelacionamentoEspecie(Int32 idTitulo, Int32 idSituacao)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "TituloEspecie.* ";
            String join = "FROM TituloEspecie ";
            String whereDefault = "";
            String orderBy = "TituloEspecie.IdEspecie";
            String groupBy = "";

            //Busca as movimentações de acordo com o relacionamento entre movimentação financeira e título
            Prion.Tools.Request.ParametrosRequest parametro = new Prion.Tools.Request.ParametrosRequest();
            Prion.Tools.Request.Filtro f = new Request.Filtro("IdSituacao", "IN", idSituacao.ToString());
            parametro.Filtro.Add(f);

            f = new Request.Filtro("IdTitulo", "IN", idTitulo.ToString());
            parametro.Filtro.Add(f);

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy,groupBy, parametro);
            Int32 qntResultados = lista.DataTable.Rows.Count;
            Int32[] ids = new Int32[qntResultados];

            for (Int32 i = 0; i < qntResultados; i++)
            {
                ids[i] = Conversor.ToInt32(lista.DataTable.Rows[i]["IdEspecie"].ToString());
            }

            return ids;

        }
              
        /// <summary>
        /// Cria o relacionamento entre Cheque e Título
        /// </summary>
        /// <param name="idCheque"></param>
        /// <param name="idTitulo"></param>
        /// <param name="IdSituacao"></param>  
        /// <returns>id da chavce primária</returns>
        public Int32 CriarRelacionamentoCheque(Int32 idCheque, Int32 idTitulo, Int32 idSituacao)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO TituloCheque(IdCheque, IdTitulo,IdSituacao) " +
                        "VALUES(@IdCheque, @IdTitulo,@IdSituacao)";

            parametros.Add(this.Conexao.CriarParametro("@IdCheque", DbType.Int32, idCheque));
            parametros.Add(this.Conexao.CriarParametro("@IdTitulo", DbType.Int32, idTitulo));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, idSituacao));

            Int32 id = this.Conexao.Insert(sql, parametros);
            return id;
        }

        /// <summary>
        /// Carrega os ids de Cheque de determinado Título
        /// </summary>       
        /// <param name="idTitulo"></param>
        /// <param name="IdSituacao"></param>   
        /// <returns>ids dos cheques utilizados para efetuar o pagamento de um título</returns>
        public Int32[] CarregarRelacionamentoCheque(Int32 idTitulo, Int32 idSituacao)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "TituloCheque.* ";
            String join = "FROM TituloCheque ";
            String whereDefault = "";
            String orderBy = "TituloCheque.IdCheque";
            String groupBy = "";
            //Busca as movimentações de acordo com o relacionamento entre movimentação financeira e título
            Prion.Tools.Request.ParametrosRequest parametro = new Prion.Tools.Request.ParametrosRequest();
            Prion.Tools.Request.Filtro f = new Request.Filtro("IdSituacao", "IN", idSituacao.ToString());
            parametro.Filtro.Add(f);

            f = new Request.Filtro("IdTitulo", "IN", idTitulo.ToString());
            parametro.Filtro.Add(f);

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy,groupBy, parametro);
            Int32 qntResultados = lista.DataTable.Rows.Count;
            Int32[] ids = new Int32[qntResultados];

            for (Int32 i = 0; i < qntResultados; i++)
            {
                ids[i] = Conversor.ToInt32(lista.DataTable.Rows[i]["IdCheque"].ToString());
            }

            return ids;

        }

        /// <summary>
        /// Atualiza a situação do relacionamento entre Cheque e Título
        /// </summary>
        /// <param name="idCheque"></param>
        /// <param name="idTitulo"></param>
        /// <param name="IdSituacao"></param>  
        /// <returns>id da chave primária</returns>
        public Int32 AtualizarRelacionamentoCheque(Int32[] idTitulo, Int32 idSituacao)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE TituloCheque SET IdSituacao=@IdSituacao " +
                "WHERE IdTitulo IN (" + Conversor.ToString(",", idTitulo) + ")";
           

            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, idSituacao));

            Int32 id = this.Conexao.Update(sql, parametros);
            return id;
        }

        /// <summary>
        /// Grava o log de MovimentacaoFinanceira
        /// </summary>
        /// <param name="idRegistro">id do registro</param>
        /// <param name="tipoOperacaoLog">tipo de operação: INSERIR, ALTERAR, EXCLUIR</param>
        /// <returns>true/false </returns>
        protected Boolean GravarLog(Int32 idRegistro, TipoOperacaoLog tipoOperacaoLog)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sqlLog = " if EXISTS (SELECT Id FROM MovimentacaoFinanceira WHERE Id = @Id) " +
                            " INSERT INTO MovimentacaoFinanceiraLog(IdUsuarioLog, IdOperacaoLog, Id, Valor, DataCadastro)" +
                            " SELECT @IdUsuarioLog, @IdOperacaoLog, @Id, Valor, DataCadastro FROM MovimentacaoFinanceira WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "MovimentacaoFinanceiraLog");
        }
    }
}
