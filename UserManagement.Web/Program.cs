using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Data.Extensions;
using UserManagement.Services.Extensions;
using UserManagement.Web.Mappings;
using Westwind.AspNetCore.Markdown;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddDataAccess()
    .AddDomainServices()
    .AddMarkdown()
    .AddControllersWithViews();

builder.Services.AddSingleton(new MapperConfiguration(cfg =>
{
    cfg.AddProfile<MappingProfiles>();
}).CreateMapper());

var app = builder.Build();

app.UseMarkdown();

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
