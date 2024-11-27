using BCSH2BDAS2.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System.Runtime.CompilerServices;

namespace BCSH2BDAS2.Helpers;

public class TransportationContext(DbContextOptions<TransportationContext> options) : DbContext(options)
{
    public DbSet<Garaz> Garaze { get; set; }
    public DbSet<JizniRad> JizdniRady { get; set; }
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

    #region DML procedures

    public async Task DMLGarazeAsync(Garaz garaz)
    {
        string sql = $"{ConvertDMLMethodName()}(:idGaraz, :nazev, :kapacita);";
        OracleParameter[] sqlParams = [ new OracleParameter("idGaraz", ConvertId(garaz.IdGaraz)),
                                        new OracleParameter("nazev", garaz.Nazev),
                                        new OracleParameter("kapacita", garaz.Kapacita)];
        await DMLPackageCall(sql, sqlParams);
    }

    public async Task DMLJizdni_RadyAsync(JizniRad jizdniRad)
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
        string sql = $"{ConvertDMLMethodName()}(:idUzivatel, :jmeno, :heslo);";
        OracleParameter[] sqlParams = [ new OracleParameter("idUzivatel", ConvertId(uzivatel.IdUzivatel)),
                                        new OracleParameter("jmeno", uzivatel.UzivatelskeJmeno),
                                        new OracleParameter("heslo", uzivatel.Heslo)];
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
                                        new OracleParameter("idSpoj", zaznamTrasy.IdSpoj),
                                        new OracleParameter("idZastavka", zaznamTrasy.IdZastavka),
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

    public async Task<List<Garaz>?> GetGarazeAsync()
    {
        return await GetDBView<Garaz>(ConvertMethodNameToView());
    }

    public async Task<Garaz?> GetGarazeByIdAsync(int id)
    {
        string whereClause = $"IDGARAZ = {id}";
        return await GetDBView<Garaz>(ConvertMethodNameToView(), whereClause);
    }

    public async Task<List<JizniRad>?> GetJizdni_RadyAsync()
    {
        return await GetDBView<JizniRad>(ConvertMethodNameToView());
    }

    public async Task<JizniRad?> GetJizdni_RadyByIdAsync(int id)
    {
        string whereClause = $"IDJIZDNI_RAD = {id}";
        return await GetDBView<JizniRad>(ConvertMethodNameToView(), whereClause);
    }

    public async Task<List<Linka>?> GetLinkyAsync()
    {
        return await GetDBView<Linka>(ConvertMethodNameToView());
    }

    public async Task<Linka?> GetLinkyByIdAsync(int id)
    {
        string whereClause = $"IDLINKA = {id}";
        return await GetDBView<Linka>(ConvertMethodNameToView(), whereClause);
    }

    public async Task<List<Model>?> GetModelyAsync()
    {
        return await GetDBView<Model>(ConvertMethodNameToView());
    }

    public async Task<Model?> GetModelyByIdAsync(int id)
    {
        string whereClause = $"IDMODEL = {id}";
        return await GetDBView<Model>(ConvertMethodNameToView(), whereClause);
    }

    public async Task<List<Role>?> GetRoleAsync()
    {
        return await GetDBView<Role>(ConvertMethodNameToView());
    }

    public async Task<Role?> GetRoleByIdAsync(int id)
    {
        string whereClause = $"IDROLE = {id}";
        return await GetDBView<Role>(ConvertMethodNameToView(), whereClause);
    }

    public async Task<List<Schema>?> GetSchemataAsync()
    {
        return await GetDBView<Schema>(ConvertMethodNameToView());
    }

    public async Task<Schema?> GetSchemataByIdAsync(int id)
    {
        string whereClause = $"IDSCHEMA = {id}";
        return await GetDBView<Schema>(ConvertMethodNameToView(), whereClause);
    }

    public async Task<List<Spoj>?> GetSpojeAsync()
    {
        return await GetDBView<Spoj>(ConvertMethodNameToView());
    }

