using System;
using System.Collections.Generic;
using System.Data;
using Prion.Data;
using Prion.Tools;
using System.Text;


namespace NotaAzul.Repository
{
    public class ContasReceber : Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public ContasReceber(ref DBFacade conexao)
            : base(ref conexao)
        { 
        }


        ~ContasReceber()
        {
        }


        /// <summary>
        /// Busca todas as mensalidades e títulos de um aluno
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Base</returns>
        public Prion.Generic.Models.Lista BuscarTodasMensalidadesDeUmAluno(Int32 idMensalidade)
        {
            String campos = "Titulo.*,MensalidadeTitulo.IdMensalidade,Curso.Nome as NomeCurso, Aluno.Nome as NomeAluno ";
            String join = " FROM Titulo INNER JOIN MensalidadeTitulo ON Titulo.Id = MensalidadeTitulo.IdTitulo "
                +" INNER JOIN Mensalidade ON Mensalidade.Id=MensalidadeTitulo.IdMensalidade "
                +" INNER JOIN MatriculaCurso ON Mensalidade.IdMatriculaCurso = MatriculaCurso.Id "
                + " INNER JOIN Matricula ON MatriculaCurso.IdMatricula = Matricula.Id "
                + " INNER JOIN Aluno ON Aluno.Id = Matricula.IdAluno "
                +" INNER JOIN Turma ON Turma.Id = MatriculaCurso.IdTurma "
                +" INNER JOIN CursoAnoLetivo ON Turma.IdCursoAnoLetivo=CursoAnoLetivo.Id "
                +" INNER JOIN Curso ON CursoAnoLetivo.IdCurso = Curso.Id  ";

            Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this._conexao);
            Prion.Generic.Models.Situacao situacaoAberta = (Prion.Generic.Models.Situacao)repSituacao.BuscarPelaSituacao("Mensalidade", "Aberta");            
            
            
            
