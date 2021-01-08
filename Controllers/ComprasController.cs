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
    [Route("api/[controller]")]
    [ApiController]
    public class ComprasController : ControllerBase
    {
        private readonly Context _context;

        public ComprasController(Context context)
        {
            _context = context;
        }

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

        // POST: api/Compras
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Compra>> PostCompra(Compra compra)
        {
            // Obter produto da compra
            compra.produto = await _context.Produtos.FindAsync(compra.produto_id);

            // Verificar se tem produto disponível no estoque
            if (compra.produto != null && compra.produto.qtde_estoque >= compra.qtde_comprada)
            {
                // Requisitar POST da API de pagamento para validação                  
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5000/api/pagamentos/compras");

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<Compra>(client.BaseAddress, compra);
                    postTask.Wait();

                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        // Registrar data da última venda no produto
                        compra.data_compra = DateTime.Today.ToString();
                        compra.produto.data_ultima_venda = compra.data_compra;

                        // Decrementar qtde em estoque
                        compra.produto.qtde_estoque -= compra.qtde_comprada;

                        _context.Compras.Add(compra);
                        await _context.SaveChangesAsync();

                        return CreatedAtAction(nameof(GetCompra), new { id = compra.Id }, compra);
                    }
                }
            }
            return BadRequest();
            
        }

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
    }
}
