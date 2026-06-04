
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
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
                var existingUser = await _context.AccountUsers.FirstOrDefaultAsync(x => x.Email == dto.Email);

                if (existingUser == null)
                {
                    return new Tuple<int, string>(0, "This User Not Exist, Please Login");
                }
                if (existingUser.Password != dto.Password)
                {
                    return new Tuple<int, string>(1, "Password Is Incorrect");
                }
                return new Tuple<int, string>(2, "Login Successful");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}