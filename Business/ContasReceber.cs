using System;
using System.Collections.Generic;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;
using GenericRepository = Prion.Generic.Repository;
using Prion.Tools;


namespace NotaAzul.Business
{
    public class ContasReceber:Base
    {
        /// <summary>
        /// Construtor da Classe
        /// </summary>
        public ContasReceber()
        { 
        }

        /// <summary>
        /// Carrega as mensalidades e seus títulos de determinado aluno
        /// </summary>
        /// <param name="id">id de uma mensalidade</param>
        /// <returns></returns>
        public List<Models.MensalidadeTitulo> Carregar(Int32 idMensalidade)
        {
            Repository.ContasReceber repContasReceber = new Repository.ContasReceber(ref this.Conexao);
            List<Models.MensalidadeTitulo> mensalidades = ListTo.CollectionToList<Models.MensalidadeTitulo>(repContasReceber.BuscarTodasMensalidadesDeUmAluno(idMensalidade).ListaObjetos);

            return mensalidades;
        }

        /// <summary>
        /// Carrega as mensalidades e seus títulos de determinado aluno
        /// </summary>
        /// <param name="id">id de uma mensalidade</param>
        /// <returns></returns>
        public Prion.Generic.Models.Lista CarregarMensalidadePaga(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.ContasReceber repContasReceber = new Repository.ContasReceber(ref this.Conexao);
          
            return repContasReceber.DataTableMensalidadePaga(parametro);
        }

        /// <summary>
        /// Carrega as mensalidades e seus títulos de determinado aluno
        /// </summary>
        /// <param name="id">id de uma mensalidade</param>
        /// <returns></returns>
        public Prion.Generic.Models.Lista CarregarMensalidadesPagas(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.ContasReceber repContasReceber = new Repository.ContasReceber(ref this.Conexao);
            return repContasReceber.BuscarTodasMensalidadesPagas(parametro);

        }
       
        /// <summary>
        /// Obtém um DataTable de ContasPagar que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaContasReceber(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.ContasReceber repContasReceber = new Repository.ContasReceber(ref this.Conexao);
            repContasReceber.Entidades.Adicionar(parametro.Entidades);
           

            return repContasReceber.DataTable(parametro);
        }

