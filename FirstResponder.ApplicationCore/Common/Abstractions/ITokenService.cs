using FirstResponder.ApplicationCore.Entities.UserAggregate;

namespace FirstResponder.ApplicationCore.Common.Abstractions;

public interface ITokenService
{
    string GenerateToken(User user);
}