    public async Task<Spoj?> GetSpojeByIdAsync(int id)
    {
        string whereClause = $"IDSPOJ = {id}";
        return await GetDBView<Spoj>(ConvertMethodNameToView(), whereClause);
    }

    public async Task<List<TarifniPasmo>?> GetTarifni_PasmaAsync()
    {
        return await GetDBView<TarifniPasmo>(ConvertMethodNameToView());
    }

    public async Task<TarifniPasmo?> GetTarifni_PasmaByIdAsync(int id)
    {
        string whereClause = $"IDTARIFNI_PASMO = {id}";
        return await GetDBView<TarifniPasmo>(ConvertMethodNameToView(), whereClause);
    }

    public async Task<List<TypVozidla>?> GetTypy_VozidelAsync()
    {
        return await GetDBView<TypVozidla>(ConvertMethodNameToView());
    }

    public async Task<TypVozidla?> GetTypy_VozidelByIdAsync(int id)
    {
        string whereClause = $"IDTYP_VOZIDLA = {id}";
        return await GetDBView<TypVozidla>(ConvertMethodNameToView(), whereClause);
    }

    public async Task<List<Udrzba>?> GetUdrzbyAsync()
    {
        return await GetDBView<Udrzba>(ConvertMethodNameToView());
    }

    public async Task<Udrzba?> GetUdrzbyByIdAsync(int id)
    {
        string whereClause = $"IDUDRZBA = {id}";
        return await GetDBView<Udrzba>(ConvertMethodNameToView(), whereClause);
    }

    public async Task<IUser?> GetUzivatelOrPracovnikByNamePwdAsync(string name, string pwdHash)
    {
        string sql = @" DECLARE
                        v_uzivatel_json CLOB;
                        BEGIN
                        v_uzivatel_json := GetUzivatelOrPracovnikByJmenoHash(:p_uzivatelske_jmeno,:p_hash);
                        :p_result := v_uzivatel_json;
                        END;";
        OracleParameter[] sqlParams = [ new OracleParameter("p_uzivatelske_jmeno", OracleDbType.Varchar2, name, ParameterDirection.Input),
                                        new OracleParameter("p_hash", OracleDbType.Varchar2, pwdHash, ParameterDirection.Input)];
        var a = await GetObjectFromDB<Dictionary<string, string>>(sql, sqlParams);
        IUser? user = null;
        if (a != null)
        {
            string json = JsonConvert.SerializeObject(a);
            user = a.ContainsKey("IdRole") ? JsonConvert.DeserializeObject<Pracovnik>(json) : JsonConvert.DeserializeObject<Uzivatel>(json);
        }
        return user;
    }

    public async Task<bool> GetUzivatelOrPracovnikUsernameExistsAsync(string name)
    {
        string sql = @"DECLARE
                     v_result CLOB;
                     BEGIN
                     v_result := GetUzivatelOrPracovnikUsernameExists(:p_uzivatelske_jmeno);
                     :p_result := v_result;
                     END;";
        OracleParameter[] sqlParams = [new OracleParameter("p_uzivatelske_jmeno", OracleDbType.Varchar2, name, ParameterDirection.Input)];
        var result = await GetObjectFromDB<Dictionary<string, bool>>(sql, sqlParams);
        bool exists = false;
        result?.TryGetValue("exists", out exists);
        return exists;
    }

    public async Task<List<Uzivatel>?> GetUzivateleAsync()
    {
        return await GetDBView<Uzivatel>(ConvertMethodNameToView());
    }

    public async Task<Uzivatel?> GetUzivateleByIdAsync(int id)
    {
        string whereClause = $"IDUZIVATEL = {id}";
        return await GetDBView<Uzivatel>(ConvertMethodNameToView(), whereClause);
    }

    public async Task<List<Pracovnik>?> GetPracovniciAsync()
    {
        return await GetDBView<Pracovnik>(ConvertMethodNameToView());
    }

    public async Task<Pracovnik?> GetPracovniciByIdAsync(int id)
    {
        string whereClause = $"IDPRACOVNIK = {id}";
        return await GetDBView<Pracovnik>(ConvertMethodNameToView(), whereClause);
    }

