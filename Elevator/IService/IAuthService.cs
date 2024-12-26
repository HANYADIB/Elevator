using Elevator.Models.auth;

namespace Elevator.IService
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> GetTokenAsync(LoginRequestModel model);
    }
}
