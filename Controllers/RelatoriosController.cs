using System;
using System.Web.Mvc;
using GenericHelpers = Prion.Generic.Helpers;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using GenericTools = Prion.Tools;

namespace NotaAzul.Controllers
{
    public class RelatoriosController:BaseController
    {
        #region "Views"


        /// <summary>
        /// Retorna a view model de corporação
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Corporacao carregarCorporacao()
        {
            Prion.Generic.Models.Corporacao corporacao;

            Business.Corporacao biCorporacao = new Business.Corporacao();
            Business.Bairro biBairro = new Business.Bairro();
            Business.Cidade biCidade = new Business.Cidade();

            Int32 idEstado = Prion.Tools.Conversor.ToInt32(Sistema.GetConfiguracaoSistema("estado:id_uf_default"));
            // carregar o estado usando a variável idEstado

            corporacao = biCorporacao.Carregar(Sistema.UsuarioLogado.IdCorporacao);
            corporacao.Bairro = (Prion.Generic.Models.Bairro)biBairro.Carregar(corporacao.IdBairro).Get(0);
            corporacao.Cidade = (Prion.Generic.Models.Cidade)biCidade.Carregar(corporacao.IdCidade).Get(0);
            corporacao.Estado.UF = "RJ"; // TMZ: mudar para usar o objeto de estado carregado logo acima

            return corporacao;
        }
        
        /// <summary>
        /// Retorna a view Relatorio com o filtro referente à Contas Pagas
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewContasPagas()
        {
            //Para se criar a página de filtro, é necessário passar pela ViewData, os filtros(Ex:Periodo,Aluno) na ordem desejada,o tipo de relatório
            //(Ex:ContasPagas,MovimentaçãoFinanceira),um booleano para se criar os radio buttons, caso esse booleano seja true é necessário
            // passar as opções dos radio
            ViewData["Filtro"] = "Periodo";
            ViewData["TipoRelatorio"] = "ContasPagas";
            ViewData["SelecionarFiltro"] = false;

            ViewModels.Corporacao.vmDados vmDadosCorporacao = new ViewModels.Corporacao.vmDados();
            vmDadosCorporacao.Corporacao = carregarCorporacao();
            return View("Relatorio",vmDadosCorporacao);
        }

        /// <summary>
        /// Retorna a view Relatorio com o filtro referente à Contas à Pagar
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewContasAPagar()
        {
            //Para se criar a página de filtro, é necessário passar pela ViewData, os filtros(Ex:Periodo,Aluno) na ordem desejada,o tipo de relatório
            //(Ex:ContasPagas,MovimentaçãoFinanceira),um booleano para se criar os radio buttons, caso esse booleano seja true é necessário
            // passar as opções dos radio
            ViewData["Filtro"] = "Periodo";
            ViewData["TipoRelatorio"] = "ContasAPagar";
            ViewData["SelecionarFiltro"] = false;

            ViewModels.Corporacao.vmDados vmDadosCorporacao = new ViewModels.Corporacao.vmDados();
            vmDadosCorporacao.Corporacao = carregarCorporacao();
            return View("Relatorio", vmDadosCorporacao);
        }

        /// <summary>
        /// Retorna a view Contas Recebidas
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewContasRecebidas()
        {
            //Para se criar a página de filtro, é necessário passar pela ViewData, os filtros(Ex:Periodo,Aluno) na ordem desejada,o tipo de relatório
            //(Ex:ContasPagas,MovimentaçãoFinanceira),um booleano para se criar os radio buttons, caso esse booleano seja true é necessário
            // passar as opções dos radio
            ViewModels.Corporacao.vmDados vmDadosCorporacao = new ViewModels.Corporacao.vmDados();
            vmDadosCorporacao.Corporacao = carregarCorporacao();
            return View("RelatorioContasRecebidas", vmDadosCorporacao);
        }

        /// <summary>
        /// Retorna a view Relatorio com o filtro referente à Contas à Receber
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewContasAReceber()
        {
            //Para se criar a página de filtro, é necessário passar pela ViewData, os filtros(Ex:Periodo,Aluno) na ordem desejada,o tipo de relatório
            //(Ex:ContasPagas,MovimentaçãoFinanceira),um booleano para se criar os radio buttons, caso esse booleano seja true é necessário
            // passar as opções dos radio
            ViewData["Filtro"] = "Periodo";
            ViewData["TipoRelatorio"] = "ContasAReceber";
            ViewData["SelecionarFiltro"] = false;

            ViewModels.Corporacao.vmDados vmDadosCorporacao = new ViewModels.Corporacao.vmDados();
            vmDadosCorporacao.Corporacao = carregarCorporacao();
            return View("Relatorio", vmDadosCorporacao);
        }

        /// <summary>
        /// Retorna a view Relatorio com o filtro referente aos Alunos Matriculados 
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewAlunosMatriculados()
        {
            //Para se criar a página de filtro, é necessário passar pela ViewData, os filtros(Ex:Periodo,Aluno) na ordem desejada,o tipo de relatório
            //(Ex:ContasPagas,MovimentaçãoFinanceira),um booleano para se criar os radio buttons, caso esse booleano seja true é necessário
            // passar as opções dos radio
            ViewData["Filtro"] = "Turma";
            ViewData["TipoRelatorio"] = "AlunosMatriculados";
            ViewData["SelecionarFiltro"] = false;

            ViewModels.Corporacao.vmDados vmDadosCorporacao = new ViewModels.Corporacao.vmDados();
            vmDadosCorporacao.Corporacao = carregarCorporacao();
            return View("Relatorio", vmDadosCorporacao);
        }

