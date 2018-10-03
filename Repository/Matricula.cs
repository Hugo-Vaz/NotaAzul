using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Prion.Data;
using Prion.Tools;

namespace NotaAzul.Repository
{
    public class Matricula : Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public Matricula(ref DBFacade conexao)
            : base(ref conexao)
        { 
        }


        ~Matricula()
        { 
        }


        /// <summary>
        /// Busca todas as Matriculas da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "Matricula.* ";
            String join = "FROM Matricula";
            String whereDefault = "";
            String orderBy = "Matricula.Id";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy,groupBy, parametro);
            List<Models.Matricula> listaMatricula = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaMatricula = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaMatricula != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaMatricula);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }

        /// <summary>
        /// Retorna, através de um (ou vários) ID, uma lista de objetos do tipo Models.Matricula
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
            StringBuilder campos = new StringBuilder(), join = new StringBuilder(), stuff = new StringBuilder();
            campos.Append("Matricula.* ");
            join.Append("FROM Matricula");
            stuff.Append(" ");
            
            if (Entidades.Carregar(Helpers.Entidade.Situacao.ToString()))
            {
                campos.Append(", " + Prion.Generic.Helpers.MapeamentoTabela.Situacao());
                join.Append(" INNER JOIN Situacao ON Matricula.IdSituacao = Situacao.Id ");
            }

            if (Entidades.Carregar(Helpers.Entidade.Aluno.ToString()))
            {
                campos.Append(", " + Helpers.MapeamentoTabela.Aluno());
                join.Append(" INNER JOIN Aluno ON Matricula.IdAluno = Aluno.Id ");
            }
            
            if (Entidades.Carregar(Helpers.Entidade.MatriculaCurso.ToString()))
            {
                // feito assim para concatenar vários registros em uma única linha
                stuff.Append(", STUFF ( (SELECT DISTINCT ', ' + Turma.Nome  " +
                                "FROM Turma " +
                                "INNER JOIN MatriculaCurso ON MatriculaCurso.IdTurma = Turma.Id " +
                                "Where MatriculaCurso.IdMatricula=Matricula.Id " +
                                "FOR XML PATH(''), TYPE).value('.', 'varchar(max)'), 1, 2, '') as Turmas ");

                stuff.Append(", STUFF ( (SELECT DISTINCT ', ' + Turno.Nome " +
                                "FROM Turno " +
                                "INNER JOIN Turma ON Turno.Id = Turma.IdTurno " +
                                "INNER JOIN MatriculaCurso ON MatriculaCurso.IdTurma = Turma.Id " +
                                "Where MatriculaCurso.IdMatricula=Matricula.Id " +
                                "FOR XML PATH(''), TYPE).value('.', 'varchar(max)'), 1, 2, '') as Turnos ");

                stuff.Append(", STUFF ( (SELECT DISTINCT ', ' + Curso.Nome " +
                                "FROM Curso " +
                                "INNER JOIN CursoAnoLetivo ON CursoAnoLetivo.IdCurso=Curso.Id " +
                                "INNER JOIN Turma ON Turma.IdCursoAnoLetivo = CursoAnoLetivo.Id " +
                                "INNER JOIN MatriculaCurso ON MatriculaCurso.IdTurma = Turma.Id " +
                                "Where MatriculaCurso.IdMatricula=Matricula.Id " +
                                "FOR XML PATH(''), TYPE).value('.', 'varchar(max)'), 1, 2, '') as Cursos ");
            }

            String whereDefault = "";
            String orderBy = "Matricula.DataCadastro DESC";
            String groupBy = "";

            return this.Select("SELECT " + campos.ToString() + stuff.ToString(), join.ToString(), whereDefault, orderBy,groupBy, parametro);
        }


        /// <summary>
        /// Carrega uma lista de objetos do tipo Models.Observacao, representando todas as observações relacionadas a esta matricula
        /// </summary>
        /// <param name="idMatricula"></param>
        /// <param name="parametro">Lista de parâmetros utilizado para filtrar uma matricula através da sua Situação</param>
        /// <returns></returns>
        public List<Models.Observacao> BuscarObservacoes(Int32 idMatricula, Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            DataTable dataTable = null;
            List<Models.Observacao> lista = null;
            List<DbParameter> parametros = new List<DbParameter>();

            String sql = "SELECT O.* FROM Matricula M " +
                            "INNER JOIN MatriculaObservacao MO ON M.Id = MO.IdMatricula " +
                            "INNER JOIN Observacao O ON MO.IdObservacao = O.Id " +
                            "WHERE (M.Id = @IdMatricula) " +
                            "O.DataCadastro DESC";

            parametros.Add(this.Conexao.CriarParametro("@IdMatricula", DbType.Int32, idMatricula));

            Prion.Tools.FormatadoraSQL f = new FormatadoraSQL(Sistema.TipoBancoDados);
            dataTable = this.Conexao.Select(sql);

            Repository.Observacao repObservacao = new Repository.Observacao(ref this._conexao);
            lista = repObservacao.DataTableToObject(dataTable);

            return lista;
        }

        public bool ChecarSeExisteMatriculaParaAnoLetivo(Int32 anoLetivo,Int32 idAluno)
        {
            List<DbParameter> parametros = new List<DbParameter>();

            string sql = @"Select Count(Aluno.Nome) from MatriculaCurso
                INNER JOIN Matricula ON MatriculaCurso.IdMatricula = Matricula.Id
                INNER JOIN Aluno ON Aluno.Id = Matricula.IdAluno
                INNER JOIN Turma ON MatriculaCurso.IdTurma = Turma.Id
                INNER JOIN CursoAnoLetivo ON Turma.IdCursoAnoLetivo = CursoAnoLetivo.Id
                INNER JOIN Curso ON Curso.Id = CursoAnoLetivo.IdCurso
                Where CursoAnoLetivo.AnoLetivo = @AnoLetivo and Aluno.Id = @IdAluno and Curso.CursoCurricular = 1 and Matricula.IdSituacao <> 
                (SELECT Situacao.Id FROM Situacao INNER JOIN SituacaoTipo ON SituacaoTipo.Id = Situacao.IdSituacaoTipo WHERE Situacao.Nome ='Excluído' AND SituacaoTipo.Nome='Matrícula')";

            parametros.Add(this.Conexao.CriarParametro("@AnoLetivo", DbType.Int32, anoLetivo));
            parametros.Add(this.Conexao.CriarParametro("@IdAluno", DbType.Int32, idAluno));

            Prion.Generic.Models.Lista list = this.Select(sql, parametros);

            return Convert.ToInt32(list.DataTable.Rows[0][0].ToString()) < 1;
        }


        /// <summary>
        /// Insere um registro em Matricula através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Matricula</param>
        /// <returns></returns>
        protected override Int32 Inserir(Prion.Generic.Models.Base objeto)
        {
            Models.Matricula oMatricula = (Models.Matricula) objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO Matricula(IdAluno, IdSituacao,NumeroMatricula) " +
                        "VALUES(@IdAluno, @IdSituacao, @NumeroMatricula)";
                       
            
            parametros.Add(this.Conexao.CriarParametro("@IdAluno", DbType.Int32, oMatricula.IdAluno));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, oMatricula.IdSituacao));
            parametros.Add(this.Conexao.CriarParametro("@NumeroMatricula", DbType.String, oMatricula.NumeroMatricula));
            Int32 id = this.Conexao.Insert(sql, parametros);

            GravarLog(id, TipoOperacaoLog.Inserir);

            return id;
        }


        /// <summary>
        /// Altera um registro de Matricula através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Matricula</param>
        /// <returns></returns>
        protected override Int32 Alterar(Prion.Generic.Models.Base objeto)
        {
            Models.Matricula oMatricula = (Models.Matricula) objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE Matricula SET " +
                        "IdAluno=@IdAluno, IdSituacao=@IdSituacao, NumeroMatricula=@NumeroMatricula " + 
                        "WHERE Id=@Id";

            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, oMatricula.Id));            
            parametros.Add(this.Conexao.CriarParametro("@IdAluno", DbType.Int32, oMatricula.IdAluno));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, oMatricula.IdSituacao));
            parametros.Add(this.Conexao.CriarParametro("@NumeroMatricula", DbType.String, oMatricula.NumeroMatricula));

            Int32 linhasAfetadas = this.Conexao.Update(sql, parametros);

            GravarLog(oMatricula.Id, TipoOperacaoLog.Alterar);

            return linhasAfetadas;
        }




        /// <summary>
        /// Exclui um registro de Matricula através do Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Caso a exclusão seja efetuada retorna um null,caso não retorna uma String</returns>
        //public String Excluir(Int32 id)
        //{
        //    // se o parâmetro for null, retorna 0, informando que não excluiu nenhum registro
        //    if (id == 0) { return "Nenhum registro foi excluído"; }

        //    String mensagem = VerificarPossibilidadeDeExcluirMatricula(id);
        //    String sql;
        //    if (mensagem != null)
        //    {
        //        sql = "UPDATE Matricula SET IdSituacao = " +
        //            " (SELECT Situacao.Id FROM Situacao INNER JOIN SituacaoTipo ON Situacao.IdSituacaoTipo = SituacaoTipo.Id WHERE SituacaoTipo.Nome = 'Matrícula' AND Situacao.Nome = 'Excluído')" +
        //            " WHERE Matricula.Id IN (" + id.ToString() + ");"+
        //            " UPDATE Mensalidade SET IdSituacao = " +
        //            " (SELECT Situacao.Id FROM Situacao INNER JOIN SituacaoTipo ON Situacao.IdSituacaoTipo = SituacaoTipo.Id WHERE SituacaoTipo.Nome = 'Mensalidade' AND Situacao.Nome = 'Inserto')" +
        //            " FROM Mensalidade INNER JOIN MatriculaCurso ON MatriculaCurso.Id = Mensalidade.IdMatriculaCurso WHERE MatriculaCurso.IdMatricula IN (" + id.ToString() + ") AND Mensalidade.IdSituacao <>" +
        //            " (SELECT Situacao.Id FROM Situacao INNER JOIN SituacaoTipo ON Situacao.IdSituacaoTipo = SituacaoTipo.Id WHERE SituacaoTipo.Nome = 'Mensalidade' AND Situacao.Nome = 'Quitada') " +
        //            " UPDATE Titulo SET IdSituacao = " +
        //            " (SELECT Situacao.Id FROM Situacao INNER JOIN SituacaoTipo ON Situacao.IdSituacaoTipo = SituacaoTipo.Id WHERE SituacaoTipo.Nome = 'Título' AND Situacao.Nome = 'Cancelado') " +
        //            " FROM Titulo INNER JOIN MensalidadeTitulo ON MensalidadeTitulo.IdTitulo = Titulo.Id " +
        //            " INNER JOIN Mensalidade ON Mensalidade.Id = MensalidadeTitulo.IdMensalidade " +
        //            " INNER JOIN MatriculaCurso ON MatriculaCurso.Id = Mensalidade.IdMatriculaCurso " +
        //            " WHERE MatriculaCurso.IdMatricula IN (" + id.ToString() + ") AND Titulo.IdSituacao <> " +
        //            " (SELECT Situacao.Id FROM Situacao INNER JOIN SituacaoTipo ON Situacao.IdSituacaoTipo = SituacaoTipo.Id WHERE SituacaoTipo.Nome = 'Título' AND Situacao.Nome = 'Quitado');" +
        //            " UPDATE Boleto SET Boleto.StatusBoleto='Cancelado',Boleto.RemessaGerado='1' FROM Boleto INNER JOIN MensalidadeTitulo ON BoletoTitulo.IdTitulo = MensalidadeTitulo.IdTitulo " +
        //            " INNER JOIN Mensalidade ON Mensalidade.Id = MensalidadeTitulo.IdMensalidade INNER JOIN MatriculaCurso ON Mensalidade.IdMatriculaCurso = MatriculaCurso.Id " +
        //            " INNER JOIN Matricula ON Matricula.Id = MatriculaCurso.IdMatricula  INNER JOIN Aluno ON Aluno.Id = Matricula.IdAluno ";


        //        GravarLog(id, TipoOperacaoLog.Excluir);
        //        this.Conexao.Delete(sql);

        //        return mensagem;
        //    }

        //    GravarLog(id, TipoOperacaoLog.Excluir);
        //    //Só está sendo gravado o log(no caso de exclusão) de matrícula, são necessárias alterações nos logs de MatrículaCurso e Mensalidade

        //    sql = "DELETE FROM Matricula WHERE Id IN(" + id.ToString()+ ")";

        //    this.Conexao.Delete(sql);

        //    return null ;
        //}
        public void Excluir(Int32 id)
        {           
           
            String sql;
            sql = "UPDATE Matricula SET IdSituacao = " +
                " (SELECT Situacao.Id FROM Situacao INNER JOIN SituacaoTipo ON Situacao.IdSituacaoTipo = SituacaoTipo.Id WHERE SituacaoTipo.Nome = 'Matrícula' AND Situacao.Nome = 'Excluído')" +
                " WHERE Matricula.Id IN (" + id.ToString() + ");" +
                " UPDATE Boleto SET Boleto.StatusBoleto='Cancelado',Boleto.RemessaGerado='1' FROM Boleto INNER JOIN BoletoTitulo ON Boleto.Id = BoletoTitulo.IdBoleto INNER JOIN MensalidadeTitulo ON BoletoTitulo.IdTitulo = MensalidadeTitulo.IdTitulo " +
                " INNER JOIN Mensalidade ON Mensalidade.Id = MensalidadeTitulo.IdMensalidade INNER JOIN MatriculaCurso ON Mensalidade.IdMatriculaCurso = MatriculaCurso.Id " +
                " INNER JOIN Matricula ON Matricula.Id = MatriculaCurso.IdMatricula  INNER JOIN Aluno ON Aluno.Id = Matricula.IdAluno " +
                " WHERE Matricula.Id IN (" + id.ToString() + ") AND Boleto.StatusBoleto='Aberto' AND Boleto.DataVencimento > " + DateTime.Now.ToString("yyyy-MM-dd"); 

                GravarLog(id, TipoOperacaoLog.Excluir);
                this.Conexao.Delete(sql);    
                     
        }

        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de Matricula
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.Matricula com os registros do DataTable</returns>
        public List<Models.Matricula> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable==null || dataTable.Rows.Count == 0) { return null; }


            List<Models.Matricula> lista = new List<Models.Matricula>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.Matricula
            for (int i=0; i<dataTable.Rows.Count; i++)
            {
                Models.Matricula matricula = new Models.Matricula();

                matricula.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());                
                matricula.IdAluno = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdAluno"].ToString());
                matricula.IdSituacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdSituacao"].ToString());
                matricula.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                matricula.NumeroMatricula = dataTable.Rows[i][nomeBase + "NumeroMatricula"].ToString();
                matricula.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(matricula);
                    continue;
                }

                // carrega as demais entidades necessárias
                matricula = CarregarEntidades(Entidades, matricula);

                lista.Add(matricula);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista; 
        }


        /// <summary>
        /// Carrega as demais entidades relacionadas a este objeto
        /// </summary>
        /// <param name="modulos">lista de módulos que serão utilizados para carregar as demais entidades</param>
        /// <param name="objTurno">Objeto do tipo Models.Base que será convertido em Models.Matricula</param>
        protected Models.Matricula CarregarEntidades(Prion.Tools.Entidade modulos, Prion.Generic.Models.Base objMatricula)
        {
            Boolean modoCompleto = Entidades.Carregar(Helpers.Entidade.Tudo.ToString());
            Models.Matricula matricula = (Models.Matricula)objMatricula;

            // verifica se possui o módulo COMPLETO ou se possui o módulo SITUACAO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Situacao.ToString())))
            {
                Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this._conexao);
                repSituacao.Entidades = modulos;

                // carrega um objeto do tipo Models.Situacao, representando a situação do Matricula
                matricula.Situacao = (Prion.Generic.Models.Situacao)repSituacao.BuscarPeloId(matricula.IdSituacao).Get(0);
            }
                                   
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Observacao.ToString())))
            {
                Prion.Tools.Request.ParametrosRequest param = new Request.ParametrosRequest();
                matricula.Observacoes = BuscarObservacoes(matricula.Id);
            }

            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Aluno.ToString())))
            {
                Repository.Aluno repAluno = new Repository.Aluno(ref this._conexao);
                repAluno.Entidades = modulos;

                matricula.Aluno = (Models.Aluno)repAluno.Buscar(matricula.IdAluno).ListaObjetos[0];

            }
          
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.MatriculaCurso.ToString())))
            {
                Repository.MatriculaCurso repMatriculaCurso = new MatriculaCurso(ref this._conexao);
                repMatriculaCurso.Entidades = modulos;

                matricula.ListaMatriculaCurso =Prion.Tools.ListTo.CollectionToList<Models.MatriculaCurso>(repMatriculaCurso.Buscar(matricula.Id).ListaObjetos);
            }

            // retorna o objeto de Matricula com as entidades que foram carregadas
            return matricula;
        }


        /// <summary>
        /// Grava o log de Matricula
        /// </summary>
        /// <param name="idRegistro">id do registro</param>
        /// <param name="tipoOperacaoLog">tipo de operação: INSERIR, ALTERAR, EXCLUIR</param>
        /// <returns>true/false </returns>
        protected Boolean GravarLog(Int32 idRegistro, TipoOperacaoLog tipoOperacaoLog)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sqlLog = " if EXISTS (SELECT Id FROM Matricula WHERE Id = @Id) " +
                            " INSERT INTO MatriculaLog(IdUsuarioLog, IdOperacaoLog, Id, " + 
                                "IdAluno, IdSituacao, DataCadastro, NumeroMatricula)" +
                            " SELECT @IdUsuarioLog, @IdOperacaoLog, @Id, " + 
                                "IdAluno, IdSituacao, DataCadastro, NumeroMatricula FROM Matricula WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "MatriculaLog");
        }


        /// <summary>
        /// Salva todos os relacionamentos de matricula
        /// </summary>
        /// <param name="objeto">Objeto base que será convertido para um objeto do tipo Models.Matricula</param>
        /// <param name="idMatricula">id do último registro inserido</param>
        protected override Prion.Generic.Helpers.Retorno SalvarEntidade(Prion.Generic.Models.Base objeto, Int32 idMatricula)
        {
            Models.Matricula matricula = (Models.Matricula)objeto;
            Repository.MatriculaCurso repMatriculaCurso = new Repository.MatriculaCurso(ref this._conexao);
            Repository.Boleto repBoleto = new Repository.Boleto(ref this._conexao);

            Repository.AlunoFilantropia repAlunoFilantropia = new Repository.AlunoFilantropia(ref this._conexao);

            // se o objeto de aluno estiver no estado 'NOVO', 
            // chama o método que irá definir o seu id (idMatricula) em todos os objetos da lista de MatrículaCurso
            if (matricula.EstadoObjeto == Prion.Generic.Helpers.Enums.EstadoObjeto.Novo)
            {
                AtualizarIdMatricula(matricula, idMatricula);
            }

            if (matricula.AlunoFilantropia.ValorBolsa != 0)
            {
                repAlunoFilantropia.Salvar(matricula.AlunoFilantropia);
            }

            /*for(Int32 i=0,len = matricula.ListaBoletos.Count;i< len; i++)
            {
                Prion.Generic.Helpers.Retorno retBoleto = repBoleto.Salvar(matricula.ListaBoletos[i]);
                for(Int32 j=0,len2 = matricula.ListaMatriculaCurso.Count;j< len2; j++)
                {
                    matricula.ListaMatriculaCurso[j].Mensalidades[i].IdBoleto = retBoleto.UltimoId;
                }
            }*/

            Prion.Generic.Helpers.Retorno retorno = repMatriculaCurso.Salvar(matricula.ListaMatriculaCurso);

            return retorno;
        }


        /// <summary>
        /// Atualiza o Idmatricula de todos os registros de matriculaCurso
        /// </summary>
        /// <param name="aluno"></param>
        /// <param name="idMatricula"></param>
        private void AtualizarIdMatricula(Models.Matricula matricula, Int32 idMatricula)
        {
            if (matricula == null)
            {
                return;
            }

            for (Int32 i = 0; i < matricula.ListaMatriculaCurso.Count; i++)
            {
                matricula.ListaMatriculaCurso[i].IdMatricula = idMatricula;
            }
        }

        /// <summary>
        /// Verifica se um registro de MatriculaCurso pode ser excluído
        /// </summary>
        /// <param name="idMatricula">id de matrícula</param>
        /// <returns>Null, caso exista alguma mensalidade paga, retorna uma String com uma mensagem de erro</returns>
        protected String VerificarPossibilidadeDeExcluirMatricula(Int32 idMatricula)
        {
            // Seleciona o idSituação para uma mensalidade quitada        

            Prion.Generic.Repository.Situacao repSituacao = new  Prion.Generic.Repository.Situacao(ref this._conexao);
            Prion.Generic.Models.Situacao situacao = repSituacao.BuscarPelaSituacao("Mensalidade", "Quitada");

            if (situacao == null)
            {
                return "Não foi encontrada nenhuma situação";
            }

            String mensagem = null;

            // Seleciona mensalidades quitadas pertencentes às matrículas cujo id está presente no array
            String sql = "SELECT Mensalidade.*,Curso.Nome as NomeCurso FROM Mensalidade " +
                    "INNER JOIN MatriculaCurso ON Mensalidade.IdMatriculaCurso=MatriculaCurso.Id " +
                    "INNER JOIN Matricula ON Matricula.Id=MatriculaCurso.IdMatricula " +
                    "INNER JOIN Turma ON Turma.ID = MatriculaCurso.IdTurma " +
                    "INNER JOIN CursoAnoLetivo ON Turma.IdCursoAnoLetivo =CursoAnoLetivo.Id " +
                    "INNER JOIN Curso ON CursoAnoLetivo.IdCurso = Curso.Id " +
                    "WHERE MatriculaCurso.IdMatricula IN (" + idMatricula.ToString() + ") " +
                    "AND Mensalidade.IdSituacao = " + situacao.Id.ToString() + " AND Curso.CursoCurricular='1' ";

            Prion.Generic.Models.Lista lista = this.Select(sql,null);

            if (lista.Count != 0)
            {
                mensagem = lista.DataTable.Rows[0]["NomeCurso"].ToString();
                
            }
            return mensagem;
        }
    }
 }