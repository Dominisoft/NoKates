using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NoKates.Common.Infrastructure.Configuration;
using NoKates.Common.Infrastructure.CustomExceptions;
using NoKates.Common.Infrastructure.Extensions;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;
using NoKates.Identity.Models;

namespace NoKates.Identity.Services
{
    public interface IAuthenticationService
    {
        string Authenticate(string username, string password);

    }
    public class AuthenticationService : IAuthenticationService
    {
        public string Authenticate(string username, string password)
        {

            var userRepo = RepositoryHelper.CreateRepository<User>();
            var roleRepo = RepositoryHelper.CreateRepository<Role>();

            var user = userRepo.GetAll().FirstOrDefault(u => u.Username?.Trim() == username.Trim() || u.Email?.Trim() == username.Trim());
            if (user == null)
            {
                throw new AuthorizationException("Incorrect Username or Password");
            }

            var roleIds = user.Roles.Remove("[", "]", "\"").Split(",");
            var roles = roleIds.Select(role => roleRepo.Get(Convert.ToInt32(role))).ToList();

            var userRoles = new UserRoles
            {
                User = user,
                Roles = roles
            };
            return GetToken(userRoles);

        }
        private string GetToken(UserRoles userRoles)
        {
            ConfigurationValues.TryGetValue(out var key, "JwtKey");
            ConfigurationValues.TryGetValue(out var issuer, "JwtIssuer");

            var user = userRoles.User;
            var roles = userRoles.Roles;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("valid", "1"),
                new Claim("userid", user.Id.ToString()),
                new Claim("name", user.Username)
            };


            var effectivePermissions = userRoles.Roles.SelectMany(role => role.AllowedEndpoints?.Trim('[', ']')?.Split(",")).ToList();
            effectivePermissions.AddRange(userRoles.User.AdditionalEndpointPermissions?.Trim('[', ']')?.Split(","));


            foreach (var perm in effectivePermissions)
            {
                permClaims.Add(new Claim("endpoint_permission", perm.Remove("\"")));
            }

            foreach (var role in userRoles.Roles)
            {
                permClaims.Add(new Claim("role_name", role.Name));
            }
            var expirationDays = roles.Any(r => r.Name == "Service") ? 60 : 5;

            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(issuer, //Issure    
                            issuer,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddDays(expirationDays),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt_token;
        }
    }
}
