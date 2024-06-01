using Microsoft.AspNetCore.Identity;
using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Repository
{
    public interface IAccountRepository
    {
        Task <IdentityResult> CreateUserAsync(SignUpModel signUpModel);
        Task<SignInResult> PasswordSignInAsync(SignInModel signInModel);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordViewModel model);
        Task SignOutAsync();
        Task<AppUser> GetUserById(string userId);
        Task SendForgotPasswordEmail(AppUser user, string token);
        Task GenerateForgotPasswordTokenAsync(AppUser user);
        Task<AppUser> GetUserByEmailAsync(string email);
        string GetUserId();
    }
}