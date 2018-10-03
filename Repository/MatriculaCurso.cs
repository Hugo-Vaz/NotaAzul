using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Prion.Data;
using Prion.Tools;

namespace NotaAzul.Repository
{
    public class MatriculaCurso : Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public MatriculaCurso(ref DBFacade conexao)
            : base(ref conexao)
        {
        }

        ~MatriculaCurso()
        {
        }


        /// <summary>
        /// Busca todas as MatriculaCurso de uma mesma matrícula.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Int32 idMatricula)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "MatriculaCurso.* ";
            String join = "FROM MatriculaCurso ";
            String whereDefault = "WHERE IdMatricula = "+idMatricula.ToString();
            String orderBy = " MatriculaCurso.IdMatricula";
            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy);
            List<Models.MatriculaCurso> listaMatriculaCurso = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaMatriculaCurso = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaMatriculaCurso!=null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaMatriculaCurso);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase,lista.Count);
            return listaObj;
        }
        

        /// <summary>
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros sem WHERE
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista DataTable(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder campos = new StringBuilder(), join = new StringBuilder();
            campos.Append("MatriculaCurso.* ");
            join.Append("FROM MatriculaCurso");

            String whereDefault = "";
            String orderBy = "MatriculaCurso.DataCadastro";
            String groupBy = "";

            return this.Select("SELECT " + campos.ToString(), join.ToString(), whereDefault, orderBy,groupBy, parametro);
        }


        /// <summary>
        /// Insere um registro em MatriculaCurso através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.MatriculaCurso</param>
        /// <returns></returns>
        protected override int Inserir(Prion.Generic.Models.Base objeto){
            Models.MatriculaCurso oMatriculaCurso= (Models.MatriculaCurso) objeto;
            List<DbParameter> parametros= new List<DbParameter>();
            String sql=" INSERT INTO MatriculaCurso(IdMatricula, IdTurma) VALUES (@IdMatricula, @IdTurma)";
            parametros.Add(this.Conexao.CriarParametro("@IdMatricula",DbType.Int32, oMatriculaCurso.IdMatricula));
            parametros.Add(this.Conexao.CriarParametro("@IdTurma",DbType.Int32, oMatriculaCurso.IdTurma));

            Int32 id = this.Conexao.Insert(sql, parametros);
           
            GravarLog(id, TipoOperacaoLog.Inserir);

            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.MatriculaCurso</param>
        /// <returns></returns>
        protected override int Alterar(Prion.Generic.Models.Base objeto)
        {
            return 0;
        }

        /// <summary>
        /// Exclui um registro de MatriculaCurso através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.MatriculaCurso</param>
        /// <returns>Quantidade de registros excluídos</returns>
        protected override Int32 Excluir(Prion.Generic.Models.Base objeto)
        {
            Models.MatriculaCurso oMatriculaCurso = (Models.MatriculaCurso)objeto;    
            return Excluir(oMatriculaCurso.Id);
        }


        /// <summary>
        /// Exclui um registro de MatriculaCurso através do Id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>Quantidade de registros excluídos</returns>
        public Int32 Excluir(params Int32[] ids)
        {
            //Se o parâmetro for NULL,retorna 0, informando que não excluiu nenhum registro
            if (ids == null) { return 0; }

            if (VerificarPossibilidadeDeExclusao(ids) == false) { return 0; }
            
            for (Int32 indice = 0; indice < ids.Length; indice++)
            {
                //    GravarLog(ids[indice], TipoOperacaoLog.Excluir);
            }

            Int32 retorno = -1;
            String strId = Conversor.ToString(",",ids);            
                                   
            String sql = "DELETE FROM MatriculaCurso WHERE Id IN(" + strId + ")";
            
            
            return retorno;
        }


        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de MatriculaCurso
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.MatriculaCurso com os registros do DataTable</returns>
        public List<Models.MatriculaCurso> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }

            List<Models.MatriculaCurso> lista = new List<Models.MatriculaCurso>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.MatriculaCurso
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Models.MatriculaCurso matriculaCurso = new Models.MatriculaCurso();

                matriculaCurso.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                matriculaCurso.IdMatricula = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdMatricula"].ToString());
                matriculaCurso.IdTurma = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdTurma"].ToString());
                matriculaCurso.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                matriculaCurso.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(matriculaCurso);
                    continue;
                }

                // carrega as demais entidades necessárias
                matriculaCurso = CarregarEntidades(Entidades, matriculaCurso);

                lista.Add(matriculaCurso);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }


        /// <summary>
        /// Grava o log de MatriculaCurso
        /// </summary>
        /// <param name="idRegistro">id do registro</param>
        /// <param name="tipoOperacaoLog">tipo de operação: INSERIR, ALTERAR, EXCLUIR</param>
        /// <returns>true/false </returns>
        protected Boolean GravarLog(Int32 idRegistro,TipoOperacaoLog tipoOperacaoLog)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sqlLog="IF EXISTS (SELECT Id FROM MatriculaCurso WHERE Id=@Id)"+
                    "INSERT INTO MatriculaCursoLog(IdUsuarioLog,IdOperacaoLog,Id, IdMatricula, IdTurma,DataCadastro)"+
                    "SELECT @IdUsuarioLog, @IdOperacaoLog, @Id, IdMatricula, IdTurma, DataCadastro    FROM MatriculaCurso WHERE Id=@Id";
   
            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog",DbType.Int32,UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog",DbType.Int32,(Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id",DbType.Int32,idRegistro));
            return Log.GravarLog(sqlLog,parametros,"MatriculaCurso");

        }

        /// <summary>
        /// Carrega as demais entidades relacionadas a este objeto
        /// </summary>
        /// <param name="modulos">lista de módulos que serão utilizados para carregar as demais entidades</param>
        /// <param name="objTurno">Objeto do tipo Models.Base que será convertido em Models.MatriculaCurso</param>
        protected Models.MatriculaCurso CarregarEntidades(Prion.Tools.Entidade modulos, Prion.Generic.Models.Base objMatriculaCurso)
        {
            Boolean modoCompleto = Entidades.Carregar(Helpers.Entidade.Tudo.ToString());
            Models.MatriculaCurso matriculaCurso = (Models.MatriculaCurso)objMatriculaCurso;

            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Turma.ToString())))
            {
                Repository.Turma repTurma = new Repository.Turma(ref this._conexao);
                repTurma.Entidades = modulos;

                matriculaCurso.Turma = (Models.Turma)repTurma.Buscar(matriculaCurso.IdTurma).Get(0);
            }

            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Mensalidade.ToString())))
            {
                Repository.Mensalidade repMensalidade = new Repository.Mensalidade(ref this._conexao);
                repMensalidade.Entidades = modulos;

                matriculaCurso.Mensalidades = Prion.Tools.ListTo.CollectionToList<Models.Mensalidade>(repMensalidade.Buscar(matriculaCurso.Id).ListaObjetos);
            }

            return matriculaCurso;
        }

        /// <summary>
        /// Salva todos os relacionamentos de matricula
        /// </summary>
        /// <param name="objeto">Objeto base que será convertido para um objeto do tipo Models.Matricula</param>
        /// <param name="idMatriculaCurso">id do último registro inserido</param>
        protected override Prion.Generic.Helpers.Retorno SalvarEntidade(Prion.Generic.Models.Base objeto, Int32 idMatriculaCurso)
        {
            Models.MatriculaCurso matriculaCurso = (Models.MatriculaCurso)objeto;
            Repository.Mensalidade repMensalidade = new Repository.Mensalidade(ref this._conexao);

            // se o objeto de aluno estiver no estado 'NOVO', 
            // chama o método que irá definir o seu id (idAluno) em todos os objetos da lista de responsáveis
            if (matriculaCurso.EstadoObjeto == Prion.Generic.Helpers.Enums.EstadoObjeto.Novo)
            {
                AtualizarIdMatriculaCurso(matriculaCurso, idMatriculaCurso);
            }

            Prion.Generic.Helpers.Retorno retorno = repMensalidade.Salvar(matriculaCurso.Mensalidades);

            return retorno;
        }


        /// <summary>
        /// Atualiza o Idmatricula de todos os registros de matriculaCurso
        /// </summary>
        /// <param name="aluno"></param>
        /// <param name="idMatriculaCurso"></param>
        private void AtualizarIdMatriculaCurso(Models.MatriculaCurso matriculaCurso, Int32 idMatriculaCurso)
        {
            if (matriculaCurso == null)
            {
                return;
            }

            for (Int32 i = 0; i < matriculaCurso.Mensalidades.Count; i++)
            {
                matriculaCurso.Mensalidades[i].IdMatriculaCurso = idMatriculaCurso;
            }
        }

        /// <summary>
        /// Verifica se um registro de MatriculaCurso pode ser excluído
        /// </summary>
        /// <param name="ids">id de MatriculaCurso</param>
        /// <returns>True,caso exista alguma mensalidade paga,retorna false</returns>
        protected Boolean VerificarPossibilidadeDeExclusao(params Int32[] ids)
        {
            Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this._conexao);
            Prion.Generic.Models.Situacao situacaoQuitada = repSituacao.BuscarPelaSituacao("Mensalidade", "Quitada");

            String strIds = Conversor.ToString(",", ids);

             String sql = "SELECT * FROM Mensalidade INNER JOIN MatriculaCurso on Mensalidade.IdMatriculaCurso=MatriculaCurso.Id INNER JOIN Turma on Turma.Id = MatriculaCurso.IdTurma inner join CursoAnoLetivo"+
                "  on CursoAnoLetivo.Id = Turma.IdCursoAnoLetivo inner join Curso on Curso.Id = CursoAnoLetivo.IdCurso  " +
                " WHERE MatriculaCurso.IdMatricula IN (" + strIds + ")"
                + "AND Mensalidade.IdSituacao = " + situacaoQuitada.Id.ToString()+" Curso.CursoCurricular=1 ";

            Prion.Generic.Models.Lista lista = this.Select(sql,null);
     
            return (lista.Count == 0);
        }
    }
}