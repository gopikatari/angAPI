using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
//  public Startup(IHostingEnvironment env)
//         {
//             var builder = new ConfigurationBuilder()
//                 .SetBasePath(env.ContentRootPath)
//                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//                 .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
//                 .AddEnvironmentVariables();
//             Configuration = builder.Build();
//         }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var key=Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value);
            services.AddDbContext<DataContext>(options=>options.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddTransient<Seed>();
            services.AddAutoMapper();
            services.AddCors();
            services.AddScoped<IAuthRepository,AuthRepository>();
            services.AddScoped<IDatingRepository,DatingRepository>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options=>
                {
                    options.TokenValidationParameters=new TokenValidationParameters{
                        ValidateIssuerSigningKey=true,
                        IssuerSigningKey=new SymmetricSecurityKey(key),
                        ValidateIssuer=false,
                        ValidateAudience=false

                    };
                });
        services.AddMvc().AddJsonOptions(opt=>{
            opt.SerializerSettings.ReferenceLoopHandling=Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Seed seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else{
                app.UseExceptionHandler(builder => 
                {
                    builder.Run(async  context =>
                    {
                       context.Response.StatusCode=(int)HttpStatusCode.InternalServerError;
                        var error=context.Features.Get<IExceptionHandlerFeature>();
                        if(error!=null){
                            context.Response.AddApplicationError(error.Error.Message);
                            await  context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }
            //seeder.SeedUser();
            app.UseCors(a=>a.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
