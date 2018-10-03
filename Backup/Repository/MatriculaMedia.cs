using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Prion.Data;
using System.Data;
using Prion.Tools;
using System.Data.Common;

namespace NotaAzul.Repository
{
    public class MatriculaMedia:Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public MatriculaMedia(ref DBFacade conexao)
            : base(ref conexao)
        { 
        }

        ~MatriculaMedia()
        { 
        }


        /// <summary>
        /// Busca todas as Nota da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "MatriculaMedia.*,Disciplina.Id as IdDisciplina,ComposicaoNotaPeriodo.PeriodoDeAvaliacao as PeriodoAvaliacao  ";
            String join = " FROM MatriculaMedia INNER JOIN ComposicaoNotaPeriodo ON ComposicaoNotaPeriodo .Id =MatriculaMedia.IdComposicaoNotaPeriodo " +
                        " INNER JOIN ProfessorDisciplina ON ProfessorDisciplina.Id = ComposicaoNotaPeriodo.IdProfessorDisciplina " +
                        " INNER JOIN Disciplina ON Disciplina.Id = ProfessorDisciplina.IdDisciplina ";
            String whereDefault = "";
            String orderBy = "MatriculaMedia.Id";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy, groupBy, parametro);
            List<Models.MatriculaMedia> listaMatriculaMedia = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaMatriculaMedia = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaMatriculaMedia != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaMatriculaMedia);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }

        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de Nota
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.MatriculaMedia com os registros do DataTable</returns>
        public List<Models.MatriculaMedia> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }


            List<Models.MatriculaMedia> lista = new List<Models.MatriculaMedia>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.Aluno
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Models.MatriculaMedia matriculaMedia = new Models.MatriculaMedia();

                matriculaMedia.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                matriculaMedia.IdMatricula = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdMatricula"].ToString());
                matriculaMedia.IdComposicaoNotaPeriodo = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdComposicaoNotaPeriodo"].ToString());
                matriculaMedia.IdDisciplina = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdDisciplina"].ToString());
                matriculaMedia.PeriodoAvaliacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "PeriodoAvaliacao"].ToString());
                matriculaMedia.ValorAlcancado = Conversor.ToDecimal(dataTable.Rows[i][nomeBase + "ValorAlcancado"].ToString());
                matriculaMedia.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                matriculaMedia.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(matriculaMedia);
                    continue;
                }

                matriculaMedia = CarregarEntidades(Entidades, matriculaMedia);
                lista.Add(matriculaMedia);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }

        /// <summary>
        /// Insere um registro em Nota através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.FormaDeAvaliacao</param>
        /// <returns></returns>
        protected override int Inserir(Prion.Generic.Models.Base objeto)
        {
            Models.MatriculaMedia oMatriculaMedia = (Models.MatriculaMedia)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO MatriculaMedia(IdMatricula, IdComposicaoNotaPeriodo, ValorAlcancado) " +
                            "VALUES (@IdMatricula, @IdComposicaoNotaPeriodo, @ValorAlcancado)";

            parametros.Add(this.Conexao.CriarParametro("@IdMatricula", DbType.Int32, oMatriculaMedia.IdMatricula));
            parametros.Add(this.Conexao.CriarParametro("@IdComposicaoNotaPeriodo", DbType.Int32, oMatriculaMedia.IdComposicaoNotaPeriodo));
            parametros.Add(this.Conexao.CriarParametro("@ValorAlcancado", DbType.Decimal, oMatriculaMedia.ValorAlcancado));

            Int32 id = this.Conexao.Insert(sql, parametros);
            GravarLog(id, TipoOperacaoLog.Inserir);

            return id;
        }

        /// <summary>
        /// Altera um registro de Nota através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Nota</param>
        /// <returns></returns>
        protected override Int32 Alterar(Prion.Generic.Models.Base objeto)
        {
            Models.MatriculaMedia oMatriculaMedia = (Models.MatriculaMedia)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE MatriculaMedia SET IdMatricula=@IdMatricula, IdComposicaoNotaPeriodo=@IdComposicaoNotaPeriodo, ValorAlcancado=@ValorAlcancado " +
                            " WHERE Id =@Id ";

            parametros.Add(this.Conexao.CriarParametro("@IdMatricula", DbType.Int32, oMatriculaMedia.IdMatricula));
            parametros.Add(this.Conexao.CriarParametro("@IdComposicaoNotaPeriodo", DbType.Int32, oMatriculaMedia.IdComposicaoNotaPeriodo));
            parametros.Add(this.Conexao.CriarParametro("@ValorAlcancado", DbType.Decimal, oMatriculaMedia.ValorAlcancado));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, oMatriculaMedia.Id));

            Int32 id = this.Conexao.Update(sql, parametros);
            GravarLog(id, TipoOperacaoLog.Alterar);

            return id;
        }

        /// <summary>
        /// Salva a Nota
        /// </summary>
        /// <param name="objeto">Objeto base que será convertido para um objeto do tipo Models.Aluno</param>
        /// <param name="idAluno">id do último registro inserido</param>
        protected override Prion.Generic.Helpers.Retorno SalvarEntidade(Prion.Generic.Models.Base objeto, Int32 idAluno)
        {
            Models.MatriculaMedia media = (Models.MatriculaMedia)objeto;
            Repository.MatriculaNota repMatriculaNota = new Repository.MatriculaNota(ref this._conexao);
            

            Prion.Generic.Helpers.Retorno retorno = repMatriculaNota.Salvar(media.Notas);

            return retorno;
        }

        /// <summary>
        /// Carrega as demais entidades relacionadas a este objeto
        /// </summary>
        /// <param name="modulos">lista de módulos que serão utilizados para carregar as demais entidades</param>
        /// <param name="objTurno">Objeto do tipo Models.Base que será convertido em Models.Aluno</param>
        protected Models.MatriculaMedia CarregarEntidades(Prion.Tools.Entidade modulos, Prion.Generic.Models.Base objAluno)
        {
            Boolean modoCompleto = Entidades.Carregar(Helpers.Entidade.Tudo.ToString());
            Models.MatriculaMedia matriculaMedia = (Models.MatriculaMedia)objAluno;


            // verifica se possui o módulo COMPLETO ou se possui o módulo CORPORACAO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.MatriculaNota.ToString())))
            {
                Repository.MatriculaNota repMatriculaNota = new Repository.MatriculaNota(ref this._conexao);
                repMatriculaNota.Entidades = modulos;
                Prion.Tools.Request.ParametrosRequest parametro = new Prion.Tools.Request.ParametrosRequest();
                Prion.Tools.Request.Filtro f = new Request.Filtro("ComposicaoNota.IdComposicaoNotaPeriodo", "=", matriculaMedia.IdComposicaoNotaPeriodo);
                parametro.Filtro.Add(f);

                f = new Request.Filtro("MatriculaNota.IdMatricula", "=", matriculaMedia.IdMatricula);
                parametro.Filtro.Add(f);

                // carrega um objeto do tipo Models.Corporacao
                matriculaMedia.Notas = Prion.Tools.ListTo.CollectionToList < Models.MatriculaNota >(repMatriculaNota.Buscar(parametro).ListaObjetos);
            }

            // retorna o objeto de Aluno com as entidades que foram carregadas
            return matriculaMedia;
        }

        /// <summary>
        /// Grava o log de Nota
        /// </summary>
        /// <param name="idRegistro">id do registro</param>
        /// <param name="tipoOperacaoLog">tipo de operação: INSERIR, ALTERAR, EXCLUIR</param>
        /// <returns>true/false </returns>
        protected Boolean GravarLog(Int32 idRegistro, TipoOperacaoLog tipoOperacaoLog)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sqlLog = " if EXISTS (SELECT Id FROM MatriculaMedia WHERE Id = @Id) " +
                            " INSERT INTO MatriculaMediaLog(IdUsuarioLog, IdOperacaoLog, Id, IdMatricula, IdComposicaoNotaPeriodo, ValorAlcancado, DataCadastro)" +
                            " SELECT @IdUsuarioLog, @IdOperacaoLog, @Id,IdMatricula, IdComposicaoNotaPeriodo, ValorAlcancado, DataCadastro FROM MatriculaMedia WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "MatriculaMediaLog");
        }
    }
}