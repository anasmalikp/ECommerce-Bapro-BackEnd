using AutoMapper;
using BaproBackend.Data;
using BaproBackend.Data.DTO;
using BaproBackend.Data.Interfaces;
using BaproBackend.Data.Models;
using BaproBackend.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BaproBackend.Services
{
    public class UserServices:IUserServices
    {
        private readonly IDataProvider provider;
        private readonly IMapper mapper;
        private readonly IPasswordHasher hasher;
        private readonly IConfiguration config;
        public UserServices(IDataProvider provider, IMapper mapper, IPasswordHasher hasher, IConfiguration config)
        {
            this.provider = provider;
            this.mapper = mapper;
            this.hasher = hasher;
            this.config = config;
        }

        public async Task<bool> Register(UsersDTO user)
        {
            try
            {

                var existingEmail = await provider.GetAllByCondition<users>(Constants.Tables.users.ToString(), new users { email = user.email });
                var existingUsername = await provider.GetAllByCondition<users>(Constants.Tables.users.ToString(), new users { username = user.username });
                if (existingEmail.Count()!=0 && existingUsername.Count()!=0)
                {
                    Console.WriteLine("Username or Email already exists");
                    return false;
                }
                var usr = mapper.Map<users>(user);
                usr.id = Constants.GenerateId();
                usr.password = hasher.HashPassword(user.password);
                var result = await provider.Insert<users>(Constants.Tables.users.ToString(), usr);
                if(result < 1)
                {
                    Console.WriteLine("something went wrong while registering");
                    return false;
                }
                Console.WriteLine("user registered successfully");
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        public async Task<string> Login(Login credentials)
        {
            var user = await provider.GetAllByCondition<users>(Constants.Tables.users.ToString(),new users { username = credentials.username });
            if(user.Count()== 0)
            {
                Console.WriteLine("wrong username or user doesn't exist");
                return null;
            }
            var possibleUser = user.FirstOrDefault();
            var password = hasher.VerifyPassword(credentials.password, possibleUser.password);
            if(password)
            {
                return GetToken(possibleUser);
            }
            Console.WriteLine("wrong password");
            return null;
        }

        private string GetToken(users user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("hdhdhdhdhdhhdyyryryrrydhdhdhdhdffzsfzfsfasfasasszfszfdhcbcbbc"));
            var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim (ClaimTypes.NameIdentifier, user.id),
                new Claim (ClaimTypes.Name, user.username)
            };
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddDays(1));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
