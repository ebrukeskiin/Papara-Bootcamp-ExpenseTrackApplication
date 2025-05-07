using ExpenseTrack.Api.Configurations;
using ExpenseTrack.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrack.Api;

public class MsSqlDbContext : DbContext
{
    public MsSqlDbContext(DbContextOptions<MsSqlDbContext> options) : base(options)
    {

    }

    // DbSet'ler
    public DbSet<User> Users { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
    public DbSet<ExpenseDocument> ExpenseDocuments { get; set; }
    public DbSet<PaymentTransaction> PaymentTransactions { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // load all configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MsSqlDbContext).Assembly);

        //modelBuilder.Entity<User>().HasData(
        //new User
        //{
        //    Id = 1,
        //    FirstName = "Admin",
        //    LastName = "Kullanıcı",
        //    Email = "admin@firma.com",
        //    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
        //    Role = UserRole.Admin,
        //    IBAN = "TR000000000000000000000001",
        //    InsertedDate = new DateTime(2024, 1, 1),
        //    InsertedUser = "system",
        //    IsActive = true
        //},
        //new User
        //{
        //    Id = 2,
        //    FirstName = "Personel",
        //    LastName = "Kullanıcı",
        //    Email = "personel@firma.com",
        //    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Personel123!"),
        //    Role = UserRole.Personel,
        //    IBAN = "TR000000000000000000000002",
        //    InsertedDate = new DateTime(2024, 1, 1),
        //    InsertedUser = "system",
        //    IsActive = true
        //}
    //);

        base.OnModelCreating(modelBuilder);
    }

}