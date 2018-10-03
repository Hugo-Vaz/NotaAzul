using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Prion.Data;
using Prion.Tools;
using System.Text;

namespace NotaAzul.Repository
{
    public class CursoAnoLetivo : Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public CursoAnoLetivo(ref DBFacade conexao)
            : base(ref conexao)
        { 
        }


        ~CursoAnoLetivo()
        { 
        }


        /// <summary>
        /// Busca todas os CursoAnoLetivo da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "CursoAnoLetivo.* ";
            String join = "FROM CursoAnoLetivo";
            String whereDefault = "";
            String orderBy = "CursoAnoLetivo.Id";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy,groupBy, parametro);
            List<Models.CursoAnoLetivo> listaCursoAnoLetivo = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaCursoAnoLetivo = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaCursoAnoLetivo != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaCursoAnoLetivo);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }


        /// <summary>
        /// Retorna, através de um (ou vários) ID, uma lista de objetos do tipo Models.CursoAnoLetivo
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
            StringBuilder campos = new StringBuilder();
            StringBuilder join = new StringBuilder();
            String whereDefault = "";
            String orderBy = "CursoAnoLetivo.AnoLetivo";
            String groupBy = "";

            campos.Append("CursoAnoLetivo.* ");
            join.Append("FROM CursoAnoLetivo");

            if (Entidades.Carregar(Helpers.Entidade.Curso.ToString()))
            {
                campos.Append(", " + Helpers.MapeamentoTabela.Curso());
                join.Append(" INNER JOIN Curso ON CursoAnoLetivo.IdCurso = Curso.Id ");
                orderBy = "Curso.Nome";
            }

            if (Entidades.Carregar(Helpers.Entidade.Situacao.ToString()))
            {
                campos.Append(", " + Prion.Generic.Helpers.MapeamentoTabela.Situacao());
                join.Append(" INNER JOIN Situacao ON CursoAnoLetivo.IdSituacao = Situacao.Id ");
            }         


            return this.Select("SELECT " + campos.ToString(), join.ToString(), whereDefault, orderBy,groupBy, parametro);
        }
        

        /// <summary>
        /// Insere um registro em CursoAnoLetivo através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.CursoAnoLetivo</param>
        /// <returns></returns>
        protected override Int32 Inserir(Prion.Generic.Models.Base objeto)
        {
            Models.CursoAnoLetivo oCursoAnoLetivo = (Models.CursoAnoLetivo) objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO CursoAnoletivo(IdCurso, IdSituacao, AnoLetivo, ValorMatricula, ValorMensalidade, QuantidadeMensalidades) " + 
                        "VALUES(@IdCurso, @IdSituacao, @AnoLetivo, @ValorMatricula, @ValorMensalidade, @QuantidadeMensalidades)";

            parametros.Add(this.Conexao.CriarParametro("@IdCurso", DbType.Int32, oCursoAnoLetivo.IdCurso));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, oCursoAnoLetivo.IdSituacao));
            parametros.Add(this.Conexao.CriarParametro("@AnoLetivo", DbType.Int32, oCursoAnoLetivo.AnoLetivo));
            parametros.Add(this.Conexao.CriarParametro("@ValorMatricula", DbType.Decimal, oCursoAnoLetivo.ValorMatricula));
            parametros.Add(this.Conexao.CriarParametro("@ValorMensalidade", DbType.Decimal, oCursoAnoLetivo.ValorMensalidade));
            parametros.Add(this.Conexao.CriarParametro("@QuantidadeMensalidades", DbType.Int32, oCursoAnoLetivo.QuantidadeMensalidades));

            Int32 id = this.Conexao.Insert(sql, parametros);

            GravarLog(id, TipoOperacaoLog.Inserir);

            return id;
        }


        /// <summary>
        /// Altera um registro de CursoAnoLetivo através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.CursoAnoLetivo</param>
        /// <returns></returns>
        protected override Int32 Alterar(Prion.Generic.Models.Base objeto)
        {
            Models.CursoAnoLetivo oCursoAnoLetivo = (Models.CursoAnoLetivo) objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE CursoAnoLetivo SET IdCurso=@IdCurso, IdSituacao=@IdSituacao, AnoLetivo=@AnoLetivo, " + 
                        "ValorMatricula=@ValorMatricula, ValorMensalidade=@ValorMensalidade, " + 
                        "QuantidadeMensalidades=@QuantidadeMensalidades WHERE Id=@Id";

            parametros.Add(this.Conexao.CriarParametro("@IdCurso", DbType.Int32, oCursoAnoLetivo.IdCurso));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, oCursoAnoLetivo.IdSituacao));
            parametros.Add(this.Conexao.CriarParametro("@AnoLetivo", DbType.Int32, oCursoAnoLetivo.AnoLetivo));
            parametros.Add(this.Conexao.CriarParametro("@ValorMatricula", DbType.Decimal, oCursoAnoLetivo.ValorMatricula));
            parametros.Add(this.Conexao.CriarParametro("@ValorMensalidade", DbType.Decimal, oCursoAnoLetivo.ValorMensalidade));
            parametros.Add(this.Conexao.CriarParametro("@QuantidadeMensalidades", DbType.Int32, oCursoAnoLetivo.QuantidadeMensalidades));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, oCursoAnoLetivo.Id));

            Int32 linhasAfetadas = this.Conexao.Update(sql, parametros);

            GravarLog(oCursoAnoLetivo.Id, TipoOperacaoLog.Alterar);

            return linhasAfetadas;
        }
        

        /// <summary>
        /// Exclui um registro de CursoAnoLetivo através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.CursoAnoLetivo</param>
        /// <returns>Quantidade de registros excluídos</returns>
        protected override Int32 Excluir(Prion.Generic.Models.Base objeto)
        {
            Models.CursoAnoLetivo oCursoAnoLetivo = (Models.CursoAnoLetivo)objeto;
            return Excluir(oCursoAnoLetivo.Id);
        }


        /// <summary>
        /// Exclui um registro de CursoAnoLetivo através do Id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>Quantidade de registros excluídos</returns>
        public Int32 Excluir(params Int32[] ids)
        {
            // se o parâmetro for null, retorna 0, informando que não excluiu nenhum registro
            if (ids == null) { return 0; }

            for (Int32 indice=0; indice<ids.Length; indice++)
            {
                GravarLog(ids[indice], TipoOperacaoLog.Excluir);
            }

            Int32 retorno = -1;
            String strId = Conversor.ToString(",", ids);
            String sql = "DELETE FROM CursoAnoLetivo WHERE Id IN("+ strId +")";

            retorno = this.Conexao.Delete(sql);
            return retorno;
        }


        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de CursoAnoLetivo
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.CursoAnoLetivo com os registros do DataTable</returns>
        public List<Models.CursoAnoLetivo> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable==null || dataTable.Rows.Count == 0) { return null; }


            List<Models.CursoAnoLetivo> lista = new List<Models.CursoAnoLetivo>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.CursoAnoLetivo
            for (int i=0; i<dataTable.Rows.Count; i++)
            {
                Models.CursoAnoLetivo cursoAnoLetivo = new Models.CursoAnoLetivo();

                cursoAnoLetivo.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                cursoAnoLetivo.IdCurso = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdCurso"].ToString());
                cursoAnoLetivo.IdSituacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdSituacao"].ToString());
                cursoAnoLetivo.AnoLetivo = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "AnoLetivo"].ToString());
                cursoAnoLetivo.ValorMatricula = Conversor.ToDecimal(dataTable.Rows[i][nomeBase + "ValorMatricula"].ToString());
                cursoAnoLetivo.ValorMensalidade = Conversor.ToDecimal(dataTable.Rows[i][nomeBase + "ValorMensalidade"].ToString());
                cursoAnoLetivo.QuantidadeMensalidades = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "QuantidadeMensalidades"].ToString());
                cursoAnoLetivo.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                cursoAnoLetivo.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(cursoAnoLetivo);
                    continue;
                }

                // carrega as demais entidades necessárias
                cursoAnoLetivo = CarregarEntidades(Entidades, cursoAnoLetivo);

                lista.Add(cursoAnoLetivo);
            }

                // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
                return (lista.Count == 0) ? null : lista; 
            }


        /// <summary>
        /// Carrega as demais entidades relacionadas a este objeto
        /// </summary>
        /// <param name="modulos">lista de módulos que serão utilizados para carregar as demais entidades</param>
        /// <param name="objTurno">Objeto do tipo Models.Base que será convertido em Models.CursoAnoLetivo</param>
        protected Models.CursoAnoLetivo CarregarEntidades(Prion.Tools.Entidade modulos, Prion.Generic.Models.Base objCursoAnoLetivo)
        {
            Boolean modoCompleto = Entidades.Carregar(Helpers.Entidade.Tudo.ToString());
            Models.CursoAnoLetivo cursoAnoLetivo = (Models.CursoAnoLetivo)objCursoAnoLetivo;

            // verifica se possui o módulo COMPLETO ou se possui o módulo SITUACAO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Situacao.ToString())))
            {
                Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this._conexao);
                repSituacao.Entidades = modulos;

                // carrega um objeto do tipo Models.Situacao, representando a situação do CursoAnoLetivo
                cursoAnoLetivo.Situacao = (Prion.Generic.Models.Situacao)repSituacao.BuscarPeloId(cursoAnoLetivo.IdSituacao).Get(0);
            }

            // verifica se possui o módulo COMPLETO ou se possui o módulo CURSO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Curso.ToString())))
            {
                Repository.Curso repCurso = new Repository.Curso(ref this._conexao);
                repCurso.Entidades = modulos;

                // carrega um objeto do tipo Models.Curso
                cursoAnoLetivo.Curso = (Models.Curso)repCurso.Buscar(cursoAnoLetivo.IdCurso).Get(0);
            }          

            // retorna o objeto de CursoAnoLetivo com as entidades que foram carregadas
            return cursoAnoLetivo;
        }


        /// <summary>
        /// Grava o log de CursoAnoLetivo
        /// </summary>
        /// <param name="idRegistro">id do registro</param>
        /// <param name="tipoOperacaoLog">tipo de operação: INSERIR, ALTERAR, EXCLUIR</param>
        /// <returns>true/false </returns>
        protected Boolean GravarLog(Int32 idRegistro, TipoOperacaoLog tipoOperacaoLog)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sqlLog = " if EXISTS (SELECT Id FROM CursoAnoLetivo WHERE Id = @Id) " +
                            " INSERT INTO CursoAnoLetivoLog(IdUsuarioLog, IdOperacaoLog, Id, IdCurso, IdSituacao, AnoLetivo, ValorMatricula, ValorMensalidade, QuantidadeMensalidades, DataCadastro)" +
                            " SELECT @IdUsuarioLog, @IdOperacaoLog, @Id, IdCurso, IdSituacao, AnoLetivo, ValorMatricula, ValorMensalidade, QuantidadeMensalidades, DataCadastro FROM CursoAnoLetivo WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "CursoAnoLetivoLog");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected Boolean Existe(Prion.Generic.Models.Base objeto)
        {
            Models.CursoAnoLetivo curso = (Models.CursoAnoLetivo)objeto;

            Prion.Tools.ValidacaoUniqueKey validacao1 = new Prion.Tools.ValidacaoUniqueKey();
            validacao1.Adicionar("IdCurso", curso.IdCurso);
            validacao1.Adicionar("AnoLetivo", curso.AnoLetivo);
            validacao1.Mensagem = String.Format("O Curso informado já esta cadastrado para o ano letivo {0}", curso.AnoLetivo);

            List<Prion.Tools.ValidacaoUniqueKey> lista = new List<ValidacaoUniqueKey>();
            lista.Add(validacao1);

            //return this.ValidarUniqueKey(validacao1);
            return false;
        }
    }
}