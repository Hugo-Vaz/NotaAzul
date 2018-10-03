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
using HtmlAgilityPack;

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
                    retorno.Sucesso = false;

                    return retorno;
                }
                string htmlString = string.Empty;

                using (StreamReader stream = new StreamReader(upload.InputStream))
                {
                    htmlString = stream.ReadToEnd();
                }
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(htmlString);

                List<Models.BoletoQuitado> boletosOperacao = this.PegarBoletosDoArquivoRemessa(document);

                Int32 mensalidadeQuitadaStatus = repSituacao.BuscarPelaSituacao("Mensalidade","Quitada").Id,
                    mensalidadeAbertaStatus = repSituacao.BuscarPelaSituacao("Mensalidade", "Aberta").Id,
                    boletosPagos = 0;
              
                

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

        private List<Models.BoletoQuitado> PegarBoletosDoArquivoRemessa(HtmlDocument document)
        {
            List<Models.BoletoQuitado> boletos = new List<Models.BoletoQuitado>();

            var tables = document.DocumentNode.SelectNodes("//*[contains(@class,'pdf2xl')]");

            //Quote Info
            var table = tables[0];
            var rows = table.Descendants("tr")
                            .Select(n => n.Elements("td").Select(e => e.InnerText).ToArray());

            return boletos;
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