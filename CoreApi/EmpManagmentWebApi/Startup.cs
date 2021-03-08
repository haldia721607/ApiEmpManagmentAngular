using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EmpManagmentWebApi.IServices;
using EmpManagmentWebApi.Security;
using EmpManagmentWebApi.Services;
using EmpManagmentWebApiBLL.AccountBs;
using EmpManagmentWebApiBLL.ComplaientBs;
using EmpManagmentWebApiBLL.Employee;
using EmpManagmentWebApiBOL.Tables;
using EmpManagmentWebApiDAL.DbContextClass;
using EmpManagmentWebApiIBLL.AccountRepository;
using EmpManagmentWebApiIBLL.ComplaientRepository;
using EmpManagmentWebApiIBLL.EmployeeRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
namespace EmpManagmentWebApi
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        public static IConfiguration StaticConfig { get; private set; }
        public string SpecificOrigin = "AllowOrigin";
        [Obsolete]
        public Startup(IConfiguration _configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            this.configuration = _configuration;
            StaticConfig = configuration;
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
            services.AddCors(options =>
            {
                options.AddPolicy(name: SpecificOrigin,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200", "http://localhost:53201")
                                            .AllowAnyMethod()
                                            .AllowAnyHeader()
                                            .AllowCredentials();
                    });
            });

            //services.AddCors();
            //Inject AspNetCoreIdentity dependdencies
            services.AddIdentityCore<IdentityUser>().AddRoles<IdentityRole>()
                                                      .AddEntityFrameworkStores<EmployeeDbContext>()
                                                      .AddDefaultTokenProviders();
            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                    .ConfigureApiBehaviorOptions(options =>
                    {
                        options.SuppressModelStateInvalidFilter = true;
                    });


            //Inject AppDbContext dependencies
            services.AddDbContextPool<EmployeeDbContext>(item => item.UseSqlServer(configuration.GetConnectionString("EmployeeDBConnection"), assembly => assembly.MigrationsAssembly(typeof(EmployeeDbContext).Assembly.FullName)));

            services.AddTransient<IUserServices, UserService>();

            services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddEntityFrameworkStores<EmployeeDbContext>()
               .AddDefaultTokenProviders();

            services.AddMemoryCache();
            //To set Scope of page life cycle 
            services.AddScoped<IAccountRepository, SqlCommanRepositry>();

            services.AddScoped<IComplaientRepositery, SqlComplaientBs>();

            services.AddScoped<EmployeeRepository, EmployeeBll>();
            //services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            //{
            //    options.Password.RequiredLength = 6;
            //    options.Password.RequiredUniqueChars = 3;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.SignIn.RequireConfirmedEmail = true;
            //    options.Lockout.MaxFailedAccessAttempts = 5;
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            //    options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
            //})
            //.AddEntityFrameworkStores<EmployeeDbContext>()
            //.AddDefaultTokenProviders();
            //.AddTokenProvider<CustomEmailConfirmationTokenProvider<ApplicationUser>>("CustomEmailConfirmation");

            //Set token life span to 5 hours this change will apply all token provider method if we want specify to a specific token generate then create to custom token provider (Default 1 day )
            //services.Configure<DataProtectionTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromHours(5));

            // Changes token lifespan of just the Email Confirmation Token type
            //services.Configure<CustomEmailConfirmationTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromDays(3));



            // services.AddDistributedMemoryCache();



            //Adding mvc 
            // services.AddMvc();
            //services.AddMvc(option =>
            //{
            //    option.EnableEndpointRouting = true;
            //    var policy = new AuthorizationPolicyBuilder()
            //                .RequireAuthenticatedUser()
            //                .Build();
            //    option.Filters.Add(new AuthorizeFilter(policy));
            //    option.SuppressAsyncSuffixInActionNames = false;
            //});



            //Adding Application Cookie
            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.Cookie.HttpOnly = true;
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
            //    options.LoginPath = new PathString("/Comman/Account/Login");
            //    options.LogoutPath = new PathString("/Comman/Account/Logout");
            //    options.AccessDeniedPath = new PathString("/User/Administration/AccessDenied");
            //    options.SlidingExpiration = true;

            //});

            //Configure JWT Authentication
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = System.Text.Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            //Adding Authorization and role policy
            services.AddAuthorization();
            //services.AddAuthorization(options =>
            //{
            //    //We can add edt police directly here ... this code is commentd for use antoher way of this method
            //    options.AddPolicy("EditRolePolicy", policy => policy.RequireAssertion(context =>
            //                context.User.IsInRole("Admin") &&
            //                context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") ||
            //                context.User.IsInRole("Super Admin")));

            //    options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role"));
            //    options.AddPolicy("EditRolePolicy", policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));
            //    //If Handlers failuer then do not invoke next Handlers
            //    options.InvokeHandlersAfterFailure = false;
            //    options.AddPolicy("CreateRolePolicy", policy => policy.RequireClaim("Create Role"));
            //    options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin", "Super Admin"));
            //    options.AddPolicy("UserRolePolicy", policy => policy.RequireRole("User", "Manager"));
            //});

           

            //services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();

            ////Register the secod custom authorization handler
            //services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();

            //services.AddSingleton<DataProtectionPurposeStrings>();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public  void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseCors(options => options.SetIsOriginAllowed(x => _ = true).AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            //app.UseCors("AllowOrigin");
            app.UseRouting();
            //app.UseCors(SpecificOrigin);
            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:4200", "http://localhost:53201")
                       .AllowCredentials()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
            app.UseAuthentication();

            app.UseAuthorization();
            //IServiceScopeFactory serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            //using (IServiceScope scope = serviceScopeFactory.CreateScope())
            //{
            //    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            //    //Create Admin Role
            //    if (!(await roleManager.RoleExistsAsync("Admin")))
            //    {
            //        var role = new IdentityRole();
            //        role.Name = "ADMIN";
            //        await roleManager.CreateAsync(role);
            //    }

            //    //Create Admin User
            //    if ((await userManager.FindByNameAsync("admin")) == null)
            //    {
            //        ApplicationUser user = new ApplicationUser();
            //        user.UserName = "admin";
            //        user.Email = "admin@gmail.com";
            //        user.CountryId = 101;
            //        user.StateId = 1644;
            //        user.CityId = 17616;
            //        var userPassword = "Admin@123";
            //        var chkUser = await userManager.CreateAsync(user, userPassword);
            //        if (chkUser.Succeeded)
            //        {
            //            await userManager.AddToRoleAsync(user, "ADMIN");
            //        }
            //    }
            //    ApplicationUser userByName = await userManager.FindByNameAsync("admin");
            //    if (!(await userManager.IsInRoleAsync(userByName, "Admin")))
            //    {
            //        var userResult = await userManager.AddToRoleAsync(userByName, "Admin");
            //    }
            //    //Create Employee Role
            //    if (!(await roleManager.RoleExistsAsync("Employee")))
            //    {
            //        var role = new IdentityRole();
            //        role.Name = "Employee";
            //        await roleManager.CreateAsync(role);
            //    }
            //}
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
