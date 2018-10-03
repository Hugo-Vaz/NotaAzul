using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GenericHelpers = Prion.Generic.Helpers;

namespace NotaAzul.Controllers
{
    public class TituloController:BaseController
    {
        #region "Views"
        /// <summary>
        /// Retorna a view Lista, com os registros de Título
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
            ViewModels.Titulo.vmDados vmDados = new ViewModels.Titulo.vmDados();
            Business.Situacao biSituacao = new Business.Situacao();

            //carrega a lista de situações de um Título
            vmDados.Situacao = biSituacao.CarregarSituacoesPelaSituacao("Título","Aberto");
   
            return View("Dados", vmDados);
        }

        #endregion "Views"


        #region "Requisições"

        /// <summary>
        /// Carrega um registro do tipo Prion.Generic.Models.Titulo através do seu Id
        /// </summary>
        /// <param name="id">id do Título</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
            Business.Titulo biTitulo = new Business.Titulo();
            Prion.Generic.Models.Titulo titulo = (Prion.Generic.Models.Titulo)biTitulo.Carregar(id).Get(0);

            return Json(new { success = true, obj = titulo }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Exclui um Título pelo Id
        /// </summary>
        /// <returns>Um retorno no formato de Json</returns>
        public ActionResult Excluir()
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.Titulo biTitulo = new Business.Titulo();
            GenericHelpers.Retorno retorno = biTitulo.Excluir(Prion.Tools.Conversor.ToInt(Request.Form["id"]));

            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = (retorno.Sucesso) ? "Título excluído com sucesso" : retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Retorna um JSON contendo uma lista de Titulos que estão com a situação igual 'Aberto'
        /// </summary>
        /// <param name="term">uma string representando um nome(ou parte) de um Título</param>
        /// <returns>Json contendo objetos do tipo Prion.Generic.Models.Titulo</returns>
        public ActionResult ListaTituloEmAbertoPorDescricao(String term)
        {
            Business.Titulo biTitulo = new Business.Titulo();
            Prion.Generic.Models.Lista lista = biTitulo.CarregarEmAberto(term += "%");
            List<Prion.Generic.Models.Titulo> listaTitulos = Prion.Tools.ListTo.CollectionToList<Prion.Generic.Models.Titulo>(lista.ListaObjetos);

            if (listaTitulos == null) { return Json(new { label = "Nada foi encontrado" }, JsonRequestBehavior.AllowGet); }

            var result = from titulo in listaTitulos select new { label = titulo.Descricao, value = titulo };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Salva os dados de um Título
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult Salvar(ViewModels.Titulo.vmDados vModel)
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.Titulo biTitulo = new Business.Titulo();
            GenericHelpers.Retorno retorno = biTitulo.Salvar(vModel.Titulo);

            mensagem.EstadoObjeto = retorno.EstadoObjeto;
            mensagem.UltimoId = retorno.UltimoId;
            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Retorna uma lista no formato JSON do tipo Prion.Generic.Models.Titulo
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLista()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.Titulo biTitulo = new Business.Titulo();

            Prion.Generic.Models.Lista lista = biTitulo.ListaTitulos(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}