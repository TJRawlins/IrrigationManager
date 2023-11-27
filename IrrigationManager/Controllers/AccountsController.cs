using IrrigationManager.Data;
using IrrigationManager.DTOs;
using IrrigationManager.Interfaces;
using IrrigationManager.Models;
using IrrigationManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace IrrigationManager.Controllers {
    public class AccountsController : BaseApiController {

        // Include the token service
        private readonly IMSContext _context;
        private readonly ITokenService _tokenService;
        public AccountsController(IMSContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // POST: api/accounts/register
        [HttpPost("register")]
        // string fname, string lname, string email, string username, string password
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto) {

            if (await UserExists(registerDto.Username!)) return BadRequest("Username is taken");

            // Create Salt - "using" allows to dispose class after being used
            using var hmac = new HMACSHA512();

            var user = new User {
                Firstname = registerDto.Firstname!,
                Lastname = registerDto.Lastname!,
                Email = registerDto.Email!,
                Username = registerDto.Username!.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password!)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return new UserDto {
                Username = user.Username,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto) {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == loginDto.Username);

            if (user == null) return Unauthorized("Invalid Username");

            // Create the same hash byte array
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password!));

            // Compare the entered pw byte array with the one in the database byte for byte
            for(int i = 0; i < computedHash.Length; i++) {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }
            return new UserDto {
                Username = user.Username,
                Token = _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists( string username) {
            return await _context.Users.AnyAsync(x => x.Username == username.ToLower());
        }

    }
}
