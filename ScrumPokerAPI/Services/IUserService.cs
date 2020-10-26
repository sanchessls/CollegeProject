
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ScrumPokerPlanning.APIViewModel;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ScrumPokerPlanning.Services
{
    public interface  IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);
        Task<UserManagerResponse> LoginUserAsync(LoginViewModel model);
    }

    public class UserService : IUserService
    {
        private UserManager<IdentityUser> _userManager;
        private IConfiguration _configuration;

        public UserService(UserManager<IdentityUser> userManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }


        public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException("Register model is null.");
            }

            UserManagerResponse validation = ValidateRegisterModel(model);

            if (validation != null)
            {
                return validation;
            }

            IdentityUser identityUser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email
            };

            IdentityResult newUser = await _userManager.CreateAsync(identityUser,model.Password);

            if (newUser.Succeeded)
            {
                return new UserManagerResponse
                {
                    Message = "User created successfully!",
                    IsSuccess = true
                };
            }
            else
            {
                return new UserManagerResponse
                {
                    Message = "Failed to create the user",
                    IsSuccess = false,
                    Errors = newUser.Errors.Select(x => x.Description)
                };
            }
        }


        public async Task<UserManagerResponse> LoginUserAsync(LoginViewModel model)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "There is no user with that email address!",
                    IsSuccess = false
                };
            }

            UserManagerResponse validation = ValidateLoginModel(model);

            if (validation != null)
            {
                return validation;
            }

            bool result = await _userManager.CheckPasswordAsync(user,model.Password);

            if (!result)
            {
                return new UserManagerResponse
                {
                    Message = "Invalid password!",
                    IsSuccess = false
                };
            }
            else
            {
                var claims = new[]
                {
                    new Claim("Email",model.Email),
                    new Claim(ClaimTypes.Name,user.Id)
                };

                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: _configuration["AuthSettings:Issuer"],
                    audience: _configuration["AuthSettings:Audience"],                    
                    claims: claims,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)                    
                    );

                string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

                return new UserManagerResponse
                {
                    Message = tokenAsString,
                    IsSuccess = true,
                    ExpireDate = token.ValidTo
                };
            }


        }

        private UserManagerResponse ValidateRegisterModel(RegisterViewModel model)
        {
            //validating password and confirm password
            if (model.Password != model.ConfirmPassword)
            {
                return new UserManagerResponse
                {
                    Message = "Passwords don't match!",
                    IsSuccess = false
                };
            }

            return null;
        }

        private UserManagerResponse ValidateLoginModel(LoginViewModel model)
        {
            return null;
        }
    }
}
