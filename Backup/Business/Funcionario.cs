using System;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.Business
{
    public class Funcionario : Base
    {
         /// <summary>
        /// Construtor da Classe
        /// </summary>
        public Funcionario()
        { 
        }


        /// <summary>
        /// Carrega um ou mais registros de Funcionario
        /// </summary>
        /// <param name="ids">id de Funcionario</param>
        /// <returns></returns>
        public GenericModels.Lista Carregar(Prion.Tools.Request.ParametrosRequest parametro, params Int32[] ids)
        {
            Prion.Generic.Repository.Funcionario repFuncionario = new Prion.Generic.Repository.Funcionario(ref this.Conexao);
            repFuncionario.Entidades.Adicionar(parametro.Entidades);
            return repFuncionario.BuscarPeloId(ids);
        }


        /// <summary>
        /// Exclui um ou mais registros de Funcionario
        /// </summary>
        /// <param name="ids">id de Funcionario</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] ids)
        {
            try
            {
                Prion.Generic.Repository.Funcionario repFuncionario = new Prion.Generic.Repository.Funcionario(ref this.Conexao);
                repFuncionario.Excluir(ids);
            }
            catch (Exception e)
            {
                return (new GenericHelpers.Retorno(e.Message, false));
            }

            return (new GenericHelpers.Retorno());
        }


        /// <summary>
        /// Obtém um DataTable de Funcionarios que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaFuncionarios(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Prion.Generic.Repository.Funcionario repFuncionario = new Prion.Generic.Repository.Funcionario(ref this.Conexao);
            repFuncionario.Entidades.Adicionar(parametro.Entidades);

            return repFuncionario.Lista(parametro);
        }


        /// <summary>
        /// Salva um objeto de Funcionario no Banco de Dados
        /// </summary>
        /// <param name="funcionario">objeto do tipo Models.Funcionario</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(Prion.Generic.Models.Funcionario funcionario)
        {
            this.Validar(funcionario);

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            funcionario.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

            //mudar o estado do objeto para ALTERADO somente se id<>0
            if (funcionario.Id != 0) { funcionario.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }


            try
            {
                Prion.Generic.Repository.Funcionario repFuncionario = new Prion.Generic.Repository.Funcionario(ref this.Conexao);
                Prion.Generic.Repository.FuncionarioTelefone repTelefone = new Prion.Generic.Repository.FuncionarioTelefone(ref this.Conexao);
                FormatarCampos(ref funcionario);

                retorno = repFuncionario.Salvar(funcionario);
                for (Int32 i = 0; i < funcionario.Telefones.Count; i++)
                {
                    funcionario.Telefones[i].IdFuncionario = (retorno.UltimoId == 0) ? funcionario.Id : retorno.UltimoId;                    
                }
                repTelefone.Salvar(funcionario.Telefones);
                repFuncionario = null;

                retorno.Mensagem = (funcionario.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Funcionário inserido com sucesso." : "Funcionário atualizado com sucesso.";
                this.Conexao.Commit();
            }
            catch (Exception e)
            {
                this.Conexao.Rollback();
                retorno = new GenericHelpers.Retorno(e.Message, false);
            }

            return retorno;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="funcionario"></param>
        private void FormatarCampos(ref GenericModels.Funcionario funcionario)
        {
            // remove a formatação do cpf
            funcionario.Cpf = funcionario.Cpf.Replace(".", "").Replace("-", "");
        }


        /// <summary>
        /// Valida os atributos obrigatórios de uma Model.Funcionario
        /// </summary>
        /// <param name="funcionario">Objeto do tipo Model.Funcionario</param>
        private void Validar(Prion.Generic.Models.Funcionario funcionario)
        {
            if (funcionario == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }

            if (funcionario.IdSituacao == 0)
            {
                throw new Prion.Tools.PrionException("A Situação tem que se informada.");
            }

            if (funcionario.Nome.Trim() == "")
            {
                throw new Prion.Tools.PrionException("O nome do Funcionário não pode ser vazio.");
            }

            if ((funcionario.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (funcionario.Id == 0))
            {
                throw new Prion.Tools.PrionException("O id do Funcionário não pode ser vazio.");
            }
        }
    }
}
