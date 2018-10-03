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
    public class ComposicaoNota:Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public ComposicaoNota(ref DBFacade conexao)
            : base(ref conexao)
        { 
        }

        ~ComposicaoNota()
        { 
        }

        /// <summary>
        /// Busca todas as Nota da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "ComposicaoNota.* ";
            String join = "FROM ComposicaoNota ";
            String whereDefault = "";
            String orderBy = "ComposicaoNota.Id";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy, groupBy, parametro);
            List<Models.ComposicaoNota> listaNota = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaNota = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaNota != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaNota);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }

        /// <summary>
        /// Retorna, através de um ID de ComposicaoNota, uma lista de objetos do tipo Models.Nota
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Prion.Generic.Models.Lista Buscar(Int32 id)
        {
            Prion.Tools.Request.ParametrosRequest parametro = new Prion.Tools.Request.ParametrosRequest();
            Prion.Tools.Request.Filtro f = new Request.Filtro("IdComposicaoNotaPeriodo", "=", id);

            parametro.Filtro.Add(f);

            return Buscar(parametro);
        }

        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de Nota
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.Nota com os registros do DataTable</returns>
        public List<Models.ComposicaoNota> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }


            List<Models.ComposicaoNota> lista = new List<Models.ComposicaoNota>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.Aluno
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Models.ComposicaoNota nota = new Models.ComposicaoNota();

                nota.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                nota.IdComposicaoNotaPeriodo = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdComposicaoNotaPeriodo"].ToString());
                nota.Peso = Conversor.ToDecimal(dataTable.Rows[i][nomeBase + "Peso"].ToString());                 
                nota.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                nota.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(nota);
                    continue;
                }

                // carrega as demais entidades necessárias
                nota = CarregarEntidades(Entidades, nota);

                lista.Add(nota);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }

        /// <summary>
        /// Exclui um registro de Nota através do Id
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

            String sql = "DELETE FROM ComposicaoNota " +
                         " WHERE Nota.Id IN(" + strId + ")";

            retorno = this.Conexao.Delete(sql);
            return retorno;
        }

        /// <summary>
        /// Exclui um registro de Nota através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Nota</param>
        /// <returns>Quantidade de registros excluídos</returns>
        protected override Int32 Excluir(Prion.Generic.Models.Base objeto)
        {
            Models.ComposicaoNota oNota = (Models.ComposicaoNota)objeto;
            return Excluir(oNota.Id);
        }

         /// <summary>
        /// Insere um registro em Nota através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.FormaDeAvaliacao</param>
        /// <returns></returns>
        protected override int Inserir(Prion.Generic.Models.Base objeto)
        {
            Models.ComposicaoNota oNota = (Models.ComposicaoNota)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO ComposicaoNota(IdComposicaoNotaPeriodo, Peso) " +
                            "VALUES (@IdComposicaoNotaPeriodo, @Peso)";

            parametros.Add(this.Conexao.CriarParametro("@IdComposicaoNotaPeriodo", DbType.Int32, oNota.IdComposicaoNotaPeriodo));
            parametros.Add(this.Conexao.CriarParametro("@Peso", DbType.String, oNota.Peso));                  

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
            Models.ComposicaoNota oNota = (Models.ComposicaoNota)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE ComposicaoNota SET IdComposicaoNotaPeriodo=@IdComposicaoNotaPeriodo, Peso=@Peso " +
                            " WHERE Id =@Id ";

            parametros.Add(this.Conexao.CriarParametro("@IdComposicaoNotaPeriodo", DbType.Int32, oNota.IdComposicaoNotaPeriodo));
            parametros.Add(this.Conexao.CriarParametro("@Peso", DbType.String, oNota.Peso));                 
            parametros.Add(this.Conexao.CriarParametro("@Id",DbType.Int32,oNota.Id));

            Int32 id = this.Conexao.Update(sql, parametros);
            GravarLog(id, TipoOperacaoLog.Alterar);

            return id;
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
            String sqlLog = " if EXISTS (SELECT Id FROM ComposicaoNota WHERE Id = @Id) " +
                            " INSERT INTO ComposicaoNotaLog(IdUsuarioLog, IdOperacaoLog, Id, IdComposicaoNotaPeriodo, Peso, DataCadastro)" +
                            " SELECT @IdUsuarioLog, @IdOperacaoLog, @Id,IdComposicaoNotaPeriodo, Peso, DataCadastro FROM ComposicaoNota WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "ComposicaoNotaLog");
        }


        /// <summary>
        /// Carrega as demais entidades relacionadas a este objeto
        /// </summary>
        /// <param name="modulos">lista de módulos que serão utilizados para carregar as demais entidades</param>
        /// <param name="objNota">Objeto do tipo Models.Base que será convertido em Models.Nota</param>
        protected Models.ComposicaoNota CarregarEntidades(Prion.Tools.Entidade modulos, Prion.Generic.Models.Base objNota)
        {
            Boolean modoCompleto = Entidades.Carregar(Helpers.Entidade.Tudo.ToString());
            Models.ComposicaoNota nota = (Models.ComposicaoNota)objNota;


            // verifica se possui o módulo COMPLETO ou se possui o módulo CORPORACAO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Nota.ToString())))
            {
                Repository.FormaDeAvaliacao repFormaDeAvaliacao = new Repository.FormaDeAvaliacao(ref this._conexao);
                repFormaDeAvaliacao.Entidades = modulos;

                // carrega um objeto do tipo Models.Corporacao
                nota.ListaFormasDeAvaliacao = Prion.Tools.ListTo.CollectionToList<Models.FormaDeAvaliacao>(repFormaDeAvaliacao.Buscar(nota.Id).ListaObjetos);
            }


            // retorna o objeto de Nota com as entidades que foram carregadas
            return nota;
        }


        /// <summary>
        /// Salva todos os relacionamentos de nota
        /// </summary>
        /// <param name="objeto">Objeto base que será convertido para um objeto do tipo Models.Nota</param>
        /// <param name="idNota">id do último registro inserido</param>
        protected override Prion.Generic.Helpers.Retorno SalvarEntidade(Prion.Generic.Models.Base objeto, Int32 idNota)
        {
            Models.ComposicaoNota nota = (Models.ComposicaoNota)objeto;
            Repository.FormaDeAvaliacao repFormaDeAvaliacao = new Repository.FormaDeAvaliacao(ref this._conexao);

            // se o objeto de aluno estiver no estado 'NOVO', 
            // chama o método que irá definir o seu id (idAluno) em todos os objetos da lista de responsáveis
            if (nota.EstadoObjeto == Prion.Generic.Helpers.Enums.EstadoObjeto.Novo)
            {
                AtualizarIdNota(nota, idNota);
            }

            else if (nota.EstadoObjeto == Prion.Generic.Helpers.Enums.EstadoObjeto.Excluido)
            {
                for (Int32 i = 0; i < nota.ListaFormasDeAvaliacao.Count; i++)
                {
                    nota.ListaFormasDeAvaliacao[i].EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Excluido;
                }
            }

            else
            {
                AtualizarEstadoNota(nota);
            }

            Prion.Generic.Helpers.Retorno retorno = repFormaDeAvaliacao.Salvar(nota.ListaFormasDeAvaliacao);

            return retorno;
        }

        /// <summary>
        /// Atualiza o IdNota de todas os registros de nota
        /// </summary>
        /// <param name="nota"></param>
        /// <param name="idNota"></param>
        private void AtualizarIdNota(Models.ComposicaoNota nota, Int32 idNota)
        {
            if (nota == null)
            {
                return;
            }

            for (Int32 i = 0; i < nota.ListaFormasDeAvaliacao.Count; i++)
            {
                nota.ListaFormasDeAvaliacao[i].IdComposicaoNota = idNota;
            }
        }

        /// <summary>
        /// Atualiza o estado de todas os registros de nota
        /// </summary>
        /// <param name="nota"></param>        
        private void AtualizarEstadoNota(Models.ComposicaoNota nota)
        {
            if (nota == null)
            {
                return;
            }

            for (Int32 i = 0; i < nota.ListaFormasDeAvaliacao.Count; i++)
            {
                nota.ListaFormasDeAvaliacao[i].EstadoObjeto = (nota.ListaFormasDeAvaliacao[i].Id == 0) ? Prion.Generic.Helpers.Enums.EstadoObjeto.Novo : Prion.Generic.Helpers.Enums.EstadoObjeto.Alterado;
            }
        }
    }
}