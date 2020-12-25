using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StyledByAdeola.Infrastructure;
using StyledByAdeola.Models;
using StyledByAdeola.ServiceContracts;
using StyledByAdeola.Services;
using System;
using System.Linq;
using System.Security.Claims;

namespace StyledByAdeola
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAntiforgery(options => {
                options.HeaderName = "X-XSRF-TOKEN";
            });
            services.AddControllersWithViews();

            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1",
                    new OpenApiInfo { Title = "StyledByAdeola API", Version = "v1" });
            });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            // setup services required for Entity Framework Core,
            // the UseSqlServer method sets up the support required for storing data using Microsoft SQL Server.
            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(Configuration["ConnectionStrings:StyledByAdeolaIdentity"])
            );

            // setup the services for ASP.NET Core Identity
            //AddIdentity method has type parameters that specify the class used to represent users and the class used to represent roles.
            //AddEntityFrameworkStores specifies that identity should use entity framework core to store and retrieve its data
            //AddDefaultTokenProviders use the default configuration to support oprations that require a token
            services.AddIdentity<AppUser, IdentityRole>
                     (
                        opts =>
                        {
                            opts.User.RequireUniqueEmail = true;
                            //opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz";
                            opts.Password.RequiredLength = 6;
                            opts.Password.RequireNonAlphanumeric = false;
                            opts.Password.RequireLowercase = false;
                            opts.Password.RequireUppercase = false;
                            opts.Password.RequireDigit = false;
                        }
                     )
                    .AddEntityFrameworkStores<AppIdentityDbContext>()
                    .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = Configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                    options.CallbackPath = Configuration["Authentication:Google:CallBackPath"];
                })
                .AddMicrosoftAccount(microsoftOptions =>
                {
                    microsoftOptions.ClientId = Configuration["Authentication:Microsoft:ClientId"];
                    microsoftOptions.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
                    microsoftOptions.CallbackPath = Configuration["Authentication:Microsoft:CallBackPath"];
                });

            services.AddApplicationInsightsTelemetry();
            services.AddTransient<IProductRepository<ProductDocDb>, CosmosProductRepository>();
            services.AddTransient<IOrderRepository, CosmosOrderRepository>();
            services.AddTransient<IPasswordValidator<AppUser>, CustomPasswordValidator>();

            //use the HttpContextAccessor class when implementations of the IHttpContextAccessor interface are required. 
            //This service is required so I can access the current session in the SessionCart class
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // initializes the client based on the configuration as a singleton instance to be injected
            services.AddSingleton<ICosmosDb>(InitializeCosmosClientInstanceAsync(Configuration.GetSection("CosmosDb")));
            services.AddSingleton<IBlobStorage>(InitializeAzureStorageBlob(Configuration.GetSection("BlobStorage")));

            // specifies that the same object should be used to satisfy related requests for Cart instances.
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddMemoryCache();
            // registers the services used to access session data
            services.AddSession(options => {
                options.Cookie.Name = "StyledByAdeola.Session";
                options.IdleTimeout = System.TimeSpan.FromHours(48);
                options.Cookie.HttpOnly = false;
                options.Cookie.IsEssential = true;
            });
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("DCUsers", policy =>
                {
                    policy.RequireRole("Users");
                    policy.RequireClaim(ClaimTypes.StateOrProvince, "DC");
                });
            });

            services.AddControllers().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAntiforgery antiforgery)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            // allows the session system to automatically associate requests with sessions when they arrive from client
            app.UseSession();
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json",
                        "StyledByAdeola API");
                });
            }
            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.Use(nextDelegate => context => {
                string path = context.Request.Path.Value;
                string[] directUrls = { "/admin", "/store", "/cart", "/braintree", "checkout" };
                if (path.StartsWith("/api") || string.Equals("/", path)
                        || directUrls.Any(url => path.StartsWith(url)))
                {
                    var tokens = antiforgery.GetAndStoreTokens(context);
                    context.Response.Cookies.Append("XSRF-TOKEN",
                        tokens.RequestToken, new CookieOptions()
                        {
                            HttpOnly = false,
                            Secure = false,
                            IsEssential = true
                        });
                }
                return nextDelegate(context);
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                string strategy = Configuration
                    .GetValue<string>("DevTools:ConnectionStrategy");

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    if (strategy == "proxy")
                    {
                        spa.UseProxyToSpaDevelopmentServer("http://127.0.0.1:4200");
                    }
                    else if (strategy == "managed")
                    {
                        spa.UseAngularCliServer(npmScript: "start");
                    }
                }
            });
        }

        /// Creates a Cosmos DB database and a container with the specified partition key. 
        private static CosmosDBService InitializeCosmosClientInstanceAsync(IConfigurationSection configurationSection)
        {
            string databaseName = configurationSection.GetSection("DatabaseName").Value;
            string containerName = configurationSection.GetSection("ContainerName").Value;
            string account = configurationSection.GetSection("Account").Value;
            string key = configurationSection.GetSection("Key").Value;
            Microsoft.Azure.Cosmos.Fluent.CosmosClientBuilder clientBuilder = new Microsoft.Azure.Cosmos.Fluent.CosmosClientBuilder(account, key);
            Microsoft.Azure.Cosmos.CosmosClient client = clientBuilder
                                .WithConnectionModeDirect()
                                .Build();
            CosmosDBService cosmosDbService = new CosmosDBService(client, databaseName);
            client.CreateDatabaseIfNotExistsAsync(databaseName);
            return cosmosDbService;
        }

        private static BlobStorageService InitializeAzureStorageBlob(IConfigurationSection configurationSection)
        {
            string blobBaseUrl = configurationSection.GetSection("BlobBaseUrl").Value;
            string accountName = configurationSection.GetSection("AccountName").Value;
            string accountKey = configurationSection.GetSection("AccountKey").Value;
            // Create a URI to the blob
            Uri blobUri = new Uri("https://" + accountName + blobBaseUrl);
            BlobStorageService blobStorageService = new BlobStorageService(blobUri, accountName, accountKey);
            return blobStorageService;
        }
    }
}
