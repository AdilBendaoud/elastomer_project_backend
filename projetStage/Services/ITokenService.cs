using projetStage.Models;

namespace projetStage.Services
{
    public interface ITokenService
    {
        string GenerateToken(string userCode, List<string> roles);
    }
}
