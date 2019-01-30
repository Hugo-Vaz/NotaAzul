using Prion.Data;
using Prion.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Web;

namespace NotaAzul.Repository
{
    public class Boleto : Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public Boleto(ref DBFacade conexao)
            : base(ref conexao)
        {
        }

        ~Boleto()
        {
        }

        /// <summary>
        /// Busca todas as Boleto da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "Boleto.*,Aluno.Nome as NomeAluno";
            StringBuilder join = new StringBuilder();
            join.Append("FROM Boleto INNER JOIN BoletoTitulo ON BoletoTitulo.IdBoleto = Boleto.Id ");
            join.Append(" INNER JOIN MensalidadeTitulo ON BoletoTitulo.IdTitulo = MensalidadeTitulo.IdTitulo ");
            join.Append(" INNER JOIN Mensalidade ON Mensalidade.Id = MensalidadeTitulo.IdMensalidade ");
            join.Append(" INNER JOIN MatriculaCurso ON Mensalidade.IdMatriculaCurso = MatriculaCurso.Id ");
            join.Append(" INNER JOIN Matricula ON Matricula.Id = MatriculaCurso.IdMatricula ");
            join.Append(" INNER JOIN Aluno ON Aluno.Id = Matricula.IdAluno ");

            String whereDefault = "";
            String orderBy = "Boleto.DataVencimento";
            String groupBy = "";
            parametro.Paginar = false;
            Prion.Generic.Models.Lista lista = this.Select("SELECT DISTINCT " + campos, join.ToString(), whereDefault, orderBy, groupBy, parametro);
            List<Models.Boleto> listaBoleto = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaBoleto = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaBoleto != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaBoleto);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }

        /// <summary>
        /// Busca todas as Boleto da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Models.Boleto BuscarPorMensalidade(Int32 idMensalidade)
        {
            Prion.Tools.Request.ParametrosRequest parametro = new Request.ParametrosRequest();
            Prion.Tools.Request.Filtro f = new Request.Filtro("MensalidadeTitulo.IdMensalidade", "=", idMensalidade.ToString());
            parametro.Filtro.Add(f);

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM Boleto INNER JOIN MensalidadeTitulo ON MensalidadeTitulo.IdTitulo = Boleto.IdTitulo");

            Prion.Generic.Models.Lista lista = this.Select(sql.ToString(), parametro);           

            Models.Boleto boleto = this.DataTableToObject(lista.DataTable)[0];
           
            return boleto;
        }

        /// <summary>
        /// Retorna, através de um (ou vários) ID, uma lista de objetos do tipo Models.Boleto
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Prion.Generic.Models.Lista Buscar(bool arquivoRem = false, params Int32[] ids)
        {
            Prion.Tools.Request.ParametrosRequest parametro = new Prion.Tools.Request.ParametrosRequest();
            Prion.Tools.Request.Filtro f = new Request.Filtro("Boleto.Id", "IN", Conversor.ToString(",", ids));
            
            parametro.Filtro.Add(f);

            if (arquivoRem)
            {
                Prion.Tools.Request.Filtro f2 = new Request.Filtro("Boleto.RemessaGerado", "=", 0);
                parametro.Filtro.Add(f2);
            }

            return Buscar(parametro);
        }

        /// <summary>
        /// Retorna, através de um (ou vários) ID, uma lista de objetos do tipo Models.Boleto
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Prion.Generic.Models.Lista Buscar(Int32 idAluno)
        {
            Prion.Tools.Request.ParametrosRequest parametro = new Prion.Tools.Request.ParametrosRequest();
            Prion.Tools.Request.Filtro f = new Request.Filtro("Aluno.Id", "=", idAluno);

            parametro.Filtro.Add(f);

          
            Prion.Tools.Request.Filtro f2 = new Request.Filtro("Boleto.RemessaGerado", "=", 0);
            parametro.Filtro.Add(f2);
           

            return Buscar(parametro);
        }

        /// <summary>
        /// Retorna, através de um (ou vários) ID, uma lista de objetos do tipo Models.Boleto
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Prion.Generic.Models.Lista Buscar(int mes,bool arquivoRem = false)
        {
            Prion.Tools.Request.ParametrosRequest parametro = new Prion.Tools.Request.ParametrosRequest();
            Prion.Tools.Request.Filtro f = new Request.Filtro("MONTH(Boleto.DataVencimento)", "=", mes);
            //Prion.Tools.Request.Filtro f2 = new Request.Filtro("YEAR(Boleto.DataVencimento)", "=", 2017);
            //Prion.Tools.Request.Filtro f3 = new Request.Filtro(" Boleto.DataEmissao", ">", "'2016-12-20'");

            if (arquivoRem)
            {
                Prion.Tools.Request.Filtro f3 = new Request.Filtro("Boleto.RemessaGerado", "=", 0);
                parametro.Filtro.Add(f3);
            }

            parametro.Filtro.Add(f);
            //parametro.Filtro.Add(f2);
            //parametro.Filtro.Add(f3);


            return Buscar(parametro);
        }

        /// <summary>
        /// Retorna, através de um ID, a quantidade de contratos efetuados
        /// </summary>
        /// <param name="idCorporacao"></param>
        /// <returns></returns>
        public Int32 BuscarQuantidadeBoleto()
        {
            String sql = "SELECT COUNT(DISTINCT Boleto.Id) as Quantidade " +
                "FROM Boleto";

            Prion.Generic.Models.Lista lista = this.Select(sql, null);
            Int32 quantidade = Convert.ToInt32(lista.DataTable.Rows[0]["Quantidade"].ToString());
            return quantidade;
        }

        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de Boletos
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.Boleto com os registros do DataTable</returns>
        public List<Models.Boleto> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }


            List<Models.Boleto> lista = new List<Models.Boleto>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.Boleto
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Models.Boleto boleto = new Models.Boleto();

                boleto.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                boleto.IdCorporacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdCorporacao"].ToString());
                boleto.StatusBoleto = dataTable.Rows[i][nomeBase + "StatusBoleto"].ToString();
                boleto.NomeAluno = dataTable.Rows[i]["NomeAluno"].ToString();
                boleto.Valor = Conversor.ToDecimal(dataTable.Rows[i][nomeBase + "Valor"].ToString());
                boleto.Juros = Conversor.ToDecimal(dataTable.Rows[i][nomeBase + "Juros"].ToString());
                boleto.Desconto = Conversor.ToDecimal(dataTable.Rows[i][nomeBase + "Desconto"].ToString());
                boleto.NumeroDocumento = dataTable.Rows[i][nomeBase + "NumeroDocumento"].ToString();
                boleto.Convenio = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Convenio"].ToString());
                boleto.Carteira = dataTable.Rows[i][nomeBase + "Carteira"].ToString();
                boleto.CodigoBanco = dataTable.Rows[i][nomeBase + "CodigoBanco"].ToString();
                boleto.NumeroAgencia = dataTable.Rows[i][nomeBase + "NumeroAgencia"].ToString();
                boleto.NumeroConta = dataTable.Rows[i][nomeBase + "NumeroConta"].ToString();
                boleto.NossoNumero = dataTable.Rows[i][nomeBase + "NossoNumero"].ToString();
                boleto.DataEmissao = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataEmissao"].ToString());
                boleto.DataVencimento = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataVencimento"].ToString());
                boleto.ValorPago = Conversor.ToDecimal(dataTable.Rows[i][nomeBase + "ValorPago"].ToString());
                boleto.DataPagamento = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataPagamento"].ToString());
                boleto.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                boleto.RemessaGerado = Conversor.ToBoolean(dataTable.Rows[i][nomeBase + "RemessaGerado"].ToString());
                boleto.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(boleto);
                    continue;
                }
                lista.Add(boleto);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }

        /// <summary>
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros sem WHERE
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista DataTable(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            if (parametro.Paginar == false)
            {
                StringBuilder campos = new StringBuilder(), join = new StringBuilder();
                campos.Append("Boleto.*,Aluno.Nome as NomeAluno ");
                campos.Append(" ,CAST(CASE WHEN Boleto.RemessaGerado = 1 THEN 'Sim' ELSE 'Não' END AS varchar(3)) as RemessaGeradoStr ");
                join.Append("FROM Boleto INNER JOIN BoletoTitulo ON BoletoTitulo.IdBoleto = Boleto.Id ");
                join.Append(" INNER JOIN MensalidadeTitulo ON BoletoTitulo.IdTitulo = MensalidadeTitulo.IdTitulo ");
                join.Append(" INNER JOIN Mensalidade ON Mensalidade.Id = MensalidadeTitulo.IdMensalidade ");
                join.Append(" INNER JOIN MatriculaCurso ON Mensalidade.IdMatriculaCurso = MatriculaCurso.Id ");
                join.Append(" INNER JOIN Matricula ON Matricula.Id = MatriculaCurso.IdMatricula ");
                join.Append(" INNER JOIN Aluno ON Aluno.Id = Matricula.IdAluno ");

                String whereDefault = "";
                String orderBy = "Boleto.DataVencimento,Aluno.Nome";
                String groupBy = "";
                String distinct = (parametro.Paginar) ? " " : " DISTINCT ";
                Prion.Generic.Models.Lista lista = this.Select("SELECT " + distinct + campos.ToString(), join.ToString(), whereDefault, orderBy, groupBy, parametro);
                return lista;
            }
            else
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" WITH tabela1 AS ( ");
                sql.Append(" SELECT DISTINCT  Boleto.*, Aluno.Nome as NomeAluno, ");
                sql.Append(" CAST(CASE WHEN Boleto.RemessaGerado = 1 THEN 'Sim' ELSE 'Não' END AS varchar(3)) as RemessaGeradoStr, ");
                sql.Append("  DENSE_RANK() OVER(ORDER BY ");
                if (string.IsNullOrEmpty(parametro.OrderBy))
                {
                    sql.Append(" Aluno.Nome ");
                }
                else
                {
                    sql.Append(parametro.OrderBy);
                    sql.Append(parametro.Ordem);
                }

                sql.Append(" ) AS IdRegistro ");
                sql.Append(" FROM Boleto  INNER JOIN BoletoTitulo ON BoletoTitulo.IdBoleto = Boleto.Id ");
                sql.Append(" INNER JOIN MensalidadeTitulo ON BoletoTitulo.IdTitulo = MensalidadeTitulo.IdTitulo ");
                sql.Append(" INNER JOIN Mensalidade ON Mensalidade.Id = MensalidadeTitulo.IdMensalidade ");
                sql.Append(" INNER JOIN MatriculaCurso ON Mensalidade.IdMatriculaCurso = MatriculaCurso.Id ");
                sql.Append(" INNER JOIN Matricula ON Matricula.Id = MatriculaCurso.IdMatricula ");
                sql.Append(" INNER JOIN Aluno ON Aluno.Id = Matricula.IdAluno Where Boleto.StatusBoleto <> 'Cancelado' ");
                sql.Append(" ) SELECT* FROM tabela1 WHERE IdRegistro BETWEEN  ");
                sql.Append(parametro.Inicio);
                sql.Append("   AND  ");
                sql.Append(parametro.Pagina * parametro.Quantidade);

                Prion.Generic.Models.Lista lista = this.Select(sql.ToString(),null);
                return lista;
            }
        }

        /// <summary>
        /// Insere um registro em Boleto através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Boleto</param>
        /// <returns></returns>
        protected override int Inserir(Prion.Generic.Models.Base objeto)
        {
            Models.Boleto oBoleto = (Models.Boleto)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO Boleto(StatusBoleto, IdCorporacao, Valor, Desconto, Juros, Convenio, Carteira, NossoNumero, NumeroDocumento, " +
                        " CodigoBanco, NumeroAgencia, NumeroConta, DataEmissao, DataVencimento, ValorPago, DataPagamento, DataCadastro)" +
                        " VALUES (@Status, @IdCorporacao, @Valor, @Desconto, @Juros, @Convenio, @Carteira, @NossoNumero, @NumeroDocumento," +
                        " @CodigoBanco, @NumeroAgencia, @NumeroConta, @DataEmissao, @DataVencimento, @ValorPago, @DataPagamento, @DataCadastro)";

            parametros.Add(this.Conexao.CriarParametro("@IdCorporacao", DbType.Int32, oBoleto.IdCorporacao));
            parametros.Add(this.Conexao.CriarParametro("@Status", DbType.String, oBoleto.StatusBoleto));
            parametros.Add(this.Conexao.CriarParametro("@Valor", DbType.Decimal, oBoleto.Valor));
            parametros.Add(this.Conexao.CriarParametro("@Desconto", DbType.Decimal, oBoleto.Desconto));
            parametros.Add(this.Conexao.CriarParametro("@Juros", DbType.Decimal, oBoleto.Juros));
            parametros.Add(this.Conexao.CriarParametro("@Convenio", DbType.Int32, oBoleto.Convenio));
            parametros.Add(this.Conexao.CriarParametro("@Carteira", DbType.String, oBoleto.Carteira));
            parametros.Add(this.Conexao.CriarParametro("@CodigoBanco", DbType.String, oBoleto.CodigoBanco));
            parametros.Add(this.Conexao.CriarParametro("@NumeroDocumento", DbType.String, oBoleto.NumeroDocumento));
            parametros.Add(this.Conexao.CriarParametro("@NumeroAgencia", DbType.String, oBoleto.NumeroAgencia));
            parametros.Add(this.Conexao.CriarParametro("@NumeroConta", DbType.String, oBoleto.NumeroConta));
            parametros.Add(this.Conexao.CriarParametro("@NossoNumero", DbType.String, oBoleto.NossoNumero));
            parametros.Add(this.Conexao.CriarParametro("@DataEmissao", DbType.DateTime, oBoleto.DataEmissao));
            parametros.Add(this.Conexao.CriarParametro("@DataVencimento", DbType.DateTime, oBoleto.DataVencimento));
            parametros.Add(this.Conexao.CriarParametro("@ValorPago", DbType.Decimal, 0));
            parametros.Add(this.Conexao.CriarParametro("@DataPagamento", DbType.DateTime, null));
            parametros.Add(this.Conexao.CriarParametro("@DataCadastro", DbType.DateTime, Helpers.DataHora.GetCurrentNow()));

            Int32 id = this.Conexao.Insert(sql, parametros);
           // GravarLog(id, TipoOperacaoLog.Inserir);

            return id;
        }


        /// <summary>
        /// Cria os relacionamentos entre Titulo e Boleto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Boleto</param>
        /// <returns></returns>
        public void CriarRelacionamentoBoletoTitulo(Int32 idBoleto,List<Int32>idsMensalidade)
        {  

            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO BoletoTitulo " +
                "SELECT @IdBoleto,MensalidadeTitulo.IdTitulo FROM MensalidadeTitulo WHERE MensalidadeTitulo.IdMensalidade IN ("+ Conversor.ToString(",",idsMensalidade.ToArray()) +")";

            parametros.Add(this.Conexao.CriarParametro("@IdBoleto", DbType.Int32, idBoleto));
            

            this.Conexao.Insert(sql, parametros);           
        }

        /// <summary>
        /// Altera um registro de Boleto através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Boleto</param>
        /// <returns></returns>
        protected override Int32 Alterar(Prion.Generic.Models.Base objeto)
        {
            Models.Boleto oBoleto = (Models.Boleto)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE Boleto SET StatusBoleto=@Status, IdCorporacao=@IdCorporacao, Valor=@Valor, Desconto=@Desconto, Juros=@Juros,Convenio=@Convenio, Carteira=@Carteira, " +
                         " NumeroDocumento=@NumeroDocumento, CodigoBanco=@CodigoBanco, NumeroAgencia=@NumeroAgencia, NumeroConta=@NumeroConta, NossoNumero=@NossoNumero, " +
                         " DataEmissao=@DataEmissao, DataVencimento=@DataVencimento " +
                         " WHERE Id=@Id";

            parametros.Add(this.Conexao.CriarParametro("@Status", DbType.String, oBoleto.StatusBoleto));
            parametros.Add(this.Conexao.CriarParametro("@IdCorporacao", DbType.Int32, oBoleto.IdCorporacao));
            parametros.Add(this.Conexao.CriarParametro("@Valor", DbType.Decimal, oBoleto.Valor));
            parametros.Add(this.Conexao.CriarParametro("@Desconto", DbType.Decimal, oBoleto.Desconto));
            parametros.Add(this.Conexao.CriarParametro("@Juros", DbType.Decimal, oBoleto.Juros));
            parametros.Add(this.Conexao.CriarParametro("@Convenio", DbType.Int32, oBoleto.Convenio));
            parametros.Add(this.Conexao.CriarParametro("@Carteira", DbType.String, oBoleto.Carteira));
            parametros.Add(this.Conexao.CriarParametro("@NumeroDocumento", DbType.String, oBoleto.NumeroDocumento));
            parametros.Add(this.Conexao.CriarParametro("@CodigoBanco", DbType.String, oBoleto.CodigoBanco));
            parametros.Add(this.Conexao.CriarParametro("@NumeroAgencia", DbType.String, oBoleto.NumeroAgencia));
            parametros.Add(this.Conexao.CriarParametro("@NumeroConta", DbType.String, oBoleto.NumeroConta));
            parametros.Add(this.Conexao.CriarParametro("@NossoNumero", DbType.String, oBoleto.NossoNumero));
            parametros.Add(this.Conexao.CriarParametro("@DataEmissao", DbType.DateTime, oBoleto.DataEmissao));
            parametros.Add(this.Conexao.CriarParametro("@DataVencimento", DbType.DateTime, oBoleto.DataVencimento));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, oBoleto.Id));


            Int32 linhasAfetadas = this.Conexao.Update(sql, parametros);

           // GravarLog(oBoleto.Id, TipoOperacaoLog.Alterar);

            return linhasAfetadas;
        }


        /// <summary>
        /// Exclui um registro de Boleto através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Boleto</param>
        /// <returns>Quantidade de registros excluídos</returns>
        protected override Int32 Excluir(Prion.Generic.Models.Base objeto)
        {
            Models.Boleto oBoleto = (Models.Boleto)objeto;
            return Excluir(oBoleto.Id);
        }


        /// <summary>
        /// Exclui um registro de Boleto através do Id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>Quantidade de registros excluídos</returns>
        public Int32 Excluir(params Int32[] ids)
        {
            // se o parâmetro for null, retorna 0, informando que não excluiu nenhum registro
            if (ids == null) { return 0; }

            for (Int32 indice = 0; indice < ids.Length; indice++)
            {
               // GravarLog(ids[indice], TipoOperacaoLog.Excluir);
            }

            Int32 retorno = -1;
            String strId = Conversor.ToString(",", ids);
            String sql = "DELETE FROM Boleto WHERE Id IN (" + strId + ")";

            retorno = this.Conexao.Delete(sql);

            return retorno;
        }
              
        /// <summary>
        /// Altera a situação de Boleto 
        /// </summary>        
        /// <returns></returns>
        public void QuitarBoleto(string nossoNumero, Decimal valorPago, DateTime dataPagamento,Int32 situacaoTitulo,Int32 situacaoMensalidade,string statusBoleto)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = " UPDATE Boleto SET  StatusBoleto=@statusBoleto, ValorPago=@ValorPago, DataPagamento=@DataPagamento " +
                         " WHERE NossoNumero Like @NossoNumero";

            parametros.Add(this.Conexao.CriarParametro("@ValorPago", DbType.Decimal, valorPago));
            parametros.Add(this.Conexao.CriarParametro("@DataPagamento", DbType.DateTime, dataPagamento));
            parametros.Add(this.Conexao.CriarParametro("@NossoNumero", DbType.String, nossoNumero));
            parametros.Add(this.Conexao.CriarParametro("@statusBoleto", DbType.String, statusBoleto));


            this.Conexao.Update(sql, parametros);

            parametros.Add(this.Conexao.CriarParametro("@Situacao", DbType.Int32, situacaoTitulo));
            sql = " UPDATE Titulo SET IdSituacao=@Situacao,ValorPago=@ValorPago,DataOperacao=@DataPagamento WHERE Id IN " +
                " (SELECT Titulo.Id FROM Titulo INNER JOIN BoletoTitulo ON BoletoTitulo.IdTitulo = Titulo.Id " +
                " INNER JOIN Boleto ON Boleto.Id = BoletoTitulo.IdBoleto " +
                " WHERE Boleto.NossoNumero = @NossoNumero)";

            this.Conexao.Update(sql, parametros);

            parametros = new List<DbParameter>();            
            parametros.Add(this.Conexao.CriarParametro("@NossoNumero", DbType.String, nossoNumero));
            parametros.Add(this.Conexao.CriarParametro("@Situacao", DbType.Int32, situacaoMensalidade));

            sql = " UPDATE Mensalidade SET IdSituacao=@Situacao WHERE Id IN" +
              " (SELECT Mensalidade.Id FROM Mensalidade INNER JOIN MensalidadeTitulo ON MensalidadeTitulo.IdMensalidade=Mensalidade.Id " +
              " INNER JOIN BoletoTitulo ON BoletoTitulo.IdTitulo = MensalidadeTitulo.IdTitulo " +
              " INNER JOIN Boleto ON Boleto.Id = BoletoTitulo.IdBoleto " +
              " WHERE Boleto.NossoNumero = @NossoNumero)";

            this.Conexao.Update(sql, parametros);

        }


        /// <summary>
        /// Altera a situação de Boleto 
        /// </summary>        
        /// <returns></returns>
        public void QuitarBoleto(Models.OperacaoNetEmpresa operacao)
        {
            Int32[] boletos = this.PegarBoletosASeremQuitados(operacao);
            string ids = String.Join(",", boletos);

            Int32 situacaoTitulo, situacaoMensalidade;
            Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this._conexao);

            List<DbParameter> parametros = new List<DbParameter>();
            //string statusBoleto = (operacao.ValorPago + operacao.ValorOscilacao >= operacao.ValorTitulo)
            //    ? "Quitado"
            //    : "Aberto";

            string statusBoleto = "Quitado";

            if (statusBoleto.Equals("Quitado"))
            {
                situacaoTitulo = repSituacao.BuscarPelaSituacao("Título", "Quitado").Id;
                situacaoMensalidade = repSituacao.BuscarPelaSituacao("Mensalidade", "Quitada").Id;
            }
            else
            {
                situacaoTitulo = repSituacao.BuscarPelaSituacao("Título", "Aberto").Id;
                situacaoMensalidade = repSituacao.BuscarPelaSituacao("Mensalidade", "Aberta").Id;
            }

            String sql =@" UPDATE Boleto SET  StatusBoleto=@StatusBoleto, ValorPago=@ValorPago, DataPagamento=@DataPagamento 
                            WHERE Id IN (@ids)";

            parametros.Add(this.Conexao.CriarParametro("@ValorPago", DbType.Decimal, operacao.ValorPago));
            parametros.Add(this.Conexao.CriarParametro("@DataPagamento", DbType.DateTime, operacao.DataLeitura));
            parametros.Add(this.Conexao.CriarParametro("@StatusBoleto", DbType.String, statusBoleto));
            parametros.Add(this.Conexao.CriarParametro("@ids", DbType.String, ids));


            this.Conexao.Update(sql, parametros);

            parametros = new List<DbParameter>();
            parametros.Add(this.Conexao.CriarParametro("@Situacao", DbType.Int32, situacaoTitulo));
            parametros.Add(this.Conexao.CriarParametro("@ValorPago", DbType.Decimal, operacao.ValorPago));
            parametros.Add(this.Conexao.CriarParametro("@DataPagamento", DbType.DateTime, operacao.DataLeitura));
            parametros.Add(this.Conexao.CriarParametro("@ids", DbType.String, ids));

            sql = " UPDATE Titulo SET IdSituacao=@Situacao,ValorPago=@ValorPago,DataOperacao=@DataPagamento WHERE Id IN " +
                " (SELECT Titulo.Id FROM Titulo INNER JOIN BoletoTitulo ON BoletoTitulo.IdTitulo = Titulo.Id " +
                " WHERE BoletoTitulo.IdBoleto IN (@ids))";

            this.Conexao.Update(sql, parametros);

            parametros = new List<DbParameter>();
            parametros.Add(this.Conexao.CriarParametro("@Situacao", DbType.Int32, situacaoMensalidade));
            parametros.Add(this.Conexao.CriarParametro("@ids", DbType.String, ids));

            sql = " UPDATE Mensalidade SET IdSituacao=@Situacao WHERE Id IN" +
              " (SELECT Mensalidade.Id FROM Mensalidade INNER JOIN MensalidadeTitulo ON MensalidadeTitulo.IdMensalidade=Mensalidade.Id " +
              " INNER JOIN BoletoTitulo ON BoletoTitulo.IdTitulo = MensalidadeTitulo.IdTitulo " +
              " WHERE BoletoTitulo.IdBoleto IN (@ids))";


            this.Conexao.Update(sql, parametros);

        }


        /// <summary>
        /// Altera a situação de Boleto 
        /// </summary>        
        /// <returns></returns>
        public void SalvarOperacaoNetEmpresa(Models.OperacaoNetEmpresa operacao)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO OperacaoNetEmpresa(Tipo,Pagador,SeuNumero,NossoNumero,ValorPago,ValorTitulo,ValorOscilacao,DataVencimento,DataLeitura,NomeArquivo) " +
                        " VALUES (@Tipo,@Pagador,@SeuNumero,@NossoNumero,@ValorPago,@ValorTitulo,@ValorOscilacao,@DataVencimento,@DataLeitura,@NomeArquivo)";

            parametros.Add(this.Conexao.CriarParametro("@Tipo", DbType.String, operacao.Tipo));
            parametros.Add(this.Conexao.CriarParametro("@Pagador", DbType.String, operacao.Pagador));
            parametros.Add(this.Conexao.CriarParametro("@SeuNumero", DbType.String, operacao.SeuNumero));
            parametros.Add(this.Conexao.CriarParametro("@NossoNumero", DbType.String, operacao.NossoNumero));
            parametros.Add(this.Conexao.CriarParametro("@ValorPago", DbType.Decimal, operacao.ValorPago));
            parametros.Add(this.Conexao.CriarParametro("@ValorTitulo", DbType.Decimal, operacao.ValorTitulo));
            parametros.Add(this.Conexao.CriarParametro("@ValorOscilacao", DbType.Decimal, operacao.ValorOscilacao));
            parametros.Add(this.Conexao.CriarParametro("@DataVencimento", DbType.DateTime, operacao.DataVencimento));
            parametros.Add(this.Conexao.CriarParametro("@DataLeitura", DbType.DateTime, operacao.DataLeitura));
            parametros.Add(this.Conexao.CriarParametro("@NomeArquivo", DbType.String, operacao.NomeArquivo));

            Int32 id = this.Conexao.Insert(sql, parametros);
        }

        /// <summary>
        /// Grava o log de Boleto
        /// </summary>
        /// <param name="idRegistro">id do registro</param>
        /// <param name="tipoOperacaoLog">tipo de operação: INSERIR, ALTERAR, EXCLUIR</param>
        /// <returns>true/false </returns>
        protected Boolean GravarLog(Int32 idRegistro, TipoOperacaoLog tipoOperacaoLog)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sqlLog = " INSERT INTO BoletoLog(IdUsuarioLog, IdOperacaoLog, DataLog, Id, StatusBoleto, IdCorporacao, Valor, Desconto, Juros, " +
                            " Convenio, Carteira, NumeroDocumento, Banco, CodigoBanco, NumeroAgencia, CodigoCedente, NossoNumero, DataEmissao, DataVencimento, ValorPago, DataPagamento)" +
                            " SELECT @IdUsuarioLog, @IdOperacaoLog, @DataLog, @Id, StatusBoleto, IdCorporacao, Valor, Desconto, Juros, Convenio, Carteira, NumeroDocumento, Banco," +
                            " CodigoBanco, NumeroAgencia, CodigoCedente, NossoNumero, DataEmissao, DataVencimento, ValorPago, DataPagamento FROM Boleto WHERE Id = @Id ";


            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@DataLog", DbType.DateTime, Helpers.DataHora.GetCurrentNow()));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "BoletoLog");
        }

        /// <summary>
        /// Insere um registro em BoletoRetorno 
        /// </summary>
        /// <returns></returns>
        public void InserirRetorno(string nomeArquivo,string numeroDocumento,string operacao,DateTime dataOperacao)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO BoletoRetorno(NomeArquivo, NumeroDocumento, Operacao, DataOperacao) " +
                        " VALUES (@NomeArquivo, @NumeroDocumento, @Operacao, @DataOperacao)";

            parametros.Add(this.Conexao.CriarParametro("@NomeArquivo", DbType.String, nomeArquivo));
            parametros.Add(this.Conexao.CriarParametro("@NumeroDocumento", DbType.String, numeroDocumento));
            parametros.Add(this.Conexao.CriarParametro("@Operacao", DbType.String, operacao));
            parametros.Add(this.Conexao.CriarParametro("@DataOperacao", DbType.DateTime, dataOperacao));
           
            Int32 id = this.Conexao.Insert(sql, parametros);
            // GravarLog(id, TipoOperacaoLog.Inserir);
        }

        public bool VerificarSeArquivoFoiLido(string nomeArquivo)
        {
            string sql = "SELECT Count(Id) FROM OperacaoNetEmpresa WHERE NomeArquivo='" + nomeArquivo + "' AND YEAR(DataLeitura) = " + DateTime.Now.Year.ToString();
            Int32 qnt = Convert.ToInt32(this.Conexao.Select(sql).Rows[0][0].ToString());

            return qnt > 0;
        }

        public void ArquivoRemessaGerado(Int32 id)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE Boleto SET RemessaGerado=1 WHERE Id=@Id";

            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, id));           
            this.Conexao.Update(sql, parametros);
        }

        public void QuitarBoletoManualmente(Int32[] ids)
        {
            String sql = "UPDATE Boleto SET StatusBoleto = 'Quitado na Escola',RemessaGerado=1,ValorPago=Valor,DataPagamento=GETDATE() WHERE Id  IN (" + Conversor.ToString(",", ids.ToArray()) + ")";
            this.Conexao.Update(sql);
        }

        public void CancelarBoletoManualmente(Int32[] ids)
        {
            String sql = "UPDATE Boleto SET RemessaGerado=1,StatusBoleto = 'Cancelado' WHERE Id IN (" + Conversor.ToString(",", ids.ToArray()) + ")";
            this.Conexao.Update(sql);
        }

        public void AtualizarNumeroSequencial(Int32 sequencial)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            parametros.Add(this.Conexao.CriarParametro("@Sequencial", DbType.Int32, sequencial));

            String sql = "UPDATE ConfiguracaoSistema SET Valor=@Sequencial WHERE Atributo='boleto_sequencial'";
            this.Conexao.Update(sql, parametros);
        }

        public void AtualizarValorBoletos(Int32[] ids,decimal valor,decimal desconto)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE Boleto SET Desconto=@Desconto,Valor= @Valor WHERE Id IN (" + Conversor.ToString(",", ids.ToArray()) + ")";
            parametros.Add(this.Conexao.CriarParametro("@Desconto", DbType.Decimal, valor));
            parametros.Add(this.Conexao.CriarParametro("@Valor", DbType.Decimal, desconto));

            this.Conexao.Update(sql,parametros);
        }

        public Int32[] BuscarProximosBoletos(Int32 idBoleto)
        {
            string sql = @"SELECT DISTINCT Boleto.Id from MatriculaCurso
            INNER JOIN Matricula ON MatriculaCurso.IdMatricula = Matricula.Id
            INNER JOIN Aluno ON Aluno.Id = Matricula.IdAluno
            INNER JOIN Turma ON MatriculaCurso.IdTurma = Turma.Id
            Inner JOIN Mensalidade ON Mensalidade.IdMatriculaCUrso = MatriculaCurso.Id
            INNER JOIN MensalidadeTitulo ON  MensalidadeTitulo.IdMensalidade = Mensalidade.Id
            INNER JOIN BoletoTitulo ON BoletoTitulo.IdTitulo = MensalidadeTitulo.IdTitulo
            INNER JOIN Boleto ON Boleto.Id = BoletoTitulo.IdBoleto
            WHERE Matricula.Id = (select DISTINCT Matricula.Id from MatriculaCurso
            INNER JOIN Matricula ON MatriculaCurso.IdMatricula = Matricula.Id
            INNER JOIN Aluno ON Aluno.Id = Matricula.IdAluno
            INNER JOIN Turma ON MatriculaCurso.IdTurma = Turma.Id
            Inner JOIN Mensalidade ON Mensalidade.IdMatriculaCUrso = MatriculaCurso.Id
            INNER JOIN MensalidadeTitulo ON  MensalidadeTitulo.IdMensalidade = Mensalidade.Id
            INNER JOIN BoletoTitulo ON BoletoTitulo.IdTitulo = MensalidadeTitulo.IdTitulo WHERE BoletoTitulo.IdBoleto = @IdBoleto) AND Boleto.DataVencimento > GETDATE()";

            List<DbParameter> parametros = new List<DbParameter>();
            parametros.Add(this.Conexao.CriarParametro("@IdBoleto", DbType.Int32, idBoleto));

            Prion.Generic.Models.Lista lista = this.Select(sql, parametros);
            List<Int32> ids = new List<int>();

            for (int i = 0, len = lista.DataTable.Rows.Count; i < len; i++)
            {
                ids.Add(Convert.ToInt32(lista.DataTable.Rows[0][0].ToString()));
            }

            return ids.ToArray();
        }

        public Int32[] PegarBoletosASeremQuitados(Models.OperacaoNetEmpresa operacao)
        {
            string sql = @"Select distinct Boleto.Id from Boleto
                            INNER JOIN BoletoTitulo ON Boleto.Id = BoletoTitulo.IdBoleto
                            INNER JOIN MensalidadeTitulo ON MensalidadeTitulo.IdTitulo = BoletoTitulo.IdTitulo
                            INNER JOIN Mensalidade ON Mensalidade.Id = MensalidadeTitulo.IdMensalidade
                            INNER JOIN MatriculaCurso ON Mensalidade.IdMatriculaCurso = MatriculaCurso.Id
                            INNER JOIN Turma ON Turma.Id = MatriculaCurso.IdTurma
                            INNER JOIN CursoAnoLetivo ON CursoAnoLetivo.Id = Turma.IdCursoAnoLetivo
                            INNER JOIN Matricula ON MatriculaCurso.IdMatricula = Matricula.Id
                            INNER JOIN Aluno ON Aluno.Id = Matricula.IdAluno
                            Where Matricula.NumeroMatricula like @Matricula AND MONTH(Boleto.DataVencimento) = @MesVencimento AND YEAR(Boleto.DataVencimento) = @AnoVencimento AND StatusBoleto = 'Aberto'";

            List<DbParameter> parametros = new List<DbParameter>();
            parametros.Add(this.Conexao.CriarParametro("@Matricula", DbType.String, operacao.Matricula));
            parametros.Add(this.Conexao.CriarParametro("@MesVencimento", DbType.Int32, operacao.DataVencimento.Month));
            parametros.Add(this.Conexao.CriarParametro("@AnoVencimento", DbType.Int32, operacao.DataVencimento.Year));

            Prion.Generic.Models.Lista lista = this.Select(sql, parametros);
            List<Int32> ids = new List<int>();

            for (int i = 0, len = lista.DataTable.Rows.Count; i < len; i++)
            {
                ids.Add(Convert.ToInt32(lista.DataTable.Rows[0][0].ToString()));
            }

            return ids.ToArray();
        }
    }
}