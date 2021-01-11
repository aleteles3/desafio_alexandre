using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prova_alexandre.Data;
using prova_alexandre.Models;
using System.Net.Http;

namespace prova_alexandre.Controllers
{
    [Route("api/compras")]
    [ApiController]
    public class ComprasController : ControllerBase
    {
        private readonly Context _context;

        public ComprasController(Context context)
        {
            _context = context;
        }

        /*
        // GET: api/Compras
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Compra>>> GetCompras()
        {
            return await _context.Compras.ToListAsync();
        }

        // GET: api/Compras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Compra>> GetCompra(int id)
        {
            var compra = await _context.Compras.FindAsync(id);

            if (compra == null)
            {
                return NotFound();
            }

            return compra;
        }
        */

        /*
        // PUT: api/Compras/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompra(int id, Compra compra)
        {
            if (id != compra.Id)
            {
                return BadRequest();
            }

            _context.Entry(compra).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompraExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        */

        /// <summary>
        /// Realizar uma nova compra. Esta rota invocará a rota de pagamentos.
        /// </summary>
        /// <param name="compra">Modelo da compra.</param>
        /// <response code="200">Produto cadastrado com sucesso.</response>
        /// <response code="400">Ocorreu um erro desconhecido / Pagamento não aprovado.</response>
        /// <response code="412">Valores informados não são válidos.</response>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(412)]
        public async Task<ActionResult<Compra>> PostCompra(Compra compra)
        {
            if (compra == null)
            {
                return BadRequest("Ocorreu um erro desconhecido");
            }

            if (!ModelState.IsValid)
            {
                return StatusCode(412, "Os valores informados não são válidos.");
            }

            // Obter produto da compra
            compra.produto = await _context.Produtos.FindAsync(compra.produto_id);

            // Verificar se tem produto disponível no estoque
            if (compra.produto != null && compra.produto.qtde_estoque >= compra.qtde_comprada)
            {

                // Requisitar POST da API de pagamento para validação                  
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5000/api/pagamento/compras");

                    var pagamento = new Pagamento();
                    pagamento.cartao = compra.cartao;
                    pagamento.valor = compra.produto.valor_unitario * compra.qtde_comprada;
                 
                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<Pagamento>(client.BaseAddress, pagamento);
                    postTask.Wait();

                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        pagamento = await result.Content.ReadAsAsync<Pagamento>();

                        if (pagamento.estado == "APROVADO")
                        {
                            // Registrar data da última venda no produto
                            compra.data_compra = DateTime.Today.ToString();
                            compra.produto.data_ultima_venda = compra.data_compra;

                            // Decrementar qtde em estoque
                            compra.produto.qtde_estoque -= compra.qtde_comprada;

                            _context.Compras.Add(compra);
                            await _context.SaveChangesAsync();

                            return Ok("Compra realizada com sucesso.");
                        }
                        else
                        {
                            return BadRequest("Pagamento não aprovado.");
                        }
                    }
                }
            }
            return BadRequest("Ocorreu um erro desconhecido.");
            
        }

        /*
        // DELETE: api/Compras/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Compra>> DeleteCompra(int id)
        {
            var compra = await _context.Compras.FindAsync(id);
            if (compra == null)
            {
                return NotFound();
            }

            _context.Compras.Remove(compra);
            await _context.SaveChangesAsync();

            return compra;
        }

        private bool CompraExists(int id)
        {
            return _context.Compras.Any(e => e.Id == id);
        }
        */
    }
}
