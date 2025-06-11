
using Application.DTOs.Auth;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IGenericRepository<User> userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<User?> RegisterAsync(UserForRegisterDto userForRegisterDto)
    {
        // Check if user already exists (basic check, consider case sensitivity and normalization)
        var existingUsers = await _userRepository.GetAllAsync();
        if (existingUsers.Any(u => u.Username.Equals(userForRegisterDto.Username, StringComparison.OrdinalIgnoreCase)))
        {
            return null; // Or throw an exception
        }

        CreatePasswordHash(userForRegisterDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var user = new User
        {
            Username = userForRegisterDto.Username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = string.IsNullOrWhiteSpace(userForRegisterDto.Role) ? "User" : userForRegisterDto.Role
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
        return user;
    }

    public async Task<string?> LoginAsync(UserForLoginDto userForLoginDto)
    {
        var users = await _userRepository.GetAllAsync(); // In a real app, query by username directly
        var user = users.FirstOrDefault(u => u.Username.Equals(userForLoginDto.Username, StringComparison.OrdinalIgnoreCase));

        if (user == null || !VerifyPasswordHash(userForLoginDto.Password, user.PasswordHash, user.PasswordSalt))
        {
            return null; // Invalid credentials
        }

        return GenerateJwtToken(user);
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var secretKey = _configuration["JwtSettings:SecretKey"] ?? throw new InvalidOperationException("JWT Secret Key not configured.");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(1), // Token expiry
            Issuer = _configuration["JwtSettings:Issuer"],
            Audience = _configuration["JwtSettings:Audience"],
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
