using Microsoft.EntityFrameworkCore;
using webquanlydouong.Data;
using webquanlydouong.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Thời gian hết hạn session
});
// Đăng ký ApplicationDbContext với SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Bỏ Identity hoàn toàn (không sử dụng UserManager và SignInManager nữa)
// builder.Services.AddIdentity<User, IdentityRole>() sẽ bị xóa

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

// Bỏ phần Authorization liên quan đến Identity
// app.UseAuthorization(); // Không cần nếu không dùng Identity

// Đảm bảo người dùng chưa đăng nhập sẽ chuyển hướng đến trang đăng nhập.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");  // Đảm bảo vào Login mặc định

app.Run();
