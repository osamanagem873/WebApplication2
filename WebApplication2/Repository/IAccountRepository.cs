using Microsoft.AspNetCore.Identity;
using WebApplication2.Models;

namespace WebApplication2.Repository
{
    public interface IAccountRepository
    {
        Task <IdentityResult> CreateUserAsync(SignUpModel signUpModel);
        Task<SignInResult> PasswordSignInAsync(SignInModel signInModel);
        Task SignOutAsync();
    }
}