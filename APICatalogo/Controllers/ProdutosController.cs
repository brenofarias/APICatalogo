using APICatalogo.Context;
using APICatalogo.Controllers.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get() 
        { 
            var produtos = _context.Produtos.ToList();

            if(produtos is null)
            {
                return NotFound("Produtos não encontrados!");
            }

            return produtos;
        }

        [HttpGet("{id:int}", Name ="ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoID == id);

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
    }
}
