using GroceryItemLookup.Data;
using GroceryItemLookup.Models;
using Microsoft.EntityFrameworkCore;

namespace GroceryItemLookup.Services
{
    public class ProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly GroceryItemLookup.Data.GroceryItemLookupContext _context;

        public ProductService(ILogger<ProductService> logger, GroceryItemLookupContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Product.Include(p => p.Department).ToListAsync();
        }

        public Product? GetProductBySKU(int sku)
        {
            return _context.Product
                .Include(p => p.Department)
                .FirstOrDefault(p => p.SKU == sku);
        }

        public IEnumerable<Department> GetDepartments()
        {
            return _context.Department.ToList();
        }
        public Department? GetDepartmentById(int id)
        {
            return _context.Department.FirstOrDefault(d => d.ID == id);
        }

        public async Task AddProduct(Product product)
        {
            _context.Product.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProduct(Product product)
        {
            _context.Product.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(int sku)
        {
            var product = await _context.Product.FindAsync(sku);
            if (product != null)
            {
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
