using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4.Models;

namespace IdentityServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var cn = "Server=localhost;Port=3306;Database=ghettobasa;Uid=root;Pwd=qris;";

            var builder = services.AddIdentityServer()
                .AddInMemoryApiResources(
                    new ApiResource[]
                    {
                        new ApiResource( "ghettoBasa-api" )
                        {
                            ApiSecrets = {
                                new Secret( "ghettoBAsa@2020".Sha256() )
                            }
                        }
                    }
                )
                .AddInMemoryIdentityResources(
                    new IdentityResource[]
                    {
                        new IdentityResources.OpenId()
                    }
                )
                .AddInMemoryClients(
                    new Client[]
                    {
                        new Client
                        {
                            ClientId = "ghettoBasa-frontend",
                            ClientSecrets = {
                                new Secret( "ghettoBAsaFront@2020".Sha256() )
                            },

                            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                            AllowedScopes = {
                                "ghettoBasa-api"
                            },
                        }
                    }
                );

            builder.AddDeveloperSigningCredential();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseIdentityServer();

            

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Identity Server!");
            });
        }
    }
}
