
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
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

        public async Task<Tuple<int, string>> LoginUser(UserDto dto)
        {
            try
            {
                if (dto==null)
                {
                    return new Tuple<int, string>(1, "Please Fill All The Details");
                }

                var existingUser = await _context.AccountUsers.FirstOrDefaultAsync(x => x.Email == dto.Email);

                if (existingUser == null)
                {
                    return new Tuple<int, string>(0, "This User Not Exist, Please Login");
                }
                //if (existingUser.Password != dto.Password)
                //{
                //    return new Tuple<int, string>(1, "Password Is Incorrect");
                //}

                var PasswordHasher = new PasswordHasher<string>();
                var verifyPassword = PasswordHasher.VerifyHashedPassword(dto.Email, existingUser.Password, dto.Password);

                if ( verifyPassword == PasswordVerificationResult.Success)
                {
                    return new Tuple<int, string>(2, "Login Successful");
                }

                else if(verifyPassword == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    existingUser.Password = PasswordHashing(dto);
                    _context.AccountUsers.Update(existingUser);
                    _context.SaveChanges();
                    return new Tuple<int, string>(2, "Login Successful, New Hash Generated");
                }

                else if(verifyPassword == PasswordVerificationResult.Failed)
                {
                    return new Tuple<int, string>(1, "Password Is Incorrect");
                }
                return new Tuple<int, string>(1, "");
            }
            catch (Exception)
            {
                throw;
            }
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