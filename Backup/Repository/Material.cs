using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Prion.Data;
using Prion.Tools;
using System.Text;

namespace NotaAzul.Repository
{
    public class Material : Prion.Generic.Repository.Base
    {
         /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public Material(ref DBFacade conexao)
            : base(ref conexao)
        { 
        }


        ~Material()
        { 
        }


        /// <summary>
        /// Busca todas as Material da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "Material.* ";
            String join = "FROM Material";
            String whereDefault = "";
            String orderBy = "Material.Nome";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy,groupBy, parametro);
            List<Models.Material> listaMaterial = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaMaterial = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaMaterial != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaMaterial);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }


        /// <summary>
        /// Retorna, através de um (ou vários) ID, uma lista de objetos do tipo Models.Material
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
        /// Retorna um objeto do tipo Lista contendo um datatable e o total de registros sem WHERE
        /// </summary>
        /// <returns>DataTable</returns>
        public Prion.Generic.Models.Lista DataTable(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            StringBuilder campos = new StringBuilder(), join = new StringBuilder();
            campos.Append("Material.* ");
            join.Append(" FROM Material");

            if (Entidades.Carregar(Helpers.Entidade.Situacao.ToString()))
            {
                campos.Append(", " + Prion.Generic.Helpers.MapeamentoTabela.Situacao());
                join.Append(" INNER JOIN Situacao ON Material.IdSituacao = Situacao.Id ");
            }

            String whereDefault = "";
            String orderBy = "Material.Nome";
            String groupBy = "";

            return this.Select("SELECT " + campos.ToString(), join.ToString(), whereDefault, orderBy,groupBy, parametro);
        }
        

        /// <summary>
        /// Insere um registro em Material através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Material</param>
        /// <returns></returns>
        protected override Int32 Inserir(Prion.Generic.Models.Base objeto)
        {
            Models.Material oMaterial = (Models.Material) objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO Material(IdCorporacao, IdSituacao, Nome) VALUES(@IdCorporacao, @IdSituacao, @Nome)";

            parametros.Add(this.Conexao.CriarParametro("@IdCorporacao", DbType.String, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, oMaterial.IdSituacao));
            parametros.Add(this.Conexao.CriarParametro("@Nome", DbType.String, StringHelper.OnlyOneSpace(oMaterial.Nome), 255));
            
            Int32 id = this.Conexao.Insert(sql, parametros);

            GravarLog(id, TipoOperacaoLog.Inserir);

            return id;
        }


        /// <summary>
        /// Altera um registro de Material através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Material</param>
        /// <returns></returns>
        protected override Int32 Alterar(Prion.Generic.Models.Base objeto)
        {
            Models.Material oMaterial = (Models.Material) objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE Material SET IdSituacao=@IdSituacao, Nome=@Nome WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, oMaterial.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdSituacao", DbType.Int32, oMaterial.IdSituacao));
            parametros.Add(this.Conexao.CriarParametro("@Nome", DbType.String, StringHelper.OnlyOneSpace(oMaterial.Nome), 255));
                       
            Int32 linhasAfetadas = this.Conexao.Update(sql, parametros);

            GravarLog(oMaterial.Id, TipoOperacaoLog.Alterar);

            return linhasAfetadas;
        }


        /// <summary>
        /// Exclui um registro de Material através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.Material</param>
        /// <returns>Quantidade de registros excluídos</returns>
        protected override Int32 Excluir(Prion.Generic.Models.Base objeto)
        {
            Models.Material oMaterial = (Models.Material)objeto;
            return Excluir(oMaterial.Id);
        }


        /// <summary>
        /// Exclui um registro de Material através do Id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>Quantidade de registros excluídos</returns>
        public Int32 Excluir(params Int32[] ids)
        {
            // se o parâmetro for null, retorna 0, informando que não excluiu nenhum registro
            if (ids == null) { return 0; }

            for (Int32 indice=0; indice<ids.Length; indice++)
            {
                GravarLog(ids[indice], TipoOperacaoLog.Excluir);
            }

            Int32 retorno = -1;
            String strId = Conversor.ToString(",", ids);
            String sql = "DELETE FROM Material WHERE Id IN("+ strId +")";

            retorno = this.Conexao.Delete(sql);
            return retorno;
        }


        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de Material
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.Material com os registros do DataTable</returns>
        public List<Models.Material> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable==null || dataTable.Rows.Count == 0) { return null; }


            List<Models.Material> lista = new List<Models.Material>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.Material
            for (int i=0; i<dataTable.Rows.Count; i++)
            {
                Models.Material material = new Models.Material();

                material.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                material.IdCorporacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdCorporacao"].ToString());
                material.IdSituacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdSituacao"].ToString());
                material.Nome = dataTable.Rows[i][nomeBase + "Nome"].ToString();
                material.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                material.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(material);
                    continue;
                }

                // carrega as demais entidades necessárias
                material = CarregarEntidades(Entidades, material);

                lista.Add(material);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista; 
        }


        /// <summary>
        /// Carrega as demais entidades relacionadas a este objeto
        /// </summary>
        /// <param name="modulos">lista de módulos que serão utilizados para carregar as demais entidades</param>
        /// <param name="objTurno">Objeto do tipo Models.Base que será convertido em Models.Material</param>
        protected Models.Material CarregarEntidades(Prion.Tools.Entidade modulos, Prion.Generic.Models.Base objMaterial)
        {
            Boolean modoCompleto = Entidades.Carregar(Helpers.Entidade.Tudo.ToString());
            Models.Material material = (Models.Material)objMaterial;


            // verifica se possui o módulo COMPLETO ou se possui o módulo CORPORACAO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Corporacao.ToString())))
            {
                Prion.Generic.Repository.Corporacao repCorporacao = new Prion.Generic.Repository.Corporacao(ref this._conexao);
                repCorporacao.Entidades = modulos;

                // carrega um objeto do tipo Prion.Generic.Models.Corporacao
                material.Corporacao = repCorporacao.BuscarPeloId(material.IdCorporacao);
            }

            // verifica se possui o módulo COMPLETO ou se possui o módulo SITUACAO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Situacao.ToString())))
            {
                Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this._conexao);
                repSituacao.Entidades = modulos;

                // carrega um objeto do tipo Prion.Generic.Models.Situacao.
                material.Situacao = (Prion.Generic.Models.Situacao)repSituacao.BuscarPeloId(material.IdSituacao).Get(0);
            }

            // retorna o objeto de Material com as entidades que foram carregadas
            return material;
        }


        /// <summary>
        /// Grava o log de Material
        /// </summary>
        /// <param name="idRegistro">id do registro</param>
        /// <param name="tipoOperacaoLog">tipo de operação: INSERIR, ALTERAR, EXCLUIR</param>
        /// <returns>true/false </returns>
        protected Boolean GravarLog(Int32 idRegistro, TipoOperacaoLog tipoOperacaoLog)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sqlLog = " if EXISTS (SELECT Id FROM Material WHERE Id = @Id) " +
                            " INSERT INTO MaterialLog(IdUsuarioLog, IdOperacaoLog, Id, IdCorporacao, IdSituacao, Nome, DataCadastro)" +
                            " SELECT @IdUsuarioLog, @IdOperacaoLog, @Id, IdCorporacao, IdSituacao, Nome, DataCadastro FROM Material WHERE Id = @Id";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuarioLog", DbType.Int32, UsuarioLogado.Id));
            parametros.Add(this.Conexao.CriarParametro("@IdOperacaoLog", DbType.Int32, (Int32)tipoOperacaoLog));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, idRegistro));

            return Log.GravarLog(sqlLog, parametros, "MaterialLog");
        }
    }
}