using Microsoft.AspNetCore.Mvc;
using SellerProduct.IServices;
using SellerProduct.Models;
using SellerProduct.Services;
using System.Diagnostics;

namespace SellerProduct.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductServices productServices;// Interface
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            productServices = new ProductServices(); // DI
        }

        public IActionResult Index()
        {
            return View(); // Trả về - hiển thị view Cùng tên với Action
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Test()
        {
            return View();
        }

        public IActionResult Redirect()
        {
            return RedirectToAction("Test");   // Chuyển hướng về action Test
        }
        // truyền dữ liệu từ Action qua View
        public ActionResult Show()
        {
            Product product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "+ Ngọc Bảo",
                Description = "Học lại",
                Supplier = "Mama Bank",
                Price = 672000,
                AvailableQuantity = 1,
                Status = 1
            };
            return View(product); // Truyền trực tiếp 1 Obj Model duy nhất sang View

        }
        public ActionResult ShowAllProduct()
        {
            List<Product> products = productServices.GetAllProducts();
            return View(products); // Truyền trực tiếp 1 Obj Model duy nhất sang View
        }

        public IActionResult Create() // Hiển thị form
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product p)
        {
            if (productServices.CreateProduct(p))
            {
                return RedirectToAction("ShowAllProduct");
            }
            else return BadRequest();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}