        /// <summary>
        /// Retorna a view Relatorio com o filtro referente aos Alunos Isentos 
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewAlunosBolsistas()
        {
            //Para se criar a página de filtro, é necessário passar pela ViewData, os filtros(Ex:Periodo,Aluno) na ordem desejada,o tipo de relatório
            //(Ex:ContasPagas,MovimentaçãoFinanceira),um booleano para se criar os radio buttons, caso esse booleano seja true é necessário
            // passar as opções dos radio
            ViewData["Filtro"] = "Turma";
            ViewData["TipoRelatorio"] = "AlunosBolsistas";
            ViewData["SelecionarFiltro"] = false;

            ViewModels.Corporacao.vmDados vmDadosCorporacao = new ViewModels.Corporacao.vmDados();
            vmDadosCorporacao.Corporacao = carregarCorporacao();
            return View("Relatorio", vmDadosCorporacao);
        }

        /// <summary>
        /// Retorna a view Relatorio com o filtro referente aos Alunos Matriculados 
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewAlunosInadimplentes()
        {
            //Para se criar a página de filtro, é necessário passar pela ViewData, os filtros(Ex:Periodo,Aluno) na ordem desejada,o tipo de relatório
            //(Ex:ContasPagas,MovimentaçãoFinanceira),um booleano para se criar os radio buttons, caso esse booleano seja true é necessário
            // passar as opções dos radio
            ViewData["Filtro"] = "Turma";
            ViewData["TipoRelatorio"] = "AlunosInadimplentes";
            ViewData["SelecionarFiltro"] = false;

            ViewModels.Corporacao.vmDados vmDadosCorporacao = new ViewModels.Corporacao.vmDados();
            vmDadosCorporacao.Corporacao = carregarCorporacao();
            return View("Relatorio", vmDadosCorporacao);
        }

        /// <summary>
        /// Retorna a view Relatorio com o filtro referente aos Alunos Matriculados 
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewAlunosInadimplentesBoleto()
        {
            //Para se criar a página de filtro, é necessário passar pela ViewData, os filtros(Ex:Periodo,Aluno) na ordem desejada,o tipo de relatório
            //(Ex:ContasPagas,MovimentaçãoFinanceira),um booleano para se criar os radio buttons, caso esse booleano seja true é necessário
            // passar as opções dos radio        
           

            ViewModels.Corporacao.vmDados vmDadosCorporacao = new ViewModels.Corporacao.vmDados();
            vmDadosCorporacao.Corporacao = carregarCorporacao();
            return View("RelatorioInadimplentes", vmDadosCorporacao);
        }

        /// <summary>
        /// Retorna a view Relatorio com o filtro referente aos Alunos Matriculados 
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewArquivoRetorno()
        {
            //Para se criar a página de filtro, é necessário passar pela ViewData, os filtros(Ex:Periodo,Aluno) na ordem desejada,o tipo de relatório
            //(Ex:ContasPagas,MovimentaçãoFinanceira),um booleano para se criar os radio buttons, caso esse booleano seja true é necessário
            // passar as opções dos radio        
            
            ViewModels.Corporacao.vmDados vmDadosCorporacao = new ViewModels.Corporacao.vmDados();
            vmDadosCorporacao.Corporacao = carregarCorporacao();
            return View("RelatorioArquivoRetorno", vmDadosCorporacao);
        }



        /// <summary>
        /// Retorna a view Relatorio com o filtro referente aos Alunos Isentos 
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewAlunosIsentos()
        {
            //Para se criar a página de filtro, é necessário passar pela ViewData, os filtros(Ex:Periodo,Aluno) na ordem desejada,o tipo de relatório
            //(Ex:ContasPagas,MovimentaçãoFinanceira),um booleano para se criar os radio buttons, caso esse booleano seja true é necessário
            // passar as opções dos radio
            ViewData["Filtro"] = "Periodo,Turma";
            ViewData["TipoRelatorio"] = "AlunosIsentos";
            ViewData["SelecionarFiltro"] = false;

            ViewModels.Corporacao.vmDados vmDadosCorporacao = new ViewModels.Corporacao.vmDados();
            vmDadosCorporacao.Corporacao = carregarCorporacao();
            return View("Relatorio", vmDadosCorporacao);
        }

        /// <summary>
        /// Retorna a view Relatorio com o filtro referente aos Alunos Matriculados 
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewMovimentacaoFinanceira()
        {
            //Para se criar a página de filtro, é necessário passar pela ViewData, os filtros(Ex:Periodo,Aluno) na ordem desejada,o tipo de relatório
            //(Ex:ContasPagas,MovimentaçãoFinanceira),um booleano para se criar os radio buttons, caso esse booleano seja true é necessário
            // passar as opções dos radio
            ViewData["Filtro"] = "Periodo,Situacao";
            ViewData["TipoRelatorio"] = "MovimentacaoFinanceira";
            ViewData["SelecionarFiltro"] = false;
            ViewModels.Relatorios.vmDados vmDados = new ViewModels.Relatorios.vmDados();

            Business.Situacao biSituacao = new Business.Situacao();
            vmDados.Situacoes = new List<Prion.Generic.Models.Situacao>();
            vmDados.Situacoes.Add(biSituacao.CarregarSituacoesPelaSituacao("Genérico", "Concluído"));
            vmDados.Situacoes.Add(biSituacao.CarregarSituacoesPelaSituacao("Genérico", "Cancelado"));

            vmDados.Corporacao= carregarCorporacao();
            return View("Relatorio", vmDados);
        }

