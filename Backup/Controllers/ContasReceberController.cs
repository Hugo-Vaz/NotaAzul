using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using GenericHelpers = Prion.Generic.Helpers;

namespace NotaAzul.Controllers
{
    public class ContasReceberController:BaseController
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


        /// <summary>
        /// Retorna a view MensalidadesPagas
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewMensalidadesPagas()
        {
            return View("MensalidadesPagas");
        }

        /// <summary>
        /// Retorna a view Recibo
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewRecibo()
        {
            ViewModels.Corporacao.vmDados vmDadosCorporacao = new ViewModels.Corporacao.vmDados();

            Business.Corporacao biCorporacao = new Business.Corporacao();
            Business.Bairro biBairro = new Business.Bairro();
            Business.Cidade biCidade = new Business.Cidade();

            Int32 idEstado = Prion.Tools.Conversor.ToInt32(Sistema.GetConfiguracaoSistema("estado:id_uf_default"));
            // carregar o estado usando a variável idEstado

            vmDadosCorporacao.Corporacao = biCorporacao.Carregar(Sistema.UsuarioLogado.IdCorporacao);
            vmDadosCorporacao.Corporacao.Bairro = (Prion.Generic.Models.Bairro)biBairro.Carregar(vmDadosCorporacao.Corporacao.IdBairro).Get(0);
            vmDadosCorporacao.Corporacao.Cidade = (Prion.Generic.Models.Cidade)biCidade.Carregar(vmDadosCorporacao.Corporacao.IdCidade).Get(0);
            vmDadosCorporacao.Corporacao.Estado.UF = "RJ"; // TMZ: mudar para usar o objeto de estado carregado logo acima

            return View("Recibo", vmDadosCorporacao);
        }
        #endregion "Views"        

        #region "Requisições"

        /// <summary>
        /// Carrega uma lista de registros do tipo Models.Mensalidade através do seu Id
        /// </summary>
        /// <param name="id">id de ContasPagar</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
            Business.ContasReceber biContasReceber = new Business.ContasReceber();
            List<Models.MensalidadeTitulo> listaMensalidades = biContasReceber.Carregar(id);

            return Json(new { success = true, obj = listaMensalidades }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Carrega uma lista de registros do tipo Models.Mensalidade através do seu Id
        /// </summary>
        /// <param name="id">id de ContasPagar</param>
        /// <returns></returns>
        public ActionResult CarregarPagamento(Int32 id)
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);

            Prion.Tools.Request.Filtro f = new Prion.Tools.Request.Filtro("Mensalidade.Id", "=", id);
            parametros.Filtro.Add(f);

            Business.ContasReceber biContasReceber = new Business.ContasReceber();
            Prion.Generic.Models.Lista lista = biContasReceber.CarregarMensalidadePaga(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, obj = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Carrega uma lista de registros do tipo Models.Mensalidade,que já foram pagas
        /// </summary>
        /// <param name="id">id de ContasPagar</param>
        /// <returns></returns>
        public ActionResult GetListaMensalidadesPagas()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.ContasReceber biContasReceber = new Business.ContasReceber();
            parametros = JsonFiltro(Request.Form["Query"], parametros);
            Prion.Generic.Models.Lista lista = biContasReceber.CarregarMensalidadesPagas(parametros);

            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }       


