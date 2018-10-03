using System;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.Business
{
    public class Cargo : Base
    {
        /// <summary>
        /// Construtor da Classe
        /// </summary>
        public Cargo()
        { 
        }

        /// <summary>
        /// Carrega um ou mais registros de Cargo
        /// </summary>
        /// <param name="ids">id de Cargo</param>
        /// <returns></returns>
        public GenericModels.Lista Carregar(params Int32[] ids)
        {
            Prion.Generic.Repository.Cargo repCargo = new Prion.Generic.Repository.Cargo(ref this.Conexao);
            return repCargo.BuscarPeloId(ids);
        }

        /// <summary>
        /// Exclui um ou mais registros de Cargo
        /// </summary>
        /// <param name="ids">id de Cargo</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] ids)
        {
            try
            {
                Prion.Generic.Repository.Cargo repCargo = new Prion.Generic.Repository.Cargo(ref this.Conexao);
                repCargo.Excluir(ids);
            }
            catch (Exception e)
            {
                return (new GenericHelpers.Retorno(e.Message, false));
            }

            return (new GenericHelpers.Retorno());
        }

        /// <summary>
        /// Obtém um DataTable de Cargos que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaCargos(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Prion.Generic.Repository.Cargo repCargo = new Prion.Generic.Repository.Cargo(ref this.Conexao);
            repCargo.Entidades.Adicionar(parametro.Entidades);

            return repCargo.Lista(parametro);
        }

        /// <summary>
        /// Salva um objeto de Cargo no Banco de Dados
        /// </summary>
        /// <param name="cargo">objeto do tipo Prion.Generic.Models.Cargo</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(Prion.Generic.Models.Cargo cargo)
        {
            this.Validar(cargo);

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            cargo.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

            //mudar o estado do objeto para ALTERADO somente se id<>0
            if (cargo.Id != 0) { cargo.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }
            {
                try
                {
                    Prion.Generic.Repository.Cargo repCargo = new Prion.Generic.Repository.Cargo(ref this.Conexao);

                    retorno = repCargo.Salvar(cargo);
                    repCargo = null;

                    retorno.Mensagem = (cargo.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Cargo inserido com sucesso." : "Cargo atualizado com sucesso.";
                    this.Conexao.Commit();
                }
                catch (Exception e)
                {
                    this.Conexao.Rollback();
                    retorno = new GenericHelpers.Retorno(e.Message, false);
                }

                return retorno;
            }
        }

        /// <summary>
        /// Valida os atributos obrigatórios de uma Prion.Generic.Model.Cargo
        /// </summary>
        /// <param name="cargo">Objeto do tipo Prion.Generic.Model.Cargo</param>
        private void Validar(Prion.Generic.Models.Cargo cargo)
        {
            if (cargo == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }

            if (cargo.IdSituacao == 0)
            {
                throw new Prion.Tools.PrionException("A Situação tem que se informada.");
            }

            if (cargo.Nome.Trim() == "")
            {
                throw new Prion.Tools.PrionException("O nome do Cargo não pode ser vazio.");
            }

            if ((cargo.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (cargo.Id == 0))
            {
                throw new Prion.Tools.PrionException("O id do Cargo não pode ser vazio.");
            }
        }
    }
}