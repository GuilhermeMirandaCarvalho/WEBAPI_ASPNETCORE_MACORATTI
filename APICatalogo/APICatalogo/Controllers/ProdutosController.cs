using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;

        public ProdutosController(IUnitOfWork context)
        {
            _uof = context;
        }


        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPorPreco()
        {
            return _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
        }


        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _uof.ProdutoRepository.Get().ToList();
            if (!produtos.Any())
            {
                return NotFound("Produtos não encontrados");
            }
            return produtos;
        }

        [HttpGet("{id:int}", Name="Obter Produto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
            if (produto is null)
            {
                return NotFound("Produto não encontrado");

            }
            return produto;
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {          
            _uof.ProdutoRepository.Add(produto);
            _uof.Commit();

            return new CreatedAtRouteResult("Obter Produto", new { id = produto.ProdutoId }, produto);

        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto) 
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest();
            }

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();

            return Ok(produto);
        
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p=> p.ProdutoId == id);
            //var produto = _uof.Produtos.Find(id);

            if (produto is null)
            {
                return NotFound("Produto não localizado");
            }

            _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();

            return Ok(produto);
        }


    }
    

  


}
