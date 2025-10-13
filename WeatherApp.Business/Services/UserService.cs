using WeatherApp.Business.DTOs;
using WeatherApp.Business.Interfaces;
using WeatherApp.Data.Repositories;
using WeatherApp.Models;

namespace Business.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepo;

        public UserService(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepo.GetAllAsync();
            return users.Select(u => new UserDTO
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email
            }).ToList();
        }

        public async Task AddUserAsync(UserDTO user)
        {
            var entity = new User
            {
                Username = user.Username,
                Email = user.Email
            };
            await _userRepo.AddAsync(entity);
        }
    }
}