    public async Task<List<Vozidlo>?> GetVozidlaAsync()
    {
        return await GetDBView<Vozidlo>(ConvertMethodNameToView());
    }

    public async Task<Vozidlo?> GetVozidlaByIdAsync(int id)
    {
        string whereClause = $"IDVOZIDLO = {id}";
        return await GetDBView<Vozidlo>(ConvertMethodNameToView(), whereClause);
    }

    public async Task<List<Zastavka>?> GetZastavkyAsync()
    {
        return await GetDBView<Zastavka>(ConvertMethodNameToView());
    }

    public async Task<Zastavka?> GetZastavkyByIdAsync(int id)
    {
        string whereClause = $"IDZASTAVKA = {id}";
        return await GetDBView<Zastavka>(ConvertMethodNameToView(), whereClause);
    }

    public async Task<List<ZaznamTrasy>?> GetZaznamy_TrasyAsync()
    {
        return await GetDBView<ZaznamTrasy>(ConvertMethodNameToView());
    }

    public async Task<ZaznamTrasy?> GetZaznamy_TrasyByIdAsync(int id)
    {
        string whereClause = $"IDZAZNAM_TRASY = {id}";
        return await GetDBView<ZaznamTrasy>(ConvertMethodNameToView(), whereClause);
    }

    public async Task<List<Znacka>?> GetZnackyAsync()
    {
        return await GetDBView<Znacka>(ConvertMethodNameToView());
    }

    public async Task<Znacka?> GetZnackyByIdAsync(int id)
    {
        string whereClause = $"IDZNACKA = {id}";
        return await GetDBView<Znacka>(ConvertMethodNameToView(), whereClause);
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
    }

    #endregion EF Core config

    #region Helper methods

    private static string ConvertDMLMethodName([CallerMemberName] string methodName = "") => methodName.ToUpper().Replace("DML", "DML_").Replace("ASYNC", string.Empty);

    private static int? ConvertId(int id) => id == 0 ? null : id;

    private static string ConvertMethodNameToView([CallerMemberName] string methodName = "") => methodName.ToLower().Replace("get", string.Empty).Replace("async", string.Empty).Replace("byid", string.Empty);

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

    private async Task<T?> GetDBView<T>(string viewName, string whereClause) where T : class
    {
        string sql = @$"DECLARE
                        v_{viewName}_json CLOB;
                        BEGIN
                        SELECT JSON_OBJECT(*) INTO v_{viewName}_json
                        FROM {viewName.ToUpper()}_VIEW
                        WHERE {whereClause};
                        :p_result := v_{viewName}_json;
                        END;";
        return await GetObjectFromDB<T>(sql.ToString());
    }

    private async Task<List<T>?> GetDBView<T>(string viewName) where T : class
    {
        string sql = @$"DECLARE
                        v_{viewName}_json CLOB;
                        BEGIN
                        SELECT JSON_ARRAYAGG(JSON_OBJECT(*) RETURNING CLOB) INTO v_{viewName}_json
                        FROM {viewName.ToUpper()}_VIEW;
                        :p_result := v_{viewName}_json;
                        END;";
        return await GetObjectFromDB<List<T>>(sql.ToString());
    }

    private async Task<T?> GetObjectFromDB<T>(string sql, OracleParameter[]? sqlParams = null) where T : class
    {
        try
        {
            var connection = Database.GetDbConnection();
            var resultParam = new OracleParameter("p_result", OracleDbType.Clob, ParameterDirection.Output);
            string resultJson = string.Empty;

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                if (sqlParams != null)
                    command.Parameters.AddRange(sqlParams);
                command.Parameters.Add(resultParam);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                if (resultParam.Value != DBNull.Value && !((OracleClob)resultParam.Value).IsNull)
                    resultJson = ((OracleClob)resultParam.Value).Value;
                await connection.CloseAsync();
            }
            return JsonConvert.DeserializeObject<T>(resultJson);
        }
        catch (Exception)
        {
            return null;
        }
    }

    #endregion Helper methods
}