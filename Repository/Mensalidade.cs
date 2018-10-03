using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Prion.Data;
using Prion.Tools;
using System.Text;

namespace NotaAzul.Repository
{
    public class Mensalidade : Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public Mensalidade(ref DBFacade conexao)
            : base(ref conexao)
        { 
        }


        ~Mensalidade()
        {
        }


        /// <summary>
        /// Busca todas as Mensalidade da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "Mensalidade.* ";
            String join = "FROM Mensalidade ";
            String whereDefault = "";
            String orderBy = "Mensalidade.DataCadastro";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy,groupBy, parametro);
            List<Models.Mensalidade> listaMensalidade = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaMensalidade = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaMensalidade != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaMensalidade);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }


        /// <summary>
        /// Retorna, através de um (ou vários) IdMatriculaCurso, uma lista de objetos do tipo Models.Mensalidade
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Prion.Generic.Models.Lista Buscar(params Int32[] ids)
        {
            Prion.Tools.Request.ParametrosRequest parametro = new Prion.Tools.Request.ParametrosRequest();
            Prion.Tools.Request.Filtro f = new Request.Filtro("IdMatriculaCurso", "IN", Conversor.ToString(",", ids));

            parametro.Filtro.Add(f);

            return Buscar(parametro);
        }

        /// <summary>
        /// Retorna, através de um (ou vários) Id, uma lista de objetos do tipo Models.Mensalidade
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Prion.Generic.Models.Lista BuscarPeloId(params Int32[] ids)
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
            campos.Append("Mensalidade.* ");
            join.Append("FROM Mensalidade");

            if (Entidades.Carregar(Helpers.Entidade.Situacao.ToString()))
            {
                campos.Append(", " + Prion.Generic.Helpers.MapeamentoTabela.Situacao());
                join.Append(" INNER JOIN Situacao ON Mensalidade.IdSituacao = Situacao.Id ");
            }

            String whereDefault = "";
            String orderBy = "Mensalidade.DataCadastro";
            String groupBy = "";

            return this.Select("SELECT " + campos.ToString(), join.ToString(), whereDefault, orderBy,groupBy, parametro);
        }


        /// <summary>
        /// Insere um registro em Mensalidade através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Mensalidade</param>
        /// <returns></returns>
        protected override int Inserir(Prion.Generic.Models.Base objeto)
        {
            Models.Mensalidade oMensalidade = (Models.Mensalidade)objeto;
            Prion.Generic.Models.Titulo titulo = CriarTituloAPartirDeMensalidade(oMensalidade) ;
            Prion.Generic.Repository.Titulo repTitulo = new Prion.Generic.Repository.Titulo(ref this._conexao);
            
            List<DbParameter> parametros = new List<DbParameter>();

            String sql = "INSERT INTO Mensalidade(IdCorporacao, IdSituacao,IdMatriculaCurso, Valor, Acrescimo, Desconto, Isento, DataVencimento)" +
                            "VALUES (@IdCorporacao, @IdSituacao,@IdMatriculaCurso, @Valor, @Acrescimo, @Desconto, @Isento, @DataVencimento)";

            parametros.Add(this.Conexao.CriarParametro("@IdCorporacao", DbType.Int32, 1));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, oMensalidade.IdSituacao));
            parametros.Add(this.Conexao.CriarParametro("@IdMatriculaCurso", DbType.Int32, oMensalidade.IdMatriculaCurso));
            parametros.Add(this.Conexao.CriarParametro("@Valor", DbType.Decimal, oMensalidade.Valor));
            parametros.Add(this.Conexao.CriarParametro("@Acrescimo", DbType.Decimal, oMensalidade.Acrescimo));
            parametros.Add(this.Conexao.CriarParametro("@Desconto", DbType.Decimal, oMensalidade.Desconto));
            parametros.Add(this.Conexao.CriarParametro("@Isento", DbType.Boolean, oMensalidade.Isento));
            parametros.Add(this.Conexao.CriarParametro("@DataVencimento", DbType.DateTime, oMensalidade.DataVencimento));
           
            Int32 id = this.Conexao.Insert(sql, parametros);

            GravarLog(id, TipoOperacaoLog.Inserir);

            Prion.Generic.Helpers.Retorno retorno = repTitulo.Salvar(titulo);
            CriarRelacionamentoTituloMensalidade(id, retorno.UltimoId);
            //CriarRelacionamentoTituloBoleto(oMensalidade.IdBoleto, retorno.UltimoId);
            return id;
        }


        /// <summary>
        /// Altera um registro de Mensalidade através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Mensalidade</param>
        /// <returns></returns>
        protected override Int32 Alterar(Prion.Generic.Models.Base objeto)
        {
            Models.Mensalidade oMensalidade = (Models.Mensalidade)objeto;

            Prion.Generic.Models.Titulo titulo = CriarTituloAPartirDeMensalidade(oMensalidade);
            titulo.Id = CarregarRelacionamentoMensalidadeTitulo(oMensalidade.Id);
            titulo.DataOperacao = CarregarDataOperacaoTitulo(titulo.Id);
            titulo.IdSituacao = CarregarSituacaoTitulo(titulo.Id);
            titulo.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Alterado;

            Prion.Generic.Repository.Titulo repTitulo = new Prion.Generic.Repository.Titulo(ref this._conexao);
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE Mensalidade SET  IdSituacao=@IdSituacao,IdMatriculaCurso=@IdMatriculaCurso,Valor=@Valor, " + 
                            "Acrescimo=@Acrescimo, Desconto=@Desconto, Isento=@Isento, DataVencimento=@DataVencimento " + 
                            "WHERE Id=@Id";

            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, oMensalidade.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, oMensalidade.IdSituacao));
            parametros.Add(this.Conexao.CriarParametro("@IdMatriculaCurso", DbType.Int32, oMensalidade.IdMatriculaCurso));
            parametros.Add(this.Conexao.CriarParametro("@Valor", DbType.Decimal, oMensalidade.Valor));
            parametros.Add(this.Conexao.CriarParametro("@Acrescimo", DbType.Decimal, oMensalidade.Acrescimo));
            parametros.Add(this.Conexao.CriarParametro("@Desconto", DbType.Decimal, oMensalidade.Desconto));
            parametros.Add(this.Conexao.CriarParametro("@Isento", DbType.Boolean, oMensalidade.Isento));
            parametros.Add(this.Conexao.CriarParametro("@DataVencimento", DbType.DateTime, oMensalidade.DataVencimento));
           

            Int32 linhasAfetadas = this.Conexao.Update(sql, parametros);
            repTitulo.Salvar(titulo);
            GravarLog(oMensalidade.Id, TipoOperacaoLog.Alterar);

            return linhasAfetadas;
        }


        /// <summary>
        /// Exclui um registro de Mensalidade através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Mensalidade</param>
        /// <returns>Quantidade de registros excluídos</returns>
        protected override Int32 Excluir(Prion.Generic.Models.Base objeto)
        {
            Models.Mensalidade oMensalidade = (Models.Mensalidade)objeto;
            return Excluir(oMensalidade.Id);
        }


        /// <summary>
        /// Exclui um registro de Mensalidade através do Id
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
            String sql = "DELETE FROM Mensalidade WHERE Mensalidade.Id IN(" + strId + ") AND Mensalidade.IdSituacao !="+
                " (SELECT Situacao.Id FROM Situacao INNER JOIN SituacaoTipo on Situacao.IdSituacaoTipo=SituacaoTipo.Id WHERE Situacao.Nome = 'Quitada' AND SituacaoTipo.Nome='Mensalidade')";

            retorno = this.Conexao.Delete(sql);
            return retorno;
        }


        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de Mensalidade
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.Mensalidade com os registros do DataTable</returns>
        public List<Models.Mensalidade> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }


            List<Models.Mensalidade> lista = new List<Models.Mensalidade>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.Mensalidade
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Models.Mensalidade mensalidade = new Models.Mensalidade();

                mensalidade.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                mensalidade.IdCorporacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdCorporacao"].ToString());
                mensalidade.IdMatriculaCurso = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdMatriculaCurso"].ToString());
                mensalidade.IdSituacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdSituacao"].ToString());
                mensalidade.Valor = Conversor.ToDecimal(dataTable.Rows[i][nomeBase + "Valor"].ToString());
                mensalidade.Acrescimo = Conversor.ToDecimal(dataTable.Rows[i][nomeBase + "Acrescimo"].ToString());
                mensalidade.Desconto = Conversor.ToDecimal(dataTable.Rows[i][nomeBase + "Desconto"].ToString());
                mensalidade.Isento = Conversor.ToBoolean(dataTable.Rows[i][nomeBase + "Isento"].ToString());
                mensalidade.DataVencimento = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataVencimento"].ToString());
                mensalidade.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                mensalidade.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(mensalidade);
                    continue;
                }

                // carrega as demais entidades necessárias
                mensalidade = CarregarEntidades(Entidades, mensalidade);

                lista.Add(mensalidade);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }


        /// <summary>
        /// Carrega as demais entidades relacionadas a este objeto
        /// </summary>
        /// <param name="modulos">lista de módulos que serão utilizados para carregar as demais entidades</param>
        /// <param name="objTurno">Objeto do tipo Models.Base que será convertido em Models.Mensalidade</param>
        protected Models.Mensalidade CarregarEntidades(Prion.Tools.Entidade modulos, Prion.Generic.Models.Base objMensalidade)
        {
            Boolean modoCompleto = Entidades.Carregar(Helpers.Entidade.Tudo.ToString());
            Models.Mensalidade mensalidade = (Models.Mensalidade)objMensalidade;

            // verifica se possui o módulo COMPLETO ou se possui o módulo SITUACAO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Situacao.ToString())))
            {
                Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this._conexao);
                repSituacao.Entidades = modulos;

                // carrega um objeto do tipo Models.Situacao, representando a situação do Mensalidade
                mensalidade.Situacao = (Prion.Generic.Models.Situacao)repSituacao.BuscarPeloId(mensalidade.IdSituacao).Get(0);
            }

            // verifica se possui o módulo COMPLETO ou se possui o módulo CORPORACAO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Corporacao.ToString())))
            {
                Prion.Generic.Repository.Corporacao repCorporacao = new Prion.Generic.Repository.Corporacao(ref this._conexao);
                repCorporacao.Entidades = modulos;

                // carrega um objeto do tipo Models.Corporacao
                mensalidade.Corporacao = (Prion.Generic.Models.Corporacao)repCorporacao.BuscarPeloId(mensalidade.IdCorporacao);
            }


            // verifica se possui o módulo COMPLETO ou se possui o módulo Titulo
          
                Prion.Generic.Repository.Titulo repTitulo = new Prion.Generic.Repository.Titulo(ref this._conexao);
                repTitulo.Entidades = modulos;

                Repository.ContasReceber repContasReceber = new Repository.ContasReceber(ref this._conexao);
                Int32[] idsTitulo = repContasReceber.CarregarRelacionamentoMensalidadeTitulo(mensalidade.Id);

                // carrega um objeto do tipo Models.Corporacao
                mensalidade.Titulo = (Prion.Generic.Models.Titulo)repTitulo.BuscarPeloId(idsTitulo).Get(0);
            

            // retorna o objeto de Mensalidade com as entidades que foram carregadas
            return mensalidade;
        }

         /// <summary>
        /// Cria um objeto de título a partir de um objeto de mensalidade
        /// </summary>
        /// <param name="oMensalidade">objeto de mensalidade</param>       
        /// <returns>obj de Título</returns>
        protected Prion.Generic.Models.Titulo CriarTituloAPartirDeMensalidade(Models.Mensalidade oMensalidade)
        {
            Prion.Generic.Models.Titulo titulo = new Prion.Generic.Models.Titulo();
            Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this._conexao);
            Prion.Generic.Repository.TituloTipo reptTituloTipo = new Prion.Generic.Repository.TituloTipo(ref this._conexao);

            //Cria o filtro para buscar o tipo de título correspondente à mensalidade
            Prion.Tools.Request.ParametrosRequest parametro = new Prion.Tools.Request.ParametrosRequest();
            Prion.Tools.Request.Filtro f = new Request.Filtro("TituloTipo.Nome", "LIKE", "Mensalidade");

            parametro.Filtro.Add(f);

            titulo.Acrescimo = oMensalidade.Acrescimo;
            titulo.Corporacao = oMensalidade.Corporacao;
            titulo.DataVencimento = oMensalidade.DataVencimento;
            titulo.Desconto = oMensalidade.Desconto;
            titulo.IdCorporacao = oMensalidade.IdCorporacao;
            titulo.Valor = oMensalidade.Valor;            
            titulo.TipoOperacao = Prion.Generic.Helpers.Enums.TipoOperacao.Credito;
            titulo.Descricao = "Mensalidade";
            //Busca a situação "ABERTO" de Título
            titulo.Situacao = repSituacao.BuscarPelaSituacao("Título", "Aberto");
            titulo.IdSituacao = titulo.Situacao.Id;

            titulo.TituloTipo = (Prion.Generic.Models.TituloTipo)reptTituloTipo.Buscar(parametro).Get(0);
            titulo.IdTituloTipo = titulo.TituloTipo.Id;

            return titulo;
        }
               

        /// <summary>
        /// Cria o relacionamento entre um título e uma mensalidade
        /// </summary>
        /// <param name="idMensalidade"></param>      
        /// <param name="idTitulo"></param>       
        /// <returns>Chave do relacionamento</returns>
        protected Int32 CriarRelacionamentoTituloMensalidade(Int32 idMensalidade, Int32 idTitulo)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO MensalidadeTitulo(IdMensalidade, IdTitulo) " +
                        "VALUES(@IdMensalidade, @IdTitulo)";

            parametros.Add(this.Conexao.CriarParametro("@IdMensalidade", DbType.Int32, idMensalidade));
            parametros.Add(this.Conexao.CriarParametro("@IdTitulo", DbType.Int32, idTitulo));
       
            Int32 id = this.Conexao.Insert(sql, parametros);
            return id;
        }

        /// <summary>
        /// Cria o relacionamento entre um título e uma mensalidade
        /// </summary>
        /// <param name="idMensalidade"></param>      
        /// <param name="idTitulo"></param>       
        /// <returns>Chave do relacionamento</returns>
        protected Int32 CriarRelacionamentoTituloBoleto(Int32 idBoleto, Int32 idTitulo)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO BoletoTitulo(IdBoleto, IdTitulo) " +
                        "VALUES(@IdBoleto, @IdTitulo)";

            parametros.Add(this.Conexao.CriarParametro("@IdMensalidade", DbType.Int32, idBoleto));
            parametros.Add(this.Conexao.CriarParametro("@IdTitulo", DbType.Int32, idTitulo));

            Int32 id = this.Conexao.Insert(sql, parametros);
            return id;
        }

        /// <summary>
        /// Carrega o id de um Título
        /// </summary>       
        /// <param name="idMensalidade"></param>
        /// <param name="IdSituacao"></param>   
        /// <returns>id de um título</returns>
        public Int32 CarregarRelacionamentoMensalidadeTitulo(Int32 idMensalidade)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "MensalidadeTitulo.* ";
            String join = "FROM MensalidadeTitulo ";
            String whereDefault = "";
            String orderBy = "MensalidadeTitulo.IdTitulo";
            String groupBy = "";

            //Busca as movimentações de acordo com o relacionamento entre movimentação financeira e título
            Prion.Tools.Request.ParametrosRequest parametro = new Prion.Tools.Request.ParametrosRequest();
            Prion.Tools.Request.Filtro f = new Request.Filtro("IdMensalidade", "IN", idMensalidade);
            parametro.Filtro.Add(f);

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy, groupBy, parametro);
            Int32 qntResultados = lista.DataTable.Rows.Count;
            Int32 idTitulo = 0;
            for (Int32 i = 0; i < qntResultados; i++)
            {
                idTitulo = Conversor.ToInt32(lista.DataTable.Rows[i]["IdTitulo"].ToString());
            }

            return idTitulo;

        }

        /// <summary>
        /// Carrega o IdSituacao de um Título
        /// </summary>       
        /// <param name="idTitulo"></param>          
        /// <returns>id de um título</returns>
        public Int32 CarregarSituacaoTitulo(Int32 idTitulo)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "Titulo.IdSituacao ";
            String join = "FROM Titulo ";
            String whereDefault = "";
            String orderBy = "Titulo.IdSituacao";
            String groupBy = "";

            //Busca as movimentações de acordo com o relacionamento entre movimentação financeira e título
            Prion.Tools.Request.ParametrosRequest parametro = new Prion.Tools.Request.ParametrosRequest();
            Prion.Tools.Request.Filtro f = new Request.Filtro("Id", "IN", idTitulo);
            parametro.Filtro.Add(f);

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy, groupBy, parametro);
            Int32 qntResultados = lista.DataTable.Rows.Count;
            Int32 idSituacao = 0;
            for (Int32 i = 0; i < qntResultados; i++)
            {
                idSituacao = Conversor.ToInt32(lista.DataTable.Rows[i]["IdSituacao"].ToString());
            }

            return idSituacao;

        }

        /// <summary>
        /// Carrega a data de Operacao de um Título
        /// </summary>       
        /// <param name="idTitulo"></param>   
        /// <returns>dataOperacao</returns>
        public DateTime CarregarDataOperacaoTitulo(Int32 idTitulo)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "Titulo.DataOperacao ";
            String join = "FROM Titulo ";
            String whereDefault = "";
            String orderBy = "Titulo.DataOperacao";
            String groupBy = "";

            //Busca as movimentações de acordo com o relacionamento entre movimentação financeira e título
            Prion.Tools.Request.ParametrosRequest parametro = new Prion.Tools.Request.ParametrosRequest();
            Prion.Tools.Request.Filtro f = new Request.Filtro("Id", "IN", idTitulo);
            parametro.Filtro.Add(f);

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy, groupBy, parametro);
            Int32 qntResultados = lista.DataTable.Rows.Count;
            DateTime dataOperacao = new DateTime();
            for (Int32 i = 0; i < qntResultados; i++)
            {
                dataOperacao = Conversor.ToDateTime(lista.DataTable.Rows[i]["DataOperacao"].ToString());
            }

            return dataOperacao;

        }

        /// <summary>
        /// Altera um registro de Mensalidade através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Mensalidade</param>
        /// <returns></returns>
        public void AlterarDataVencimento(Int32 diaVencimento,Int32 idAluno)
        {
                     
            List<DbParameter> parametros = new List<DbParameter>();
            Business.Situacao biSituacao = new Business.Situacao();            


            String sql = " if EXISTS (SELECT Mensalidade.Id FROM Mensalidade INNER JOIN MatriculaCurso ON Mensalidade.IdMatriculaCurso=MatriculaCurso.Id " +
                            " INNER JOIN Matricula ON Matricula.Id=MatriculaCurso.IdMatricula "+            
                            " WHERE Mensalidade.IdSituacao =(SELECT Situacao.Id FROM Situacao INNER JOIN SituacaoTipo "+
                            " ON Situacao.IdSituacaoTipo = SituacaoTipo.Id WHERE Situacao.Nome='Aberta' AND SituacaoTipo.Nome='Mensalidade') " +
                            " AND Matricula.IdAluno = @IdAluno) " +
                            " UPDATE Mensalidade SET  Mensalidade.DataVencimento = DATEADD(dd,@DiaVencimento-DatePart(dd, Mensalidade.DataVencimento),Mensalidade.DataVencimento) " +
                            " FROM Mensalidade INNER JOIN MatriculaCurso ON Mensalidade.IdMatriculaCurso=MatriculaCurso.Id " +
                            " INNER JOIN Matricula ON Matricula.Id=MatriculaCurso.IdMatricula WHERE Mensalidade.IdSituacao = (SELECT Situacao.Id FROM Situacao INNER JOIN SituacaoTipo "+
                            " ON Situacao.IdSituacaoTipo = SituacaoTipo.Id WHERE Situacao.Nome='Aberta' AND SituacaoTipo.Nome='Mensalidade') " +
                            " AND Matricula.IdAluno = @IdAluno";

           
            parametros.Add(this.Conexao.CriarParametro("@DiaVencimento", DbType.Int32, diaVencimento));
            parametros.Add(this.Conexao.CriarParametro("@IdAluno", DbType.Int32, idAluno));

            this.Conexao.Scalar(sql, parametros);

            sql = " if EXISTS (SELECT Titulo.Id FROM Titulo INNER JOIN MensalidadeTitulo on MensalidadeTitulo.IdTitulo = Titulo.Id INNER JOIN Mensalidade ON Mensalidade.Id = MensalidadeTitulo.IdMensalidade " +
                            " INNER JOIN MatriculaCurso ON Mensalidade.IdMatriculaCurso=MatriculaCurso.Id " +
                            " INNER JOIN Matricula ON Matricula.Id=MatriculaCurso.IdMatricula " +
                            " WHERE Mensalidade.IdSituacao =(SELECT Situacao.Id FROM Situacao INNER JOIN SituacaoTipo " +
                            " ON Situacao.IdSituacaoTipo = SituacaoTipo.Id WHERE Situacao.Nome='Aberta' AND SituacaoTipo.Nome='Mensalidade') " +
                            " AND Matricula.IdAluno = @IdAluno) " +
                            " UPDATE Titulo SET Titulo.DataVencimento = DATEADD(dd,@DiaVencimento-DatePart(dd, Titulo.DataVencimento),Titulo.DataVencimento) " +
                            " FROM Titulo INNER JOIN MensalidadeTitulo on MensalidadeTitulo.IdTitulo = Titulo.Id INNER JOIN Mensalidade ON Mensalidade.Id = MensalidadeTitulo.IdMensalidade " +
                            " INNER JOIN MatriculaCurso ON Mensalidade.IdMatriculaCurso=MatriculaCurso.Id " +
                            " INNER JOIN Matricula ON Matricula.Id=MatriculaCurso.IdMatricula " +
                            " WHERE Mensalidade.IdSituacao =(SELECT Situacao.Id FROM Situacao INNER JOIN SituacaoTipo " +
                            " ON Situacao.IdSituacaoTipo = SituacaoTipo.Id WHERE Situacao.Nome='Aberta' AND SituacaoTipo.Nome='Mensalidade') " +
                            " AND Matricula.IdAluno = @IdAluno";

            this.Conexao.Scalar(sql, parametros);

            String sqlLog = " if EXISTS (SELECT Mensalidade.Id FROM Mensalidade INNER JOIN MatriculaCurso ON Mensalidade.IdMatriculaCurso=MatriculaCurso.Id " +
                            " INNER JOIN Matricula ON Matricula.Id=MatriculaCurso.IdMatricula " +
                            " WHERE Mensalidade.IdSituacao =(SELECT Situacao.Id FROM Situacao INNER JOIN SituacaoTipo " +
                            " ON Situacao.IdSituacaoTipo = SituacaoTipo.Id WHERE Situacao.Nome='Aberta' AND SituacaoTipo.Nome='Mensalidade') " +
                            " AND Matricula.IdAluno = @IdAluno) " +
                            " INSERT INTO MensalidadeLog(IdUsuarioLog, IdOperacaoLog, Id, IdCorporacao, IdMatriculaCurso, IdSituacao, Valor, Acrescimo, Desconto, Isento, DataVencimento, DataCadastro)" +
                            " SELECT @IdUsuarioLog, @IdOperacaoLog, Mensalidade.Id, Mensalidade.IdCorporacao, Mensalidade.IdMatriculaCurso,"+
                            " Mensalidade.IdSituacao,Mensalidade.Valor,Mensalidade.Acrescimo,Mensalidade.Desconto,Mensalidade.Isento,Mensalidade.DataVencimento, Mensalidade.DataCadastro "+
                            " FROM Mensalidade INNER JOIN MatriculaCurso ON Mensalidade.IdMatriculaCurso=MatriculaCurso.Id " +
                            " INNER JOIN Matricula ON Matricula.Id=MatriculaCurso.IdMatricula WHERE Mensalidade.IdSituacao = (SELECT Situacao.Id FROM Situacao INNER JOIN SituacaoTipo "+
                            " ON Situacao.IdSituacaoTipo = SituacaoTipo.Id WHERE Situacao.Nome='Aberta' AND SituacaoTipo.Nome='Mensalidade') " +
                            " AND Matricula.IdAluno = @IdAluno";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)TipoOperacaoLog.Alterar));
           
            Log.GravarLog(sqlLog, parametros, "MensalidadeLog");

            sqlLog = " if EXISTS (SELECT Titulo.Id FROM Titulo INNER JOIN MensalidadeTitulo on MensalidadeTitulo.IdTitulo = Titulo.Id INNER JOIN Mensalidade ON Mensalidade.Id = MensalidadeTitulo.IdMensalidade " +
                            " INNER JOIN MatriculaCurso ON Mensalidade.IdMatriculaCurso=MatriculaCurso.Id " +
                            " INNER JOIN Matricula ON Matricula.Id=MatriculaCurso.IdMatricula " +
                            " WHERE Mensalidade.IdSituacao =(SELECT Situacao.Id FROM Situacao INNER JOIN SituacaoTipo " +
                            " ON Situacao.IdSituacaoTipo = SituacaoTipo.Id WHERE Situacao.Nome='Aberta' AND SituacaoTipo.Nome='Mensalidade') " +
                            " AND Matricula.IdAluno = @IdAluno) " +
                            " INSERT INTO TituloLog(IdUsuarioLog, IdOperacaoLog, Id, IdCorporacao, IdTituloTipo,IdSituacao, Numero, Descricao, Valor, Observacao, Acrescimo, Desconto, DataVencimento,"+ 
                            " DataOperacao, DataCadastro, Tipo, ValorPago)" +
                            " SELECT @IdUsuarioLog, @IdOperacaoLog, Titulo.Id, Titulo.IdCorporacao, Titulo.IdTituloTipo,Titulo.IdSituacao, Titulo.Numero, Titulo.Descricao, Titulo.Valor,"+
                            " Titulo.Observacao, Titulo.Acrescimo, Titulo.Desconto, Titulo.DataVencimento," +
                            " Titulo.DataOperacao, Titulo.DataCadastro, Titulo.Tipo, Titulo.ValorPago " +
                            " FROM Titulo INNER JOIN MensalidadeTitulo on MensalidadeTitulo.IdTitulo = Titulo.Id INNER JOIN Mensalidade ON Mensalidade.Id = MensalidadeTitulo.IdMensalidade " +
                            " INNER JOIN MatriculaCurso ON Mensalidade.IdMatriculaCurso=MatriculaCurso.Id " +
                            " INNER JOIN Matricula ON Matricula.Id=MatriculaCurso.IdMatricula " +
                            " WHERE Mensalidade.IdSituacao =(SELECT Situacao.Id FROM Situacao INNER JOIN SituacaoTipo " +
                            " ON Situacao.IdSituacaoTipo = SituacaoTipo.Id WHERE Situacao.Nome='Aberta' AND SituacaoTipo.Nome='Mensalidade') " +
                            " AND Matricula.IdAluno = @IdAluno";

            Log.GravarLog(sqlLog, parametros, "TituloLog");
           
        }

        /// <summary>
        /// Grava o log de Mensalidade
        /// </summary>
        /// <param name="idRegistro">id do registro</param>
        /// <param name="tipoOperacaoLog">tipo de operação: INSERIR, ALTERAR, EXCLUIR</param>
        /// <returns>true/false </returns>
        protected Boolean GravarLog(Int32 idRegistro, TipoOperacaoLog tipoOperacaoLog)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sqlLog = " if EXISTS (SELECT Id FROM Mensalidade WHERE Id = @Id) " +
                            " INSERT INTO MensalidadeLog(IdUsuarioLog, IdOperacaoLog, Id, IdCorporacao, IdMatriculaCurso, IdSituacao, Valor, Acrescimo, Desconto, Isento, DataVencimento, DataCadastro)" +
                            " SELECT @IdUsuarioLog, @IdOperacaoLog, @Id, IdCorporacao, IdMatriculaCurso, IdSituacao, Valor, Acrescimo, Desconto, Isento, DataVencimento, DataCadastro FROM Mensalidade WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "MensalidadeLog");
        }
    }
}