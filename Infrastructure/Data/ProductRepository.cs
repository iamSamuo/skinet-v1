using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ProductRepository(StoreContext context) : IProductRepository
    {

        public void AddProduct(Product product)

        {
            context.Products.Add(product);
        }

        public void DeleteProduct(Product product)
        {
            context.Products.Remove(product);
        }

        public async Task<IReadOnlyList<string>> GetProductBrands()
        {
            return await context.Products.Select(product => product.Brand).Distinct().ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await context.Products.FindAsync(id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type)
        {
            // add filtering logic 
            var query = context.Products.AsQueryable();


            if (!string.IsNullOrWhiteSpace(brand))
                query = query.Where(product => product.Brand == brand);

            if (!string.IsNullOrWhiteSpace(type))
                query = query.Where(product => product.Type == type);


            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<string>> GetProductTypes()
        {
            return await context.Products.Select(product => product.Type).Distinct().ToListAsync();
        }

        public bool ProductExists(int id)
        {
            return context.Products.Any(p => p.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return  await context.SaveChangesAsync() > 0;
        }

        public void UpdateProduct(Product product)
        {
            //telling EF Core that the entity has been modified
            context.Entry(product).State = EntityState.Modified;
        }

       

    }
}
