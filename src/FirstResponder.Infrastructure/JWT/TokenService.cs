using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FirstResponder.Infrastructure.JWT;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IRefreshTokensRepository _refreshTokensRepository;

    public TokenService(IConfiguration configuration, IRefreshTokensRepository refreshTokensRepository)
    {
        _configuration = configuration;
        _refreshTokensRepository = refreshTokensRepository;
    }
    
    public string GenerateAccessToken(User user)
    {
        // Generate JWT token
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("FullName", user.FullName),
            new Claim("UserType", user.Type.ToString())
        };

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Issuer"],
            claims,
            expires: DateTime.Now.AddMinutes(_configuration.GetValue<int>("Jwt:ExpireMinutes")),
            signingCredentials: credentials
        );
        
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenString;
    }

    public Guid? GetUserIdFromExpiredToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
            }, out var validatedToken);
            
            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = Guid.Parse(jwtToken.Subject);
            return userId;
        }
        catch
        {
            return null;
        }
    }

    public async Task<RefreshToken> GenerateAndStoreRefreshToken(User user)
    {
        string token;
        
        do
        {
            var number = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(number);
        
            token = Convert.ToBase64String(number);
        } while (await _refreshTokensRepository.GetRefreshToken(token) != null);
        
        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = token,
            Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:RefreshExpireMinutes"))
        };
        
        await _refreshTokensRepository.AddRefreshToken(refreshToken);
        
        return refreshToken;
    }

    public async Task<RefreshToken?> GetRefreshTokenModel(string token)
    {
        return await _refreshTokensRepository.GetRefreshToken(token);
    }

    public async Task DeleteRefreshToken(RefreshToken refreshToken)
    {
        await _refreshTokensRepository.DeleteRefreshToken(refreshToken);
    }
}