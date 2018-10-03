using System;
using System.Web.Mvc;
using GenericHelpers = Prion.Generic.Helpers;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace NotaAzul.Controllers
{
    public class ProfessorController:BaseController
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
        /// Carrega um registro do tipo Models.CursoAnoLetivo atráves do seu ID
        /// </summary>
        /// <param name="id">id do CursoAnoLetivo</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);

            Business.Professor biProfessor = new Business.Professor();
            Models.Professor professor = (Models.Professor)biProfessor.Carregar(parametros, id);

            return Json(new { success = true, obj = professor }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retornar uma lista no formato JSON 
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLista()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.Funcionario bifuncionario = new Business.Funcionario();


            Prion.Tools.Request.Filtro f = new Prion.Tools.Request.Filtro("Funcionario.IdCargo", "=", "(SELECT Cargo.Id FROM Cargo WHERE Cargo.Nome = 'Professora')");
            parametros.Filtro.Add(f);

            Prion.Generic.Models.Lista lista = bifuncionario.ListaFuncionarios(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Associa as disciplinas a determinado professor
        /// </summary>    
        /// <returns></returns>
        public ActionResult Salvar()
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.Professor biProfessor = new Business.Professor();

            Int32 idFuncionario = Convert.ToInt32(Request.Form["Funcionario.Id"]);
            Int32 idTurma = Convert.ToInt32(Request.Form["Turma.Id"]);
            List<Models.ProfessorDisciplina> listaProfessorDisciplinas = JsonDisciplinas(Request.Form["Ids"],idFuncionario,idTurma);

            GenericHelpers.Retorno retorno = biProfessor.Salvar(listaProfessorDisciplinas);

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
        private List<Models.ProfessorDisciplina> JsonDisciplinas(String json,Int32 idFuncionario,Int32 idTurma)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<Models.ProfessorDisciplina> listaProfessorDisciplinas = new List<Models.ProfessorDisciplina>();

            js.RegisterConverters(new JavaScriptConverter[] { new Prion.Tools4.DynamicJsonConverter() });
            dynamic result = js.Deserialize(json, typeof(object)) as dynamic;
            Int32 tamanhoArray = result.Ids.Count;
            
            for (Int32 i = 0; i < tamanhoArray; i++)
            {
                Models.ProfessorDisciplina professorDisciplina = new Models.ProfessorDisciplina();
                professorDisciplina.IdDisciplina = Convert.ToInt32(result.Ids[i]["Id"]);
                professorDisciplina.EstadoObjeto = (GenericHelpers.Enums.EstadoObjeto)Convert.ToInt32(result.Ids[i]["Estado"]);
                professorDisciplina.IdProfessor = idFuncionario;
                professorDisciplina.IdTurma = idTurma;
                listaProfessorDisciplinas.Add(professorDisciplina);
            }

            return listaProfessorDisciplinas;
        }
        #endregion
    }
}