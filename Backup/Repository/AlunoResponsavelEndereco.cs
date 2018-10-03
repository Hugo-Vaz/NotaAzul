using System;
using System.Collections.Generic;
using Prion.Data;
using Prion.Tools;
using System.Data;

namespace NotaAzul.Repository
{
    public class AlunoResponsavelEndereco : Prion.Generic.Repository.Base
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="Db">Uma instância de DBFacade</param>
        public AlunoResponsavelEndereco(ref DBFacade conexao)
            : base(ref conexao)
        { 
        }


        ~AlunoResponsavelEndereco()
        { 
        }


        /// <summary>
        /// Busca todas as AlunoResponsavelEndereco da base de dados.
        /// </summary>
        /// <returns>Retorna uma lista de objetos do tipo Models.Base</returns>
        public Prion.Generic.Models.Lista Buscar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            // IMPORTANTE quando houver um JOIN, colocar o nome da tabela junto do nome do campo
            String campos = "Endereco.* ";
            String join = "FROM AlunoResponsavelEndereco " +
                            "INNER JOIN Endereco ON Endereco.Id = AlunoResponsavelEndereco.IdEndereco ";
            String whereDefault = "";
            String orderBy = "Endereco.DataCadastro";
            String groupBy = "";

            Prion.Generic.Models.Lista lista = this.Select("SELECT " + campos, join, whereDefault, orderBy,groupBy, parametro);

            Prion.Generic.Repository.Endereco repEndereco = new Prion.Generic.Repository.Endereco(ref this._conexao);
            List<Prion.Generic.Models.Endereco> listaEndereco = null;
            List<Prion.Generic.Models.Base> listaBase = null;

            listaEndereco = this.DataTableToObject(lista.DataTable);

            // se obteve algum registro, faz a conversão da lista de objetos
            if (listaEndereco != null)
            {
                listaBase = ListTo.CollectionToList<Prion.Generic.Models.Base>(listaEndereco);
            }

            Prion.Generic.Models.Lista listaObj = new Prion.Generic.Models.Lista(listaBase, lista.Count);
            return listaObj;
        }


        /// <summary>
        /// Retorna, através de um (ou vários) ID, uma lista de objetos do tipo Models.Endereco
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Prion.Generic.Models.Lista Buscar(params Int32[] idsAlunoResponsavel)
        {
            Prion.Tools.Request.ParametrosRequest parametro = new Prion.Tools.Request.ParametrosRequest();
            Prion.Tools.Request.Filtro f = new Request.Filtro("IdAlunoResponsavel", "IN", Conversor.ToString(",", idsAlunoResponsavel));

            parametro.Filtro.Add(f);

            return Buscar(parametro);
        }


        /// <summary>
        /// Carrega as demais entidades relacionadas a este objeto
        /// </summary>
        /// <param name="modulos">lista de módulos que serão utilizados para carregar as demais entidades</param>
        /// <param name="objAlunoResponsaelEndereco">Objeto do tipo Models.Base que será convertido em Prion.Generic.Models.Endereco</param>
        protected Prion.Generic.Models.Endereco CarregarEntidades(Prion.Tools.Entidade modulos, Prion.Generic.Models.Base objAlunoResponsaelEndereco)
        {
            Boolean modoCompleto = Entidades.Carregar(Helpers.Entidade.Tudo.ToString());
            Prion.Generic.Models.Endereco endereco = (Prion.Generic.Models.Endereco)objAlunoResponsaelEndereco;

            if ((modoCompleto) || (Entidades.Carregar(Helpers.Entidade.Endereco.ToString())))
            {
                Prion.Generic.Repository.Estado repEstado =new  Prion.Generic.Repository.Estado(ref this._conexao);
                endereco.Estado =(Prion.Generic.Models.Estado)repEstado.BuscarPeloId(endereco.IdEstado).Get(0);

                Prion.Generic.Repository.Cidade repCidade = new Prion.Generic.Repository.Cidade(ref this._conexao);
                endereco.Cidade = (Prion.Generic.Models.Cidade)repCidade.BuscarPeloId(endereco.IdCidade).Get(0);

                Prion.Generic.Repository.Bairro repBairro = new Prion.Generic.Repository.Bairro(ref this._conexao);
                endereco.Bairro = (Prion.Generic.Models.Bairro)repBairro.BuscarPeloId(endereco.IdBairro).Get(0);
            }

            return endereco;
        }


        /// <summary>
        /// Recebe um DataTable como parâmetro e carrega uma lista de Enderecos
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>lista de objetos do tipo Prion.Generic.Models.Endereco com os registros do DataTable</returns>
        public List<Prion.Generic.Models.Endereco> DataTableToObject(DataTable dataTable, string nomeBase = "")
        {
            // verifica se o parâmetro 'dataTable' é igual a NULL. Se for, sai sem fazer nada
            if (dataTable == null || dataTable.Rows.Count == 0) { return null; }


            List<Prion.Generic.Models.Endereco> lista = new List<Prion.Generic.Models.Endereco>();
            Boolean modoBasico = Entidades.Carregar(Helpers.Entidade.Basico.ToString());

            // varre o dataReader criando, a cada iteração, um objeto do tipo Models.AlunoResponsavel
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Prion.Generic.Models.Endereco endereco = new Prion.Generic.Models.Endereco();

                endereco.Id = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "Id"].ToString());
                endereco.IdSituacao = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdSituacao"].ToString());
                endereco.IdBairro = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdBairro"].ToString());
                endereco.IdCidade = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdCidade"].ToString());
                endereco.IdEstado = Conversor.ToInt32(dataTable.Rows[i][nomeBase + "IdEstado"].ToString());
                endereco.Apelido = dataTable.Rows[i][nomeBase + "Apelido"].ToString();
                endereco.Cep = dataTable.Rows[i][nomeBase + "Cep"].ToString();
                endereco.Complemento = dataTable.Rows[i][nomeBase + "Complemento"].ToString();
                endereco.DadosEndereco = dataTable.Rows[i][nomeBase + "Endereco"].ToString();
                endereco.Numero = dataTable.Rows[i][nomeBase + "Numero"].ToString();
                endereco.Referencia = dataTable.Rows[i][nomeBase + "Referencia"].ToString();                
                endereco.DataCadastro = Conversor.ToDateTime(dataTable.Rows[i][nomeBase + "DataCadastro"].ToString());
                endereco.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;

                // se estiver no modoBasico, não precisa verificar se é para carregar os demais módulos
                if (modoBasico)
                {
                    lista.Add(endereco);
                    continue;
                }

                // carrega as demais entidades necessárias
                endereco = CarregarEntidades(Entidades, endereco);

                lista.Add(endereco);
            }

            // se a lista não tiver nenhum objeto, retorna NULL, caso contrário retorna a própria lista
            return (lista.Count == 0) ? null : lista;
        }
    }
}