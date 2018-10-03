using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Prion.Data;
using Prion.Tools;
using System.Text;

namespace NotaAzul.Repository
{
    public class Curso : Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        /// <param name="objBase">Objeto do tipo Models.Segmento, que será associado (caso seja diferente != null) ao curso</param>
        public Curso(ref DBFacade conexao, Models.Segmento objBase = null)
            : base(ref conexao, objBase)
        {
        }


        ~Curso()
        {
        }


        /// <summary>
        /// Busca todos os Cursos da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "Curso.* ";
            String join = "FROM Curso";
            String whereDefault = "";
            String orderBy = "Curso.Nome";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy,groupBy, parametro);
            List<Models.Curso> listaCurso = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaCurso = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaCurso != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaCurso);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }


        /// <summary>
        /// Retorna, através de um (ou vários) ID, uma lista de objetos do tipo Models.Curso
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
            campos.Append("Curso.* ");
            join.Append("FROM Curso");

            if (Entidades.Carregar(Helpers.Entidade.Situacao.ToString()))
            {
                campos.Append(", " + Prion.Generic.Helpers.MapeamentoTabela.Situacao());
                join.Append(" INNER JOIN Situacao ON Curso.IdSituacao = Situacao.Id ");
            }

            if (Entidades.Carregar(Helpers.Entidade.Segmento.ToString()))
            {
                campos.Append(", " + Helpers.MapeamentoTabela.Segmento());
                join.Append(" INNER JOIN Segmento ON Curso.IdSegmento = Segmento.Id ");
            }

            String whereDefault = "";
            String orderBy = "Curso.Nome";
            String groupBy = "";

            return this.Select("SELECT " + campos.ToString(), join.ToString(), whereDefault, orderBy,groupBy, parametro);
        }


        /// <summary>
        /// Insere um registro em Curso através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Curso</param>
        /// <returns></returns>
        protected override Int32 Inserir(Prion.Generic.Models.Base objeto)
        {
            Models.Curso oCurso = (Models.Curso)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO Curso(IdCorporacao, IdSegmento, IdSituacao, Nome, CursoCurricular)" +
                            "VALUES(@IdCorporacao, @IdSegmento, @IdSituacao, @Nome, @CursoCurricular)";

            parametros.Add(this.Conexao.CriarParametro("@IdCorporacao", DbType.Int32, UsuarioLogado.IdCorporacao));
            parametros.Add(this.Conexao.CriarParametro("@IdSegmento", DbType.Int32, oCurso.IdSegmento));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, oCurso.IdSituacao)); 
            parametros.Add(this.Conexao.CriarParametro("@Nome", DbType.String, StringHelper.OnlyOneSpace(oCurso.Nome), 100));
            parametros.Add(this.Conexao.CriarParametro("@CursoCurricular", DbType.Boolean, oCurso.CursoCurricular));
            Int32 id = this.Conexao.Insert(sql, parametros);

            GravarLog(id, TipoOperacaoLog.Inserir);

            return id;
        }


        /// <summary>
        /// Altera um registro de Curso através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Curso</param>
        /// <returns></returns>
        protected override Int32 Alterar(Prion.Generic.Models.Base objeto)
        {
            Models.Curso oCurso = (Models.Curso)objeto;
            List<DbParameter> parametros = new List<DbParameter>();

            String sql = "UPDATE Curso SET IdSegmento = @IdSegmento, IdSituacao = @IdSituacao, Nome = @Nome, CursoCurricular = @CursoCurricular " +
                            "WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, oCurso.Id));
            //parametros.Add(this.Conexao.CriarParametro("@IdCorporacao", DbType.Int32, oCurso.IdCorporacao));
            parametros.Add(this.Conexao.CriarParametro("@IdSegmento", DbType.Int32, oCurso.IdSegmento));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, oCurso.IdSituacao));
            parametros.Add(this.Conexao.CriarParametro("@Nome", DbType.String, StringHelper.OnlyOneSpace(oCurso.Nome), 100));
            parametros.Add(this.Conexao.CriarParametro("@CursoCurricular", DbType.Boolean, oCurso.CursoCurricular));
            Int32 linhasAfetadas = this.Conexao.Update(sql, parametros);

            GravarLog(oCurso.Id, TipoOperacaoLog.Alterar);

            return linhasAfetadas;
        }


        /// <summary>
        /// Exclui um registro de Curso através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Curso</param>
        /// <returns>Quantidade de registros excluídos</returns>
        protected override Int32 Excluir(Prion.Generic.Models.Base objeto)
        {
            Models.Curso oCurso = (Models.Curso)objeto;
            return Excluir(oCurso.Id);
        }


        /// <summary>
        /// Exclui um registro de Curso através do Id
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
            String sql = "DELETE FROM Curso WHERE Id IN (" + strId + ")";

            retorno = this.Conexao.Delete(sql);

            return retorno;
        }


        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de usuários
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.Curso com os registros do DataTable</returns>
        public List<Models.Curso> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }

            List<Models.Curso> lista = new List<Models.Curso>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.Curso
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Models.Curso curso = new Models.Curso();

                curso.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                curso.IdCorporacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdCorporacao"].ToString());
                curso.IdSegmento = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdSegmento"].ToString());
                curso.IdSituacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdSituacao"].ToString());
                curso.Nome = dataTable.Rows[i][nomeBase + "Nome"].ToString();
                curso.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                curso.CursoCurricular = Conversor.ToBoolean(dataTable.Rows[i][nomeBase + "CursoCurricular"].ToString());
                curso.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(curso);
                    continue;
                }


                // carrega as demais entidades necessárias
                curso = CarregarEntidades(Entidades, curso);

                lista.Add(curso);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }


        /// <summary>
        /// Carrega as demais entidades relacionadas a este objeto
        /// </summary>
        /// <param name="modulos">lista de módulos que serão utilizados para carregar as demais entidades</param>
        /// <param name="objCurso">Objeto do tipo Models.Base que será convertido em Models.Curso</param>
        protected Models.Curso CarregarEntidades(Prion.Tools.Entidade modulos, Prion.Generic.Models.Base objCurso)
        {
            Boolean modoCompleto = Entidades.Carregar(Helpers.Entidade.Tudo.ToString());
            Models.Curso curso = (Models.Curso)objCurso;


            // verifica se possui o módulo COMPLETO ou se possui o módulo SEGMENTO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Segmento.ToString())))
            {
                if (ObjBase == null)
                {
                    Repository.Segmento repSegmento = new Repository.Segmento(ref this._conexao);
                    repSegmento.Entidades = modulos;

                    // carrega um objeto do tipo Models.Segmento
                    curso.Segmento = (Models.Segmento)repSegmento.Buscar(curso.IdSegmento).Get(0);
                }
                else
                {
                    curso.Segmento = (Models.Segmento)ObjBase;
                }
            }


            // verifica se possui o módulo COMPLETO ou se possui o módulo SITUACAO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Situacao.ToString())))
            {
                Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this._conexao);
                repSituacao.Entidades = modulos;

                // carrega um objeto do tipo Models.Situacao, representando a situação do Curso
                curso.Situacao = (Prion.Generic.Models.Situacao)repSituacao.BuscarPeloId(curso.IdSituacao).Get(0);
            }

            // verifica se possui o módulo COMPLETO ou se possui o módulo CORPORACAO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Corporacao.ToString())))
            {
                Prion.Generic.Repository.Corporacao repCorporacao = new Prion.Generic.Repository.Corporacao(ref this._conexao);
                repCorporacao.Entidades = modulos;

                // carrega um objeto do tipo Models.Corporacao
                curso.Corporacao = (Prion.Generic.Models.Corporacao)repCorporacao.BuscarPeloId(curso.IdCorporacao);
            }

            // retorna o objeto de Curso com as entidades que foram carregadas
            return curso;
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="objetoPai"></param>
        ///// <returns></returns>
        //protected override Prion.Generic.Helpers.Retorno SalvarEntidade(Prion.Generic.Models.Base objetoPai, Int32 idCurso)
        //{
        //    Models.Curso oCurso = (Models.Curso)objetoPai;
        //    Prion.Generic.Helpers.Retorno retorno = null;

        //    // ************************************************************************************
        //    // NÃO HÁ NECESSIDADE DE SALVAR O OBJETO MODELS.SITUACAO
        //    // Esse objeto não é alterado pelo usuário
        //    // ************************************************************************************


        //    if (oCurso.Segmento != null)
        //    {
        //        // salva o objeto Models.Segmento
        //        Repository.Segmento repSegmento = new Repository.Segmento(ref this._conexao);
        //        retorno = repSegmento.Salvar(oCurso.Segmento);

        //        if (!retorno.Sucesso) { return retorno; }
        //    }


        //    if ((oCurso.Turmas != null) && (oCurso.Turmas.Count > 0))
        //    {
        //        // salva a lista de objetos do tipo Models.Turma
        //        Repository.Turma repTurma = new Repository.Turma(ref this._conexao);
        //        retorno = repTurma.Salvar(oCurso.Turmas);

        //        if (!retorno.Sucesso) { return retorno; }
        //    }

        //    return retorno;
        //}


        /// <summary>
        /// Grava o log de Curso
        /// </summary>
        /// <param name="idRegistro">id do registro</param>
        /// <param name="tipoOperacaoLog">tipo de operação: INSERIR, ALTERAR, EXCLUIR</param>
        /// <returns>true/false </returns>
        protected virtual Boolean GravarLog(Int32 idRegistro, TipoOperacaoLog tipoOperacaoLog)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sqlLog = "if EXISTS (SELECT Id FROM Curso WHERE Id = @Id) " +
                            "INSERT INTO CursoLog(IdUsuarioLog, IdOperacaoLog, Id, IdCorporacao, IdSegmento, IdSituacao, Nome,CursoCurricular, DataCadastro) " +
                            "SELECT @IdUsuarioLog, @IdOperacaoLog, @Id, IdCorporacao, IdSegmento, IdSituacao, Nome, CursoCurricular,DataCadastro FROM Curso WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "CursoLog");
        }
    }
}