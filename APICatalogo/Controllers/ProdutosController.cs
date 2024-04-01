using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("primeiro")]
        public async Task<ActionResult<Produto>> GetPrimeiro()
        {
            var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync();

            if (produto is null)
            {
                return NotFound("Produtos não encontrados!");
            }
                
            return produto;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get() 
        { 
            var produtos = await _context.Produtos.AsNoTracking().ToListAsync();

            if(produtos is null)
            {
                return NotFound("Produtos não encontrados!");
            }

            return produtos;
        }

        [HttpGet("{id:int}", Name ="ObterProduto")]
        public async Task<ActionResult<Produto>> Get([FromQuery]int id)
        {
            var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoID == id);

            if(produto is null)
            {
                return NotFound("Produto não encontrado!");
            }

            return produto;
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            // Verifica se existe dados
            if(produto is null)
                return BadRequest();

            // Recupera todos os produtos
            var produtos = _context.Produtos.ToList();
            // Verifica se já existe um produto com o mesmo nome
            bool existe = produtos.Any(p => p.Nome == produto.Nome);
            if (existe)
                return BadRequest("Produto já existe!");

            // Cria na memória
            _context.Produtos.Add(produto);
            // Salva no banco
            _context.SaveChanges();

            // Retorna os dados do produto cadastrado na rota Get(id)
            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoID }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoID)
                return BadRequest();

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoID == id);

            if (produto is null)
                return NotFound("Produto não encontrado!");

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }
    }
}
