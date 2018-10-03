using System;
using System.Collections.Generic;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;
using GenericRepository = Prion.Generic.Repository;


namespace NotaAzul.Business
{
    public class ContasPagar:Base
    {
          /// <summary>
        /// Construtor da Classe
        /// </summary>
        public ContasPagar()
        { 
        }


        /// <summary>
        /// Carrega um ou mais registros de títulos pagos
        /// </summary>
        /// <param name="ids">id do Cheque Estado</param>
        /// <returns></returns>
        public GenericModels.Lista Carregar(params Int32[] ids)
        {
            Repository.ContasPagar repContasPagar = new Repository.ContasPagar(ref this.Conexao);
            return repContasPagar.Buscar(ids);
        }


        /// <summary>
        /// Obtém um DataTable de ContasPagar que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaContasPagar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.ContasPagar repContasPagar = new Repository.ContasPagar(ref this.Conexao);
            repContasPagar.Entidades.Adicionar(parametro.Entidades);
           

            return repContasPagar.DataTable(parametro);
        }


        /// <summary>
        /// Salva um objeto de Prion.Generic.Models.Titulo no Banco de Dados
        /// </summary>
        /// <param name="titulos">uma lista de objetos do tipo Prion.Generic.Models.Titulo</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(List<GenericModels.Titulo> titulos)
        {
            this.Validar(titulos);
            
            Decimal valorTotal = 0;
            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;


            GenericRepository.Titulo repTitulo;
            Repository.MovimentacaoFinanceira repMovimentacaoFinanceira;
            GenericRepository.Situacao repSituacao;
            GenericModels.Situacao situacao;
            //GenericRepository.Cheque repCheque;
            Repository.Cheque repCheque;
            GenericRepository.Cartao repCartao;
            GenericRepository.Especie repEspecie;

            try
            {
                repTitulo = new GenericRepository.Titulo(ref this.Conexao);
                repSituacao = new GenericRepository.Situacao(ref this.Conexao);

                // carrega a situação do título
                situacao = (GenericModels.Situacao)repSituacao.BuscarPelaSituacao("Título", "Quitado");

                // varre a lista de título
                foreach (GenericModels.Titulo titulo in titulos)
                {
                    // se o titulo estiver marcado como excluido, não atualiza as suas informações
                    if (titulo.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Excluido)
                    {
                        continue;
                    }

                    valorTotal += titulo.Valor;

                    //Atualiza o objeto de título com a data de operação
                    titulo.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado;
                    titulo.Situacao = situacao;
                    titulo.IdSituacao = situacao.Id;

                    repTitulo.Salvar(titulo);
                }

                // carrega a situação 'Concluído' para utilizar na movimentação financeira, e nos relacionamentos com Cartão, Cheque e Espécie
                situacao = (GenericModels.Situacao)repSituacao.BuscarPelaSituacao("Genérico", "Concluído");

                Models.MovimentacaoFinanceira movimentacaoFinanceira = new Models.MovimentacaoFinanceira();
                movimentacaoFinanceira.Valor = valorTotal;
                movimentacaoFinanceira.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;
               

                //Business.MovimentacaoFinanceira buMovimentacaoFinanceira = new Business.MovimentacaoFinanceira();
                //buMovimentacaoFinanceira.Salvar(movimentacaoFinanceira);

                repMovimentacaoFinanceira = new Repository.MovimentacaoFinanceira(ref this.Conexao);
                retorno = repMovimentacaoFinanceira.Salvar(movimentacaoFinanceira);

                if (!retorno.Sucesso)
                {
                    this.Conexao.Rollback();
                    return retorno;

                }

                Int32 idMovimentacaoFinanceira = retorno.UltimoId;

                //Responsável por criar o relacionamento entre as formas de pagamento e um Título
                repCartao = new GenericRepository.Cartao(ref this.Conexao);
                //repCheque = new GenericRepository.Cheque(ref this.Conexao);
                repCheque = new Repository.Cheque(ref this.Conexao);
                repEspecie = new GenericRepository.Especie(ref this.Conexao);

                foreach (GenericModels.Titulo titulo in titulos)
                {
                    // se o titulo estiver marcado como excluido, não cria o relacionamento com a movimentação financeira
                    if (titulo.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Excluido)
                    {
                        continue;
                    }

                    //Cria o relacionamento entre uma uma movimentação Financeira e um Título
                    repMovimentacaoFinanceira.CriarRelacionamento(idMovimentacaoFinanceira, titulo.Id, situacao.Id);
                    List<Prion.Generic.Models.Base> listaBase;
                    Func<Int32, Int32, Int32, Int32> executarAposSalvar;
                   
                    listaBase = Prion.Tools.ListTo.CollectionToList<Prion.Generic.Models.Base>(titulo.ListaCartao);
                    executarAposSalvar = repMovimentacaoFinanceira.CriarRelacionamentoCartao;
                    CriarRelacionamentoComTitulo(repCartao, listaBase, titulo.Id, situacao.Id, executarAposSalvar);   
                  
                    listaBase = Prion.Tools.ListTo.CollectionToList<Prion.Generic.Models.Base>(titulo.ListaCheque);
                    executarAposSalvar = repMovimentacaoFinanceira.CriarRelacionamentoCheque;
                    CriarRelacionamentoComTitulo(repCheque, listaBase, titulo.Id, situacao.Id, executarAposSalvar);
                    
                    listaBase = Prion.Tools.ListTo.CollectionToList<Prion.Generic.Models.Base>(titulo.ListaEspecie);
                    executarAposSalvar = repMovimentacaoFinanceira.CriarRelacionamentoEspecie;
                    CriarRelacionamentoComTitulo(repEspecie, listaBase, titulo.Id, situacao.Id, executarAposSalvar);
                   
                }
                
                retorno.Mensagem = (retorno.Sucesso) ? "Pagamento do Título efetuado com sucesso." : retorno.Mensagem;
                this.Conexao.Commit();
            }
            catch (Exception e)
            {
                this.Conexao.Rollback();
                retorno = new GenericHelpers.Retorno(e.Message, false);
            }
            finally
            {
                repMovimentacaoFinanceira = null;
                repTitulo = null;
                repSituacao = null;
                //repCheque;
                repCartao = null;
                repEspecie = null;
            }

            return retorno;
            
        }


