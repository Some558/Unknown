using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Unknown.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<UnknownContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UnknownContext") ?? throw new InvalidOperationException("Connection string 'UnknownContext' not found.")));

// �F�؊֌W�ǉ�
builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<UnknownContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

async Task CreateRolesAndUsers(IServiceProvider serviceProvider)
{
    // ロール管理サービス
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    // ユーザー管理サービス
    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

    // ロール作成
    string[] roles = { "Admin", "Customer" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // 管理者作成
    var adminEmail = "admin@example.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true // メール確認済みに
        };

        var createResult = await userManager.CreateAsync(adminUser, "Admin123!"); // 強力なパスワード

        if (createResult.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

// アプリケーション実行前に実行
using (var scope = app.Services.CreateScope())
{
    await CreateRolesAndUsers(scope.ServiceProvider);
}