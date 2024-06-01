using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Services;
using WebApplication2.ViewModels;

namespace WebApplication2.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AccountRepository(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IHttpContextAccessor httpContextAccessor,
            AppDbContext appDbContext,
            IConfiguration configuration,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _appDbContext = appDbContext;
            _configuration = configuration;
            _emailService = emailService;
        }
        public async Task<IdentityResult> CreateUserAsync(SignUpModel userModel)
        {
            var user = new AppUser()
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email,
                UserName = userModel.Email,
                PhoneNumber = userModel.PhoneNumber,
                Address = userModel.Address,
                DateOfBirth =userModel.BirthDate

            };
            var result =  await _userManager.CreateAsync(user,userModel.Password);
            return result;
        }
        public async Task<SignInResult> PasswordSignInAsync(SignInModel signInModel)
        {
           return await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, signInModel.RememberMe,false);
           
        }
        public string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(principal);
            return userId;

        }
        public async Task GenerateForgotPasswordTokenAsync(AppUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                await SendForgotPasswordEmail(user, token);
            }
        }
        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            return await _userManager.ResetPasswordAsync(await _userManager.FindByIdAsync(model.UserId), model.Token, model.NewPassword);
        }
        public async Task SendForgotPasswordEmail(AppUser user, string token)
        {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = _configuration.GetSection("Application:ForgotPassword").Value;

            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", user.FirstName),
                    new KeyValuePair<string, string>("{{Link}}",
                        string.Format(appDomain + confirmationLink, user.Id, token))
                }
            };

            await _emailService.SendEmailForForgotPassword(options);
        }
        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public Task<AppUser> GetUserById(string userId)
        {
            throw new NotImplementedException();
        }

     
    }
}
 