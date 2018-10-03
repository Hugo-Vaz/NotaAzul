using System;
using GenericHelpers = Prion.Generic.Helpers;

namespace NotaAzul.Business
{
    public class Turno : Base
    {
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public Turno()
        {
        }

        /// <summary>
        /// Carrega um registro de Turno
        /// </summary>
        /// <param name="ids">id do Turno</param>
        /// <returns></returns>
        public Prion.Generic.Models.Lista Carregar(params Int32[] ids)
        {
            Repository.Turno repTurno = new Repository.Turno(ref this.Conexao);
            return repTurno.Buscar(ids);
        }

        /// <summary>
        /// Exclui um ou mais registros de Turno
        /// </summary>
        /// <param name="id">id do Turno</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] id)
        {
            try
            {
                Repository.Turno repTurno = new Repository.Turno(ref this.Conexao);
                repTurno.Excluir(id);
            }
            catch (Exception e)
            {
                return (new GenericHelpers.Retorno(e.Message, false));
            }

            return (new GenericHelpers.Retorno());
        }

        /// <summary>
        /// Obtém a lista de Turnos
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista Lista(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Turno repTurno = new Repository.Turno(ref this.Conexao);
            return repTurno.Buscar(parametro);
        }

        /// <summary>
        /// Obtém um DataTable de Turnos que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaTurnos(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Turno repTurno = new Repository.Turno(ref this.Conexao);
            repTurno.Entidades.Adicionar(parametro.Entidades);

            return repTurno.DataTable(parametro);
        }

        /// <summary>
        /// Salva um objeto de Turno no banco de dados
        /// </summary>
        /// <param name="turno">objeto do tipo Models.Turno</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(Models.Turno turno)
        {
            this.Validar(turno);

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = new GenericHelpers.Retorno("", true);

            turno.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

            // mudar o estado do objeto para ALTERADO apenas se ID <> 0
            if (turno.Id != 0) { turno.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }

            try
            {
                Repository.Turno repTurno = new Repository.Turno(ref this.Conexao);

                retorno = repTurno.Salvar(turno);
                repTurno = null;

                retorno.Mensagem = (turno.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Turno inserido com sucesso." : "Turno atualizado com sucesso";
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
        /// Valida os atributos obrigatórios de uma Model.Turno
        /// </summary>
        /// <param name="turno">Objeto do tipo Model.Turno</param>
        private void Validar(Models.Turno turno)
        {
            if (turno == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }

            if (turno.IdSituacao == 0)
            {
                throw new Prion.Tools.PrionException("A situação do turno não pode ser vazio.");
            }

            if (turno.Nome.Trim() == "")
            {
                throw new Prion.Tools.PrionException("O nome do turno não pode ser vazio.");
            }

            if ((turno.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (turno.Id == 0))
            {
                throw new Prion.Tools.PrionException("O id do turno não pode ser vazio.");
            }
        }
    }
}