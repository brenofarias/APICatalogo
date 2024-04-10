using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;
        public CategoriasController(AppDbContext context, ILogger<CategoriasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            _logger.LogInformation("================ GET /categorias ================");

            try
            {
                // throw new DataMisalignedException();

                var categorias = _context.Categorias.AsNoTracking().ToList();

                if (categorias is null)
                   return NotFound("Não existe categorias cadastradas!");

                return categorias;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação.");
            }

        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult<Categoria>> Get(int id)
        {
           var categoria = await _context.Categorias.FirstOrDefaultAsync(p => p.CategoriaID == id);

            if (categoria == null)
            {
                _logger.LogWarning($"Categoria com id = {id} não encontrada...");
                return NotFound($"Categoria com id = {id} não encontrada...");
            }

            return Ok(categoria);
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            _logger.LogInformation("================ GET /categorias/produtos ================");

            return _context.Categorias.Include(p => p.Produtos).Where(c => c.CategoriaID <= 5).AsNoTracking().ToList();
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            // Verifica se existe dados
            if (categoria is null)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos");
            }

            // Recupera todos as categoria
            var produtos = _context.Produtos.ToList();
            // Verifica se já existe uma categoria com o mesmo nome
            bool existe = produtos.Any(p => p.Nome == categoria.Nome);
            if (existe)
            {
                _logger.LogWarning($"Categoria já existe...");
                return BadRequest("Categoria já existe");
            }

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            // Retorna os dados do produto cadastrado na rota Get(id)
            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoria.CategoriaID }, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaID)
            {
                _logger.LogWarning($"Categoria de id = {id} não existe...");
                return BadRequest($"Categoria de id = {id} não existe");
            }

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges(); 

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaID == id);
            if (categoria is null)
            {
                _logger.LogWarning($"Categoria de id = {id} não existe...");
                return BadRequest($"Categoria de id = {id} não existe");
            }

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }
    }
}
