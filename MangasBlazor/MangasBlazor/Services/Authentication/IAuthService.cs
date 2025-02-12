namespace MangasBlazor.Services.Authentication
{
    using MangasBlazor.Models;

    public interface IAuthService
    {
        Task<LoginResult> Login(LoginModel loginModel);

        Task Logout();
    }
}
