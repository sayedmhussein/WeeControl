using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace WeeControl.Applications.BaseLib.Services
{
    public class JwtTokenService
    {
        static public IEnumerable<Claim> GetClaims(string token)
        {
            try
            {
                var c = token.Split('.');
                var a = JwtPayload.Base64UrlDeserialize(c[1]);
                return a.Claims;
            }
            catch
            {
                return null;
            }
        }

        private IEnumerable<Claim> claims;

        public JwtTokenService(string token)
        {
            claims = GetClaims(token);
        }

        public bool IsIssuedBefore(DateTime dateTime)
        {
            var iat = claims.FirstOrDefault(x => x.Type == "iat");
            return true;
        }
    }
}
