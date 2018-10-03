using System;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;
using GenericRepository = Prion.Generic.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Prion.Tools;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Data;
using System.Globalization;
using BoletoNet;

namespace NotaAzul.Business
{
    public class Boleto : Base
    {
        /// <summary>
        /// Construtor da Classe
        /// </summary>
        public Boleto()
        {
        }

        /// <summary>
        /// Busca a quantidade de boletos cadastrados no sistema
        /// </summary>
        /// <param name="idCorporacaoComprador"></param>
        /// <returns></returns>
        public Int32 BuscarQuantidadeBoleto()
        {
            try
            {
                Repository.Boleto repBoleto = new Repository.Boleto(ref this.Conexao);
                return repBoleto.BuscarQuantidadeBoleto();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Carrega um ou mais registros de Boleto
        /// </summary>
        /// <param name="ids">id do Boleto</param>
        /// <returns></returns>
        public GenericModels.Lista Carregar(bool arquivoRem = false, params Int32[] ids)
        {
            try
            {
                Repository.Boleto repBoleto = new Repository.Boleto(ref this.Conexao);
                return repBoleto.Buscar(arquivoRem, ids);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Carrega um ou mais registros de Boleto
        /// </summary>
        /// <param name="ids">id do Boleto</param>
        /// <returns></returns>
        public GenericModels.Lista CarregarPorAluno(Int32 idAluno)
        {
            try
            {
                Repository.Boleto repBoleto = new Repository.Boleto(ref this.Conexao);
                return repBoleto.Buscar(idAluno);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Carrega um ou mais registros de Boleto
        /// </summary>
        /// <param name="ids">id do Boleto</param>
        /// <returns></returns>
        public GenericModels.Lista BoletosMensal(int mes,bool gerarArquivo = false)
        {
            try
            {
                Repository.Boleto repBoleto = new Repository.Boleto(ref this.Conexao);
                return repBoleto.Buscar(mes,gerarArquivo);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Exclui um ou mais registros de Boleto
        /// </summary>
        /// <param name="id">id do Boleto</param>
        /// <returns></returns>
        public GenericHelpers.Retorno Excluir(params Int32[] ids)
        {
            try
            {
                Repository.Boleto repBoleto = new Repository.Boleto(ref this.Conexao);
                repBoleto.Excluir(ids);
            }
            catch (Exception e)
            {
                throw e;
            }

            return (new GenericHelpers.Retorno());
        }

        /// <summary>
        /// Obtém a lista de Boletos
        /// </summary>
        /// <returns>Lista de Boletos</returns>
        public Prion.Generic.Models.Lista Lista(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            try
            {
                Repository.Boleto repBoleto = new Repository.Boleto(ref this.Conexao);
                return repBoleto.DataTable(parametro);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Obtém um DataTable de Boletos que será exibido na lista na interface
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista ListaBoletosAbertos(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            try
            {
                Repository.Boleto repBoleto = new Repository.Boleto(ref this.Conexao);
                repBoleto.Entidades.Adicionar(parametro.Entidades);
                parametro.Filtro.Add(new Request.Filtro("Boleto.Status", "=", "'Aberto'"));

                return repBoleto.DataTable(parametro);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Salva um objeto de Boleto no banco de dados
        /// </summary>
        /// <param name="boleto">objeto do tipo Models.Boleto</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(Models.Boleto boleto)
        {
            this.Validar(boleto);

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            boleto.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;

            // mudar o estado do objeto para ALTERADO apenas se ID <> 0
            if (boleto.Id != 0) { boleto.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }

            try
            {
                Repository.Boleto rpBoleto = new Repository.Boleto(ref this.Conexao);

                retorno = rpBoleto.Salvar(boleto);
                rpBoleto = null;
                retorno.Mensagem = (boleto.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Novo) ? "Boleto inserido com sucesso." : "Segunda via gerada com sucesso";
                this.Conexao.Commit();
            }
            catch (Exception e)
            {
                this.Conexao.Rollback();
                throw e;
            }

            return retorno;
        }

        /// <summary>
        /// Salva um objeto de Boleto no banco de dados
        /// </summary>
        /// <param name="boleto">objeto do tipo Models.Boleto</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(List<Models.Boleto> boletos)
        {

            this.Conexao.UsarTransacao = true;
            GenericHelpers.Retorno retorno = null;

            try
            {
                Repository.Boleto rpBoleto = new Repository.Boleto(ref this.Conexao);
                foreach (Models.Boleto boleto in boletos)
                {
                    boleto.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Novo;
                    // mudar o estado do objeto para ALTERADO apenas se ID <> 0
                    if (boleto.Id != 0) { boleto.EstadoObjeto = GenericHelpers.Enums.EstadoObjeto.Alterado; }
                    retorno = rpBoleto.Salvar(boleto);
                    rpBoleto.CriarRelacionamentoBoletoTitulo(retorno.UltimoId, boleto.IdsMensalidade);
                }

                rpBoleto = null;
                retorno.Mensagem = "Boletos criados com sucesso.";
                this.Conexao.Commit();
            }
            catch (Exception e)
            {
                this.Conexao.Rollback();
                throw e;
            }

            return retorno;
        }

        /// <summary>
        /// Monta o Html do Boleto
        /// </summary>
        /// <param name="dadosBoleto">objeto do tipo Models.Boleto</param>
        /// <returns>String com o endereço do Boleto</returns>
        public String GerarHtmlBoleto(Models.Boleto dadosBoleto, String enderecoTemp)
        {
            try
            {
                String nomeArquivo = "Boleto_" + Helpers.DataHora.GetCurrentNow().ToString("yyyyMMdd_HHmmss") + ".html";
                //enderecoTemp, "\\Content\\temp\\boleto", nomeArquivo
                BoletoNet.BoletoBancario boletoBancario = new BoletoNet.BoletoBancario();
                BoletoNet.Boleto boleto = new BoletoNet.Boleto();
                GenericRepository.Corporacao repCorporacao = new GenericRepository.Corporacao(ref this.Conexao);
                GenericModels.Corporacao corporacaoCedente = repCorporacao.BuscarPeloId(Helpers.Settings.IdCorporacao());
                Repository.AlunoResponsavel repResponsavel = new Repository.AlunoResponsavel(ref this.Conexao);
                Models.AlunoResponsavel  resp = repResponsavel.BuscarResponsavelPorBoleto(dadosBoleto.Id);

                boletoBancario.CodigoBanco = 237;
                boleto.Banco = new BoletoNet.Banco(237);                
                

                boleto.Cedente = new BoletoNet.Cedente();
                boleto.Cedente.CPFCNPJ = corporacaoCedente.CNPJ;
                boleto.Cedente.Nome = corporacaoCedente.Nome;
                boleto.Cedente.Convenio = dadosBoleto.Convenio;
                boleto.Cedente.ContaBancaria = new BoletoNet.ContaBancaria();
                boleto.Cedente.ContaBancaria.Agencia = dadosBoleto.NumeroAgencia;
                boleto.Cedente.ContaBancaria.Conta = dadosBoleto.NumeroConta.Split('-')[0];
                boleto.Cedente.ContaBancaria.DigitoAgencia = dadosBoleto.CodigoBanco;
                boleto.Cedente.ContaBancaria.DigitoConta = dadosBoleto.NumeroConta.Split('-')[1];
                boleto.Cedente.Codigo = "4999512";
                boleto.ContaBancaria = boleto.Cedente.ContaBancaria;

                boleto.Carteira = "09";
                boleto.NumeroDocumento = "00000007102";//dadosBoleto.NumeroDocumento; // ESTE NÚMERO É SEQUENCIAL
                boleto.NossoNumero = "00000007102";// dadosBoleto.NossoNumero; // ESTE NÚMERO É SEQUENCIAL        
                //boleto.DigitoNossoNumero = "6";  
                boleto.DataProcessamento = DateTime.Now;//dadosBoleto.DataEmissao;
                boleto.DataDocumento = DateTime.Now;
                boleto.DataVencimento = new DateTime(2018,5,15); //dadosBoleto.DataVencimento;
                dadosBoleto.Valor = 500;dadosBoleto.Desconto = 50;
                boleto.ValorBoleto = dadosBoleto.Valor - dadosBoleto.Desconto;
                //boleto.ValorCobrado = dadosBoleto.Valor - dadosBoleto.Desconto;

                //boleto.ValorDesconto = dadosBoleto.Desconto;
                decimal multa = (dadosBoleto.Desconto * 100 / (dadosBoleto.Valor - dadosBoleto.Desconto));
                //boleto.OutrosAcrescimos = (boleto.ValorBoleto / 100 * multa);
                //boleto.DataOutrosAcrescimos = boleto.DataVencimento;
                //boleto.ValorMulta = (boleto.ValorBoleto / 100 * 2) + (boleto.ValorBoleto / 100) + (boleto.ValorBoleto / 100 * multa);
                
                //boleto.PercMulta = 3 + multa;

                boleto.Sacado = new BoletoNet.Sacado();
                boleto.Sacado.CPFCNPJ = resp.CPF;
                boleto.Sacado.Nome = resp.Nome;
                boleto.Sacado.Endereco.Logradouro = resp.Endereco.DadosEndereco.Split(' ')[0];//Utiliza o split para separar por espaço, e pegar o valor do índice 0, que corresponde o logradouro
                boleto.Sacado.Endereco.End = resp.Endereco.DadosEndereco.Replace(boleto.Sacado.Endereco.Logradouro, "");//Substitui o Logradouro por vazio
                boleto.Sacado.Endereco.Bairro = resp.Endereco.Bairro.Nome;
                boleto.Sacado.Endereco.Complemento = resp.Endereco.Complemento;
                boleto.Sacado.Endereco.Cidade = resp.Endereco.Cidade.Nome;
                boleto.Sacado.Endereco.UF = resp.Endereco.Estado.UF;
                boleto.Sacado.Endereco.CEP = resp.Endereco.Cep;
                boleto.LocalPagamento = "Até o vencimento, preferencialmente nas agências do Bradesco";
                boleto.EspecieDocumento = new BoletoNet.EspecieDocumento(237, "12");

                BoletoNet.Instrucao_Bradesco instrução = new BoletoNet.Instrucao_Bradesco();
                instrução.Descricao = "Caso o pagamento seja efetuado após a data de vencimento, o mesmo deverá ser efetuado com o acréscimo de (R$: " + string.Format("{0:0.00}",(boleto.ValorBoleto / 100 * multa)) + ") e com os devidos juros (R$: " + string.Format("{0:0.00}", (boleto.ValorBoleto / 100)) + " ao mês) e multas (R$: " + string.Format("{0:0.00}", (boleto.ValorBoleto / 100 * 2)) + " ao mês). Após 10 dias de atraso boleto sujeito a protesto.";
                boleto.Instrucoes.Add(instrução);

                #region "CodigoDeBarras"
                string campoLivre = boleto.ContaBancaria.Agencia + boleto.Carteira + boleto.NossoNumero + boleto.ContaBancaria.Conta + "0";
                boleto.CodigoBarra.CodigoBanco = "237";
                boleto.CodigoBarra.ValorDocumento = boleto.ValorBoleto.ToString();
                boleto.CodigoBarra.Moeda = 9;
                boleto.CodigoBarra.CampoLivre = campoLivre;
                boleto.CodigoBarra.FatorVencimento = 0;
                #endregion "CodigoDeBarras"

                //boleto.FormataCampos();
                boleto.Valida();

                boletoBancario.MostrarCodigoCarteira = false;
                boletoBancario.Boleto = boleto;
                boletoBancario.GeraImagemCodigoBarras(boleto);
                boletoBancario.GerarArquivoRemessa = true;
                string html = boletoBancario.MontaHtmlEmbedded();

                String enderecoBoleto = Helpers.Settings.EnderecoSistema() + "/Content/temp/boleto/" + nomeArquivo;
                
                File.WriteAllText(enderecoTemp + nomeArquivo,html);
                return enderecoBoleto;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Monta o Html do Boleto
        /// </summary>
        /// <param name="dadosBoleto">objeto do tipo Models.Boleto</param>
        /// <returns>String com o endereço do Boleto</returns>
        public String GerarRemBoleto(Prion.Generic.Models.Lista boletos, String enderecoTemp)
        {
            try
            {
                String nomeArquivo = "CB" + Helpers.DataHora.GetCurrentNow().ToString("ddMM") + "A2" + ".REM";
                //enderecoTemp, "\\Content\\temp\\boleto", nomeArquivo
                GenericRepository.Corporacao repCorporacao = new GenericRepository.Corporacao(ref this.Conexao);
                GenericModels.Corporacao corporacaoCedente = repCorporacao.BuscarPeloId(Helpers.Settings.IdCorporacao());
                Repository.AlunoResponsavel repResponsavel = new Repository.AlunoResponsavel(ref this.Conexao);
                Repository.Boleto repBoleto = new Repository.Boleto(ref this.Conexao);
                BoletoNet.BoletoBancario boletoBancario = new BoletoNet.BoletoBancario();
                BoletoNet.Boletos list = new BoletoNet.Boletos();
                BoletoNet.Boleto boleto;

                for(Int32 i=0,len = boletos.Count;i< len;i++) {
                    Models.Boleto dadosBoleto = (Models.Boleto)boletos.ListaObjetos[i];
                    Models.AlunoResponsavel resp = repResponsavel.BuscarResponsavelPorBoleto(dadosBoleto.Id);
                    boleto = new BoletoNet.Boleto();
                   
                    boleto.Banco = new BoletoNet.Banco(237);

                    boleto.Cedente = new BoletoNet.Cedente();
                    boleto.Cedente.CPFCNPJ = corporacaoCedente.CNPJ;
                    boleto.Cedente.Nome = corporacaoCedente.Nome;
                    boleto.Cedente.Convenio = dadosBoleto.Convenio;
                    boleto.Cedente.ContaBancaria = new BoletoNet.ContaBancaria();
                    boleto.Cedente.ContaBancaria.Agencia = dadosBoleto.NumeroAgencia;
                    boleto.Cedente.ContaBancaria.Conta = dadosBoleto.NumeroConta.Split('-')[0];
                    boleto.Cedente.ContaBancaria.DigitoAgencia = dadosBoleto.CodigoBanco;
                    boleto.Cedente.ContaBancaria.DigitoConta = dadosBoleto.NumeroConta.Split('-')[1];
                    boleto.Cedente.Codigo = "4999512";
                    boleto.ContaBancaria = boleto.Cedente.ContaBancaria;

                    boleto.Carteira = "09";
                    boleto.NumeroDocumento = "00000007102";//dadosBoleto.NumeroDocumento; // ESTE NÚMERO É SEQUENCIAL
                    boleto.NossoNumero = "00000007102";// dadosBoleto.NossoNumero; // ESTE NÚMERO É SEQUENCIAL        
                                                       //boleto.DigitoNossoNumero = "6";  
                    boleto.DataProcessamento = DateTime.Now;//dadosBoleto.DataEmissao;
                    boleto.DataDocumento = DateTime.Now;
                    boleto.DataVencimento = new DateTime(2018, 5, 15); //dadosBoleto.DataVencimento;
                    dadosBoleto.Valor = 500; dadosBoleto.Desconto = 50;
                    boleto.ValorBoleto = dadosBoleto.Valor - dadosBoleto.Desconto;
                    boleto.ValorCobrado = dadosBoleto.Valor - dadosBoleto.Desconto;
                    boleto.ValorDesconto = dadosBoleto.Desconto;
                    decimal multa =(dadosBoleto.Desconto * 100 / (dadosBoleto.Valor - dadosBoleto.Desconto));

                    boleto.ValorMulta = (boleto.ValorBoleto / 100 * 2) + (boleto.ValorBoleto / 100);
                    boleto.PercMulta = 3;

                    boleto.Sacado = new BoletoNet.Sacado();
                    boleto.Sacado.CPFCNPJ = resp.CPF;
                    boleto.Sacado.Nome = resp.Nome;
                    boleto.Sacado.Endereco.Logradouro = resp.Endereco.DadosEndereco.Split(' ')[0];//Utiliza o split para separar por espaço, e pegar o valor do índice 0, que corresponde o logradouro
                    boleto.Sacado.Endereco.End = resp.Endereco.DadosEndereco.Replace(boleto.Sacado.Endereco.Logradouro, "");//Substitui o Logradouro por vazio
                    boleto.Sacado.Endereco.Bairro = resp.Endereco.Bairro.Nome;
                    boleto.Sacado.Endereco.Complemento = resp.Endereco.Complemento;
                    boleto.Sacado.Endereco.Cidade = resp.Endereco.Cidade.Nome;
                    boleto.Sacado.Endereco.UF = resp.Endereco.Estado.UF;
                    boleto.Sacado.Endereco.CEP = resp.Endereco.Cep;

                    boleto.EspecieDocumento = new BoletoNet.EspecieDocumento(237, "12");

                    BoletoNet.Instrucao_Bradesco instrução = new BoletoNet.Instrucao_Bradesco();
                    instrução.Descricao = "Caso o pagamento seja efetuado após a data de vencimento, o mesmo deverá ser efetuado com o acréscimo de (R$: " + string.Format("{0:0.00}", (boleto.ValorBoleto / 100 * multa)) + ") e com os devidos juros (R$: " + string.Format("{0:0.00}", (boleto.ValorBoleto / 100)) + " ao mês) e multas (R$: " + string.Format("{0:0.00}", (boleto.ValorBoleto / 100 * 2)) + " ao mês). Após 10 dias de atraso boleto sujeito a protesto.";
                    boleto.Instrucoes.Add(instrução);

                    #region "CodigoDeBarras"
                    string campoLivre = boleto.ContaBancaria.Agencia + boleto.Carteira + boleto.NossoNumero + boleto.ContaBancaria.Conta + "0";
                    boleto.CodigoBarra.CodigoBanco = "237";
                    boleto.CodigoBarra.ValorDocumento = boleto.ValorBoleto.ToString();
                    boleto.CodigoBarra.Moeda = 9;
                    boleto.CodigoBarra.CampoLivre = campoLivre;
                    boleto.CodigoBarra.FatorVencimento = 0;
                    #endregion "CodigoDeBarras"

                    //boleto.FormataCampos();
                    //boleto.Valida();
                    list.Add(boleto);
                    repBoleto.ArquivoRemessaGerado(dadosBoleto.Id);
                }
                String endereco = Helpers.Settings.EnderecoSistema() + "/Content/temp/remessa/" + nomeArquivo;
                int sequencial = Convert.ToInt32(Sistema.GetConfiguracaoSistema("boleto_sequencial")) + 1;
                repBoleto.AtualizarNumeroSequencial(sequencial);

                using (MemoryStream memoryStr = new MemoryStream())
                {
                    var objREMESSA = new BoletoNet.ArquivoRemessa(BoletoNet.TipoArquivo.CNAB400);
                    objREMESSA.GerarArquivoRemessa(list[0].Cedente.Convenio.ToString(), list[0].Banco, list[0].Cedente, list, memoryStr, sequencial);
                    Byte[] teste = memoryStr.ToArray();
                    File.WriteAllBytes(enderecoTemp + nomeArquivo, teste);
                }

                return nomeArquivo;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Lê o arquivo de retorno do boleto pago,cada três linhas representam um boleto, e apenas a segunda linha contém as informações necessárias
        /// Por isso toda vez que se chega a terceira linha, o valor 0 é atribuído a variável
        /// </summary>        
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno LerArquivo(System.Web.HttpPostedFileBase upload)
        {
            try
            {
                Repository.Boleto repBoleto = new Repository.Boleto(ref this.Conexao);
                GenericRepository.Situacao repSituacao = new GenericRepository.Situacao(ref this.Conexao);
                GenericHelpers.Retorno retorno = new GenericHelpers.Retorno();

                if (repBoleto.VerificarSeArquivoFoiLido(upload.FileName))
                {
                    retorno.Mensagem = "Esse arquivo já foi lido pelo sistema";
                    retorno.Sucesso =  false;

                    return retorno;
                }

                Int32 situacaoMensalidade = repSituacao.BuscarPelaSituacao("Mensalidade","Quitada").Id;
                Int32 situacaoTitulo = repSituacao.BuscarPelaSituacao("Título","Quitado").Id;
                Int32 boletosPagos = 0;

                #region "old"
                //while (!String.IsNullOrEmpty(textLine = reader.ReadLine()))
                //{
                //    if (numeroLinhas < 2)
                //    {
                //        numeroLinhas++;
                //        continue;
                //    }

                //    String nossoNumero = textLine.Substring(70, 11);
                //    String codigo = textLine.Substring(109, 2);
                //    String dataPagamento = textLine.Substring(110, 6);
                //    String dataVencimento = textLine.Substring(146, 6);

                //    String valorStr = textLine.Substring(152, 13);
                //    String valorPagoStr = textLine.Substring(253, 13);

                //    //Insere uma vírgula antes dos dois últimos zeros, que representam os centavos
                //    Decimal valor = Conversor.ToDecimal(valorStr.Insert(valorStr.Length - 2, ","));
                //    Decimal valorPago = Conversor.ToDecimal(valorPagoStr.Insert(valorPagoStr.Length - 2, ","));

                //    DateTime pagamento = new DateTime(Convert.ToInt32("20" + dataPagamento.Substring(4, 2)), Convert.ToInt32(dataPagamento.Substring(2, 2)), Convert.ToInt32(dataPagamento.Substring(0, 2)));

                //    //if (valor != valorPago)
                //    //{
                //    //    continue;
                //    //}

                //    //Altera a situação de um boleto
                //    //repBoleto.QuitarBoleto(nossoNumero, valorPago, pagamento,situacaoTitulo,situacaoMensalidade);                    

                //    boletosPagos++;
                //}
                #endregion "old"

                BoletoNet.ArquivoRetornoCNAB400 cnab400 = new BoletoNet.ArquivoRetornoCNAB400();
                cnab400.LerArquivoRetorno(new BoletoNet.Banco(237), upload.InputStream);

                foreach (DetalheRetorno detalhe in cnab400.ListaDetalhe)
                {
                    repBoleto.InserirRetorno(upload.FileName, detalhe.NumeroDocumento, detalhe.DescricaoOcorrencia, detalhe.DataOcorrencia);
                    if(detalhe.ValorPago > 0)
                    {
                        //string statusBoleto = ((detalhe.ValorTitulo - detalhe.Descontos == detalhe.ValorPago)
                        //    || (detalhe.ValorTitulo == detalhe.ValorPago) || (detalhe.ValorMulta + detalhe.ValorTitulo == detalhe.ValorPago)) ? "Quitado" : "Aberto";

                        string statusBoleto = ((detalhe.DataLiquidacao < detalhe.DataVencimento && ((detalhe.ValorTitulo - detalhe.Descontos) <=detalhe.ValorPago)) 
                            ||((detalhe.DataLiquidacao > detalhe.DataVencimento && ((detalhe.ValorTitulo + detalhe.ValorMulta) <= detalhe.ValorPago)))) ? "Quitado" : "Aberto";

                        repBoleto.QuitarBoleto(detalhe.NossoNumero, detalhe.ValorPago, detalhe.DataOcorrencia, situacaoTitulo, situacaoMensalidade, statusBoleto);
                        boletosPagos++;
                    }
                }
               
                retorno.Mensagem = (boletosPagos > 0) ? "O pagamento foi efetuado" : "Nenhum pagamento foi efetuado";
                retorno.Sucesso = (boletosPagos > 0) ? true : false;


                return retorno;
            }
            catch (Exception e)
            {
                this.Conexao.Rollback();
                GenericHelpers.Retorno retorno = new GenericHelpers.Retorno();
                retorno.Mensagem = e.Message;
                retorno.Sucesso = false;
                return retorno;
            }
        }

        /// <summary>
        /// Monta o pdf a ser enviado por email
        /// </summary>
        /// <returns></returns>
        public void MontarPdfRelatorio(DataTable dt, String urlRelatorio)
        {
            try
            {
                Document document = new Document();

                if (System.IO.File.Exists(urlRelatorio))
                {
                    System.IO.File.Delete(urlRelatorio);
                }

                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(urlRelatorio, FileMode.Create));
                document.Open();
                iTextSharp.text.Font font5 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 5);
                Int32 iteração = 1;
              
                PdfPTable table = new PdfPTable(dt.Columns.Count + 1);
                Paragraph titulo = new Paragraph();

                table.WidthPercentage = 100;
                //table.KeepTogether = true;//Impede que a tabela quebre


                titulo.Add("Parcelas à Vencer");
                titulo.Alignment = Element.ALIGN_CENTER;
                document.Add(titulo);
                document.Add(Chunk.NEWLINE);


                foreach (DataColumn c in dt.Columns)
                {
                    table.AddCell(new Phrase(c.ColumnName, font5));
                }

                //Adiciona uma coluna de Total
                table.AddCell(new Phrase("Total", font5));
                Decimal total = 0;
                String numeroContrato = "";

                foreach (DataRow r in dt.Rows)
                {
                    for (Int32 i = 0, len = dt.Columns.Count; i < len; i++)
                    {
                        if (r[i].GetType().Name.ToLower() == "decimal")
                        {
                            table.AddCell(new Phrase(Convert.ToDecimal(r[i]).ToString("C", CultureInfo.CreateSpecificCulture("pt-BR")), font5));
                            if (r[0].ToString() != numeroContrato)
                            {
                                total += Convert.ToDecimal(r[i]);
                                numeroContrato = r[0].ToString();
                            }
                        }
                        else if (r[i].GetType().Name.ToLower() == "datetime")
                        {
                            table.AddCell(new Phrase(Convert.ToDateTime(r[i]).ToString("d"), font5));
                        }
                        else
                        {
                            table.AddCell(new Phrase(r[i].ToString(), font5));
                        }
                    }

                    table.AddCell(new Phrase(total.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR")), font5));

                }

                document.Add(table);
                document.Add(Chunk.NEWLINE);
                iteração++;

                document.Close();

            }
            catch (Exception e)
            {
                throw e;
            }
        }       

        public void QuitarManualmente(Int32[] ids)
        {
            Repository.Boleto repBoleto = new Repository.Boleto(ref this.Conexao);
            repBoleto.QuitarBoletoManualmente(ids);
        }

        public void CancelarManualmente(Int32[] ids)
        {
            Repository.Boleto repBoleto = new Repository.Boleto(ref this.Conexao);
            repBoleto.CancelarBoletoManualmente(ids);
        }

        public void AtualizarValorBoleto(Int32 idBoleto, decimal valor, decimal desconto, bool replicar)
        {
            Repository.Boleto repBoleto = new Repository.Boleto(ref this.Conexao);
            List<Int32> ids = new List<Int32>();
            ids.Add(idBoleto);

            if (replicar)
            {
                ids = repBoleto.BuscarProximosBoletos(idBoleto).ToList();
            }

            repBoleto.AtualizarValorBoletos(ids.ToArray(), valor, desconto);
        }

        /// <summary>
        /// Valida os atributos obrigatórios de uma Model.Boleto
        /// </summary>
        /// <param name="Boleto">Objeto do tipo Model.Boleto</param>
        private void Validar(Models.Boleto Boleto)
        {
            if (Boleto == null)
            {
                throw new Prion.Tools.PrionException("Objeto não definido.");
            }

            if (Boleto.Valor == 0)
            {
                throw new Prion.Tools.PrionException("O valor do Boleto não pode ser 0.");
            }

            if ((Boleto.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Alterado) && (Boleto.Id == 0))
            {
                throw new Prion.Tools.PrionException("O id do Boleto não pode ser vazio.");
            }
        }
    }
}