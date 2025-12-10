using GroceryItemLookup.Models;
using GroceryItemLookup.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GroceryItemLookup.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ILogger<DepartmentController> _logger;
        private readonly ProductService _productService;
        private readonly DepartmentService _departmentService;

        public DepartmentController(ILogger<DepartmentController> logger, DepartmentService departmentService, ProductService productService)
        {
            _logger = logger;
            _departmentService = departmentService;
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            var departments = await _departmentService.GetDepartments();
            return View(departments);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var department = _departmentService.GetDepartmentById(id);
            if (department == null)
            {
                return NotFound();
            }
            var products = await _productService.GetProductsByDepartment(id);
            ViewBag.Products = products;

            return View(department);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState invalid:");
                foreach (var kvp in ModelState)
                {
                    foreach (var error in kvp.Value.Errors)
                    {
                        Console.WriteLine($"{kvp.Key}: {error.ErrorMessage}");
                    }
                }
                return View();
            }

            await _departmentService.AddDepartment(department);
            TempData["Message"] = "Department created successfully";
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public IActionResult Edit(int id)
        {

            var department = _departmentService.GetDepartmentById(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                await _departmentService.UpdateDepartment(department);
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var department = _departmentService.GetDepartmentById(id);
            if (department == null)
            {
                return NotFound();
            }
            var products = await _departmentService.GetProductsInDepartment(id);
            ViewBag.Products = products;
            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _departmentService.DeleteDepartment(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
