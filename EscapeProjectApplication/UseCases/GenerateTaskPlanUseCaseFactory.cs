using EscapeProjectApplication.Services.Configuration;

namespace EscapeProjectApplication.UseCases
{
    public interface GenerateTaskPlanUseCaseFactory
    {
        GenerateTaskPlanUseCase Create(ConfigurationService configService);
    }
}
