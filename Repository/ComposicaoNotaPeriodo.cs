using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Prion.Data;
using Prion.Tools;
using System.Data;
using System.Data.Common;

namespace NotaAzul.Repository
{
    public class ComposicaoNotaPeriodo:Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public ComposicaoNotaPeriodo(ref DBFacade conexao)
            : base(ref conexao)
        { 
        }

        ~ComposicaoNotaPeriodo()
        { 
        }

        /// <summary>
        /// Busca todas as ComposicaoNota da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "ComposicaoNotaPeriodo.* ";
            String join = "FROM ComposicaoNotaPeriodo INNER JOIN ProfessorDisciplina ON ComposicaoNotaPeriodo.IdProfessorDisciplina = ProfessorDisciplina.Id";
            String whereDefault = "";
            String orderBy = "ComposicaoNotaPeriodo.Id";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy, groupBy, parametro);
            List<Models.ComposicaoNotaPeriodo> listaComposicaoNota = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaComposicaoNota = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaComposicaoNota != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaComposicaoNota);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }
 

        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de Nota
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.ComposicaoNota com os registros do DataTable</returns>
        public List<Models.ComposicaoNotaPeriodo> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }


            List<Models.ComposicaoNotaPeriodo> lista = new List<Models.ComposicaoNotaPeriodo>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.Aluno
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Models.ComposicaoNotaPeriodo composicaoNota = new Models.ComposicaoNotaPeriodo();

                composicaoNota.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                composicaoNota.IdProfessorDisciplina = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdProfessorDisciplina"].ToString());
                composicaoNota.PeriodoDeAvaliacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "PeriodoDeAvaliacao"].ToString());
                composicaoNota.FormaDivisaoAnoLetivo = dataTable.Rows[i][nomeBase + "FormaDivisaoAnoLetivo"].ToString();
                composicaoNota.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                composicaoNota.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(composicaoNota);
                    continue;
                }

                // carrega as demais entidades necessárias
                composicaoNota = CarregarEntidades(Entidades, composicaoNota);

                lista.Add(composicaoNota);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }

        /// <summary>
        /// Exclui um registro de ComposicaoNota através do Id
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

            String sql = "DELETE FROM ComposicaoNotaPeriodo" +
                        "WHERE ComposicaoNotaPeriodo.Id IN(" + strId + ")";

            retorno = this.Conexao.Delete(sql);
            return retorno;
        }

         /// <summary>
        /// Insere um registro em Nota através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.FormaDeAvaliacao</param>
        /// <returns></returns>
        protected override int Inserir(Prion.Generic.Models.Base objeto)
        {
            Models.ComposicaoNotaPeriodo oComposicaoNota = (Models.ComposicaoNotaPeriodo)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO ComposicaoNotaPeriodo(IdProfessorDisciplina, PeriodoDeAvaliacao, FormaDivisaoAnoLetivo) " +
                            "VALUES (@IdProfessorDisciplina, @PeriodoDeAvaliacao, @FormaDivisaoAnoLetivo)";

            parametros.Add(this.Conexao.CriarParametro("@IdProfessorDisciplina", DbType.Int32, oComposicaoNota.IdProfessorDisciplina));
            parametros.Add(this.Conexao.CriarParametro("@PeriodoDeAvaliacao", DbType.Int32, oComposicaoNota.PeriodoDeAvaliacao));
            parametros.Add(this.Conexao.CriarParametro("@FormaDivisaoAnoLetivo", DbType.String, oComposicaoNota.FormaDivisaoAnoLetivo));

            Int32 id = this.Conexao.Insert(sql, parametros);
            GravarLog(id, TipoOperacaoLog.Inserir);

            return id;
        }

         /// <summary>
        /// Altera um registro de Nota através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.ComposicaoNota</param>
        /// <returns></returns>
        protected override Int32 Alterar(Prion.Generic.Models.Base objeto)
        {
            Models.ComposicaoNotaPeriodo oComposicaoNota = (Models.ComposicaoNotaPeriodo)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE ComposicaoNotaPeriodo SET IdProfessorDisciplina=@IdProfessorDisciplina, PeriodoDeAvaliacao=@PeriodoDeAvaliacao, FormaDivisaoAnoLetivo=@FormaDivisaoAnoLetivo " +
                            " WHERE Id =@Id ";

            parametros.Add(this.Conexao.CriarParametro("@IdProfessorDisciplina", DbType.Int32, oComposicaoNota.IdProfessorDisciplina));
            parametros.Add(this.Conexao.CriarParametro("@PeriodoDeAvaliacao", DbType.Int32, oComposicaoNota.PeriodoDeAvaliacao));
            parametros.Add(this.Conexao.CriarParametro("@FormaDivisaoAnoLetivo", DbType.String, oComposicaoNota.FormaDivisaoAnoLetivo)); 
            parametros.Add(this.Conexao.CriarParametro("@Id",DbType.Int32,oComposicaoNota.Id));

            Int32 id = this.Conexao.Update(sql, parametros);
            GravarLog(id, TipoOperacaoLog.Alterar);

            return id;
        }

        /// <summary>
        /// Grava o log de ComposicaoNota
        /// </summary>
        /// <param name="idRegistro">id do registro</param>
        /// <param name="tipoOperacaoLog">tipo de operação: INSERIR, ALTERAR, EXCLUIR</param>
        /// <returns>true/false </returns>
        protected Boolean GravarLog(Int32 idRegistro, TipoOperacaoLog tipoOperacaoLog)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sqlLog = " if EXISTS (SELECT Id FROM ComposicaoNotaPeriodo WHERE Id = @Id) " +
                            " INSERT INTO ComposicaoNotaPeriodoLog(IdUsuarioLog, IdOperacaoLog, Id, IdProfessorDisciplina, PeriodoDeAvaliacao, FormaDivisaoAnoLetivo, DataCadastro)" +
                            " SELECT @IdUsuarioLog, @IdOperacaoLog, @Id, IdProfessorDisciplina, PeriodoDeAvaliacao, FormaDivisaoAnoLetivo, DataCadastro FROM ComposicaoNotaPeriodo WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "ComposicaoNotaPeriodoLog");
        }

        /// <summary>
        /// Carrega as demais entidades relacionadas a este objeto
        /// </summary>
        /// <param name="modulos">lista de módulos que serão utilizados para carregar as demais entidades</param>
        /// <param name="objComposicaoNota">Objeto do tipo Models.Base que será convertido em Models.ComposicaoNota</param>
        protected Models.ComposicaoNotaPeriodo CarregarEntidades(Prion.Tools.Entidade modulos, Prion.Generic.Models.Base objComposicaoNota)
        {
            Boolean modoCompleto = Entidades.Carregar(Helpers.Entidade.Tudo.ToString());
            Models.ComposicaoNotaPeriodo composicaoNota = (Models.ComposicaoNotaPeriodo)objComposicaoNota;


            // verifica se possui o módulo COMPLETO ou se possui o módulo CORPORACAO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Nota.ToString())))
            {
                Repository.ComposicaoNota repNota = new Repository.ComposicaoNota(ref this._conexao);
                repNota.Entidades = modulos;

                // carrega um objeto do tipo Models.Corporacao
                composicaoNota.ListaNotas = Prion.Tools.ListTo.CollectionToList<Models.ComposicaoNota>(repNota.Buscar(composicaoNota.Id).ListaObjetos);
            }

            

            // retorna o objeto de Aluno com as entidades que foram carregadas
            return composicaoNota;
        }


        /// <summary>
        /// Salva todos os relacionamentos de composicaoNota
        /// </summary>
        /// <param name="objeto">Objeto base que será convertido para um objeto do tipo Models.ComposicaoNota</param>
        /// <param name="idComposicaoNota">id do último registro inserido</param>
        protected override Prion.Generic.Helpers.Retorno SalvarEntidade(Prion.Generic.Models.Base objeto, Int32 idComposicaoNota)
        {
            Models.ComposicaoNotaPeriodo composicaoNota = (Models.ComposicaoNotaPeriodo)objeto;
            Repository.ComposicaoNota repNota = new Repository.ComposicaoNota(ref this._conexao);

            // se o objeto de aluno estiver no estado 'NOVO', 
            // chama o método que irá definir o seu id (idAluno) em todos os objetos da lista de responsáveis
            if (composicaoNota.EstadoObjeto == Prion.Generic.Helpers.Enums.EstadoObjeto.Novo)
            {
                AtualizarIdComposicaoNota(composicaoNota, idComposicaoNota);
            }
            else
            {
                AtualizarEstadoNota(composicaoNota);
            }

            Prion.Generic.Helpers.Retorno retorno = repNota.Salvar(composicaoNota.ListaNotas);

            return retorno;
        }

        /// <summary>
        /// Atualiza o IdComposicaoNota de todas os registros de nota
        /// </summary>
        /// <param name="composicaoNota"></param>
        /// <param name="idAluno"></param>
        private void AtualizarIdComposicaoNota(Models.ComposicaoNotaPeriodo composicaoNota, Int32 idComposicaoNota)
        {
            if (composicaoNota == null)
            {
                return;
            }

            for (Int32 i = 0; i < composicaoNota.ListaNotas.Count; i++)
            {
                composicaoNota.ListaNotas[i].IdComposicaoNotaPeriodo = idComposicaoNota;
            }
        }

        /// <summary>
        /// Atualiza o estado de todas os registros de nota
        /// </summary>
        /// <param name="composicaoNota"></param>        
        private void AtualizarEstadoNota(Models.ComposicaoNotaPeriodo composicaoNota)
        {
            if (composicaoNota == null)
            {
                return;
            }

            for (Int32 i = 0; i < composicaoNota.ListaNotas.Count; i++)
            {
                if (composicaoNota.ListaNotas[i].EstadoObjeto == Prion.Generic.Helpers.Enums.EstadoObjeto.Excluido)
                {
                    continue;
                }
                composicaoNota.ListaNotas[i].EstadoObjeto = (composicaoNota.ListaNotas[i].Id == 0) ? Prion.Generic.Helpers.Enums.EstadoObjeto.Novo : Prion.Generic.Helpers.Enums.EstadoObjeto.Alterado;
            }
        }
    }
}
   