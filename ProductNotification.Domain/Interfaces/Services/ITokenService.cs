using ProductNotification.Domain.Entities;

namespace ProductNotification.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        public string GenerateToken(User user);
    }
}