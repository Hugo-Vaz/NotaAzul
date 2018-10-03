using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Prion.Data;
using Prion.Tools;
using System.Text;

namespace NotaAzul.Repository
{
    public class Relatorio : Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public Relatorio(ref DBFacade conexao)
            : base(ref conexao)
        { 
        }

        ~Relatorio()
        { 
        }

        /// <summary>
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros referentes ao relatório de contas pagas em determinado período de tempo
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista GerarRelatorioContasPagas(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder campos = new StringBuilder(), join = new StringBuilder();
            campos.Append("Titulo.* ");
            campos.Append(",TituloTipo.Nome as TituloTipo ");
            join.Append(" FROM Titulo INNER JOIN TituloTipo ON Titulo.IdTituloTipo = TituloTipo.Id ");

            Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this._conexao);
            Prion.Generic.Models.Situacao situacaoQuitada = (Prion.Generic.Models.Situacao)repSituacao.BuscarPelaSituacao("Título", "Quitado");


            String whereDefault = " WHERE Titulo.Tipo = 'D' AND Titulo.IdSituacao = " + situacaoQuitada.Id.ToString();            
            String orderBy = " Titulo.Id";
            String groupBy = "";
            return this.Select("SELECT " + campos.ToString(), join.ToString(), whereDefault, orderBy,groupBy, parametro,null,"AND");
        }

        /// <summary>
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros referentes ao relatório de contas a serem pagas em determinado período de tempo
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista GerarRelatorioContasAPagar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder campos = new StringBuilder(), join = new StringBuilder();
            campos.Append("Titulo.* ");
            campos.Append(",TituloTipo.Nome as TituloTipo ");
            join.Append(" FROM Titulo INNER JOIN TituloTipo ON Titulo.IdTituloTipo = TituloTipo.Id ");

            Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this._conexao);
            Prion.Generic.Models.Situacao situacaoAberta = (Prion.Generic.Models.Situacao)repSituacao.BuscarPelaSituacao("Título", "Aberto");


            String whereDefault = " WHERE Titulo.Tipo = 'D' AND Titulo.IdSituacao = " + situacaoAberta.Id.ToString();            
            String orderBy = " Titulo.Id";
            String groupBy = "";
            return this.Select("SELECT " + campos.ToString(), join.ToString(), whereDefault, orderBy,groupBy, parametro, null, "AND");
        }

        /// <summary>
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros referentes ao relatório de contas recebidas em determinado período de tempo
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista GerarRelatorioContasRecebidas(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder campos = new StringBuilder(), join = new StringBuilder();
            campos.Append("Titulo.* ");
            campos.Append(",TituloTipo.Nome as TituloTipo ");
            campos.Append(",Aluno.Nome as AlunoNome ");
            join.Append(" FROM Titulo INNER JOIN TituloTipo ON Titulo.IdTituloTipo = TituloTipo.Id ");
            join.Append(" INNER JOIN MensalidadeTitulo ON Titulo.Id = MensalidadeTitulo.IdTitulo ");
            join.Append(" INNER JOIN Mensalidade ON MensalidadeTitulo.IdMensalidade=Mensalidade.Id ");
            join.Append(" INNER JOIN MatriculaCurso ON Mensalidade.IdMatriculaCurso = MatriculaCurso.Id ");
            join.Append(" INNER JOIN Matricula ON MatriculaCurso.IdMatricula=Matricula.Id ");
            join.Append(" INNER JOIN Aluno ON Matricula.IdAluno = Aluno.Id "); 

            Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this._conexao);
            Prion.Generic.Models.Situacao situacaoQuitada = (Prion.Generic.Models.Situacao)repSituacao.BuscarPelaSituacao("Título", "Quitado");


            String whereDefault = " WHERE Titulo.Tipo = 'C' AND Titulo.IdSituacao = " + situacaoQuitada.Id.ToString();            
            String orderBy = " Titulo.Id";
            String groupBy = "";

            return this.Select("SELECT " + campos.ToString(), join.ToString(), whereDefault, orderBy,groupBy, parametro, null, "AND");
        }

        /// <summary>
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros referentes ao relatório de contas a serem recebidas em determinado período de tempo
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista GerarRelatorioContasAReceber(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder campos = new StringBuilder(), join = new StringBuilder();
            campos.Append("Titulo.* ");
            campos.Append(",TituloTipo.Nome as TituloTipo ");
            campos.Append(",Aluno.Nome as AlunoNome ");
            join.Append(" FROM Titulo INNER JOIN TituloTipo ON Titulo.IdTituloTipo = TituloTipo.Id ");
            join.Append(" INNER JOIN MensalidadeTitulo ON Titulo.Id = MensalidadeTitulo.IdTitulo ");
            join.Append(" INNER JOIN Mensalidade ON MensalidadeTitulo.IdMensalidade=Mensalidade.Id ");
            join.Append(" INNER JOIN MatriculaCurso ON Mensalidade.IdMatriculaCurso = MatriculaCurso.Id ");
            join.Append(" INNER JOIN Matricula ON MatriculaCurso.IdMatricula=Matricula.Id ");
            join.Append(" INNER JOIN Aluno ON Matricula.IdAluno = Aluno.Id "); 
           
            Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this._conexao);
            Prion.Generic.Models.Situacao situacaoAberta = (Prion.Generic.Models.Situacao)repSituacao.BuscarPelaSituacao("Título", "Aberto");


            String whereDefault = " WHERE Titulo.Tipo = 'C' AND Titulo.IdSituacao = " + situacaoAberta.Id.ToString();            
            String orderBy = " Titulo.Id";
            String groupBy = "";

            return this.Select("SELECT " + campos.ToString(), join.ToString(), whereDefault, orderBy,groupBy, parametro, null, "AND");
        }

        /// <summary>
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros referentes ao relatório de alunos inadimplentes em determinado período de tempo
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista GerarRelatorioAlunosBolsistas(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder campos = new StringBuilder(), join = new StringBuilder(), stuff = new StringBuilder();
            campos.Append("Matricula.NumeroMatricula as NumeroMatricula,");
            campos.Append("Aluno.Nome as NomeAluno,");
            campos.Append("AlunoFilantropia.ValorBolsa as ValorBolsa,");

            stuff.Append(" STUFF ( (SELECT  ', ' + Curso.Nome " +
                    "FROM Curso " +
                    "INNER JOIN CursoAnoLetivo ON CursoAnoLetivo.IdCurso=Curso.Id " +
                    "INNER JOIN Turma ON Turma.IdCursoAnoLetivo = CursoAnoLetivo.Id " +
                    "INNER JOIN MatriculaCurso ON MatriculaCurso.IdTurma = Turma.Id " +
                    "Where MatriculaCurso.IdMatricula=Matricula.Id and Matricula.IdAluno= Aluno.Id " +
                    "FOR XML PATH(''), TYPE).value('.', 'varchar(max)'), 1, 2, '') as Cursos,");
            stuff.Append(" STUFF ( (SELECT  ', '+ Turma.Nome FROM Turma " +
                    "INNER JOIN MatriculaCurso ON MatriculaCurso.IdTurma = Turma.Id " +
                    "INNER JOIN Matricula ON Matricula.Id = MatriculaCurso.IdMatricula " +
                    "Where Matricula.Id=MatriculaCurso.IdMatricula and Matricula.IdAluno= Aluno.Id " +
                    "FOR XML PATH(''), TYPE).value('.', 'varchar(max)'), 1, 2, '') as Turmas");
           

            join.Append(" FROM Aluno INNER JOIN Matricula on Matricula.IdAluno = Aluno.Id ");
            join.Append(" INNER JOIN AlunoFilantropia on AlunoFilantropia.IdAluno = Aluno.Id ");
            join.Append(" INNER JOIN MatriculaCurso ON MatriculaCurso.IdMatricula = Matricula.Id ");
            join.Append(" INNER JOIN Turma ON Turma.Id = MatriculaCurso.IdTurma ");            

            String whereDefault = "";
            String orderBy = " Aluno.Nome";
            String groupBy = " Matricula.NumeroMatricula,Aluno.Nome,Matricula.Id,Matricula.IdAluno,Aluno.Id,AlunoFilantropia.ValorBolsa ";
            
            return this.Select("SELECT " + campos.ToString() + stuff.ToString(), join.ToString(),whereDefault, orderBy, groupBy, parametro, null, "AND");
        }

        /// <summary>
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros referentes ao relatório de alunos inadimplentes em determinado período de tempo
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista GerarRelatorioAlunosInadimplentes(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder campos = new StringBuilder(), join = new StringBuilder(),stuff = new StringBuilder();
            campos.Append("Matricula.NumeroMatricula as NumeroMatricula,");
            campos.Append("Aluno.Nome as NomeAluno,");
            campos.Append("SUM(Titulo.Valor) as ValorTotal,");

            stuff.Append(" STUFF ( (SELECT DISTINCT ', ' + Curso.Nome " +
                    "FROM Curso " +
                    "INNER JOIN CursoAnoLetivo ON CursoAnoLetivo.IdCurso=Curso.Id " +
                    "INNER JOIN Turma ON Turma.IdCursoAnoLetivo = CursoAnoLetivo.Id " +
                    "INNER JOIN MatriculaCurso ON MatriculaCurso.IdTurma = Turma.Id " +
                    "Where MatriculaCurso.IdMatricula=Matricula.Id and Matricula.IdAluno= Aluno.Id " +
                    "FOR XML PATH(''), TYPE).value('.', 'varchar(max)'), 1, 2, '') as Cursos,");
            stuff.Append(" STUFF ( (SELECT DISTINCT ', '+ Turma.Nome FROM Turma " +  
                    "INNER JOIN MatriculaCurso ON MatriculaCurso.IdTurma = Turma.Id "+
                    "INNER JOIN Matricula ON Matricula.Id = MatriculaCurso.IdMatricula "+
                    "Where Matricula.Id=MatriculaCurso.IdMatricula and Matricula.IdAluno= Aluno.Id "+
                    "FOR XML PATH(''), TYPE).value('.', 'varchar(max)'), 1, 2, '') as Turmas,");

            stuff.Append("  STUFF ( (SELECT DISTINCT ', '+ CONVERT(varchar,Titulo.DataVencimento,103) FROM Titulo "+
                    "INNER JOIN MensalidadeTitulo ON MensalidadeTitulo.IdTitulo = Titulo.Id "+
                    "INNER JOIN Mensalidade ON Mensalidade.Id=MensalidadeTitulo.IdMensalidade "+  
                    "INNER JOIN MatriculaCurso ON MatriculaCurso.Id = Mensalidade.IdMatriculaCurso "+
                    "INNER JOIN Matricula ON Matricula.Id = MatriculaCurso.IdMatricula "+
                    "Where Matricula.Id=MatriculaCurso.IdMatricula and Matricula.IdAluno= Aluno.Id and ((Titulo.DataOperacao is null) or (Titulo.DataOperacao = '')) AND Titulo.DataVencimento < '" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                    "FOR XML PATH(''), TYPE).value('.', 'varchar(max)'), 1, 2, '') as Vencimentos ");

            join.Append(" FROM Aluno INNER JOIN Matricula on Matricula.IdAluno = Aluno.Id ");
            join.Append(" INNER JOIN MatriculaCurso ON MatriculaCurso.IdMatricula = Matricula.Id ");
            join.Append(" INNER JOIN Turma ON Turma.Id = MatriculaCurso.IdTurma ");
            join.Append(" INNER JOIN Mensalidade ON Mensalidade.IdMatriculaCurso = MatriculaCurso.Id ");
            join.Append(" INNER JOIN MensalidadeTitulo ON MensalidadeTitulo.IdMensalidade = Mensalidade.Id ");
            join.Append(" INNER JOIN Titulo ON Titulo.Id = MensalidadeTitulo.IdTitulo ");

            String whereDefault = "WHERE ((Titulo.IdSituacao = (SELECT Situacao.Id FROM Situacao INNER JOIN SituacaoTipo ON SituacaoTipo.Id = Situacao.IdSituacaoTipo WHERE SituacaoTipo.Nome='Título' and Situacao.Nome='Aberto')" +
                ")) AND (YEAR(Titulo.DataVencimento) < 2017)";            
            String orderBy = " Aluno.Nome";
            String groupBy = " Matricula.NumeroMatricula,Aluno.Nome,Matricula.Id,Matricula.IdAluno,Aluno.Id ";
            String where = this.MontarWhere(parametro, whereDefault, "AND");
            var list =this.Select("SELECT " + campos.ToString() + stuff.ToString(), join.ToString(), where, orderBy,groupBy, parametro, null, "AND");

            return list;
        }

        /// <summary>
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros referentes ao relatório de alunos isentos em determinado período de tempo
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista GerarRelatorioAlunosIsentos(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder campos = new StringBuilder(), join = new StringBuilder(), stuff = new StringBuilder();
            campos.Append("Matricula.NumeroMatricula as NumeroMatricula,");
            campos.Append("Aluno.Nome as NomeAluno,");          

            stuff.Append(" STUFF ( (SELECT  ', ' + Curso.Nome " +
                    "FROM Curso " +
                    "INNER JOIN CursoAnoLetivo ON CursoAnoLetivo.IdCurso=Curso.Id " +
                    "INNER JOIN Turma ON Turma.IdCursoAnoLetivo = CursoAnoLetivo.Id " +
                    "INNER JOIN MatriculaCurso ON MatriculaCurso.IdTurma = Turma.Id " +
                    "Where MatriculaCurso.IdMatricula=Matricula.Id and Matricula.IdAluno= Aluno.Id " +
                    "FOR XML PATH(''), TYPE).value('.', 'varchar(max)'), 1, 2, '') as Cursos,");
            stuff.Append(" STUFF ( (SELECT  ', '+ Turma.Nome FROM Turma " +
                    "INNER JOIN MatriculaCurso ON MatriculaCurso.IdTurma = Turma.Id " +
                    "INNER JOIN Matricula ON Matricula.Id = MatriculaCurso.IdMatricula " +
                    "Where Matricula.Id=MatriculaCurso.IdMatricula and Matricula.IdAluno= Aluno.Id " +
                    "FOR XML PATH(''), TYPE).value('.', 'varchar(max)'), 1, 2, '') as Turmas");
          

            join.Append(" FROM Aluno INNER JOIN Matricula on Matricula.IdAluno = Aluno.Id ");
            join.Append(" INNER JOIN MatriculaCurso ON MatriculaCurso.IdMatricula = Matricula.Id ");
            join.Append(" INNER JOIN Turma ON Turma.Id = MatriculaCurso.IdTurma ");
            join.Append(" INNER JOIN Mensalidade ON Mensalidade.IdMatriculaCurso = MatriculaCurso.Id ");
           

            String whereDefault = "WHERE (Mensalidade.Isento = 'true') ";
            String orderBy = " Aluno.Nome";
            String groupBy = " Matricula.NumeroMatricula,Aluno.Nome,Matricula.Id,Matricula.IdAluno,Aluno.Id ";
            String where = this.MontarWhere(parametro, whereDefault, "AND");
            return this.Select("SELECT " + campos.ToString() + stuff.ToString(), join.ToString(), where, orderBy, groupBy, parametro, null, "AND");
        }

        /// <summary>
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros referentes ao relatório de alunos matriculados em determinado período de tempo
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista GerarRelatorioAlunosMatriculados(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder campos = new StringBuilder(), join = new StringBuilder();
            campos.Append("Aluno.Nome as NomeAluno ");
            campos.Append(",Matricula.NumeroMatricula as NumeroMatricula ");
            campos.Append(",Turno.Nome as NomeTurno ");
            campos.Append(",Curso.Nome as NomeCurso ");
            campos.Append(",Turma.Nome as NomeTurma ");
            join.Append(" FROM Aluno INNER JOIN Matricula on Aluno.Id = Matricula.IdAluno ");
            join.Append(" INNER JOIN MatriculaCurso on Matricula.Id = MatriculaCurso.IdMatricula ");
            join.Append(" INNER JOIN Turma on MatriculaCurso.IdTurma = Turma.Id ");
            join.Append(" INNER JOIN Turno on Turma.IdTurno = Turno.Id ");
            join.Append(" INNER JOIN CursoAnoLetivo on Turma.IdCursoAnoLetivo = CursoAnoLetivo.Id ");
            join.Append(" INNER JOIN Curso ON CursoAnoLetivo.IdCurso = Curso.Id ");            

            String whereDefault = "";
            String orderBy = " Aluno.Nome";
            String groupBy = "";

            return this.Select("SELECT " + campos.ToString(), join.ToString(), whereDefault, orderBy,groupBy, parametro, null, "AND");
        }

        /// <summary>
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros referentes ao relatório de movimentações financeiras realizadas em determinado período de tempo
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista GerarRelatorioMovimentacaoFinanceira(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder campos = new StringBuilder(), join = new StringBuilder(), stuff = new StringBuilder();
            campos.Append("MovimentacaoFinanceira.Valor as Valor ");
            campos.Append(",Titulo.Tipo ");
            join.Append(" FROM MovimentacaoFinanceira INNER JOIN MovimentacaoFinanceiraTitulo ON MovimentacaoFinanceira.Id = MovimentacaoFinanceiraTitulo.IdMovimentacaoFinanceira ");
            join.Append(" INNER JOIN Titulo ON MovimentacaoFinanceiraTitulo.IdTitulo=Titulo.Id ");            
            stuff.Append(" ,STUFF ( (SELECT Distinct ', '+ Titulo.Descricao FROM Turma"
                  +" INNER JOIN MovimentacaoFinanceiraTitulo ON MovimentacaoFinanceira.Id = MovimentacaoFinanceiraTitulo.IdMovimentacaoFinanceira"
                  +" INNER JOIN Titulo ON MovimentacaoFinanceiraTitulo.IdTitulo=Titulo.Id  "
                  +" Where MovimentacaoFinanceiraTitulo.IdMovimentacaoFinanceira=MovimentacaoFinanceira.Id and MovimentacaoFinanceiraTitulo.IdTitulo = Titulo.Id"
                  +" FOR XML PATH(''), TYPE).value('.', 'varchar(max)'), 1, 2, '') as Titulos");
            String whereDefault = "";
            String orderBy = " MovimentacaoFinanceira.DataCadastro";
            String groupBy = "";
            return this.Select("SELECT " + campos.ToString() + stuff.ToString(), join.ToString(), whereDefault, orderBy,groupBy,parametro, null, "AND");
        }

        /// <summary>
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros referentes ao relatório de movimentações financeiras realizadas em determinado período de tempo,contendo informações do pagador
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista GerarRelatorioMovimentacaoFinanceiraResponsavel(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Prion.Tools.Request.Filtro f1 = (Prion.Tools.Request.Filtro)parametro.Filtro[0];
            Prion.Tools.Request.Filtro f2 = (Prion.Tools.Request.Filtro)parametro.Filtro[1];

            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder campos = new StringBuilder(), join = new StringBuilder();
            campos.Append(" Boleto.ValorPago as Valor,Boleto.DataVencimento as DataVencimento ");
            campos.Append(" ,Boleto.DataPagamento as DataPagamento ");
            campos.Append(",AlunoResponsavel.Nome as ResponsavelNome ");
            campos.Append(",AlunoResponsavel.CPF as Cpf,Aluno.Nome as AlunoNome");
            campos.Append(",(Endereco.Endereco + ',' + Endereco.Numero + ', '+Bairro.Nome+', '+Cidade.Nome+', ' + Estado.UF) as Endereco");
            join.Append(" FROM Boleto ");
            join.Append(" INNER JOIN BoletoTitulo ON BoletoTitulo.IdBoleto = Boleto.Id ");            
            join.Append(" INNER JOIN MensalidadeTitulo ON MensalidadeTitulo.IdTitulo = BoletoTitulo.IdTitulo ");
            join.Append(" INNER JOIN Mensalidade ON Mensalidade.Id = MensalidadeTitulo.IdMensalidade ");
            join.Append(" INNER JOIN MatriculaCurso ON MatriculaCurso.Id = Mensalidade.IdMatriculaCurso ");
            join.Append(" INNER JOIN Matricula ON MatriculaCurso.IdMatricula = Matricula.Id ");
            join.Append(" INNER JOIN Aluno ON Aluno.Id = Matricula.IdAluno ");
            join.Append(" INNER JOIN AlunoResponsavel ON AlunoResponsavel.IdAluno = Matricula.IdAluno ");
            join.Append(" INNER JOIN AlunoResponsavelEndereco ON AlunoResponsavelEndereco.IdAlunoResponsavel = AlunoResponsavel.Id ");
            join.Append(" INNER JOIN Endereco ON AlunoResponsavelEndereco.IdEndereco = Endereco.Id ");
            join.Append(" INNER JOIN Bairro ON Endereco.IdBairro = Bairro.Id ");
            join.Append(" INNER JOIN Cidade ON Endereco.IdCidade = Cidade.Id ");
            join.Append(" INNER JOIN Estado ON Endereco.IdEstado = Estado.Id ");
            StringBuilder whereDefault = new StringBuilder();
            whereDefault.Append("WHERE (Boleto.StatusBoleto = 'Quitado' OR Boleto.StatusBoleto = 'Quitado na escola') AND ((");
            whereDefault.Append(f1.Nome);
            whereDefault.Append(" BETWEEN ");
            whereDefault.Append(f1.Valor1);
            whereDefault.Append(" AND ");
            whereDefault.Append(f1.Valor2);
            whereDefault.Append(") OR (");
            whereDefault.Append(f2.Nome);
            whereDefault.Append(" BETWEEN ");
            whereDefault.Append(f2.Valor1);
            whereDefault.Append(" AND ");
            whereDefault.Append(f2.Valor2);
            whereDefault.Append(")) ");


            String orderBy = " ORDER BY Boleto.DataPagamento";
           // String groupBy = "Boleto.DataPagamento , Boleto.ValorPago,AlunoResponsavel.Nome,AlunoResponsavel.CPF";            

            return this.Select("SELECT DISTINCT " + campos.ToString() + join.ToString() + whereDefault.ToString() + orderBy, null);            
        }

        /// <summary>
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros referentes ao relatório de mensalidades pagas com atraso
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista GerarRelatorioMensalidadesAtrasadas(Prion.Tools.Request.ParametrosRequest parametro = null)
        {

            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder campos = new StringBuilder(), join = new StringBuilder();
            campos.Append("Matricula.NumeroMatricula as NumeroMatricula");
            campos.Append(",Aluno.Nome as NomeAluno ");
            campos.Append(",Curso.Nome as NomeCurso ");
            campos.Append(",Turma.Nome as NomeTurma ");
            campos.Append(",Mensalidade.DataVencimento as DataVencimento ");
            campos.Append(",Mensalidade.Valor as Valor ");

            join.Append(" FROM Matricula INNER JOIN Aluno ON Matricula.IdAluno=Aluno.Id ");
            join.Append(" INNER JOIN MatriculaCurso ON Matricula.Id = MatriculaCurso.IdMatricula");
            join.Append(" INNER JOIN Turma ON MatriculaCurso.IdTurma = Turma.Id ");
            join.Append(" INNER JOIN CursoAnoLetivo ON Turma.IdCursoAnoLetivo = CursoAnoLetivo.Id ");
            join.Append(" INNER JOIN Curso ON CursoAnoLetivo.IdCurso = Curso.Id ");
            join.Append(" INNER JOIN Mensalidade ON MatriculaCurso.Id = Mensalidade.IdMatriculaCurso ");
            join.Append(" INNER JOIN MensalidadeTitulo ON Mensalidade.Id = MensalidadeTitulo.IdMensalidade ");
            join.Append(" INNER JOIN Titulo ON MensalidadeTitulo.IdTitulo = Titulo.Id ");

            String whereDefault = "";
            String orderBy = " Aluno.Nome,Curso.Nome,Turma.Nome,Mensalidade.DataVencimento ";
            String groupBy = "";

            Prion.Tools.Request.Filtro f = new Prion.Tools.Request.Filtro("Titulo.DataOperacao ", ">", "Titulo.DataVencimento");
            parametro.Filtro.Add(f);

            return this.Select("SELECT " + campos.ToString(), join.ToString(), whereDefault, orderBy, groupBy, parametro, null, "AND");
        }

        /// <summary>
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros referentes ao relatório de mensalidades pagas adiantadas
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista GerarRelatorioMensalidadesAdiantadas(Prion.Tools.Request.ParametrosRequest parametro = null)
        {

            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder campos = new StringBuilder(), join = new StringBuilder();
            campos.Append("Matricula.NumeroMatricula as NumeroMatricula");
            campos.Append(",Aluno.Nome as NomeAluno ");
            campos.Append(",Curso.Nome as NomeCurso ");
            campos.Append(",Turma.Nome as NomeTurma ");
            campos.Append(",Mensalidade.DataVencimento as DataVencimento ");
            campos.Append(",Mensalidade.Valor as Valor ");

            join.Append(" FROM Matricula INNER JOIN Aluno ON Matricula.IdAluno=Aluno.Id ");
            join.Append(" INNER JOIN MatriculaCurso ON Matricula.Id = MatriculaCurso.IdMatricula");
            join.Append(" INNER JOIN Turma ON MatriculaCurso.IdTurma = Turma.Id ");
            join.Append(" INNER JOIN CursoAnoLetivo ON Turma.IdCursoAnoLetivo = CursoAnoLetivo.Id ");
            join.Append(" INNER JOIN Curso ON CursoAnoLetivo.IdCurso = Curso.Id ");
            join.Append(" INNER JOIN Mensalidade ON MatriculaCurso.Id = Mensalidade.IdMatriculaCurso ");
            join.Append(" INNER JOIN MensalidadeTitulo ON Mensalidade.Id = MensalidadeTitulo.IdMensalidade ");
            join.Append(" INNER JOIN Titulo ON MensalidadeTitulo.IdTitulo = Titulo.Id ");

            String whereDefault = "";
            String orderBy = " Curso.Nome,Turma.Nome,Mensalidade.DataVencimento ";
            String groupBy = "";

            Prion.Tools.Request.Filtro f = new Prion.Tools.Request.Filtro("Titulo.DataOperacao ", "<", "Titulo.DataVencimento");
            parametro.Filtro.Add(f);

            return this.Select("SELECT " + campos.ToString(), join.ToString(), whereDefault, orderBy, groupBy, parametro, null, "AND");
        }

        // <summary>
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros referentes ao relatório de alunos matriculados em determinado período de tempo
        /// </summary>
        /// <returns>DataTable</returns>
        public Models.DadosCobranca GerarRelatorioParaCobranca()
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            Models.DadosCobranca resultado = new Models.DadosCobranca();
            StringBuilder sql = new StringBuilder();

            sql.Append("SLECT Count(Distinct Aluno.Id) as QuantidadeMatricula ");

            sql.Append(" FROM Aluno INNER JOIN Matricula ON Aluno.Id = Matricula.IdAluno ");
            sql.Append(" INNER JOIN MatriculaCurso ON Matricula.Id = MatriculaCurso.IdMatricula ");
            sql.Append(" INNER JOIN Turma ON MatriculaCurso.IdTurma = Turma.Id");
            sql.Append(" INNER JOIN CursoAnoLetivo ON CursoAnoLetivo.Id = Turma.IdCursoAnoLetivo ");

            sql.Append(" WHERE CursoAnoLetivo.AnoLetivo = '");
            sql.Append(DateTime.Now.Year.ToString());
            sql.Append("' AND Matricula.IdSituacao = (SELECT Situacao.Id FROM Situacao WHERE Situacao.Nome = 'Ativo' ");
            sql.Append(" AND Situacao.IdSituacaoTipo = (SELECT SituacaoTipo.Id FROM SituacaoTipo WHERE SituacaoTipo.Nome = 'Matrícula'))");


            Prion.Generic.Models.Lista lista = this.Select(sql.ToString(), null);
            resultado.AlunosMatriculados = Convert.ToInt32(lista.DataTable.Rows[0]["QuantidadeMatricula"].ToString());

            if (DateTime.Now.Month > 1)
            {
                sql.Append("AND MONTH(Matricula.DataCadastro) = ");
                sql.Append(DateTime.Now.Month);

                lista = this.Select(sql.ToString(), null);
                resultado.MatriculadosNoUltimoMes = Convert.ToInt32(lista.DataTable.Rows[0]["QuantidadeMatricula"].ToString());
            }


            return resultado;
        }

        /// <summary>
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros referentes ao relatório de alunos inadimplentes em determinado período de tempo
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista GerarRelatorioAlunosInadimplentesBoleto(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT DISTINCT Matricula.NumeroMatricula as NumeroMatricula,Aluno.Nome as NomeAluno, ");
            sql.Append("Boleto.NossoNumero as NossoNumero,Boleto.Valor as Valor,Boleto.DataVencimento as Vencimento, ");
            sql.Append("(SELECT TOP 1 AlunoResponsavelTelefone.Numero FROM AlunoResponsavelTelefone WHERE AlunoResponsavelTelefone.IdAlunoResponsavel = AlunoResponsavel.Id) as Telefone, AlunoResponsavel.Nome as Responsavel,  AlunoResponsavel.CPF as ResponsavelCPF ");
            sql.Append("FROM Aluno INNER JOIN Matricula on Matricula.IdAluno = Aluno.Id ");
            sql.Append("INNER JOIN MatriculaCurso ON MatriculaCurso.IdMatricula = Matricula.Id ");
            sql.Append("INNER JOIN Turma ON Turma.Id = MatriculaCurso.IdTurma ");
            sql.Append("INNER JOIN Mensalidade ON Mensalidade.IdMatriculaCurso = MatriculaCurso.Id ");
            sql.Append("INNER JOIN MensalidadeTitulo ON MensalidadeTitulo.IdMensalidade = Mensalidade.Id ");
            sql.Append("INNER JOIN BoletoTitulo ON BoletoTitulo.IdTitulo = MensalidadeTitulo.IdTitulo ");
            sql.Append("INNER JOIN Boleto ON Boleto.Id = BoletoTitulo.IdBoleto ");
            sql.Append("INNER JOIN AlunoResponsavel ON AlunoResponsavel.Id = ");
            sql.Append("(SELECT TOP 1 AlunoResponsavel.Id FROM AlunoResponsavel WHERE AlunoResponsavel.IdAluno = Aluno.Id) ");          
          

            sql.Append("WHERE Boleto.DataVencimento < GETDATE() AND Boleto.StatusBoleto = 'Aberto' ");

            return this.Select(sql.ToString(),parametro);
        }


        /// <summary>
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros referentes ao relatório de alunos inadimplentes em determinado período de tempo
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista GerarRelatorioBoletoSQuitados(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT DISTINCT Matricula.NumeroMatricula as NumeroMatricula,Aluno.Nome as NomeAluno, ");
            sql.Append("Boleto.NossoNumero as NossoNumero,Boleto.Valor as Valor,Boleto.DataVencimento as Vencimento, Boleto.DataPagamento as Pagamento ");
            sql.Append("FROM Aluno INNER JOIN Matricula on Matricula.IdAluno = Aluno.Id ");
            sql.Append("INNER JOIN MatriculaCurso ON MatriculaCurso.IdMatricula = Matricula.Id ");
            sql.Append("INNER JOIN Turma ON Turma.Id = MatriculaCurso.IdTurma ");
            sql.Append("INNER JOIN Mensalidade ON Mensalidade.IdMatriculaCurso = MatriculaCurso.Id ");
            sql.Append("INNER JOIN MensalidadeTitulo ON MensalidadeTitulo.IdMensalidade = Mensalidade.Id ");
            sql.Append("INNER JOIN BoletoTitulo ON BoletoTitulo.IdTitulo = MensalidadeTitulo.IdTitulo ");
            sql.Append("INNER JOIN Boleto ON Boleto.Id = BoletoTitulo.IdBoleto ");
            sql.Append("WHERE  Boleto.StatusBoleto = 'Quitado' ");

            return this.Select(sql.ToString(), parametro);
        }

        /// <summary>
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros referentes ao relatório de alunos inadimplentes em determinado período de tempo
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista GerarRelatorioArquivoRetorno(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT BoletoRetorno.NomeArquivo as NomeArquivo, BoletoRetorno.NumeroDocumento as NumeroDocumento, ");
            sql.Append(" BoletoRetorno.Operacao as Operacao,BoletoRetorno.DataOperacao as DataOperacao, Boleto.Valor as Valor, Boleto.ValorPago as ValorPago ");
            sql.Append(" FROM BoletoRetorno ");
            sql.Append(" INNER JOIN Boleto ON Boleto.NossoNumero = '0' + BoletoRetorno.NumeroDocumento ");         

            return this.Select(sql.ToString(), parametro);
        }

    }
}