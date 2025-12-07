using GroceryItemLookup.Models;
using GroceryItemLookup.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GroceryItemLookup.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ProductService _productService;

        public ProductController(ILogger<ProductController> logger, ProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProducts();
            return View(products);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var product = _productService.GetProductBySKU(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var departments = _productService.GetDepartments();
            ViewBag.Departments = new SelectList(departments, "ID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
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

                var departments = _productService.GetDepartments();
                ViewBag.Departments = new SelectList(departments, "ID", "Name", product.DepartmentID);
                return View(product);
            }

            var department = _productService.GetDepartmentById(product.DepartmentID);
            if (department != null)
            {
                product.Department = department;
            }

            await _productService.AddProduct(product);
            TempData["Message"] = "Product created successfully";
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public IActionResult Edit(int id)
        {
            var departments = _productService.GetDepartments();
            ViewBag.Departments = new SelectList(departments, "ID", "Name");

            var product = _productService.GetProductBySKU(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productService.UpdateProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = _productService.GetProductBySKU(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productService.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
