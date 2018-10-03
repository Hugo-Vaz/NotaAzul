using System;
using System.Web.Mvc;
using GenericHelpers = Prion.Generic.Helpers;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Linq;


namespace NotaAzul.Controllers
{
    public class AlunoController : BaseController
    {
        #region "Views"

        /// <summary>
        /// Retorna a view Novo
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewDados()
        {
            ViewModels.Aluno.vmDados vmDados = new ViewModels.Aluno.vmDados();

            //carrega a lista de situacoes de um Aluno
            Business.Situacao biSituacao = new Business.Situacao();
            vmDados.Situacoes = biSituacao.CarregarSituacoesPeloTipo("Aluno");
            vmDados.SituacoesResponsavel = biSituacao.CarregarSituacoesPeloTipo("AlunoResponsavel");
            vmDados.SituacaoEndereco = biSituacao.CarregarSituacoesPelaSituacao("AlunoResponsavelEndereco", "Ativo").Id;

            return View("Dados", vmDados);
        }


        /// <summary>
        /// Retorna a view Lista, com os registros de Aluno
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewLista()
        {
            return View("Lista");
        }


        #endregion "Views"


        #region "Requisições"

        /// <summary>
        /// Carrega um registro do tipo Models.Aluno atraves do seu Id
        /// </summary>
        /// <param name="id">id do Aluno</param>
        /// <returns></returns>
        public ActionResult Carregar(Int32 id)
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);

            Business.Aluno biAluno = new Business.Aluno();
            Models.Aluno aluno = (Models.Aluno)biAluno.Carregar(parametros, id).Get(0);

            return Json(new { success = true, obj = aluno }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Carrega um registro do tipo Models.Matricula atraves do IdAluno
        /// </summary>
        /// <param name="id">id do Aluno</param>
        /// <returns></returns>
        public ActionResult CarregarFichaFinanceira(Int32 id)
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);

            Business.Aluno biAluno = new Business.Aluno();
            Models.Matricula matricula = biAluno.CarregarFichaFinanceira(parametros, id);

            return Json(new { success = true, obj = matricula }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Exclui um Aluno pelo Id
        /// </summary>
        /// <param name="id">id do Aluno</param>
        /// <returns>Um retorno no formato de Json</returns>
        public ActionResult Excluir()
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.Aluno biAluno = new Business.Aluno();
            GenericHelpers.Retorno retorno = biAluno.Excluir(Prion.Tools.Conversor.ToInt(Request.Form["id"]));

            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }


        /// <summary>
        /// Salva os dados de um Aluno
        /// </summary>
        /// <param name="vModel"></param>
        /// <returns></returns>
        public ActionResult Salvar(ViewModels.Aluno.vmDados vModel)
        {
            Prion.Generic.Helpers.Mensagem mensagem = new GenericHelpers.Mensagem();
            Business.Aluno biAluno = new Business.Aluno();
            Business.Mensalidade biMensalidade = new Business.Mensalidade();

            List<Models.AlunoResponsavel> listaResponsaveis = JsonResponsaveis(Request.Form["responsaveis"], vModel.Aluno.Id);
            vModel.Aluno.Responsaveis = listaResponsaveis;
            Boolean alterarDataVencimento = Convert.ToBoolean(Request.Form["alterarDataVencimento"]);
            if (alterarDataVencimento == true)
            {
                biMensalidade.AlterarDiaVencimento(vModel.Aluno.DiaPagamento, vModel.Aluno.Id);
            }
            GenericHelpers.Retorno retorno = biAluno.Salvar(vModel.Aluno);

            mensagem.EstadoObjeto = retorno.EstadoObjeto;
            mensagem.UltimoId = retorno.UltimoId;
            mensagem.Tipo = (retorno.Sucesso) ? GenericHelpers.TipoMensagem.Sucesso : GenericHelpers.TipoMensagem.Erro;
            mensagem.TextoMensagem = retorno.Mensagem;
            mensagem.Sucesso = retorno.Sucesso;

            return Json(new { success = mensagem.Sucesso, mensagem = mensagem });
        }