        /// <summary>
        /// Retorna a view Relatorio com o filtro referente à Mensalidades atrasadas
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewMensalidadesAtrasadas()
        {
            //Para se criar a página de filtro, é necessário passar pela ViewData, os filtros(Ex:Periodo,Aluno) na ordem desejada,o tipo de relatório
            //(Ex:ContasPagas,MovimentaçãoFinanceira),um booleano para se criar os radio buttons, caso esse booleano seja true é necessário
            // passar as opções dos radio
            String[] arrayOpcoes = { "Selecionar por Turma", "Selecionar por Aluno"};
            String[] arrayValores = { "CursoTurma", "Aluno" };
            ViewData["Filtro"] = "CursoTurma,Aluno";
            ViewData["TipoRelatorio"] = "MensalidadesAtrasadas";
            ViewData["SelecionarFiltro"] = true;
            ViewData["OpcoesRadio"] = arrayOpcoes;
            ViewData["ValoresRadio"] = arrayValores;
            ViewData["PrimeiroFiltroDoRadio"] = "FiltroCursoTurma";

            ViewModels.Corporacao.vmDados vmDadosCorporacao = new ViewModels.Corporacao.vmDados();
            vmDadosCorporacao.Corporacao = carregarCorporacao();
            return View("Relatorio", vmDadosCorporacao);            
        }

        /// <summary>
        /// Retorna a view Relatorio com o filtro referente à Mensalidades adiantadas
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewMensalidadesAdiantadas()
        {
            //Para se criar a página de filtro, é necessário passar pela ViewData, os filtros(Ex:Periodo,Aluno) na ordem desejada,o tipo de relatório
            //(Ex:ContasPagas,MovimentaçãoFinanceira),um booleano para se criar os radio buttons, caso esse booleano seja true é necessário
            // passar as opções dos radio
            String[] arrayOpcoes = { "Selecionar por Turma", "Selecionar por Aluno" };
            String[] arrayValores = { "CursoTurma", "Aluno" };
            ViewData["Filtro"] = "CursoTurma,Aluno";
            ViewData["TipoRelatorio"] = "MensalidadesAdiantadas";
            ViewData["SelecionarFiltro"] = true;
            ViewData["OpcoesRadio"] = arrayOpcoes;
            ViewData["ValoresRadio"] = arrayValores;
            ViewData["PrimeiroFiltroDoRadio"] = "FiltroCursoTurma";

            ViewModels.Corporacao.vmDados vmDadosCorporacao = new ViewModels.Corporacao.vmDados();
            vmDadosCorporacao.Corporacao = carregarCorporacao();
            return View("Relatorio", vmDadosCorporacao);
        }

        /// <summary>
        /// Retorna a view Relatorio com o filtro referente à Contas Pagas
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewMovFinanceiraResponsavel()
        {
            //Para se criar a página de filtro, é necessário passar pela ViewData, os filtros(Ex:Periodo,Aluno) na ordem desejada,o tipo de relatório
            //(Ex:ContasPagas,MovimentaçãoFinanceira),um booleano para se criar os radio buttons, caso esse booleano seja true é necessário
            // passar as opções dos radio
            ViewData["Filtro"] = "Periodo";
            ViewData["TipoRelatorio"] = "MovimentacaoFinanceiraResponsavel";
            ViewData["SelecionarFiltro"] = false;

            ViewModels.Corporacao.vmDados vmDadosCorporacao = new ViewModels.Corporacao.vmDados();
            vmDadosCorporacao.Corporacao = carregarCorporacao();
            return View("Relatorio", vmDadosCorporacao);
        }

        #endregion "Views"

        #region "Requisições"

