using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using GenericHelpers = Prion.Generic.Helpers;

namespace NotaAzul.Controllers
{
    public class NotasAlunoController : BaseController
    {
        #region "Views"

       
        /// <summary>
        /// Retorna a view Lista
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewLista()
        {
            ViewData["FormaDeConceito"] = Sistema.GetConfiguracaoSistema("notas:forma_conceito");
            ViewData["DivisaoAnoLetivo"] = Sistema.GetConfiguracaoSistema("notas:divisao_ano_letivo");        
            return View("Lista");
        }

        /// <summary>
        /// Retorna a view Dados
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewDados()
        {
            Business.ProfessorConfiguracao biConfig = new Business.ProfessorConfiguracao();
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Models.ProfessorConfiguracao configuracao = new Models.ProfessorConfiguracao();  
            Models.Usuario  usuario= (Models.Usuario)Session["UsuarioLogado"];

            Prion.Tools.Request.Filtro f = new Prion.Tools.Request.Filtro("ProfessorConfiguracao.IdUsuario", "=", usuario.Id);
            parametros.Filtro.Add(f);

            configuracao=(Models.ProfessorConfiguracao)biConfig.Carregar(parametros).Get(0);            
            
            ViewData["FormaDeConceito"] = Sistema.GetConfiguracaoSistema("notas:forma_conceito");
            ViewData["DivisaoAnoLetivo"] = Sistema.GetConfiguracaoSistema("notas:divisao_ano_letivo");
            ViewData["TipoMedia"] = (configuracao != null) ? configuracao.TipoMedia : ""; 
            return View("Dados");
        }

        #endregion "Views"

        #region "Requisições"

        /// <summary>
        /// Carrega um registro do tipo Models.MatriculaFormaDeAvaliacao atráves do seu ID
        /// </summary>
        /// <param name="id">id do MatriculaFormaDeAvaliacao</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            

            Business.NotasAluno biNotasAluno = new Business.NotasAluno();
            List<Models.MatriculaFormaDeAvaliacao> listaMatriculaFormaDeAvalicao = Prion.Tools.ListTo.CollectionToList<Models.MatriculaFormaDeAvaliacao>(biNotasAluno.Carregar(parametros,id).ListaObjetos);

            return Json(new { success = true, obj = listaMatriculaFormaDeAvalicao }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Carrega um registro do tipo Models.ComposicaoNota atráves do seu ID
        /// </summary>
        /// <param name="id">id do ComposicaoNota</param>
        /// <returns></returns>
        public ActionResult CarregarComposicaoNotaPeriodo(Int32 id)
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);

            Prion.Tools.Request.Filtro f = new Prion.Tools.Request.Filtro("ProfessorDisciplina.IdDisciplina", "=", id);
            parametros.Filtro.Add(f);

            Business.ComposicaoNotaPeriodo biComposicaoNota = new Business.ComposicaoNotaPeriodo();
            Models.ComposicaoNotaPeriodo composicaoNota = (Models.ComposicaoNotaPeriodo)biComposicaoNota.Carregar(parametros).Get(0);

            return Json(new { success = true, obj = composicaoNota }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Carrega todos registros do tipo Models.MatriculaFormaDeAvaliacao através de um Id de matrícula
        /// </summary>
        /// <param name="id">id da Matricula</param>
        /// <returns></returns>
        public ActionResult CarregarFormasDeAvaliacaoDeUmAluno(Int32 id)
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);            

            Business.NotasAluno biNotasAluno = new Business.NotasAluno();
            List<Models.MatriculaFormaDeAvaliacao> listaMatriculaFormaDeAvalicao = Prion.Tools.ListTo.CollectionToList<Models.MatriculaFormaDeAvaliacao>(biNotasAluno.Carregar(parametros,id).ListaObjetos);

            Models.MatriculaMedia matriculaMedia = (Models.MatriculaMedia)biNotasAluno.CarregarMedia(parametros, id).Get(0);

            ViewModels.NotasAluno.vmDados obj = new ViewModels.NotasAluno.vmDados();
            obj.ListaMatriculaFormasDeAvaliacao = listaMatriculaFormaDeAvalicao;
            obj.MatriculaMedia = matriculaMedia;

