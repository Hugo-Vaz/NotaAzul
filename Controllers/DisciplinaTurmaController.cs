using System;
using System.Web.Mvc;
using GenericHelpers = Prion.Generic.Helpers;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace NotaAzul.Controllers
{
    public class DisciplinaTurmaController : BaseController
    {
        #region "Views"

        /// <summary>
        /// Retorna a view Novo
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewDados()
        {            
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
        /// Carrega um registro do tipo Models.Turma atráves do seu ID
        /// </summary>
        /// <param name="id">id da Turma</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);

            Business.Turma biTurma = new Business.Turma();
            Models.Turma turma = (Models.Turma)biTurma.Carregar(parametros, id).Get(0);

            return Json(new { success = true, obj = turma }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retornar uma lista no formato JSON do tipo Models.Aluno
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLista()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.Turma biTurma = new Business.Turma();
            String anoLetivo = Request.QueryString["AnoLetivo"];

            Prion.Tools.Request.Filtro f = new Prion.Tools.Request.Filtro("CursoAnoLetivo.AnoLetivo","=",anoLetivo);
            parametros.Filtro.Add(f);

            Prion.Generic.Models.Lista lista = biTurma.ListaTurmas(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Associa as disciplinas a determinada Turma
        /// </summary>        
        /// <returns></returns>
        public ActionResult Salvar()
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.Turma biTurma = new Business.Turma();
            
            Int32[] idsDisciplinas = JsonDisciplinas(Request.Form["Ids"]);
            Int32 idTurma = Convert.ToInt32(Request.Form["Turma.Id"]);
            //Converte o json com o estado dos objetos correspondentes a disciplina
            Int32[] estados = JsonEstadosObjeto(Request.Form["Ids"]);

            GenericHelpers.Retorno retorno = biTurma.CriarRelacionamento(idTurma, idsDisciplinas, estados);

            mensagem.EstadoObjeto = retorno.EstadoObjeto;
            mensagem.UltimoId = retorno.UltimoId;
            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

         /// <summary>
        /// Transforma um JSON de ids em um array
        /// </summary>
        /// <param name="json">string no formato JSON</param>
        /// <returns>Array com ids das disciplinas</returns>
        private Int32[] JsonDisciplinas(String json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            js.RegisterConverters(new JavaScriptConverter[] { new Prion.Tools4.DynamicJsonConverter() });
            dynamic result = js.Deserialize(json, typeof(object)) as dynamic;

            Int32 tamanhoArray = result.Ids.Count;
            Int32[] idsDisciplina = new Int32[tamanhoArray];            

            for (Int32 i = 0; i < tamanhoArray; i++)
            {
                idsDisciplina[i] = Convert.ToInt32(result.Ids[i]["Id"]);               
            }

            return idsDisciplina;
        }


        /// <summary>
        /// Transforma um JSON de estados em um array
        /// </summary>
        /// <param name="json">string no formato JSON</param>
        /// <returns>Array com ids com os "estados dos ids"</returns>
        private Int32[] JsonEstadosObjeto(String json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            js.RegisterConverters(new JavaScriptConverter[] { new Prion.Tools4.DynamicJsonConverter() });
            dynamic result = js.Deserialize(json, typeof(object)) as dynamic;

            Int32 tamanhoArray = result.Ids.Count;            
            Int32[] estados = new Int32[tamanhoArray];

            for (Int32 i = 0; i < tamanhoArray; i++)
            {
                estados[i] = Convert.ToInt32(result.Ids[i]["Estado"]);
            }

            return estados;
        }
        #endregion
    }
}