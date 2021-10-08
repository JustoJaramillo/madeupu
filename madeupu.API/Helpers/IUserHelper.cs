using madeupu.API.Data.Entities;
using madeupu.API.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace madeupu.API.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserAsync(string email);
        Task<IdentityResult> AddUserAsync(User user, string password);
        Task AddUserToRoleAsync(User user, string roleName);
        Task CheckRoleAsync(string roleName);
        Task<bool> IsUserInRoleAsync(User user, string roleName);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();



        /*
        Task<User> GetUserAsync(Guid id);

        Task<User> AddUserAsync(AddUserViewModel model, Guid imageId, UserType userType);

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IdentityResult> DeleteUserAsync(User user);


        
        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

        Task<IdentityResult> ConfirmEmailAsync(User user, string token);

        Task<string> GenerateEmailConfirmationTokenAsync(User user);

        Task<string> GeneratePasswordResetTokenAsync(User user);

        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);

        Task<SignInResult> ValidatePasswordAsync(User user, string password);*/


    }
}
