using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Prion.Data;
using Prion.Tools;

namespace NotaAzul.Repository
{
    public class AlunoResponsavelGrauResponsabilidade : Prion.Generic.Repository.Base
    {
        public AlunoResponsavelGrauResponsabilidade(ref DBFacade conexao)
            : base(ref conexao)
        {
        }

        ~AlunoResponsavelGrauResponsabilidade()
        { 
        }


         /// <summary>
        /// Busca todas as AlunoResponsavelGrauResponsabilidade da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "AlunoResponsavelGrauResponsabilidade.* ";
            String join = "FROM AlunoResponsavelGrauResponsabilidade ";
            String whereDefault = "";
            String orderBy = "AlunoResponsavelGrauResponsabilidade.Id";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy,groupBy, parametro);
            List<Models.AlunoResponsavelGrauResponsabilidade> listaResponsabilidade = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaResponsabilidade = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaResponsabilidade != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaResponsabilidade);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }


        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de AlunoResponsavelGrauResponsabilidade
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.AlunoResponsavelGrauResponsabilidade com os registros do DataTable</returns>
        public List<Models.AlunoResponsavelGrauResponsabilidade> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }


            List<Models.AlunoResponsavelGrauResponsabilidade> lista = new List<Models.AlunoResponsavelGrauResponsabilidade>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.AlunoResponsavelGrauResponsabilidade
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Models.AlunoResponsavelGrauResponsabilidade alunoResponsavelGrauResponsabilidade = new Models.AlunoResponsavelGrauResponsabilidade();

                alunoResponsavelGrauResponsabilidade.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                alunoResponsavelGrauResponsabilidade.IdAlunoResponsavel = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdAlunoResponsavel"].ToString());
                alunoResponsavelGrauResponsabilidade.IdAluno = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdAluno"].ToString());
                alunoResponsavelGrauResponsabilidade.Tipo = dataTable.Rows[i][nomeBase + "Tipo"].ToString();
                alunoResponsavelGrauResponsabilidade.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                alunoResponsavelGrauResponsabilidade.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(alunoResponsavelGrauResponsabilidade);
                    continue;
                }

                // carrega as demais entidades necessárias
                alunoResponsavelGrauResponsabilidade = CarregarEntidades(Entidades, alunoResponsavelGrauResponsabilidade);

                lista.Add(alunoResponsavelGrauResponsabilidade);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }


        /// <summary>
        /// Exclui um registro de Matricula através do Id
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
            String sql = "DELETE FROM AlunoResponsavelGrauResponsabilidade WHERE Id IN(" + strId + ")";

            retorno = this.Conexao.Delete(sql);
            return retorno;
        }


        /// <summary>
        /// Insere um registro em AlunoResponsavelGrauResponsabilidade através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.AlunoResponsavelGrauResponsabilidade</param>
        /// <returns></returns>
        protected override Int32 Inserir(Prion.Generic.Models.Base objeto)
        {
            Models.AlunoResponsavelGrauResponsabilidade oAlunoResponsavelGrauResponsabilidade = (Models.AlunoResponsavelGrauResponsabilidade) objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO AlunoResponsavelGrauResponsabilidade(IdAlunoResponsavel, IdAluno, Tipo) " + 
                        "VALUES(@IdAlunoResponsavel, @IdAluno, @Tipo)";

            parametros.Add(this.Conexao.CriarParametro("@IdAlunoResponsavel", DbType.Int32, oAlunoResponsavelGrauResponsabilidade.IdAlunoResponsavel));
            parametros.Add(this.Conexao.CriarParametro("@IdAluno", DbType.Int32, oAlunoResponsavelGrauResponsabilidade.IdAluno));
            parametros.Add(this.Conexao.CriarParametro("@Tipo", DbType.String, oAlunoResponsavelGrauResponsabilidade.Tipo));

            Int32 id = this.Conexao.Insert(sql, parametros);

            GravarLog(id, TipoOperacaoLog.Inserir);

            return id;
        }


        /// <summary>
        /// Altera um registro de AlunoResponsavelGrauResponsabilidade através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.AlunoResponsavelGrauResponsabilidade</param>
        /// <returns></returns>
        protected override Int32 Alterar(Prion.Generic.Models.Base objeto)
        {
            Models.AlunoResponsavelGrauResponsabilidade oAlunoResponsavelGrauResponsabilidade = (Models.AlunoResponsavelGrauResponsabilidade) objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE AlunoResponsavelGrauResponsabilidade SET IdAlunoResponsavel=@IdAlunoResponsavel, " + 
                        "IdAluno=@IdAluno, Tipo=@Tipo WHERE Id=@Id";

            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, oAlunoResponsavelGrauResponsabilidade.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdAlunoResponsavel", DbType.Int32, oAlunoResponsavelGrauResponsabilidade.IdAlunoResponsavel));
            parametros.Add(this.Conexao.CriarParametro("@IdAluno", DbType.Int32, oAlunoResponsavelGrauResponsabilidade.IdAluno));
            parametros.Add(this.Conexao.CriarParametro("@Tipo", DbType.String, oAlunoResponsavelGrauResponsabilidade.Tipo));

            Int32 linhasAfetadas = this.Conexao.Update(sql, parametros);

            GravarLog(oAlunoResponsavelGrauResponsabilidade.Id, TipoOperacaoLog.Alterar);

            return linhasAfetadas;
        }


        /// <summary>
        /// Exclui um registro de AlunoResponsavelGrauResponsabilidade através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.AlunoResponsavelGrauResponsabilidade</param>
        /// <returns>Quantidade de registros excluídos</returns>
        protected override Int32 Excluir(Prion.Generic.Models.Base objeto)
        {
            Models.AlunoResponsavelGrauResponsabilidade oAlunoResponsavelGrauResponsabilidade = (Models.AlunoResponsavelGrauResponsabilidade)objeto;
            return Excluir(oAlunoResponsavelGrauResponsabilidade.Id);
        }


        /// <summary>
        /// Carrega as demais entidades relacionadas a este objeto
        /// </summary>
        /// <param name="modulos">lista de módulos que serão utilizados para carregar as demais entidades</param>
        /// <param name="objTurno">Objeto do tipo Models.Base que será convertido em Models.AlunoResponsavelGrauResponsabilidade</param>
        protected Models.AlunoResponsavelGrauResponsabilidade CarregarEntidades(Prion.Tools.Entidade modulos, Prion.Generic.Models.Base objAlunoResponsavelGrauResponsabilidade)
        {
            // ************************************************************************************
            // NÃO HÁ NECESSIDADE DE CARREGAR OS DEMAIS OBJETOS (Chaves Estrangeiras)
            // Nunca iremos carregar apenas uma lista de Responsabilidade
            // ************************************************************************************

            return (Models.AlunoResponsavelGrauResponsabilidade)objAlunoResponsavelGrauResponsabilidade;
        }


        /// <summary>
        /// Grava o log de AlunoResponsavelGrauResponsabilidade
        /// </summary>
        /// <param name="idRegistro">id do registro</param>
        /// <param name="tipoOperacaoLog">tipo de operação: INSERIR, ALTERAR, EXCLUIR</param>
        /// <returns>true/false </returns>
        protected Boolean GravarLog(Int32 idRegistro, TipoOperacaoLog tipoOperacaoLog)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sqlLog = " if EXISTS (SELECT Id FROM AlunoResponsavelGrauResponsabilidade WHERE Id = @Id) " +
                            " INSERT INTO AlunoResponsavelGrauResponsabilidadeLog(IdUsuarioLog, IdOperacaoLog, Id, IdAlunoResponsavel, IdAluno, Tipo, DataCadastro)" + 
                            " SELECT @IdUsuarioLog, @IdOperacaoLog, @Id, IdAlunoResponsavel, IdAluno, Tipo, DataCadastro FROM AlunoResponsavelGrauResponsabilidade WHERE Id = @Id" ;

            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "AlunoResponsavelGrauResponsabilidadeLog");
        }
    }
}
