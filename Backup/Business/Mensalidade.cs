using System;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;
using System.Collections.Generic;

namespace NotaAzul.Business
{
    public class Mensalidade : Base
    {
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public Mensalidade()
        {
        }

        /// <summary>
        /// Atualizar datas de vencimento
        /// </summary>
        /// <param name="diaVencimento">dia de vencimento</param>
        /// <param name="idAluno">id do Aluno</param>
        /// <returns></returns>
        public void AlterarDiaVencimento(Int32 diaVencimento,Int32 idAluno)
        {
            Repository.Mensalidade repMensalidade = new Repository.Mensalidade(ref this.Conexao);
            repMensalidade.AlterarDataVencimento(diaVencimento, idAluno);
        }


        /// <summary>
        /// Carrega um ou mais registros de Mensalidade
        /// </summary>
        /// <param name="ids">id do Mensalidade</param>
        /// <returns></returns>
        public GenericModels.Lista Carregar(params Int32[] ids)
        {
            Repository.Mensalidade repMensalidade = new Repository.Mensalidade(ref this.Conexao);
            return repMensalidade.Buscar(ids);
        }

        /// <summary>
        /// Exclui um ou mais registros de Mensalidade
        /// </summary>
        /// <param name="id">id do Mensalidade</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] ids)
        {
            try
            {
                Repository.Mensalidade repMensalidade = new Repository.Mensalidade(ref this.Conexao);
                repMensalidade.Excluir(ids);
            }
            catch (Exception e)
            {
                return (new GenericHelpers.Retorno(e.Message, false));
            }

            return (new GenericHelpers.Retorno());
        }

        /// <summary>
        /// Obtém a lista de Mensalidades
        /// </summary>
        /// <returns>Lista de Mensalidades</returns>
        public Prion.Generic.Models.Lista Lista(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Mensalidade repMensalidade = new Repository.Mensalidade(ref this.Conexao);
            return repMensalidade.Buscar(parametro);
        }

        /// <summary>
        /// Obtém um DataTable de Mensalidades que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaMensalidades(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Mensalidade repMensalidade = new Repository.Mensalidade(ref this.Conexao);
            repMensalidade.Entidades.Adicionar(parametro.Entidades);

            return repMensalidade.DataTable(parametro);
        }

        /// <summary>
        /// Salva um objeto de Mensalidade no banco de dados
        /// </summary>
        /// <param name="listaMensalidades">objeto do tipo Models.Mensalidade</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(List<Models.Mensalidade> listaMensalidades)
        {

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            for(Int32 i=0;i<listaMensalidades.Count;i++){
                this.Validar(listaMensalidades[i]);                

                listaMensalidades[i].EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

                // mudar o estado do objeto para ALTERADO apenas se ID <> 0
                if (listaMensalidades[i].Id != 0) { listaMensalidades[i].EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }

                try
                {
                    Repository.Mensalidade rpMensalidade = new Repository.Mensalidade(ref this.Conexao);

                    retorno = rpMensalidade.Salvar(listaMensalidades);
                    rpMensalidade = null;

                    retorno.Mensagem = (listaMensalidades[i].EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Mensalidade inserida com sucesso." : "Mensalidade atualizada com sucesso";
                    this.Conexao.Commit();
                }
                catch (Exception e)
                {
                    this.Conexao.Rollback();
                    retorno = new GenericHelpers.Retorno(e.Message, false);
                }
            }
            return retorno;
        }

        /// <summary>
        /// Valida os atributos obrigatórios de uma Model.Mensalidade
        /// </summary>
        /// <param name="mensalidade">Objeto do tipo Model.Mensalidade</param>
        private void Validar(Models.Mensalidade mensalidade)
        {
            if (mensalidade == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }

            
            if (mensalidade.Valor == 0)
            {
                throw new Prion.Tools.PrionException("O valor tem que ser informado.");
            }

            if (Prion.Tools.IsNull.Date(mensalidade.DataVencimento))
            {
                throw new Prion.Tools.PrionException("A data de vencimento tem que ser informada.");
            }

            if ((mensalidade.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (mensalidade.Id == 0))
            {
                throw new Prion.Tools.PrionException("O id do mensalidade não pode ser vazio.");
            }
        }
    }
}