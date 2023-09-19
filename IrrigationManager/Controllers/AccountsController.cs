﻿using IrrigationManager.Data;
using IrrigationManager.DTOs;
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
        // string fname, string lname, string email, string username, string password
        public async Task<ActionResult<User>> Register(RegisterDto registerDto) {

            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            // Create Salt - "using" allows to dispose class after being used
            using var hmac = new HMACSHA512();

            var user = new User {
                Firstname = registerDto.Firstname,
                Lastname = registerDto.Lastname,
                Email = registerDto.Email,
                Username = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return user;
        }

        private async Task<bool> UserExists( string username) {
            return await _context.Users.AnyAsync(x => x.Username == username.ToLower());
        }

    }
}
