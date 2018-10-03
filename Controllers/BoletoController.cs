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


        /// <summary>
        /// Gera o html e salva o objeto de um boleto
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult GerarBoleto(Int32 idBoleto)
        {
            try
            {
                Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
                Business.Boleto biBoleto = new Business.Boleto();

                Models.Boleto boleto = (Models.Boleto)biBoleto.Carregar(false,idBoleto).Get(0);
                String enderecoBoleto = biBoleto.GerarHtmlBoleto(boleto, Server.MapPath("~/Content/temp/boleto/"));

                return Json(new { success = true, enderecoBoleto = enderecoBoleto }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                String msg = Helpers.Log.Salvar(e, "Erro ao gerar boleto", Request);
                Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
                mensagem.TextoMensagem = e.Message;
                mensagem.Sucesso = false;

                return Json(new { success = false, mensagem = mensagem }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Gera o html e salva o objeto de um boleto
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult ImprimirBoletoPorAluno()
        {
            try
            {
                Int32 idAluno = Convert.ToInt32(Request.Form["IdAluno"]);
                Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
                Business.Boleto biBoleto = new Business.Boleto();
                List<string> enderecos = new List<string>();

                List<Models.Boleto> boletos = ListTo.CollectionToList<Models.Boleto>(biBoleto.CarregarPorAluno(idAluno).ListaObjetos);
                foreach (Models.Boleto boleto in boletos)
                {
                   enderecos.Add(biBoleto.GerarHtmlBoleto(boleto, Server.MapPath("~/Content/temp/boleto/")));
                }
                return Json(new { success = true, enderecos = enderecos.ToArray() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                String msg = Helpers.Log.Salvar(e, "Erro ao gerar boleto", Request);
                Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
                mensagem.TextoMensagem = e.Message;
                mensagem.Sucesso = false;

                return Json(new { success = false, mensagem = mensagem }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Gera o html e salva o objeto de um boleto
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult GerarArquivoRem(string idsStr)
        {
            try
            {
                Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
                Business.Boleto biBoleto = new Business.Boleto();
                List<int> ids = new List<int>();
                string[] arr = idsStr.Split(',');
                for(int i=0; i < arr.Length; i++)
                {
                    ids.Add(Convert.ToInt32(arr[i]));
                }
                Prion.Generic.Models.Lista boletos = biBoleto.Carregar(true,ids.ToArray());
                if (boletos.Count < 1)
                {
                    Prion.Generic.Helpers.Mensagem erro = new Prion.Generic.Helpers.Mensagem();
                    erro.TextoMensagem = "O arquivo remessa já havia sido gerado para os boletos selecionados";
                    erro.Sucesso = false;

                    return Json(new { success = false, mensagem = erro }, JsonRequestBehavior.AllowGet);
                }
                String enderecoBoleto = biBoleto.GerarRemBoleto(boletos, Server.MapPath("~/Content/temp/remessa/"));

                return Json(new { success = true, enderecoBoleto = enderecoBoleto }, JsonRequestBehavior.AllowGet);
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
        /// Gera o html e salva o objeto de um boleto
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult GerarArquivoMes(int mes)
        {
            try
            {
                Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
                Business.Boleto biBoleto = new Business.Boleto();
               
                Prion.Generic.Models.Lista boletos = biBoleto.BoletosMensal(mes,true);
                if (boletos.Count < 1)
                {
                    Prion.Generic.Helpers.Mensagem erro = new Prion.Generic.Helpers.Mensagem();
                    erro.TextoMensagem = "O arquivo remessa já havia sido gerado para os boletos selecionados";
                    erro.Sucesso = false;

                    return Json(new { success = false, mensagem = erro }, JsonRequestBehavior.AllowGet);
                }

                String nomeArquivo = biBoleto.GerarRemBoleto(boletos, Server.MapPath("~/Content/temp/remessa/"));

                return Json(new { success = true, nomeArquivo = nomeArquivo }, JsonRequestBehavior.AllowGet);
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

        public FileResult Download(string fileName)
        {
            String endereco = Server.MapPath("~/Content/temp/remessa/");
            Byte[] file = System.IO.File.ReadAllBytes(endereco + fileName);

            return File(file, "application/x-msdownload",fileName);
        }

        /// <summary>
        /// Retornar uma lista no formato JSON do tipo Models.Aluno
        /// </summary>
        /// <returns></returns>
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

        public ActionResult CancelarBoletosManualmente(string idsStr)
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

                biBoleto.CancelarManualmente(ids.ToArray());

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
    }
}