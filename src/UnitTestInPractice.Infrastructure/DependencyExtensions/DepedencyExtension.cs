using Microsoft.Extensions.DependencyInjection;
using UnitTestInPractice.Infrastructure.Repository;

namespace UnitTestInPractice.Infrastructure.DependencyExtensions;

public static class DepedencyExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IMovieRepository, AssessmentRepository>();
        services.AddScoped<IReadOnlyMovieRepository, ReadOnlyAssessmentRepository>();
        services.AddScoped<IEventLogRepository, EventLogRepository>();

        return services;
    }
}

