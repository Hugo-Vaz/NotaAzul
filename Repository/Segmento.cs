using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Prion.Data;
using Prion.Tools;
using System.Text;

namespace NotaAzul.Repository
{
    public class Segmento : Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public Segmento(ref DBFacade conexao)
            : base(ref conexao)
        {
        }


        ~Segmento()
        {
        }


        /// <summary>
        /// Busca todos os Segmentos da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "Segmento.* ";
            String join = "FROM Segmento";
            String whereDefault = "";
            String orderBy = "Segmento.Nome";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy,groupBy, parametro);
            List<Models.Segmento> listaSegmento = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaSegmento = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaSegmento != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaSegmento);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }


        /// <summary>
        /// Retorna, através de um (ou vários) ID, uma lista de objetos do tipo Models.Segmento
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
            campos.Append("Segmento.* ");
            join.Append("FROM Segmento");

            if (Entidades.Carregar(Helpers.Entidade.Situacao.ToString()))
            {
                campos.Append(", " + Prion.Generic.Helpers.MapeamentoTabela.Situacao());
                join.Append(" INNER JOIN Situacao ON Segmento.IdSituacao = Situacao.Id ");
            }

            String whereDefault = "";
            String orderBy = "Segmento.Nome";
            String groupBy = "";

            return this.Select("SELECT " + campos.ToString(), join.ToString(), whereDefault, orderBy,groupBy, parametro);
        }
        

        /// <summary>
        /// Insere um registro em Segmento através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Segmento</param>
        /// <returns></returns>
        protected override Int32 Inserir(Prion.Generic.Models.Base objeto)
        {
            Models.Segmento oSegmento = (Models.Segmento)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO Segmento(IdCorporacao, IdSituacao, Nome) VALUES(@IdCorporacao, @IdSituacao, @Nome)";

            parametros.Add(this.Conexao.CriarParametro("@IdCorporacao", DbType.Int32, UsuarioLogado.IdCorporacao));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, oSegmento.IdSituacao));
            parametros.Add(this.Conexao.CriarParametro("@Nome", DbType.String, StringHelper.OnlyOneSpace(oSegmento.Nome), 255));
            Int32 id = this.Conexao.Insert(sql, parametros);

            GravarLog(id, TipoOperacaoLog.Inserir);

            return id;
        }


        /// <summary>
        /// Altera um registro de Segmento através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Segmento</param>
        /// <returns></returns>
        protected override Int32 Alterar(Prion.Generic.Models.Base objeto)
        {
            Models.Segmento oSegmento = (Models.Segmento)objeto;
            List<DbParameter> parametros = new List<DbParameter>();

            String sql = "UPDATE Segmento SET IdCorporacao = @IdCorporacao, IdSituacao = @IdSituacao, Nome = @Nome WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, oSegmento.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdCorporacao", DbType.Int32, UsuarioLogado.IdCorporacao));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, oSegmento.IdSituacao));
            parametros.Add(this.Conexao.CriarParametro("@Nome", DbType.String, StringHelper.OnlyOneSpace(oSegmento.Nome), 255));

            Int32 linhasAfetadas = this.Conexao.Update(sql, parametros);

            GravarLog(oSegmento.Id, TipoOperacaoLog.Alterar);

            return linhasAfetadas;
        }


        /// <summary>
        /// Exclui um registro de Segmento através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Segmento</param>
        /// <returns>Quantidade de registros excluídos</returns>
        protected override Int32 Excluir(Prion.Generic.Models.Base objeto)
        {
            Models.Segmento oSegmento = (Models.Segmento)objeto;
            return Excluir(oSegmento.Id);
        }


        /// <summary>
        /// Exclui um registro de Segmento através do Id
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
            String sql = "DELETE FROM Segmento WHERE Id IN (" + strId + ")";

            retorno = this.Conexao.Delete(sql);

            return retorno;
        }


        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de Segmento
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.Segmento com os registros do DataTable</returns>
        public List<Models.Segmento> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }

            List<Models.Segmento> lista = new List<Models.Segmento>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.Segmento
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Models.Segmento segmento = new Models.Segmento();

                segmento.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                segmento.IdCorporacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdCorporacao"].ToString());
                segmento.IdSituacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdSituacao"].ToString());
                segmento.Nome = dataTable.Rows[i][nomeBase + "Nome"].ToString();
                segmento.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                segmento.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(segmento);
                    continue;
                }


                // carrega as demais entidades necessárias
                segmento = CarregarEntidades(Entidades, segmento);

                lista.Add(segmento);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }


        /// <summary>
        /// Carrega as demais entidades relacionadas a este objeto
        /// </summary>
        /// <param name="modulos">lista de módulos que serão utilizados para carregar as demais entidades</param>
        /// <param name="objSegmento">Objeto do tipo Models.Base que será convertido em Models.Segmento</param>
        protected Models.Segmento CarregarEntidades(Prion.Tools.Entidade modulos, Prion.Generic.Models.Base objSegmento)
        {
            Boolean modoCompleto = Entidades.Carregar(Helpers.Entidade.Tudo.ToString());
            Models.Segmento segmento = (Models.Segmento)objSegmento;


            // verifica se possui o módulo COMPLETO ou se possui o módulo CORPORACAO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Corporacao.ToString())))
            {
                Prion.Generic.Repository.Corporacao repCorporacao = new Prion.Generic.Repository.Corporacao(ref this._conexao);

                repCorporacao.Entidades = modulos;
                segmento.Corporacao = repCorporacao.BuscarPeloId(segmento.IdCorporacao);
            }


            // verifica se possui o módulo COMPLETO ou se possui o módulo SITUACAO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Situacao.ToString())))
            {
                Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this._conexao);
                repSituacao.Entidades = modulos;

                // carrega um objeto do tipo Models.Situacao, representando a situação do Segmento
                segmento.Situacao = (Prion.Generic.Models.Situacao)repSituacao.BuscarPeloId(segmento.IdSituacao).Get(0);
            }


            // verifica se possui o módulo COMPLETO ou se possui o módulo CURSO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Curso.ToString())))
            {
                Repository.Curso repCurso = new Repository.Curso(ref this._conexao, segmento);
                repCurso.Entidades = modulos;

                // carrega uma lista de objetos do tipo Models.Cursos
                Prion.Tools.Request.ParametrosRequest p = new Request.ParametrosRequest();
                p.Filtro.Add(new Prion.Tools.Request.Filtro("IdSegmento", "=", segmento.Id.ToString()));

                segmento.Cursos = ListTo.CollectionToList<Models.Curso>(repCurso.Buscar(p).ListaObjetos);
            }


            // retorna o objeto de Segmento com as entidades que foram carregadas
            return segmento;
        }


        ///// <summary>
        ///// Salva todos os objetos relacionados à Segmento
        ///// </summary>
        ///// <param name="objetoPai">Objeto do tipo Models.Base que será convertido em Models.Segmento</param>
        ///// <returns></returns>
        //protected override Prion.Generic.Helpers.Retorno SalvarEntidade(Prion.Generic.Models.Base objetoPai, Int32 idSegmento)
        //{
        //    Models.Segmento oSegmento = (Models.Segmento)objetoPai;
        //    Prion.Generic.Helpers.Retorno retorno = null;


        //    // ************************************************************************************
        //    // NÃO HÁ NECESSIDADE DE SALVAR O OBJETO MODELS.CORPORACAO E MODELS.SITUACAO
        //    // Esses objetos não são alterados pelo usuário
        //    // ************************************************************************************


        //    if (oSegmento.Cursos != null)
        //    {
        //        // salva a lista de objetos do tipo Models.Curso
        //        Repository.Curso repCurso = new Repository.Curso(ref this._conexao);
        //        retorno = repCurso.Salvar(oSegmento.Cursos);

        //        if (!retorno.Sucesso) { return retorno; }
        //    }

        //    return retorno;
        //}


        /// <summary>
        /// Grava o log de Segmento
        /// </summary>
        /// <param name="idRegistro">id do registro</param>
        /// <param name="tipoOperacaoLog">tipo de operação: INSERIR, ALTERAR, EXCLUIR</param>
        /// <returns>true/false </returns>
        protected virtual Boolean GravarLog(Int32 idRegistro, TipoOperacaoLog tipoOperacaoLog)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sqlLog = "if EXISTS (SELECT Id FROM Segmento WHERE Id = @Id) " +
                            "INSERT INTO SegmentoLog(IdUsuarioLog, IdOperacaoLog, Id, IdCorporacao, IdSituacao, Nome, DataCadastro) " +
                            "SELECT @IdUsuarioLog, @IdOperacaoLog, @Id, IdCorporacao, IdSituacao, Nome, DataCadastro FROM Segmento WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "SegmentoLog");
        }
    }
}