using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Prion.Data;
using Prion.Tools;

namespace NotaAzul.Repository
{
    public class Receita : Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public Receita(ref DBFacade conexao)
            : base(ref conexao)
        { 
        }


        ~Receita()
        { 
        }


        /// <summary>
        /// Busca todas as Receita da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "Receita.* ";
            String join = "FROM Receita";
            String whereDefault = "";
            String orderBy = "Receita.Descricao";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy,groupBy, parametro);
            List<Models.Receita> listaReceita = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaReceita = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaReceita != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaReceita);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }
        

        /// <summary>
        /// Insere um registro em Receita através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Receita</param>
        /// <returns></returns>
        protected override Int32 Inserir(Prion.Generic.Models.Base objeto)
        {
            Models.Receita oReceita = (Models.Receita)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO Receita(IdCorporacao, Descricao, Valor) VALUES(@IdCorporacao, @Descricao, @Valor)";

            parametros.Add(this.Conexao.CriarParametro("@IdCorporacao", DbType.Int32, UsuarioLogado.IdCorporacao));
            parametros.Add(this.Conexao.CriarParametro("@Descricao", DbType.String, StringHelper.OnlyOneSpace(oReceita.Descricao)));
            parametros.Add(this.Conexao.CriarParametro("@Valor", DbType.Decimal, oReceita.Valor));
            
            Int32 id = this.Conexao.Insert(sql, parametros);

            GravarLog(id, TipoOperacaoLog.Inserir);

            return id;
        }


        /// <summary>
        /// Altera um registro de Receita através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Receita</param>
        /// <returns></returns>
        protected override Int32 Alterar(Prion.Generic.Models.Base objeto)
        {
            Models.Receita oReceita = (Models.Receita)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE Receita SET Descricao=@Descricao, Valor=@Valor WHERE Id=@Id";

            parametros.Add(this.Conexao.CriarParametro("@Descricao", DbType.String, StringHelper.OnlyOneSpace(oReceita.Descricao)));
            parametros.Add(this.Conexao.CriarParametro("@Valor", DbType.Decimal, oReceita.Valor));
            
            Int32 linhasAfetadas = this.Conexao.Update(sql, parametros);

            GravarLog(oReceita.Id, TipoOperacaoLog.Alterar);

            return linhasAfetadas;
        }


        /// <summary>
        /// Exclui um registro de Receita através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Receita</param>
        /// <returns>Quantidade de registros excluídos</returns>
        protected override Int32 Excluir(Prion.Generic.Models.Base objeto)
        {
            Models.Receita oReceita = (Models.Receita)objeto;
            return Excluir(oReceita.Id);
        }


        /// <summary>
        /// Exclui um registro de Receita através do Id
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
            String sql = "DELETE FROM Receita WHERE Id IN(" + strId + ")";

            retorno = this.Conexao.Delete(sql);
            return retorno;
        }


        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de Receitas
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.Receita com os registros do DataTable</returns>
        public List<Models.Receita> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }


            List<Models.Receita> lista = new List<Models.Receita>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.Receita
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Models.Receita receita = new Models.Receita();

                receita.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                receita.IdCorporacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdCorporacao"].ToString());
                receita.Descricao = dataTable.Rows[i][nomeBase + "Descricao"].ToString();
                receita.Valor = Conversor.ToDecimal(dataTable.Rows[i][nomeBase + "Valor"].ToString());
                receita.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                receita.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(receita);
                    continue;
                }

                lista.Add(receita);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }


        /// <summary>
        /// Carrega as demais entidades relacionadas a este objeto
        /// </summary>
        /// <param name="modulos">lista de módulos que serão utilizados para carregar as demais entidades</param>
        /// <param name="objReceita">Objeto do tipo Models.Base que será convertido em Models.Receita</param>
        protected Models.Receita CarregarEntidades(Prion.Tools.Entidade modulos, Prion.Generic.Models.Base objReceita)
        {
            Boolean modoCompleto = Entidades.Carregar(Helpers.Entidade.Tudo.ToString());
            Models.Receita receita = (Models.Receita)objReceita;


            // verifica se possui o módulo COMPLETO ou se possui o módulo CORPORACAO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Corporacao.ToString())))
            {
                Prion.Generic.Repository.Corporacao repCorporacao = new Prion.Generic.Repository.Corporacao(ref this._conexao);
                repCorporacao.Entidades = modulos;

                // carrega um objeto do tipo Models.Corporacao
                receita.Corporacao = (Prion.Generic.Models.Corporacao)repCorporacao.BuscarPeloId(receita.IdCorporacao);
            }


            // retorna o objeto de Receita com as entidades que foram carregadas
            return receita;
        }


        /// <summary>
        /// Grava o log de Receita
        /// </summary>
        /// <param name="idRegistro">id do registro</param>
        /// <param name="tipoOperacaoLog">tipo de operação: INSERIR, ALTERAR, EXCLUIR</param>
        /// <returns>true/false </returns>
        protected Boolean GravarLog(Int32 idRegistro, TipoOperacaoLog tipoOperacaoLog)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sqlLog = " if EXISTS (SELECT Id FROM Receita WHERE Id = @Id) " +
                            " INSERT INTO ReceitaLog(IdUsuarioLog, IdOperacaoLog, Id, IdCorporacao, Descricao, Valor, DataCadastro)" +
                            " SELECT @IdUsuarioLog, @IdOperacaoLog, @Id, IdCorporacao, Descricao, Valor, DataCadastro FROM Receita WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "ReceitaLog");
        }
    }
}