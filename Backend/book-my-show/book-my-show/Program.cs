
using book_my_show.Data;
using book_my_show.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace book_my_show
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));  //db context
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<EncryptionService>();
            builder.Services.AddScoped<IMoviesRepository, MoviesRepository>();
            //builder.Services.AddSingleton<EncryptionService>(provider => new EncryptionService("vVSN0DxoyjeoSomev1ym1EFmodCtUEooKYH4DB65S00="));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder.WithOrigins("*")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
