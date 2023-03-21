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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
