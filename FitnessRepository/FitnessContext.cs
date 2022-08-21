using Microsoft.EntityFrameworkCore;

namespace FitnessRepository;

public class FitnessContext: DbContext
{
    public FitnessContext(DbContextOptions<FitnessContext> options) : base(options)
    {
        
    }
    
    public DbSet<Supplements> Supplements { get; set; }
}