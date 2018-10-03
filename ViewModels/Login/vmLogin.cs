using System.ComponentModel.DataAnnotations;

namespace NotaAzul.ViewModels.Login
{
    public class vmLogin
    {
        [Required(ErrorMessage="O campo login é obrigatório")]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required(ErrorMessage="O campo senha é obrigatório")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; }
    }
}