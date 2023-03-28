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

        public IActionResult Details(Guid id)
        {
            var product = productServices.GetProductById(id);
            return View(product); 
        }
        public IActionResult Delete(Guid id)
        {
            if (productServices.DeleteProduct(id))
            {
                return RedirectToAction("ShowAllProduct");
            }
            else return BadRequest();
        }
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var product = productServices.GetProductById(id);
            return View(product);
        }
        public IActionResult Edit(Product p)
        {
            if (productServices.UpdateProduct(p))
            {
                return RedirectToAction("ShowAllProduct");
            }
            else return BadRequest();
        }

        public IActionResult ViewBag_ViewData()
        {
            var products = productServices.GetAllProducts();
            // ViewBag chứa dữ liệu dạng dynamic, khi cần sử dụng
            // ta không cần khởi tạo mà gán thẳng giá trị vào
            ViewBag.Products = products;
            ViewBag.Messages = "Bảo mất giá trông chán đời <3";
            // ViewData chứa dữ liệu dạng Generic, dữ liệu này được
            // lưu dưới dạng key-value
            ViewData["Products"] = products;
            ViewData["Values"] = "Giá trị của Bảo là ông chị";
            // trong đó "Product" là key còn products là value
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}