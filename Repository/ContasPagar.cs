using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Prion.Data;
using Prion.Tools;
using System.Text;

namespace NotaAzul.Repository
{
    public class ContasPagar : Prion.Generic.Repository.Base
    {
         /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public ContasPagar(ref DBFacade conexao)
            : base(ref conexao)
        { 
        }


        ~ContasPagar()
        {
        }


        /// <summary>
        /// Busca todos os títulos pagos da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "Titulo.* ";
            String join = " FROM Titulo INNER JOIN MovimentacaoFinanceiraTitulo ON Titulo.Id = MovimentacaoFinanceiraTitulo.IdTitulo ";
            String whereDefault = "";
            String orderBy = "Titulo.Id";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy,groupBy, parametro);
            List<Prion.Generic.Models.Titulo> listaTitulos = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaTitulos = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaTitulos != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaTitulos);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }


        /// <summary>
        /// Retorna, através de um (ou vários) ID, uma lista de objetos do tipo Prion.Generic.Models.Titulo
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
            campos.Append("Titulo.* ");
            join.Append(" FROM Titulo INNER JOIN MovimentacaoFinanceiraTitulo ON Titulo.Id = MovimentacaoFinanceiraTitulo.IdTitulo ");

            Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this._conexao);
            Prion.Generic.Models.Situacao situacaoQuitada = (Prion.Generic.Models.Situacao)repSituacao.BuscarPelaSituacao("Título", "Quitado");
            Prion.Generic.Models.Situacao situacaoCancelada = (Prion.Generic.Models.Situacao)repSituacao.BuscarPelaSituacao("Genérico", "Cancelado");


            String whereDefault = " WHERE Titulo.Tipo = 'D' AND Titulo.IdSituacao = " + situacaoQuitada.Id.ToString() + " AND  MovimentacaoFinanceiraTitulo.IdSituacao != " + situacaoCancelada.Id;
            String where = this.MontarWhere(parametro, whereDefault);
            String orderBy = " Titulo.Id";
            String groupBy = "";

            return this.Select("SELECT " + campos.ToString(), join.ToString(), where, orderBy,groupBy, parametro);
        }

        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de Titulos que já foram pagos
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipoPrion.Generic.Models.Titulo com os registros do DataTable</returns>
        public List<Prion.Generic.Models.Titulo> DataTableToObject(DataTable dataTable, String nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }


            List<Prion.Generic.Models.Titulo > lista = new List<Prion.Generic.Models.Titulo>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Prion.Generic.Models.Titulo
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Prion.Generic.Models.Titulo titulo = new Prion.Generic.Models.Titulo();

                titulo.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                titulo.IdCorporacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdCorporacao"].ToString());
                titulo.IdTituloTipo = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdTituloTipo"].ToString());
                titulo.Descricao = dataTable.Rows[i][nomeBase + "Descricao"].ToString();
                titulo.Observacao = dataTable.Rows[i][nomeBase + "Observacao"].ToString();
                titulo.Numero = dataTable.Rows[i][nomeBase + "Numero"].ToString();
                titulo.Valor = Conversor.ToDecimal(dataTable.Rows[i][nomeBase + "Valor"].ToString());
                titulo.Desconto = Conversor.ToDecimal(dataTable.Rows[i][nomeBase + "Desconto"].ToString());
                titulo.DataVencimento = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataVencimento"].ToString());
                titulo.DataOperacao = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataOperacao"].ToString());
                titulo.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                titulo.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(titulo);
                    continue;
                }

                lista.Add(titulo);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }


        /// <summary>
        /// Carrega as demais entidades relacionadas a este objeto
        /// </summary>
        /// <param name="modulos">lista de módulos que serão utilizados para carregar as demais entidades</param>
        /// <param name="objTitulo">Objeto do tipo Models.Base que será convertido emPrion.Generic.Models.Titulo</param>
        protected Prion.Generic.Models.Titulo CarregarEntidades(Prion.Tools.Entidade modulos, Prion.Generic.Models.Base objTitulo)
        {
            Boolean modoCompleto = Entidades.Carregar(Helpers.Entidade.Tudo.ToString());
            Prion.Generic.Models.Titulo titulo = (Prion.Generic.Models.Titulo)objTitulo;


            // verifica se possui o módulo COMPLETO ou se possui o módulo CORPORACAO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Corporacao.ToString())))
            {
                Prion.Generic.Repository.Corporacao repCorporacao = new Prion.Generic.Repository.Corporacao(ref this._conexao);
                repCorporacao.Entidades = modulos;

                // carrega um objeto do tipo Models.Corporacao
                titulo.Corporacao = (Prion.Generic.Models.Corporacao)repCorporacao.BuscarPeloId(titulo.IdCorporacao);
            }

            // verifica se possui o módulo COMPLETO ou se possui o módulo Situação
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Situacao.ToString())))
            {
                Prion.Generic.Repository.Situacao repSituacao = new Prion.Generic.Repository.Situacao(ref this._conexao);
                repSituacao.Entidades = modulos;

                // carrega um objeto do tipo Models.Situacao
                titulo.Situacao = (Prion.Generic.Models.Situacao)repSituacao.BuscarPeloId(titulo.IdSituacao).Get(0);
            }

            // verifica se possui o módulo COMPLETO ou se possui o módulo CORPORACAO
            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.TipoTitulo.ToString())))
            {
                Prion.Generic.Repository.TituloTipo repTituloTipo = new Prion.Generic.Repository.TituloTipo(ref this._conexao);
                repTituloTipo.Entidades = modulos;

                // carrega um objeto do tipo Models.Corporacao
                titulo.TituloTipo = (Prion.Generic.Models.TituloTipo)repTituloTipo.BuscarPeloId(titulo.IdTituloTipo).Get(0);
            }


            // retorna o objeto de Receita com as entidades que foram carregadas
            return titulo;
        }

       
    }
}