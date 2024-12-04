using BCSH2BDAS2.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System.Runtime.CompilerServices;

namespace BCSH2BDAS2.Helpers;

public class TransportationContext(DbContextOptions<TransportationContext> options) : DbContext(options)
{
    public DbSet<DatabazovyObjekt> DatabazoveObjekty { get; set; }
    public DbSet<Garaz> Garaze { get; set; }
    public DbSet<JizdniRad> JizdniRady { get; set; }
    public DbSet<Linka> Linky { get; set; }
    public DbSet<Log> Logy { get; set; }
    public DbSet<Model> Modely { get; set; }
    public DbSet<NakladyNaVozidlo> NakladyNaVozidla { get; set; }
    public DbSet<Pracovnik> Pracovnici { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<Schema> Schemata { get; set; }
    public DbSet<Spoj> Spoje { get; set; }
    public DbSet<TarifniPasmo> TarifniPasma { get; set; }
    public DbSet<TypVozidla> TypyVozidel { get; set; }
    public DbSet<Udrzba> Udrzby { get; set; }
    public DbSet<Uzivatel> Uzivatele { get; set; }
    public DbSet<Vozidlo> Vozidla { get; set; }
    public DbSet<Zastavka> Zastavky { get; set; }
    public DbSet<ZaznamTrasy> ZaznamyTras { get; set; }
    public DbSet<Znacka> Znacky { get; set; }

    #region Procedury

    public async Task AddPayAsync(double multiplier, int? minPay)
    {
        const string sql = @"   BEGIN
                                    NAVYSENI_PLATU(:p_multiplier, :p_min_pay);
                                END;";
        await using var command = Database.GetDbConnection().CreateCommand();
        command.CommandText = sql;
        command.CommandType = CommandType.Text;
        command.Parameters.Add(new OracleParameter("p_multiplier", multiplier));
        command.Parameters.Add(new OracleParameter("p_min_pay", minPay));
        await Database.OpenConnectionAsync();
        await command.ExecuteNonQueryAsync();
        await Database.CloseConnectionAsync();
    }

    public async Task ImportRecordsAsync(string csv, char oddelovac)
    {
        const string sql = @"   DECLARE
                                    v_csv CLOB;
                                BEGIN
                                    v_csv := :p_csv;
                                    CSV_DO_ZAZNAMU_TRASY(v_csv, :p_oddelovac);
                                END;";
        await using var command = Database.GetDbConnection().CreateCommand();
        command.CommandText = sql;
        command.CommandType = CommandType.Text;
        command.Parameters.Add(new OracleParameter("p_csv", csv));
        command.Parameters.Add(new OracleParameter("p_oddelovac", oddelovac));
        await Database.OpenConnectionAsync();
        await command.ExecuteNonQueryAsync();
        await Database.CloseConnectionAsync();
    }

    public async Task MakeOfExistingAsync(int id_spoj, TimeOnly od, TimeOnly _do, int interval)
    {
        const string sql = @"   BEGIN
                                    PLANOVANI_JR(:p_id_spoj, POM_FCE.CAS_NA_DATE(:p_od), POM_FCE.CAS_NA_DATE(:p_do), :p_interval);
                                END;";
        await using var command = Database.GetDbConnection().CreateCommand();
        command.CommandText = sql;
        command.CommandType = CommandType.Text;
        command.Parameters.Add(new OracleParameter("p_id_spoj", id_spoj));
        command.Parameters.Add(new OracleParameter("p_od", od.ToString("t")));
        command.Parameters.Add(new OracleParameter("p_do", _do.ToString("t")));
        command.Parameters.Add(new OracleParameter("p_interval", interval));
        await Database.OpenConnectionAsync();
        await command.ExecuteNonQueryAsync();
        await Database.CloseConnectionAsync();
    }

    #endregion Procedury

    #region Funkce

    public async Task<string> DijkstraAsync(int idZastavkaStart, int idZastavkaEnd, DateTime timeStart)
    {
        const string sql = @$"  DECLARE
                                    v_result_json CLOB;
                                BEGIN
                                    v_result_json := DIJKSTRA_CORE(:id_zastavka_start, :id_zastavka_end, :time_start);
                                    :p_result := v_result_json;
                                END;";
        var resultParam = new OracleParameter("p_result", OracleDbType.Clob, ParameterDirection.Output);
        OracleParameter[] sqlParams = [ new("id_zastavka_start", OracleDbType.Int32, idZastavkaStart, ParameterDirection.Input),
                                        new("id_zastavka_end", OracleDbType.Int32, idZastavkaEnd, ParameterDirection.Input),
                                        new("time_start", OracleDbType.Date, timeStart, ParameterDirection.Input)];
        string? resultJson = null;

        var connection = Database.GetDbConnection();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = sql;
            command.Parameters.AddRange(sqlParams);
            command.Parameters.Add(resultParam);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            if (resultParam.Value != DBNull.Value && !((OracleClob)resultParam.Value).IsNull)
                resultJson = ((OracleClob)resultParam.Value).Value;
            await connection.CloseAsync();
        }
        return resultJson ?? Resource.DB_RESPONSE_NO_DATA;
    }

