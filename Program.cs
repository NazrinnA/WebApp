using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;
using WebApplication2.DataAccess;
using WebApplication2.Repository.Implementations;
using WebApplication2.Repository.Interfaces;

namespace WebApplication2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
            builder.Services.AddDbContext<AppDb>(op =>
            {
                op.UseSqlServer(builder.Configuration.GetConnectionString("default"));
            });
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddScoped<IAuthorBookRepository, AuthorBookRepository>();
            builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}