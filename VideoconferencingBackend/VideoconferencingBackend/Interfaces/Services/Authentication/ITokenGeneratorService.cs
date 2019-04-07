using VideoconferencingBackend.Models.DBModels;

namespace VideoconferencingBackend.Interfaces.Services.Authentication
{
    public interface ITokenGeneratorService
    {
        string GenerateToken(User user);
    }
}
