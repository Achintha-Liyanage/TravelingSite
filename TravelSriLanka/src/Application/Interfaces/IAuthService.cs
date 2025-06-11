
using Application.DTOs.Auth;
using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<User?> RegisterAsync(UserForRegisterDto userForRegisterDto);
    Task<string?> LoginAsync(UserForLoginDto userForLoginDto);
    // bool UserExists(string username); // Might be useful
}