    public async Task<string?> GetTabulkaDoCsvAsync(string nazev, char oddelovac)
    {
        const string sql = @"   DECLARE
                                    v_csv CLOB;
                                BEGIN
                                    SELECT TABULKA_DO_CSV(:p_nazev, :p_oddelovac) INTO v_csv
                                FROM DUAL;
                                    :p_result := v_csv;
                                END;";
        await using var command = Database.GetDbConnection().CreateCommand();
        command.CommandText = sql;
        command.CommandType = CommandType.Text;
        var resultParam = new OracleParameter("p_result", OracleDbType.Clob, ParameterDirection.Output);

        command.Parameters.Add(new OracleParameter("p_nazev", nazev));
        command.Parameters.Add(new OracleParameter("p_oddelovac", oddelovac));
        command.Parameters.Add(resultParam);
        await Database.OpenConnectionAsync();
        await command.ExecuteNonQueryAsync();
        var result = resultParam.Value;
        var str = ((OracleClob?)result)?.Value;
        await Database.CloseConnectionAsync();
        return str;
    }

    public async Task<string?> GetTabulkaDoJsonAsync(string nazev)
    {
        const string sql = """
                           DECLARE
                               v_json CLOB;
                           BEGIN
                               SELECT TABULKA_DO_JSON(:p_nazev) INTO v_json
                               FROM DUAL;
                               :p_result := v_json;
                           END;
                           """;
        await using var command = Database.GetDbConnection().CreateCommand();
        command.CommandText = sql;
        command.CommandType = CommandType.Text;
        var resultParam = new OracleParameter("p_result", OracleDbType.Clob, ParameterDirection.Output);

        command.Parameters.Add(new OracleParameter("p_nazev", nazev));
        command.Parameters.Add(resultParam);
        await Database.OpenConnectionAsync();
        await command.ExecuteNonQueryAsync();
        var result = resultParam.Value;
        var str = ((OracleClob?)result)?.Value;
        await Database.CloseConnectionAsync();
        return str;
    }

    #endregion Funkce

    #region DML procedures

