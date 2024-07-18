using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Data.Repositories;
using SchoolAPI.Models;
using FluentValidation.AspNetCore;
using FluentValidation;
using SchoolAPI.Validators;

internal class Program
{
    [Obsolete]
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers()
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<StudentValidator>());

        // Add DbContext
        builder.Services.AddDbContext<SchoolContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Add UnitOfWork
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register Validators
        builder.Services.AddTransient<IValidator<Student>, StudentValidator>();
        builder.Services.AddTransient<IValidator<Course>, CourseValidator>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
