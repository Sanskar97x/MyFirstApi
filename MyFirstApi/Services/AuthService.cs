
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using MyFirstApi.Data;
using MyFirstApi.Dto;
using MyFirstApi.IService;

namespace MyFirstApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }


        private string GetJwtToken(UserDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new InvalidOperationException("User name cannot be null or empty");

            var claims = new[]
            {
        new Claim(ClaimTypes.Name, dto.Name),
        new Claim(ClaimTypes.NameIdentifier, dto.Id.ToString())
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("yjhL7EPo4cdohugQWG1PR7kMD1mPy0DUJBMUq2YkK2W")
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = "rohan-client",
                Audience = "rohan-backend",
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = creds
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }

        public async Task<Tuple<int, string>>RegisterUser(UserDto dto)
        {
            try
            {
                var existingUser = await _context.AccountUsers.AnyAsync(x => x.Email == dto.Email);

                if (existingUser)
                {
                    return new Tuple<int, string>(0, "This User is already exist, please register");
                }

                _context.AccountUsers.Add(new Entities.User
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Email = dto.Email,
                    Password = PasswordHashing(dto),
                });

                await _context.SaveChangesAsync();


                return new Tuple<int, string>(1, "User Registered Successfully");
            }
            catch
            {
                throw;
            }
         }

        private string PasswordHashing(UserDto dto)
        {
            var PasswordHasher = new PasswordHasher<string>();

            var hash = PasswordHasher.HashPassword(dto.Email, dto.Password);
            return hash;
        }

    }
}