        /// <summary>
        /// Retorna uma lista no formato JSON com os dados para o relatório de Contas Pagas
        /// </summary>
        /// <returns></returns>
        public ActionResult GerarContasPagas()
        {
            GenericTools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            GenericTools.Request.ParametrosRequest parametros = new GenericTools.Request.ParametrosRequest();
            JsonFiltro(Request.Form["Filtro"],parametros);
            parametros.Paginar = false;

            Business.Relatorio biRelatorio = new Business.Relatorio();
            Prion.Generic.Models.Lista lista = biRelatorio.GerarRelatorioContasPagas(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retorna uma lista no formato JSON com os dados para o relatório de Contas Pagas
        /// </summary>
        /// <returns></returns>
        public ActionResult GerarContasAPagar()
        {
            GenericTools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            GenericTools.Request.ParametrosRequest parametros = new GenericTools.Request.ParametrosRequest();
            JsonFiltro(Request.Form["Filtro"], parametros);
            parametros.Paginar = false;

            Business.Relatorio biRelatorio = new Business.Relatorio();
            Prion.Generic.Models.Lista lista = biRelatorio.GerarRelatorioContasAPagar(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retorna uma lista no formato JSON com os dados para o relatório de Contas Recebidas
        /// </summary>
        /// <returns></returns>
        public ActionResult GerarContasRecebidas()
        {
            GenericTools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            GenericTools.Request.ParametrosRequest parametros = new GenericTools.Request.ParametrosRequest();
            JsonFiltro(Request.Form["Filtro"], parametros);
            parametros.Paginar = false;

            Business.Relatorio biRelatorio = new Business.Relatorio();
            Prion.Generic.Models.Lista lista = biRelatorio.GerarRelatorioContasRecebidas(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retorna uma lista no formato JSON com os dados para o relatório de Contas Pagas
        /// </summary>
        /// <returns></returns>
        public ActionResult GerarContasAReceber()
        {
            GenericTools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            GenericTools.Request.ParametrosRequest parametros = new GenericTools.Request.ParametrosRequest();
            JsonFiltro(Request.Form["Filtro"], parametros);
            parametros.Paginar = false;

            Business.Relatorio biRelatorio = new Business.Relatorio();
            Prion.Generic.Models.Lista lista = biRelatorio.GerarRelatorioContasAReceber(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retorna uma lista no formato JSON com os dados para o relatório de AlunosMatriculados
        /// </summary>
        /// <returns></returns>
        public ActionResult GerarAlunosMatriculados()
        {
            GenericTools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            GenericTools.Request.ParametrosRequest parametros = new GenericTools.Request.ParametrosRequest();
            JsonFiltro(Request.Form["Filtro"], parametros);
            parametros.Paginar = false;

            Business.Relatorio biRelatorio = new Business.Relatorio();
            Prion.Generic.Models.Lista lista = biRelatorio.GerarRelatorioAlunosMatriculados(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retorna uma lista no formato JSON com os dados para o relatório de AlunosBolsistas
        /// </summary>
        /// <returns></returns>
        public ActionResult GerarAlunosBolsistas()
        {
            GenericTools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            GenericTools.Request.ParametrosRequest parametros = new GenericTools.Request.ParametrosRequest();
            JsonFiltro(Request.Form["Filtro"], parametros);
            parametros.Paginar = false;

            Business.Relatorio biRelatorio = new Business.Relatorio();
            Prion.Generic.Models.Lista lista = biRelatorio.GerarRelatorioAlunosBolsistas(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retorna uma lista no formato JSON com os dados para o relatório de AlunosInadimplentes
        /// </summary>
        /// <returns></returns>
        public ActionResult GerarAlunosInadimplentes()
        {
            GenericTools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            GenericTools.Request.ParametrosRequest parametros = new GenericTools.Request.ParametrosRequest();
            JsonFiltro(Request.Form["Filtro"], parametros);
            parametros.Paginar = false;

            Business.Relatorio biRelatorio = new Business.Relatorio();
            Prion.Generic.Models.Lista lista = biRelatorio.GerarRelatorioAlunosInadimplentes(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retorna uma lista no formato JSON com os dados para o relatório de AlunosIsentos
        /// </summary>
        /// <returns></returns>
        public ActionResult GerarAlunosIsentos()
        {
            GenericTools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            GenericTools.Request.ParametrosRequest parametros = new GenericTools.Request.ParametrosRequest();
            JsonFiltro(Request.Form["Filtro"], parametros);
            parametros.Paginar = false;

            Business.Relatorio biRelatorio = new Business.Relatorio();
            Prion.Generic.Models.Lista lista = biRelatorio.GerarRelatorioAlunosIsentos(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retorna uma lista no formato JSON com os dados para o relatório de Movimentações Financeiras
        /// </summary>
        /// <returns></returns>
        public ActionResult GerarMovimentacaoFinanceira()
        {
            GenericTools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            GenericTools.Request.ParametrosRequest parametros = new GenericTools.Request.ParametrosRequest();
            JsonFiltro(Request.Form["Filtro"], parametros);
            parametros.Paginar = false;

            Business.Relatorio biRelatorio = new Business.Relatorio();
            Prion.Generic.Models.Lista lista = biRelatorio.GerarRelatorioMovimentacaoFinanceira(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retorna uma lista no formato JSON com os dados para o relatório de Movimentações Financeiras/Responsável
        /// </summary>
        /// <returns></returns>
        public ActionResult GerarMovimentacaoFinanceiraResponsavel()
        {
            GenericTools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            GenericTools.Request.ParametrosRequest parametros = new GenericTools.Request.ParametrosRequest();
            JsonFiltro(Request.Form["Filtro"], parametros);
            parametros.Paginar = false;

            Business.Relatorio biRelatorio = new Business.Relatorio();
            Prion.Generic.Models.Lista lista = biRelatorio.GerarRelatorioMovimentacaoFinanceiraResponsavel(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retorna uma lista no formato JSON com os dados para o relatório de Movimentações Financeiras/Responsável
        /// </summary>
        /// <returns></returns>
        public ActionResult GerarAlunosInadimplentesBoleto()
        {
            GenericTools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            GenericTools.Request.ParametrosRequest parametros = new GenericTools.Request.ParametrosRequest();
            parametros.Paginar = false;

            Int32 quantidadeBoletos = Convert.ToInt32(Sistema.GetConfiguracaoSistema("boleto:nosso_numero"));

            Business.Relatorio biRelatorio = new Business.Relatorio();
            Prion.Generic.Models.Lista lista = biRelatorio.GerarRelatorioAlunosInadimplentesBoleto(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retorna uma lista no formato JSON com os dados para o relatório de Movimentações Financeiras/Responsável
        /// </summary>
        /// <returns></returns>
        public ActionResult GerarBoletosQuitados()
        {
            GenericTools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            GenericTools.Request.ParametrosRequest parametros = new GenericTools.Request.ParametrosRequest();
            parametros.Paginar = false;

            Business.Relatorio biRelatorio = new Business.Relatorio();
            Prion.Generic.Models.Lista lista = biRelatorio.GerarRelatorioBoletosQuitados(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retorna uma lista no formato JSON com os dados para o relatório de Movimentações Financeiras/Responsável
        /// </summary>
        /// <returns></returns>
        public ActionResult GerarArquivoRetorno()
        {
            GenericTools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            GenericTools.Request.ParametrosRequest parametros = new GenericTools.Request.ParametrosRequest();
            parametros.Paginar = false;

            Business.Relatorio biRelatorio = new Business.Relatorio();
            Prion.Generic.Models.Lista lista = biRelatorio.GerarRelatorioArquivoRet(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        #region "PDF local"
        /// <summary>
        /// Cria um arquivo html temporário, e cria um pdf a partir do mesmo
        /// </summary>
        /// <returns></returns>
        public ActionResult GerarPdf()
        {
            GenericTools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            GenericTools.Request.ParametrosRequest parametros = new GenericTools.Request.ParametrosRequest();
            String content = Request.Form["Html"];
            String tipoRelatorio = Request.Form["NomeRelatorio"];
            String formatoPagina = (tipoRelatorio.Substring(0, 6) == "Alunos" || tipoRelatorio.Substring(0, 6) == "Contas")
                ? "\"landscape\""
                : "\"portrait\"";
            Int32 length = content.Length % 4;
            //Caso o mod do tamanho da string por 4, não seja zero.Serão adicionados caracteres a string de acordo com a diferença entre o tamanho da String e o Mod
            //Pois não é possível decodificar string de Base64,cujo length não seja divisível por 4
            if (length != 0)
            {
                content = content.PadRight(content.Length + (4 - length), '=');
            }

            String html = Prion.Tools.Conversor.Base64Decode(content),
                 enderecoDocumentos = Helpers.Settings.EnderecoDocumentos(),
                 urlDocumentos = Helpers.Settings.UrlDocumentos(),
                 disco = Helpers.Settings.Disco(),
                 nomeRelatorio = Request.Form["NomeRelatorio"] + "-" + DateTime.Now.ToString("dd-MM-yyyy(hh-mm)");

            StreamWriter streamPaginaTemp = new StreamWriter(enderecoDocumentos + "/temp/" + nomeRelatorio + ".html", false, System.Text.Encoding.UTF8);
            streamPaginaTemp.Write(html);
            streamPaginaTemp.Close();

            Helpers.GeradorDePdf geradorDePdf = new Helpers.GeradorDePdf();
            geradorDePdf.CriarPdfDeHtml(disco, enderecoDocumentos + "phantomjs/", urlDocumentos + "temp/" + nomeRelatorio + ".html", nomeRelatorio, "\"A4\"", formatoPagina);
            streamPaginaTemp.Dispose();
            String enderecoDoArquivo = urlDocumentos + "relatorios/" + nomeRelatorio + ".pdf";

            return Json(new { success = true, url = enderecoDoArquivo }, JsonRequestBehavior.AllowGet);
        }

        public FileResult GerarPDFMovimentacaoFinanceiraResponsavel()
        {
            GenericTools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            GenericTools.Request.ParametrosRequest parametros = new GenericTools.Request.ParametrosRequest();
            JsonFiltro(Request.Form["Filtro"], parametros);
            parametros.Paginar = false;

            Business.Relatorio biRelatorio = new Business.Relatorio();

            Prion.Generic.Models.Lista lista = biRelatorio.GerarRelatorioMovimentacaoFinanceiraResponsavel(parametros);

            Business.Boleto biBoleto = new Business.Boleto();
            String endereco = Helpers.Settings.EnderecoSistema() + "/Content/temp/MovimentacaoFinanceira.pdf";
            if (System.IO.File.Exists(endereco))
            {
                System.IO.File.Delete(endereco);
            }

            biBoleto.MontarPdfRelatorio(lista.DataTable, endereco);
            Byte[] file = System.IO.File.ReadAllBytes(endereco);
            FileContentResult result = new FileContentResult(file, "application/pdf");

            return result;
        }

        /// <summary>
        /// Retorna uma lista no formato JSON com os dados para o relatório de Contas Pagas
        /// </summary>
        /// <returns></returns>
        public FileResult GerarPDFContasPagas()
        {
            GenericTools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            GenericTools.Request.ParametrosRequest parametros = new GenericTools.Request.ParametrosRequest();
            JsonFiltro(Request.Form["Filtro"], parametros);
            parametros.Paginar = false;

            Business.Relatorio biRelatorio = new Business.Relatorio();
            Prion.Generic.Models.Lista lista = biRelatorio.GerarRelatorioContasPagas(parametros);
            Business.Boleto biBoleto = new Business.Boleto();
            String endereco = Helpers.Settings.EnderecoSistema() + "/Content/temp/ContasPagas.pdf";
            if (System.IO.File.Exists(endereco))
            {
                System.IO.File.Delete(endereco);
            }

            biBoleto.MontarPdfRelatorio(lista.DataTable, endereco);
            Byte[] file = System.IO.File.ReadAllBytes(endereco);
            FileContentResult result = new FileContentResult(file, "application/pdf");

            return result;
        }

        /// <summary>
        /// Retorna uma lista no formato JSON com os dados para o relatório de Contas Pagas
        /// </summary>
        /// <returns></returns>
        public FileResult GerarPDFContasAPagar()
        {
            GenericTools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            GenericTools.Request.ParametrosRequest parametros = new GenericTools.Request.ParametrosRequest();
            JsonFiltro(Request.Form["Filtro"], parametros);
            parametros.Paginar = false;

            Business.Relatorio biRelatorio = new Business.Relatorio();
            Prion.Generic.Models.Lista lista = biRelatorio.GerarRelatorioContasAPagar(parametros);

            Business.Boleto biBoleto = new Business.Boleto();
            String endereco = Helpers.Settings.EnderecoSistema() + "/Content/temp/ContasPagar.pdf";
            if (System.IO.File.Exists(endereco))
            {
                System.IO.File.Delete(endereco);
            }

            biBoleto.MontarPdfRelatorio(lista.DataTable, endereco);
            Byte[] file = System.IO.File.ReadAllBytes(endereco);
            FileContentResult result = new FileContentResult(file, "application/pdf");

            return result;
        }

        /// <summary>
        /// Retorna uma lista no formato JSON com os dados para o relatório de Contas Recebidas
        /// </summary>
        /// <returns></returns>
        public FileResult GerarPDFContasRecebidas()
        {
            GenericTools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            GenericTools.Request.ParametrosRequest parametros = new GenericTools.Request.ParametrosRequest();
            JsonFiltro(Request.Form["Filtro"], parametros);
            parametros.Paginar = false;

            Business.Relatorio biRelatorio = new Business.Relatorio();
            Prion.Generic.Models.Lista lista = biRelatorio.GerarRelatorioContasRecebidas(parametros);

            Business.Boleto biBoleto = new Business.Boleto();
            String endereco = Helpers.Settings.EnderecoSistema() + "/Content/temp/ContasRecebidas.pdf";
            if (System.IO.File.Exists(endereco))
            {
                System.IO.File.Delete(endereco);
            }

            biBoleto.MontarPdfRelatorio(lista.DataTable, endereco);
            Byte[] file = System.IO.File.ReadAllBytes(endereco);
            FileContentResult result = new FileContentResult(file, "application/pdf");

            return result;
        }

        /// <summary>
        /// Retorna uma lista no formato JSON com os dados para o relatório de Contas Pagas
        /// </summary>
        /// <returns></returns>
        public FileResult GerarPDFContasAReceber()
        {
            GenericTools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            GenericTools.Request.ParametrosRequest parametros = new GenericTools.Request.ParametrosRequest();
            JsonFiltro(Request.Form["Filtro"], parametros);
            parametros.Paginar = false;

            Business.Relatorio biRelatorio = new Business.Relatorio();
            Prion.Generic.Models.Lista lista = biRelatorio.GerarRelatorioContasAReceber(parametros);

            Business.Boleto biBoleto = new Business.Boleto();
            String endereco = Helpers.Settings.EnderecoSistema() + "/Content/temp/ContasAReceber.pdf";
            if (System.IO.File.Exists(endereco))
            {
                System.IO.File.Delete(endereco);
            }

            biBoleto.MontarPdfRelatorio(lista.DataTable, endereco);
            Byte[] file = System.IO.File.ReadAllBytes(endereco);
            FileContentResult result = new FileContentResult(file, "application/pdf");

            return result;
        }

        /// <summary>
        /// Retorna uma lista no formato JSON com os dados para o relatório de AlunosMatriculados
        /// </summary>
        /// <returns></returns>
        public FileResult GerarPDFAlunosMatriculados()
        {
            GenericTools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            GenericTools.Request.ParametrosRequest parametros = new GenericTools.Request.ParametrosRequest();
            JsonFiltro(Request.Form["Filtro"], parametros);
            parametros.Paginar = false;

            Business.Relatorio biRelatorio = new Business.Relatorio();
            Prion.Generic.Models.Lista lista = biRelatorio.GerarRelatorioAlunosMatriculados(parametros);

            Business.Boleto biBoleto = new Business.Boleto();
            String endereco = Helpers.Settings.EnderecoSistema() + "/Content/temp/AlunosMatriculados.pdf";
            if (System.IO.File.Exists(endereco))
            {
                System.IO.File.Delete(endereco);
            }

            biBoleto.MontarPdfRelatorio(lista.DataTable, endereco);
            Byte[] file = System.IO.File.ReadAllBytes(endereco);
            FileContentResult result = new FileContentResult(file, "application/pdf");

            return result;
        }

        /// <summary>
        /// Retorna uma lista no formato JSON com os dados para o relatório de AlunosBolsistas
        /// </summary>
        /// <returns></returns>
        public FileResult GerarPDFAlunosBolsistas()
        {
            GenericTools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            GenericTools.Request.ParametrosRequest parametros = new GenericTools.Request.ParametrosRequest();
            JsonFiltro(Request.Form["Filtro"], parametros);
            parametros.Paginar = false;

            Business.Relatorio biRelatorio = new Business.Relatorio();
            Prion.Generic.Models.Lista lista = biRelatorio.GerarRelatorioAlunosBolsistas(parametros);

            Business.Boleto biBoleto = new Business.Boleto();
            String endereco = Helpers.Settings.EnderecoSistema() + "/Content/temp/AlunosBolsistas.pdf";
            if (System.IO.File.Exists(endereco))
            {
                System.IO.File.Delete(endereco);
            }

            biBoleto.MontarPdfRelatorio(lista.DataTable, endereco);
            Byte[] file = System.IO.File.ReadAllBytes(endereco);
            FileContentResult result = new FileContentResult(file, "application/pdf");

            return result;
        }

        /// <summary>
        /// Retorna uma lista no formato JSON com os dados para o relatório de AlunosInadimplentes
        /// </summary>
        /// <returns></returns>
        public FileResult GerarPDFAlunosInadimplentes()
        {
            GenericTools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            GenericTools.Request.ParametrosRequest parametros = new GenericTools.Request.ParametrosRequest();
            JsonFiltro(Request.Form["Filtro"], parametros);
            parametros.Paginar = false;

            Business.Relatorio biRelatorio = new Business.Relatorio();
            Prion.Generic.Models.Lista lista = biRelatorio.GerarRelatorioAlunosInadimplentes(parametros);

            Business.Boleto biBoleto = new Business.Boleto();
            String endereco = Helpers.Settings.EnderecoSistema() + "/Content/temp/AlunosInadimplentes.pdf";
            if (System.IO.File.Exists(endereco))
            {
                System.IO.File.Delete(endereco);
            }

            biBoleto.MontarPdfRelatorio(lista.DataTable, endereco);
            Byte[] file = System.IO.File.ReadAllBytes(endereco);
            FileContentResult result = new FileContentResult(file, "application/pdf");

            return result;
        }

        /// <summary>
        /// Retorna uma lista no formato JSON com os dados para o relatório de AlunosIsentos
        /// </summary>
        /// <returns></returns>
        public ActionResult GerarPDFAlunosIsentos()
        {
            GenericTools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            GenericTools.Request.ParametrosRequest parametros = new GenericTools.Request.ParametrosRequest();
            JsonFiltro(Request.Form["Filtro"], parametros);
            parametros.Paginar = false;

            Business.Relatorio biRelatorio = new Business.Relatorio();
            Prion.Generic.Models.Lista lista = biRelatorio.GerarRelatorioAlunosIsentos(parametros);

            Business.Boleto biBoleto = new Business.Boleto();
            String endereco = Helpers.Settings.EnderecoSistema() + "/Content/temp/AlunosIsentos.pdf";
            if (System.IO.File.Exists(endereco))
            {
                System.IO.File.Delete(endereco);
            }

            biBoleto.MontarPdfRelatorio(lista.DataTable, endereco);
            Byte[] file = System.IO.File.ReadAllBytes(endereco);
            FileContentResult result = new FileContentResult(file, "application/pdf");

            return result;
        }

        /// <summary>
        /// Retorna uma lista no formato JSON com os dados para o relatório de Mensalidades Atrasadas
        /// </summary>
        /// <returns></returns>
        public FileResult GerarPDFMensalidadesAtrasadas()
        {
            GenericTools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            GenericTools.Request.ParametrosRequest parametros = new GenericTools.Request.ParametrosRequest();
            JsonFiltro(Request.Form["Filtro"], parametros);
            parametros.Paginar = false;

            Business.Relatorio biRelatorio = new Business.Relatorio();
            Prion.Generic.Models.Lista lista = biRelatorio.GerarRelatorioMensalidadesAtrasadas(parametros);

            Business.Boleto biBoleto = new Business.Boleto();
            String endereco = Helpers.Settings.EnderecoSistema() + "/Content/temp/MensalidadesAtrasadas.pdf";
            if (System.IO.File.Exists(endereco))
            {
                System.IO.File.Delete(endereco);
            }

            biBoleto.MontarPdfRelatorio(lista.DataTable, endereco);
            Byte[] file = System.IO.File.ReadAllBytes(endereco);
            FileContentResult result = new FileContentResult(file, "application/pdf");

            return result;
        }

        /// <summary>
        /// Retorna uma lista no formato JSON com os dados para o relatório de Mensalidades Adiantadas
        /// </summary>
        /// <returns></returns>
        public FileResult GerarPDFMensalidadesAdiantadas()
        {
            GenericTools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            GenericTools.Request.ParametrosRequest parametros = new GenericTools.Request.ParametrosRequest();
            JsonFiltro(Request.Form["Filtro"], parametros);
            parametros.Paginar = false;

            Business.Relatorio biRelatorio = new Business.Relatorio();
            Prion.Generic.Models.Lista lista = biRelatorio.GerarRelatorioMensalidadesAdiantadas(parametros);

            Business.Boleto biBoleto = new Business.Boleto();
            String endereco = Helpers.Settings.EnderecoSistema() + "/Content/temp/MensalidadesAdiantadas.pdf";
            if (System.IO.File.Exists(endereco))
            {
                System.IO.File.Delete(endereco);
            }

            biBoleto.MontarPdfRelatorio(lista.DataTable, endereco);
            Byte[] file = System.IO.File.ReadAllBytes(endereco);
            FileContentResult result = new FileContentResult(file, "application/pdf");

            return result;
        }

        #endregion "PDF Local"

        /// <summary>
        /// Transforma um JSON contendo os filtros para montar o relatório em Request.Filtro
        /// </summary>
        /// <param name="json">string no formato JSON</param>
        /// <param name="parametros"></param>
        /// <returns>Um ParametroRequest com seus respectivos filtros adicionados</returns>
        private Prion.Tools.Request.ParametrosRequest JsonFiltro(String json, Prion.Tools.Request.ParametrosRequest parametros)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            js.RegisterConverters(new JavaScriptConverter[] { new Prion.Tools4.DynamicJsonConverter() });
            dynamic result = js.Deserialize(json, typeof(object)) as dynamic;

            /**
             * o formato do JSON varia de acordo com o tipo
             * JSON de Filtro por Periodo
                "Tipo" : "Periodo",
                "DataInicial":"00-00-0000",
                "DataFinal":"00-00-0000",
            **/
            for (Int32 i = 0; i < result.Filtros.Count; i++)
            {
                String tipoFiltro = result.Filtros[i]["Filtro.Tipo"];
                String tipoRelatorio = result.Filtros[i]["Relatorio.Tipo"];
                GenericTools.Request.Filtro f = null;
                String dataInicial,dataFinal;
                Int32 idSituacao,idAluno,idTurma,idCurso;

                switch (tipoFiltro)
                {
                    case "Periodo":
                        dataInicial = "'" + Convert.ToDateTime(result.Filtros[i]["Filtro.DataInicial"]).ToString("yyyy-MM-dd") + "'";
                        dataFinal = "'" + Convert.ToDateTime(result.Filtros[i]["Filtro.DataFinal"]).ToString("yyyy-MM-dd") + "'";
                        if (tipoRelatorio == "ContasPagas" || tipoRelatorio == "ContasRecebidas")
                        {
                            f = new GenericTools.Request.Filtro("Titulo.DataOperacao", "BETWEEN", dataInicial, dataFinal);
                            parametros.Filtro.Add(f);                           
                        }
                        if (tipoRelatorio == "ContasAPagar" || tipoRelatorio == "ContasAReceber")
                        {
                            f = new GenericTools.Request.Filtro("Titulo.DataVencimento", "BETWEEN", dataInicial, dataFinal);
                            parametros.Filtro.Add(f);
                        }
                        if (tipoRelatorio == "AlunosMatriculados")
                        {
                            //f = new GenericTools.Request.Filtro("Matricula.DataCadastro", "BETWEEN", dataInicial, dataFinal);
                            //parametros.Filtro.Add(f); 
                        }
                        if (tipoRelatorio == "MovimentacaoFinanceira" || tipoRelatorio == "MovimentacaoFinanceiraResponsavel")
                        {
                            f = new GenericTools.Request.Filtro("Boleto.DataPagamento", "BETWEEN", dataInicial, dataFinal);
                            GenericTools.Request.Filtro f2 = new GenericTools.Request.Filtro("Boleto.DataVencimento", "BETWEEN", dataInicial, dataFinal);

                            parametros.Filtro.Add(f);
                            parametros.Filtro.Add(f2);
                        }
                        break;

                    case "Situacao":
                        idSituacao = Convert.ToInt32(result.Filtros[i]["Situacao"]);
                        if (tipoRelatorio == "MovimentacaoFinanceira")
                        {
                            f = new GenericTools.Request.Filtro("MovimentacaoFinanceiraTitulo.IdSituacao", "=", idSituacao);
                            parametros.Filtro.Add(f);
                        }
                        break;

                    case "Turma":
                        idTurma = Convert.ToInt32(result.Filtros[i]["Filtro.Turma"]);
                        f = new GenericTools.Request.Filtro("Turma.Id", "IN", idTurma);
                        parametros.Filtro.Add(f);
                        break;

                    case "Aluno":
                        idAluno = Convert.ToInt32(result.Filtros[i]["Filtro.Aluno"]);
                        f = new GenericTools.Request.Filtro("Aluno.Id", "IN", idAluno);
                        parametros.Filtro.Add(f);
                        break;

                    case "CursoTurma":
                        idCurso =Convert.ToInt32(result.Filtros[i]["Filtro.Curso"]);
                        idTurma =(result.Filtros[i]["Filtro.Turma"] !="")? Convert.ToInt32(result.Filtros[i]["Filtro.Turma"]):0;
                        if (idTurma != 0)
                        {                            
                            idAluno =(result.Filtros[i]["Filtro.Aluno"]!="")? Convert.ToInt32(result.Filtros[i]["Filtro.Aluno"]):0;
                                f = new GenericTools.Request.Filtro("Turma.Id", "IN", idTurma);
                                if (idAluno != 0) { parametros.Filtro.Add(new GenericTools.Request.Filtro("Aluno.Id", "IN", idAluno)); }                                                        
                        }
                        else
                        {
                            f = new GenericTools.Request.Filtro("CursoAnoLetivo.Id", "IN", idCurso);
                        }
                        parametros.Filtro.Add(f);
                        break;

                }

            }
            return parametros;
        }


      

        #endregion 
    }
}