using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
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
    public DbSet<Uzivatel> Uzivatele { get; set; }
    public DbSet<Vozidlo> Vozidla { get; set; }
    public DbSet<Zastavka> Zastavky { get; set; }
    public DbSet<ZaznamTrasy> ZaznamyTras { get; set; }
    public DbSet<Znacka> Znacky { get; set; }

    public async Task CreateVozidlo(Vozidlo vozidlo)
    {
        using var command = Database.GetDbConnection().CreateCommand();
        command.CommandText = "BEGIN CreateVozidlo(:rokVyroby, :najeteKilometry, :kapacita, :maKlimatizaci, :idGaraz, :idModel); END;";
        command.CommandType = CommandType.Text;

        command.Parameters.Add(new OracleParameter("rokVyroby", vozidlo.RokVyroby));
        command.Parameters.Add(new OracleParameter("najeteKilometry", vozidlo.NajeteKilometry));
        command.Parameters.Add(new OracleParameter("kapacita", vozidlo.Kapacita));
        command.Parameters.Add(new OracleParameter("maKlimatizaci", vozidlo.MaKlimatizaci ? 1 : 0));
        command.Parameters.Add(new OracleParameter("idGaraz", vozidlo.IdGaraz));
        command.Parameters.Add(new OracleParameter("idModel", vozidlo.IdModel));
        await Database.OpenConnectionAsync();
        await command.ExecuteNonQueryAsync();
        await Database.CloseConnectionAsync();
    }

    public async Task CreateZastavka(Zastavka zastavka)
    {
        using var command = Database.GetDbConnection().CreateCommand();
        command.CommandText = "BEGIN CreateZastavka(:nazev, :souradniceX, :souradniceY, :idPasmo); END;";
        command.CommandType = CommandType.Text;

        command.Parameters.Add(new OracleParameter("nazev", zastavka.Nazev));
        command.Parameters.Add(new OracleParameter("souradniceX", zastavka.SouradniceX));
        command.Parameters.Add(new OracleParameter("souradniceY", zastavka.SouradniceY));
        command.Parameters.Add(new OracleParameter("idPasmo", zastavka.IdPasmo));
        await Database.OpenConnectionAsync();
        await command.ExecuteNonQueryAsync();
        await Database.CloseConnectionAsync();
    }

    public async Task<Vozidlo?> GetVozidloById(int id)
    {
        string sql = @"DECLARE
                     v_vozidlo_json CLOB;
                     BEGIN
                     v_vozidlo_json := GetVozidloById(:p_id_vozidlo);
                     :p_result := v_vozidlo_json;
                     END;";
        OracleParameter[] sqlParams = [new OracleParameter("p_id_vozidlo", OracleDbType.Int32, id, ParameterDirection.Input)];
        return await GetObjectFromDB<Vozidlo>(sql, sqlParams);
    }

    public async Task<Zastavka?> GetZastavkaById(int id)
    {
        string sql = @"DECLARE
                     v_zastavka_json CLOB;
                     BEGIN
                     v_zastavka_json := GetZastavkaById(:p_id_zastavka);
                     :p_result := v_zastavka_json;
                     END;";
        OracleParameter[] sqlParams = [new OracleParameter("p_id_zastavka", OracleDbType.Int32, id, ParameterDirection.Input)];
        return await GetObjectFromDB<Zastavka>(sql, sqlParams);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("ST69612");
        modelBuilder.Entity<Udrzba>()
            .HasDiscriminator<char>("TYP_UDRZBY")
            .HasValue<Cisteni>('c')
            .HasValue<Oprava>('o')
            .HasValue<Udrzba>('x');
    }

    private async Task<T?> GetObjectFromDB<T>(string sql, OracleParameter[] sqlParams) where T : class
    {
        var connection = Database.GetDbConnection();
        var resultParam = new OracleParameter("p_result", OracleDbType.Clob, ParameterDirection.Output);
        string resultJson = string.Empty;

        using (var command = connection.CreateCommand())
        {
            command.CommandText = sql;
            command.Parameters.AddRange(sqlParams);
            command.Parameters.Add(resultParam);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            if (resultParam.Value != DBNull.Value)
                resultJson = ((OracleClob)resultParam.Value).Value;
            await connection.CloseAsync();
        }
        return JsonConvert.DeserializeObject<T>(resultJson);
    }
}

// PRÁCE S KURZOREM
//public async Task<Vozidlo?> GetVozidloById(int idVozidlo)
//{
//    Vozidlo? vozidlo = null;

//    using (var command = Database.GetDbConnection().CreateCommand())
//    {
//        command.CommandText = "BEGIN :result := GetVozidloById(:idVozidlo); END;";
//        command.CommandType = CommandType.Text;

//        // Output parameter to receive the returned row
//        var resultParam = new OracleParameter("result", OracleDbType.Object, ParameterDirection.ReturnValue);
//        command.Parameters.Add(resultParam);

//        // Input parameter for the ID
//        command.Parameters.Add(new OracleParameter("idVozidlo", idVozidlo));

//        await Database.OpenConnectionAsync();
//        await command.ExecuteNonQueryAsync();
//        resultParam.Value.ToString();
//        //using (var reader = await command.ExecuteReaderAsync())
//        //    if (await reader.ReadAsync())
//        //        vozidlo = new Vozidlo
//        //        {
//        //            IdVozidlo = reader.GetInt32(reader.GetOrdinal("ID_VOZIDLO")),
//        //            RokVyroby = reader.GetInt16(reader.GetOrdinal("ROK_VYROBY")),
//        //            NajeteKilometry = reader.GetInt32(reader.GetOrdinal("NAJETE_KILOMETRY")),
//        //            Kapacita = reader.GetInt32(reader.GetOrdinal("KAPACITA")),
//        //            MaKlimatizaci = reader.GetBoolean(reader.GetOrdinal("MA_KLIMATIZACI")),
//        //            IdGaraz = reader.GetInt32(reader.GetOrdinal("ID_GARAZ")),
//        //            IdModel = reader.GetInt32(reader.GetOrdinal("ID_MODEL"))
//        //        };

//        await Database.CloseConnectionAsync();
//    }
//    return vozidlo;
//}