        /// <summary>
        /// Retorna uma lista no formato JSON do tipo Models.ContasPagar
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLista()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.ContasReceber biContasReceber = new Business.ContasReceber();
            parametros = JsonFiltro(Request.Form["Query"], parametros);
            Prion.Generic.Models.Lista lista = biContasReceber.ListaContasReceber(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Salva os dados de uma ContasReceber
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult Salvar(ViewModels.ContasPagar.vmDados vModel)
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.ContasReceber biContasReceber = new Business.ContasReceber();

            // obtém uma lista de títulos que estão sendo pagos 
            List<Models.MensalidadeTitulo> listaMensalidadesAPagar = JsonMensalidades(Request.Form["MensalidadesPagas"]);
            GenericHelpers.Retorno retorno = biContasReceber.Salvar(listaMensalidadesAPagar);

            mensagem.EstadoObjeto = retorno.EstadoObjeto;
            mensagem.UltimoId = retorno.UltimoId;
            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Cancela um pagamento de uma mensalidade pelo ID
        /// </summary>
        /// <param name="id">id da Mensalidade</param>
        /// <returns>retorno no formato JSON</returns>
        public ActionResult Excluir()
        {
            Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();

            Business.ContasReceber biContasReceber = new Business.ContasReceber();
            GenericHelpers.Retorno retorno = biContasReceber.Excluir(Prion.Tools.Conversor.ToInt(Request.Form["Id"]));

            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = (retorno.Sucesso) ? "O pagamento foi cancelado e as mensalidades voltaram a ficar em aberto." : retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

         /// <summary>
        /// Transforma um JSON de titulos em uma lista de objetos do tipo Prion.Generic.Models.Titulo
        /// </summary>
        /// <param name="json">string no formato JSON</param>
        /// <returns>Lista de objetos do tipo Prion.Generic.Models.Titulo</returns>
        private List<Models.MensalidadeTitulo> JsonMensalidades(String json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            js.RegisterConverters(new JavaScriptConverter[] { new Prion.Tools4.DynamicJsonConverter() });
            dynamic result = js.Deserialize(json, typeof(object)) as dynamic;
            List<Models.MensalidadeTitulo> listaMensalidadeTitulo = new List<Models.MensalidadeTitulo>();


            for (Int32 i = 0; i < result.Length; i++)
            {
                Models.MensalidadeTitulo mensalidadeTitulo = new Models.MensalidadeTitulo();
                int totalFormasPagamento = result[i].FormasPagamento.Count;
                int totalMensalidadesPagas = result[i].Mensalidades.Count;
                mensalidadeTitulo.MensalidadesPagas = new Int32[totalMensalidadesPagas];
                mensalidadeTitulo.TitulosPagos = new Int32[totalMensalidadesPagas];
                mensalidadeTitulo.ValorTotal = result[i].ValorTotal;
                mensalidadeTitulo.ValorPago = result[i].ValorPago;
                mensalidadeTitulo.DataOperacao = Convert.ToDateTime(result[i].DataOperacao);

                for (Int32 j = 0; j < totalMensalidadesPagas; j++)
                {
                    mensalidadeTitulo.TitulosPagos[j] = Convert.ToInt32(result[i].Mensalidades[j]["object"]["IdTitulo"]);            
                    mensalidadeTitulo.MensalidadesPagas[j] = Convert.ToInt32(result[i].Mensalidades[j]["object"]["Id"]);
                }

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
                            cheque.Proprio = Convert.ToBoolean(result[i].FormasPagamento[j]["Cheque.Proprio"]);
                            cheque.TelefoneEmissor = result[i].FormasPagamento[j]["Cheque.TelefoneEmissor"];

                            mensalidadeTitulo.ListaCheque.Add(cheque);

                            break;
                        case "cartão":
                            Prion.Generic.Models.Cartao cartao = new Prion.Generic.Models.Cartao();
                            cartao.Valor = result[i].FormasPagamento[j]["Valor"];
                            cartao.Bandeira = result[i].FormasPagamento[j]["Cartao.Bandeira"];

                            mensalidadeTitulo.ListaCartao.Add(cartao);

                            break;
                        case "espécie":
                            Prion.Generic.Models.Especie especie = new Prion.Generic.Models.Especie();
                            especie.Valor = result[i].FormasPagamento[j]["Valor"];

                            mensalidadeTitulo.ListaEspecie.Add(especie);

                            break;
                    }

                }

                listaMensalidadeTitulo.Add(mensalidadeTitulo);
            }

            return listaMensalidadeTitulo;
        }


        /// <summary>
        /// Transforma um JSON contendo os filtros para montar o relatório em Request.Filtro
        /// </summary>
        /// <param name="json">string no formato JSON</param>
        /// <param name="parametros"></param>
        /// <returns>Um ParametroRequest com seus respectivos filtros adicionados</returns>
        private Prion.Tools.Request.ParametrosRequest JsonFiltro(String json, Prion.Tools.Request.ParametrosRequest parametros)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            Prion.Tools.Request.Filtro f = null;
            if (json == null) { return parametros; }
            js.RegisterConverters(new JavaScriptConverter[] { new Prion.Tools4.DynamicJsonConverter() });
            dynamic result = js.Deserialize(json, typeof(object)) as dynamic;            

            for (Int32 i = 0; i < result.Filtros.Count; i++)
            {
                String campo = result.Filtros[i]["Campo"];
                String operador = result.Filtros[i]["Operador"];
                String valor = result.Filtros[i]["Valor"];
                f = new Prion.Tools.Request.Filtro(campo, operador, valor);
                parametros.Filtro.Add(f);
            }
            return parametros;
        }
        #endregion "Requisições"
    }
}