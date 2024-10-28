using Microsoft.EntityFrameworkCore;

namespace BCSH2BDAS2.Models;

public class TransportationContext(DbContextOptions<TransportationContext> options) : DbContext(options)
{
    public DbSet<Cisteni> Cisteni { get; set; }
    public DbSet<Garaz> Garaze { get; set; }
    public DbSet<JizniRad> JizdniRady { get; set; }
    public DbSet<Linka> Linky { get; set; }
    public DbSet<Model> Modely { get; set; }
    public DbSet<Oprava> Opravy { get; set; }
    public DbSet<Spoj> Spoje { get; set; }
    public DbSet<TarifniZona> TarifniZony { get; set; }
    public DbSet<TypVozidla> TypyVozidel { get; set; }
    public DbSet<Udrzba> Udrzby { get; set; }
    public DbSet<Vozidlo> Vozidla { get; set; }
    public DbSet<Zastavka> Zastavky { get; set; }
    public DbSet<ZaznamTrasy> ZaznamyTras { get; set; }
    public DbSet<Znacka> Znacky { get; set; }
    public DbSet<Uzivatel> Uzivatele { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("ST69612");
        modelBuilder.Entity<Udrzba>()
            .HasDiscriminator<char>("TYP_UDRZBY")
            .HasValue<Cisteni>('c')
            .HasValue<Oprava>('o')
            .HasValue<Udrzba>('x');
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine);
    }
}