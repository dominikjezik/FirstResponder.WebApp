using System.Text;
using FirstResponder.ApplicationCore.Aeds.Queries;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.Infrastructure.DbContext;
using FirstResponder.Infrastructure.FileStorage;
using FirstResponder.Infrastructure.Identity;
using FirstResponder.Infrastructure.JWT;
using FirstResponder.Infrastructure.Mail;
using FirstResponder.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

// JWT
builder.Services.AddAuthentication()
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
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
    options.AddPolicy("IsEmployee", policy => policy
        .RequireClaim("UserType", UserType.Employee.ToString()));
    
    options.AddPolicy("IsResponderOrEmployee", policy => policy
        .RequireClaim("UserType", UserType.Responder.ToString(), UserType.Employee.ToString()));
    
    options.AddPolicy("Bearer", policy => policy
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
    
    options.AddPolicy("CookieOrBearer", policy => policy
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(IdentityConstants.ApplicationScheme, JwtBearerDefaults.AuthenticationScheme));
});

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetAedItemsQuery>());

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
builder.Services.AddScoped<IGroupsRepository, GroupsRepository>();
builder.Services.AddScoped<IIncidentsRepository, IncidentsRepository>();

builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddSingleton<IFileService, LocalFileService>();
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddSingleton<IMailService, MailKitService>();

// Seedovanie databázy
// builder.Services.AddTransient<DatabaseSeeder>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    
    // Seedovanie databázy
    // using var serviceScope = app.Services.CreateScope();
    // var seeder = serviceScope.ServiceProvider.GetService<DatabaseSeeder>();
    // seeder.Seed();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();