namespace xUnitSample.Infrastructure.Helpers;

public interface IJwtHelper
{
    string GenerateJwtToken(string id);
}