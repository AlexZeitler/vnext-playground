using System.IdentityModel.Tokens;
using idsrv3.Config;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.DependencyInjection;
using Thinktecture.IdentityServer.Core.Configuration;

namespace idsrv3
{
    public class Startup
    {
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // for web api
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment environment)
        {
            app.Map("/core", core =>
            {
                var factory = InMemoryFactory.Create(Users.Get(), Clients.Get(), Scopes.Get());

                var idsrvOptions = new IdentityServerOptions
                {
                    IssuerUri = "https://idsrv3.com",
                    SiteName = "test vnext Identity server",
                    Factory = factory,
                    SigningCertificate = Certificate.Get(environment),
                    RequireSsl = false,
                    CorsPolicy = CorsPolicy.AllowAll,
                    AuthenticationOptions = new AuthenticationOptions()
                };

                core.UseIdentityServer(idsrvOptions);
            });

            app.Map("/api", api =>
            {
                api.UseOAuthBearerAuthentication(options =>
                {
                    options.Authority = Constants.AuthorizationUrl;
                    // didn't try yet if options.MetadataAddress is necessary...
                    options.MetadataAddress = Constants.AuthorizationUrl + "/.well-known/openid-configuration";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new X509SecurityKey(Certificate.Get(environment)),
                        ValidAudience = "https://idsrv3.com/resources"
                    };
                    ;

                    options.AutomaticAuthentication = true;

                    options.SecurityTokenValidators = new[]
                    {
                        new JwtSecurityTokenHandler()
                    };
                });


                // for web api
                api.UseMvc();
            });
        }
    }
}