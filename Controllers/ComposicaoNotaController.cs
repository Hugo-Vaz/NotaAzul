using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using GenericHelpers = Prion.Generic.Helpers;

namespace NotaAzul.Controllers
{
    public class ComposicaoNotaController:BaseController
    {
        #region "Views"

        /// <summary>
        /// Retorna a view Novo
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewDados()
        {
            ViewData["FormaDeConceito"] = Sistema.GetConfiguracaoSistema("notas:forma_conceito");
            ViewData["DivisaoAnoLetivo"] = Sistema.GetConfiguracaoSistema("notas:divisao_ano_letivo");
            return View("Dados");
        }

        /// <summary>
        /// Retorna a view Novo
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewLista()
        {
            return View("Lista");
        }

        #endregion "Views"

        #region "Requisições"

        /// <summary>
        /// Carrega um registro do tipo Models.ComposicaoNota atráves do seu ID
        /// </summary>
        /// <param name="id">id do ComposicaoNota</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);

            Prion.Tools.Request.Filtro f = new Prion.Tools.Request.Filtro("ComposicaoNotaPeriodo.IdProfessorDisciplina", "=", id);
            parametros.Filtro.Add(f);

            Business.ComposicaoNotaPeriodo biComposicaoNota = new Business.ComposicaoNotaPeriodo();
            Models.ComposicaoNotaPeriodo composicaoNota = (Models.ComposicaoNotaPeriodo)biComposicaoNota.Carregar(parametros).Get(0);

            return Json(new { success = true, obj = composicaoNota }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Carrega um registro do tipo Models.ComposicaoNota atráves do seu ID
        /// </summary>
        /// <param name="id">id do ComposicaoNota</param>
        /// <returns></returns>
        public ActionResult CarregarPorBimestre()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);          

            Business.ComposicaoNotaPeriodo biComposicaoNota = new Business.ComposicaoNotaPeriodo();
            Models.ComposicaoNotaPeriodo composicaoNota = (Models.ComposicaoNotaPeriodo)biComposicaoNota.Carregar(parametros).Get(0);

            return Json(new { success = true, obj = composicaoNota }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retornar uma lista no formato JSON 
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLista()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.Disciplina biDisciplina= new Business.Disciplina();
            Models.Usuario usuarioLogado = (Models.Usuario)Session["UsuarioLogado"];

            Prion.Tools.Request.Filtro f = new Prion.Tools.Request.Filtro("Usuario.Id", "=",usuarioLogado.Id );
            parametros.Filtro.Add(f);

            Prion.Generic.Models.Lista lista = biDisciplina.ListaDisciplinas(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Salva os dados de uma ComposicaoNota
        /// </summary>        
        /// <returns></returns>
        public ActionResult Salvar()
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.ComposicaoNotaPeriodo biComposicaoNota = new Business.ComposicaoNotaPeriodo();


            Models.ComposicaoNotaPeriodo composicaoNota = JsonComposicaoNota(Request.Form["ComposicaoNota"]);
            GenericHelpers.Retorno retorno = biComposicaoNota.Salvar(composicaoNota);

            mensagem.EstadoObjeto = retorno.EstadoObjeto;
            mensagem.UltimoId = retorno.UltimoId;
            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Transforma um JSON em um objeto de Model.ComposicaoNota
        /// </summary>
        /// <param name="json">string no formato JSON</param>
        /// <returns>Um objeto do tipo Models.ComposicaoNota</returns>
        private Models.ComposicaoNotaPeriodo JsonComposicaoNota(String json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            js.RegisterConverters(new JavaScriptConverter[] { new Prion.Tools4.DynamicJsonConverter() });
            dynamic result = js.Deserialize(json, typeof(object)) as dynamic;
            Models.ComposicaoNotaPeriodo composicaoNotaPeriodo = new Models.ComposicaoNotaPeriodo();

            composicaoNotaPeriodo.IdProfessorDisciplina = Convert.ToInt32(result.IdProfessorDisciplina);
            composicaoNotaPeriodo.Id = Convert.ToInt32(result.Id);
            composicaoNotaPeriodo.FormaDivisaoAnoLetivo = result.FormaDivisao;
            composicaoNotaPeriodo.PeriodoDeAvaliacao =  Convert.ToInt32(result.PeriodoAvaliacao);

            for (Int32 i = 0; i < result.Notas.Count; i++)
            {
                Models.ComposicaoNota composicaoNota = new Models.ComposicaoNota();
                composicaoNota.Id = Convert.ToInt32(result.Notas[i]["Id"]);
                composicaoNota.IdComposicaoNotaPeriodo = composicaoNotaPeriodo.Id;
                composicaoNota.Peso = Convert.ToDecimal(result.Notas[i]["Peso"]);
                composicaoNota.EstadoObjeto = ((Prion.Generic.Helpers.Enums.EstadoObjeto)Convert.ToInt32(result.Notas[i]["EstadoObjeto"]) == GenericHelpers.Enums.EstadoObjeto.Excluido)
                        ? GenericHelpers.Enums.EstadoObjeto.Excluido
                        : GenericHelpers.Enums.EstadoObjeto.Novo;
                for (Int32 j = 0; j < result.Notas[i]["FormasAvaliacao"].Count; j++)
                {
                    Models.FormaDeAvaliacao formaDeAvaliacao = new Models.FormaDeAvaliacao();
                    formaDeAvaliacao.Id = Convert.ToInt32(result.Notas[i]["FormasAvaliacao"][j]["Id"]);
                    formaDeAvaliacao.IdComposicaoNota = composicaoNota.Id;
                    formaDeAvaliacao.Tipo = result.Notas[i]["FormasAvaliacao"][j]["formaAvaliacao"];
                    formaDeAvaliacao.Valor = Convert.ToDecimal(result.Notas[i]["FormasAvaliacao"][j]["Valor"]);
                    formaDeAvaliacao.DataAvaliacao = Convert.ToDateTime(result.Notas[i]["FormasAvaliacao"][j]["DataAvaliacao"]);
                    composicaoNota.ListaFormasDeAvaliacao.Add(formaDeAvaliacao);
                }

                composicaoNotaPeriodo.ListaNotas.Add(composicaoNota);
            }

            return composicaoNotaPeriodo;
        }


        #endregion
    }
}
   