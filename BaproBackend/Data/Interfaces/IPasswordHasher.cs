using BaproBackend.Data.Models;

namespace BaproBackend.Data.Interfaces
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashed);
    }
}
