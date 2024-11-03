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

    public async Task CreateUzivatelAsync(Uzivatel uzivatel)
    {
        string sql = "BEGIN CreateUzivatel(:jmeno, :heslo); END;";
        OracleParameter[] sqlParams = [ new OracleParameter("jmeno", uzivatel.Jmeno),
                                        new OracleParameter("heslo", uzivatel.Heslo)];
        await CreateInDb(sql, sqlParams);
    }

    public async Task CreateVozidloAsync(Vozidlo vozidlo)
    {
        string sql = "BEGIN CreateVozidlo(:rokVyroby, :najeteKilometry, :kapacita, :maKlimatizaci, :idGaraz, :idModel); END;";
        OracleParameter[] sqlParams = [ new OracleParameter("rokVyroby", vozidlo.RokVyroby),
                                        new OracleParameter("najeteKilometry", vozidlo.NajeteKilometry),
                                        new OracleParameter("kapacita", vozidlo.Kapacita),
                                        new OracleParameter("maKlimatizaci", vozidlo.MaKlimatizaci ? 1 : 0),
                                        new OracleParameter("idGaraz", vozidlo.IdGaraz),
                                        new OracleParameter("idModel", vozidlo.IdModel)];
        await CreateInDb(sql, sqlParams);
    }

    public async Task CreateZastavkaAsync(Zastavka zastavka)
    {
        string sql = "BEGIN CreateZastavka(:nazev, :souradniceX, :souradniceY, :idPasmo); END;";
        OracleParameter[] sqlParams = [ new OracleParameter("nazev", zastavka.Nazev),
                                        new OracleParameter("souradniceX", zastavka.SouradniceX),
                                        new OracleParameter("souradniceY", zastavka.SouradniceY),
                                        new OracleParameter("idPasmo", zastavka.IdPasmo)];
        await CreateInDb(sql, sqlParams);
    }

    public async Task<Uzivatel?> GetUzivatelByIdAsync(int id)
    {
        string sql = @"DECLARE
                     v_uzivatel_json CLOB;
                     BEGIN
                     v_uzivatel_json := GetUzivatelById(:p_id_uzivatel);
                     :p_result := v_uzivatel_json;
                     END;";
        OracleParameter[] sqlParams = [new OracleParameter("p_id_uzivatel", OracleDbType.Int32, id, ParameterDirection.Input)];
        return await GetObjectFromDB<Uzivatel>(sql, sqlParams);
    }

    public async Task<Uzivatel?> GetUzivatelByNamePwdAsync(string name, string pwdHash)
    {
        string sql = @"DECLARE
                     v_uzivatel_json CLOB;
                     BEGIN
                     v_uzivatel_json := GetUzivatelByJmenoHash(:p_jmeno_uzivatel,:p_hash_uzivatel);
                     :p_result := v_uzivatel_json;
                     END;";
        OracleParameter[] sqlParams = [ new OracleParameter("p_jmeno_uzivatel", OracleDbType.Varchar2, name, ParameterDirection.Input),
                                        new OracleParameter("p_hash_uzivatel", OracleDbType.Varchar2, pwdHash, ParameterDirection.Input)];
        return await GetObjectFromDB<Uzivatel>(sql, sqlParams);
    }

    public async Task<List<Uzivatel?>?> GetUzivateleAsync()
    {
        string sql = @"DECLARE
                     v_uzivatele_json CLOB;
                     BEGIN
                     v_uzivatele_json := GetUzivatele();
                     :p_result := v_uzivatele_json;
                     END;";
        OracleParameter[] sqlParams = [];

        return await GetObjectFromDB<List<Uzivatel?>>(sql, sqlParams);
    }

    public async Task<List<Vozidlo?>?> GetVozidlaAsync()
    {
        string sql = @"DECLARE
                     v_vozidla_json CLOB;
                     BEGIN
                     v_vozidla_json := GetVozidla();
                     :p_result := v_vozidla_json;
                     END;";
        OracleParameter[] sqlParams = [];
        return await GetObjectFromDB<List<Vozidlo?>>(sql, sqlParams);
    }

    public async Task<Vozidlo?> GetVozidloByIdAsync(int id)
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

    public async Task<Zastavka?> GetZastavkaByIdAsync(int id)
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

    public async Task<List<Zastavka?>?> GetZastavkyAsync()
    {
        string sql = @"DECLARE
                     v_zastavky_json CLOB;
                     BEGIN
                     v_zastavky_json := GetZastavky();
                     :p_result := v_zastavky_json;
                     END;";
        OracleParameter[] sqlParams = [];
        return await GetObjectFromDB<List<Zastavka?>>(sql, sqlParams);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
#if DEBUG
        modelBuilder.HasDefaultSchema("ST69642");
#elif RELEASE
        modelBuilder.HasDefaultSchema("ST69612");
#endif
        modelBuilder.Entity<Udrzba>()
            .HasDiscriminator<char>("TYP_UDRZBY")
            .HasValue<Cisteni>('c')
            .HasValue<Oprava>('o')
            .HasValue<Udrzba>('x');
    }

    private async Task CreateInDb(string sql, OracleParameter[] sqlParams)
    {
        using var command = Database.GetDbConnection().CreateCommand();
        command.CommandText = sql;
        command.CommandType = CommandType.Text;
        command.Parameters.AddRange(sqlParams);
        await Database.OpenConnectionAsync();
        await command.ExecuteNonQueryAsync();
        await Database.CloseConnectionAsync();
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