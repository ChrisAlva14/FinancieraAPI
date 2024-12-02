using FinancieraAPI.DTOs;

namespace FinancieraAPI.Services
{
    public interface IUserServices
    {
        Task<int> PostUser(UserRequest user);

        Task<List<UserResponse>> GetUsers();

        Task<UserResponse> GetUser(int userId);

        Task<int> PutUser(int UserId, UserRequest user);

        Task<int> DeleteUser(int userId);

        Task<UserResponse> Login(UserRequest user);
    }
}
