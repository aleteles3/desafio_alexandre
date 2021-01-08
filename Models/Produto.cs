using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace prova_alexandre.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(1024, ErrorMessage = "Este campo suporte até 1024 caracteres.")]
        public string nome { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Este campo não deve conter valores negativos.")]
        public double valor_unitario { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Este campo não deve conter valores negativos.")]
        public int qtde_estoque { get; set; }

        public string data_ultima_venda { get; set; }

        public double valor_ultima_venda { get; set; }
    }
}
