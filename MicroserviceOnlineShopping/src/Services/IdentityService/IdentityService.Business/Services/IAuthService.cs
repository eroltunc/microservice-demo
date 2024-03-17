using EventBus.Base.Abstraction;
using IdentityService.Business.IntegrationEvents.Events;
using IdentityService.Core.Results;
using IdentityService.Entity.Concrete;
using IdentityService.Entity.Models.RequestModels;
using IdentityService.Entity.Models.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Security.Claims;

namespace IdentityService.Business.Services
{
    public interface IAuthService
    {
        Task<IDataResult<Dictionary<string, string[]>>> Register(CreateUserRequestModel createUserRequestModel);
        Task<IDataResult<TokenResponseModel>> Login(LoginUserRequestModel loginUserRequestModel, bool? useCookies, bool? useSessionCookies);
        Task<IDataResult<UserInfoResponseModel>> Info();
    }
    public class AuthService : IAuthService
    {
        private readonly IEventBus _eventBus;
        private readonly ITokenService _tokenService;
        // Validate the email address using DataAnnotations like the UserValidator does when RequireUniqueEmail = true.
        private static readonly EmailAddressAttribute _emailAddressAttribute = new();
        // We'll figure out a unique endpoint name based on the final route pattern during endpoint generation.
        string? confirmEmailEndpointName = null;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;

        public AuthService
            (
            ITokenService tokenService, 
            IEventBus eventBus,
            IUserStore<ApplicationUser> userStore, 
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager
            )
        {
            _tokenService = tokenService;
            _eventBus = eventBus;
            _userStore = userStore;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IDataResult<Dictionary<string, string[]>>> Register(CreateUserRequestModel createUserRequestModel)
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException($"requires a user store with email support.");
            }

            var emailStore = (IUserEmailStore<ApplicationUser>)_userStore;
            var email = createUserRequestModel.Email;

            if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
            {
                return CreateValidationProblem(IdentityResult.Failed(_userManager.ErrorDescriber.InvalidEmail(email)));
            }
            var user = new ApplicationUser()
            {
                Email = createUserRequestModel.Email,
                FullName = createUserRequestModel.FullName,
                Status = "New User",
                LastSeen = DateTime.Now
            };

            await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, createUserRequestModel.Password);

            if (!result.Succeeded)
                return CreateValidationProblem(result);

            return new SuccessDataResult<Dictionary<string, string[]>>();
        }
        public async Task<IDataResult<TokenResponseModel>> Login(LoginUserRequestModel loginUserRequestModel, bool? useCookies, bool? useSessionCookies)
        {
            var useCookieScheme = (useCookies == true) || (useSessionCookies == true);
            var isPersistent = (useCookies == true) && (useSessionCookies != true);
            var result = await _signInManager.PasswordSignInAsync(loginUserRequestModel.Email, loginUserRequestModel.Password, isPersistent, lockoutOnFailure: true);

            if (result.RequiresTwoFactor)
            {
                if (!string.IsNullOrEmpty(loginUserRequestModel.TwoFactorCode))
                {
                    result = await _signInManager.TwoFactorAuthenticatorSignInAsync(loginUserRequestModel.TwoFactorCode, isPersistent, rememberClient: isPersistent);
                }
                else if (!string.IsNullOrEmpty(loginUserRequestModel.TwoFactorRecoveryCode))
                {
                    result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(loginUserRequestModel.TwoFactorRecoveryCode);
                }
            }

            if (!result.Succeeded)
                return new ErrorDataResult<TokenResponseModel>(result.ToString());
            var user = await _userManager.FindByEmailAsync(loginUserRequestModel.Email);
            var eventMessage = new UserLoggedIntegrationEvent(loginUserRequestModel.Email, user.FullName);
            _eventBus.Publish(eventMessage);
            return _tokenService.GetTokenAsync(user.Email, user.FullName,user.Id);
        }
        public async Task<IDataResult<UserInfoResponseModel>> Info()
        {
        
            var sistem = _signInManager.Context.User;
            var email = GetUserEmailFromClaims(sistem);
            if (email == null)
                return new ErrorDataResult<UserInfoResponseModel>("Email bulunamadı.");
            var user2 = await _userManager.FindByEmailAsync(email!);
            if (user2 is not { } user)
                return new ErrorDataResult<UserInfoResponseModel>();
            return CreateInfoResponseAsync(user);
        }
        private string? GetUserEmailFromClaims(ClaimsPrincipal claimsPrincipal) => claimsPrincipal.FindFirstValue(ClaimTypes.Email);
        private static IDataResult<UserInfoResponseModel> CreateInfoResponseAsync(ApplicationUser user)
        {
            return new SuccessDataResult<UserInfoResponseModel>(new UserInfoResponseModel()
            {
                Email = user.Email!,
                FullName = user.FullName,
                IsEmailConfirmed = user.EmailConfirmed
            });
        }
        private IDataResult<Dictionary<string, string[]>> CreateValidationProblem(IdentityResult result)
        {
          
            Debug.Assert(!result.Succeeded);
            var errorDictionary = new Dictionary<string, string[]>(1);

            foreach (var error in result.Errors)
            {
                string[] newDescriptions;

                if (errorDictionary.TryGetValue(error.Code, out var descriptions))
                {
                    newDescriptions = new string[descriptions.Length + 1];
                    Array.Copy(descriptions, newDescriptions, descriptions.Length);
                    newDescriptions[descriptions.Length] = error.Description;
                }
                else
                    newDescriptions = [error.Description];
                errorDictionary[error.Code] = newDescriptions;
            }
            return new ErrorDataResult<Dictionary<string, string[]>>(errorDictionary);
        }
    }
}
