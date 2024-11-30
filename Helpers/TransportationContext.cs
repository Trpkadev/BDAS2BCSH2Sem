using BCSH2BDAS2.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Runtime.CompilerServices;

namespace BCSH2BDAS2.Helpers;

public class TransportationContext(DbContextOptions<TransportationContext> options) : DbContext(options)
{
    public DbSet<Garaz> Garaze { get; set; }
    public DbSet<JizdniRad> JizdniRady { get; set; }
    public DbSet<Linka> Linky { get; set; }
    public DbSet<Model> Modely { get; set; }
    public DbSet<Spoj> Spoje { get; set; }
    public DbSet<TarifniPasmo> TarifniPasma { get; set; }
    public DbSet<TypVozidla> TypyVozidel { get; set; }
    public DbSet<Udrzba> Udrzby { get; set; }
    public DbSet<Uzivatel> Uzivatele { get; set; }
    public DbSet<Vozidlo> Vozidla { get; set; }
    public DbSet<Zastavka> Zastavky { get; set; }
    public DbSet<ZaznamTrasy> ZaznamyTras { get; set; }
    public DbSet<Znacka> Znacky { get; set; }
    public DbSet<Log> Logy { get; set; }
    public DbSet<Pracovnik> Pracovnici { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<Schema> Schemata { get; set; }
    public DbSet<DatabazovyObjekt> DatabazoveObjekty { get; set; }
    public DbSet<NakladyNaVozidlo> NakladyNaVozidla { get; set; }

    #region DML procedures

    public async Task DMLGarazeAsync(Garaz garaz)
    {
        string sql = $"{ConvertDMLMethodName()}(:idGaraz, :nazev, :kapacita);";
        OracleParameter[] sqlParams = [ new OracleParameter("idGaraz", ConvertId(garaz.IdGaraz)),
                                        new OracleParameter("nazev", garaz.Nazev),
                                        new OracleParameter("kapacita", garaz.Kapacita)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLJizdni_RadyAsync(JizdniRad jizdniRad)
    {
        string sql = $"{ConvertDMLMethodName()}(:idSpoj, :idZastavka, :casPrijezdu, :casOdjezdu);";
        OracleParameter[] sqlParams = [ new OracleParameter("idSpoj", jizdniRad.IdSpoj),
                                        new OracleParameter("idZastavka", jizdniRad.IdZastavka),
                                        new OracleParameter("casPrijezdu", jizdniRad.CasPrijezdu),
                                        new OracleParameter("casOdjezdu", jizdniRad.CasOdjezdu)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLLinkyAsync(Linka linka)
    {
        string sql = $"{ConvertDMLMethodName()}(:idLinka, :nazev, :typVozidla, :cislo);";
        OracleParameter[] sqlParams = [ new OracleParameter("idLinka", ConvertId(linka.IdLinka)),
                                        new OracleParameter("nazev", linka.Nazev),
                                        new OracleParameter("typVozidla", linka.IdTypVozidla),
                                        new OracleParameter("cislo", linka.Cislo)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLModelyAsync(Model model)
    {
        string sql = $"{ConvertDMLMethodName()}(:idModel, :idTypVozidla, :idZnacka, :nazev, :jeNizkopodlazni);";
        OracleParameter[] sqlParams = [ new OracleParameter("idModel", ConvertId(model.IdModel)),
                                        new OracleParameter("idTypVozidla", model.IdTypVozidla),
                                        new OracleParameter("idZnacka", model.IdZnacka),
                                        new OracleParameter("nazev", model.Nazev),
                                        new OracleParameter("jeNizkopodlazni", model.JeNizkopodlazni)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLPracovniciAsync(Pracovnik pracovnik)
    {
        string sql = $"{ConvertDMLMethodName()}(:idPracovnik, :uzivatelskeJmeno, :heslo, :idNadrizeny, :idRole, :hodinovaMzda, :jmeno, :prijmeni, :telefonniCislo, :email, :rodneCislo, :idUzivatel);";
        OracleParameter[] sqlParams = [ new OracleParameter("idPracovnik", ConvertId(pracovnik.IdPracovnik)),
                                        new OracleParameter("idNadrizeny", pracovnik.IdNadrizeny),
                                        new OracleParameter("idRole", pracovnik.IdNadrizeny),
                                        new OracleParameter("hodinovaMzda", pracovnik.HodinovaMzda),
                                        new OracleParameter("jmeno", pracovnik.Jmeno),
                                        new OracleParameter("prijmeni", pracovnik.Prijmeni),
                                        new OracleParameter("telefonniCislo", pracovnik.TelefonniCislo),
                                        new OracleParameter("email", pracovnik.Email),
                                        new OracleParameter("rodneCislo", pracovnik.RodneCislo),
                                        new OracleParameter("idUzivatel", pracovnik.IdUzivatel)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLRoleAsync(Role role)
    {
        string sql = $"{ConvertDMLMethodName()}(:idRole, :nazev, :prava);";
        OracleParameter[] sqlParams = [ new OracleParameter("idRole", ConvertId(role.IdRole)),
                                        new OracleParameter("nazev", role.Nazev),
                                        new OracleParameter("prava", role.Prava)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLSchemataAsync(Schema schema)
    {
        string sql = $"{ConvertDMLMethodName()}(:idSchema, :nazevSchematu, :nazevSouboru, :typSouboru, :velikostSouboru, :datumZmeny, :soubor);";
        OracleParameter[] sqlParams = [ new OracleParameter("idSchema", ConvertId(schema.IdSchema)),
                                        new OracleParameter("nazevSchematu", schema.NazevSchematu),
                                        new OracleParameter("nazevSouboru", schema.NazevSouboru),
                                        new OracleParameter("typSouboru", schema.TypSouboru),
                                        new OracleParameter("velikostSouboru", schema.VelikostSouboru),
                                        new OracleParameter("datumZmeny", schema.DatumZmeny),
                                        new OracleParameter("soubor", schema.Soubor)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLSpojeAsync(Spoj spoj)
    {
        string sql = $"{ConvertDMLMethodName()}(:idSpoj, :idLinka, :garantovaneNizkopodlazni, :jedeVeVsedniDen, :jedeVSobotu, :jedeVNedeli);";
        OracleParameter[] sqlParams = [ new OracleParameter("idSpoj", ConvertId(spoj.IdSpoj)),
                                        new OracleParameter("idLinka", spoj.IdLinka),
                                        new OracleParameter("garantovaneNizkopodlazni", spoj.GarantovaneNizkopodlazni),
                                        new OracleParameter("jedeVeVsedniDen", spoj.JedeVeVsedniDen),
                                        new OracleParameter("jedeVSobotu", spoj.JedeVSobotu ? 1 : 0),
                                        new OracleParameter("jedeVNedeli", spoj.JedeVNedeli)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLTarifni_PasmaAsync(TarifniPasmo tarifniPasmo)
    {
        string sql = $"{ConvertDMLMethodName()}(:idPasmo, :nazev);";
        OracleParameter[] sqlParams = [ new OracleParameter("idPasmo", ConvertId(tarifniPasmo.IdPasmo)),
                                        new OracleParameter("nazev", tarifniPasmo.Nazev)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLTypy_VozidelAsync(TypVozidla typVozidla)
    {
        string sql = $"{ConvertDMLMethodName()}(:idTypVozidla, :nazev);";
        OracleParameter[] sqlParams = [ new OracleParameter("idTypVozidla", ConvertId(typVozidla.IdTypVozidla)),
                                        new OracleParameter("nazev", typVozidla.Nazev)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLUdrzbyAsync(Udrzba udrzba)
    {
        string sql = $"{ConvertDMLMethodName()}(:idUdrzba, :idVozidlo, :datum,:typUdrzby, :umytoVMycce, :cistenoOzonem, :typUdrzby, :popisUkonu, :cena);";
        OracleParameter[] sqlParams = [ new OracleParameter("idUdrzba", ConvertId(udrzba.IdUdrzba)),
                                        new OracleParameter("idVozidlo", udrzba.IdVozidlo),
                                        new OracleParameter("datum", OracleDbType.Date, udrzba.Datum, ParameterDirection.Input),
                                        new OracleParameter("popisUkonu", udrzba.PopisUkonu),
                                        new OracleParameter("cena", udrzba.Cena),
                                        new OracleParameter("umytoVMycce", udrzba.UmytoVMycce),
                                        new OracleParameter("cistenoOzonem", udrzba.CistenoOzonem),
                                        new OracleParameter("typUdrzby", OracleDbType.Char, udrzba.TypUdrzby, ParameterDirection.Input)];
        await DMLPackageCall(sql.ToString(), sqlParams);
    }

    public async Task DMLUzivateleAsync(Uzivatel uzivatel)
    {
        string sql = $"{ConvertDMLMethodName()}(:idUzivatel, :jmeno, :heslo, :role);";
        OracleParameter[] sqlParams = [ new OracleParameter("idUzivatel", ConvertId(uzivatel.IdUzivatel)),
                                        new OracleParameter("jmeno", uzivatel.UzivatelskeJmeno),
                                        new OracleParameter("heslo", uzivatel.Heslo),
                                        new OracleParameter("role", uzivatel.IdRole)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLVozidlaAsync(Vozidlo vozidlo)
    {
        string sql = $"{ConvertDMLMethodName()}(:idVozidlo,:rokVyroby, :najeteKilometry, :kapacita, :maKlimatizaci, :idGaraz, :idModel);";
        OracleParameter[] sqlParams = [ new OracleParameter("idVozidlo", ConvertId(vozidlo.IdVozidlo)),
                                        new OracleParameter("rokVyroby", vozidlo.RokVyroby),
                                        new OracleParameter("najeteKilometry", vozidlo.NajeteKilometry),
                                        new OracleParameter("kapacita", vozidlo.Kapacita),
                                        new OracleParameter("maKlimatizaci", vozidlo.MaKlimatizaci ? 1 : 0),
                                        new OracleParameter("idGaraz", vozidlo.IdGaraz),
                                        new OracleParameter("idModel", vozidlo.IdModel)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLZastavkyAsync(Zastavka zastavka)
    {
        string sql = $"{ConvertDMLMethodName()}(:idZastavka, :nazev, :souradniceX, :souradniceY, :idPasmo);";
        OracleParameter[] sqlParams = [ new OracleParameter("idZastavka", ConvertId(zastavka.IdZastavka)),
                                        new OracleParameter("nazev", zastavka.Nazev),
                                        new OracleParameter("souradniceX", zastavka.SouradniceX),
                                        new OracleParameter("souradniceY", zastavka.SouradniceY),
                                        new OracleParameter("idPasmo", zastavka.IdPasmo)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLZaznamy_TrasyAsync(ZaznamTrasy zaznamTrasy)
    {
        string sql = $"{ConvertDMLMethodName()}(:idZaznam, :idSpoj, :idZastavka, :idVozidlo, :casPrijezdu, :casOdjezdu);";
        OracleParameter[] sqlParams = [ new OracleParameter("idZaznam", ConvertId(zaznamTrasy.IdZaznam)),
                                        new OracleParameter("idJizdniRad", zaznamTrasy.IdJizniRad),
                                        new OracleParameter("idVozidlo", zaznamTrasy.IdVozidlo),
                                        new OracleParameter("casPrijezdu", zaznamTrasy.CasPrijezdu),
                                        new OracleParameter("casOdjezdu", zaznamTrasy.CasOdjezdu)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLZnackyAsync(Znacka znacka)
    {
        string sql = $"{ConvertDMLMethodName()}(:ídZnacka, :nazev);";
        OracleParameter[] sqlParams = [ new OracleParameter("ídZnacka", ConvertId(znacka.IdZnacka)),
                                        new OracleParameter("nazev", znacka.Nazev)];
        await DMLPackageCall(sql, sqlParams);
    }

    #endregion DML procedures

    #region views

    public async Task<List<Garaz>> GetGarazeAsync()
    {
        return await Garaze.FromSqlRaw("SELECT * FROM GARAZE_VIEW").ToListAsync();
    }

    public async Task<Garaz?> GetGarazByIdAsync(int id)
    {
        return await Garaze.FromSqlRaw("SELECT * FROM GARAZE_VIEW WHERE ID_GARAZ = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<List<JizdniRad>> GetJizdni_RadyAsync()
    {
        return await JizdniRady.FromSqlRaw("SELECT * FROM JIZDNI_RADY_VIEW").ToListAsync();
    }

    public async Task<JizdniRad?> GetJizdni_RadByIdAsync(int id)
    {
        return await JizdniRady.FromSqlRaw("SELECT * FROM JIZDNI_RADY_VIEW WHERE ID_JIZDNI_RAD = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<List<Linka>> GetLinkyAsync()
    {
        return await Linky.FromSqlRaw("SELECT * FROM LINKY_VIEW").ToListAsync();
    }

    public async Task<Linka?> GetLinkaByIdAsync(int id)
    {
        return await Linky.FromSqlRaw("SELECT * FROM LINKY_VIEW WHERE ID_LINKA = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<List<Model>> GetModelyAsync()
    {
        return await Modely.FromSqlRaw("SELECT * FROM MODELY_VIEW").ToListAsync();
    }

    public async Task<Model?> GetModelByIdAsync(int id)
    {
        return await Modely.FromSqlRaw("SELECT * FROM MODELY_VIEW WHERE ID_MODEL = {0}", id).FirstOrDefaultAsync();
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

    public async Task<List<Schema>> GetSchemataAsync()
    {
        return await Schemata.FromSqlRaw("SELECT * FROM SCHEMATA_VIEW").ToListAsync();
    }

    public async Task<Schema?> GetSchemaByIdAsync(int id)
    {
        return await Schemata.FromSqlRaw("SELECT * FROM SCHEMATA_VIEW WHERE ID_SCHEMA = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<List<Spoj>> GetSpojeAsync()
    {
        return await Spoje.FromSqlRaw("SELECT * FROM SPOJE_VIEW").ToListAsync();
    }

    public async Task<Spoj?> GetSpojByIdAsync(int id)
    {
        return await Spoje.FromSqlRaw("SELECT * FROM SPOJE_VIEW WHERE ID_SPOJ = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<List<TarifniPasmo>> GetTarifni_PasmaAsync()
    {
        return await TarifniPasma.FromSqlRaw("SELECT * FROM TARIFNI_PASMA_VIEW").ToListAsync();
    }

    public async Task<TarifniPasmo?> GetTarifni_PasmoByIdAsync(int id)
    {
        return await TarifniPasma.FromSqlRaw("SELECT * FROM TARIFNI_PASMA_VIEW WHERE ID_PASMO = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<List<TypVozidla>> GetTypy_VozidelAsync()
    {
        return await TypyVozidel.FromSqlRaw("SELECT * FROM TYPY_VOZIDEL_VIEW").ToListAsync();
    }

    public async Task<TypVozidla?> GetTyp_VozidlaByIdAsync(int id)
    {
        return await TypyVozidel.FromSqlRaw("SELECT * FROM TYPY_VOZIDEL_VIEW WHERE ID_TYP_VOZIDLA = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<List<Udrzba>> GetUdrzbyAsync()
    {
        return await Udrzby.FromSqlRaw("SELECT * FROM UDRZBY_VIEW").ToListAsync();
    }

    public async Task<Udrzba?> GetUdrzbaByIdAsync(int id)
    {
        return await Udrzby.FromSqlRaw("SELECT * FROM UDRZBY_VIEW WHERE ID_UDRZBA = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<List<Uzivatel>> GetUzivateleAsync()
    {
        return await Uzivatele.FromSqlRaw("SELECT * FROM UZIVATELE_VIEW").ToListAsync();
    }

    public async Task<Uzivatel?> GetUzivatelByIdAsync(int id)
    {
        return await Uzivatele.FromSqlRaw("SELECT * FROM UZIVATELE_VIEW WHERE ID_UZIVATEL = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<Uzivatel?> GetUzivatelByNamePwdAsync(string name, string pwdHash)
    {
        return await Uzivatele.FromSqlRaw("SELECT * FROM UZIVATELE_VIEW WHERE UZIVATELSKE_JMENO = {0} AND HESLO = {1}", name, pwdHash).FirstOrDefaultAsync();
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

    public async Task<List<Zastavka>> GetZastavkyAsync()
    {
        return await Zastavky.FromSqlRaw("SELECT * FROM ZASTAVKY_VIEW").ToListAsync();
    }

    public async Task<Zastavka?> GetZastavkaByIdAsync(int id)
    {
        return await Zastavky.FromSqlRaw("SELECT * FROM ZASTAVKY_VIEW WHERE ID_ZASTAVKA = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<List<ZaznamTrasy>> GetZaznamy_TrasyAsync()
    {
        return await ZaznamyTras.FromSqlRaw("SELECT * FROM ZAZNAMY_TRASY_VIEW").ToListAsync();
    }

    public async Task<ZaznamTrasy?> GetZaznam_TrasyByIdAsync(int id)
    {
        return await ZaznamyTras.FromSqlRaw("SELECT * FROM ZAZNAMY_TRASY_VIEW WHERE ID_ZAZNAM = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<List<Znacka>> GetZnackyAsync()
    {
        return await Znacky.FromSqlRaw("SELECT * FROM ZNACKY_VIEW").ToListAsync();
    }

    public async Task<Znacka?> GetZnackaByIdAsync(int id)
    {
        return await Znacky.FromSqlRaw("SELECT * FROM ZNACKY_VIEW WHERE ID_ZNACKA = {0}", id).FirstOrDefaultAsync();
    }

    public async Task<List<Log>> GetLogyAsync()
    {
        return await Logy.FromSqlRaw("SELECT * FROM LOGY_VIEW").ToListAsync();
    }

    public async Task<List<DatabazovyObjekt>> GetDBObjektyAsync()
    {
        return await DatabazoveObjekty.FromSqlRaw("SELECT * FROM DB_OBJEKTY").ToListAsync();
    }

    public async Task<List<NakladyNaVozidlo>> GetNakladyNaVozidla()
    {
        return await NakladyNaVozidla.FromSqlRaw("SELECT * FROM NAKLADY_VOZIDLA").ToListAsync();
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

    private static string ConvertDMLMethodName([CallerMemberName] string methodName = "") => methodName.ToUpper().Replace("DML", "DML_").Replace("ASYNC", string.Empty);

    private static int? ConvertId(int id) => id == 0 ? null : id;

    //private static string ConvertMethodNameToView([CallerMemberName] string methodName = "") => methodName.ToLower().Replace("get", string.Empty).Replace("async", string.Empty).Replace("byid", string.Empty);

    private async Task DMLPackageCall(string sql, OracleParameter[] sqlParams)
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