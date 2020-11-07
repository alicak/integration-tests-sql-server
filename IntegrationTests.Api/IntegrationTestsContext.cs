using IntegrationTests.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Api
{
    public class IntegrationTestsContext : DbContext
    {
    	public IntegrationTestsContext(DbContextOptions<IntegrationTestsContext> options) : base(options) { }
    	
        public DbSet<Book> Books { get; set; }
    }
}
