using backend.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "ALL", policy =>
    {

        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();

    });


});

builder.Services.AddControllers();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = null;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DarkforgeDBContext>(options =>
{

    options.UseSqlServer(builder.Configuration.GetConnectionString("Default Connection"));

});


builder.Services.AddHostedService<CleanupService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseRouting();

app.UseHttpsRedirection();

app.UseCors("ALL");


app.MapControllers();

app.Run();