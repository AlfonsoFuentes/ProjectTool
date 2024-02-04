using Domain.Entities.Account;
using Microsoft.AspNetCore.Identity;
using Server.Services;
using System.Security.Claims;
var builder = WebApplication.CreateBuilder(args);



builder.AddServerServices();



builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging();
}
app.MapIdentityApi<AplicationUser>();
app.MapPost("/Logout", async (ClaimsPrincipal user, SignInManager<AplicationUser> signInManager) =>
{
    await signInManager.SignOutAsync();
    return TypedResults.Ok();
});
app.UseHttpsRedirection();
// activate the CORS policy
app.UseCors("wasm");

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