        /// <summary>
        /// Retornar uma lista no formato JSON do tipo Models.Aluno
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLista()
        {
            Prion.Tools.Request.ProcessarRequest request = new Prion.Tools.Request.ProcessarRequest();
            Prion.Tools.Request.ParametrosRequest parametros = request.Processar(Sistema.Lista, Request);
            Business.Aluno biAluno = new Business.Aluno();
            parametros = JsonFiltro(Request.Form["Query"], parametros);
            Prion.Generic.Models.Lista lista = biAluno.ListaAlunos(parametros);
            String rowsStr = Prion.Tools.Conversor.ToJson(lista.DataTable);

            return Json(new { success = true, page = parametros.Inicio, total = lista.Count, rows = rowsStr }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Retorna um JSON contendo uma lista de alunos
        /// </summary>
        /// <param name="term">uma string representando um nome(ou parte) de um aluno</param>
        /// <returns>Json contendo objetos do tipo Models.Aluno</returns>
        public ActionResult ListaAlunoPorNome(String term)
        {
            Business.Aluno biAluno = new Business.Aluno();
            Prion.Generic.Models.Lista lista = biAluno.Carregar(term);
            List<Models.Aluno> listaAluno = Prion.Tools.ListTo.CollectionToList<Models.Aluno>(lista.ListaObjetos);

            if (listaAluno == null) { return Json(new { label = "Nada foi encontrado" }, JsonRequestBehavior.AllowGet); }

            var result = from aluno in listaAluno select new { label = aluno.Nome, value = aluno.Id };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #endregion


        #region "Requisições Responsável"

        /// <summary>
        /// Retorna um JSON contendo uma lista de responsáveis
        /// </summary>
        /// <param name="term">uma string representando um nome(ou parte) de um responsável</param>
        /// <returns>Json contendo objetos do tipo Models.AlunoResponsavel</returns>
        public ActionResult ListaResponsavelPorNome(String term)
        {
            Business.Aluno biAluno = new Business.Aluno();
            Prion.Generic.Models.Lista lista = biAluno.CarregarResponsaveis(term);
            List<Models.AlunoResponsavel> listaResponsavel = Prion.Tools.ListTo.CollectionToList<Models.AlunoResponsavel>(lista.ListaObjetos);

            if (listaResponsavel == null) { return Json(new { label = "Nada foi encontrado" }, JsonRequestBehavior.AllowGet); }

            var result = from responsavel in listaResponsavel select new { label = responsavel.Nome, value = responsavel.Id };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Transforma um JSON de responsaveis em uma lista de objetos do tipo Models.AlunoResponsavel
        /// </summary>
        /// <param name="json">string no formato JSON</param>
        /// <returns>Lista de objetos do tipo Models.AlunoResponsavel</returns>
        private List<Models.AlunoResponsavel> JsonResponsaveis(String json, Int32 idAluno)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            
            js.RegisterConverters(new JavaScriptConverter[] { new Prion.Tools4.DynamicJsonConverter() });
            dynamic result = js.Deserialize(json, typeof(object)) as dynamic;
            List<Models.AlunoResponsavel> listaResponsaveis = new List<Models.AlunoResponsavel>();

            /**
             * exemplo de JSON de responsável
                "Id" : "",
                "Apelido" : "Principal",
                "Nome" : "responsável",
                "CPF" : "000.000.000-00",
                "GrauParentesco" : "avó",
                "Profissao" : "profissão",
                "IdSituacao" : "23",
                "Financeiro" : boolean
                "Endereco" : {
                    "IdSituacao" : "26",
                    "DadosEndereco" : "endereço",
                    "Numero" : "numero",
                    "Complemento" : "complemento",
                    "Cep" : "28615-066",
                    "Referencia" : "referencia",
                    "IdEstado" : "19",
                    "IdCidade" : "20",
                    "IdBairro" : "17"
                },
                "Telefones: [{
                    "Ddd": "22",
                    "Numero": "25231398",
                    "Observacao": "Observacao",
                    "Preferencial": "1" ou "0",
                    "TipoTelefone": "Comercial"
                }]
            **/

            for (Int32 i = 0; i < result.Length; i++)
            {
                Models.AlunoResponsavel responsavel = new Models.AlunoResponsavel();
                responsavel.Id = Convert.ToInt32(result[i].Id);
                responsavel.IdSituacao = Convert.ToInt32(result[i].IdSituacao);
                responsavel.IdAluno = idAluno;
                responsavel.EstadoObjeto = (Prion.Generic.Helpers.Enums.EstadoObjeto)result[i].EstadoObjeto;
                responsavel.Nome = result[i].Nome;
                responsavel.CPF = result[i].CPF.Replace(".", "").Replace("-", "");
                responsavel.GrauParentesco = result[i].GrauParentesco;
                responsavel.Profissao = result[i].Profissao;
                responsavel.RG = result[i].RG;
                responsavel.Email = result[i].Email;
                responsavel.MoraCom = (Convert.ToString(result[i].MoraCom).ToLower() == "true" || Convert.ToString(result[i].MoraCom) == "1");
                responsavel.Financeiro = (Convert.ToString(result[i].Financeiro).ToLower() == "true" || Convert.ToString(result[i].Financeiro) == "1");
      
                // se o responsável estiver marcado como excluído, não há necessidade de percorrer endereço e telefone
                if (responsavel.EstadoObjeto == GenericHelpers.Enums.EstadoObjeto.Excluido)
                {
                    listaResponsaveis.Add(responsavel);
                    continue;
                }

                // obtém os dados do endereço que estão no objeto 'Endereco'
                // dúvidas? Veja o exemplo do JSON logo acima
                responsavel.Endereco.Id = Convert.ToInt32(result[i].Endereco.Id);
                responsavel.Endereco.EstadoObjeto = (Prion.Generic.Helpers.Enums.EstadoObjeto)result[i].Endereco.EstadoObjeto;
                responsavel.Endereco.IdSituacao = Convert.ToInt32(result[i].Endereco.IdSituacao);
                responsavel.Endereco.IdEstado = Convert.ToInt32(result[i].Endereco.IdEstado);
                responsavel.Endereco.IdCidade = Convert.ToInt32(result[i].Endereco.IdCidade);
                responsavel.Endereco.IdBairro = Convert.ToInt32(result[i].Endereco.IdBairro);
                responsavel.Endereco.Apelido = "Principal";
                responsavel.Endereco.DadosEndereco = result[i].Endereco.DadosEndereco;
                responsavel.Endereco.Numero = result[i].Endereco.Numero;
                responsavel.Endereco.Complemento = result[i].Endereco.Complemento;
                responsavel.Endereco.Cep = result[i].Endereco.Cep;
                responsavel.Endereco.Referencia = result[i].Endereco.Referencia;

                // verifica se o objeto de telefone é == null
                // se existe algum telefone na lista
                // se o último telefone da lista é vazio e o seu atributo EstadoObjeto é diferente à Excluido
                if ( 
                        (result[i].Telefones == null) || 
                        (result[i].Telefones.Count == 0) || 
                        (result[i].Telefones[result[i].Telefones.Count - 1]["Numero"] == "" && (Prion.Generic.Helpers.Enums.EstadoObjeto)Convert.ToInt32(result[i].Telefones[result[i].Telefones.Count - 1]["EstadoObjeto"]) != Prion.Generic.Helpers.Enums.EstadoObjeto.Excluido))
                {
                    listaResponsaveis.Add(responsavel);
                    continue;
                }


                // obtém os dados do telefone que estão no objeto "Telefones"
                for (Int32 j = 0; j < result[i].Telefones.Count; j++)
                {
                    NotaAzul.Models.AlunoResponsavelTelefone telefone = new Models.AlunoResponsavelTelefone();
                    telefone.EstadoObjeto = (Prion.Generic.Helpers.Enums.EstadoObjeto)Convert.ToInt32(result[i].Telefones[j]["EstadoObjeto"]);

                    // verifica se o telefone da iteração atual não foi Excluido
                    if ((Prion.Generic.Helpers.Enums.EstadoObjeto)Convert.ToInt32(result[i].Telefones[j]["EstadoObjeto"]) != Prion.Generic.Helpers.Enums.EstadoObjeto.Excluido)
                    {   
                        telefone.Ddd = result[i].Telefones[j]["Ddd"];
                        telefone.Numero = result[i].Telefones[j]["Numero"];
                        telefone.Observacao = result[i].Telefones[j]["Observacao"];

                        // se o objeto ainda estiver como Consultado, os atributos "Preferencial" e "TipoTelefone" estarão como boolean
                        if ((Prion.Generic.Helpers.Enums.EstadoObjeto)Convert.ToInt32(result[i].Telefones[j]["EstadoObjeto"]) == Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado)
                        {
                            telefone.Preferencial = result[i].Telefones[j]["Preferencial"];
                            telefone.TipoTelefone = result[i].Telefones[j]["TipoTelefone"];
                            telefone.EstadoObjeto = Prion.Generic.Helpers.Enums.EstadoObjeto.Consultado;
                        }
                        else
                        {
                            // se caiu aqui é porque o EstadoObjeto esta como Alterado ou Novo, nesse caso os atributos "Preferencial" e "TipoTelefone" estarão como string
                            telefone.Preferencial = ((result[i].Telefones[j]["Preferencial" + j]) == "true" || result[i].Telefones[j]["Preferencial" + j] == "True");
                            telefone.TipoTelefone = result[i].Telefones[j]["TipoTelefone" + j];
                        }
                    }

                    telefone.IdAlunoResponsavel = Convert.ToInt32(result[i].Id);
                    telefone.Id = Convert.ToInt32(result[i].Telefones[j]["Id"]);
                    

                    responsavel.Telefones.Add(telefone);
                }

                listaResponsaveis.Add(responsavel);
            }

            return listaResponsaveis;
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ResponsavelExcluir()
        {
            return null;
        }

        #endregion "Requisições Responsável"
    }
}
