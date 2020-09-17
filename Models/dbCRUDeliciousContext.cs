using Microsoft.EntityFrameworkCore;            //Need this for inheriting : DbContext

namespace CSCRUDelicious.Models
{
    public class dbCRUDeliciousContext : DbContext
    {
        public dbCRUDeliciousContext(DbContextOptions options) : base(options) { }
        //creating constructor accessing DbContext (base) constructor, passing param options into base method
        //options is passed in from the StartUp.cs file - Dependency Injection
        public DbSet<Dish> Dishes { get; set; }           // !!! DbSet name must match MySQL db table name; one DbSet per table
    }
}