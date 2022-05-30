using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WeeControl.SharedKernel.Interfaces;

public interface IJwtService
{
    string GenerateToken(SecurityTokenDescriptor descriptor);

    ClaimsPrincipal ExtractClaimPrincipalWithoutValidationParameter(string token);
    ClaimsPrincipal ExtractClaimPrincipalWithValidationParameter(string token, TokenValidationParameters parameters);
}