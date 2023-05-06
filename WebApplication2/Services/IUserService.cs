namespace WebApplication2.Services
{
    public interface IUserService
    {
        string GetUserId();
        bool IsAuthenticated();
    }
}