using Microsoft.Extensions.DependencyInjection;
using UnitTestInPractice.Infrastructure.Repository;

namespace UnitTestInPractice.Infrastructure.DependencyExtensions;

public static class DepedencyExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IAssessmentRepository, AssessmentRepository>();
        services.AddScoped<IReadOnlyAssessmentRepository, ReadOnlyAssessmentRepository>();
        

        return services;
    }
}

