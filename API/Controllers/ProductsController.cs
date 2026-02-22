using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ProductsController(IProductRepository productRepository) : ControllerBase
    {
       
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
        {

            return Ok(await productRepository.GetProductsAsync(brand,type,sort));
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetProductBrands()
        {

            return Ok(await productRepository.GetProductBrands());
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetProductTypes()
        {

            return Ok(await productRepository.GetProductTypes());
        }

        [HttpGet("{id:int}")] 
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return product;
        }
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
      
            productRepository.AddProduct(product);
            if(await productRepository.SaveChangesAsync())
            {
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            return BadRequest("Problem creating product");

        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id , Product product)
        {
            if (id != product.Id && !ProductExists(id)) { return BadRequest("Can not update this product"); }
            
            productRepository.UpdateProduct(product);
            if (await productRepository.SaveChangesAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem updating product");


        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await productRepository.GetProductByIdAsync(id);
            if (product == null) return NotFound("Product does not exist!");

            productRepository.DeleteProduct(product);
            if (await productRepository.SaveChangesAsync())
            {
                return NoContent();
            }
            
            return BadRequest("Problem deleting product");
        }
        private bool ProductExists(int id) {
            return productRepository.ProductExists(id);
        }
    }
}
