using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Aeds.Queries;
using FirstResponder.ApplicationCore.Enums;
using FirstResponder.Infrastructure.DbContext;
using FirstResponder.Infrastructure.FileStorage;
using FirstResponder.Infrastructure.Identity;
using FirstResponder.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomUserClaimsPrincipalFactory>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
});

// Configure routing
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true); 
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsEmployee", policy => policy.RequireClaim("UserType", UserType.Employee.ToString()));
});

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetAllAedsQuery>());

// LocalFileService configuration
builder.Services.Configure<LocalFileServiceOptions>(options =>
{
    options.UploadsFolderPath = Path.Combine(builder.Environment.WebRootPath, "uploads");
});

// Custom repositories and services
builder.Services.AddScoped<IAedRepository, AedRepository>();
builder.Services.AddScoped<IAedManufacturersRepository, AedManufacturersRepository>();
builder.Services.AddScoped<IAedModelsRepository, AedModelsRepository>();
builder.Services.AddScoped<IAedLanguagesRepository, AedLanguagesRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddSingleton<IFileService, LocalFileService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseStatusCodePagesWithReExecute("/error/{0}");

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();