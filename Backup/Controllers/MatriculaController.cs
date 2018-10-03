using System;
using System.Web.Mvc;
using GenericHelpers = Prion.Generic.Helpers;
using System.Web.Script.Serialization;
using System.Collections.Generic;


namespace NotaAzul.Controllers
{
    public class MatriculaController : BaseController
    {
        #region "Views"

        /// <summary>
        /// Retorna a view Lista, com os registros de Matricula
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
            ViewModels.Matricula.vmDados vmDados = new ViewModels.Matricula.vmDados();
            Business.Situacao biSituacao = new Business.Situacao();
           
            // Carrega a lista de situações de um Matricula
            vmDados.Situacoes = biSituacao.CarregarSituacoesPeloTipo("Matrícula");            
            return View("Dados", vmDados);
        }

        /// <summary>
        /// Retorna a view Ficha
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewFicha()
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

            return View("Ficha", vmDadosCorporacao);
        }

        /// <summary>
        /// Retorna a view CarneMensalidade
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewCarneMensalidade()
        {
            ViewModels.Corporacao.vmDados vmDadosCorporacao = new ViewModels.Corporacao.vmDados();

            Business.Corporacao biCorporacao = new Business.Corporacao();
            vmDadosCorporacao.Corporacao = biCorporacao.Carregar(Sistema.UsuarioLogado.IdCorporacao);

            return View("CarneMensalidade", vmDadosCorporacao);
        }
        #endregion "Views"


        #region "Requisições"

        /// <summary>
        /// Carrega um registro do tipo Models.Matricula atráves do seu ID
        /// </summary>
        /// <param name="id">id do Matricula</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
                       
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            
            Business.Matricula biMatricula = new Business.Matricula();
            Models.Matricula matricula = (Models.Matricula)biMatricula.Carregar(parametros,id).Get(0);

            return Json(new { success = true, obj = matricula }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Exclui um Matricula pelo ID
        /// </summary>
        /// <param name="id">id do Matricula</param>
        /// <returns>retorno no formato JSON</returns>
        public ActionResult Excluir()
        {
            Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
            Business.Matricula biMatricula = new Business.Matricula();
            GenericHelpers.Retorno retorno = biMatricula.Excluir(Prion.Tools.Conversor.ToInt(Request.Form["id"]));

            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = (retorno.Sucesso) ? "Matricula excluída com sucesso" : retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }     

        /// <summary>
        /// Salva os dados de um Matricula
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult Salvar(ViewModels.Matricula.vmDados vModel)
        {
            Prion.Generic.Helpers.Mensagem mensagem = new Prion.Generic.Helpers.Mensagem();
            Business.Matricula biMatricula = new Business.Matricula();

            List<Models.MatriculaCurso> listaMatriculaCurso = JsonMatriculaCurso(Request.Form["MatriculaCurso"], vModel.Matricula);
            Int32 valorBolsa = Convert.ToInt32(Request.Form["BolsistaValor"].ToString());
            Int32 anoLetivo = Convert.ToInt32(Request.Form["Matricula.CursoAnoLetivo.AnoLetivo"].ToString());

            if (valorBolsa != 0)
            {
                vModel.Matricula.AlunoFilantropia = MontarFilantropia(valorBolsa, anoLetivo, vModel.Matricula);
            }

            vModel.Matricula.ListaMatriculaCurso = listaMatriculaCurso;
            GenericHelpers.Retorno retorno = biMatricula.Salvar(vModel.Matricula);
            
            mensagem.EstadoObjeto = retorno.EstadoObjeto;
            mensagem.UltimoId = retorno.UltimoId;
            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }

        /// <summary>
        /// Retorna uma lista no formato JSON de objetos do tipo Models.Matricula
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLista()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.Matricula biMatricula = new Business.Matricula();
            parametros = JsonFiltro(Request.Form["Query"], parametros);

            Prion.Generic.Models.Lista lista = biMatricula.ListaMatriculas(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Transforma um JSON de mensalidades em uma lista de objetos do tipo Models.MatriculaCurso
        /// </summary>
        /// <param name="json">string no formato JSON</param>
        /// <param name="matricula">objeto do tipo Models.Matricula</param>
        /// <returns>Lista de objetos do tipo Models.Mensalidade</returns>
        private List<Models.MatriculaCurso> JsonMatriculaCurso(String json, Models.Matricula matricula)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();           
            js.RegisterConverters(new JavaScriptConverter[] { new Prion.Tools4.DynamicJsonConverter() });
            dynamic result = js.Deserialize(json, typeof(object)) as dynamic;


            /**
             * Exemplo Json de MatriculaCurso
                "Id":0,
                "Aluno":{
                     "IdAluno"="9", 
                     "DiaPagamento"="5",
                 }
                "MatriculaCurso":[{                
                    "IdCursoAnoLetivo":"2",
                    "Mensalidades":[
                        {
                        "MesStr":"Janeiro",
                        "Mes":2,
                        "Valor":300,
                        "Acrescimo":0,
                        "Desconto":0,
                        "Isento":false
                        }]
                    }]
                }
             */
            List<Models.MatriculaCurso> listaMatriculaCurso= new List<Models.MatriculaCurso>();
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);

            Prion.Generic.Models.Situacao situacaoMensalidade = new Prion.Generic.Models.Situacao();
            Business.Situacao biSituacao = new Business.Situacao();
            Business.Aluno biAluno = new Business.Aluno();
           
            matricula.Aluno = (Models.Aluno)biAluno.Carregar(parametros,matricula.Aluno.Id).Get(0);

            situacaoMensalidade = biSituacao.CarregarSituacoesPelaSituacao("Mensalidade", "Aberta");
            Int16 mesMensalidade = 0;
            Int32 diaMensalidade = 0, diaDefaultMensalidade;            
            diaDefaultMensalidade = Prion.Tools.Conversor.ToInt32(Sistema.GetConfiguracaoSistema("mensalidade:dia_default_pagamento"));

            //Pega nas configurações do sistema,seo o sistema deverá jogar a mensalidade de um dia inválido para o último dia do mês ou para o primeiro do próximo
            //Padrões: ultimoDiaMesAtual ou primeiroDiaProximoMes
            Business.ConfiguracaoSistema biConfig = new Business.ConfiguracaoSistema();
            String trocaParaDiaValido = Sistema.GetConfiguracaoSistema("mensalidade:dia_default_troca");

            for (Int32 i = 0; i < result.MatriculaCurso.Count; i++)
            {
                Models.MatriculaCurso matriculaCurso = new Models.MatriculaCurso();
                matriculaCurso.EstadoObjeto = (Prion.Generic.Helpers.Enums.EstadoObjeto)Convert.ToInt32(result.MatriculaCurso[i]["EstadoObjeto"]);               
                matriculaCurso.Id = Convert.ToInt32(result.MatriculaCurso[i]["Id"]);
                matriculaCurso.IdMatricula = Convert.ToInt32(result.Id);
                matriculaCurso.IdTurma = Convert.ToInt32(result.MatriculaCurso[i]["IdTurma"]);

                // se não houver mensalidades...
                if (result.MatriculaCurso[i]["Mensalidades"] == null || result.MatriculaCurso[i]["Mensalidades"].Count == 0)
                {
                    // adiciona o objeto matriculaCurso
                    listaMatriculaCurso.Add(matriculaCurso);
                    continue;
                }

                // se chegou aqui é porque existe alguma mensalidade no curso da iteração atual
                for (Int32 j = 0; j < result.MatriculaCurso[i]["Mensalidades"].Count; j++)
                {
                    mesMensalidade = Convert.ToSByte(result.MatriculaCurso[i]["Mensalidades"][j]["Mes"]);

                    // se o dia de pagamento do aluno não tiver sido informado, pega o default da configuração do sistema, caso contrário pega do aluno
                    diaMensalidade = (matricula.Aluno.DiaPagamento == 0) ? diaDefaultMensalidade : matricula.Aluno.DiaPagamento;

                    Models.Mensalidade mensalidade = new Models.Mensalidade();
                    mensalidade.Id = Convert.ToInt32(result.MatriculaCurso[i]["Mensalidades"][j]["Id"]);
                    mensalidade.EstadoObjeto = (Prion.Generic.Helpers.Enums.EstadoObjeto)Convert.ToInt32(result.MatriculaCurso[i]["Mensalidades"][j]["EstadoObjeto"]); 
                    mensalidade.IdMatriculaCurso = matriculaCurso.Id;
                    situacaoMensalidade = biSituacao.CarregarSituacoesPelaSituacao("Mensalidade", result.MatriculaCurso[i]["Mensalidades"][j]["Situacao"]);
                    mensalidade.IdSituacao = situacaoMensalidade.Id;
                    mensalidade.Valor = Convert.ToDecimal(result.MatriculaCurso[i]["Mensalidades"][j]["Valor"]);
                    mensalidade.Acrescimo = Convert.ToDecimal(result.MatriculaCurso[i]["Mensalidades"][j]["Acrescimo"]);
                    mensalidade.Desconto = Convert.ToDecimal(result.MatriculaCurso[i]["Mensalidades"][j]["Desconto"]);
                    mensalidade.Isento = (Convert.ToString(result.MatriculaCurso[i]["Mensalidades"][j]["Isento"]).ToLower() == "true" 
                        || Convert.ToString(result.MatriculaCurso[i]["Mensalidades"][j]["Isento"]).ToLower() == "1");


                    // BLOCO PARA VALIDAÇÃO DE DATA "INEXISTENTE" (exemplo: 31/02/2014, 31/04/2014...)
                    // gera uma data baseada no mês e ano da mensalidade da iteração atual
                    DateTime primeiroDia = Convert.ToDateTime("01/" + mesMensalidade.ToString() + "/" + result.AnoLetivo);

                    // pega o último dia do mês e ano gerado acima
                    DateTime ultimoDia = primeiroDia.AddMonths(1).AddDays(-1);

                    if (diaMensalidade > ultimoDia.Day) {
                        if (trocaParaDiaValido.ToLower() == "primeirodiaproximomes")
                        {
                            // incrementa um dia na data atual
                            ultimoDia = ultimoDia.AddDays(1);
                            diaMensalidade = ultimoDia.Day;
                            mesMensalidade = Convert.ToInt16(ultimoDia.Month);
                        }
                        else
                        {
                            diaMensalidade = ultimoDia.Day;
                            mesMensalidade = Convert.ToInt16(ultimoDia.Month);
                        }
                    }

                    mensalidade.DataVencimento = new DateTime(Convert.ToInt32(result.AnoLetivo), mesMensalidade, diaMensalidade);
                    
                    //caso seja domingo ou sábado, o vencimento passará para segunda feira
                    if (mensalidade.DataVencimento.DayOfWeek == DayOfWeek.Sunday)
                    {
                        mensalidade.DataVencimento.AddDays(1);
                    }
                    else if (mensalidade.DataVencimento.DayOfWeek == DayOfWeek.Saturday)
                    {                        
                        mensalidade.DataVencimento.AddDays(2);
                    }

                    matriculaCurso.Mensalidades.Add(mensalidade);
                }

                listaMatriculaCurso.Add(matriculaCurso);
            }

            return listaMatriculaCurso;
        }

        /// <summary>
        /// Cria um objeto de aluno filantropia
        /// </summary>
        /// <param name="valor">valor</param>
        /// <param name="matricula">objeto do tipo Models.Matricula</param>
        /// <returns>um objeto de Models.AlunoFilantropia</returns>
        private Models.AlunoFilantropia MontarFilantropia(Int32 valor,Int32 anoLetivo, Models.Matricula matricula)
        {
            Models.AlunoFilantropia filantropia = new Models.AlunoFilantropia();
            filantropia.Aluno = matricula.Aluno;
            filantropia.Id = matricula.IdAlunoFilantropia;
            filantropia.EstadoObjeto = (filantropia.Id == 0) ? GenericHelpers.Enums.EstadoObjeto.Novo : GenericHelpers.Enums.EstadoObjeto.Alterado;
            filantropia.AnoLetivo = anoLetivo;
            filantropia.ValorBolsa = valor;

            return filantropia;
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

        #endregion
    }
}