        /// <summary>
        /// Cancela um ou mais pagamentos de títulos
        /// Altera a situação do título, marcando-o como 'Aberto'
        /// Altera a situação da movimentação financeira, marcando-a como 'Cancelada'
        /// </summary>
        /// <param name="ids">id de titulo</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] ids)
        {
            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;
            GenericRepository.Titulo repTitulo;
            Repository.MovimentacaoFinanceira repMovimentacaoFinanceira;
            GenericRepository.Situacao repSituacao;

            try
            {
                repTitulo = new GenericRepository.Titulo(ref this.Conexao);
                repMovimentacaoFinanceira = new Repository.MovimentacaoFinanceira(ref this.Conexao);
                repSituacao = new GenericRepository.Situacao(ref this.Conexao);

                GenericModels.Situacao situacaoAberta = (GenericModels.Situacao)repSituacao.BuscarPelaSituacao("Título", "Aberto");
                GenericModels.Situacao situacaoCancelada = (GenericModels.Situacao)repSituacao.BuscarPelaSituacao("Genérico", "Cancelado");
                GenericModels.Situacao situacaoConcluida = repSituacao.BuscarPelaSituacao("Genérico", "Concluído");
                List<Models.MovimentacaoFinanceira> movimentacoes;


                // carrega uma lista de títulos através de seu ID
                List<GenericModels.Titulo> titulos = Prion.Tools.ListTo.CollectionToList<GenericModels.Titulo>(repTitulo.BuscarPeloId(ids).ListaObjetos);

                // varre a lista de título para excluir o relacionamento com a movimentação financeira
                foreach (GenericModels.Titulo titulo in titulos)
                {
                    //Atualiza o objeto de título com a data de operação, marcando o título como 'Aberto'
                    titulo.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado;
                    titulo.Situacao = situacaoAberta;
                    titulo.IdSituacao = situacaoAberta.Id;
                    titulo.DataOperacao = new DateTime();
                    repTitulo.Salvar(titulo);                   
                 

                    movimentacoes = Prion.Tools.ListTo.CollectionToList<Models.MovimentacaoFinanceira>(repMovimentacaoFinanceira.BuscarMovimentacoesConcluidasDeUmTitulo(situacaoConcluida.Id, titulo.Id).ListaObjetos);

                    foreach (Models.MovimentacaoFinanceira movimentacaoFinanceira in movimentacoes)
                    {
                        //Atualiza o objeto de título com a data de operação
                        repMovimentacaoFinanceira.AtualizarRelacionamento(movimentacaoFinanceira.Id, titulo.Id, situacaoCancelada.Id);
                    }

                }

                repMovimentacaoFinanceira.AtualizarRelacionamentoCartao(ids, situacaoCancelada.Id);
                repMovimentacaoFinanceira.AtualizarRelacionamentoCheque(ids, situacaoCancelada.Id);
                repMovimentacaoFinanceira.AtualizarRelacionamentoEspecie(ids, situacaoCancelada.Id);

                retorno = new GenericHelpers.Retorno();
                this.Conexao.Commit();
            }
            catch (Exception e)
            {
                this.Conexao.Rollback();
                retorno = new GenericHelpers.Retorno(e.Message, false);
            }
            finally
            {

                repTitulo = null;
                repMovimentacaoFinanceira = null;
                repSituacao = null;
            }

            return retorno;
            
        }


