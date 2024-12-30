
using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;
using SurveyBasket.Api.Authentication.Filters;
using SurveyBasket.Api.Health;
using System.Threading.RateLimiting;

namespace SurveyBasket.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services
            , IConfiguration configuration)
        {
            // Add services to the container.
            services.AddControllers();

            //CORS part to include any origin (*) "Wildcard"
            services.AddCors(options => options.
            AddPolicy("Allow All",
            builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyHeader()));

            services.AddSwagerServices();
            services.AddMapsterServices();
            services.AddFluentValidationServices();
            services.AddAuthConfig(configuration);
            services.AddHangFireServices(configuration);

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<IPollService, PollService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IVoteService, VoteService>();
            services.AddScoped<IResultService, ResultService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IEmailSender, EmailService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            services.AddHttpContextAccessor();

            services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));


            //RateLimiter
            services.AddRateLimiter(rateLimiterOptions =>
            {
                rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                //rateLimiterOptions.AddPolicy("CustomUserLimiter", httpContext =>
                //    RateLimitPartition.GetFixedWindowLimiter
                //    (
                //        partitionKey: httpContext.User.GetUserId(),
                //        factory: x => new FixedWindowRateLimiterOptions
                //        {
                //            PermitLimit = 2,
                //            Window = TimeSpan.FromSeconds(15)
                //        }
                //    )
                //);
                //rateLimiterOptions.AddPolicy("CustomIpAdrressRateLimit", httpContext =>
                //    RateLimitPartition.GetFixedWindowLimiter
                //    (
                //        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                //        factory : x => new FixedWindowRateLimiterOptions
                //        {
                //            PermitLimit = 2,
                //            Window = TimeSpan.FromSeconds(15)
                //        }
                //    )
                //);
                rateLimiterOptions.AddConcurrencyLimiter("ConcurrencyLimiter", options =>
                {
                    options.PermitLimit = 500; //max number i can handle
                    options.QueueLimit = 50; // max number i can add to queue
                    options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
                });
                //rateLimiterOptions.AddTokenBucketLimiter("BucketLimiter", options =>
                //{
                //    options.TokenLimit = 2;
                //    options.AutoReplenishment = true;
                //    options.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
                //    options.TokensPerPeriod = 2;
                //    options.QueueLimit = 1;
                //});
                //rateLimiterOptions.AddFixedWindowLimiter("FixedWindowLimiter", options =>
                //{
                //    options.AutoReplenishment = true;
                //    options.QueueLimit = 1;
                //    options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
                //    options.PermitLimit = 2;
                //    options.Window = TimeSpan.FromSeconds(15);
                //});
                //rateLimiterOptions.AddSlidingWindowLimiter("SlidingWindowLimiter", options =>
                //{
                //    options.QueueLimit = 1;
                //    options.PermitLimit = 2;
                //    options.SegmentsPerWindow = 2;
                //    options.Window = TimeSpan.FromSeconds(20); // time span for entire window will be divided into segements
                //});
            });
            //HealthCheck
            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>(name: "DataBase")
                .AddHangfire(options => options.MinimumAvailableServers = 1)
                .AddCheck<MailServiceHealthChecker>(name : "Mail Service");

            //Versioning
            //services.AddApiVersioning(options =>
            //{
            //    options.ApiVersionReader = new UrlSegmentApiVersionReader();
            //}).AddApiExplorer(options =>
            //{
            //    options.GroupNameFormat = "'v'V";
            //    options.SubstituteApiVersionInUrl = true;
            //});
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                //"api-version" is the name of var that will recieve the version
                options.ApiVersionReader = new HeaderApiVersionReader("api-version");
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
            return services;
        }
        private static IServiceCollection AddSwagerServices(this IServiceCollection services)
        {           
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
        private static IServiceCollection AddMapsterServices(this IServiceCollection services)
        {
            // for global config of mapster
            var mappingConfig = TypeAdapterConfig.GlobalSettings;
            mappingConfig.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(mappingConfig));
            return services;
        }
        private static IServiceCollection AddFluentValidationServices(this IServiceCollection services)
        {
            //for fluent validating in (Validations/)
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddFluentValidationAutoValidation();
            return services;
        }
        private static IServiceCollection AddAuthConfig(this IServiceCollection services,
        IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IAuthorizationHandler , PermissionAuthorizationHandler>();
            services.AddTransient<IAuthorizationPolicyProvider ,  PermissionAuthorizationPolicyProvider>(); 
            services.AddSingleton<IJwtProvider, JwtProvider>();
            //data annotaion validating in JwtOptions
            services.AddOptions<JwtOptions>().BindConfiguration(JwtOptions.SectionName).ValidateDataAnnotations()
                .ValidateOnStart();

            var setting = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(setting.Key)),
                    ValidIssuer = setting.Issuer,
                    ValidAudience = setting.Audience
                };
            });
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;

                //default values except accessAttempts
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;
            });
            return services;
        }

        private static IServiceCollection AddHangFireServices(this IServiceCollection services , IConfiguration configuration)
        {
            // Add Hangfire services.
            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

            // Add the processing server as IHostedService
            services.AddHangfireServer();
            return services;
        }
    }
}
