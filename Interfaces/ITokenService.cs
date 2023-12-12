using IrrigationManager.Models;

namespace IrrigationManager.Interfaces {
    public interface ITokenService {
        /* 
         * CONTRACT BETWEEN INTERFACE & IMPLEMENTATION
         * Any class that implements this interface:
         * - Has to support this method
         * - Has to return a string from this method
         * - Has to take User as an argument
        */ 
        string CreateToken(User user);
    }
}
