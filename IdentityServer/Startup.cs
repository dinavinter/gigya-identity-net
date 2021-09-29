// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer.Security;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace IdentityServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment environment)
        {
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // uncomment, if you want to add an MVC-based UI
            services.AddControllersWithViews();

            //session storage
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddRazorPages()
                .AddRazorOptions(options =>
                {
                    options.ViewLocationFormats.Add("/{0}.cshtml");
                    options.PageViewLocationFormats.Add("/{0}.cshtml");
                });

            services.AddAuthentication(options =>
                {
                    // options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    // options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer("Gigya-JWT" /*"workflow"*/, options =>
                {
                    options.SetJwksOptions(new JwkOptions()
                    {
                        JwksUri = new Uri("https://accounts.us1.gigya.com/accounts.getJWTPublicKey?&v2"),
                    });

              
                    options.SecurityTokenValidators.Insert(0, new GigyaJwtTokenHandler());
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;

                    options.Authority = "https://accounts.us1.gigya.com/accounts.getJWTPublicKey?&v2";
                  
                    options.TokenValidationParameters.ValidTypes = new[] { "JWT" };
                    options.TokenValidationParameters.ValidateAudience = false;
                    options.TokenValidationParameters.ValidateIssuer = false;
                });

            var builder = services.AddIdentityServer(options =>
                {
                    // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                    options.EmitStaticAudienceClaim = true;
                })
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients);

            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors(configurePolicy => configurePolicy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());


            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseSession(new SessionOptions()
            // {
            //     Cookie = new CookieBuilder()
            //     {
            //         HttpOnly = true,
            //         
            //         
            //     }
            // });
            //
            // uncomment if you want to add MVC
            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();


            // uncomment, if you want to add MVC
            app.UseAuthorization();


            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });

            //session storage
            app.UseSession();
            app.UseAuthentication();
        }
    }
}