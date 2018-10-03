using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotaAzul.Models
{
    public class BoletoQuitado
    {
        public string Identificador { get; set; }
        public decimal ValorPago { get; set; }
        public DateTime DataOperacao { get; set; }
        public string NomeAluno { get; set; }
        public string NomeResponsavel { get; set; }
    }
}