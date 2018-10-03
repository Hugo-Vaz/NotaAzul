using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using GenericHelpers = Prion.Generic.Helpers;

namespace NotaAzul.Controllers
{
    public class ContasPagarController:BaseController
    {
        #region "Views"
        /// <summary>
        /// Retorna a view Lista, com os registros de ContasPagar
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewLista()
        {
            return View("Lista");
        }

        /// <summary>
        /// Retorna a view Novo
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewDados()
        {
            ViewModels.ContasPagar.vmDados vmDados = new ViewModels.ContasPagar.vmDados();
           
            return View("Dados", vmDados);
        }

        /// <summary>
        /// Retorna a view Pagamento
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewPagamento()
        {
            return View("Pagamento");
        }

        /// <summary>
        /// Retorna a view FormaPagamento
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewFormaPagamento()
        {
            return View("FormasPagamento");
        }
        #endregion "Views"


        #region "Requisições"

        /// <summary>
        /// Carrega um registro do tipo Models.ContasPagar através do seu Id
        /// </summary>
        /// <param name="id">id de ContasPagar</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
            Business.ContasPagar biContasPagar = new Business.ContasPagar();
            Prion.Generic.Models.Titulo titulo = (Prion.Generic.Models.Titulo)biContasPagar.Carregar(id).Get(0);

            return Json(new { success = true, obj = titulo }, JsonRequestBehavior.AllowGet);
        }       

        /// <summary>
        /// Salva os dados de uma ContasPagar
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult Salvar(ViewModels.ContasPagar.vmDados vModel)
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.ContasPagar biContasPagar = new Business.ContasPagar();

            // obtém uma lista de títulos que estão sendo pagos 
            List<Prion.Generic.Models.Titulo> listaTitulos = JsonTitulos(Request.Form["titulosPagos"]);
            GenericHelpers.Retorno retorno = biContasPagar.Salvar(listaTitulos);

            mensagem.EstadoObjeto = retorno.EstadoObjeto;
            mensagem.UltimoId = retorno.UltimoId;
            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Cancela um pagamento de uma conta pelo ID
        /// </summary>
        /// <param name="id">id do Título</param>
        /// <returns>retorno no formato JSON</returns>
        public ActionResult Excluir()
        {
            Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();

            Business.ContasPagar biContasPagar = new Business.ContasPagar();
            GenericHelpers.Retorno retorno = biContasPagar.Excluir(Prion.Tools.Conversor.ToInt(Request.Form["id"]));

            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = (retorno.Sucesso) ? "O pagamento foi cancelado e os títulos voltaram a ficar em abertos." : retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Retorna uma lista no formato JSON do tipo Models.ContasPagar
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLista()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.ContasPagar biContasPagar = new Business.ContasPagar();

            Prion.Generic.Models.Lista lista = biContasPagar.ListaContasPagar(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Transforma um JSON de titulos em uma lista de objetos do tipo Prion.Generic.Models.Titulo
        /// </summary>
        /// <param name="json">string no formato JSON</param>
        /// <returns>Lista de objetos do tipo Prion.Generic.Models.Titulo</returns>
        private List<Prion.Generic.Models.Titulo> JsonTitulos(String json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            js.RegisterConverters(new JavaScriptConverter[] { new Prion.Tools4.DynamicJsonConverter() });
            dynamic result = js.Deserialize(json, typeof(object)) as dynamic;
            List<Prion.Generic.Models.Titulo> listaTitulos = new List<Prion.Generic.Models.Titulo>();
            

            for (Int32 i = 0; i < result.Length; i++)
            {
                Prion.Generic.Models.Titulo titulo = new Prion.Generic.Models.Titulo();
                titulo.Id = Convert.ToInt32(result[i].Id);
                titulo.IdSituacao = Convert.ToInt32(result[i].IdSituacao);
                titulo.IdCorporacao = Sistema.UsuarioLogado.IdCorporacao;
                titulo.IdTituloTipo = Convert.ToInt32(result[i].IdTituloTipo);
                titulo.EstadoObjeto = (Prion.Generic.Helpers.Enums.EstadoObjeto)result[i].EstadoObjeto;
                titulo.Descricao = result[i].Descricao;                
                titulo.Numero = result[i].Numero;
                titulo.Observacao = result[i].Observacao;
                titulo.Valor = Convert.ToDecimal(result[i].Valor);
                titulo.Acrescimo = Convert.ToDecimal(result[i].Acrescimo);
                titulo.Desconto = Convert.ToDecimal(result[i].Desconto);
                titulo.DataVencimento = Prion.Tools.Conversor.MilisecondsToDateTime(result[i].DataVencimento);
                titulo.DataOperacao = Convert.ToDateTime(result[i].DataOperacao);
                titulo.TipoOperacao = (GenericHelpers.Enums.TipoOperacao)Convert.ToInt32(result[i].TipoOperacao);
                titulo.ValorPago = Convert.ToDecimal(result[i].ValorPago);
                int totalFormasPagamento = result[i].FormasPagamento.Count;

                for (Int32 j = 0; j < totalFormasPagamento; j++)
                {
                    String tipoPagamento = result[i].FormasPagamento[j]["Tipo"];

                    switch (tipoPagamento.ToLower())
                    {
                        case "cheque":  
                            Prion.Generic.Models.Cheque cheque = new Prion.Generic.Models.Cheque();
                            cheque.Valor = result[i].FormasPagamento[j]["Valor"];
                            cheque.Agencia = result[i].FormasPagamento[j]["Cheque.Agencia"];
                            cheque.Alinea = result[i].FormasPagamento[j]["Cheque.Alinea"];
                            cheque.Banco = result[i].FormasPagamento[j]["Cheque.Banco"];
                            cheque.BomPara = Convert.ToDateTime(result[i].FormasPagamento[j]["Cheque.BomPara"]);
                            cheque.ContaCorrente = result[i].FormasPagamento[j]["Cheque.ContaCorrente"];
                            cheque.IdCorporacao = Sistema.UsuarioLogado.IdCorporacao;
                            cheque.IdResponsavel = Convert.ToInt32(result[i].FormasPagamento[j]["Cheque.IdResponsavel"]);
                            cheque.NomeEmissor = result[i].FormasPagamento[j]["Cheque.NomeEmissor"];
                            cheque.Numero = result[i].FormasPagamento[j]["Cheque.Numero"];
                            cheque.Proprio =Convert.ToBoolean( result[i].FormasPagamento[j]["Cheque.Proprio"]);
                            cheque.TelefoneEmissor = result[i].FormasPagamento[j]["Cheque.TelefoneEmissor"];
                            
                           titulo.ListaCheque.Add(cheque);
                        
                            break;
                        case "cartão":
                            Prion.Generic.Models.Cartao cartao = new Prion.Generic.Models.Cartao();
                            cartao.Valor = result[i].FormasPagamento[j]["Valor"];
                            cartao.Bandeira = result[i].FormasPagamento[j]["Cartao.Bandeira"];

                            titulo.ListaCartao.Add (cartao);

                            break;
                        case "espécie":
                            Prion.Generic.Models.Especie especie = new Prion.Generic.Models.Especie();
                            especie.Valor = result[i].FormasPagamento[j]["Valor"];

                            titulo.ListaEspecie.Add(especie);

                            break;
                    }

                }

                listaTitulos.Add(titulo);
            }

            return listaTitulos;
        }

        #endregion
    }
}