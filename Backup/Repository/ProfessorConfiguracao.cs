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
    public class ProfessorConfiguracao:Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public ProfessorConfiguracao(ref DBFacade conexao)
            : base(ref conexao)
        { 
        }

        ~ProfessorConfiguracao()
        { 
        }

        /// <summary>
        /// Busca todas as ProfessorConfiguracao da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "ProfessorConfiguracao.* ";
            String join = "FROM ProfessorConfiguracao ";
            String whereDefault = "";
            String orderBy = "ProfessorConfiguracao.Id";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy, groupBy, parametro);
            List<Models.ProfessorConfiguracao> listaProfessorConfiguracao = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaProfessorConfiguracao = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaProfessorConfiguracao != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaProfessorConfiguracao);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }


        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de Nota
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Models.ProfessorConfiguracao com os registros do DataTable</returns>
        public List<Models.ProfessorConfiguracao> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }


            List<Models.ProfessorConfiguracao> lista = new List<Models.ProfessorConfiguracao>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.Aluno
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Models.ProfessorConfiguracao professorConfiguracao = new Models.ProfessorConfiguracao();

                professorConfiguracao.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                professorConfiguracao.IdUsuario = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdUsuario"].ToString());
                professorConfiguracao.TipoMedia = dataTable.Rows[i][nomeBase + "TipoMedia"].ToString();
                professorConfiguracao.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());                

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(professorConfiguracao);
                    continue;
                }

                // carrega as demais entidades necessárias
                professorConfiguracao = CarregarEntidades(Entidades, professorConfiguracao);

                lista.Add(professorConfiguracao);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }

        /// <summary>
        /// Insere um registro em ProfessorConfiguracao através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.ProfessorConfiguracao</param>
        /// <returns></returns>
        protected override int Inserir(Prion.Generic.Models.Base objeto)
        {
            Models.ProfessorConfiguracao oProfessorConfiguracao = (Models.ProfessorConfiguracao)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO ProfessorConfiguracao(IdUsuario, TipoMedia) " +
                            "VALUES (@IdUsuario, @TipoMedia)";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuario", DbType.Int32, oProfessorConfiguracao.IdUsuario));            
            parametros.Add(this.Conexao.CriarParametro("@TipoMedia", DbType.String, oProfessorConfiguracao.TipoMedia));

            Int32 id = this.Conexao.Insert(sql, parametros);          

            return id;
        }

        /// <summary>
        /// Altera um registro de Nota através do objeto
        /// </summary>
        /// <param name="objeto">Objeto do tipo Models.Base que será convertido em Models.ComposicaoNota</param>
        /// <returns></returns>
        protected override Int32 Alterar(Prion.Generic.Models.Base objeto)
        {
            Models.ProfessorConfiguracao oProfessorConfiguracao = (Models.ProfessorConfiguracao)objeto;
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "UPDATE ProfessorConfiguracao SET IdUsuario=@IdUsuario, TipoMedia=@TipoMedia " +
                            " WHERE Id =@Id ";

            parametros.Add(this.Conexao.CriarParametro("@IdUsuario", DbType.Int32, oProfessorConfiguracao.IdUsuario));            
            parametros.Add(this.Conexao.CriarParametro("@TipoMedia", DbType.String, oProfessorConfiguracao.TipoMedia));
            parametros.Add(this.Conexao.CriarParametro("@Id", DbType.Int32, oProfessorConfiguracao.Id));

            Int32 id = this.Conexao.Update(sql, parametros);    

            return id;
        }

        /// <summary>
        /// Carrega as demais entidades relacionadas a este objeto
        /// </summary>
        /// <param name="modulos">lista de módulos que serão utilizados para carregar as demais entidades</param>
        /// <param name="objProfessorConfiguracao">Objeto do tipo Models.Base que será convertido em Models.ProfessorConfiguracao</param>
        protected Models.ProfessorConfiguracao CarregarEntidades(Prion.Tools.Entidade modulos, Prion.Generic.Models.Base objProfessorConfiguracao)
        {
            Boolean modoCompleto = Entidades.Carregar(Helpers.Entidade.Tudo.ToString());
            Models.ProfessorConfiguracao professorConfiguracao = (Models.ProfessorConfiguracao)objProfessorConfiguracao;


            // verifica se possui o módulo COMPLETO ou se possui o módulo CORPORACAO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Usuario.ToString())))
            {
                Prion.Generic.Repository.Usuario repUsuario = new Prion.Generic.Repository.Usuario(ref this._conexao);
                repUsuario.Entidades = modulos;

                // carrega um objeto do tipo Models.Corporacao
                professorConfiguracao.Usuario =(Models.Usuario)repUsuario.BuscarPeloId(professorConfiguracao.IdUsuario).Get(0);
            }



            // retorna o objeto de Aluno com as entidades que foram carregadas
            return professorConfiguracao;
        }
    }
}