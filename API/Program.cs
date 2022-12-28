using Persistence;
using Application.Activities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Application.Core;

var builder = WebApplication.CreateBuilder(args);

// add services to the container

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
            });
            builder.Services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddCors(opt => 
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:3000");
                });
            });

            builder.Services.AddMediatR(typeof(List.Handler).Assembly);
            builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);


// configure the http request pipeline

var app = builder.Build();


if (builder.Environment.IsDevelopment())
            {
              
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });



            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<DataContext>();
                await context.Database.MigrateAsync();

                await Seed.SeedData(context);
            }
            catch (Exception ex)
            {
                var logger= services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occured during migration");
            }


            app.Run();

