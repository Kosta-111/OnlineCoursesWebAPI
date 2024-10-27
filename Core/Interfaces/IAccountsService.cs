using Core.Models;

namespace Core.Interfaces;

public interface IAccountsService
{
    Task Register(RegisterModel model);
    Task Login(LoginModel model);
    Task Logout();
    Task SendPasswordResetToken(PasswordResetTokenRequest req);
    Task ResetPassword(ResetPasswordModel model);
}
