using AuthServer;
using AuthServer.Data;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("default")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowClients", policy =>
    {
        policy.AllowAnyOrigin()
              .WithOrigins("http://localhost:7500")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddIdentityServer(opt =>
{
    opt.Events.RaiseErrorEvents = true;
    opt.Events.RaiseInformationEvents = true;
    opt.Events.RaiseFailureEvents = true;
    opt.EmitStaticAudienceClaim = true;
}).AddInMemoryIdentityResources([new IdentityResources.OpenId(), new IdentityResources.Profile()])
.AddInMemoryApiScopes(Config.GetApiScopes(builder.Configuration))
.AddInMemoryClients(Config.GetClients(builder.Configuration))
.AddAspNetIdentity<IdentityUser>()
.AddDeveloperSigningCredential();

var app = builder.Build();

app.UseCors("AllowClients");

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers();

app.Run();