        /// <summary>
        /// Valida os atributos obrigatórios de uma GenericModels.Titulo
        /// </summary>
        /// <param name="Titulos">Uma lista de bjetos do tipo GenericModels.Titulo</param>
        private void Validar(List<GenericModels.Titulo> titulos)
        {
            foreach(GenericModels.Titulo titulo in titulos)
            {
                if (titulo == null)
                {
                    throw new Prion.Tools.PrionException("Objeto não definido.");
                }

                if (titulo.IdSituacao == 0)
                {
                    throw new Prion.Tools.PrionException("A Situação tem que se informada.");
                }

                if (titulo.IdTituloTipo == 0)
                {
                    throw new Prion.Tools.PrionException("O tipo de título tem que se informado.");
                }

                if (titulo.Descricao.Trim() == "")
                {
                    throw new Prion.Tools.PrionException("A descrição do título não pode ser vazio.");
                }

                if (titulo.Valor == 0)
                {
                    throw new Prion.Tools.PrionException("O valor título não pode ser vazio.");
                }

                if (Prion.Tools.IsNull.Date(titulo.DataVencimento))
                {
                    throw new Prion.Tools.PrionException("A data de vencimento do título não pode ser vazia.");
                }

                if (Prion.Tools.IsNull.Date(titulo.DataOperacao))
                {
                    throw new Prion.Tools.PrionException("A data da operação do título não pode ser vazia.");
                }

                if ((titulo.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (titulo.Id == 0))
                {
                    throw new Prion.Tools.PrionException("O id do Estado do título não pode ser vazio.");
                }
            }
        }


        /// <summary>
        /// Recebe um repositorio base,para salvar uma lista de objetos base
        /// Utiliza a função recebida para criar o relacionamento entre o elemento recem inserido e um título
        /// </summary>
        /// <param name="repositorio"></param>
        /// <param name="listaFormaPagamento"></param>
        /// <param name="idTitulo"></param>
        /// <param name="IdSituacao"></param>
        /// <param name="executarAposSalvar"></param>
        private GenericHelpers.Retorno CriarRelacionamentoComTitulo(Prion.Generic.Repository.Base repositorio, List<GenericModels.Base> listaFormaPagamento,Int32 idTitulo,Int32 idSituacao, Func<Int32,Int32,Int32,Int32> executarAposSalvar)
        {
            if (listaFormaPagamento.Count == 0)
            {
                return null;
            }

            GenericHelpers.Retorno retorno = null;

            foreach (GenericModels.Base formaPagamento in listaFormaPagamento)
            {
                formaPagamento.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;
                retorno = repositorio.Salvar(formaPagamento);

                // se houve algum erro ao salvar o titulo, cancela a transação e retorna o erro
                if (!retorno.Sucesso)
                {
                    this.Conexao.Rollback();
                    return retorno;
                }

                // cria um relacionamento entre o cartão e o título
                executarAposSalvar(retorno.UltimoId,idTitulo,idSituacao);           

            }

            return retorno;
        }
    }
}