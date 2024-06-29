using BaproBackend.Data.DTO;
using BaproBackend.Data.Models;

namespace BaproBackend.Services.Interfaces
{
    public interface IUserServices
    {
        Task<bool> Register(UsersDTO user);
        Task<string> Login(Login credentials);
    }
}
