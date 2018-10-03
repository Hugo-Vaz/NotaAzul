using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Prion.Data;
using Prion.Tools;

namespace NotaAzul.Repository
{
    public class AlunoFilantropia : Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public AlunoFilantropia(ref DBFacade conexao)
            : base(ref conexao)
        { 
        }

        ~AlunoFilantropia()
        { 
        }


        /// <summary>
        /// Busca todas as AlunoFilantropia da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "AlunoFilantropia.* ";
            String join = "FROM AlunoFilantropia";
            String whereDefault = "";
            String orderBy = "AlunoFilantropia.AnoLetivo";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy,groupBy, parametro);
            List<Models.AlunoFilantropia> listaAlunoFilantropia = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaAlunoFilantropia = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaAlunoFilantropia != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaAlunoFilantropia);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }


        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de AlunoFilantropia
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.AlunoFilantropia com os registros do DataTable</returns>
        public List<Models.AlunoFilantropia> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }

            List<Models.AlunoFilantropia> lista = new List<Models.AlunoFilantropia>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.AlunoFilantropia
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Models.AlunoFilantropia alunoFilantropia = new Models.AlunoFilantropia();

                alunoFilantropia.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                alunoFilantropia.IdAluno = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdAluno"].ToString());
                alunoFilantropia.AnoLetivo = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "AnoLetivo"].ToString());
                alunoFilantropia.ValorBolsa = Conversor.ToDecimal(dataTable.Rows[i][nomeBase + "ValorBolsa"].ToString());
                alunoFilantropia.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                alunoFilantropia.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(alunoFilantropia);
                    continue;
                }

                // carrega as demais entidades necessárias
                alunoFilantropia = CarregarEntidades(Entidades, alunoFilantropia);

                lista.Add(alunoFilantropia);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }


        /// <summary>
        /// Exclui um registro de AlunoFilantropia através do Id
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
            String sql = "DELETE FROM AlunoFilantropia WHERE Id IN(" + strId + ")";

            retorno = this.Conexao.Delete(sql);
            return retorno;
        }


        /// <summary>
        /// Insere um registro em AlunoFilantropia através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.AlunoFilantropia</param>
        /// <returns></returns>
        protected override int Inserir(Prion.Generic.Models.Base objeto)
        {
            Models.AlunoFilantropia oAlunoFilantropia = (Models.AlunoFilantropia)objeto;
            List<DbParameter> parametros = new List<DbParameter>();

            String sql = "INSERT INTO AlunoFilantropia(IdAluno, AnoLetivo, ValorBolsa) " + 
                        "VALUES(@IdAluno, @AnoLetivo, @ValorBolsa)";

            parametros.Add(this.Conexao.CriarParametro("@IdAluno", DbType.Int32, oAlunoFilantropia.IdAluno));
            parametros.Add(this.Conexao.CriarParametro("@AnoLetivo", DbType.Int32, oAlunoFilantropia.AnoLetivo));
            parametros.Add(this.Conexao.CriarParametro("@ValorBolsa", DbType.Decimal, oAlunoFilantropia.ValorBolsa));

            Int32 id = this.Conexao.Insert(sql, parametros);

            GravarLog(id, TipoOperacaoLog.Inserir);

            return id;
        }


        /// <summary>
        /// Altera um registro de AlunoFilantropia através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.AlunoFilantropia</param>
        /// <returns></returns>
        protected override Int32 Alterar(Prion.Generic.Models.Base objeto)
        {
            Models.AlunoFilantropia oAlunoFilantropia = (Models.AlunoFilantropia)objeto;
            List<DbParameter> parametros = new List<DbParameter>();

            String sql = "UPDATE AlunoFilantropia SET IdAluno=@IdAluno, AnoLetivo=@AnoLetivo, " + 
                        "ValorBolsa=@ValorBolsa WHERE Id=@Id";

            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, oAlunoFilantropia.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdAluno", DbType.Int32, oAlunoFilantropia.IdAluno));
            parametros.Add(this.Conexao.CriarParametro("@AnoLetivo", DbType.Int32, oAlunoFilantropia.AnoLetivo));
            parametros.Add(this.Conexao.CriarParametro("@ValorBolsa", DbType.Decimal, oAlunoFilantropia.ValorBolsa));

            Int32 linhasAfetadas = this.Conexao.Update(sql, parametros);

            GravarLog(oAlunoFilantropia.Id, TipoOperacaoLog.Alterar);

            return linhasAfetadas;
        }


        /// <summary>
        /// Exclui um registro de AlunoFilantropia através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Filantropia</param>
        /// <returns>Quantidade de registros excluídos</returns>
        protected override Int32 Excluir(Prion.Generic.Models.Base objeto)
        {
            Models.AlunoFilantropia oAlunoFilantropia = (Models.AlunoFilantropia)objeto;
            return Excluir(oAlunoFilantropia.Id);
        }


        /// <summary>
        /// Carrega as demais entidades relacionadas a este objeto
        /// </summary>
        /// <param name="modulos">lista de módulos que serão utilizados para carregar as demais entidades</param>
        /// <param name="objTurno">Objeto do tipo Models.Base que será convertido em Models.AlunoFilantropia</param>
        protected Models.AlunoFilantropia CarregarEntidades(Prion.Tools.Entidade modulos, Prion.Generic.Models.Base objAlunoFilantropia)
        {
            // ************************************************************************************
            // NÃO HÁ NECESSIDADE DE CARREGAR OS DEMAIS OBJETOS (Chaves Estrangeiras)
            // Nunca iremos carregar apenas uma lista de Filantropia
            // ************************************************************************************

            return (Models.AlunoFilantropia)objAlunoFilantropia;
        }


        /// <summary>
        /// Grava o log de AlunoFilantropia
        /// </summary>
        /// <param name="idRegistro">id do registro</param>
        /// <param name="tipoOperacaoLog">tipo de operação: INSERIR, ALTERAR, EXCLUIR</param>
        /// <returns>true/false </returns>
        protected Boolean GravarLog(Int32 idRegistro, TipoOperacaoLog tipoOperacaoLog)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sqlLog = "if EXISTS (SELECT Id FROM AlunoFilantropia WHERE Id=@Id)" +
                            "INSERT INTO AlunoFilantropiaLog(IdUsuarioLog, IdOperacaoLog, Id, IdAluno, AnoLetivo, ValorBolsa, DataCadastro)" +
                            "SELECT @IdUsuarioLog, @IdOperacaoLog, @Id, IdAluno, AnoLetivo, ValorBolsa, DataCadastro FROM AlunoFilantropia WHERE Id=@Id"; 

            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "AlunoFilantropiaLog");
        }
    }
}