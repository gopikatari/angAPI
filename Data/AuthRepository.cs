using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            this._context = context;

        }
        public async Task<bool> IsExists(string username)
        {
            if (await _context.Users.AnyAsync(x => x.UserName == username))
            {
                return true;
            }
            return false;
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
            if (user == null)
            {
                return null;
            }
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var ComputeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < ComputeHash.Length; i++)
                {
                    if (ComputeHash[i] != passwordHash[i]) return false;
                }
            }
            return true;

        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordhash, passwordsalt;
            CreatePasswordHash(password, out passwordhash, out passwordsalt);
            user.PasswordHash = passwordhash;
            user.PasswordSalt = passwordsalt;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordhash, out byte[] passwordsalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordsalt = hmac.Key;
                passwordhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}