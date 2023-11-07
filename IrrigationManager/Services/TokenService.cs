using IrrigationManager.Interfaces;
using IrrigationManager.Models;
using Microsoft.IdentityModel.Tokens;

namespace IrrigationManager.Services {
    public class TokenService : ITokenService {


        private readonly SymmetricSecurityKey _key;


        // store secret key into configuration
        public TokenService(IConfiguration config)
        {
            
        }

        public string CreateToken(User user) {
            throw new NotImplementedException();
        }
    }
}
