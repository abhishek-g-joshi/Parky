using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Repository.IRepository;
using AutoMapper;
using ParkyAPI.ParkyMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddScoped<INationalParkRepository, NationalParkRepository>();
builder.Services.AddScoped<ITrailRepository, TrailRepository>();


builder.Services.AddAutoMapper(typeof(ParkyMappings));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("ParkyOpenAPISpec",
        new Microsoft.OpenApi.Models.OpenApiInfo()
        {
            Title = "Parky API",
            Version = "1",
            Description = "Udemy Parky API",
            Contact = new Microsoft.OpenApi.Models.OpenApiContact()
            {
              Email = "abhishek.joshi@mindbodyonline.com",
              Name = "Abhishek Joshi"
            },
            License = new Microsoft.OpenApi.Models.OpenApiLicense()
            {
                Name =  "MIT License"
            }
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("swagger/ParkyOpenAPISpec/swagger.json", "Parky API");
    options.RoutePrefix = "";
});
app.UseRouting();

/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/



app.UseAuthorization();

app.MapControllers();

app.Run();
