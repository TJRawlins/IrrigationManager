using IrrigationManager.Interfaces;
using IrrigationManager.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IrrigationManager.Services {
    public class TokenService : ITokenService {

        // used to encrypt and decrypt key on server
        private readonly SymmetricSecurityKey _key;


        // store secret key into configuration
        public TokenService(IConfiguration config)
        {
            // encode instance of key into bytes array
            // get TokenKey from appsettings.json
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]!));
        }

        public string CreateToken(User user) {

            // claimed information list about a particular user
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.NameId, user.Username)
            };

            // token which signs that user is who they claim to be - encrypt key
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // descrip tokey to return
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds
            };

            // create token handler instance then create token by passing in descriptor
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
