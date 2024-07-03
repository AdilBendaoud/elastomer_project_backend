namespace projetStage.Services
{
    public interface ITokenService
    {
        string GenerateToken(string email, string role);
    }
}
