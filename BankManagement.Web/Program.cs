// ?? Configure MVC + HttpClient for API calls
// Dependencies: BankManagement.Application (DTOs), BankManagement.Web (services)
using BankManagement.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// ?? Add MVC Services
builder.Services.AddControllersWithViews();

// ?? Register ApiService with HttpClient
builder.Services.AddHttpClient<IApiService, ApiService>();

// ?? Optional: Configure JsonSerializer options
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

var app = builder.Build();

// ?? Configure HTTP Request Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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