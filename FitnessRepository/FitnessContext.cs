using FitnessRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessRepository;

public class FitnessContext: DbContext
{
    public FitnessContext(DbContextOptions<FitnessContext> options) : base(options)
    {
        
    }
    
    public DbSet<UserSupplementActivity> UserSupplementActivity { get; set; }
    public DbSet<UserSupplement> UserSupplements { get; set; }
    public DbSet<Supplements> Supplements { get; set; }
    public DbSet<Users> Users { get; set; }
    public DbSet<UserBody> UserBody { get; set; }
    public DbSet<UserBloodPressure> UserBloodPressure { get; set; }
    public DbSet<UserWeight> UserWeight { get; set; }
}