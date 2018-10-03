using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotaAzul.Controllers
{
    public class BoletimController : BaseController
    {
        #region "Views"

        /// <summary>
        /// Retorna a view BoletimImpressao
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewBoletimImpressao()
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

            return View("BoletimImpressao", vmDadosCorporacao);
        }
        
        
        /// <summary>
        /// Retorna a view Novo
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewDados()
        {
            
            return View("Dados");
        }


        /// <summary>
        /// Retorna a view Lista, com os registros de Aluno
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewLista()
        {
            ViewData["DivisaoAnoLetivo"] = Sistema.GetConfiguracaoSistema("notas:divisao_ano_letivo");
            ViewData["MediaMinima"] = Sistema.GetConfiguracaoSistema("notas:media_minima");
            return View("Lista");
        }


        #endregion "Views"

        #region "Requisições"

        /// <summary>
        /// Carrega  registros do tipo Models.Disciplina atráves da matrícula do aluno selecionado
        /// </summary>
        /// <param name="id">id do ComposicaoNota</param>
        /// <returns></returns>
        public ActionResult CarregarDisciplinasDeUmaMatricula(Int32 id)
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);

            Prion.Tools.Request.Filtro f = new Prion.Tools.Request.Filtro("Matricula.Id", "=", id);
            parametros.Filtro.Add(f);

            f = new Prion.Tools.Request.Filtro("MatriculaCurso.IdTurma", "IN", "(SELECT MatriculaCurso.IdTurma from MatriculaCurso inner join Matricula ON MatriculaCurso.IdMatricula = Matricula.Id " +
                " INNER JOIN Turma on Turma.Id = MatriculaCurso.IdTurma " +
                " INNER JOIN CursoAnoLetivo on CursoAnoLetivo.Id = Turma.IdCursoAnoLetivo " +
                " INNER JOIN Curso on CursoAnoLetivo.IdCurso = Curso.Id " +
                " WHERE Matricula.Id =" + id + "  AND Curso.CursoCurricular = 'true')");

            parametros.Filtro.Add(f);

            Business.Disciplina biDisciplina= new Business.Disciplina();
            List<Models.Disciplina> listaDisciplinas = Prion.Tools.ListTo.CollectionToList <Models.Disciplina>(biDisciplina.Carregar(parametros).ListaObjetos);

            return Json(new { success = true, obj = listaDisciplinas }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Carrega  registros do tipo Models.MatriculaMEdia atráves da matrícula do aluno selecionado
        /// </summary>
        /// <param name="id">id do ComposicaoNota</param>
        /// <returns></returns>
        public ActionResult CarregarMediasDeUmaMatricula(Int32 id)
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
                     

            Business.NotasAluno biNotasAluno = new Business.NotasAluno();
            List<Models.MatriculaMedia> listaMatriculaMedia = Prion.Tools.ListTo.CollectionToList<Models.MatriculaMedia>(biNotasAluno.CarregarMedia(parametros,id).ListaObjetos);

            return Json(new { success = true, obj = listaMatriculaMedia }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}