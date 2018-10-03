using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Prion.Data;
using Prion.Tools;
using System.Text;

namespace NotaAzul.Repository
{
    public class AlunoResponsavel : Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public AlunoResponsavel(ref DBFacade conexao)
            : base(ref conexao)
        { 
        }

        ~AlunoResponsavel()
        { 
        }


        /// <summary>
        /// Busca todas as AlunoResponsavel da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "AlunoResponsavel.*";
            String join = "FROM AlunoResponsavel";
            String whereDefault = "";
            String orderBy = "AlunoResponsavel.Nome";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy,groupBy, parametro);
            List<Models.AlunoResponsavel> listaAlunoResponsavel = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaAlunoResponsavel = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaAlunoResponsavel != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaAlunoResponsavel);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }

        /// <summary>
        /// Busca um responsável através de um boleto
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Models.AlunoResponsavel BuscarResponsavelPorBoleto(Int32 idBoleto, Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder sql = new StringBuilder(), where = new StringBuilder();
            sql.Append("SELECT AlunoResponsavel.* FROM AlunoResponsavel ");
            sql.Append(" INNER JOIN Aluno ON Aluno.Id = AlunoResponsavel.IdAluno ");
            sql.Append(" INNER JOIN Matricula ON Aluno.Id = Matricula.IdAluno ");
            sql.Append(" INNER JOIN MatriculaCurso ON Matricula.Id = MatriculaCurso.IdMatricula ");
            sql.Append(" INNER JOIN Mensalidade ON Mensalidade.IdMatriculaCurso = MatriculaCurso.Id ");
            sql.Append(" INNER JOIN MensalidadeTitulo ON Mensalidade.Id = MensalidadeTitulo.IdMensalidade ");
            sql.Append(" INNER JOIN BoletoTitulo ON BoletoTitulo.IdTitulo = MensalidadeTitulo.IdTitulo ");
            where.Append(" Where AlunoResponsavel.Financeiro = 1 AND BoletoTitulo.IdBoleto = ");
            where.Append(idBoleto);


            Prion.Generic.Models.Lista lista = this.Select(sql.ToString() + where.ToString(), parametro);

            if(lista.DataTable.Rows.Count == 0)
            {
                where = new StringBuilder();
                where.Append(" Where  BoletoTitulo.IdBoleto = ");
                where.Append(idBoleto);

                lista = this.Select(sql.ToString() + where.ToString(), parametro);
            }

            List<Models.AlunoResponsavel> listaAlunoResponsavel = null;
            Entidades.Adicionar("Endereco");
            listaAlunoResponsavel = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaAlunoResponsavel != null)
            {
                return listaAlunoResponsavel[0];
            }

            return null;
        }


        /// <summary>
        /// Retorna, através de um (ou vários) ID, uma lista de objetos do tipo Models.AlunoResponsavel
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
        /// Retorna, através de um nome , uma lista de objetos do tipo Models.AlunoResponsavel
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Prion.Generic.Models.Lista Buscar(String nomeResponsavel)
        {
            Prion.Tools.Request.ParametrosRequest parametro = new Prion.Tools.Request.ParametrosRequest();
            Prion.Tools.Request.Filtro f = new Request.Filtro("Nome", "LIKE", nomeResponsavel+"%");

            parametro.Filtro.Add(f);

            return Buscar(parametro);
        }

        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de AlunoResponsavel
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.AlunoResponsavel com os registros do DataTable</returns>
        public List<Models.AlunoResponsavel> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }


            List<Models.AlunoResponsavel> lista = new List<Models.AlunoResponsavel>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.AlunoResponsavel
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Models.AlunoResponsavel alunoResponsavel = new Models.AlunoResponsavel();

                alunoResponsavel.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                alunoResponsavel.IdSituacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdSituacao"].ToString());
                alunoResponsavel.IdImagem = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdImagem"].ToString());
                alunoResponsavel.IdAluno = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdAluno"].ToString());
                alunoResponsavel.Nome = dataTable.Rows[i][nomeBase + "Nome"].ToString();
                alunoResponsavel.CPF = dataTable.Rows[i][nomeBase + "CPF"].ToString();
                alunoResponsavel.RG = dataTable.Rows[i][nomeBase + "RG"].ToString();
                alunoResponsavel.Email = dataTable.Rows[i][nomeBase + "Email"].ToString();
                alunoResponsavel.GrauParentesco = dataTable.Rows[i][nomeBase + "GrauParentesco"].ToString();
                alunoResponsavel.Profissao = dataTable.Rows[i][nomeBase + "Profissao"].ToString();
                alunoResponsavel.Financeiro = Conversor.ToBoolean(dataTable.Rows[i][nomeBase + "Financeiro"].ToString());
                alunoResponsavel.MoraCom = Conversor.ToBoolean(dataTable.Rows[i][nomeBase + "MoraCom"].ToString());
                alunoResponsavel.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                alunoResponsavel.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(alunoResponsavel);
                    continue;
                }

                // carrega as demais entidades necessárias
                alunoResponsavel = CarregarEntidades(Entidades, alunoResponsavel);

                lista.Add(alunoResponsavel);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }


        /// <summary>
        /// Exclui um registro de AlunoResponsavel através do Id
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
            String sql = "UPDATE AlunoResponsavel SET IdSituacao=(SELECT Id From Situacao WHERE IdSituacaoTipo=(SELECT Id FROM SituacaoTipo WHERE Nome='AlunoResponsavel') AND Nome='Inativo') WHERE Id IN(" + strId + ")";

            retorno = this.Conexao.Delete(sql);
            return retorno;
        }


        /// <summary>
        /// Insere um registro em AlunoResponsavel através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.AlunoResponsavel</param>
        /// <returns></returns>
        protected override Int32 Inserir(Prion.Generic.Models.Base objeto)
        {
            Models.AlunoResponsavel oAlunoResponsavel = (Models.AlunoResponsavel) objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO AlunoResponsavel(IdSituacao, IdImagem, IdAluno, Nome, CPF, GrauParentesco, Profissao, Financeiro, MoraCom, RG, Email) " +
                        "VALUES(@IdSituacao, @IdImagem, @IdAluno, @Nome, @CPF, @GrauParentesco, @Profissao, @Financeiro, @MoraCom, @RG, @Email)";

            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, oAlunoResponsavel.IdSituacao));
            parametros.Add(this.Conexao.CriarParametro("@IdImagem", DbType.Int32, Conversor.ToDBNull(oAlunoResponsavel.IdImagem)));
            parametros.Add(this.Conexao.CriarParametro("@IdAluno", DbType.Int32, oAlunoResponsavel.IdAluno));
            parametros.Add(this.Conexao.CriarParametro("@Nome", DbType.String, StringHelper.OnlyOneSpace(oAlunoResponsavel.Nome)));
            parametros.Add(this.Conexao.CriarParametro("@CPF", DbType.String, oAlunoResponsavel.CPF, 11));
            parametros.Add(this.Conexao.CriarParametro("@GrauParentesco", DbType.String, oAlunoResponsavel.GrauParentesco, 50));
            parametros.Add(this.Conexao.CriarParametro("@Profissao", DbType.String, StringHelper.OnlyOneSpace(oAlunoResponsavel.Profissao), 50));
            parametros.Add(this.Conexao.CriarParametro("@Financeiro", DbType.Boolean, oAlunoResponsavel.Financeiro));
            parametros.Add(this.Conexao.CriarParametro("@MoraCom", DbType.Boolean, oAlunoResponsavel.MoraCom));
            parametros.Add(this.Conexao.CriarParametro("@RG", DbType.String, oAlunoResponsavel.RG, 30));
            parametros.Add(this.Conexao.CriarParametro("@Email", DbType.String, oAlunoResponsavel.Email));
            Int32 id = this.Conexao.Insert(sql, parametros);

           // GravarLog(id, TipoOperacaoLog.Inserir);

            return id;
        }


        /// <summary>
        /// Altera um registro de AlunoResponsavel através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.AlunoResponsavel</param>
        /// <returns></returns>
        protected override Int32 Alterar(Prion.Generic.Models.Base objeto)
        {
            Models.AlunoResponsavel oAlunoResponsavel = (Models.AlunoResponsavel) objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE AlunoResponsavel SET IdSituacao=@IdSituacao, IdImagem=@IdImagem, IdAluno=@IdAluno, " +
                        "Nome=@Nome, CPF=@CPF, GrauParentesco=@GrauParentesco, Profissao=@Profissao, Financeiro=@Financeiro, MoraCom=@MoraCom, RG=@RG ,Email=@Email " + 
                        "WHERE Id=@Id";

            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, oAlunoResponsavel.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, oAlunoResponsavel.IdSituacao));
            parametros.Add(this.Conexao.CriarParametro("@IdImagem", DbType.Int32, Conversor.ToDBNull(oAlunoResponsavel.IdImagem)));
            parametros.Add(this.Conexao.CriarParametro("@IdAluno", DbType.Int32, oAlunoResponsavel.IdAluno));
            parametros.Add(this.Conexao.CriarParametro("@Nome", DbType.String, StringHelper.OnlyOneSpace(oAlunoResponsavel.Nome)));
            parametros.Add(this.Conexao.CriarParametro("@CPF", DbType.String, oAlunoResponsavel.CPF, 11));
            parametros.Add(this.Conexao.CriarParametro("@GrauParentesco", DbType.String, oAlunoResponsavel.GrauParentesco, 50));
            parametros.Add(this.Conexao.CriarParametro("@Profissao", DbType.String, StringHelper.OnlyOneSpace(oAlunoResponsavel.Profissao), 50));
            parametros.Add(this.Conexao.CriarParametro("@Financeiro", DbType.Boolean, oAlunoResponsavel.Financeiro));
            parametros.Add(this.Conexao.CriarParametro("@MoraCom", DbType.Boolean, oAlunoResponsavel.MoraCom));
            parametros.Add(this.Conexao.CriarParametro("@RG", DbType.String, oAlunoResponsavel.RG, 30));
            parametros.Add(this.Conexao.CriarParametro("@Email", DbType.String, oAlunoResponsavel.Email));
            Int32 linhasAfetadas = this.Conexao.Update(sql, parametros);

           // GravarLog(oAlunoResponsavel.Id, TipoOperacaoLog.Alterar);

            return linhasAfetadas;
        }


        /// <summary>
        /// Exclui um registro de AlunoResponsavel através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.AlunoResponsavel</param>
        /// <returns>Quantidade de registros excluídos</returns>
        protected override Int32 Excluir(Prion.Generic.Models.Base objeto)
        {
            Models.AlunoResponsavel oAlunoResponsavel = (Models.AlunoResponsavel)objeto;
            return Excluir(oAlunoResponsavel.Id);
        }


        /// <summary>
        /// Carrega as demais entidades relacionadas a este objeto
        /// </summary>
        /// <param name="modulos">lista de módulos que serão utilizados para carregar as demais entidades</param>
        /// <param name="objTurno">Objeto do tipo Models.Base que será convertido em Models.AlunoResponsavel</param>
        protected Models.AlunoResponsavel CarregarEntidades(Prion.Tools.Entidade modulos, Prion.Generic.Models.Base objAlunoResponsavel)
        {
            Boolean modoCompleto = Entidades.Carregar(Helpers.Entidade.Tudo.ToString());
            Models.AlunoResponsavel alunoResponsavel = (Models.AlunoResponsavel)objAlunoResponsavel;


            // verifica se possui o módulo COMPLETO ou se possui o módulo SITUACAO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Situacao.ToString())))
            {
                Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this._conexao);
                repSituacao.Entidades = modulos;

                // carrega um objeto do tipo Models.Situacao, representando a situação do Responsável
                alunoResponsavel.Situacao = (Prion.Generic.Models.Situacao)repSituacao.BuscarPeloId(alunoResponsavel.IdSituacao).Get(0);
            }


            // verifica se possui o módulo COMPLETO ou se possui o módulo IMAGEM
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Imagem.ToString())))
            {
                Prion.Generic.Repository.Imagem repImagem = new Prion.Generic.Repository.Imagem(ref this._conexao);
                repImagem.Entidades = modulos;

                // carrega um objeto do tipo Models.Imagem, representando a foto do aluno (por exemplo)
                alunoResponsavel.Imagem = (Prion.Generic.Models.Imagem)repImagem.BuscarPeloId(alunoResponsavel.IdImagem).Get(0);
            }


            // verifica se possui o módulo COMPLETO ou se possui o módulo ENDERECO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Endereco.ToString())))
            {
                Repository.AlunoResponsavelEndereco repEndereco = new Repository.AlunoResponsavelEndereco(ref this._conexao);
                repEndereco.Entidades = modulos;

                // carrega um objeto do tipo Models.Endereco, representando os endereços de um responsável
                alunoResponsavel.Endereco = (Prion.Generic.Models.Endereco)repEndereco.Buscar(alunoResponsavel.Id).Get(0);
            }

            // verifica se possui o módulo completo ou o se possui o módulo TELEFONE
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Telefone.ToString())))
            {
                Repository.AlunoResponsavelTelefone repTelefone = new Repository.AlunoResponsavelTelefone(ref this._conexao);
                repTelefone.Entidades = modulos;

                // carrega objeto do tipo Models.Telefone, representando os telefones de um responsável
                Prion.Tools.Request.ParametrosRequest p = new Request.ParametrosRequest();
                p.Filtro.Add(new Prion.Tools.Request.Filtro("IdAlunoResponsavel", "=", alunoResponsavel.Id.ToString()));
                
                alunoResponsavel.Telefones = Prion.Tools.ListTo.CollectionToList<Models.AlunoResponsavelTelefone>(repTelefone.Buscar(p).ListaObjetos);
            }

            // retorna o objeto de AlunoResponsavel com as entidades que foram carregadas
            return alunoResponsavel;
        }


        /// <summary>
        /// Salva todas as entidades: Endereço e relacionamento de endereço com responsável
        /// </summary>
        /// <param name="objeto">Objeto base que será convertido em um objeto do tipo Models.AlunoResponsavel</param>
        /// <returns></returns>
        protected override Prion.Generic.Helpers.Retorno SalvarEntidade(Prion.Generic.Models.Base objeto, Int32 idAlunoResponsavel)
        {
            Models.AlunoResponsavel oAlunoResponsavel = (Models.AlunoResponsavel) objeto;
            
            // atualiza o Id do relacionamento entre Telefone e AlunoResponsavel 
            for (Int32 i = 0; i < oAlunoResponsavel.Telefones.Count; i++)
            {
                if (oAlunoResponsavel.Telefones[i].IdAlunoResponsavel == 0)
                {
                    oAlunoResponsavel.Telefones[i].IdAlunoResponsavel = idAlunoResponsavel;
                }
            }

            NotaAzul.Repository.AlunoResponsavelTelefone repTelefone = new AlunoResponsavelTelefone(ref this._conexao);
            repTelefone.Salvar(oAlunoResponsavel.Telefones);

            Prion.Generic.Repository.Endereco repEndereco = new Prion.Generic.Repository.Endereco(ref this._conexao);
            Prion.Generic.Helpers.Retorno retorno = repEndereco.Salvar(oAlunoResponsavel.Endereco);

            // só vai criar o relacionamento de Responsável com Endereço se for um Responsável novo
            if (oAlunoResponsavel.EstadoObjeto == Prion.Generic.Helpers.Enums.EstadoObjeto.Novo)
            {
                CriarRelacionamentoComEndereco(idAlunoResponsavel, retorno.UltimoId);
            }
            
            return retorno;
        }


        /// <summary>
        /// Grava o log de AlunoResponsavel
        /// </summary>
        /// <param name="idRegistro">id do registro</param>
        /// <param name="tipoOperacaoLog">tipo de operação: INSERIR, ALTERAR, EXCLUIR</param>
        /// <returns>true/false </returns>
        protected Boolean GravarLog(Int32 idRegistro, TipoOperacaoLog tipoOperacaoLog)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sqlLog = " if EXISTS (SELECT Id FROM AlunoResponsavel WHERE Id = @Id) " +
                            " INSERT INTO AlunoResponsavelLog(IdUsuarioLog, IdOperacaoLog, Id, IdSituacao, IdImagem, IdAluno, " +
                                "Nome, CPF, GrauParentesco, Profissao, Financeiro, DataCadastro, MoraCom, RG)" +
                            " SELECT @IdUsuarioLog, @IdOperacaoLog, @Id, IdSituacao, IdImagem, IdAluno, " +
                                "Nome, CPF, GrauParentesco, Profissao, Financeiro, DataCadastro, MoraCom, RG " + 
                                "FROM AlunoResponsavel WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "AlunoResponsavelLog");
        }


        /// <summary>
        /// Cria o relacionamento entre AlunoResponsavel e Endereço
        /// </summary>
        /// <param name="idResponsavel"></param>
        /// <param name="idEndereco"></param>
        private Int32 CriarRelacionamentoComEndereco(Int32 idResponsavel, Int32 idEndereco)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO AlunoResponsavelEndereco(IdAlunoResponsavel, IdEndereco) " +
                        "VALUES(@IdAlunoResponsavel, @IdEndereco)";

            parametros.Add(this.Conexao.CriarParametro("@IdAlunoResponsavel", DbType.Int32, idResponsavel));
            parametros.Add(this.Conexao.CriarParametro("@IdEndereco", DbType.Int32, idEndereco));
            
            Int32 id = this.Conexao.Insert(sql, parametros);
            return id;
        }
    }
}