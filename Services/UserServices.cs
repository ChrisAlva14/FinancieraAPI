using AutoMapper;
using FinancieraAPI.DTOs;
using FinancieraAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancieraAPI.Services
{
    public class UserServices : IUserServices
    {
        private readonly FinancieraDbContext _context;
        private readonly IMapper _IMapper;

        public UserServices(FinancieraDbContext context, IMapper iMapper)
        {
            _context = context;
            _IMapper = iMapper;
        }

        public async Task<int> DeleteUser(int userId)
        {
            var user = await _context.Usuarios.FindAsync(userId);

            if (user == null)
            {
                return -1;
            }

            _context.Usuarios.Remove(user);

            return await _context.SaveChangesAsync();
        }

        public async Task<UserResponse> GetUser(int userId)
        {
            var user = await _context.Usuarios.FindAsync(userId);

            var userResponse = _IMapper.Map<Usuario, UserResponse>(user);

            return userResponse;
        }

        public async Task<List<UserResponse>> GetUsers()
        {
            var users = await _context.Usuarios.ToListAsync();

            var usersList = _IMapper.Map<List<Usuario>, List<UserResponse>>(users);

            return usersList;
        }

        public async Task<UserResponse> Login(UserRequest user)
        {
            var userEntity = await _context.Usuarios.FirstOrDefaultAsync(o =>
                o.Username == user.Username && o.UserPassword == user.UserPassword
            );

            var userResponse = _IMapper.Map<Usuario, UserResponse>(userEntity);

            return userResponse;
        }

        public async Task<int> PostUser(UserRequest user)
        {
            var userRequest = _IMapper.Map<UserRequest, Usuario>(user);

            await _context.Usuarios.AddAsync(userRequest);

            await _context.SaveChangesAsync();

            return userRequest.UserId;
        }

        public async Task<int> PutUser(int UserId, UserRequest user)
        {
            var entitiy = await _context.Usuarios.FindAsync(UserId);

            if (entitiy == null)
            {
                return -1;
            }

            entitiy.Username = user.Username;
            entitiy.UserPassword = user.UserPassword;
            entitiy.UserRole = user.UserRole;

            _context.Usuarios.Update(entitiy);

            return await _context.SaveChangesAsync();
        }
    }
}
