using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotaAzul.Models
{
    public class OperacaoNetEmpresa
    {
        public string Tipo { get; set; }
        public string Pagador { get; set; }
        public string SeuNumero { get; set; }
        public string NossoNumero { get; set; }
        public string Matricula {
            get {
                return string.IsNullOrEmpty(this.SeuNumero) ? "" : this.SeuNumero.Split('/').FirstOrDefault();
            }
        } 
        public decimal ValorTitulo { get; set; }
        public decimal ValorPago { get; set; }
        public decimal ValorOscilacao { get; set; }
        public DateTime DataVencimento { get; set; }
        public string NomeArquivo { get; set; }
        public DateTime DataLeitura { get; set; }
    }
}
