using Prion.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using GenericHelpers = Prion.Generic.Helpers;

namespace NotaAzul.Controllers
{
    public class BoletoController:BaseController
    {

        /// <summary>
        /// Retorna a view Lista, com os registros de Boleto
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewLista()
        {
            return View("Lista");
        }


        /// <summary>
        /// Retorna a view Dados, com os registros de Boleto
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewDados()
        {
            return View("Dados");
        }

        /// <summary>
        /// Retorna a view Dados, com os registros de Boleto
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewRemessa()
        {
            return View("DadosRemessa");
        }

        public ActionResult ViewDadosBoleto()
        {
            return View("DadosBoleto");
        }       

       

        public FileResult Download(string fileName)
        {
            String endereco = Server.MapPath("~/Content/temp/remessa/");
            Byte[] file = System.IO.File.ReadAllBytes(endereco + fileName);

            return File(file, "application/x-msdownload",fileName);
        }

        /// <summary>
        /// Carrega um registro do tipo Models.Bairro atráves do seu ID
        /// </summary>
        /// <param name="id">id do Bairro</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
            Business.Boleto biBoleto = new Business.Boleto();
            Models.Boleto boleto = (Models.Boleto)biBoleto.Carregar(false,id).Get(0);

            return Json(new { success = true, obj = boleto }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Faz o upload do arquivo de retorno
        /// </summary>        
        /// <returns></returns>
        public ActionResult UploadArquivoRetorno(System.Web.HttpPostedFileBase upload)
        {
            try
            {
                Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
                Business.Boleto biBoleto = new Business.Boleto();
                GenericHelpers.Retorno retorno = biBoleto.LerArquivo(upload);

                mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
                mensagem.TextoMensagem = retorno.Mensagem;
                mensagem.Sucesso = retorno.Sucesso;

                return Json(new { success = mensagem.Sucesso, mensagem = mensagem, ultimoId = mensagem.UltimoId });
            }
            catch (Exception e)
            {
                String msg = Helpers.Log.Salvar(e, "Erro ao fazer upload do arquivo de retorno", Request);
                Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
                mensagem.TextoMensagem = msg;
                mensagem.Sucesso = false;

                return Json(new { success = false, mensagem = mensagem });
            }
        }

        public ActionResult QuitarBoletosManualmente(string idsStr)
        {
            try
            {
                Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
                Business.Boleto biBoleto = new Business.Boleto();
                List<int> ids = new List<int>();
                string[] arr = idsStr.Split(',');
                for (int i = 0; i < arr.Length; i++)
                {
                    ids.Add(Convert.ToInt32(arr[i]));
                }

                biBoleto.QuitarManualmente(ids.ToArray());

                return Json(new { success = true, mensagem = ids.Count.ToString() + " boleto(s) quitado(s) manualmente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                String msg = Helpers.Log.Salvar(e, "Erro ao gerar boleto", Request);
                Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
                mensagem.TextoMensagem = msg;
                mensagem.Sucesso = false;

                return Json(new { success = false, mensagem = mensagem }, JsonRequestBehavior.AllowGet);
            }
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
            f = new Prion.Tools.Request.Filtro("Boleto.StatusBoleto", "<>", "'Cancelado'");
            parametros.Filtro.Add(f);

            parametros.Paginar = false;
            return parametros;
        }

        public ActionResult Salvar(Int32 idBoleto,decimal valor,decimal desconto,bool replicar)
        {
            Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
            Business.Boleto biBoleto = new Business.Boleto();
            biBoleto.AtualizarValorBoleto(idBoleto, valor, desconto, replicar);

            return Json(new { success = true });
        }

        public ActionResult GetLista()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.Boleto biBoleto = new Business.Boleto();
            parametros = JsonFiltro(Request.Form["Query"], parametros);
            Prion.Generic.Models.Lista lista = biBoleto.Lista(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }
       
    }
}