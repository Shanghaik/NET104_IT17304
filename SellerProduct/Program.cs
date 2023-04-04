using SellerProduct.IServices;
using SellerProduct.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IProductServices, ProductServices>();
/*
 * AddSingleton: Tạo ra 1 đối tượng services tồn tại cho đến khi vòng đời
 * của ứng dụng kết thúc. Services này sẽ được dùng chung cho các request
 * Loại đăng kí này phù hợp với các services mang tính toàn cục và không thay đổi
 * AddScoped: Mỗi request cụ thể sẽ tạo ra 1 đối tượng services, đói tượng này
 * được giữ nguyên trong quá trình xử lý request. Phù hợp cho các services
 * mà phục vụ cho một loại request cụ thể.
 * AddTransient: Mỗi request sẽ nhận một services cụ thể khi có yêu cầu. Mỗi
 * services sẽ được tạo mới tại thời điểm có yêu cầu. Phù hợp cho các services
 * có nhiều trạng thái, nhiều nhiều cầu http và mang tính linh động hơn.
 */
Dictionary<string, TimeSpan> sessionTimes = 
    new Dictionary<string, TimeSpan>();

// Set TimeSpan cho từng session
sessionTimes.Add("Cart", new TimeSpan(0, 0, 10)); // 30 phút cho session1
sessionTimes.Add("Test", new TimeSpan(0, 0, 10)); // 1 giờ cho session2
sessionTimes.Add("mitom2trung", new TimeSpan(0, 0, 20)); // 45 phút cho session3

// Đăng ký session vào dịch vụ
builder.Services.AddSession(options =>
{
    // Thiết lập thời gian timeout cho session tương ứng với từng key trong Dictionary
    foreach (var key in sessionTimes.Keys)
    {
        TimeSpan duration;
        if (sessionTimes.TryGetValue(key, out duration))
        {
            options.IdleTimeout = duration;
        }
    }
});
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromSeconds(10);
//}); // Thêm cái này để sử dụng được Session với timeOut = 10 giây
 // Thêm cái này để sử dụng được Session với timeOut = 10 giây
// Tất cả dịch vụ đăng kí phải trước cái dòng ở dưới OK?
var app = builder.Build();// Thực hiện tất cả các services được cài đặt

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession(); // Add thêm cái này nữa để SD được Session
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
