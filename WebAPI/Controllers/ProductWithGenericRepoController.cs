using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Entity;
using WebAPI.Repositories;
using WebAPI.VeiwModel;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductWithGenericRepoController : ControllerBase
    {
        private readonly IRepository<Product> _productrepository;

        public ProductWithGenericRepoController(IRepository<Product> productrepository)
        {
            _productrepository = productrepository;
        }

        [HttpGet]

        public async Task<IActionResult> Get()
        {
            var products = await _productrepository.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productrepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductRequest product)
        {
            var productEntity = new Product()
            {
                ProductName = product.ProductName,
                Price = product.Price,
            };
            var createdProductReponse = await _productrepository.AddAsync(productEntity);
            return CreatedAtAction(nameof(GetById), new { id = createdProductReponse.ProductId }, createdProductReponse);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> Put(int id, [FromBody] ProductRequest product)
        {
            var productEntity = await _productrepository.GetByIdAsync(id);
            if (productEntity == null)
            {
                return NotFound();
            }
            productEntity.ProductName = product.ProductName;
            productEntity.Price = product.Price;
            await _productrepository.UpdateAsync(productEntity);
            return NoContent();
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productrepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            await _productrepository.DeleteAsync(product);
            return NoContent();
        }
    }
}
