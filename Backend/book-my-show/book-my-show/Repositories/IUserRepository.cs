using book_my_show.Model;

namespace book_my_show.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsername(string username);
        Task<bool> UserExists(string username);
        Task RegisterUser(User user);
        Task UpdateUserLocation(User user);
        Task<string> GetUserLocation(string username);

        //Task<List<User>> GetUsersAsync(); // new one added























    }
}
