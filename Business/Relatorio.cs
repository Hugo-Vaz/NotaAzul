using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;


namespace NotaAzul.Business
{
    public class Relatorio:Base
    {
        /// <summary>
        /// Construtor da Classe
        /// </summary>
        public Relatorio()
        { 
        }

        /// <summary>
        /// Obtém um DataTable que será utilizado no relatório de Contas pagas em um determinado período de tempo
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista GerarRelatorioContasPagas(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Relatorio repRelatorio = new Repository.Relatorio(ref this.Conexao);
            repRelatorio.Entidades.Adicionar(parametro.Entidades);

            return repRelatorio.GerarRelatorioContasPagas(parametro);
        }

        /// <summary>
        /// Obtém um DataTable que será utilizado no relatório de Contas à pagar em um determinado período de tempo
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista GerarRelatorioContasAPagar(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Relatorio repRelatorio = new Repository.Relatorio(ref this.Conexao);
            repRelatorio.Entidades.Adicionar(parametro.Entidades);

            return repRelatorio.GerarRelatorioContasAPagar(parametro);
        }

        /// <summary>
        /// Obtém um DataTable que será utilizado no relatório de Contas recebidas em um determinado período de tempo
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista GerarRelatorioContasRecebidas(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Relatorio repRelatorio = new Repository.Relatorio(ref this.Conexao);
            repRelatorio.Entidades.Adicionar(parametro.Entidades);

            return repRelatorio.GerarRelatorioContasRecebidas(parametro);
        }

        /// <summary>
        /// Obtém um DataTable que será utilizado no relatório de Contas à receber em um determinado período de tempo
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista GerarRelatorioContasAReceber(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Relatorio repRelatorio = new Repository.Relatorio(ref this.Conexao);
            repRelatorio.Entidades.Adicionar(parametro.Entidades);

            return repRelatorio.GerarRelatorioContasAReceber(parametro);
        }
        
        /// <summary>
        /// Obtém um DataTable que será utilizado no relatório de Alunos matriculados em um determinado período de tempo
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista GerarRelatorioAlunosMatriculados(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Relatorio repRelatorio = new Repository.Relatorio(ref this.Conexao);
            repRelatorio.Entidades.Adicionar(parametro.Entidades);

            return repRelatorio.GerarRelatorioAlunosMatriculados(parametro);
        }

        /// <summary>
        /// Obtém um DataTable que será utilizado no relatório de Alunos bolsistas em um determinado período de tempo
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista GerarRelatorioAlunosBolsistas(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Relatorio repRelatorio = new Repository.Relatorio(ref this.Conexao);
            repRelatorio.Entidades.Adicionar(parametro.Entidades);

            return repRelatorio.GerarRelatorioAlunosBolsistas(parametro);
        }

        /// <summary>
        /// Obtém um DataTable que será utilizado no relatório de Alunos inadimplentes em um determinado período de tempo
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista GerarRelatorioAlunosInadimplentes(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Relatorio repRelatorio = new Repository.Relatorio(ref this.Conexao);
            repRelatorio.Entidades.Adicionar(parametro.Entidades);

            return repRelatorio.GerarRelatorioAlunosInadimplentes(parametro);
        }

        /// <summary>
        /// Obtém um DataTable que será utilizado no relatório de Alunos isentos em um determinado período de tempo
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista GerarRelatorioAlunosIsentos(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Relatorio repRelatorio = new Repository.Relatorio(ref this.Conexao);
            repRelatorio.Entidades.Adicionar(parametro.Entidades);

            return repRelatorio.GerarRelatorioAlunosIsentos(parametro);
        }
       

        /// <summary>
        /// Obtém um DataTable que será utilizado no relatório de movimentações financeiras realizadas em um determinado período de tempo
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista GerarRelatorioMovimentacaoFinanceira(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Relatorio repRelatorio = new Repository.Relatorio(ref this.Conexao);
            repRelatorio.Entidades.Adicionar(parametro.Entidades);

            return repRelatorio.GerarRelatorioMovimentacaoFinanceira(parametro);
        }

        /// <summary>
        /// Obtém um DataTable que será utilizado no relatório de movimentações financeiras realizadas em um determinado período de tempo,contendo informações do pagador
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista GerarRelatorioMovimentacaoFinanceiraResponsavel(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Relatorio repRelatorio = new Repository.Relatorio(ref this.Conexao);
            repRelatorio.Entidades.Adicionar(parametro.Entidades);

            return repRelatorio.GerarRelatorioMovimentacaoFinanceiraResponsavel(parametro);
        }

        /// <summary>
        /// Obtém um DataTable que será utilizado no relatório de mensalidades atrasadas
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista GerarRelatorioMensalidadesAtrasadas(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Relatorio repRelatorio = new Repository.Relatorio(ref this.Conexao);
            repRelatorio.Entidades.Adicionar(parametro.Entidades);

            return repRelatorio.GerarRelatorioMensalidadesAtrasadas(parametro);
        }

        /// <summary>
        /// Obtém um DataTable que será utilizado no relatório de mensalidades adiantadas
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista GerarRelatorioMensalidadesAdiantadas(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Relatorio repRelatorio = new Repository.Relatorio(ref this.Conexao);
            repRelatorio.Entidades.Adicionar(parametro.Entidades);

            return repRelatorio.GerarRelatorioMensalidadesAdiantadas(parametro);
        }

        /// <summary>
        /// Obtém um DataTable que será utilizado no relatório de Contas pagas em um determinado período de tempo
        /// </summary>
        /// <returns></returns>
        public void GerarRelatorioEmail()
        {
            Repository.Relatorio repRelatorio = new Repository.Relatorio(ref this.Conexao);
            Models.DadosCobranca resultado = repRelatorio.GerarRelatorioParaCobranca();
        }

        /// <summary>
        /// Obtém um DataTable que será utilizado no relatório de Contas à pagar em um determinado período de tempo
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista GerarRelatorioAlunosInadimplentesBoleto(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Relatorio repRelatorio = new Repository.Relatorio(ref this.Conexao);
            repRelatorio.Entidades.Adicionar(parametro.Entidades);

            return repRelatorio.GerarRelatorioAlunosInadimplentesBoleto(parametro);
        }

        /// <summary>
        /// Obtém um DataTable que será utilizado no relatório de Contas à pagar em um determinado período de tempo
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista GerarRelatorioBoletosQuitados(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Relatorio repRelatorio = new Repository.Relatorio(ref this.Conexao);
            repRelatorio.Entidades.Adicionar(parametro.Entidades);

            return repRelatorio.GerarRelatorioBoletoSQuitados(parametro);
        }

        /// <summary>
        /// Obtém um DataTable que será utilizado no relatório de Contas à pagar em um determinado período de tempo
        /// </summary>
        /// <returns></returns>
        public Prion.Generic.Models.Lista GerarRelatorioArquivoRet(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Relatorio repRelatorio = new Repository.Relatorio(ref this.Conexao);
            repRelatorio.Entidades.Adicionar(parametro.Entidades);

            return repRelatorio.GerarRelatorioArquivoRetorno(parametro);
        }

        public Prion.Generic.Models.Lista GerarRelatorioNetEmpresa(Prion.Tools.Request.ParametrosRequest parametro = null)
        {
            Repository.Relatorio repRelatorio = new Repository.Relatorio(ref this.Conexao);
            repRelatorio.Entidades.Adicionar(parametro.Entidades);

            return repRelatorio.GerarRelatorioNetEmpresa(parametro);
        }
    }
}