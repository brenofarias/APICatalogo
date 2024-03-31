using APICatalogo.Context;
using APICatalogo.Controllers.Models;
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
        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
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
        public ActionResult<Categoria> Get(int id)
        {
            try
            {
                var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(c => c.CategoriaID == id);

                if (categoria is null)
                    return NotFound($"Categoria com ID {id} não encontrada!");

                return Ok(categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação.");
            }


        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            // return _context.Categorias.Include(p => p.Produtos).ToList();

            return _context.Categorias.Include(p => p.Produtos).Where(c => c.CategoriaID <= 5).AsNoTracking().ToList();
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            // Verifica se existe dados
            if (categoria is null)
                return BadRequest();

            // Recupera todos as categoria
            var produtos = _context.Produtos.ToList();
            // Verifica se já existe uma categoria com o mesmo nome
            bool existe = produtos.Any(p => p.Nome == categoria.Nome);
            if (existe)
                return BadRequest("Categoria já existe!");

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
                return BadRequest();

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges(); 

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaID == id);
            if (categoria is null)
                return BadRequest($"Categoria com ID {id} não encontrada!");

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }
    }
}
