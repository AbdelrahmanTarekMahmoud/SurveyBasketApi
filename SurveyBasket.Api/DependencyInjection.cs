
namespace SurveyBasket.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services
            ,IConfiguration configuration)
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
            services.AddScoped<IAuthService , AuthService>();
            services.AddScoped<IQuestionService , QuestionService>();
            services.AddScoped<IVoteService, VoteService>();
            services.AddScoped<IResultService , ResultService>();
            services.AddScoped<ICacheService , CacheService>();
            services.AddScoped<IEmailSender , EmailService>();
            services.AddScoped<INotificationService ,  NotificationService>();
            services.AddScoped<IUserService , UserService>();

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            services.AddHttpContextAccessor(); 

            services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));
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
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
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
