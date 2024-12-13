//using book_my_show.Data;
//using book_my_show.Model;
//using Microsoft.Data.SqlClient;
//using Microsoft.EntityFrameworkCore;
//using System.Data;

//namespace book_my_show.Repositories
//{
//    public class UserRepository :IUserRepository
//    {
//        private readonly AppDbContext _context;
//        private readonly EncryptionService _encryptionService;

//        public UserRepository(AppDbContext context, EncryptionService encryptionService)
//        {
//            _context = context;
//            _encryptionService = encryptionService;
//        }

//        public async Task<User> GetUserByUsername(string username)
//        {
//            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
//        }

//        public async Task<bool> UserExists(string username)
//        {
//            return await _context.Users.AnyAsync(u => u.Username == username);
//        }

//        public async Task RegisterUser(User user)
//        {
//            //user.Password = _encryptionService.Encrypt(user.Password);
//            //await _context.Users.AddAsync(user);
//            //await _context.SaveChangesAsync(); // old method
//            user.Password = _encryptionService.Encrypt(user.Password);

//            var usernameParam = new SqlParameter("@Username", user.Username);
//            var passwordParam = new SqlParameter("@Password", user.Password);
//            var locationParam = new SqlParameter("@Location", user.Location);

//            await _context.Database.ExecuteSqlRawAsync("EXEC dbo.RegisterUser @Username, @Password, @Location", usernameParam, passwordParam, locationParam); // method with SP
//        }

//        public async Task UpdateUserLocation(User user)
//        {
//            //var existingUser = await GetUserByUsername(user.Username);
//            //if (existingUser != null && !string.IsNullOrEmpty(user.Location))
//            //{
//            //    existingUser.Location = user.Location;
//            //    await _context.SaveChangesAsync();
//            //} //old code
//            if (!string.IsNullOrEmpty(user.Location))
//            {
//                var usernameParam = new SqlParameter("@Username", user.Username);
//                var locationParam = new SqlParameter("@Location", user.Location);

//                await _context.Database.ExecuteSqlRawAsync("EXEC UpdateUserLocation @Username, @Location", usernameParam, locationParam);
//            }   //method with SP

//        }

//        public async Task<string> GetUserLocation(string username)
//        {
//            //var user = await GetUserByUsername(username);
//            //return user?.Location;
//            string location = null;

//            using (var command = _context.Database.GetDbConnection().CreateCommand())
//            {
//                command.CommandText = "GetUserLocation"; 
//                command.CommandType = CommandType.StoredProcedure;    

//                var parameter = new SqlParameter("@Username", username);
//                command.Parameters.Add(parameter);

//                await _context.Database.OpenConnectionAsync();

//                using (var result = await command.ExecuteReaderAsync())
//                {
//                    if (await result.ReadAsync())
//                    {
//                        location = result.GetString(0); 
//                    }
//                }
//            }

//            return location;
//        }



//        //public async Task<List<User>> GetUsersAsync()
//        //{
//        //    return await _context.GetUsersAsync();
//        //}  // new one added
//    }
//}   //code with DbContext


// Repositories/UserRepository.cs
using System.Data;
using System.Data.SqlClient;
using Dapper;
using book_my_show.Model;
using System.Threading.Tasks;

namespace book_my_show.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private readonly EncryptionService _encryptionService;

        public UserRepository(IConfiguration configuration, EncryptionService encryptionService)
        {
            
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _encryptionService = encryptionService;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryFirstOrDefaultAsync<User>(
                    "SELECT * FROM Users WHERE Username = @Username",
                    new { Username = username }
                );
            }
        }

        public async Task<bool> UserExists(string username)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.ExecuteScalarAsync<bool>(
                    "SELECT COUNT(1) FROM Users WHERE Username = @Username",
                    new { Username = username }
                );
            }
        }

        public async Task RegisterUser(User user)
        {
            user.Password = _encryptionService.Encrypt(user.Password);
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    Username = user.Username,
                    Password = user.Password,
                    Location = user.Location
                };
                await db.ExecuteAsync("EXEC dbo.RegisterUser @Username, @Password, @Location", parameters);
            }
        }

        public async Task UpdateUserLocation(User user)
        {
            if (!string.IsNullOrEmpty(user.Location))
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var parameters = new
                    {
                        Username = user.Username,
                        Location = user.Location
                    };
                    await db.ExecuteAsync("EXEC UpdateUserLocation @Username, @Location", parameters);
                }
            }
        }

        public async Task<string> GetUserLocation(string username)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new { Username = username };
                return await db.QueryFirstOrDefaultAsync<string>(
                    "GetUserLocation",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }
    }
}  // code with dapper

