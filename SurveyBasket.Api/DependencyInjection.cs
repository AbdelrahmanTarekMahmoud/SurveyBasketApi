
namespace SurveyBasket.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            // Add services to the container.
            services.AddControllers();
            services.AddSwagerServices();
            services.AddMapsterServices();
            services.AddFluentValidationServices();


            services.AddScoped<IPollService, PollService>();

            

            return services;
        }
        public static IServiceCollection AddSwagerServices(this IServiceCollection services)
        {           
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
        public static IServiceCollection AddMapsterServices(this IServiceCollection services)
        {
            // for global config of mapster
            var mappingConfig = TypeAdapterConfig.GlobalSettings;
            mappingConfig.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(mappingConfig));
            return services;
        }
        public static IServiceCollection AddFluentValidationServices(this IServiceCollection services)
        {
            //for fluent validating in (Validations/)
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddFluentValidationAutoValidation();
            return services;
        }
    }
}
