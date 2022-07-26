using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToDoListInfrastructure.Database;
using ToDoListInfrastructure.Mappers;
using ToDoListInfrastructure.Models;
using ToDoListInfrastructure.Models.Repositories;
using ToDoListInfrastructure.Models.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ToDoListAppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnectionString"));
});

builder.Services.AddDbContext<IdentityToDoListDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("identityConnectionString"));
});

builder.Services.AddSingleton(AutoMapperConfig.Initialize());

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityToDoListDbContext>();

builder.Services.AddScoped<IToDoListRepository, ToDoListRepository>();
builder.Services.AddScoped<IToDoListService, ToDoListService>();

builder.Services.AddScoped<IToDoEntryRepository, ToDoEntryRepository>();
builder.Services.AddScoped<IToDoEntryService, ToDoEntryService>();

builder.Services.AddScoped<INotesRepository, NotesRepository>();
builder.Services.AddScoped<INotesService, NotesService>();

builder.Services.AddTransient<ProgressStatusFormatingService>();
builder.Services.AddTransient<DateFormattingService>();
var app = builder.Build();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "pagination",
    pattern: "{controller=ToDoList}/{action=Index}/Page{listPage}");

app.MapControllerRoute(
    name: "pagination 1",
    pattern: "{controller=ToDoList}/{action=HiddenList}/Page{listPage}");

app.MapControllerRoute(
    name: "Pagination 2",
    pattern: "{controller=ToDoEntry}/{action=Index}/Page{listPage}");

app.MapControllerRoute(
    name: "pagination 3",
    pattern: "{controller=ToDoEntry}/{action=ReadItemsToday}/Page{listPage}");

app.MapControllerRoute(
    name: "pagination 4",
    pattern: "{controller=ToDoEntry}/{action=Index}/Page{listPage}/{hideCompleted}");

app.Run();
