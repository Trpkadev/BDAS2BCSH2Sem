using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;

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
    public DbSet<TarifniPasmo> TarifniPasma { get; set; }
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

    public async Task<Vozidlo?> GetVozidloById(int idVozidlo)
    {
        Vozidlo? vozidlo = null;

        using (var command = Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = "BEGIN :result := GetVozidloById(:idVozidlo); END;";
            command.CommandType = CommandType.Text;

            // Output parameter to receive the returned row
            var resultParam = new OracleParameter("result", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            command.Parameters.Add(resultParam);

            // Input parameter for the ID
            command.Parameters.Add(new OracleParameter("idVozidlo", idVozidlo));

            await Database.OpenConnectionAsync();

            using (var reader = await command.ExecuteReaderAsync())
                if (await reader.ReadAsync())
                    vozidlo = new Vozidlo
                    {
                        IdVozidlo = reader.GetInt32(reader.GetOrdinal("ID_VOZIDLO")),
                        RokVyroby = reader.GetInt16(reader.GetOrdinal("ROK_VYROBY")),
                        NajeteKilometry = reader.GetInt32(reader.GetOrdinal("NAJETE_KILOMETRY")),
                        Kapacita = reader.GetInt32(reader.GetOrdinal("KAPACITA")),
                        MaKlimatizaci = reader.GetBoolean(reader.GetOrdinal("MA_KLIMATIZACI")),
                        IdGaraz = reader.GetInt32(reader.GetOrdinal("ID_GARAZ")),
                        IdModel = reader.GetInt32(reader.GetOrdinal("ID_MODEL"))
                    };

            await Database.CloseConnectionAsync();
        }
        return vozidlo;
    }

    public async Task<Zastavka?> GetZastavkaById(int idZastavka)
    {
        Zastavka? zastavka = null;

        using (var command = Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = "BEGIN :result := GetZastavkaById(:idZastavka); END;";
            command.CommandType = CommandType.Text;

            // Output parameter to receive the returned row
            var resultParam = new OracleParameter("result", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            command.Parameters.Add(resultParam);

            // Input parameter for the ID
            command.Parameters.Add(new OracleParameter("idZastavka", idZastavka));

            await Database.OpenConnectionAsync();

            using (var reader = await command.ExecuteReaderAsync())
                if (await reader.ReadAsync())
                    zastavka = new Zastavka
                    {
                        IdZastavka = reader.GetInt32(reader.GetOrdinal("ID_ZASTAVKA")),
                        Nazev = reader.GetString(reader.GetOrdinal("NAZEV")),
                        SouradniceX = reader.GetDouble(reader.GetOrdinal("SOURADNICE_X")),
                        SouradniceY = reader.GetDouble(reader.GetOrdinal("SOURADNICE_Y")),
                        IdPasmo = reader.GetInt32(reader.GetOrdinal("ID_PASMO"))
                    };

            await Database.CloseConnectionAsync();
        }
        return zastavka;
    }
}