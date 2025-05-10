using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Avalentini.Expensi.Domain.Data.Repository;
using Avalentini.Expensi.IdentityServer.Data.Entities;
using IdentityModel;

namespace Avalentini.Expensi.IdentityServer
{

    public class ProfileService : IProfileService
    {
        //services
        private readonly IRepository<UserEntity> _userRepository;

        public ProfileService(IRepository<UserEntity> userRepository)
        {
            _userRepository = userRepository;
        }

        //Get user profile date in terms of claims when calling /connect/userinfo
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                //depending on the scope accessing the user data.
                // todo not working for sure
                if (!string.IsNullOrEmpty(context.Subject.Identity.Name))
                {
                    //get user from db (in my case this is by email)
                    var id = context.Subject.Identity.Name;
                    var user = await _userRepository.Get(id);

                    if (user != null)
                    {
                        var claims = GetUserClaims(user);

                        //set issued claims to return
                        context.IssuedClaims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
                    }
                }
                else
                {
                    //get subject from context (this was set ResourceOwnerPasswordValidator.ValidateAsync),
                    //where and subject was set to my user id.
                    var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub");

                    if (!string.IsNullOrEmpty(userId?.Value) && long.Parse(userId.Value) > 0)
                    {
                        //get user from db (find user by user id)
                        var user = await _userRepository.Get(userId.Value);

                        // issue the claims for the user
                        if (user != null)
                        {
                            var claims = GetUserClaims(user);

                            //context.IssuedClaims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
                            // TODO: remember to filter claims based on RequestedClaimTypes
                            context.IssuedClaims = claims.ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //check if user account is active.
        public async Task IsActiveAsync(IsActiveContext context)
        {
            try
            {
                //get subject from context (set in ResourceOwnerPasswordValidator.ValidateAsync),
                var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "user_id");

                if (!string.IsNullOrEmpty(userId?.Value) && long.Parse(userId.Value) > 0)
                {
                    var user = await _userRepository.Get(userId.Value);

                    if (user != null)
                    {
                        // todo refactor
                        context.IsActive = true;
                        //if (user.IsActive)
                        //{
                        //    context.IsActive = user.IsActive;
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //build claims array from user data
        public static Claim[] GetUserClaims(UserEntity user)
        {
            return new []
            {
                new Claim("user_id", user.Id.ToString() ?? ""),
                new Claim(JwtClaimTypes.Name, (!string.IsNullOrEmpty(user.Firstname) && !string.IsNullOrEmpty(user.Lastname)) ? (user.Firstname + " " + user.Lastname) : ""),
                new Claim(JwtClaimTypes.GivenName, user.Firstname  ?? ""),
                new Claim(JwtClaimTypes.FamilyName, user.Lastname  ?? ""),
                new Claim(JwtClaimTypes.Email, user.ContactEmail  ?? ""),

                //roles
                //new Claim(JwtClaimTypes.Role, user.Role)
            };
        }
    }

}
