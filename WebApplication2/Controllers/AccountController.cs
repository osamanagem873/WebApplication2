using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Repository;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ICartRepository _cartRepository;

        public AccountController(IAccountRepository accountRepository, IHttpContextAccessor httpContextAccessor,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ICartRepository cartRepository
            )
        {
            _accountRepository = accountRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
            _cartRepository = cartRepository;
        }

     
        public IActionResult SignUp()
        {
            return View();
        }

       
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpModel userModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.CreateUserAsync(userModel);
                if (!result.Succeeded)
                {
                    foreach (var errorMessage in result.Errors)
                    {
                        ModelState.AddModelError("", errorMessage.Description);
                    }
                    return View(userModel);
                }
                ModelState.Clear();
                return RedirectToAction("Login");
            }
            return View(userModel);
        }

        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

       
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(SignInModel signInModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.PasswordSignInAsync(signInModel);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Credentials");
                }
            }
            return View(signInModel);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    if (error.Code == "PasswordMismatch")
                    {
                        ModelState.AddModelError("CurrentPassword", "Incorrect password.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var model = new EditProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Address = model.Address;
            user.PhoneNumber = model.PhoneNumber;
            user.DateOfBirth = model.DateOfBirth;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // code here
                var user = await _accountRepository.GetUserByEmailAsync(model.Email);
                if (user != null)
                {
                    await _accountRepository.GenerateForgotPasswordTokenAsync(user);
                }

                ModelState.Clear();
                model.EmailSent = true;
            }
            return View(model);
        }

        
        
        public ViewResult ResetPassword(string uid, string token)
        {
            ResetPasswordViewModel resetPasswordModel = new ResetPasswordViewModel
            {
                Token = token,
                UserId = uid
            };
            return View(resetPasswordModel);
        }

        [AllowAnonymous, HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Token = model.Token.Replace(' ', '+');
                var result = await _accountRepository.ResetPasswordAsync(model);
                if (result.Succeeded)
                {
                    ModelState.Clear();
                    model.IsSuccess = true;
                    return View(model);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            // Delete cart items and set cart item count to zero if cart item count is not zero
            var userId = _accountRepository.GetUserId();
            var cartItemCount =  await _cartRepository.GetCartItemCount(userId);
            if (cartItemCount != 0 && !string.IsNullOrEmpty(userId))
            {
                await _cartRepository.DeleteAllCartItems(userId);
                
            }

            // Sign out user and redirect to home page
            await _accountRepository.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}