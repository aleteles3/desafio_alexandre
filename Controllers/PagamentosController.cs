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
    [Route("api/pagamentos/compras")]
    [ApiController]
    public class PagamentosController : ControllerBase
    {
        [HttpPost]
        public ActionResult<Pagamento> PostPagamento(Compra compra)
        {
            if (compra.produto.valor_unitario * compra.qtde_comprada > 100)
            {
                return Ok();
            }

            return BadRequest();

        }
    }
}
