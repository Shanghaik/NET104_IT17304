﻿using Microsoft.AspNetCore.Mvc;
using SellerProduct.IServices;
using SellerProduct.Models;
using SellerProduct.Services;
using System.Diagnostics;
using System.Text;

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
            // Đọc dữ liệu ra từ Session
            var sessionData = HttpContext.Session.GetString("mitom2trung");
            if (sessionData == null)
                ViewData["data"] = "Session không tồn tại";
            else ViewData["data"] = sessionData;
            var sessionData2 = HttpContext.Session.GetString("Test");
            if (sessionData2 == null)
                ViewData["data2"] = "Session không tồn tại";
            else ViewData["data2"] = sessionData2;
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
        // Các ảnh không nằm trong thư mục root khi chạy sẽ không hiển thị
        // ra với các phương thức cơ bản => để hienr thị được ta cần phải
        // thực hiện cách sau: Lấy đường dẫn ảnh => copy ảnh đó vào wwwroot
        // sau đó thực hiện hiển thị như bình thường
        public IActionResult Create(Product product, IFormFile imageFile) // Thực hiện thêm
        {
            // Trong trường hợp chúng ta thực hiện với thuộc tính Description
            // Thuộc tính này đang là string => Không thể thao tác trực tiếp
            // với các file => Truyền thêm 1 tham số vào Action này
            // Truyền thêm 1 tham số imageFile kiểu IFormFile
            // Bước 1: Kiểm tra đường dãn tới ảnh được lấy từ form
            if (imageFile != null && imageFile.Length > 0) // Không null không rỗng
            {
                // Thực hiện trỏ tới thư mục root để lát thực hiện việc copy
                var path = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot", "images", imageFile.FileName); // Bước 2
                // Kết quả: aaa/wwwroot/images/xxx.jpg, Path.Combine = tổng hợp đường dẫn
                var stream = new FileStream(path, FileMode.Create);
                // Vì chúng ta thực hiện việc copy => Tạo mới => Create
                imageFile.CopyTo(stream); // Copy ảnh chọn ở form vào wwwroot/images
                // Gán lại giá trị cho thuộc tính Description => Bước 3
                product.Description = imageFile.FileName; // Bước 4
            }
            if (productServices.CreateProduct(product))
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

        public IActionResult TestSession()
        {
            string message = "Em đói lắm không nghĩ được đâu";
            // Đưa dữ liệu vào phiên làm việc (Session)
            HttpContext.Session.SetString("mitom2trung", message);
            HttpContext.Session.SetString("Test", "Session tét");
            // Đọc dữ liệu ra từ Session
            var sessionData = HttpContext.Session.GetString("mitom2trung");
            var sessionData2 = HttpContext.Session.GetString("Test");
            ViewData["data"] = sessionData;
            ViewData["data2"] = sessionData2;
            /*
             * Timeout của session được tính như thế nào:
             * Khi Session đã tồn tại, Bộ đếm thời gian sẽ được kích hoạt 
             * ngay sau khi request cuối cùng được thực thi. Nếu sau khoảng
             * thời gian idleTimeout mà không có request nào được thực thi
             * thì dữ liệu đó sẽ mất. Nếu trước khi thời gian timeout kết
             * thúc mà có 1 request bất kì được thực thi thì bộ đếm thời 
             * gian sẽ được reset
             */
            return View();
        }

        public IActionResult AddToCart(Guid id)
        {
            // B1: Dựa vào ID lấy ra sản phẩm
            var product = productServices.GetProductById(id);
            // B2: Lấy danh sách sản phẩm ra từ Session
            var products = SessionServices.GetObjFromSession(HttpContext.Session, "Cart");
            if (products.Count == 0)
            {
                products.Add(product); // Thêm trực tiếp sp vào nếu List trống
                SessionServices.SetObjToSession(HttpContext.Session, "Cart", products);
            }
            else
            {
                if (SessionServices.CheckObjInList(id, products))
                { // Kiểm tra xem list lấy ra có chứa sản phẩm mình chọn hay chưa?
                    return Content("Bình thường chúng ta sẽ thêm số lượng nhưng vì " +
                        "lười nên không thêm mà chỉ đưa ra thông báo vô ích này");
                }
                else
                {
                    products.Add(product); // Thêm trực tiếp sp vào nếu List chưa chứa sp đó
                    SessionServices.SetObjToSession(HttpContext.Session, "Cart", products);
                }
            }
            // B3: Kiểm tra và thêm SP vào giỏ hàng
            return RedirectToAction("ShowAllProduct");
        }
        public IActionResult ShowCart()
        {
            // Lấy dữ liệu từ Session để truyền vào View
            var products = SessionServices.GetObjFromSession(HttpContext.Session, "Cart");
            return View(products);  // Truyền sang view
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}