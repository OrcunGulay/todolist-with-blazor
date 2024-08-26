using LoginRegisterPage.Data;
using Microsoft.EntityFrameworkCore;

namespace LoginRegisterPage.Services
{
    public class AuthenticationService
    {
        private readonly AppDbContext _context;

        public AuthenticationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Register(string name, string email, string password)
        {
            if (_context.Users.Any(u => u.Email == email)) return false;

            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = HashPassword(password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null || !VerifyPassword(password, user.PasswordHash)) return false;

            return true;
        }

        private string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return hashedPassword == Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
