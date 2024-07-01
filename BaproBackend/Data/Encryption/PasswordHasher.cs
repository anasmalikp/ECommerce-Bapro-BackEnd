using BaproBackend.Data.Interfaces;
using BaproBackend.Data.Models;
using System.IdentityModel.Tokens.Jwt;

namespace BaproBackend.Data.Encryption
{
    public class PasswordHasher:IPasswordHasher
    {
        public string HashPassword(string password)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public bool VerifyPassword(string password, string hashed)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashed);
        }

        public static userdetails TokenDecode(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var decoded = tokenHandler.ReadJwtToken(token);
            var userid = decoded.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            var username = decoded.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
            userdetails user = new userdetails
            {
                user_id = userid,
                username = username
            };
            return user;
        }
    }
}
