using EmployeesWhoWorkedTogether.Filters;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Models;
using UtilsLib;
using UtilsLibAbstract;

namespace EmployeesWhoWorkedTogether
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder
                        .WithOrigins("http://localhost:59579","http://localhost:5080", "https://localhost:7229")  
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API V1", Version = "v1" });
                c.OperationFilter<FileUploadOperation>();
            });

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 104857600;
            });

            builder.Services.AddTransient<IFileReader, FileReader>();
            builder.Services.AddTransient<IEmployeeWorkTimeOverlapService, EmployeeWorkTimeOverlapService>();

            var app = builder.Build();

            app.UseCors("AllowSpecificOrigin");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
