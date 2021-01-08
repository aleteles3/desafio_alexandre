using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace prova_alexandre.Models
{
    public class Compra
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public int produto_id { get; set; }

        public Produto produto { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser maior que 0")]
        public int qtde_comprada { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public Cartao cartao { get; set; }

        public string data_compra { get; set; }
    }
}
