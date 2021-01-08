using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace prova_alexandre.Models
{

    public class Cartao
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [CreditCard(ErrorMessage = "Número inválido.")]
        public string numero { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string data_expiracao { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string bandeira { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MinLength(3, ErrorMessage = "Código de verificação inválido.")]
        [MaxLength(3, ErrorMessage = "Código de verificação inválido.")]
        public string cvv { get; set; }
    }
}
