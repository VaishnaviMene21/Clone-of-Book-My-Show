using book_my_show.Model;
using Microsoft.EntityFrameworkCore;

namespace book_my_show.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {

        }
       
        public DbSet<User> Users { get; set; }
        public DbSet<PaymentDetail> PaymentDetails { get; set; }

        public DbSet<Movie> Movies { get; set; }

        //public async Task<List<User>> GetUsersAsync()
        //{
        //    return await Users.FromSqlRaw("EXEC GetUsers").ToListAsync();
        //}  // new added



    }
}
