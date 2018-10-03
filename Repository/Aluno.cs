using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Prion.Data;
using Prion.Tools;
using System.Text;

namespace NotaAzul.Repository
{
    public class Aluno : Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public Aluno(ref DBFacade conexao)
            : base(ref conexao)
        { 
        }

        ~Aluno()
        { 
        }


        /// <summary>
        /// Busca todas as Aluno da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "Aluno.* ";
            String join = "FROM Aluno ";
            String whereDefault = "";
            String orderBy = "Aluno.Nome";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy,groupBy, parametro);
            List<Models.Aluno> listaAluno = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaAluno = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaAluno != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaAluno);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }


        /// <summary>
        /// Retorna, através de um (ou vários) ID, uma lista de objetos do tipo Models.Aluno
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
        /// Retorna, através do nome ou parte dele, uma lista de objetos do tipo Models.Aluno
        /// </summary>
        /// <param name="nomeAluno"></param>
        /// <returns></returns>
        public Prion.Generic.Models.Lista Buscar(String nomeAluno)
        {
            Prion.Tools.Request.ParametrosRequest parametro = new Prion.Tools.Request.ParametrosRequest();
            Prion.Tools.Request.Filtro f = new Request.Filtro("Nome", "LIKE", nomeAluno+"%");

            parametro.Filtro.Add(f);

            return Buscar(parametro);             
        }

        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de alunos
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.Aluno com os registros do DataTable</returns>
        public List<Models.Aluno> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }


            List<Models.Aluno> lista = new List<Models.Aluno>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.Aluno
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Models.Aluno aluno = new Models.Aluno();

                aluno.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                aluno.IdCorporacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdCorporacao"].ToString());
                aluno.IdSituacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdSituacao"].ToString());
                aluno.IdImagem = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdImagem"].ToString());
                aluno.Nome = dataTable.Rows[i][nomeBase + "Nome"].ToString();
                aluno.Sexo = dataTable.Rows[i][nomeBase + "Sexo"].ToString();
                aluno.GrupoSanguineo = dataTable.Rows[i][nomeBase + "GrupoSanguineo"].ToString();
                aluno.Cpf = dataTable.Rows[i][nomeBase + "CPF"].ToString();
                aluno.Rg = dataTable.Rows[i][nomeBase + "RG"].ToString();
                aluno.DiaPagamento = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "DiaPagamento"].ToString());
                aluno.DataNascimento = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataNascimento"].ToString());
                aluno.Nacionalidade = dataTable.Rows[i][nomeBase + "Nacionalidade"].ToString();
                aluno.Religiao = dataTable.Rows[i][nomeBase + "Religiao"].ToString();
                aluno.CorRaca = dataTable.Rows[i][nomeBase + "CorRaca"].ToString();
                aluno.RegistroDeNascimento = dataTable.Rows[i][nomeBase + "RegistroDeNascimento"].ToString();
                aluno.IdCidadeRegistroNascimento = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdCidadeRegistroNascimento"].ToString());
                aluno.IdEstadoRegistroNascimento = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdEstadoRegistroNascimento"].ToString());
                aluno.NumeroOrdem = dataTable.Rows[i][nomeBase + "NumeroOrdem"].ToString();
                aluno.Folhas = dataTable.Rows[i][nomeBase + "Folhas"].ToString();
                aluno.NumeroLivro = dataTable.Rows[i][nomeBase + "NumeroLivro"].ToString();
                aluno.NIS = dataTable.Rows[i][nomeBase + "NIS"].ToString();
                aluno.ObservacaoSaude = dataTable.Rows[i][nomeBase + "ObservacaoSaude"].ToString();
                aluno.ObservacaoMedicacao = dataTable.Rows[i][nomeBase + "ObservacaoMedicacao"].ToString();
                aluno.DataRegistroNascimento = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataRegistroNascimento"].ToString());
                aluno.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                aluno.GeraBoleto = Conversor.ToBoolean(dataTable.Rows[i][nomeBase + "GerarBoleto"].ToString());
                aluno.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(aluno);
                    continue;
                }

                // carrega as demais entidades necessárias
                aluno = CarregarEntidades(Entidades, aluno);

                lista.Add(aluno);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }


        /// <summary>
        /// Exclui um registro de Aluno através do Id
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
            String sqlUpdate = "UPDATE Aluno set IdSituacao = (SELECT Id From Situacao WHERE IdSituacaoTipo=(SELECT Id FROM SituacaoTipo WHERE Nome='Aluno') AND Nome='Inativo') "+
                    " WHERE Id IN (SELECT IdAluno FROM Matricula WHERE IdAluno IN ("+strId+"))"+
                    " AND Id IN ("+ strId +")";
            String sqlDelete = "DELETE FROM Aluno WHERE Id NOT IN ( "+
	                " SELECT IdAluno FROM Matricula WHERE IdAluno IN ("+strId+")) "+
                    " AND Id IN (" + strId + ")";

            retorno = this.Conexao.Delete(sqlUpdate+";"+sqlDelete);
            
            return retorno;
        }


        /// <summary>
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros sem WHERE
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista DataTable(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder campos = new StringBuilder(), join = new StringBuilder();
            campos.Append("Aluno.* ");
            join.Append(" FROM Aluno");

            if (Entidades.Carregar(Helpers.Entidade.Situacao.ToString()))
            {
                campos.Append(", " + Prion.Generic.Helpers.MapeamentoTabela.Situacao());
                join.Append(" INNER JOIN Situacao ON Aluno.IdSituacao = Situacao.Id ");
            }

            if (Entidades.Carregar(Helpers.Entidade.MatriculaCurso.ToString()))
            {
                    join.Append(" INNER JOIN Matricula ON Aluno.Id = Matricula.IdAluno ");
                    join.Append(" INNER JOIN MatriculaCurso ON Matricula.Id = MatriculaCurso.IdMatricula ");
            }

            String whereDefault = "";
            String where = this.MontarWhere(parametro, whereDefault);
            String orderBy = "Aluno.Nome";
            String groupBy = "";

            return this.Select("SELECT " + campos.ToString(), join.ToString(), where, orderBy,groupBy, parametro);
        }


        /// <summary>
        /// Insere um registro em Aluno através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Aluno</param>
        /// <returns></returns>
        protected override int Inserir(Prion.Generic.Models.Base objeto)
        {
            Models.Aluno oAluno = (Models.Aluno) objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO Aluno(IdCorporacao, IdSituacao, IdImagem, Nome, Sexo, GrupoSanguineo, CPF, RG, " +
                            "DiaPagamento, DataNascimento, Nacionalidade, Religiao, CorRaca, RegistroDeNascimento, IdCidadeRegistroNascimento, " +
                            "IdEstadoRegistroNascimento, NumeroOrdem, Folhas, NumeroLivro, DataRegistroNascimento, NIS, ObservacaoSaude, ObservacaoMedicacao,GerarBoleto) " +
                            "VALUES (@IdCorporacao, @IdSituacao, @IdImagem, @Nome, @Sexo, @GrupoSanguineo, @CPF, @RG, " +
                            "@DiaPagamento, @DataNascimento, @Nacionalidade, @Religiao, @CorRaca, @RegistroDeNascimento, @IdCidadeRegistroNascimento, " +
                            "@IdEstadoRegistroNascimento, @NumeroOrdem, @Folhas, @NumeroLivro, @DataRegistroNascimento, @NIS, @ObservacaoSaude, @ObservacaoMedicacao,@GerarBoleto)";

            parametros.Add(this.Conexao.CriarParametro("@IdCorporacao", DbType.Int32, UsuarioLogado.IdCorporacao));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, oAluno.IdSituacao));
            parametros.Add(this.Conexao.CriarParametro("@IdImagem", DbType.Int32, Conversor.ToDBNull(oAluno.IdImagem)));
            parametros.Add(this.Conexao.CriarParametro("@Nome", DbType.String, StringHelper.OnlyOneSpace(oAluno.Nome), 255));
            parametros.Add(this.Conexao.CriarParametro("@Sexo", DbType.String, oAluno.Sexo, 1));
            parametros.Add(this.Conexao.CriarParametro("@GrupoSanguineo", DbType.String, oAluno.GrupoSanguineo, 5));
            parametros.Add(this.Conexao.CriarParametro("@CPF", DbType.String, oAluno.Cpf, 11));
            parametros.Add(this.Conexao.CriarParametro("@RG", DbType.String, oAluno.Rg, 20));
            parametros.Add(this.Conexao.CriarParametro("@DiaPagamento", DbType.Int32, Conversor.ToDBNull(oAluno.DiaPagamento)));
            parametros.Add(this.Conexao.CriarParametro("@DataNascimento", DbType.DateTime, oAluno.DataNascimento));
            parametros.Add(this.Conexao.CriarParametro("@Nacionalidade", DbType.String, oAluno.Nacionalidade, 100));
            parametros.Add(this.Conexao.CriarParametro("@Religiao", DbType.String, oAluno.Religiao, 50));
            parametros.Add(this.Conexao.CriarParametro("@CorRaca", DbType.String, oAluno.CorRaca, 50));
            parametros.Add(this.Conexao.CriarParametro("@RegistroDeNascimento", DbType.String, oAluno.RegistroDeNascimento, 50));
            parametros.Add(this.Conexao.CriarParametro("@IdCidadeRegistroNascimento", DbType.Int32, oAluno.IdCidadeRegistroNascimento));
            parametros.Add(this.Conexao.CriarParametro("@IdEstadoRegistroNascimento", DbType.Int32, oAluno.IdEstadoRegistroNascimento));
            parametros.Add(this.Conexao.CriarParametro("@NumeroOrdem", DbType.String, oAluno.NumeroOrdem, 10));
            parametros.Add(this.Conexao.CriarParametro("@Folhas", DbType.String, oAluno.Folhas, 10));
            parametros.Add(this.Conexao.CriarParametro("@NumeroLivro", DbType.String, oAluno.NumeroLivro, 10));
            parametros.Add(this.Conexao.CriarParametro("@DataRegistroNascimento", DbType.Date,Conversor.ToDBNull(oAluno.DataRegistroNascimento)));
            parametros.Add(this.Conexao.CriarParametro("@NIS", DbType.String, oAluno.NIS, 15));
            parametros.Add(this.Conexao.CriarParametro("@ObservacaoSaude", DbType.String, oAluno.ObservacaoSaude, 255));
            parametros.Add(this.Conexao.CriarParametro("@ObservacaoMedicacao", DbType.String, oAluno.ObservacaoMedicacao, 255));
            parametros.Add(this.Conexao.CriarParametro("@GerarBoleto", DbType.String, oAluno.GeraBoleto, 255));

            Int32 id = this.Conexao.Insert(sql, parametros);
            GravarLog(id, TipoOperacaoLog.Inserir);

            return id;
        }


        /// <summary>
        /// Altera um registro de Aluno através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Aluno</param>
        /// <returns></returns>
        protected override Int32 Alterar(Prion.Generic.Models.Base objeto)
        {
            Models.Aluno oAluno = (Models.Aluno)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE Aluno SET IdCorporacao=@IdCorporacao, IdSituacao=@IdSituacao, IdImagem=@IdImagem, " + 
                        "Nome=@Nome, Sexo=@Sexo, GrupoSanguineo=@GrupoSanguineo, CPF=@CPF, RG=@RG, " +
                        "DiaPagamento=@DiaPagamento, DataNascimento=@DataNascimento, Nacionalidade=@Nacionalidade, " +
                        "Religiao=@Religiao, CorRaca=@CorRaca, RegistroDeNascimento=@RegistroDeNascimento, IdCidadeRegistroNascimento=@IdCidadeRegistroNascimento, IdEstadoRegistroNascimento=@IdEstadoRegistroNascimento, " +
                        "NumeroOrdem=@NumeroOrdem, Folhas=@Folhas, NumeroLivro=@NumeroLivro, DataRegistroNascimento=@DataRegistroNascimento, NIS=@NIS, " +
                        "ObservacaoSaude=@ObservacaoSaude, ObservacaoMedicacao=@ObservacaoMedicacao,GerarBoleto=@GerarBoleto " +
                        "WHERE Id=@Id";

            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, oAluno.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdCorporacao", DbType.Int32, UsuarioLogado.IdCorporacao));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, oAluno.IdSituacao));
            parametros.Add(this.Conexao.CriarParametro("@IdImagem", DbType.Int32, Conversor.ToDBNull(oAluno.IdImagem)));
            parametros.Add(this.Conexao.CriarParametro("@Nome", DbType.String, StringHelper.OnlyOneSpace(oAluno.Nome), 255));
            parametros.Add(this.Conexao.CriarParametro("@Sexo", DbType.String, oAluno.Sexo, 1));
            parametros.Add(this.Conexao.CriarParametro("@GrupoSanguineo", DbType.String, oAluno.GrupoSanguineo, 5));
            parametros.Add(this.Conexao.CriarParametro("@CPF", DbType.String, oAluno.Cpf, 11));
            parametros.Add(this.Conexao.CriarParametro("@RG", DbType.String, oAluno.Rg, 20));
            parametros.Add(this.Conexao.CriarParametro("@DiaPagamento", DbType.Int32, Conversor.ToDBNull(oAluno.DiaPagamento)));
            parametros.Add(this.Conexao.CriarParametro("@DataNascimento", DbType.DateTime, oAluno.DataNascimento));
            parametros.Add(this.Conexao.CriarParametro("@Nacionalidade", DbType.String, oAluno.Nacionalidade, 100));
            parametros.Add(this.Conexao.CriarParametro("@Religiao", DbType.String, oAluno.Religiao, 50));
            parametros.Add(this.Conexao.CriarParametro("@CorRaca", DbType.String, oAluno.CorRaca, 50));
            parametros.Add(this.Conexao.CriarParametro("@RegistroDeNascimento", DbType.String, oAluno.RegistroDeNascimento, 50));
            parametros.Add(this.Conexao.CriarParametro("@IdCidadeRegistroNascimento", DbType.Int32, oAluno.IdCidadeRegistroNascimento));
            parametros.Add(this.Conexao.CriarParametro("@IdEstadoRegistroNascimento", DbType.Int32, oAluno.IdEstadoRegistroNascimento));
            parametros.Add(this.Conexao.CriarParametro("@NumeroOrdem", DbType.String, oAluno.NumeroOrdem, 10));
            parametros.Add(this.Conexao.CriarParametro("@Folhas", DbType.String, oAluno.Folhas, 10));
            parametros.Add(this.Conexao.CriarParametro("@NumeroLivro", DbType.String, oAluno.NumeroLivro, 10));
            parametros.Add(this.Conexao.CriarParametro("@DataRegistroNascimento", DbType.Date, Conversor.ToDBNull(oAluno.DataRegistroNascimento)));
            parametros.Add(this.Conexao.CriarParametro("@NIS", DbType.String, oAluno.NIS, 15));
            parametros.Add(this.Conexao.CriarParametro("@ObservacaoSaude", DbType.String, oAluno.ObservacaoSaude, 255));
            parametros.Add(this.Conexao.CriarParametro("@ObservacaoMedicacao", DbType.String, oAluno.ObservacaoMedicacao, 255));
            parametros.Add(this.Conexao.CriarParametro("@GerarBoleto", DbType.String, oAluno.GeraBoleto, 255));

            Int32 linhasAfetadas = this.Conexao.Update(sql, parametros);

            GravarLog(oAluno.Id, TipoOperacaoLog.Alterar);

            return linhasAfetadas;
        }


        /// <summary>
        /// Exclui um registro de Aluno através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Aluno</param>
        /// <returns>Quantidade de registros excluídos</returns>
        protected override Int32 Excluir(Prion.Generic.Models.Base objeto)
        {
            Models.Aluno oAluno = (Models.Aluno)objeto;
            return Excluir(oAluno.Id);
        }


        /// <summary>
        /// Carrega as demais entidades relacionadas a este objeto
        /// </summary>
        /// <param name="modulos">lista de módulos que serão utilizados para carregar as demais entidades</param>
        /// <param name="objTurno">Objeto do tipo Models.Base que será convertido em Models.Aluno</param>
        protected Models.Aluno CarregarEntidades(Prion.Tools.Entidade modulos, Prion.Generic.Models.Base objAluno)
        {
            Boolean modoCompleto = Entidades.Carregar(Helpers.Entidade.Tudo.ToString());
            Models.Aluno aluno = (Models.Aluno)objAluno;


            // verifica se possui o módulo COMPLETO ou se possui o módulo CORPORACAO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Corporacao.ToString())))
            {
                Prion.Generic.Repository.Corporacao repCorporacao = new Prion.Generic.Repository.Corporacao(ref this._conexao);
                repCorporacao.Entidades = modulos;

                // carrega um objeto do tipo Models.Corporacao
                aluno.Corporacao = repCorporacao.BuscarPeloId(aluno.IdCorporacao);
            }


            // verifica se possui o módulo COMPLETO ou se possui o módulo SITUACAO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Situacao.ToString())))
            {
                Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this._conexao);
                repSituacao.Entidades = modulos;

                // carrega um objeto do tipo Models.Situacao, representando a situação do Aluno
                aluno.Situacao = (Prion.Generic.Models.Situacao)repSituacao.BuscarPeloId(aluno.IdSituacao).Get(0);
            }


            // verifica se possui o módulo COMPLETO ou se possui o módulo IMAGEM
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Imagem.ToString())))
            {
                Prion.Generic.Repository.Imagem repImagem = new Prion.Generic.Repository.Imagem(ref this._conexao);
                repImagem.Entidades = modulos;

                // carrega um objeto do tipo Models.Imagem, representando a foto do aluno (por exemplo)
                aluno.Imagem = (Prion.Generic.Models.Imagem)repImagem.BuscarPeloId(aluno.IdImagem).Get(0);
            }


            // verifica se possui o módulo COMPLETO ou se possui o módulo ALUNO_FILANTROPIA
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.AlunoFilantropia.ToString())))
            {
                Repository.AlunoFilantropia repAlunoFilantropia = new Repository.AlunoFilantropia(ref this._conexao);
                repAlunoFilantropia.Entidades = modulos;

                // carrega uma lista de objetos do tipo Models.AlunoFilantropia
                Prion.Tools.Request.ParametrosRequest p = new Request.ParametrosRequest();
                p.Filtro.Add(new Prion.Tools.Request.Filtro("IdAluno", "=", aluno.Id.ToString()));

                aluno.Filantropia = Prion.Tools.ListTo.CollectionToList<Models.AlunoFilantropia>(repAlunoFilantropia.Buscar(p).ListaObjetos);
            }


            // verifica se possui o módulo COMPLETO ou se possui o módulo ALUNO_RESPONSAVEL_GRAU_RESPONSABILIDADE
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.AlunoResponsavelGrauResponsabilidade.ToString())))
            {
                Repository.AlunoResponsavelGrauResponsabilidade repAlunoResponsavelGrauResponsabilidade = new Repository.AlunoResponsavelGrauResponsabilidade(ref this._conexao);
                repAlunoResponsavelGrauResponsabilidade.Entidades = modulos;

                // carrega uma lista de objetos do tipo Models.AlunoResponsavelGrauResponsabilidade
                Prion.Tools.Request.ParametrosRequest p = new Request.ParametrosRequest();
                p.Filtro.Add(new Prion.Tools.Request.Filtro("IdAluno", "=", aluno.Id.ToString()));

                aluno.GrauResponsabilidades = Prion.Tools.ListTo.CollectionToList<Models.AlunoResponsavelGrauResponsabilidade>(repAlunoResponsavelGrauResponsabilidade.Buscar(p).ListaObjetos);
            }

            // verifica se possui o módulo COMPLETO ou se possui o módulo ALUNO_RESPONSAVEL
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.AlunoResponsavel.ToString())))
            {
                Repository.AlunoResponsavel repAlunoResponsavel = new Repository.AlunoResponsavel(ref this._conexao);
                repAlunoResponsavel.Entidades = modulos;

                // carrega uma lista de objetos do tipo Models.AlunoResponsavel
                Prion.Tools.Request.ParametrosRequest p = new Request.ParametrosRequest();
                p.Filtro.Add(new Prion.Tools.Request.Filtro("IdAluno", "=", aluno.Id.ToString()));

                aluno.Responsaveis = Prion.Tools.ListTo.CollectionToList<Models.AlunoResponsavel>(repAlunoResponsavel.Buscar(p).ListaObjetos);
            }

            // verifica se possui o módulo COMPLETO ou se possui o módulo Estado
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Estado.ToString())))
            {
                Prion.Generic.Repository.Estado repEstado = new Prion.Generic.Repository.Estado(ref this._conexao);

                //carrega um objeto do tipo Generic.Models.Estado
                aluno.EstadoRegistroNascimento = (Prion.Generic.Models.Estado)repEstado.BuscarPeloId(aluno.IdEstadoRegistroNascimento).Get(0);
            }


            // verifica se possui o módulo COMPLETO ou se possui o módulo Cidade
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Cidade.ToString())))
            {
                Prion.Generic.Repository.Cidade repCidade = new Prion.Generic.Repository.Cidade(ref this._conexao);

                //carrega um objeto do tipo Generic.Models.Cidade
                Prion.Generic.Models.Cidade cidade = (Prion.Generic.Models.Cidade)repCidade.BuscarPeloId(aluno.IdCidadeRegistroNascimento).Get(0);
                aluno.CidadeRegistroNascimento = cidade;
            }

            // retorna o objeto de Aluno com as entidades que foram carregadas
            return aluno;
        }


        /// <summary>
        /// Grava o log de Aluno
        /// </summary>
        /// <param name="idRegistro">id do registro</param>
        /// <param name="tipoOperacaoLog">tipo de operação: INSERIR, ALTERAR, EXCLUIR</param>
        /// <returns>true/false </returns>
        protected Boolean GravarLog(Int32 idRegistro, TipoOperacaoLog tipoOperacaoLog)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sqlLog = " if EXISTS (SELECT Id FROM Aluno WHERE Id = @Id) " +
                            " INSERT INTO AlunoLog(IdUsuarioLog, IdOperacaoLog, Id, IdCorporacao, IdSituacao, IdImagem, " + 
                                "Nome, Sexo, GrupoSanguineo, CPF, RG, DiaPagamento, DataNascimento, Nacionalidade, Religiao, " +
                                "CorRaca, RegistroDeNascimento, IdCidadeRegistroNascimento, IdEstadoRegistroNascimento, NumeroOrdem, Folhas, NumeroLivro, DataRegistroNascimento, NIS, DataCadastro)" +
                            " SELECT @IdUsuarioLog, @IdOperacaoLog, @Id, IdCorporacao, IdSituacao, IdImagem, " + 
                                "Nome, Sexo, GrupoSanguineo, CPF, RG, DiaPagamento, DataNascimento, Nacionalidade, Religiao, " +
                                "CorRaca, RegistroDeNascimento, IdCidadeRegistroNascimento, IdEstadoRegistroNascimento, NumeroOrdem, Folhas, NumeroLivro, DataRegistroNascimento, NIS, DataCadastro FROM Aluno WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "AlunoLog");
        }


        /// <summary>
        /// Salva todos os relacionamentos de aluno
        /// </summary>
        /// <param name="objeto">Objeto base que será convertido para um objeto do tipo Models.Aluno</param>
        /// <param name="idAluno">id do último registro inserido</param>
        protected override Prion.Generic.Helpers.Retorno SalvarEntidade(Prion.Generic.Models.Base objeto, Int32 idAluno)
        {
            Models.Aluno aluno = (Models.Aluno)objeto;
            Repository.AlunoResponsavel repAlunoResponsavel = new Repository.AlunoResponsavel(ref this._conexao);

            // se o objeto de aluno estiver no estado 'NOVO', 
            // chama o método que irá definir o seu id (idAluno) em todos os objetos da lista de responsáveis
            if (aluno.EstadoObjeto == Prion.Generic.Helpers.Enums.EstadoObjeto.Novo)
            {
                AtualizarIdAluno(aluno, idAluno);
            }

            Prion.Generic.Helpers.Retorno retorno = repAlunoResponsavel.Salvar(aluno.Responsaveis);

            return retorno;
        }


        /// <summary>
        /// Atualiza o IdAluno de todos os registros de responsáveis
        /// </summary>
        /// <param name="aluno"></param>
        /// <param name="idAluno"></param>
        private void AtualizarIdAluno(Models.Aluno aluno, Int32 idAluno)
        {
            if (aluno == null)
            {
                return;
            }

            for (Int32 i = 0; i < aluno.Responsaveis.Count; i++)
            {
                aluno.Responsaveis[i].IdAluno = idAluno;
            }
        }

        /// <summary>
        /// Verifica se um registro de aluno pode ser excluído
        /// </summary>
        /// <param name="idaluno"></param>
        /// <returns>boolean </returns>
        public List<String> BuscarAlunosMatriculados(Int32[] idsAluno)
        {
         
            List<String> alunosMatriculados = new List<string>();
            String strIds = Conversor.ToString(",", idsAluno);
            // Seleciona mensalidades quitadas pertencentes às matrículas cujo id está presente no array
            String sql = "SELECT Id, Nome FROM Aluno "+
                    " WHERE Id IN (SELECT IdAluno FROM Matricula WHERE IdAluno IN ("+strIds+") )"+
                    "AND Id IN ("+ strIds +")";

            Prion.Generic.Models.Lista lista = this.Select(sql, null);

            for (Int32 i = 0; i < lista.DataTable.Rows.Count; i++)
            {
                alunosMatriculados.Add(lista.DataTable.Rows[i]["Nome"].ToString());
            }

            return alunosMatriculados;
        }
    
    }
}