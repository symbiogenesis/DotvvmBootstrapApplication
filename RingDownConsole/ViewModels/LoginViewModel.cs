using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using RingDownConsole.Models;
using RingDownConsole.Models.Enums;

namespace RingDownConsole.ViewModels
{
    public class LoginViewModel : MasterViewModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public LoginViewModel(
            IOptionsSnapshot<AppSettings> appSettings,
            UserManager<User> userManager,
            SignInManager<User> signInManager) : base(appSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Bind(Direction.ServerToClientFirstRequest)]
        public override bool ShowLogout => false;

        [Bind(Direction.ClientToServer)]
        public string UserNameInput { get; set; }

        [Bind(Direction.ClientToServer)]
        public string PasswordInput { get; set; }

        [Bind(Direction.ServerToClientFirstRequest)]
        public override string PageTitle => $"Login";

        [AllowAnonymous]
        public async Task LogIn()
        {
            if (string.IsNullOrWhiteSpace(UserNameInput))
            {
                ShowError("Please enter a username");
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordInput))
            {
                ShowError("Please enter a password");
                return;
            }

            var user = await _userManager.FindByNameAsync(UserNameInput);

            if (user == null)
                ShowError($"User: {UserNameInput} not found");

            if (_userManager.CheckPasswordAsync(user, PasswordInput).Result)
            {
                // password is correct
                var isAdmin = await _userManager.IsInRoleAsync(user, nameof(Roles.Administrator));

                if (_userManager.CheckPasswordAsync(user, PasswordInput).Result)
                {
                    // password is correct
                    var identityPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
                    var principal = ConvertPrincipal(identityPrincipal, CookieAuthenticationDefaults.AuthenticationScheme);

                    var type = principal.Identity.AuthenticationType;

                    //var result = await _authService.AuthorizeAsync(principal, nameof(Policies.User));
                    //await _signInManager.SignInAsync(user, false, "ApplicationCookie");

                    await Context.GetAuthentication().SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                }
            }
            else
            {
                throw new Exception("This username and password combination is not correct.");
            }
        }

        private ClaimsPrincipal ConvertPrincipal(ClaimsPrincipal originalPrincipal, string authenticationScheme)
        {
            var principal = new ClaimsPrincipal();

            foreach (var originalIdentity in originalPrincipal.Identities)
            {
                var identity = new ClaimsIdentity(originalIdentity.Claims, authenticationScheme);
                principal.AddIdentity(identity);
            }

            return principal;
        }
    }
}