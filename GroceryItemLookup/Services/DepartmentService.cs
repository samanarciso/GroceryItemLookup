using GroceryItemLookup.Data;
using GroceryItemLookup.Models;
using Microsoft.EntityFrameworkCore;

namespace GroceryItemLookup.Services
{
    public class DepartmentService
    {
        private readonly ILogger<DepartmentService> _logger;
        private readonly GroceryItemLookup.Data.GroceryItemLookupContext _context;

        public DepartmentService(ILogger<DepartmentService> logger, GroceryItemLookupContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsInDepartment(int departmentId)
        {
            var department = await _context.Department
                .Include(d => d.Products)
                .FirstOrDefaultAsync(d => d.ID == departmentId);

            return department?.Products ?? Enumerable.Empty<Product>();
        }

        public async Task<IEnumerable<Department>> GetDepartments()
        {
            return await _context.Department.ToListAsync();
        }
        public Department? GetDepartmentById(int id)
        {
            return _context.Department.FirstOrDefault(d => d.ID == id);
        }

        public async Task AddDepartment(Department department)
        {
            _context.Department.Add(department);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDepartment(Department department)
        {
            _context.Department.Update(department);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDepartment(int id)
        {
            var department = await _context.Department.FindAsync(id);
            if (department != null)
            {
                _context.Department.Remove(department);
                await _context.SaveChangesAsync();
            }
        }
    }
}