            String whereDefault = "WHERE MensalidadeTitulo.IdMensalidade IN(SELECT Mensalidade.Id FROM Mensalidade " 
               +" WHERE Matricula.Id=(SELECT MatriculaCurso.IdMatricula FROM MatriculaCurso" 
               +" WHERE MatriculaCurso.Id =(SELECT Mensalidade.IdMatriculaCurso FROM Mensalidade WHERE Mensalidade.Id = "+idMensalidade.ToString()+" ))"
               +" AND Mensalidade.Isento = 'False' "
               +" AND Mensalidade.IdSituacao ="+ situacaoAberta.Id.ToString()+" )";
            String orderBy = "Titulo.Id";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy);
            List<Models.MensalidadeTitulo> listaTitulos = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaTitulos = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaTitulos != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaTitulos);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;

        }

        /// <summary>
        /// Busca todas as mensalidades e títulos de um aluno
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Base</returns>
        public Prion.Generic.Models.Lista BuscarTodasMensalidadesPagas(Prion.Tools.Request.ParametrosRequest parametro = null)
        {

            StringBuilder campos = new StringBuilder(), join = new StringBuilder();
            campos.Append("Mensalidade.Id ");
            campos.Append(",Aluno.Nome as Nome ");
            campos.Append(",Curso.Nome as Curso");
            campos.Append(",Mensalidade.Valor as Valor");
            campos.Append(",Mensalidade.DataVencimento as Vencimento");

            join.Append(" FROM Mensalidade INNER JOIN MatriculaCurso ON Mensalidade.IdMatriculaCurso = MatriculaCurso.Id "
                + " INNER JOIN Matricula ON MatriculaCurso.IdMatricula = Matricula.Id "
                + " INNER JOIN Aluno ON Aluno.Id = Matricula.IdAluno "
                + " INNER JOIN Turma ON Turma.Id = MatriculaCurso.IdTurma "
                + " INNER JOIN CursoAnoLetivo ON Turma.IdCursoAnoLetivo=CursoAnoLetivo.Id "
                + " INNER JOIN Curso ON CursoAnoLetivo.IdCurso = Curso.Id  ");

            Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this._conexao);
            Prion.Generic.Models.Situacao situacaoConcluida = (Prion.Generic.Models.Situacao)repSituacao.BuscarPelaSituacao("Mensalidade", "Quitada");


            String whereDefault = "WHERE Mensalidade.IdSituacao =" + situacaoConcluida.Id.ToString();
            String orderBy = "Aluno.Nome";
            String groupBy = "";            
            
           

            return this.Select("SELECT " + campos.ToString(), join.ToString(), whereDefault, orderBy,groupBy, parametro,null,"AND");            
        }

        /// <summary>
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros sem WHERE
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista DataTable(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder campos = new StringBuilder(), join = new StringBuilder();
            campos.Append("Mensalidade.* ");
            campos.Append(",Aluno.Nome as Nome ");
            campos.Append(",Curso.Nome as Curso ");
            campos.Append(",Turma.Nome as Turma ");
            campos.Append(",Turno.Nome as Turno");

            join.Append(" FROM Mensalidade INNER JOIN MatriculaCurso ON Mensalidade.IdMatriculaCurso = MatriculaCurso.Id "
                + " INNER JOIN Turma ON Turma.Id = MatriculaCurso.IdTurma "
                + " INNER JOIN Turno ON Turno.Id = Turma.IdTurno "
                + " INNER JOIN CursoAnoLetivo ON Turma.IdCursoAnoLetivo=CursoAnoLetivo.Id "
                + " INNER JOIN Curso ON CursoAnoLetivo.IdCurso = Curso.Id "
                + " INNER JOIN Matricula ON MatriculaCurso.IdMatricula = Matricula.Id "
                + " INNER JOIN Aluno ON Aluno.Id = Matricula.IdAluno"
                );
            Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this._conexao);
            Prion.Generic.Models.Situacao situacaoAberta = (Prion.Generic.Models.Situacao)repSituacao.BuscarPelaSituacao("Mensalidade", "Aberta");
            


            String whereDefault = " WHERE Mensalidade.IdSituacao = " + situacaoAberta.Id.ToString() + " AND Mensalidade.Isento ='False'";            
            String orderBy = " Mensalidade.Id";
            String groupBy = "";

            return this.Select("SELECT " + campos.ToString(), join.ToString(), whereDefault, orderBy,groupBy, parametro,null,"AND");
        }

        /// <summary>
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros sem WHERE
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista DataTableMensalidadePaga(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder campos = new StringBuilder(), join = new StringBuilder();
        
            campos.Append("Aluno.Nome as Nome ");
            campos.Append(",Curso.Nome as Curso ");
            campos.Append(",Turma.Nome as Turma ");
            campos.Append(",Turno.Nome as Turno ");
            campos.Append(",Titulo.ValorPago as ValorPago ");
            campos.Append(",Titulo.DataVencimento as DataVencimento ");
            campos.Append(",Titulo.DataOperacao as DataOperacao ");
            campos.Append(",CASE  WHEN Titulo.DataOperacao <= Titulo.DataVencimento THEN (Titulo.Valor + Titulo.Acrescimo - Titulo.Desconto) "
                    +" ELSE (Titulo.Valor + Titulo.Acrescimo) "
                    +" END as ValorTotal ");

            join.Append(" FROM Mensalidade INNER JOIN MatriculaCurso ON Mensalidade.IdMatriculaCurso = MatriculaCurso.Id "
                + " INNER JOIN Turma ON Turma.Id = MatriculaCurso.IdTurma "
                + " INNER JOIN Turno ON Turno.Id = Turma.IdTurno "
                + " INNER JOIN CursoAnoLetivo ON Turma.IdCursoAnoLetivo=CursoAnoLetivo.Id "
                + " INNER JOIN Curso ON CursoAnoLetivo.IdCurso = Curso.Id "
                + " INNER JOIN Matricula ON MatriculaCurso.IdMatricula = Matricula.Id "
                + " INNER JOIN Aluno ON Aluno.Id = Matricula.IdAluno"
                + " INNER JOIN MensalidadeTitulo ON Mensalidade.Id = MensalidadeTitulo.IdMensalidade "
                + " INNER JOIN Titulo ON Titulo.Id = MensalidadeTitulo.IdTitulo "
                );
            Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this._conexao);
            

            String whereDefault = " ";
            String orderBy = " Mensalidade.Id";
            String groupBy = "";

            return this.Select("SELECT " + campos.ToString(), join.ToString(), whereDefault, orderBy, groupBy, parametro, null, "AND");
        } 


        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de MensalidadeTitulo de um Aluno
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.MensalidadeTitulo com os registros do DataTable</returns>
        public List<Models.MensalidadeTitulo> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }


            List<Models.MensalidadeTitulo> lista = new List<Models.MensalidadeTitulo>();           

            // varre o dataReader criando, a cada iteração, um objeto do tipo Prion.Generic.Models.Titulo
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Models.MensalidadeTitulo mensalidadeTitulo = new Models.MensalidadeTitulo();

                mensalidadeTitulo.IdTitulo = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                mensalidadeTitulo.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdMensalidade"].ToString());                
                mensalidadeTitulo.Valor = Conversor.ToDecimal(dataTable.Rows[i][nomeBase + "Valor"].ToString());               
                mensalidadeTitulo.Acrescimo = Conversor.ToDecimal(dataTable.Rows[i][nomeBase + "Acrescimo"].ToString());               
                mensalidadeTitulo.Desconto = Conversor.ToDecimal(dataTable.Rows[i][nomeBase + "Desconto"].ToString());               
                mensalidadeTitulo.DataVencimento = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataVencimento"].ToString());                
                mensalidadeTitulo.NomeCurso = dataTable.Rows[i][nomeBase + "NomeCurso"].ToString();
                mensalidadeTitulo.NomeAluno = dataTable.Rows[i][nomeBase + "NomeAluno"].ToString();
               
                lista.Add(mensalidadeTitulo);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }

        /// <summary>
        /// Carrega os ids de cartão de determinado Título
        /// </summary>       
        /// <param name="idMensalidade"></param>
        /// <param name="IdSituacao"></param>   
        /// <returns>ids dos cartões utilizados para efetuar o pagamento de um título</returns>
        public Int32[] CarregarRelacionamentoMensalidadeTitulo(params Int32[] idsMensalidade)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "MensalidadeTitulo.* ";
            String join = "FROM MensalidadeTitulo ";
            String whereDefault = "";
            String orderBy = "MensalidadeTitulo.IdTitulo";
            String groupBy = "";

            //Busca as movimentações de acordo com o relacionamento entre movimentação financeira e título
            Prion.Tools.Request.ParametrosRequest parametro = new Prion.Tools.Request.ParametrosRequest();
            Prion.Tools.Request.Filtro f =  new Request.Filtro("IdMensalidade", "IN", Conversor.ToString(",",idsMensalidade));
            parametro.Filtro.Add(f);

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy,groupBy, parametro);
            Int32 qntResultados = lista.DataTable.Rows.Count;
            Int32[] ids = new Int32[qntResultados];

            for (Int32 i = 0; i < qntResultados; i++)
            {
                ids[i] = Conversor.ToInt32(lista.DataTable.Rows[i]["IdTitulo"].ToString());
            }

            return ids;

        }
    }
}