            return Json(new { success = true, obj = obj }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retornar uma lista no formato JSON 
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLista()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.NotasAluno biNotasAluno = new Business.NotasAluno();

            Prion.Generic.Models.Lista lista = biNotasAluno.ListaAlunos(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        } 

        /// <summary>
        /// Retornar uma lista no formato JSON 
        /// </summary>
        /// <returns></returns>
        public ActionResult GetListaFormasDeAvaliacao()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.NotasAluno biNotasAluno = new Business.NotasAluno();

            Prion.Generic.Models.Lista lista = biNotasAluno.ListaFormasDeAvaliacao(parametros);
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
            Business.NotasAluno biNotasAluno = new Business.NotasAluno();


            List<Models.MatriculaFormaDeAvaliacao> listaMatriculaFormaAvaliacao = JsonMatriculaFormaDeAvaliacao(Request.Form["MatriculaFormaDeAvaliacao"]);
            GenericHelpers.Retorno retorno = biNotasAluno.Salvar(listaMatriculaFormaAvaliacao);

            mensagem.EstadoObjeto = retorno.EstadoObjeto;
            mensagem.UltimoId = retorno.UltimoId;
            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Salva os dados de uma ComposicaoNota
        /// </summary>        
        /// <returns></returns>
        public ActionResult SalvarMedia()
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.NotasAluno biNotasAluno = new Business.NotasAluno();


            Models.MatriculaMedia matriculaMedia = JsonMatriculaMedia(Request.Form["NotasMedia"]);
            GenericHelpers.Retorno retorno = biNotasAluno.SalvarMedia(matriculaMedia);
           
            mensagem.EstadoObjeto = retorno.EstadoObjeto;
            mensagem.UltimoId = retorno.UltimoId;
            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Transforma um JSON em uma List<Models.MatriculaFormaDeAvaliacao>
        /// </summary>
        /// <param name="json">string no formato JSON</param>
        /// <returns>List<Models.MatriculaFormaDeAvaliacao></returns>
        private List<Models.MatriculaFormaDeAvaliacao> JsonMatriculaFormaDeAvaliacao(String json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            js.RegisterConverters(new JavaScriptConverter[] { new Prion.Tools4.DynamicJsonConverter() });
            dynamic result = js.Deserialize(json, typeof(object)) as dynamic;
            List<Models.MatriculaFormaDeAvaliacao> listaMatriculaFormaDeAvaliacao = new List<Models.MatriculaFormaDeAvaliacao>();

            for (Int32 i = 0; i < result.Length; i++)
            {
                Models.MatriculaFormaDeAvaliacao matriculaFormaDeAvaliacao = new Models.MatriculaFormaDeAvaliacao();
                matriculaFormaDeAvaliacao.Id = Convert.ToInt32(result[i].Id);
                matriculaFormaDeAvaliacao.IdMatricula = Convert.ToInt32(result[i].IdMatricula);
                matriculaFormaDeAvaliacao.IdFormaDeAvaliacao = Convert.ToInt32(result[i].IdFormaDeAvaliacao);
                matriculaFormaDeAvaliacao.ValorAlcancado = Convert.ToDecimal(result[i].ValorAlcancado);

                listaMatriculaFormaDeAvaliacao.Add(matriculaFormaDeAvaliacao);
            }

            return listaMatriculaFormaDeAvaliacao;
        }


        /// <summary>
        /// Transforma um JSON em uma List<Models.MatriculaMedia>
        /// </summary>
        /// <param name="json">string no formato JSON</param>
        /// <returns>List<Models.MatriculaFormaDeAvaliacao></returns>
        private Models.MatriculaMedia JsonMatriculaMedia(String json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            js.RegisterConverters(new JavaScriptConverter[] { new Prion.Tools4.DynamicJsonConverter() });
            dynamic result = js.Deserialize(json, typeof(object)) as dynamic;

            Models.MatriculaMedia matriculaMedia = new Models.MatriculaMedia();
            List<Models.MatriculaNota> notas = new List<Models.MatriculaNota>();

            matriculaMedia.Id = Convert.ToInt32(result.Id);
            matriculaMedia.EstadoObjeto = (matriculaMedia.Id == 0) ? Prion.Generic.Helpers.Enums.EstadoObjeto.Novo : GenericHelpers.Enums.EstadoObjeto.Alterado;
            matriculaMedia.IdComposicaoNotaPeriodo = Convert.ToInt32(result.IdComposicaoNota);
            matriculaMedia.IdMatricula = Convert.ToInt32(result.IdMatricula);
            matriculaMedia.ValorAlcancado = Convert.ToDecimal(result.ValorAlcancado);

            for (Int32 i = 0; i < result.ListaNotas.Count; i++)
            {
                Models.MatriculaNota matriculaNota = new Models.MatriculaNota();
                matriculaNota.Id = Convert.ToInt32(result.ListaNotas[i]["Id"]);
                matriculaNota.EstadoObjeto = (matriculaNota.Id == 0) ? Prion.Generic.Helpers.Enums.EstadoObjeto.Novo : GenericHelpers.Enums.EstadoObjeto.Alterado;
                matriculaNota.IdMatricula = Convert.ToInt32(result.ListaNotas[i]["IdMatricula"]);
                matriculaNota.IdComposicaoNota = Convert.ToInt32(result.ListaNotas[i]["IdComposicaoNota"]);
                matriculaNota.ValorAlcancado = Convert.ToDecimal(result.ListaNotas[i]["ValorAlcancado"]);
                matriculaNota.ValorFinal = Convert.ToDecimal(result.ListaNotas[i]["ValorFinal"]);

                notas.Add(matriculaNota);
            }
            matriculaMedia.Notas = notas;

            return matriculaMedia;
        }
        #endregion
    }
}
