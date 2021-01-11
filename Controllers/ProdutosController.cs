using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Description;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prova_alexandre.Data;
using prova_alexandre.Models;

namespace prova_alexandre.Controllers
{
    [Route("api/produtos")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly Context _context;

        public ProdutosController(Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os produtos disponíveis para venda.
        /// </summary>
        /// <response code="200">Lista de produtos obtida com sucesso.</response>
        /// <response code="400">Ocorreu um erro desconhecido.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<Produto>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProduto()
        {
            try
            {
                var produtos = await _context.Produtos.ToListAsync();
                return Ok(produtos);
            }
            catch (Exception)
            {
                return BadRequest("Ocorreu um erro desconhecido.");
            }
        }

        /// <summary>
        /// Retorna um produto específico por ID.
        /// </summary>
        /// <param name="id">ID do produto.</param>
        /// <response code="200">Produto obtido com sucesso.</response>
        /// <response code="400">Ocorreu um erro desconhecido.</response>
        /// <response code="404">Produto não encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Produto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            try
            {
                var produto = await _context.Produtos.FindAsync(id);

                if (produto == null)
                {
                    return NotFound("Produto não encontrado.");
                }

                return Ok(produto);
            }
            catch(Exception)
            {
                return BadRequest("Ocorreu um erro desconhecido.");
            }

            
        }

        /*
        // PUT: api/Produtos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, Produto produto)
        {
            if (id != produto.Id)
            {
                return BadRequest();
            }

            _context.Entry(produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
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
        /// Cadastra um novo produto.
        /// </summary>
        /// <param name="produto">Modelo do produto.</param>
        /// <response code="200">Produto cadastrado com sucesso.</response>
        /// <response code="400">Ocorreu um erro desconhecido.</response>
        /// <response code="412">Valores informados não são válidos.</response>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(412)]
        public async Task<ActionResult<Produto>> PostProduto(Produto produto)
        {
            if (produto == null)
            {
                return BadRequest("Ocorreu um erro desconhecido");
            }

            if (!ModelState.IsValid) {
                return StatusCode(412, "Os valores informados não são válidos.");
            }

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return Ok("Produto Cadastrado");
        }

        /// <summary>
        /// Remove o cadastro de um produto.
        /// </summary>
        /// <param name="id">ID do produto a ser removido.</param>
        /// <response code="200">Produto removido com sucesso.</response>
        /// <response code="400">Ocorreu um erro desconhecido.</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Produto>> DeleteProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return BadRequest("Ocorreu um erro desconhecido.");
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return Ok("Produto excluído com sucesso.");
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }
    }
}