        /// <summary>
        /// Salva um objeto de Models.MensalidadeTitulo no Banco de Dados
        /// </summary>
        /// <param name="mensalidadeTitulo">uma lista de objetos do tipo Models.MensalidadeTitulo</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(List<Models.MensalidadeTitulo> mensalidadeTitulos)
        {
                   
            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            Repository.Mensalidade repMensalidade;
            GenericRepository.Titulo repTitulo;
            Repository.MovimentacaoFinanceira repMovimentacaoFinanceira;
            GenericRepository.Situacao repSituacao;            
            Repository.Cheque repCheque;
            GenericRepository.Cartao repCartao;
            GenericRepository.Especie repEspecie;
            
            GenericModels.Situacao situacaoQuitada;
            GenericModels.Situacao situacaoMensalidade;
            GenericModels.Situacao situacaoConcluida;
            
            List<GenericModels.Titulo> titulos = new List<GenericModels.Titulo>();
            List<Models.Mensalidade> mensalidades = new List<Models.Mensalidade>();
            Int32[] idsCheque;
            Int32[] idsCartao;
            Int32[] idsEspecie;
            
            try
            {
                repTitulo = new GenericRepository.Titulo(ref this.Conexao);
                repSituacao = new GenericRepository.Situacao(ref this.Conexao);
                repMensalidade = new Repository.Mensalidade(ref this.Conexao);

                // carrega a situação do título
                situacaoQuitada = (GenericModels.Situacao)repSituacao.BuscarPelaSituacao("Título", "Quitado");
                
                //carrega a situação da mensalidade
                situacaoMensalidade = (GenericModels.Situacao)repSituacao.BuscarPelaSituacao("Mensalidade", "Quitada");

                // carrega a situação da movimentação financeira,Cartão,Cheque e Espécie
                situacaoConcluida = (GenericModels.Situacao)repSituacao.BuscarPelaSituacao("Genérico", "Concluído");

                // varre a lista de título
                foreach (Models.MensalidadeTitulo mensalidadeTitulo in mensalidadeTitulos)
                {
                    
                    //Variável que será utilizada para calcular o valor pago em cada título
                    Decimal valorSaldo = mensalidadeTitulo.ValorPago;
                   
                    //carrega as mensalidades de acordo com o array de inteiros contendo os ids das mensalidades a serem pagas
                    repMensalidade.Entidades.Adicionar(Prion.Generic.Helpers.Entidade.Titulo.ToString());
                    mensalidades = ListTo.CollectionToList<Models.Mensalidade>(repMensalidade.BuscarPeloId(mensalidadeTitulo.MensalidadesPagas).ListaObjetos);

                    //Cria arrays, cujo tamanho é equivalente ao contador da lista correspondente a forma de pagamento
                    idsCartao = new  Int32[mensalidadeTitulo.ListaCartao.Count];
                    idsCheque = new Int32[mensalidadeTitulo.ListaCheque.Count];
                    idsEspecie = new Int32[mensalidadeTitulo.ListaEspecie.Count];
                                    
                   
                    Models.MovimentacaoFinanceira movimentacaoFinanceira = new Models.MovimentacaoFinanceira();
                    movimentacaoFinanceira.Valor = mensalidadeTitulo.ValorPago;
                    movimentacaoFinanceira.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

                    repMovimentacaoFinanceira = new Repository.MovimentacaoFinanceira(ref this.Conexao);
                    retorno = repMovimentacaoFinanceira.Salvar(movimentacaoFinanceira);
                    Int32 idMovimentacaoFinanceira = retorno.UltimoId;

                    //Responsável salvar as formas de pagamento
                    repCartao = new GenericRepository.Cartao(ref this.Conexao);                    
                    repCheque = new Repository.Cheque(ref this.Conexao);
                    repEspecie = new GenericRepository.Especie(ref this.Conexao);
                    List<Prion.Generic.Models.Base> listaBase;

                    listaBase = Prion.Tools.ListTo.CollectionToList<Prion.Generic.Models.Base>(mensalidadeTitulo.ListaCartao);
                    idsCartao = SalvarFormaPagamento(repCartao, listaBase);

                    listaBase = Prion.Tools.ListTo.CollectionToList<Prion.Generic.Models.Base>(mensalidadeTitulo.ListaCheque);
                    idsCheque = SalvarFormaPagamento(repCheque, listaBase);

                    listaBase = Prion.Tools.ListTo.CollectionToList<Prion.Generic.Models.Base>(mensalidadeTitulo.ListaEspecie);
                    idsEspecie = SalvarFormaPagamento(repEspecie, listaBase);

                    Int32 quantidadeDeTitulos = mensalidades.Count;

                    //Altera a situação dos títulos e insere a data de operação
                    //Cria os relacionamentos entre um título e movimentação financeira e suas formas de pagamento
                    //Altera a situação das mensalidades para quitada
                    foreach(Models.Mensalidade mensalidade in mensalidades)
                    {
                        mensalidade.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado;
                        mensalidade.IdSituacao = situacaoMensalidade.Id;
                        repMensalidade.Salvar(mensalidade);

                        valorSaldo -= (mensalidade.Titulo.Valor + mensalidade.Titulo.Acrescimo);                      
                        Decimal valorTotal = (mensalidade.Titulo.Valor + mensalidade.Titulo.Acrescimo);
                        if ((mensalidadeTitulo.DataOperacao <= mensalidade.Titulo.DataVencimento) 
                            || (mensalidade.Titulo.DataVencimento.DayOfWeek == DayOfWeek.Saturday && mensalidade.Titulo.DataOperacao <= mensalidade.DataCadastro.AddDays(2))
                                || (mensalidade.Titulo.DataVencimento.DayOfWeek == DayOfWeek.Sunday && mensalidade.Titulo.DataOperacao <= mensalidade.DataCadastro.AddDays(1)))
                        {
                            valorSaldo += mensalidade.Titulo.Desconto;
                            valorTotal -= mensalidade.Titulo.Desconto;
                        }
                        mensalidade.Titulo.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado;
                        mensalidade.Titulo.IdSituacao = situacaoQuitada.Id;
                        mensalidade.Titulo.DataOperacao = mensalidadeTitulo.DataOperacao;
                        mensalidade.Titulo.ValorPago = (quantidadeDeTitulos == 1) ? valorSaldo + valorTotal : valorTotal;
                        repTitulo.Salvar(mensalidade.Titulo);

                        repMovimentacaoFinanceira.CriarRelacionamento(idMovimentacaoFinanceira, mensalidade.Titulo.Id, situacaoConcluida.Id);

                        for (Int32 i = 0; i < idsCartao.Length; i++)
                        {
                            repMovimentacaoFinanceira.CriarRelacionamentoCartao(idsCartao[i], mensalidade.Titulo.Id, situacaoConcluida.Id);
                        }

                        for(Int32 i = 0; i < idsCheque.Length; i++)
                        {
                            repMovimentacaoFinanceira.CriarRelacionamentoCheque(idsCheque[i], mensalidade.Titulo.Id, situacaoConcluida.Id);
                        }

                        for (Int32 i = 0; i < idsEspecie.Length; i++)
                        {
                            repMovimentacaoFinanceira.CriarRelacionamentoEspecie(idsEspecie[i], mensalidade.Titulo.Id, situacaoConcluida.Id);
                        }

                        //Se for o ultimo título a ser salvo(pago), ou seja a variável quantidadeDeTitulos for igual a 1,o id do título 
                        //será incrementado para que o título do mês seguinte, receba o saldo do mês atual como acréscimo ou desconto
                        if (quantidadeDeTitulos == 1 && mensalidade.Titulo.DataVencimento.Month < 12)
                        {

                            Int32 idProximaMensalidade = ++mensalidade.Id;
                            Models.Mensalidade mensalidadeProximoMes =(Models.Mensalidade)repMensalidade.BuscarPeloId(idProximaMensalidade).Get(0);
                            if (mensalidadeProximoMes != null)
                            {
                                if (valorSaldo > 0)
                                {
                                    mensalidadeProximoMes.Titulo.Desconto += Math.Abs(valorSaldo);
                                    mensalidadeProximoMes.Desconto += Math.Abs(valorSaldo);
                                }
                                else if (valorSaldo < 0)
                                {
                                    mensalidadeProximoMes.Titulo.Acrescimo += Math.Abs(valorSaldo);
                                    mensalidadeProximoMes.Acrescimo += Math.Abs(valorSaldo);

                                }
                                mensalidadeProximoMes.Titulo.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Alterado;
                                mensalidade.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Alterado;
                                repTitulo.Salvar(mensalidadeProximoMes.Titulo);
                                repMensalidade.Salvar(mensalidadeProximoMes);
                            }
                        }
                       
                        quantidadeDeTitulos--;
                    }
                }
                
                retorno.Mensagem = (retorno.Sucesso) ? "Pagamento da(s) mensalidade(s) efetuado com sucesso." : retorno.Mensagem;
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
                repCheque = null;
                repCartao = null;
                repEspecie = null;
            }

            return retorno;

        }

