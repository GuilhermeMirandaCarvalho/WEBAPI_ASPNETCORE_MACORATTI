using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _context;

        public CategoriasController(IUnitOfWork context)
        {
            _context = context;
        }


        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            //return _context.Categorias.Include(p=> p.Produtos).AsNoTracking().ToList();
            return _context.CategoriaRepository.GetCategoriasProdutos().ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            //throw new DataMisalignedException();
            return _context.CategoriaRepository.Get().ToList();
        }

        [HttpGet("{id}", Name = "Obter Categoria")]
        public ActionResult<Categoria> Get(int id)
        {
            try
            {
                var categoria = _context.CategoriaRepository.GetById(p => p.CategoriaId == id);
                if (categoria is null)
                {
                    return NotFound("Categoria não encontrado");

                }
                return categoria;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar sua solicitação");
            }
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {       
            _context.CategoriaRepository.Add(categoria);
            _context.Commit();

            return new CreatedAtRouteResult("Obter Categoria", new { id = categoria.CategoriaId }, categoria);

        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest();
            }

            _context.CategoriaRepository.Update(categoria);
            _context.Commit();

            return Ok();
        }



        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);            

            if (categoria is null)
            {
                return NotFound("Categoria não localizado");
            }

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }
    }
}
