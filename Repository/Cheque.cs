using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Prion.Data;
using Prion.Tools;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.Repository
{
    public class Cheque : Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public Cheque(ref DBFacade conexao)
            : base(ref conexao)
        { 
        }

        ~Cheque()
        { 
        }


        /// <summary>
        /// Busca todas os registros de Cheque da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "Cheque.* ";
            String join = "FROM Cheque ";
            String whereDefault = "";
            String orderBy = "Cheque.Banco";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy,groupBy, parametro);
            List<GenericModels.Cheque> listaCheque = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaCheque = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaCheque != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaCheque);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }


        /// <summary>
        /// Retorna, através de um (ou vários) ID, uma lista de objetos do tipo GenericModels.Cheque
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
        /// Recebe um DataTable como parâmetro e carrega uma lista de cheques
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.Cheque com os registros do DataTable</returns>
        public List<GenericModels.Cheque> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }


            List<GenericModels.Cheque> lista = new List<GenericModels.Cheque>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.Cheque
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                GenericModels.Cheque cheque = new GenericModels.Cheque();

                cheque.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                cheque.IdCorporacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdCorporacao"].ToString());
                cheque.IdResponsavel = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdAlunoResponsavel"].ToString());
                
                cheque.NomeEmissor = dataTable.Rows[i][nomeBase + "NomeEmissor"].ToString();
                cheque.TelefoneEmissor = dataTable.Rows[i][nomeBase + "TelefoneEmissor"].ToString();
                cheque.Banco = dataTable.Rows[i][nomeBase + "Banco"].ToString();
                cheque.Agencia = dataTable.Rows[i][nomeBase + "Agencia"].ToString();
                cheque.Alinea = dataTable.Rows[i][nomeBase + "Alinea"].ToString();                
                cheque.BomPara = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "BomPara"].ToString());
                cheque.ContaCorrente = dataTable.Rows[i][nomeBase + "ContaCorrente"].ToString();
                cheque.Numero = dataTable.Rows[i][nomeBase + "Numero"].ToString();
                cheque.Valor = Conversor.ToDecimal(dataTable.Rows[i][nomeBase + "Valor"].ToString());
                cheque.Proprio = Conversor.ToBoolean(dataTable.Rows[i][nomeBase + "Proprio"].ToString());                
                cheque.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                cheque.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(cheque);
                    continue;
                }

                // carrega as demais entidades necessárias
                cheque = CarregarEntidades(Entidades, cheque);

                lista.Add(cheque);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }


        /// <summary>
        /// Exclui um registro de Cheque através do Id
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

            String sql = "DELETE FROM Cheque WHERE Id IN (" + strId + ")";

            retorno = this.Conexao.Delete(sql);
            return retorno;
        }


        /// <summary>
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros sem WHERE
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista DataTable(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder campos = new StringBuilder(), join = new StringBuilder();
            campos.Append("Cheque.* ");
            join.Append(" FROM Cheque");
           
            String whereDefault = "";
            String orderBy = "Cheque.Banco";
            String groupBy = "";

            return this.Select("SELECT " + campos.ToString(), join.ToString(), whereDefault, orderBy,groupBy, parametro);
        }


        /// <summary>
        /// Insere um registro em Cheque através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Cheque</param>
        /// <returns></returns>
        protected override int Inserir(Prion.Generic.Models.Base objeto)
        {
            GenericModels.Cheque oCheque = (GenericModels.Cheque)objeto;
            List<DbParameter> parametros = new List<DbParameter>();

            String sql = "INSERT INTO Cheque(IdCorporacao,IdAlunoResponsavel, NomeEmissor, TelefoneEmissor, Banco, Agencia, ContaCorrente, Numero, Valor, " +
                            "Alinea, Proprio, BomPara) " +
                            "VALUES (@IdCorporacao,@IdAlunoResponsavel, @NomeEmissor, @TelefoneEmissor, @Banco, @Agencia, @ContaCorrente, @Numero, @Valor, " +
                            "@Alinea, @Proprio, @BomPara)";

            parametros.Add(this.Conexao.CriarParametro("@IdCorporacao", DbType.Int32, UsuarioLogado.IdCorporacao));
            parametros.Add(this.Conexao.CriarParametro("@IdAlunoResponsavel", DbType.Int32, oCheque.IdResponsavel));
            parametros.Add(this.Conexao.CriarParametro("@NomeEmissor", DbType.String, oCheque.NomeEmissor, 50));
            parametros.Add(this.Conexao.CriarParametro("@TelefoneEmissor", DbType.String, oCheque.TelefoneEmissor, 15));
            parametros.Add(this.Conexao.CriarParametro("@Banco", DbType.String, oCheque.Banco, 5));
            parametros.Add(this.Conexao.CriarParametro("@Agencia", DbType.String,oCheque.Agencia, 10));
            parametros.Add(this.Conexao.CriarParametro("@ContaCorrente", DbType.String, (oCheque.ContaCorrente), 20));
            parametros.Add(this.Conexao.CriarParametro("@Numero", DbType.String, oCheque.Numero, 10));
            parametros.Add(this.Conexao.CriarParametro("@Valor", DbType.Decimal, oCheque.Valor));
            parametros.Add(this.Conexao.CriarParametro("@Alinea", DbType.String, oCheque.Alinea, 5));
            parametros.Add(this.Conexao.CriarParametro("@Proprio", DbType.Boolean, oCheque.Proprio));
            parametros.Add(this.Conexao.CriarParametro("@BomPara", DbType.DateTime, oCheque.BomPara));            

            Int32 id = this.Conexao.Insert(sql, parametros);
            GravarLog(id, TipoOperacaoLog.Inserir);

            return id;
        }


        /// <summary>
        /// Altera um registro de Cheque através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em GenericModels.Cheque</param>
        /// <returns></returns>
        protected override Int32 Alterar(Prion.Generic.Models.Base objeto)
        {
            GenericModels.Cheque oCheque = (GenericModels.Cheque)objeto;
            List<DbParameter> parametros = new List<DbParameter>();

            String sql = "UPDATE Cheque SET  IdAlunoResponsavel=@IdAlunoResponsavel, NomeEmissor=@NomeEmissor, TelefoneEmissor=@TelefoneEmissor,Banco=@Banco, Agencia=@Agencia,"+
                        " ContaCorrente=@ContaCorrente, Numero=@Numero, Valor=@Valor, " +
                        "Alinea=@Alinea, Proprio=@Proprio, BomPara=@BomPara " +
                        "WHERE Id=@Id";

            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, oCheque.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdAlunoResponsavel", DbType.Int32, oCheque.IdResponsavel));
            parametros.Add(this.Conexao.CriarParametro("@NomeEmissor", DbType.String, oCheque.NomeEmissor, 50));
            parametros.Add(this.Conexao.CriarParametro("@TelefoneEmissor", DbType.String, oCheque.TelefoneEmissor, 15));
            parametros.Add(this.Conexao.CriarParametro("@Banco", DbType.String, oCheque.Banco,5));
            parametros.Add(this.Conexao.CriarParametro("@Agencia", DbType.String,oCheque.Agencia,10));
            parametros.Add(this.Conexao.CriarParametro("@ContaCorrente", DbType.String, (oCheque.ContaCorrente), 20));
            parametros.Add(this.Conexao.CriarParametro("@Numero", DbType.String, oCheque.Numero, 10));
            parametros.Add(this.Conexao.CriarParametro("@Valor", DbType.Decimal, oCheque.Valor));
            parametros.Add(this.Conexao.CriarParametro("@Alinea", DbType.String, oCheque.Alinea, 5));
            parametros.Add(this.Conexao.CriarParametro("@Proprio", DbType.Boolean, oCheque.Proprio));
            parametros.Add(this.Conexao.CriarParametro("@BomPara", DbType.DateTime, oCheque.BomPara));   

            Int32 linhasAfetadas = this.Conexao.Update(sql, parametros);

            GravarLog(oCheque.Id, TipoOperacaoLog.Alterar);

            return linhasAfetadas;
        }


        /// <summary>
        /// Exclui um registro de Cheque através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em GenericModels.Cheque</param>
        /// <returns>Quantidade de registros excluídos</returns>
        protected override Int32 Excluir(Prion.Generic.Models.Base objeto)
        {
            GenericModels.Cheque oCheque = (GenericModels.Cheque)objeto;
            return Excluir(oCheque.Id);
        }


        /// <summary>
        /// Carrega as demais entidades relacionadas a este objeto
        /// </summary>
        /// <param name="modulos">lista de módulos que serão utilizados para carregar as demais entidades</param>
        /// <param name="objCheque">Objeto do tipo Models.Base que será convertido em GenericModels.Cheque</param>
        protected GenericModels.Cheque CarregarEntidades(Prion.Tools.Entidade modulos, Prion.Generic.Models.Base objCheque)
        {
            Boolean modoCompleto = Entidades.Carregar(Helpers.Entidade.Tudo.ToString());
            GenericModels.Cheque cheque = (GenericModels.Cheque)objCheque;

           
            // retorna o objeto de Cheque com as entidades que foram carregadas
            return cheque;
        }


        /// <summary>
        /// Grava o log de Cheque
        /// </summary>
        /// <param name="idRegistro">id do registro</param>
        /// <param name="tipoOperacaoLog">tipo de operação: INSERIR, ALTERAR, EXCLUIR</param>
        /// <returns>true/false </returns>
        protected virtual Boolean GravarLog(Int32 idRegistro, TipoOperacaoLog tipoOperacaoLog)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sqlLog = "if EXISTS (SELECT Id FROM Cheque WHERE Id = @Id) " +
                            "INSERT INTO ChequeLog(IdUsuarioLog, IdOperacaoLog, Id, IdCorporacao,IdAlunoResponsavel, NomeEmissor, TelefoneEmissor, Banco, Agencia, ContaCorrente, Numero, Valor, " +
                            "Alinea, Proprio, BomPara, DataCadastro) " +
                            "SELECT @IdUsuarioLog, @IdOperacaoLog, @Id,IdCorporacao, IdAlunoResponsavel, NomeEmissor, TelefoneEmissor, Banco, Agencia, ContaCorrente, Numero, Valor, " +
                            "Alinea, Proprio, BomPara, DataCadastro FROM Cheque WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "ChequeLog");
        }
       
    }
}