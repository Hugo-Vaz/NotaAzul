using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Prion.Data;
using Prion.Tools;

namespace NotaAzul.Repository
{
    public class AlunoResponsavelTelefone : Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public AlunoResponsavelTelefone(ref DBFacade conexao)
            : base(ref conexao)
        { 
        }


        ~AlunoResponsavelTelefone()
        { 
        }


        /// <summary>
        /// Busca todas as AlunoResponsavelTelefone da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "AlunoResponsavelTelefone.* ";
            String join = "FROM AlunoResponsavelTelefone";
            String whereDefault = "";
            String orderBy = "AlunoResponsavelTelefone.Preferencial DESC";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy,groupBy, parametro);
            List<Models.AlunoResponsavelTelefone> listaTelefone = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaTelefone = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaTelefone != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaTelefone);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }
        

        /// <summary>
        /// Insere um registro em AlunoResponsavelTelefone através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.AlunoResponsavelTelefone</param>
        /// <returns></returns>
        protected override Int32 Inserir(Prion.Generic.Models.Base objeto)
        {
            Models.AlunoResponsavelTelefone oAlunoResponsavelTelefone = (Models.AlunoResponsavelTelefone)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO AlunoResponsavelTelefone(IdAlunoResponsavel, TipoTelefone, Observacao, " + 
                        "DDD, Numero, Preferencial) " +
                        "VALUES(@IdAlunoResponsavel, @TipoTelefone, @Observacao, @DDD, @Numero, @Preferencial)";

            parametros.Add(this.Conexao.CriarParametro("@IdAlunoResponsavel", DbType.Int32, oAlunoResponsavelTelefone.IdAlunoResponsavel));
            parametros.Add(this.Conexao.CriarParametro("@TipoTelefone", DbType.String, oAlunoResponsavelTelefone.TipoTelefone, 50));
            parametros.Add(this.Conexao.CriarParametro("@Observacao", DbType.String, oAlunoResponsavelTelefone.Observacao, 255));
            parametros.Add(this.Conexao.CriarParametro("@DDD", DbType.String, oAlunoResponsavelTelefone.Ddd));
            parametros.Add(this.Conexao.CriarParametro("@Numero", DbType.String, oAlunoResponsavelTelefone.Numero));
            parametros.Add(this.Conexao.CriarParametro("@Preferencial", DbType.Boolean, oAlunoResponsavelTelefone.Preferencial));

            Int32 id = this.Conexao.Insert(sql, parametros);

            GravarLog(id, TipoOperacaoLog.Inserir);

            return id;
        }


        /// <summary>
        /// Altera um registro de AlunoResponsavelTelefone através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.AlunoResponsavelTelefone</param>
        /// <returns></returns>
        protected override Int32 Alterar(Prion.Generic.Models.Base objeto)
        {
            Models.AlunoResponsavelTelefone oAlunoResponsavelTelefone = (Models.AlunoResponsavelTelefone)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE AlunoResponsavelTelefone SET IdAlunoResponsavel=@IdAlunoResponsavel, " +
                        "TipoTelefone=@TipoTelefone, Observacao=@Observacao, DDD=@DDD, " + 
                        "Numero=@Numero, Preferencial=@Preferencial WHERE Id=@Id";

            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, oAlunoResponsavelTelefone.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdAlunoResponsavel", DbType.Int32, oAlunoResponsavelTelefone.IdAlunoResponsavel));
            parametros.Add(this.Conexao.CriarParametro("@TipoTelefone", DbType.String, oAlunoResponsavelTelefone.TipoTelefone, 50));
            parametros.Add(this.Conexao.CriarParametro("@Observacao", DbType.String, oAlunoResponsavelTelefone.Observacao, 50));
            parametros.Add(this.Conexao.CriarParametro("@DDD", DbType.String, oAlunoResponsavelTelefone.Ddd));
            parametros.Add(this.Conexao.CriarParametro("@Numero", DbType.String, oAlunoResponsavelTelefone.Numero));
            parametros.Add(this.Conexao.CriarParametro("@Preferencial", DbType.Boolean, oAlunoResponsavelTelefone.Preferencial));

            Int32 linhasAfetadas = this.Conexao.Update(sql, parametros);

            GravarLog(oAlunoResponsavelTelefone.Id, TipoOperacaoLog.Alterar);

            return linhasAfetadas;
        }


        /// <summary>
        /// Exclui um registro de AlunoResponsavelTelefone através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.AlunoResponsavelTelefone</param>
        /// <returns>Quantidade de registros excluídos</returns>
        protected override Int32 Excluir(Prion.Generic.Models.Base objeto)
        {
            Models.AlunoResponsavelTelefone oAlunoResponsavelTelefone = (Models.AlunoResponsavelTelefone)objeto;
            return Excluir(oAlunoResponsavelTelefone.Id);
        }


        /// <summary>
        /// Exclui um registro de AlunoResponsavelTelefone através do Id
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
            String sql = "DELETE FROM AlunoResponsavelTelefone WHERE Id IN(" + strId + ")";

            retorno = this.Conexao.Delete(sql);
            return retorno;
        }


        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de AlunoResponsavelTelefone
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.AlunoResponsavelTelefone com os registros do DataTable</returns>
        public List<Models.AlunoResponsavelTelefone> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }


            List<Models.AlunoResponsavelTelefone> lista = new List<Models.AlunoResponsavelTelefone>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.AlunoResponsavelTelefone
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Models.AlunoResponsavelTelefone alunoResponsavelTelefone = new Models.AlunoResponsavelTelefone();

                alunoResponsavelTelefone.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                alunoResponsavelTelefone.IdAlunoResponsavel = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdAlunoResponsavel"].ToString());
                alunoResponsavelTelefone.TipoTelefone = dataTable.Rows[i][nomeBase + "TipoTelefone"].ToString();
                alunoResponsavelTelefone.Observacao = dataTable.Rows[i][nomeBase + "Observacao"].ToString();
                alunoResponsavelTelefone.Ddd = dataTable.Rows[i][nomeBase + "DDD"].ToString();
                alunoResponsavelTelefone.Numero = dataTable.Rows[i][nomeBase + "Numero"].ToString();
                alunoResponsavelTelefone.Preferencial = Conversor.ToBoolean(dataTable.Rows[i][nomeBase + "Preferencial"].ToString());
                alunoResponsavelTelefone.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                alunoResponsavelTelefone.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(alunoResponsavelTelefone);
                    continue;
                }

                // carrega as demais entidades necessárias
                alunoResponsavelTelefone = CarregarEntidades(Entidades, alunoResponsavelTelefone);

                lista.Add(alunoResponsavelTelefone);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }


        /// <summary>
        /// Carrega as demais entidades relacionadas a este objeto
        /// </summary>
        /// <param name="modulos">lista de módulos que serão utilizados para carregar as demais entidades</param>
        /// <param name="objTurno">Objeto do tipo Models.Base que será convertido em Models.AlunoResponsavelTelefone</param>
        protected Models.AlunoResponsavelTelefone CarregarEntidades(Prion.Tools.Entidade modulos, Prion.Generic.Models.Base objAlunoResponsavelTelefone)
        {
            // ************************************************************************************
            // NÃO HÁ NECESSIDADE DE CARREGAR OS DEMAIS OBJETOS (Chaves Estrangeiras)
            // Nunca iremos carregar apenas uma lista de Telefones
            // ************************************************************************************

            return (Models.AlunoResponsavelTelefone)objAlunoResponsavelTelefone;
        }


        /// <summary>
        /// Grava o log de AlunoResponsavelTelefone
        /// </summary>
        /// <param name="idRegistro">id do registro</param>
        /// <param name="tipoOperacaoLog">tipo de operação: INSERIR, ALTERAR, EXCLUIR</param>
        /// <returns>true/false </returns>
        protected Boolean GravarLog(Int32 idRegistro, TipoOperacaoLog tipoOperacaoLog)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sqlLog = " if EXISTS (SELECT Id FROM AlunoResponsavelTelefone WHERE Id = @Id) " +
                            " INSERT INTO AlunoResponsavelTelefoneLog(IdUsuarioLog, IdOperacaoLog, Id, IdAlunoResponsavel, TipoTelefone, Observacao, " + 
                                "DDD, Numero, Preferencial, DataCadastro)" +
                            " SELECT @IdUsuarioLog, @IdOperacaoLog, @Id, IdAlunoResponsavel, TipoTelefone, Observacao, " + 
                                "DDD, Numero, Preferencial, DataCadastro FROM AlunoResponsavelTelefone " +
                                "WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "AlunoResponsavelTelefoneLog");
        }
    }
}