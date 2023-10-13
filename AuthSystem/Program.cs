using System.Reflection;
using AuthSystem.Api.Extensions;
using AuthSystem.Api.Extensions.Middlewares;
using Microsoft.OpenApi.Models;
using AuthSystem.Infrastructure.Mapper;
using AuthSystem.Infrastructure.Helpers;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using AuthSystem.Core.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public abstract class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);
       

        // Add services to the container.

        builder.Services.AddControllers();

        builder.Services.AddCors();
        builder.Services.AddControllers().AddJsonOptions(x =>
        {
            // serialize enums as strings in api responses (e.g. Role)
            x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });


        builder.Services.AddDbContext<DataContext>(opts =>
                opts.UseSqlServer(builder.Configuration.GetConnectionString("WebApiDatabase")));





        //builder.Services.AddDbContext<DataContext>(o =>
        //{
        //    o.UseSqlServer(options => options.MigrationsAssembly(typeof(DataContext).Assembly.FullName));
        //});
        

        builder.Services.AddDBConnection(builder.Configuration);


        // Configure JSON serialization options
        builder.Services.Configure<JsonOptions>(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        builder.Services.ConfigureLoggerService();

        // ConfigureServices method
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("your-secret-key")),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthSystem", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description =
                   "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\""
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                },
            });
        });

        // configure strongly typed settings object
        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));


        builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
        builder.Services.AddAutoMapper(Assembly.Load("AuthSystem.Infrastructure"));

        


        var app = builder.Build();



        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();


        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthSystem v1");
            //c.RoutePrefix = "swagger"; // Optionally, change the URL route for Swagger UI
        });



        app.UseAuthorization();
        app.UseAuthentication();

        


        //DatabaseHelper.EnsureLatestDatabase(builder.Services);


        // global cors policy
        app.UseCors(x => x
            .SetIsOriginAllowed(origin => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());

        // global error handler
        app.UseMiddleware<ErrorHandlerMiddleware>();

        // custom jwt auth middleware
        app.UseMiddleware<JwtMiddleware>();

        app.MapControllers();

      
        app.Run();

    }
}

