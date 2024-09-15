using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAPI.Entity;
using WebAPI.Repositories;
using WebAPI.VeiwModel;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductWithUOWController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductWithUOWController(IUnitOfWork unitOfwork)
        {
            _unitOfWork = unitOfwork;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _unitOfWork.GetRepository<Product>().GetAllAsync();
            return Ok(result);
        }

        [HttpGet("productbyname")]
        public async Task<IActionResult> GetByName(string productName)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByName(productName);
            return Ok(product);

        }

        [HttpPost]
        public async Task<IActionResult> Post(ProductRequest product)
        {
            try
            {
                using var transaction = _unitOfWork.BeginTransactionAsync();
                var productEntity = new Product
                {
                    Price = product.Price,
                    ProductName = product.ProductName
                };
             var productresult = await _unitOfWork.GetRepository<Product>().AddAsync(productEntity);
                await _unitOfWork.SaveChangesAsync();

                var orderEntity = new Order
                {
                    OrderDate = DateTime.UtcNow,  // Use UTC time
                    ProductId = productresult.ProductId
                };


                await _unitOfWork.GetRepository<Order>().AddAsync(orderEntity);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();

                return StatusCode((int)HttpStatusCode.Created, new { Id = productresult.ProductId });

            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
