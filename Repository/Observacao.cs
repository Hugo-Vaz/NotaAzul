using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Prion.Data;
using Prion.Tools;

namespace NotaAzul.Repository
{
    public class Observacao : Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public Observacao(ref DBFacade conexao)
            : base(ref conexao)
        {
        }


        ~Observacao()
        {
        }


        /// <summary>
        /// Busca todas as Observacao da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "Observacao.* ";
            String join = "FROM Observacao";
            String whereDefault = "";
            String orderBy = "Observacao.Id";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy,groupBy, parametro);
            List<Models.Observacao> listaObservacao = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaObservacao = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaObservacao != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaObservacao);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }
        

        /// <summary>
        /// Insere um registro em Observacao através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Observacao</param>
        /// <returns></returns>
        protected override Int32 Inserir(Prion.Generic.Models.Base objeto)
        {
            Models.Observacao oObservacao = (Models.Observacao)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO Observacao(IdUsuario, IdSituacao, Descricao) VALUES(@IdUsuario, @IdSituacao, @Descricao)";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuario", DbType.Int32, oObservacao.IdUsuario));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, oObservacao.IdSituacao));
            parametros.Add(this.Conexao.CriarParametro("@Descricao", DbType.String, StringHelper.OnlyOneSpace(oObservacao.Descricao)));

            Int32 id = this.Conexao.Insert(sql, parametros);

            GravarLog(id, TipoOperacaoLog.Inserir);

            return id;
        }


        /// <summary>
        /// Altera um registro de Obeservacao através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Observacao</param>
        /// <returns></returns>
        protected override Int32 Alterar(Prion.Generic.Models.Base objeto)
        {
            Models.Observacao oObservacao = (Models.Observacao)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE Observacao SET IdUsuario=@IdUsuario, IdSituacao=@IdSituacao, Descricao=@Descricao WHERE Id=@Id";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuario", DbType.Int32, oObservacao.IdUsuario));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, oObservacao.IdSituacao));
            parametros.Add(this.Conexao.CriarParametro("@Descricao", DbType.String, StringHelper.OnlyOneSpace(oObservacao.Descricao)));

            Int32 linhasAfetadas = this.Conexao.Update(sql, parametros);

            GravarLog(oObservacao.Id, TipoOperacaoLog.Alterar);

            return linhasAfetadas;
        }


        /// <summary>
        /// Exclui um registro de Observacao através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Observacao</param>
        /// <returns>Quantidade de registros excluídos</returns>
        protected override Int32 Excluir(Prion.Generic.Models.Base objeto)
        {
            Models.Observacao oObservacao = (Models.Observacao)objeto;
            return Excluir(oObservacao.Id);
        }


        /// <summary>
        /// Exclui um registro de Observacao através do Id
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
            String sql = "DELETE FROM Observacao WHERE Id IN(" + strId + ")";

            retorno = this.Conexao.Delete(sql);
            return retorno;
        }


        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de Observacao
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.Observacao com os registros do DataTable</returns>
        public virtual List<Models.Observacao> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }


            List<Models.Observacao> lista = new List<Models.Observacao>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.Observacao
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Models.Observacao observacao = new Models.Observacao();

                observacao.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                observacao.IdUsuario = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdUsuario"].ToString());
                observacao.IdSituacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdSituacao"].ToString());
                observacao.Descricao = dataTable.Rows[i][nomeBase + "Descricao"].ToString();
                observacao.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                observacao.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(observacao);
                    continue;
                }

                // carrega as demais entidades necessárias
                observacao = CarregarEntidades(Entidades, observacao);

                lista.Add(observacao);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }


        /// <summary>
        /// Carrega as demais entidades relacionadas a este objeto
        /// </summary>
        /// <param name="modulos">lista de módulos que serão utilizados para carregar as demais entidades</param>
        /// <param name="objTurno">Objeto do tipo Models.Base que será convertido em Models.Observacao</param>
        protected Models.Observacao CarregarEntidades(Prion.Tools.Entidade modulos, Prion.Generic.Models.Base objObservacao)
        {
            Boolean modoCompleto = Entidades.Carregar(Helpers.Entidade.Tudo.ToString());
            Models.Observacao observacao = (Models.Observacao)objObservacao;


            // verifica se possui o módulo COMPLETO ou se possui o módulo Usuario
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Usuario.ToString())))
            {
                Repository.Usuario repUsuario = new Repository.Usuario(ref this._conexao);
                repUsuario.Entidades = modulos;

                // carrega um objeto do tipo Models.Usuario, representando o usuário que gravou esta Observação
                observacao.Usuario = (Models.Usuario)repUsuario.BuscarPeloId(observacao.IdUsuario).Get(0);
            }


            // verifica se possui o módulo COMPLETO ou se possui o módulo Situacao
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Situacao.ToString())))
            {
                Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this._conexao);
                repSituacao.Entidades = modulos;

                // carrega um objeto do tipo Models.Situacao, representando a situação da Observação
                observacao.Situacao = (Prion.Generic.Models.Situacao)repSituacao.BuscarPeloId(observacao.IdSituacao).Get(0);
            }


            // retorna o objeto de Observacao com as entidades que foram carregadas
            return observacao;
        }


        /// <summary>
        /// Grava o log de Observacao
        /// </summary>
        /// <param name="idRegistro">id do registro</param>
        /// <param name="tipoOperacaoLog">tipo de operação: INSERIR, ALTERAR, EXCLUIR</param>
        /// <returns>true/false </returns>
        protected Boolean GravarLog(Int32 idRegistro, TipoOperacaoLog tipoOperacaoLog)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sqlLog = " if EXISTS (SELECT Id FROM Observacao WHERE Id = @Id) " +
                            " INSERT INTO ObservacaoLog(IdUsuarioLog, IdOperacaoLog, Id, IdUsuario, IdSituacao, Descricao, DataCadastro)" +
                            " SELECT @IdUsuarioLog, @IdOperacaoLog, @Id, IdUsuario, IdSituacao, Descricao, DataCadastro FROM Observacao WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@IdUsusarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "ObservacaoLog");
        }
    }
}