using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GenericModels = Prion.Generic.Models;

namespace NotaAzul.ViewModels.Relatorios
{
    public class vmDados
    {
        public GenericModels.Corporacao Corporacao { get; set; }
        public List<GenericModels.Situacao> Situacoes { get; set; }
    }
}