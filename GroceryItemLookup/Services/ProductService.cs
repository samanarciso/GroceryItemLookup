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

        public IEnumerable<Product> GetProducts()
        {
            return _context.Product.Include(p => p.Department).ToList();
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

        public void AddProduct(Product product)
        {
            _context.Product.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _context.Product.Update(product);
            _context.SaveChanges();
        }

        public void DeleteProduct(int sku)
        {
            var product = _context.Product.Find(sku);
            if (product != null)
            {
                _context.Product.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}
