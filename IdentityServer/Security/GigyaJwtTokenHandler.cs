using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityModel;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Security
{
    public class GigyaJwtTokenHandler:JwtSecurityTokenHandler
    {
        protected override ClaimsIdentity CreateClaimsIdentity(JwtSecurityToken jwtToken, string issuer,
            TokenValidationParameters validationParameters)
        {
            var claimsIdentity = base.CreateClaimsIdentity(jwtToken, issuer, validationParameters);
            claimsIdentity.AddClaim(  new Claim(JwtClaimTypes.Subject, jwtToken.Subject));
            return claimsIdentity;
        }

    
    }
}