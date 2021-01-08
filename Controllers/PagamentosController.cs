using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prova_alexandre.Data;
using prova_alexandre.Models;

namespace prova_alexandre.Controllers
{
    [Route("api/pagamento/compras")]
    [ApiController]
    public class PagamentosController : ControllerBase
    {
        [HttpPost]
        public ActionResult<Pagamento> PostPagamento(Pagamento pagamento)
        {
            pagamento.estado = "REJEITADO";

            if (pagamento.valor > 100)
            {
                pagamento.estado = "APROVADO";
            }

            return pagamento;

        }
    }
}