         /// <summary>
        /// Cancela um ou mais pagamentos de mensalidades
        /// Altera a situação do título, marcando-o como 'Aberto'
        /// Altera a situação da mensalidade, marcando a como 'Aberta'
        /// Altera a situação da movimentação financeira, marcando-a como 'Cancelada'
        /// </summary>
        /// <param name="idsMensalidade">id de titulo</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] idsMensalidade)
        {
            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;
            GenericRepository.Titulo repTitulo;
            Repository.MovimentacaoFinanceira repMovimentacaoFinanceira;
            Repository.Mensalidade repMensalidade;
            GenericRepository.Situacao repSituacao;
            Repository.ContasReceber repContasReceber;

            try
            {
                repTitulo = new GenericRepository.Titulo(ref this.Conexao);
                repMovimentacaoFinanceira = new Repository.MovimentacaoFinanceira(ref this.Conexao);
                repSituacao = new GenericRepository.Situacao(ref this.Conexao);
                repMensalidade = new Repository.Mensalidade(ref this.Conexao);
                repContasReceber = new Repository.ContasReceber(ref this.Conexao);

                GenericModels.Situacao situacaoMensalidadeAberta = (GenericModels.Situacao)repSituacao.BuscarPelaSituacao("Mensalidade", "Aberta");
                GenericModels.Situacao situacaoAberta = (GenericModels.Situacao)repSituacao.BuscarPelaSituacao("Título", "Aberto");
                GenericModels.Situacao situacaoCancelada = (GenericModels.Situacao)repSituacao.BuscarPelaSituacao("Genérico", "Cancelado");
                GenericModels.Situacao situacaoConcluida = repSituacao.BuscarPelaSituacao("Genérico", "Concluído");
                List<Models.MovimentacaoFinanceira> movimentacoes;

                // carrega uma lista de mensalidades através de seu ID
                List<Models.Mensalidade> mensalidades = Prion.Tools.ListTo.CollectionToList<Models.Mensalidade>(repMensalidade.BuscarPeloId(idsMensalidade).ListaObjetos);

                //carrega os ids de Título através do relacionamento de Mensalidade e Título
                Int32[] idsTitulo = repContasReceber.CarregarRelacionamentoMensalidadeTitulo(idsMensalidade);

                // carrega uma lista de títulos através de seu ID
                List<GenericModels.Titulo> titulos = Prion.Tools.ListTo.CollectionToList<GenericModels.Titulo>(repTitulo.BuscarPeloId(idsTitulo).ListaObjetos);

                foreach (Models.Mensalidade mensalidade in mensalidades)
                {
                    //Atualiza a situação de um objeto de Mensalidade
                    mensalidade.IdSituacao = situacaoMensalidadeAberta.Id;
                    repMensalidade.Salvar(mensalidade);
                }

                
                // varre a lista de título para "excluir" o relacionamento com a movimentação financeira
                foreach (GenericModels.Titulo titulo in titulos)
                {
                    //Atualiza o objeto de título com a data de operação, marcando o título como 'Aberto'
                    titulo.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado;
                    titulo.Situacao = situacaoAberta;
                    titulo.IdSituacao = situacaoAberta.Id;
                    titulo.DataOperacao = new DateTime();
                    repTitulo.Salvar(titulo);     
                   
                    repMovimentacaoFinanceira.AtualizarRelacionamentoCartao(idsTitulo, situacaoCancelada.Id);  
                   
                    repMovimentacaoFinanceira.AtualizarRelacionamentoCheque(idsTitulo, situacaoCancelada.Id); 
                
                    repMovimentacaoFinanceira.AtualizarRelacionamentoEspecie(idsTitulo,situacaoCancelada.Id); 

                    movimentacoes = Prion.Tools.ListTo.CollectionToList<Models.MovimentacaoFinanceira>(repMovimentacaoFinanceira.BuscarMovimentacoesConcluidasDeUmTitulo(situacaoConcluida.Id, titulo.Id).ListaObjetos);

                    foreach (Models.MovimentacaoFinanceira movimentacaoFinanceira in movimentacoes)
                    {
                        //Atualiza a situação do relacionamento entre uma Movimentação financeira e um título                 
                        repMovimentacaoFinanceira.AtualizarRelacionamento(movimentacaoFinanceira.Id, titulo.Id, situacaoCancelada.Id);
                    }
                }
                retorno = new GenericHelpers.Retorno();
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
            }

            return retorno;
        }

        /// <summary>
        /// Recebe um repositorio base,para salvar uma lista de objetos base        
        /// </summary>
        /// <param name="repositorio"></param>
        /// <param name="listaFormaPagamento"></param>
        /// <param name="idTitulo"></param>
        /// <param name="IdSituacao"></param>
        /// <param name="executarAposSalvar"></param>
        private Int32[] SalvarFormaPagamento(Prion.Generic.Repository.Base repositorio, List<GenericModels.Base> listaFormaPagamento)
        {
            if (listaFormaPagamento.Count == 0)
            {
                return new Int32[0];
            }

            Int32[] ids = new Int32[listaFormaPagamento.Count];
            Int32 contador = 0;
            GenericHelpers.Retorno retorno = null;

            foreach (GenericModels.Base formaPagamento in listaFormaPagamento)
            {
                formaPagamento.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;
                retorno = repositorio.Salvar(formaPagamento);

                // se houve algum erro ao salvar o titulo, cancela a transação e retorna o erro
                if (!retorno.Sucesso)
                {
                    this.Conexao.Rollback();
                    return null;
                }
                ids[contador] = retorno.UltimoId;
                contador++;
            }

            return ids;
        }
    }
}