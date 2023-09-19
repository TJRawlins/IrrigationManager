using IrrigationManager.Data;
using IrrigationManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace IrrigationManager.Controllers {
    public class AccountsController : BaseApiController {

        private readonly IMSContext _context;
        public AccountsController(IMSContext context)
        {
            _context = context;
        }

        // POST: api/accounts/register
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(string username, string password) {
            // Create Salt - "using" allows to dispose class after being used
            using var hmac = new HMACSHA512();

            var user = new User {
                Username = username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }


    }
}
