using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Prion.Data;
using Prion.Tools;
using System.Text;

namespace NotaAzul.Repository
{
    public class Turma : Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        /// <param name="objBase">Objeto do tipo Models.Curso, que será associado (caso seja diferente != null) à turma</param>
        public Turma(ref DBFacade conexao, Models.Curso objBase = null)
            : base(ref conexao, objBase)
        {
        }


        ~Turma()
        {
        }


        /// <summary>
        /// Busca todas as Turmas da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "Turma.* ";
            String join = "FROM Turma ";
            String whereDefault = "";
            String orderBy = "Turma.Nome";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy,groupBy, parametro);
            List<Models.Turma> listaTurma = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaTurma = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaTurma != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaTurma);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }


        /// <summary>
        /// Retorna, através de um (ou vários) ID, uma lista de objetos do tipo Models.Turma
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
            campos.Append("Turma.* ");
            join.Append("FROM Turma");

            if (Entidades.Carregar(Helpers.Entidade.Situacao.ToString()))
            {
                campos.Append(", " + Prion.Generic.Helpers.MapeamentoTabela.Situacao());
                join.Append(" INNER JOIN Situacao ON Turma.IdSituacao = Situacao.Id ");
            }

            if (Entidades.Carregar(Helpers.Entidade.CursoAnoLetivo.ToString()))
            {
                campos.Append(", " + Helpers.MapeamentoTabela.Curso());
                join.Append(" INNER JOIN CursoAnoLetivo ON Turma.IdCursoAnoLetivo = CursoAnoLetivo.Id " +
                            " INNER JOIN Curso ON Curso.Id = CursoAnoLetivo.IdCurso ");
            }

            if (Entidades.Carregar(Helpers.Entidade.Turno.ToString()))
            {
                campos.Append(", " + Helpers.MapeamentoTabela.Turno());
                join.Append(" INNER JOIN Turno ON Turma.IdTurno = Turno.Id ");
            }

            if (Entidades.Carregar(Helpers.Entidade.Funcionario.ToString()))
            {
                Prion.Tools.Request.Filtro f = new Request.Filtro("FuncionarioUsuario.IdUsuario", "=", UsuarioLogado.Id);
                parametro.Filtro.Add(f);

                join.Append(" INNER JOIN ProfessorDisciplina ON ProfessorDisciplina.IdTurma = Turma.Id ");
                join.Append(" INNER JOIN FuncionarioUsuario ON FuncionarioUsuario.IdFuncionario = ProfessorDisciplina.IdFuncionario ");
            }

            String whereDefault = "";
            String orderBy = "Turma.Nome";
            String groupBy = "";
            String distinct = (parametro.Paginar == true) ? "" : " DISTINCT ";

            return this.Select("SELECT " + distinct + campos.ToString(), join.ToString(), whereDefault, orderBy, groupBy, parametro);
        }
        

        /// <summary>
        /// Insere um registro em Turma através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Turma</param>
        /// <returns></returns>
        protected override Int32 Inserir(Prion.Generic.Models.Base objeto)
        {
            Models.Turma oTurma = (Models.Turma)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO Turma(IdCorporacao, IdCursoAnoLetivo, IdSituacao, IdTurno, Nome, QuantidadeVagas) " +
                            "VALUES(@IdCorporacao, @IdCursoAnoLetivo, @IdSituacao, @IdTurno, @Nome, @QuantidadeVagas)";

            parametros.Add(this.Conexao.CriarParametro("@IdCorporacao", DbType.Int32, UsuarioLogado.IdCorporacao));
            parametros.Add(this.Conexao.CriarParametro("@IdCursoAnoLetivo", DbType.Int32, oTurma.IdCursoAnoLetivo));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, oTurma.IdSituacao));
            parametros.Add(this.Conexao.CriarParametro("@IdTurno", DbType.Int32, oTurma.IdTurno));
            parametros.Add(this.Conexao.CriarParametro("@Nome", DbType.String, StringHelper.OnlyOneSpace(oTurma.Nome), 100));
            parametros.Add(this.Conexao.CriarParametro("@QuantidadeVagas", DbType.Int32, oTurma.QuantidadeVagas));

            Int32 id = this.Conexao.Insert(sql, parametros);

            GravarLog(id, TipoOperacaoLog.Inserir);

            return id;
        }


        /// <summary>
        /// Altera um registro de Turma através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Turma</param>
        /// <returns></returns>
        protected override Int32 Alterar(Prion.Generic.Models.Base objeto)
        {
            Models.Turma oTurma = (Models.Turma)objeto;
            List<DbParameter> parametros = new List<DbParameter>();

            String sql = "UPDATE Turma SET IdCursoAnoLetivo = @IdCursoAnoLetivo, IdSituacao = @IdSituacao, IdTurno = @IdTurno, " +
                            "Nome = @Nome, QuantidadeVagas = @QuantidadeVagas " + 
                            "WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, oTurma.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdCursoAnoLetivo", DbType.Int32, oTurma.IdCursoAnoLetivo));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, oTurma.IdSituacao));
            parametros.Add(this.Conexao.CriarParametro("@IdTurno", DbType.Int32, oTurma.IdTurno));
            parametros.Add(this.Conexao.CriarParametro("@Nome", DbType.String, StringHelper.OnlyOneSpace(oTurma.Nome), 100));
            parametros.Add(this.Conexao.CriarParametro("@QuantidadeVagas", DbType.Int32, oTurma.QuantidadeVagas));

            Int32 linhasAfetadas = this.Conexao.Update(sql, parametros);

            GravarLog(oTurma.Id, TipoOperacaoLog.Alterar);

            return linhasAfetadas;
        }


        /// <summary>
        /// Exclui um registro de Turma através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Turma</param>
        /// <returns>Quantidade de registros excluídos</returns>
        protected override Int32 Excluir(Prion.Generic.Models.Base objeto)
        {
            Models.Turma oTurma = (Models.Turma)objeto;
            return Excluir(oTurma.Id);
        }


        /// <summary>
        /// Exclui um registro de Turma através do Id
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
            String sql = "DELETE FROM Turma WHERE Id IN (" + strId + ")";

            retorno = this.Conexao.Delete(sql);

            return retorno;
        }


        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de Turmas
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.Turma com os registros do DataTable</returns>
        public List<Models.Turma> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }

            List<Models.Turma> lista = new List<Models.Turma>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.Turma
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Models.Turma turma = new Models.Turma();

                turma.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                turma.IdCorporacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdCorporacao"].ToString());
                turma.IdCursoAnoLetivo = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdCursoAnoLetivo"].ToString());
                turma.IdSituacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdSituacao"].ToString());
                turma.IdTurno = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdTurno"].ToString());
                turma.Nome = dataTable.Rows[i][nomeBase + "Nome"].ToString();
                turma.QuantidadeVagas = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "QuantidadeVagas"].ToString());
                turma.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                turma.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(turma);
                    continue;
                }


                // carrega as demais entidades necessárias
                turma = CarregarEntidades(Entidades, turma);

                lista.Add(turma);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }


        /// <summary>
        /// Carrega as demais entidades relacionadas a este objeto
        /// </summary>
        /// <param name="modulos">lista de módulos que serão utilizados para carregar as demais entidades</param>
        /// <param name="objTurma">Objeto do tipo Models.Base que será convertido em Models.Turma</param>
        protected Models.Turma CarregarEntidades(Prion.Tools.Entidade modulos, Prion.Generic.Models.Base objTurma)
        {
            Boolean modoCompleto = Entidades.Carregar(Helpers.Entidade.Tudo.ToString());
            Models.Turma turma = (Models.Turma)objTurma;


            // verifica se possui o módulo COMPLETO ou se possui o módulo CURSO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.CursoAnoLetivo.ToString())))
            {
                if (ObjBase == null)
                {
                    Repository.CursoAnoLetivo repCursoAnoLetivo = null;

                    // carrega o objeto Models.CursoAnoLetivo
                    if (repCursoAnoLetivo == null) { repCursoAnoLetivo = new Repository.CursoAnoLetivo(ref this._conexao); }
                    repCursoAnoLetivo.Entidades = modulos;

                    Prion.Tools.Request.ParametrosRequest p = new Request.ParametrosRequest();
                    p.Filtro.Add(new Prion.Tools.Request.Filtro("Id", "=", turma.IdCursoAnoLetivo.ToString()));

                    turma.CursoAnoLetivo = (Models.CursoAnoLetivo)repCursoAnoLetivo.Buscar(p).Get(0);
                }
                else
                {
                    turma.CursoAnoLetivo = (Models.CursoAnoLetivo)ObjBase;
                }
            }


            // verifica se possui o módulo COMPLETO ou se possui o módulo SITUACAO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Situacao.ToString())))
            {
                Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this._conexao);
                repSituacao.Entidades = modulos;

                // carrega um objeto do tipo Models.Situacao, representando a situação da Turma
                turma.Situacao = (Prion.Generic.Models.Situacao)repSituacao.BuscarPeloId(turma.IdSituacao).Get(0);
            }


            // verifica se possui o módulo COMPLETO ou se possui o módulo CORPORACAO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Corporacao.ToString())))
            {
                Prion.Generic.Repository.Corporacao repCorporacao = new Prion.Generic.Repository.Corporacao(ref this._conexao);
                repCorporacao.Entidades = modulos;

                // carrega um objeto do tipo Models.Corporacao
                turma.Corporacao = (Prion.Generic.Models.Corporacao)repCorporacao.BuscarPeloId(turma.IdCorporacao);
            }


            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Turno.ToString())))
            {
                Repository.Turno repTurno = new Repository.Turno(ref this._conexao);
                repTurno.Entidades = modulos;

                // carrega um objeto do tipo Models.Turno
                turma.Turno = (Models.Turno)repTurno.Buscar(turma.IdTurno).Get(0);
            }

            // verifica se possui o módulo COMPLETO ou se possui o módulo DISCIPLINA
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Disciplina.ToString())))
            {

                turma.IdsDisciplina = CarregarRelacionamento(turma.Id);
            }

            // retorna o objeto de Curso com as entidades que foram carregadas
            return turma;
        }

        /// <summary>
        ///Cria o relacionamento entre Turma e Disciplinas
        /// </summary>
        /// <param name="idCursoAnoLetivo"></param>
        /// <param name="idsDisciplina"></param>
        /// <returns></returns>
        public void CriarRelacionamento(Int32 idTurma, Int32 idDisciplina)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO DisciplinaTurma(IdTurma, IdDisciplina) " +
                        "VALUES(@IdTurma, @IdDisciplina)";

            parametros.Add(this.Conexao.CriarParametro("@IdTurma", DbType.Int32, idTurma));
            parametros.Add(this.Conexao.CriarParametro("@IdDisciplina", DbType.Int32, idDisciplina));

            this.Conexao.Insert(sql, parametros);

        }

        /// <summary>
        ///Exclui o relacionamento entre Turma e Disciplinas
        /// </summary>
        /// <param name="idTurma"></param>
        /// <param name="idsDisciplinas"></param>
        /// <returns></returns>
        public void ExcluirRelacionamento(Int32 idTurma, Int32 idDisciplina)
        {

            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "DELETE FROM DisciplinaTurma WHERE(IdTurma = @IdTurma AND IdDisciplina=@IdDisciplina) ";


            parametros.Add(this.Conexao.CriarParametro("@IdTurma", DbType.Int32, idTurma));
            parametros.Add(this.Conexao.CriarParametro("@IdDisciplina", DbType.Int32, idDisciplina));

            this.Conexao.Delete(sql, parametros);
        }

        /// <summary>
        /// Carrega os ids de disciplina de determinada Turma
        /// </summary>       
        /// <param name="idMensalidade"></param>
        /// <param name="IdSituacao"></param>   
        /// <returns>ids das disciplinas</returns>
        public Int32[] CarregarRelacionamento(params Int32[] idsTurma)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "DisciplinaTurma.* ";
            String join = "FROM DisciplinaTurma ";
            String whereDefault = "";
            String orderBy = "DisciplinaTurma.IdTurma";
            String groupBy = "";

            Prion.Tools.Request.ParametrosRequest parametro = new Prion.Tools.Request.ParametrosRequest();
            Prion.Tools.Request.Filtro f = new Request.Filtro("DisciplinaTurma.IdTurma", "IN", Conversor.ToString(",", idsTurma));
            parametro.Filtro.Add(f);

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy, groupBy, parametro);
            Int32 qntResultados = lista.DataTable.Rows.Count;
            Int32[] ids = new Int32[qntResultados];

            for (Int32 i = 0; i < qntResultados; i++)
            {
                ids[i] = Conversor.ToInt32(lista.DataTable.Rows[i]["IdDisciplina"].ToString());
            }

            return ids;

        }


        /// <summary>
        /// Grava o log de Turma
        /// </summary>
        /// <param name="idRegistro">id do registro</param>
        /// <param name="tipoOperacaoLog">tipo de operação: INSERIR, ALTERAR, EXCLUIR</param>
        /// <returns>true/false </returns>
        protected virtual Boolean GravarLog(Int32 idRegistro, TipoOperacaoLog tipoOperacaoLog)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sqlLog = "if EXISTS (SELECT Id FROM Turma WHERE Id = @Id) " +
                            "INSERT INTO TurmaLog(IdUsuarioLog, IdOperacaoLog, Id, IdCorporacao, IdCursoAnoLetivo, " + 
                                "IdSituacao, IdTurno, Nome, QuantidadeVagas, DataCadastro) " +
                            "SELECT @IdUsuarioLog, @IdOperacaoLog, @Id, IdCorporacao, IdCursoAnoLetivo, " + 
                                "IdSituacao, IdTurno, Nome, QuantidadeVagas, DataCadastro FROM Turma WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "TurmaLog");
        }
    }
}