    public async Task DMLGarazeAsync(Garaz garaz)
    {
        string sql = $"{ConvertDMLMethodName()}(:idGaraz, :nazev, :kapacita);";
        OracleParameter[] sqlParams = [ new("idGaraz", ConvertId(garaz.IdGaraz)),
                                        new("nazev", garaz.Nazev),
                                        new("kapacita", garaz.Kapacita)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLJizdni_RadyAsync(JizdniRad jizdniRad)
    {
        string sql = $"{ConvertDMLMethodName()}(:idSpoj, :idZastavka, :casPrijezdu, :casOdjezdu);";
        OracleParameter[] sqlParams = [ new("idSpoj", jizdniRad.IdSpoj),
                                        new("idZastavka", jizdniRad.IdZastavka),
                                        new("casPrijezdu", jizdniRad.CasPrijezdu),
                                        new("casOdjezdu", jizdniRad.CasOdjezdu)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLLinkyAsync(Linka linka)
    {
        string sql = $"{ConvertDMLMethodName()}(:idLinka, :nazev, :typVozidla, :cislo);";
        OracleParameter[] sqlParams = [ new("idLinka", ConvertId(linka.IdLinka)),
                                        new("nazev", linka.Nazev),
                                        new("typVozidla", linka.IdTypVozidla),
                                        new("cislo", linka.Cislo)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLModelyAsync(Model model)
    {
        string sql = $"{ConvertDMLMethodName()}(:idModel, :idTypVozidla, :idZnacka, :nazev, :jeNizkopodlazni);";
        OracleParameter[] sqlParams = [ new("idModel", ConvertId(model.IdModel)),
                                        new("idTypVozidla", model.IdTypVozidla),
                                        new("idZnacka", model.IdZnacka),
                                        new("nazev", model.Nazev),
                                        new("jeNizkopodlazni", model.JeNizkopodlazni)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLPracovniciAsync(Pracovnik pracovnik)
    {
        string sql = $"{ConvertDMLMethodName()}(:idPracovnik, :uzivatelskeJmeno, :heslo, :idNadrizeny, :idRole, :hodinovaMzda, :jmeno, :prijmeni, :telefonniCislo, :email, :rodneCislo, :idUzivatel);";
        OracleParameter[] sqlParams = [ new("idPracovnik", ConvertId(pracovnik.IdPracovnik)),
                                        new("idNadrizeny", pracovnik.IdNadrizeny),
                                        new("idRole", pracovnik.IdNadrizeny),
                                        new("hodinovaMzda", pracovnik.HodinovaMzda),
                                        new("jmeno", pracovnik.Jmeno),
                                        new("prijmeni", pracovnik.Prijmeni),
                                        new("telefonniCislo", pracovnik.TelefonniCislo),
                                        new("email", pracovnik.Email),
                                        new("rodneCislo", pracovnik.RodneCislo),
                                        new("idUzivatel", pracovnik.IdUzivatel)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLRoleAsync(Role role)
    {
        string sql = $"{ConvertDMLMethodName()}(:idRole, :nazev, :prava);";
        OracleParameter[] sqlParams = [ new("idRole", ConvertId(role.IdRole)),
                                        new("nazev", role.Nazev),
                                        new("prava", role.Prava)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLSchemataAsync(Schema schema)
    {
        string sql = $"{ConvertDMLMethodName()}(:idSchema, :nazevSchematu, :nazevSouboru, :typSouboru, :velikostSouboru, :datumZmeny, :soubor);";
        OracleParameter[] sqlParams = [ new("idSchema", ConvertId(schema.IdSchema)),
                                        new("nazevSchematu", schema.NazevSchematu),
                                        new("nazevSouboru", schema.NazevSouboru),
                                        new("typSouboru", schema.TypSouboru),
                                        new("velikostSouboru", schema.VelikostSouboru),
                                        new("datumZmeny", schema.DatumZmeny),
                                        new("soubor", schema.Soubor)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLSpojeAsync(Spoj spoj)
    {
        string sql = $"{ConvertDMLMethodName()}(:idSpoj, :idLinka, :garantovaneNizkopodlazni, :jedeVeVsedniDen, :jedeVSobotu, :jedeVNedeli);";
        OracleParameter[] sqlParams = [ new("idSpoj", ConvertId(spoj.IdSpoj)),
                                        new("idLinka", spoj.IdLinka),
                                        new("garantovaneNizkopodlazni", spoj.GarantovaneNizkopodlazni),
                                        new("jedeVeVsedniDen", spoj.JedeVeVsedniDen),
                                        new("jedeVSobotu", spoj.JedeVSobotu ? 1 : 0),
                                        new("jedeVNedeli", spoj.JedeVNedeli)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLTarifni_PasmaAsync(TarifniPasmo tarifniPasmo)
    {
        string sql = $"{ConvertDMLMethodName()}(:idPasmo, :nazev);";
        OracleParameter[] sqlParams = [ new("idPasmo", ConvertId(tarifniPasmo.IdPasmo)),
                                        new("nazev", tarifniPasmo.Nazev)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLTypy_VozidelAsync(TypVozidla typVozidla)
    {
        string sql = $"{ConvertDMLMethodName()}(:idTypVozidla, :nazev);";
        OracleParameter[] sqlParams = [ new("idTypVozidla", ConvertId(typVozidla.IdTypVozidla)),
                                        new("nazev", typVozidla.Nazev)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLUdrzbyAsync(Udrzba udrzba)
    {
        string sql = $"{ConvertDMLMethodName()}(:idUdrzba, :idVozidlo, :datum,:typUdrzby, :umytoVMycce, :cistenoOzonem, :typUdrzby, :popisUkonu, :cena);";
        OracleParameter[] sqlParams = [ new("idUdrzba", ConvertId(udrzba.IdUdrzba)),
                                        new("idVozidlo", udrzba.IdVozidlo),
                                        new("datum", OracleDbType.Date, udrzba.Datum, ParameterDirection.Input),
                                        new("popisUkonu", udrzba.PopisUkonu),
                                        new("cena", udrzba.Cena),
                                        new("umytoVMycce", udrzba.UmytoVMycce),
                                        new("cistenoOzonem", udrzba.CistenoOzonem),
                                        new("typUdrzby", OracleDbType.Char, udrzba.TypUdrzby, ParameterDirection.Input)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLUzivateleAsync(Uzivatel uzivatel)
    {
        string sql = $"{ConvertDMLMethodName()}(:idUzivatel, :jmeno, :heslo, :role);";
        OracleParameter[] sqlParams = [ new("idUzivatel", ConvertId(uzivatel.IdUzivatel)),
                                        new("jmeno", uzivatel.UzivatelskeJmeno),
                                        new("heslo", uzivatel.Heslo),
                                        new("role", uzivatel.IdRole)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLVozidlaAsync(Vozidlo vozidlo)
    {
        string sql = $"{ConvertDMLMethodName()}(:idVozidlo,:rokVyroby, :najeteKilometry, :kapacita, :maKlimatizaci, :idGaraz, :idModel);";
        OracleParameter[] sqlParams = [ new("idVozidlo", ConvertId(vozidlo.IdVozidlo)),
                                        new("rokVyroby", vozidlo.RokVyroby),
                                        new("najeteKilometry", vozidlo.NajeteKilometry),
                                        new("kapacita", vozidlo.Kapacita),
                                        new("maKlimatizaci", vozidlo.MaKlimatizaci ? 1 : 0),
                                        new("idGaraz", vozidlo.IdGaraz),
                                        new("idModel", vozidlo.IdModel)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLZastavkyAsync(Zastavka zastavka)
    {
        string sql = $"{ConvertDMLMethodName()}(:idZastavka, :nazev, :souradniceX, :souradniceY, :idPasmo);";
        OracleParameter[] sqlParams = [ new("idZastavka", ConvertId(zastavka.IdZastavka)),
                                        new("nazev", zastavka.Nazev),
                                        new("souradniceX", zastavka.SouradniceX),
                                        new("souradniceY", zastavka.SouradniceY),
                                        new("idPasmo", zastavka.IdPasmo)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLZaznamy_TrasyAsync(ZaznamTrasy zaznamTrasy)
    {
        string sql = $"{ConvertDMLMethodName()}(:idZaznam, :idSpoj, :idZastavka, :idVozidlo, :casPrijezdu, :casOdjezdu);";
        OracleParameter[] sqlParams = [ new("idZaznam", ConvertId(zaznamTrasy.IdZaznam)),
                                        new("idJizdniRad", zaznamTrasy.IdJizniRad),
                                        new("idVozidlo", zaznamTrasy.IdVozidlo),
                                        new("casPrijezdu", zaznamTrasy.CasPrijezdu),
                                        new("casOdjezdu", zaznamTrasy.CasOdjezdu)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    public async Task DMLZnackyAsync(Znacka znacka)
    {
        string sql = $"{ConvertDMLMethodName()}(:ídZnacka, :nazev);";
        OracleParameter[] sqlParams = [ new("ídZnacka", ConvertId(znacka.IdZnacka)),
                                        new("nazev", znacka.Nazev)];
        await DMLPackageCallAsync(sql, sqlParams);
    }

    #endregion DML procedures

    #region views

    public async Task<List<DatabazovyObjekt>> GetDBObjektyAsync()
    {
        return await DatabazoveObjekty.FromSqlRaw("SELECT * FROM DB_OBJEKTY").ToListAsync();
    }

    public async Task<Garaz?> GetGarazByIdAsync(int id)
    {
        return await Garaze.FromSqlRaw("SELECT * FROM GARAZE_VIEW WHERE ID_GARAZ = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<List<Garaz>> GetGarazeAsync()
    {
        return await Garaze.FromSqlRaw("SELECT * FROM GARAZE_VIEW").ToListAsync();
    }

    public async Task<JizdniRad?> GetJizdniRadByIdAsync(int id)
    {
        return await JizdniRady.FromSqlRaw("SELECT * FROM JIZDNI_RADY_VIEW WHERE ID_JIZDNI_RAD = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<List<JizdniRad>> GetJizdniRadyAsync()
    {
        return await JizdniRady.FromSqlRaw("SELECT * FROM JIZDNI_RADY_VIEW").ToListAsync();
    }

    public async Task<Linka?> GetLinkaByIdAsync(int id)
    {
        return await Linky.FromSqlRaw("SELECT * FROM LINKY_VIEW WHERE ID_LINKA = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<List<Linka>> GetLinkyAsync()
    {
        return await Linky.FromSqlRaw("SELECT * FROM LINKY_VIEW").ToListAsync();
    }

    public async Task<List<Log>> GetLogyAsync()
    {
        return await Logy.FromSqlRaw("SELECT * FROM LOGY_VIEW").ToListAsync();
    }

    public async Task<Model?> GetModelByIdAsync(int id)
    {
        return await Modely.FromSqlRaw("SELECT * FROM MODELY_VIEW WHERE ID_MODEL = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<List<Model>> GetModelyAsync()
    {
        return await Modely.FromSqlRaw("SELECT * FROM MODELY_VIEW").ToListAsync();
    }

    public async Task<List<NakladyNaVozidlo>> GetNakladyNaVozidla()
    {
        return await NakladyNaVozidla.FromSqlRaw("SELECT * FROM NAKLADY_VOZIDLA").ToListAsync();
    }

    public async Task<List<Pracovnik>> GetPracovniciAsync()
    {
        return await Pracovnici.FromSqlRaw("SELECT * FROM PRACOVNICI_VIEW").ToListAsync();
    }

    public async Task<Pracovnik?> GetPracovnikByIdAsync(int id)
    {
        return await Pracovnici.FromSqlRaw("SELECT * FROM PRACOVNICI_VIEW WHERE ID_PRACOVNIK = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<List<Role>> GetRoleAsync()
    {
        return await Role.FromSqlRaw("SELECT * FROM ROLE_VIEW").ToListAsync();
    }

    public async Task<Role?> GetRoleByIdAsync(int id)
    {
        return await Role.FromSqlRaw("SELECT * FROM ROLE_VIEW WHERE ID_ROLE = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<Schema?> GetSchemaByIdAsync(int id)
    {
        return await Schemata.FromSqlRaw("SELECT * FROM SCHEMATA_VIEW WHERE ID_SCHEMA = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<List<Schema>> GetSchemataAsync()
    {
        return await Schemata.FromSqlRaw("SELECT * FROM SCHEMATA_VIEW").ToListAsync();
    }

    public async Task<Spoj?> GetSpojByIdAsync(int id)
    {
        return await Spoje.FromSqlRaw("SELECT * FROM SPOJE_VIEW WHERE ID_SPOJ = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<List<Spoj>> GetSpojeAsync()
    {
        return await Spoje.FromSqlRaw("SELECT * FROM SPOJE_VIEW").ToListAsync();
    }

    public async Task<List<TarifniPasmo>> GetTarifni_PasmaAsync()
    {
        return await TarifniPasma.FromSqlRaw("SELECT * FROM TARIFNI_PASMA_VIEW").ToListAsync();
    }

    public async Task<TarifniPasmo?> GetTarifniPasmoByIdAsync(int id)
    {
        return await TarifniPasma.FromSqlRaw("SELECT * FROM TARIFNI_PASMA_VIEW WHERE ID_PASMO = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<TypVozidla?> GetTyp_VozidlaByIdAsync(int id)
    {
        return await TypyVozidel.FromSqlRaw("SELECT * FROM TYPY_VOZIDEL_VIEW WHERE ID_TYP_VOZIDLA = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<List<TypVozidla>> GetTypy_VozidelAsync()
    {
        return await TypyVozidel.FromSqlRaw("SELECT * FROM TYPY_VOZIDEL_VIEW").ToListAsync();
    }

    public async Task<Udrzba?> GetUdrzbaByIdAsync(int id)
    {
        return await Udrzby.FromSqlRaw("SELECT * FROM UDRZBY_VIEW WHERE ID_UDRZBA = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<List<Udrzba>> GetUdrzbyAsync()
    {
        return await Udrzby.FromSqlRaw("SELECT * FROM UDRZBY_VIEW").ToListAsync();
    }

    public async Task<Uzivatel?> GetUzivatelByIdAsync(int id)
    {
        return await Uzivatele.FromSqlRaw("SELECT * FROM UZIVATELE_VIEW WHERE ID_UZIVATEL = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<Uzivatel?> GetUzivatelByNamePwdAsync(string name, string pwdHash)
    {
        return await Uzivatele.FromSqlRaw("SELECT * FROM UZIVATELE_VIEW WHERE UZIVATELSKE_JMENO = {0} AND HESLO = {1}", name, pwdHash).FirstOrDefaultAsync();
    }

    public async Task<List<Uzivatel>> GetUzivateleAsync()
    {
        return await Uzivatele.FromSqlRaw("SELECT * FROM UZIVATELE_VIEW").ToListAsync();
    }

    public async Task<bool> GetUzivatelUsernameExistsAsync(string name)
    {
        return await Uzivatele.FromSqlRaw("SELECT * FROM UZIVATELE_VIEW WHERE UZIVATELSKE_JMENO = {0}", name).FirstOrDefaultAsync() != null;
    }

    public async Task<List<Vozidlo>> GetVozidlaAsync()
    {
        return await Vozidla.FromSqlRaw("SELECT * FROM VOZIDLA_VIEW").ToListAsync();
    }

    public async Task<Vozidlo?> GetVozidloByIdAsync(int id)
    {
        return await Vozidla.FromSqlRaw("SELECT * FROM VOZIDLA_VIEW WHERE ID_VOZIDLO = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<Zastavka?> GetZastavkaByIdAsync(int id)
    {
        return await Zastavky.FromSqlRaw("SELECT * FROM ZASTAVKY_VIEW WHERE ID_ZASTAVKA = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<List<Zastavka>> GetZastavkyAsync()
    {
        return await Zastavky.FromSqlRaw("SELECT * FROM ZASTAVKY_VIEW").ToListAsync();
    }

    public async Task<ZaznamTrasy?> GetZaznam_TrasyByIdAsync(int id)
    {
        return await ZaznamyTras.FromSqlRaw("SELECT * FROM ZAZNAMY_TRASY_VIEW WHERE ID_ZAZNAM = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<List<ZaznamTrasy>> GetZaznamy_TrasyAsync()
    {
        return await ZaznamyTras.FromSqlRaw("SELECT * FROM ZAZNAMY_TRASY_VIEW").ToListAsync();
    }

    public async Task<Znacka?> GetZnackaByIdAsync(int id)
    {
        return await Znacky.FromSqlRaw("SELECT * FROM ZNACKY_VIEW WHERE ID_ZNACKA = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<List<Znacka>> GetZnackyAsync()
    {
        return await Znacky.FromSqlRaw("SELECT * FROM ZNACKY_VIEW").ToListAsync();
    }

    #endregion views

    #region EF Core config

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

        modelBuilder.Entity<DatabazovyObjekt>().HasNoKey();
        modelBuilder.Entity<NakladyNaVozidlo>().HasNoKey();
    }

    #endregion EF Core config

    #region Helper methods

    public async Task DeleteFromTableAsync(string table, string paramName, object param)
    {
        using var command = Database.GetDbConnection().CreateCommand();
        command.CommandText = $"DELETE FROM {table} WHERE {paramName} = :Param";
        command.Parameters.Add(new OracleParameter(":Param", param));
        await Database.OpenConnectionAsync();
        await command.ExecuteNonQueryAsync();
        await Database.CloseConnectionAsync();
    }

    private static string ConvertDMLMethodName([CallerMemberName] string methodName = "") => methodName.ToUpper().Replace("DML", "DML_").Replace("ASYNC", string.Empty);

    private static int? ConvertId(int id) => id == 0 ? null : id;

    //private static string ConvertMethodNameToView([CallerMemberName] string methodName = "") => methodName.ToLower().Replace("get", string.Empty).Replace("async", string.Empty).Replace("byid", string.Empty);

    private async Task DMLPackageCallAsync(string sql, OracleParameter[] sqlParams)
    {
        using var command = Database.GetDbConnection().CreateCommand();
        command.CommandText = $@"   BEGIN
                                    DML_PROCEDURY.{sql}
                                    END;";
        command.CommandType = CommandType.Text;
        command.Parameters.AddRange(sqlParams);
        await Database.OpenConnectionAsync();
        await command.ExecuteNonQueryAsync();
        await Database.CloseConnectionAsync();
    }

    //private async Task<T?> GetDBView<T>(string viewName, string whereClause) where T : class
    //{
    //    string sql = @$"DECLARE
    //                    v_{viewName}_json CLOB;
    //                    BEGIN
    //                    SELECT JSON_OBJECT(*) INTO v_{viewName}_json
    //                    FROM {viewName.ToUpper()}_VIEW
    //                    WHERE {whereClause};
    //                    :p_result := v_{viewName}_json;
    //                    END;";
    //    return await GetObjectFromDB<T>(sql.ToString());
    //}

    //private async Task<List<T>?> GetDBView<T>(string viewName) where T : class
    //{
    //    string sql = @$"DECLARE
    //                    v_{viewName}_json CLOB;
    //                    BEGIN
    //                    SELECT JSON_ARRAYAGG(JSON_OBJECT(*) RETURNING CLOB) INTO v_{viewName}_json
    //                    FROM {viewName.ToUpper()}_VIEW;
    //                    :p_result := v_{viewName}_json;
    //                    END;";
    //    return await GetObjectFromDB<List<T>>(sql.ToString());
    //}

    //private async Task<T?> GetObjectFromDB<T>(string sql, OracleParameter[]? sqlParams = null) where T : class
    //{
    //    try
    //    {
    //        var connection = Database.GetDbConnection();
    //        var resultParam = new OracleParameter("p_result", OracleDbType.Clob, ParameterDirection.Output);
    //        string resultJson = string.Empty;

    //        using (var command = connection.CreateCommand())
    //        {
    //            command.CommandText = sql;
    //            if (sqlParams != null)
    //                command.Parameters.AddRange(sqlParams);
    //            command.Parameters.Add(resultParam);

    //            await connection.OpenAsync();
    //            await command.ExecuteNonQueryAsync();
    //            if (resultParam.Value != DBNull.Value && !((OracleClob)resultParam.Value).IsNull)
    //                resultJson = ((OracleClob)resultParam.Value).Value;
    //            await connection.CloseAsync();
    //        }
    //        return JsonConvert.DeserializeObject<T>(resultJson);
    //    }
    //    catch (Exception)
    //    {
    //        return null;
    //    }
    //}

    #endregion Helper methods
}