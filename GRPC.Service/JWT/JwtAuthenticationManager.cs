using GRPC.Service.Protos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GRPC.Service.JWT;

public static class JwtAuthenticationManager
{
    public const string JWT_TOKEN_Key = "3G9q2rFwHvMnbQrEucVa5Vt6XQpPah+Ij4Bk6iN3c9U=";
    private const int JWT_TOKEN_VALIDITY = 30;
    public static AuthenticationResponse Authenticate(AuthenticationRequest request)
    {
        if (!string.Equals(request.UserName, "muhammadjamal", StringComparison.OrdinalIgnoreCase) || !string.Equals(request.Password, "admin@123"))
            return null;

        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

        byte[] tokenKey = Encoding.UTF8.GetBytes(JWT_TOKEN_Key);

        DateTime tokenExpiryDateTime = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY);

        SecurityTokenDescriptor securityTokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(new List<Claim>{
                new Claim("username", request.UserName),
                new Claim(ClaimTypes.Role, "Admin")
            }),
            Expires = tokenExpiryDateTime,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
        };

        SecurityToken? securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

        string token = jwtSecurityTokenHandler.WriteToken(securityToken);

        return new AuthenticationResponse()
        {
            AccessToken = token,
            ExpiresIn = (int)tokenExpiryDateTime.Subtract(DateTime.Now).TotalSeconds
        };
    }
}
